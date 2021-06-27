using System.Threading.Tasks;
using BotTemplate.Database.Repositories;
using Discord;
using Discord.WebSocket;

namespace BotTemplate.Services
{
    public class UserJoined
    {
        private readonly DiscordShardedClient _client;
        private readonly IRepositoryGrouper _repos;
        public UserJoined(DiscordShardedClient client, IRepositoryGrouper repos)
        {
            _client = client;
            _repos = repos;

            _client.UserJoined += OnUserJoined;
        }

        private async Task OnUserJoined(SocketGuildUser arg)
        {
            var dbguild = _repos.Guild.Get(arg.Guild.Id);
            if (dbguild?.Settings.WelcomeUsers == true)
            {
                await arg.SendMessageAsync($"Welcome to {arg.Guild.Name}");
            }
        }
    }
}