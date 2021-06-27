using System.Threading.Tasks;
using BotTemplate.Database.Models;
using BotTemplate.Database.Repositories;
using Discord.WebSocket;

namespace BotTemplate.Services
{
    public class JoinedGuild
    {
        private readonly DiscordShardedClient _client;
        private readonly IRepositoryGrouper _repos;

        public JoinedGuild(DiscordShardedClient client, IRepositoryGrouper repos)
        {
            _client = client;
            _repos = repos;

            _client.JoinedGuild += OnUserJoined;
        }

        private async Task OnUserJoined(SocketGuild arg)
        {
            var dbguild = _repos.Guild.Get(arg.Id);
            dbguild ??= new Guild
            {
                GuildId = arg.Id,
                Settings = new GuildSettings()
            };
            await _repos.Guild.UpdateAsync(dbguild);
        }
    }
}