using System.Threading.Tasks;

namespace CreationApp.Services
{
    public interface IServiceBusConsumer
    {
        public Task RegisterOnMessageHandlerAndReceiveMessages();

        public Task CloseQueueAsync();

        public ValueTask DisposeAsync();
    }
}
