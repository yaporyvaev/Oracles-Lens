using System.IO;
using System.Threading.Tasks;
using LeagueActivityBot.ImageGeneration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace LeagueActivityBot.Telegram.Services
{
    public class ImageService
    {
        private readonly TelegramOptions _options;
        private readonly TelegramBotClient _tgClient;

        public ImageService(TelegramOptions options, TelegramBotClient tgClient)
        {
            _options = options;
            _tgClient = tgClient;
        }

        public async Task SendImage()
        {
            var bytes = ImageGenerator.Gen();
            await using var ms = new MemoryStream(bytes);
            
            var file = new InputOnlineFile(ms, "cock.jpg");
            await _tgClient.SendPhotoAsync(new ChatId(_options.TelegramChatId), file, disableNotification: true);
        }
    }
}