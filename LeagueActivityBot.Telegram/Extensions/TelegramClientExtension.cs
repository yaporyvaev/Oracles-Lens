using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LeagueActivityBot.Telegram.Extensions
{
    public static class TelegramClientExtension
    {
        public static async Task<Message> SendTextMessage(this TelegramBotClient client, long chatId, string message)
        {
            return await client.SendTextMessageAsync(new ChatId(chatId), message, disableNotification: true, parseMode:ParseMode.Html);
        }
    }
}