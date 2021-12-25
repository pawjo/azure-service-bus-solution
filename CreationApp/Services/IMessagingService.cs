using CreationApp.Models;
using System.Threading.Tasks;

namespace CreationApp.Services
{
    public interface IMessagingService
    {
        public Task<bool> SendCreateMessageAsync(int userId);

        public Task<bool> SendEditMessageAsync(int userId);

        public Task<bool> SendMessageAsync(Message message);
    }
}
