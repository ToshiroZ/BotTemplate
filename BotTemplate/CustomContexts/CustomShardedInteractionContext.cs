using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace BotTemplate.CustomContexts
{
    public class CustomShardedInteractionContext : ShardedInteractionContext, ICustomContext
    {
        private readonly DiscordShardedClient client;
        private readonly SocketInteraction interaction;

        public CustomShardedInteractionContext(DiscordShardedClient client, SocketInteraction interaction) : base(client, interaction)
        {
            this.client = client;
            this.interaction = interaction;
        }
        
        public async Task<IUserMessage> OkAsync(string message, bool ephemeral = true)
        {
            return await interaction.FollowupAsync(embed: new EmbedBuilder
                {Color = Color.Green, Description = message, Title = "OK"}.Build(), ephemeral: ephemeral);
        }
        public async Task<IUserMessage> ErrorAsync(string message, bool ephemeral = true)
        {
            return await interaction.FollowupAsync(embed: new EmbedBuilder
                {Color = Color.Red, Description = message, Title = "Error"}.Build(), ephemeral: ephemeral);
        }
    }
}