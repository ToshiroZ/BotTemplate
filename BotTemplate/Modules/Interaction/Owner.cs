using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using BotTemplate.CustomContexts;
using BotTemplate.CustomModuleBases;
using Discord;
using Discord.Interactions;

namespace BotTemplate.Modules.Interaction;

[Group("owner", "Commands only the owner of the bot can run"), RequireOwner]
public class Owner : CustomInteractionModuleBase<CustomShardedInteractionContext>
{
    private readonly IHttpClientFactory _factory;

    public Owner(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    [SlashCommand("setprofilepicture", "Sets the bots profile picture")]
    public async Task SetProfilePictureAsync([Discord.Interactions.Summary(description: "the url to the image")]string url)
    {
        await DeferAsync(true);
        if (url is null)
        {
            await ErrorAsync("Couldn't find an image");
            return;
        }
        
        await using var stream = new MemoryStream(await _factory.CreateClient().GetByteArrayAsync(url));
        await Context.Client.CurrentUser.ModifyAsync(x => x.Avatar = new Image(stream));
        var embed = new EmbedBuilder
        {
            Title = "Ok",
            Description = "Changed Profile Picture",
            ImageUrl = Context.Client.CurrentUser.GetAvatarUrl()
        };
        await FollowupAsync(null, embed: embed.Build());
    }
    [SlashCommand("changeusername", "Sets the bots username")]
    public async Task SetUsernameAsync(string name)
    {
        await DeferAsync(true);
        await Context.Client.CurrentUser.ModifyAsync(x => x.Username=name);
        await OkAsync("Changed Username");
    }
}