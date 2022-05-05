using System.Threading.Tasks;
using JetBrains.Annotations;
using LeagueActivityBot.Telegram.BotCommands.Abstractions;
using LeagueActivityBot.Telegram.BotCommands.GeneralStates;

namespace LeagueActivityBot.Telegram.BotCommands.Cancel
{
    [UsedImplicitly]
    public class CancelCommand : BaseCommand
    {
        private readonly CommandStateStore _stateStore;

        public CancelCommand(CommandStateStore stateStore)
        {
            _stateStore = stateStore;
        }

        public override Task<CommandState> Handle(long commandOwnerId, string payload)
        {
            var state = _stateStore.Get(commandOwnerId);
            if (state == null) return null;
            
            _stateStore.Reset(commandOwnerId);
            state.SetState(new FinishCommandHandlingState("Command was canceled"));
            return Task.FromResult(state);
        }
    }
}