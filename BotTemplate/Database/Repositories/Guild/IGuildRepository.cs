using System.Threading.Tasks;

namespace BotTemplate.Database.Repositories.Guild
{
    public interface IGuildRepository
    {
        void Update(Models.Guild user);
        Task UpdateAsync(Models.Guild user);
        Models.Guild Get(ulong guildid);
    }
}