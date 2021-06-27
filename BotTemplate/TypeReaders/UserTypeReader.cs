using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace BotTemplate.TypeReaders
{
    public class UserTypeReader : BaseTypeReader<IUser>
    {
        public UserTypeReader(DiscordShardedClient client, CommandService cmds) : base(client, cmds)
        {
        }

        public override async Task<TypeReaderResult> ReadAsync(ICommandContext context, string input,
            IServiceProvider services)
        {
            if (ulong.TryParse(input, out var id))
            {
                return TypeReaderResult.FromSuccess(await context.Client.GetUserAsync(id));
            }
            else if (context.Message.MentionedUserIds.Any())
            {
                var user = await context.Client.GetUserAsync(context.Message.MentionedUserIds.First());
                return TypeReaderResult.FromSuccess(user);
            }
            else if (input.Contains("#"))
            {
                var split = input.Split("#");
                var user = await context.Client.GetUserAsync(split.First(), split.Last());
                return TypeReaderResult.FromSuccess(user);
            }
            else
            {
                try
                { 
                    var users = await context.Guild.GetUsersAsync();
                    var user = users.First(x => x.Username == input || x.Nickname == input);
                    return TypeReaderResult.FromSuccess(user);
                }
                catch
                {
                    return TypeReaderResult.FromError(new Exception("User not found"));
                }
            }
        }
    }
}