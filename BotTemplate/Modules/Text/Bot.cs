using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;

namespace BotTemplate.Modules.Text
{
    
    public class Bot : InteractiveBase
    {
        [Command("about"), Summary("Gets you info about the bot")]
        public async Task AboutAsync()
        {
            var appInfo = await Context.Client.GetApplicationInfoAsync();
            var embed = new EmbedBuilder()
                .WithRandomColor()
                .AddField("Created By", appInfo.Owner.ToString(), true)
                .AddField("Bot Made On", appInfo.CreatedAt.UtcDateTime.ToString("MM/dd/yyyy hh:mm:ss tt"), true)
                .Build();
            await ReplyAndDeleteAsync("", embed: embed);
        }
    }
}