using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using BotTemplate.Database.Models;
using BotTemplate.Database.Repositories;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;

namespace BotTemplate.Modules.Text
{
    public class GuildCommands : InteractiveBase
    {
        private readonly IHttpClientFactory _factory;
        private readonly IRepositoryGrouper _repos;

        public GuildCommands(IHttpClientFactory factory, IRepositoryGrouper repos)
        {
            _factory = factory;
            _repos = repos;
        }

        [Command("stealemoji"), RequireUserPermission(GuildPermission.ManageEmojisAndStickers)]
        public async Task StealEmojiAsync(Emote emote)
        {
            await using var stream = new MemoryStream(await _factory.CreateClient().GetByteArrayAsync(emote.Url));
            
            var addedEmote = await Context.Guild.CreateEmoteAsync(emote.Name, new Image(stream));
            await OkAsync($"Added {addedEmote}");
        }
        [Command("addemoji"), RequireUserPermission(GuildPermission.ManageEmojisAndStickers)]
        public async Task StealEmojiAsync(string name, string url)
        {
            await using var stream = new MemoryStream(await _factory.CreateClient().GetByteArrayAsync(url));
            if (stream.Length > 256 * 1000)
            {
                await ErrorAsync("Image is too large");
                return;
            }

            await Context.Guild.CreateEmoteAsync(name, new Image(stream));
        }

        [Command("welcome"), RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task WelcomeAsync()
        {
            var guild = GetGuild(Context.Guild);

            guild.Settings.WelcomeUsers = !guild.Settings.WelcomeUsers;
            await _repos.Guild.UpdateAsync(guild);

            if (guild.Settings.WelcomeUsers)
            {
                await OkAsync("Welcoming Users is on");
            }
            else
            {
                await OkAsync("Welcoming Users is off");
            }
        }

        private Guild GetGuild(IGuild guild)
        {
            var dbguild = _repos.Guild.Get(guild.Id);
            if (dbguild is not null) 
                return dbguild;
            dbguild ??= new Database.Models.Guild
            {
                GuildId = guild.Id,
                Settings = new GuildSettings()
            };
            _repos.Guild.Update(dbguild);

            return dbguild;
        }
    }
}