using Discord;
using Discord.Commands;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace BotTemplate.Preconditions
{
    public class Cooldown : PreconditionAttribute
    {
        private TimeSpan CooldownLength { get; set; }
        private bool AdminsAreLimited { get; set; }
        readonly ConcurrentDictionary<CooldownInfo, DateTime> _cooldowns = new ConcurrentDictionary<CooldownInfo, DateTime>();

        public Cooldown(int seconds, bool adminsAreLimited = false)
        {
            CooldownLength = TimeSpan.FromSeconds(seconds);
            AdminsAreLimited = adminsAreLimited;
        }
        public struct CooldownInfo
        {
            public ulong UserId { get; }
            public int CommandHashCode { get; }
            public CooldownInfo(ulong userId, int commandHashCode)
            {
                UserId = userId;
                CommandHashCode = commandHashCode;
            }
        }
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if(context.User.Id == 773003001710772224) return Task.FromResult(PreconditionResult.FromSuccess());
            if (!AdminsAreLimited && context.User is IGuildUser {GuildPermissions: {Administrator: true}})
                return Task.FromResult(PreconditionResult.FromSuccess());

            var key = new CooldownInfo(context.User.Id, command.GetHashCode());
            if (_cooldowns.TryGetValue(key, out var endsAt))
            {
                var difference = endsAt.Subtract(DateTime.UtcNow);
                if (difference.Ticks > 0)
                {
                    return Task.FromResult(PreconditionResult.FromError($"You can use this command in {difference.ToHms()}"));
                }
                var time = DateTime.UtcNow.Add(CooldownLength);
                _cooldowns.TryUpdate(key, time, endsAt);
            }
            else
            {
                _cooldowns.TryAdd(key, DateTime.UtcNow.Add(CooldownLength));
            }

            return Task.FromResult(PreconditionResult.FromSuccess());
        }
    }
}
