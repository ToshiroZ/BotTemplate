using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace BotTemplate.Services
{
    public class CommandHandler
    {
        private readonly DiscordShardedClient _discord;
        private readonly CommandService _commands;
        private readonly IServiceProvider _provider;

        // DiscordSocketClient, CommandService, IConfigurationRoot, and IServiceProvider are injected automatically from the IServiceProvider
        public CommandHandler(
            DiscordShardedClient discord,
            CommandService commands,
            IConfigurationRoot config,
            IServiceProvider provider)
        {
            _discord = discord;
            _commands = commands;
            _provider = provider;

            _discord.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            if (arg is not SocketUserMessage msg)
                return;

            if (msg.Author.Id == _discord.CurrentUser.Id || msg.Author.IsBot)
                return;

            var pos = 0;
            if (msg.HasStringPrefix("*", ref pos) || msg.HasMentionPrefix(_discord.CurrentUser, ref pos))
            {
                var context = new ShardedCommandContext(_discord, msg);
                var result = await _commands.ExecuteAsync(context, pos, _provider);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    await msg.Channel.SendMessageAsync(result.ErrorReason);
            }

        }
    }
}
