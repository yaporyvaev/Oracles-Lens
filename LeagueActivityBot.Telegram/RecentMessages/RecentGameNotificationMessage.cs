namespace LeagueActivityBot.Telegram.RecentMessages
{
    public class RecentGameNotificationMessage
    {
        public RecentGameNotificationMessage(int messageId, long gameId)
        {
            MessageId = messageId;
            GameId = gameId;
        }

        public long GameId { get;  }
        public int MessageId { get; }
    }
}