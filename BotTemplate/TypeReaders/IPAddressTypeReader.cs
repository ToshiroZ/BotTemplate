using Discord.Commands;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace BotTemplate.TypeReaders
{
    public class IPAddressTypeReader : BaseTypeReader<IPAddress>
    {
        public IPAddressTypeReader(DiscordShardedClient client, CommandService cmds) : base(client, cmds)
        {
        }

        public override async Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            try
            {
                var addresses = await Dns.GetHostAddressesAsync(input);
                return addresses.Length == 0 ? TypeReaderResult.FromError(CommandError.ParseFailed, "You need to input a valid IP Address or host name") : TypeReaderResult.FromSuccess(addresses.First());
            } 
            catch
            {
                return TypeReaderResult.FromError(CommandError.ParseFailed, "You need to input a valid IP Address or host name");
            }
        }
    }
}
