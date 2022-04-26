using System;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueActivityBot.Database
{
    public static class Entry
    {
        private static readonly Action<DbContextOptionsBuilder> DefaultOptionsAction = (options) => { };
        
        public static IServiceCollection AddPostgreSqlStorage(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> optionsAction)
        {
			serviceCollection.AddDbContext<AppDbContext>(optionsAction ?? DefaultOptionsAction);
            serviceCollection.AddScoped(typeof(IRepository<>),typeof(Repository<>));
            
            return serviceCollection;
        }
    }
}