using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace BotTemplate.TypeConverters;

public class EmoteTypeConverter : BaseTypeConverter<Emote>
{

    public EmoteTypeConverter(DiscordShardedClient client, InteractionService interactive) : base(client, interactive)
    {
    }

    public override ApplicationCommandOptionType GetDiscordType()
    {
        return ApplicationCommandOptionType.String;
    }

    public override Task<TypeConverterResult> ReadAsync(IInteractionContext context,
        IApplicationCommandInteractionDataOption option, IServiceProvider services)
    {
        return Task.FromResult<TypeConverterResult>(Emote.TryParse(option.Value.ToString(), out var emote)
            ? TypeConverterResult.FromSuccess(emote)
            : TypeConverterResult.FromError(InteractionCommandError.ParseFailed, "Couldn't parse the input as an Emote"));
    }
}