using System;
using Microsoft.Extensions.Caching.Memory;

namespace LeagueActivityBot.Telegram.RecentMessages
{
    public class RecentMessageStore
    {
        private readonly IMemoryCache _memoryCache;

        public RecentMessageStore(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Save(RecentGameNotificationMessage message)
        {
            _memoryCache.Set(GetCacheKey(message.GameId), message, TimeSpan.FromHours(2));
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