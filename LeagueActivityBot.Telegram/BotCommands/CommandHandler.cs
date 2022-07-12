using System;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Telegram.Exceptions;
using LeagueActivityBot.Telegram.Extensions;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LeagueActivityBot.Telegram.BotCommands
{
    public class CommandHandler
    {
        private readonly CommandStateStore _stateStore;
        private readonly TelegramBotClient _tgClient;
        private readonly TelegramOptions _options;
        private readonly CommandFactory _commandFactory;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(
            CommandStateStore stateStore, 
            TelegramBotClient tgClient, 
            TelegramOptions options, 
            CommandFactory commandFactory,
            ILogger<CommandHandler> logger)
        {
            _stateStore = stateStore;
            _tgClient = tgClient;
            _options = options;
            _commandFactory = commandFactory;
            _logger = logger;
        }

        public async Task Handle(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                await HandleBotMessageCommand(update.Message);
            }
        }

        private async Task HandleBotMessageCommand(Message message)
        {
            var messageSenderId = message.From!.Id;

            var messagePayload = string.Join(" ", message.Text!.Split(" ").Skip(1));//todo refactor this shit
            var commandType = message.Text.Split("@").FirstOrDefault();

            var command = _commandFactory.Create(commandType);
            if(command == null) return;

            try
            {
                var state = await command.Handle(messageSenderId, messagePayload);
                
                if (state != null)
                {
                    await _tgClient.SendTextMessage(_options.TelegramChatId, state.BuildMessage());
                }
            }
            catch (BotCommandException commandException)
            {
                await _tgClient.SendTextMessage(_options.TelegramChatId, commandException.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Command handling failed");
                await _tgClient.SendTextMessage(_options.TelegramChatId, "Command handling failed. It was canceled.");
                _stateStore.Reset(messageSenderId);
            }
        }
    }
}