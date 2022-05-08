using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Constants;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Telegram.BotCommands.Abstractions;
using LeagueActivityBot.Telegram.BotCommands.GeneralStates;
using LeagueActivityBot.Telegram.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueActivityBot.Telegram.BotCommands.AddSummoner
{
    [UsedImplicitly]
    public class AddSummonerCommand : BaseCommand
    {
        private readonly CommandStateStore _stateStore;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRiotClient _riotClient;

        public AddSummonerCommand(CommandStateStore stateStore, IRiotClient riotClient, IServiceProvider serviceProvider )
        {
            _stateStore = stateStore;
            _riotClient = riotClient;
            _serviceProvider = serviceProvider;
        } 
        
        public override async Task<CommandState> Handle(long commandOwnerId, string payload)
        {
            var state = _stateStore.Get(commandOwnerId);
            if (state == null)
            {
                return CreateNewState(commandOwnerId);
            }

            return state.State switch
            {
                SummonerNameRequiredState _ => await SetSummonerInfo(state, payload),
                AddSummonerNameInNotificationsRequiredState _ => SetSummonerName(state, payload),
                ActionConfirmRequiredState _ => await ConfirmAction(state, payload),
                _ => throw new InvalidOperationException()
            };
        }

        private async Task<CommandState> ConfirmAction(CommandState state, string payload)
        {
            if (!string.Equals(payload, "yes", StringComparison.InvariantCultureIgnoreCase))
            {
                _stateStore.Reset(state.CommandOwnerId);
                state.SetState(new FinishCommandHandlingState("Command was canceled"));
                return state;
            }
            
            using var serviceScope = _serviceProvider.CreateScope();
            var repository = serviceScope.ServiceProvider.GetService<IRepository<Summoner>>();

            var context = state.Context as AddSummonerContext;
            var summoner = repository.GetAll()
                .FirstOrDefault(s => s.SummonerId == context.Summoner.SummonerId);

            if (summoner != null)
            {
                context.Summoner.Id = summoner.Id;
                await repository.Update(context.Summoner);
            }
            else
            {
                await repository.Add(context.Summoner);
            }
            
            _stateStore.Reset(state.CommandOwnerId);
            state.SetState(new FinishCommandHandlingState("Summoner was successfully added"));
            return state;
        }

        private CommandState SetSummonerName(CommandState state, string payload)
        {
            var context = state.Context as AddSummonerContext;
            context!.Summoner.RealName = payload;
            
            state.SetState(new ActionConfirmRequiredState($"Do you really want to add {context.Summoner.Name} as {context.Summoner.RealName}? (yes/no)"));
            return state;
        }

        private async Task<CommandState> SetSummonerInfo(CommandState state, string summonerName)
        {
            var summonerInfo = await _riotClient.GetSummonerInfoByName(summonerName);
            if (summonerInfo == null) throw new BotCommandException($"Summoner {summonerName} not found. Try again.");

            var summoner = new Summoner
            {
                SummonerId = summonerInfo.Id,
                Puuid = summonerInfo.Puuid,
                AccountId = summonerInfo.AccountId,
                Name = summonerName
            };
            
            var leagueInfo = (await _riotClient.GetLeagueInfo(summoner.SummonerId))
                .FirstOrDefault(l => l.QueueType == QueueType.RankedSolo);

            if (leagueInfo != null)
            {
                summoner.LeaguePoints = leagueInfo.LeaguePoints;
                summoner.Tier = leagueInfo.GetTierIntegerRepresentation();
                summoner.Rank = leagueInfo.GetRankIntegerRepresentation();
            }
            
            state.UpdateContext(new AddSummonerContext{ Summoner = summoner});
            state.SetState(new AddSummonerNameInNotificationsRequiredState());
            
            _stateStore.Update(state);
            return state;
        }

        private CommandState CreateNewState(long commandOwnerId)
        {
            var state = new CommandState(BotCommandsTypes.AddSummoner, commandOwnerId, new SummonerNameRequiredState());

            _stateStore.Add(state);
            return state;
        }
    }
}