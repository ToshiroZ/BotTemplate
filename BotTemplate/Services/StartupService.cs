using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Interactions;

namespace BotTemplate.Services
{
    public class StartupHandler
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordShardedClient _client;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;
        private readonly InteractionService _interactionService;

        public StartupHandler(
            IServiceProvider provider,
            DiscordShardedClient client,
            CommandService commands,
            IConfigurationRoot config, InteractionService interactionService)
        {
            _provider = provider;
            _config = config;
            _interactionService = interactionService;
            _client = client;
            _commands = commands;
            _client.ShardReady += ClientOnShardReady;
        }

        private async Task ClientOnShardReady(DiscordSocketClient arg)
        {
#if DEBUG
            ulong guildId = 000000000000000;
            foreach (var command in await _interactionService.RegisterCommandsToGuildAsync(guildId))
            {
                Console.WriteLine($"Registered Command {command.Name} of type {command.Type}");
            }
#else
            foreach (var command in await _interactionService.RegisterCommandsGloballyAsync())
            {
                Console.WriteLine($"Registered Command {command.Name} of type {command.Type}");
            }
#endif
        }

        public async Task StartAsync()
        {
            foreach (var reader in LoadTypeReaders(Assembly.GetEntryAssembly()))
            {
                var memberInfo = reader.GetType().BaseType?.GetGenericArguments().FirstOrDefault();
                if (memberInfo != null)
                    Console.WriteLine($"Added TypeReader for {memberInfo.Name}");
            }
            foreach (var reader in LoadTypeConverters(Assembly.GetEntryAssembly()))
            {
                var memberInfo = reader.GetType().BaseType?.GetGenericArguments().FirstOrDefault();
                if (memberInfo != null)
                    Console.WriteLine($"Added TypeConverter for {memberInfo.Name}");
            }
            await _client.LoginAsync(TokenType.Bot, _config["Token"]);
            await _client.StartAsync();
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
            await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }

        private IEnumerable<object> LoadTypeReaders(Assembly assembly)
        {
            Type[] allTypes;
            try
            {
                allTypes = assembly.GetTypes();
            }
            catch
            {
                return Enumerable.Empty<object>();
            }

            var filteredTypes = allTypes.Where(x =>
                x.BaseType != null && x.IsSubclassOf(typeof(TypeReader)) &&
                x.BaseType.GetGenericArguments().Length > 0 && !x.IsAbstract);

            var toReturn = new List<object>();
            foreach (var ft in filteredTypes)
            {
                var x = (TypeReader) Activator.CreateInstance(ft, _client, _commands);
                var baseType = ft.BaseType;
                var typeArgs = baseType?.GetGenericArguments();
                if (typeArgs != null) _commands.AddTypeReader(typeArgs[0], x);

                toReturn.Add(x);
            }

            return toReturn;
        }
        private IEnumerable<object> LoadTypeConverters(Assembly assembly)
        {
            Type[] allTypes;
            try
            {
                allTypes = assembly.GetTypes();
            }
            catch
            {
                return Enumerable.Empty<object>();
            }

            var filteredTypes = allTypes.Where(x =>
                x.BaseType != null && x.IsSubclassOf(typeof(TypeConverter)) &&
                x.BaseType.GetGenericArguments().Length > 0 && !x.IsAbstract);

            var toReturn = new List<object>();
            foreach (var ft in filteredTypes)
            {
                var x = (TypeConverter) Activator.CreateInstance(ft, _client, _interactionService);
                var baseType = ft.BaseType;
                var typeArgs = baseType?.GetGenericArguments();
                if (typeArgs != null) _interactionService.AddTypeConverter(typeArgs[0], x);

                toReturn.Add(x);
            }

            return toReturn;
        }
    }
}
