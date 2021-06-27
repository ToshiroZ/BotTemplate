using System;
using System.Linq;
using System.Threading.Tasks;

namespace BotTemplate.Database.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _database;
        public UserRepository(DatabaseContext database)
        {
            _database = database;
        }

        public void Update(Models.User user)
        {
            if (Get(user.UserId) is null)
            {
                _database.Users.Add(user);
            }
            else
            {
                _database.Update(user);
            }
            _database.SaveChanges();
        }
        public async Task UpdateAsync(Models.User user)
        {
            if (Get(user.UserId) is null)
            {
                _database.Users.Add(user);
            }
            else
            {
                _database.Update(user);
            }
            await _database.SaveChangesAsync();
        }
        public Models.User Get(ulong userId)
        {
            try
            {
                return _database.Users.FirstOrDefault(u => u.UserId.Equals(userId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}