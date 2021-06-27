using System.Linq;
using System.Threading.Tasks;

namespace BotTemplate.Database.Repositories.Guild
{
    public class GuildRepository : IGuildRepository
    {
        private readonly DatabaseContext _database;
        public GuildRepository(DatabaseContext database)
        {
            _database = database;
        }

        public void Update(Models.Guild guild)
        {
            if (Get(guild.GuildId) is null)
            {
                _database.Guilds.Add(guild);
            }
            else
            {
                _database.Update(guild);
            }
            _database.SaveChanges();
        }
        public async Task UpdateAsync(Models.Guild guild)
        {
            if (Get(guild.GuildId) is null)
            {
                _database.Guilds.Add(guild);
            }
            else
            {
                _database.Update(guild);
            }
            await _database.SaveChangesAsync();
        }
        public Models.Guild Get(ulong guildid)
        {
            try
            {
                return _database.Guilds.First(guild => guild.GuildId.Equals(guildid));
            }
            catch
            {
                return null;
            }
        }
    }
}