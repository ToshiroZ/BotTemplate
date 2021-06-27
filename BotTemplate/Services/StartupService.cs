using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BotTemplate.Services
{
    public class StartupHandler
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordShardedClient _discord;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;

        // DiscordSocketClient, CommandService, and IConfigurationRoot are injected automatically from the IServiceProvider
        public StartupHandler(
            IServiceProvider provider,
            DiscordShardedClient discord,
            CommandService commands,
            IConfigurationRoot config)
        {
            _provider = provider;
            _config = config;
            _discord = discord;
            _commands = commands;
        }

        public async Task StartAsync()
        {
            foreach (var reader in LoadTypeReaders(Assembly.GetEntryAssembly()))
            {
                var memberInfo = reader.GetType().BaseType?.GetGenericArguments().FirstOrDefault();
                if (memberInfo != null)
                    Console.WriteLine($"Added TypeReader for {memberInfo.Name}");
            }
            await _discord.LoginAsync(TokenType.Bot, _config["Token"]);
            await _discord.StartAsync();
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
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
                var x = (TypeReader) Activator.CreateInstance(ft, _discord, _commands);
                var baseType = ft.BaseType;
                var typeArgs = baseType?.GetGenericArguments();
                if (typeArgs != null) _commands.AddTypeReader(typeArgs[0], x);

                toReturn.Add(x);
            }

            return toReturn;
        }
    }
}
