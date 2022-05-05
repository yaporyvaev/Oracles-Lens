using System.Collections.Generic;
using LeagueActivityBot.Telegram.BotCommands.Abstractions;
using LeagueActivityBot.Telegram.BotCommands.GeneralStates;

namespace LeagueActivityBot.Telegram.BotCommands
{
    public class CommandStateStore
    {
        private readonly Dictionary<long, CommandState> _states = new Dictionary<long, CommandState>();

        public CommandState Get(long ownerId)
        {
            return _states.ContainsKey(ownerId) ? _states[ownerId] : null;
        }

        public void Add(CommandState state)
        {
            if (!_states.ContainsKey(state.CommandOwnerId))
            {
                _states.Add(state.CommandOwnerId, state);
            }
            else
            {
                Update(state);
            }
        }

        public void Update(CommandState state)
        {
            if (_states.ContainsKey(state.CommandOwnerId))
            {
                _states[state.CommandOwnerId] = state;
            }
        }

        public void Reset(long ownerId)
        {
            if (_states.ContainsKey(ownerId))
            {
                _states.Remove(ownerId);
            }
        }
    }

    public class CommandState
    {
        public long CommandOwnerId { get; }
        public string Type { get; }
        public BaseState State { get; private set; }
        public BaseStateContext Context { get; private set; }

        public CommandState(string type, long commandOwnerId, BaseState state)
        {
            Type = type;
            CommandOwnerId = commandOwnerId;
            State = state;
        }

        public void UpdateContext(BaseStateContext context)
        {
            Context = context;
        }
        
        public void SetState(BaseState state)
        {
            State = state;
        }

        public string BuildMessage()
        {
            return State.Message;
        }
    }
}