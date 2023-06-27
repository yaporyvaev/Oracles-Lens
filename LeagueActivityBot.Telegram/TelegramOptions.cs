namespace LeagueActivityBot.Telegram
{
    public class TelegramOptions
    {
        public string TelegramBotApiKey { get; set; }
        public long TelegramChatId { get; set; }
        public long TelegramLogChatId { get; set; }
        public string WebAppLink { get; set; }
        public bool UseWebAppMatchResults { get; set; }
    }
}
