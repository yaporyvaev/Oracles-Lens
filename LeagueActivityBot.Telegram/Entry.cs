﻿using System;
using JetBrains.Annotations;
using LeagueActivityBot.Telegram.BotCommands;
using LeagueActivityBot.Telegram.BotCommands.AddSummoner;
using LeagueActivityBot.Telegram.BotCommands.Cancel;
using LeagueActivityBot.Telegram.BotCommands.GetStatistic;
using LeagueActivityBot.Telegram.BotCommands.GetSummoners;
using LeagueActivityBot.Telegram.BotCommands.RemoveSummoner;
using LeagueActivityBot.Telegram.Factories;
using LeagueActivityBot.Telegram.Handlers;
using LeagueActivityBot.Telegram.RecentMessages;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace LeagueActivityBot.Telegram
{
    public static class Entry
    {
        [UsedImplicitly]
        public static IServiceCollection AddNotifications<TOptions>([NotNull] this IServiceCollection serviceCollection, Action<TOptions> optionsAction)
            where TOptions : TelegramOptions, new()
        {
            var options = new TOptions();
            optionsAction?.Invoke(options);

            var settings = new TelegramOptions
            {
                TelegramBotApiKey = options.TelegramBotApiKey,
                TelegramChatId = options.TelegramChatId,
                TelegramLogChatId = options.TelegramLogChatId,
                WebAppLink = options.WebAppLink,
                UseWebAppMatchResults = options.UseWebAppMatchResults,
            };

            serviceCollection.AddSingleton(settings);
            serviceCollection.AddHostedService<ChannelMessageHandler>();
            serviceCollection.AddTransient(_ => new TelegramBotClient(settings.TelegramBotApiKey));
            serviceCollection.AddTransient<TelegramBotClientWrapper>();
            serviceCollection.AddSingleton<CommandStateStore>();
            serviceCollection.AddTransient<CommandFactory>();
            serviceCollection.AddTransient<MessageDeleteService>();
            serviceCollection.AddTransient<RecentGameNotificationMessageStore>();
            
            serviceCollection.AddTransient<AddSummonerCommand>();
            serviceCollection.AddTransient<RemoveSummonerCommand>();
            serviceCollection.AddTransient<CancelCommand>();
            serviceCollection.AddTransient<GetSummonersCommand>();
            serviceCollection.AddTransient<GetStatisticCommand>();
            
            serviceCollection.AddTransient<SoloGameEndedNotificationBuilderFactory>();
            serviceCollection.AddTransient<TeamGameEndedNotificationBuilderFactory>();
            
            serviceCollection.AddTransient<CommandHandler>();

            return serviceCollection;
        }
    }
}