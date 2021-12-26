using CreationApp.Models;
using System.Threading.Tasks;

namespace CreationApp.Services
{
    public interface IUserService
    {
        public Task<bool> ActivateAsync(int userId);

        public Task<User> GetByIdAsync(int id);

        public Task<bool> ValidateAsync(int userId);
    }
}
