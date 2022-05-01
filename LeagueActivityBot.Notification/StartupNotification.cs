using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LeagueActivityBot.Notification
{
    public class StartupNotification
    {
        public static async Task SendOnStartedUpNotification(IServiceProvider serviceProvider, string appVersion)
        {
            using var scope = serviceProvider.CreateScope();
            var options = scope.ServiceProvider.GetService<NotificationOptions>();

            var tgClient = scope.ServiceProvider.GetService<TelegramBotClient>();
            await tgClient.SendTextMessageAsync(new ChatId(options.TelegramChatId), $"Я родился! Текущая версия: {appVersion}");
        }
    }
}
