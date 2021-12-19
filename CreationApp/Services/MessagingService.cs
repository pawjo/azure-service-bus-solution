using Azure.Messaging.ServiceBus;
using CreationApp.Settings;
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

        public async Task<bool> SendMessage(string messageBody)
        {
            await using (var client = new ServiceBusClient(_settings.ConnectionString))
            await using (var sender = client.CreateSender(_settings.QueueName))
            {
                var message = new ServiceBusMessage(messageBody);
                try
                {
                    await sender.SendMessageAsync(message);
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
