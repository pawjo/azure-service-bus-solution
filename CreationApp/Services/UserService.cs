using CreationApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreationApp.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;

        public UserService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> AddAsync(User newUser)
        {
            _dataContext.Users.Add(newUser);
            var added = await _dataContext.SaveChangesAsync();

            return added == 1;
        }

        public async Task<List<User>> GetActiveListAsync()
        {
            return await _dataContext.Users.Where(x => x.Active).ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _dataContext.Users.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<User>> GetListAsync()
        {
            return await _dataContext.Users.ToListAsync();
        }

        public async Task<bool> UpdateAsync(User modifiedUser)
        {
            var storedUser = await _dataContext.Users.SingleOrDefaultAsync(x => x.Id == modifiedUser.Id);

            if (storedUser == null)
            {
                return false;
            }

            storedUser.Email = modifiedUser.Email;
            storedUser.Name = modifiedUser.Name;
            storedUser.Name = modifiedUser.Name;
            storedUser.Surname = modifiedUser.Surname;
            storedUser.Active = modifiedUser.Active;

            var updated = await _dataContext.SaveChangesAsync();

            return updated == 1;
        }
    }
}
