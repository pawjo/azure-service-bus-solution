using CreationApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CreationApp.Services
{
    public interface IUserService
    {
        public Task<bool> AddAsync(User newUser);

        public Task<List<User>> GetActiveListAsync();

        public Task<User> GetByIdAsync(int id);

        public Task<List<User>> GetListAsync();

        public Task<bool> UpdateAsync(User user);
    }
}
