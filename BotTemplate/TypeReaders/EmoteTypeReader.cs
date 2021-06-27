using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace BotTemplate.TypeReaders
{
    public class EmoteTypeReader : BaseTypeReader<Emote>
    {
        private readonly DiscordShardedClient _client;

        public EmoteTypeReader(DiscordShardedClient client, CommandService cmds) : base(client, cmds)
        {
            _client = client;
        }

        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input,
            IServiceProvider services)
        {
            return Task.FromResult<TypeReaderResult>(Emote.TryParse(input, out var emote)
                ? TypeReaderResult.FromSuccess(emote)
                : TypeReaderResult.FromError(CommandError.ParseFailed, "Couldn't parse the input as an Emote"));
        }
    }
}