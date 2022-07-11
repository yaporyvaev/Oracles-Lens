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
                state = CreateNewState(commandOwnerId);
            }

            return state.State switch
            {
                SummonerNameRequiredState _ => await SetSummonerInfo(state, payload),
                _ => throw new InvalidOperationException()
            };
        }
        
        private async Task<CommandState> SetSummonerInfo(CommandState state, string summonerName)
        {
            var summonerInfo = await _riotClient.GetSummonerInfoByName(summonerName);
            if (summonerInfo == null) throw new BotCommandException($"Summoner {summonerName} not found. Operation was canceled.");

            var summonerDto = new Summoner
            {
                SummonerId = summonerInfo.Id,
                Puuid = summonerInfo.Puuid,
                AccountId = summonerInfo.AccountId,
                Name = summonerName
            };
            
            var leagueInfo = (await _riotClient.GetLeagueInfo(summonerDto.SummonerId))
                .FirstOrDefault(l => l.QueueType == QueueTypeConstants.RankedSolo);

            if (leagueInfo != null)
            {
                summonerDto.LeaguePoints = leagueInfo.LeaguePoints;
                summonerDto.Tier = leagueInfo.GetTierIntegerRepresentation();
                summonerDto.Rank = leagueInfo.GetRankIntegerRepresentation();
            }
            
            using var serviceScope = _serviceProvider.CreateScope();
            var repository = serviceScope.ServiceProvider.GetService<IRepository<Summoner>>();
            var summonerEntity = repository.GetAll()
                .FirstOrDefault(s => s.SummonerId == summonerDto.SummonerId);

            if (summonerEntity != null)
            {
                summonerDto.Id = summonerEntity.Id;
                await repository.Update(summonerDto);
            }
            else
            {
                await repository.Add(summonerDto);
            }
            
            _stateStore.Reset(state.CommandOwnerId);
            state.SetState(new FinishCommandHandlingState("Summoner was successfully added."));
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