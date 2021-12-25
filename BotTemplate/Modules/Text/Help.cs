using System.Threading.Tasks;
using Discord.Addons.CommandsExtension;
using Discord.Addons.Interactive;
using Discord.Commands;

namespace BotTemplate.Modules.Text
{
    public class Help : InteractiveBase
    {
        private readonly CommandService _commands;

        public Help(CommandService commands)
        {
            _commands = commands;
        }

        [Command("help"), Alias("assist", "h"), Summary("Shows help menu.")]
        public async Task HelpAsync([Remainder] string command = null)
        {
            var helpEmbed = _commands.GetDefaultHelpEmbed(command, "*");
            await Context.Channel.SendMessageAsync(embed: helpEmbed);
        }
    }
}