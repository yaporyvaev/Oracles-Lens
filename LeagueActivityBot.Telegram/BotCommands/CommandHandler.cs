using System;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Telegram.Exceptions;
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
            if (string.IsNullOrEmpty(messagePayload)) return;

            string commandType;
            if (messagePayload.StartsWith("/")) //Start new command
            {
                var splitMessage = messagePayload.Split(" ");
                messagePayload = string.Join(" ", splitMessage.Skip(1));
                
                if(messagePayload != BotCommandsTypes.Cancel) _stateStore.Reset(messageSenderId);
                commandType = splitMessage.FirstOrDefault();
            }
            else //Process existing command
            {
                var currentCommandState = _stateStore.Get(messageSenderId);
                if (currentCommandState == null) return;
                commandType = currentCommandState.Type;
            }

            var command = _commandFactory.Create(commandType);
            if(command == null) return;

            try
            {
                var state = await command.Handle(messageSenderId, messagePayload);

                if (state != null)
                {
                    if (state.Type == BotCommandsTypes.CreateBinaryAnswerPool)
                    {
                        await _tgClient.SendPollAsync(new ChatId(_options.TelegramChatId), state.BuildMessage(),
                            new[] {"Да", "Нет", "Хз"}, false, PollType.Regular, false);
                        return;
                    }
                    
                    await _tgClient.SendTextMessageAsync(new ChatId(_options.TelegramChatId), state.BuildMessage());
                }
            }
            catch (BotCommandException commandException)
            {
                await _tgClient.SendTextMessageAsync(new ChatId(_options.TelegramChatId), commandException.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Command handling failed");
                await _tgClient.SendTextMessageAsync(new ChatId(_options.TelegramChatId), "Command handling failed. It was canceled.");
                _stateStore.Reset(messageSenderId);
            }
        }
    }
}