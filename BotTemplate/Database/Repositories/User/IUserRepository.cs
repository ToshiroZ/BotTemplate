using System.Threading.Tasks;

namespace BotTemplate.Database.Repositories.User
{
    public interface IUserRepository
    {
        void Update(Models.User user);
        Task UpdateAsync(Models.User user);
        Models.User Get(ulong userId);
    }
}