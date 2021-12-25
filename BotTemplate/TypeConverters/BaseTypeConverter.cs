using Discord.Interactions;
using Discord.WebSocket;

namespace BotTemplate.TypeConverters;

public abstract class BaseTypeConverter<T> : TypeConverter<T>
{
    private readonly DiscordShardedClient _client;
    private readonly InteractionService _interactive;

    private BaseTypeConverter()
    {
        
    }
    protected BaseTypeConverter(DiscordShardedClient client, InteractionService interactive)
    {
        _client = client;
        _interactive = interactive;
    }
}