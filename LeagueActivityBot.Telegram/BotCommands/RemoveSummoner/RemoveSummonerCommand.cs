using System;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Telegram.BotCommands.Abstractions;
using LeagueActivityBot.Telegram.BotCommands.GeneralStates;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueActivityBot.Telegram.BotCommands.RemoveSummoner
{
    public class RemoveSummonerCommand : BaseCommand
    {
        private readonly CommandStateStore _stateStore;
        private readonly IServiceProvider _serviceProvider;

        public RemoveSummonerCommand(CommandStateStore stateStore, IServiceProvider serviceProvider)
        {
            _stateStore = stateStore;
            _serviceProvider = serviceProvider;
        }

        public override async Task<CommandState> Handle(long commandOwnerId, string payload)
        {
            var state = _stateStore.Get(commandOwnerId);
            if (state == null)
            {
                state =  CreateNewState(commandOwnerId);
            }

            return state.State switch
            {
                SummonerNameRequiredState _ => await ValidateAndSetContext(state, payload),
                _ => throw new InvalidOperationException()
            };
        }
        
        private async Task<CommandState> ValidateAndSetContext(CommandState state, string payload)
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var repository = serviceScope.ServiceProvider.GetService<IRepository<Summoner>>();
            
            var summoner = repository.GetAll()
                .FirstOrDefault(s => s.Name == payload);

            if (summoner == null)
            {
                _stateStore.Reset(state.CommandOwnerId);
                state.SetState(new FinishCommandHandlingState("Summoner not found. Command was canceled."));
                return state;
            }
            
            await repository.Remove(summoner);
            
            _stateStore.Reset(state.CommandOwnerId);
            state.SetState(new FinishCommandHandlingState("Summoner was successfully removed"));
            return state;
        }

        private CommandState CreateNewState(long commandOwnerId)
        {
            var state = new CommandState(BotCommandsTypes.RemoveSummoner, commandOwnerId, new SummonerNameRequiredState());

            _stateStore.Add(state);
            return state;
        }
    }
}