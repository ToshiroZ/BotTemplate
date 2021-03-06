using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using System.IO;
using BotTemplate.Database;
using BotTemplate.Database.Repositories;
using BotTemplate.Services;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RunMode = Discord.Commands.RunMode;

namespace BotTemplate
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;
        private readonly DiscordShardedClient _client;

        public Startup(string[] args)
        { 
            var tokenName = "BotTemplate.token";
            var directory = "";
#if DEBUG
            directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), tokenName);
#else
        directory = Path.Combine(AppContext.BaseDirectory, tokenName);
#endif
            if (!File.Exists(directory))
            {
                Console.Write("Input Token: ");
                var token = Console.ReadLine();
                var json = JsonConvert.SerializeObject(new { Token = token });
                
                File.WriteAllText(directory, json);
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(directory))
                .AddJsonFile(Path.GetFileName(directory));
            _configuration = builder.Build();
            _client = new DiscordShardedClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 50
            });
        }

        public static async Task RunAsync(string[] args)
        {

            var startup = new Startup(args);
            await startup.RunAsync();
        }

        private async Task RunAsync()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<LoggingHandler>();
            provider.GetRequiredService<CommandHandler>();
            provider.GetRequiredService<InteractionHandler>();
            await provider.GetRequiredService<StartupHandler>().StartAsync();
            await Task.Delay(-1);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var interactionService = new InteractionService(_client);
            services.AddDbContext<DatabaseContext>(x => x.UseSqlite("Data Source=BotData.db").UseLazyLoadingProxies());
            services.AddSingleton(_client)
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    LogLevel = LogSeverity.Verbose, 
                    DefaultRunMode = RunMode.Async,
                }))
                .AddSingleton<CommandHandler>() 
                .AddSingleton<StartupHandler>() 
                .AddSingleton<LoggingHandler>()
                .AddSingleton<Random>(Extensions.rnd)
                .AddSingleton<InteractionHandler>()
                .AddSingleton(_configuration)
                .AddSingleton<InteractionService>(interactionService)
                .AddScoped<IRepositoryGrouper, RepositoryGrouper>()
                .AddHttpClient()
                .AddSingleton(new InteractiveService(_client, TimeSpan.FromMinutes(1)));
        }
    }
}