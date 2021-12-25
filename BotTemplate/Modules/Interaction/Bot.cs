using System.Threading.Tasks;
using BotTemplate.CustomContexts;
using BotTemplate.CustomModuleBases;
using Discord;
using Discord.Interactions;

namespace BotTemplate.Modules.Interaction;

[Group("bot", "Commands in the bot category")]
public class Bot : CustomInteractionModuleBase<CustomShardedInteractionContext>
{
    public Bot()
    {
        
    }
    [SlashCommand("about", "Gets you info about the bot")]
    public async Task AboutAsync()
    {
        await DeferAsync(true);
        var appInfo = await Context.Client.GetApplicationInfoAsync();
        var embed = new EmbedBuilder()
            .WithRandomColor()
            .AddField("Created By", appInfo.Owner.ToString(), true)
            .AddField("Bot Made On", appInfo.CreatedAt.UtcDateTime.ToString("MM/dd/yyyy hh:mm:ss tt"), true)
            .Build();
        await FollowupAsync("", embed: embed);
    }
}