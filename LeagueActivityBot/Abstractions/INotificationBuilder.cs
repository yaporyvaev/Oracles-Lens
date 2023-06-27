using MediatR;

namespace LeagueActivityBot.Abstractions
{
    public interface INotificationBuilder
    {
        public string Build(INotification notification);
    }
}