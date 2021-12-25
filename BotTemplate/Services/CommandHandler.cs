using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace BotTemplate.Services
{
    public class CommandHandler
    {
        private readonly DiscordShardedClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _provider;

        public CommandHandler(
            DiscordShardedClient client,
            CommandService commands,
            IConfigurationRoot config,
            IServiceProvider provider)
        {
            _client = client;
            _commands = commands;
            _provider = provider;

            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            if (arg is not SocketUserMessage msg)
                return;

            if (msg.Author.Id == _client.CurrentUser.Id || msg.Author.IsBot)
                return;

            var pos = 0;
            if (msg.HasStringPrefix("*", ref pos) || msg.HasMentionPrefix(_client.CurrentUser, ref pos))
            {
                var context = new ShardedCommandContext(_client, msg);
                var result = await _commands.ExecuteAsync(context, pos, _provider);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    await msg.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }
}
