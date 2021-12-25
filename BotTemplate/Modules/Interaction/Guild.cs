using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using BotTemplate.CustomContexts;
using BotTemplate.CustomModuleBases;
using Discord;
using Discord.Interactions;

namespace BotTemplate.Modules.Interaction;

[Group("guild", "guild related commands")]
public class Guild : CustomInteractionModuleBase<CustomShardedInteractionContext>
{
    private readonly IHttpClientFactory _factory;

    public Guild(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    [SlashCommand("stealemoji", "steals an emoji from discord"),
     RequireUserPermission(GuildPermission.ManageEmojisAndStickers)]
    public async Task StealEmojiAsync([Summary(description: "the emoji you want to steal")] Emote emote)
    {
        await DeferAsync(false);
        await using var stream = new MemoryStream(await _factory.CreateClient().GetByteArrayAsync(emote.Url));

        var addedEmote = await Context.Guild.CreateEmoteAsync(emote.Name, new Image(stream));
        await OkAsync($"Added {addedEmote}");
    }

    [SlashCommand("addemoji", "adds an emoji from url"),
     Discord.Interactions.RequireUserPermission(GuildPermission.ManageEmojisAndStickers)]
    public async Task AddEmojiAsync([Summary(description: "the name of the emoji you want to create")] string name,
        [Summary(description: "the url to the image")] string url)
    {
        await DeferAsync(false);
        await using var stream = new MemoryStream(await _factory.CreateClient().GetByteArrayAsync(url));
        if (stream.Length > 256 * 1000)
        {
            await ErrorAsync("Image is too large");
            return;
        }

        var emote = await Context.Guild.CreateEmoteAsync(name, new Image(stream));
        await OkAsync($"Added {emote}");
    }
}