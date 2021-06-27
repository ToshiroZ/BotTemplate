using System;
using System.Threading.Tasks;
using BotTemplate.Database.Repositories.Guild;
using BotTemplate.Database.Repositories.User;

namespace BotTemplate.Database.Repositories
{
    public interface IRepositoryGrouper : IDisposable
    {
        IUserRepository User { get; }
        IGuildRepository Guild { get; }
        

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
