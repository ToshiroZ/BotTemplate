using System;
using System.Threading.Tasks;
using BotTemplate.Database.Repositories.Guild;
using BotTemplate.Database.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace BotTemplate.Database.Repositories
{
    public class RepositoryGrouper : IRepositoryGrouper
    {
        private readonly DatabaseContext _context;
        public RepositoryGrouper(DatabaseContext context)
        {
            context.Database.Migrate();
            _context = context;
        }
        private IUserRepository _user;
        public IUserRepository User => _user ??= new UserRepository(_context);

        private IGuildRepository _guild;
        public IGuildRepository Guild => _guild ??= new GuildRepository(_context);


        public int SaveChanges() =>
            _context.SaveChanges();

        public Task<int> SaveChangesAsync() =>
            _context.SaveChangesAsync();

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
