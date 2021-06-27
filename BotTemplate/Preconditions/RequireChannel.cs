using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BotTemplate.Preconditions
{
    public class RequireChannel : PreconditionAttribute
    {
        private readonly ulong[] _channelIds;
        private readonly bool _dmChannelIgnore;
        public RequireChannel(bool dmChannelIgnore, params ulong[] channels)
        {
            _dmChannelIgnore = dmChannelIgnore;
            _channelIds = channels;
        }
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (_dmChannelIgnore && context.Channel is IPrivateChannel || _channelIds.Contains(context.Channel.Id))
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
            return Task.FromResult(PreconditionResult.FromError("This command can not be executed in this channel"));
        }
    }
}
