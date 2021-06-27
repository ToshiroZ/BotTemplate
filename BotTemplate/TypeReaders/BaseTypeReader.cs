using Discord.Commands;
using Discord.WebSocket;

namespace BotTemplate.TypeReaders
{
    public abstract class BaseTypeReader<T> : TypeReader
    {
        private readonly DiscordShardedClient _client;
        private readonly CommandService _cmds;

        private BaseTypeReader() { }
        protected BaseTypeReader(DiscordShardedClient client, CommandService cmds)
        {
            _client = client;
            _cmds = cmds;
        }
    }
}
