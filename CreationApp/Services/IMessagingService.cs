using System.Threading.Tasks;

namespace CreationApp.Services
{
    public interface IMessagingService
    {
        public Task<bool> SendMessage(string message);
    }
}
