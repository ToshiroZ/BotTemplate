using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;

namespace BotTemplate.Modules
{
    public class Owner : InteractiveBase
    {
        private readonly IHttpClientFactory _factory;

        public Owner(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        [Command("setprofilepicture"), Alias("setprofilepic"), Summary("Sets the bots profile picture")]
        public async Task SetProfilePictureAsync(string url = null)
        {
            if (url is null && Context.Message.Attachments.Count == 0)
            {
                await ErrorAsync("Couldn't find an image");
                return;
            }

            url ??= Context.Message.Attachments.First().Url;
            await using var stream = new MemoryStream(await _factory.CreateClient().GetByteArrayAsync(url));
            await Context.Client.CurrentUser.ModifyAsync(x => x.Avatar = new Image(stream));
            var embed = new EmbedBuilder
            {
                Title = "Ok",
                Description = "Changed Profile Picture",
                ImageUrl = Context.Client.CurrentUser.GetAvatarUrl()
            };
            await ReplyAndDeleteAsync(null, embed: embed.Build());
        }
        [Command("changeusername"), Alias("changename"), Summary("Sets the bots username")]
        public async Task SetUsernameAsync([Remainder]string name = null)
        {
            await Context.Client.CurrentUser.ModifyAsync(x => x.Username=name);
            await OkAsync("Changed Username");
        }
    }
}