using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CreationApp.Services
{
    public class ServiceBusWorker : IHostedService, IDisposable
    {
        private readonly IServiceBusConsumer _serviceBusConsumer;

        public ServiceBusWorker(IServiceBusConsumer serviceBusConsumer)
        {
            _serviceBusConsumer = serviceBusConsumer;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _serviceBusConsumer.RegisterOnMessageHandlerAndReceiveMessages().ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _serviceBusConsumer.CloseQueueAsync().ConfigureAwait(false);
        }

        public async void Dispose()
        {
            await _serviceBusConsumer.DisposeAsync().ConfigureAwait(false);
        }
    }
}
