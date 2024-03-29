using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LeagueActivityBot.Telegram
{
    public class TelegramBotClientWrapper
    {
        public readonly TelegramBotClient TgClient;

        public TelegramBotClientWrapper(TelegramBotClient tgClient)
        {
            TgClient = tgClient;
        }

        public async Task<Message> SendAutoDeletableTextMessageAsync(ChatId chatId, string text, TimeSpan deleteAfter, CancellationToken cancellationToken = default)
        {
            var message = await TgClient.SendTextMessageAsync(chatId, text, ParseMode.Html, disableWebPagePreview:true, disableNotification:true, cancellationToken:cancellationToken);
            BackgroundJob.Schedule<MessageDeleteService>(t => t.DeleteMessage(chatId, message.MessageId), deleteAfter);
            
            return message;
        }
    }
}