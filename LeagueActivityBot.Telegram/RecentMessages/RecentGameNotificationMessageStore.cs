using System;
using LeagueActivityBot.Constants;
using Microsoft.Extensions.Caching.Memory;

namespace LeagueActivityBot.Telegram.RecentMessages
{
    public class RecentGameNotificationMessageStore
    {
        private readonly IMemoryCache _memoryCache;

        public RecentGameNotificationMessageStore(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Save(RecentGameNotificationMessage message)
        {
            _memoryCache.Set(GetCacheKey(message.GameId), message, TelegramMessageOptions.MessageTimeToLive.Add(TimeSpan.FromMinutes(30)));
        }
        
        public RecentGameNotificationMessage Get(long gameId)
        {
            return _memoryCache.Get<RecentGameNotificationMessage>(GetCacheKey(gameId));
        }

        private static string GetCacheKey(long gameId)
        {
            return $"rcnt_msg_{gameId}";
        }
    }
}