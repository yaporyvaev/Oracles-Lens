using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace LeagueActivityBot.Telegram
{
    public class TelegramBotClientWrapper
    {
        private readonly TelegramBotClient _tgClient;

        public TelegramBotClientWrapper(TelegramBotClient tgClient)
        {
            _tgClient = tgClient;
        }

        public async Task<Message> SendAutoDeletableTextMessageAsync(ChatId chatId,
            string text,
            TimeSpan deleteAfter,
            ParseMode? parseMode = default,
            IEnumerable<MessageEntity> entities = default,
            bool? disableWebPagePreview = default,
            bool? disableNotification = default,
            bool? protectContent = default,
            int? replyToMessageId = default,
            bool? allowSendingWithoutReply = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default)
        {
            var message = await _tgClient.SendTextMessageAsync(chatId, text, parseMode, entities, disableWebPagePreview,
                disableNotification, protectContent, replyToMessageId, allowSendingWithoutReply, replyMarkup,
                cancellationToken);

            BackgroundJob.Schedule<TelegramBotClient>(c => c.DeleteMessageAsync(chatId, message.MessageId, CancellationToken.None),deleteAfter);

            return message;
        }
    }
}