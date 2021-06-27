namespace BotTemplate.Database.Models
{
    public class Guild : Entity
    {
        public ulong GuildId { get; set; }
        public virtual GuildSettings Settings { get; set; }
    }
}