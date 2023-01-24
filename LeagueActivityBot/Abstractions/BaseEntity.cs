using System;

namespace LeagueActivityBot.Abstractions
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}