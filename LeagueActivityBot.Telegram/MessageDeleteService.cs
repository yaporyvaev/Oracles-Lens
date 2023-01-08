using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LeagueActivityBot.Telegram
{
    public class MessageDeleteService
    {
        private readonly TelegramBotClient _tgClient;

        public MessageDeleteService(TelegramBotClient tgClient)
        {
            _tgClient = tgClient;
        }

        public Task DeleteMessage(ChatId chatId, int messageId)
        {
            return _tgClient.DeleteMessageAsync(chatId, messageId, CancellationToken.None);
        }
    }
}