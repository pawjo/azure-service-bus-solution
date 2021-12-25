using Azure.Messaging.ServiceBus;
using CreationApp.Models;
using CreationApp.Settings;
using System.Text.Json;
using System.Threading.Tasks;

namespace CreationApp.Services
{
    public class MessagingService : IMessagingService
    {
        private readonly ServiceBusSettings _settings;

        public MessagingService(ServiceBusSettings settings)
        {
            _settings = settings;
        }

        public async Task<bool> SendCreateMessageAsync(int userId)
        {
            var message = new Message
            {
                Type = 1,
                UserId = userId
            };

            return await SendMessageAsync(message);
        }

        public async Task<bool> SendEditMessageAsync(int userId)
        {
            var message = new Message
            {
                Type = 2,
                UserId = userId
            };

            return await SendMessageAsync(message);
        }

        public async Task<bool> SendMessageAsync(Message message)
        {
            await using (var client = new ServiceBusClient(_settings.ConnectionString))
            await using (var sender = client.CreateSender(_settings.QueueName))
            {
                var jsonMessage = JsonSerializer.Serialize(message);
                var sbMessage = new ServiceBusMessage(jsonMessage);
                try
                {
                    await sender.SendMessageAsync(sbMessage);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
