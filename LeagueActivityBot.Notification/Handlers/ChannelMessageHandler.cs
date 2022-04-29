using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LeagueActivityBot.Notification.Handlers
{
    public class ChannelMessageHandler
    {
        private readonly TelegramBotClient _tgClient;
        private readonly NotificationOptions _options;
        private DateTime _lastStasMessage = DateTime.MinValue;

        public ChannelMessageHandler(NotificationOptions options)
        {
            _options = options;
            _tgClient =  new TelegramBotClient(_options.TelegramBotApiKey);
        }

        public void StartHandling()
        {
            using var cts = new CancellationTokenSource();
            
            _tgClient.StartReceiving(HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions {AllowedUpdates = new[] {UpdateType.Message},Offset = -1},
                cts.Token);
        }

        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update message, CancellationToken ct)
        {
            //Healthcheck
            if (message.Message.Text == "@LeagueActivityBot ты жив?")
            {
                await bot.SendTextMessageAsync(new ChatId(_options.TelegramChatId), InsultGenerator.GetInsult(), cancellationToken: ct, replyToMessageId:message.Message.MessageId);
            }
            
            if (message.Message.From.Id == 501536687 && DateTime.UtcNow.AddHours(-3) > _lastStasMessage)
            {
                _lastStasMessage = DateTime.UtcNow;
                await bot.SendTextMessageAsync(new ChatId(_options.TelegramChatId), "О, Стас пришёл (:", cancellationToken: ct);
            }

            if (new Random().Next(0, 30) == 13)
            {
                await bot.SendTextMessageAsync(new ChatId(_options.TelegramChatId), InsultGenerator.GetInsult(), cancellationToken: ct, replyToMessageId:message.Message.MessageId);
            }
        }

        private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
}