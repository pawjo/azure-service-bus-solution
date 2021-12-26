using Azure.Messaging.ServiceBus;
using CreationApp.Models;
using CreationApp.Settings;
using System;
using System.Threading.Tasks;

namespace CreationApp.Services
{
    public class ServiceBusConsumer : IServiceBusConsumer
    {
        private readonly ServiceBusClient _client;
        private ServiceBusProcessor _processor;
        private readonly ServiceBusSettings _settings;
        private readonly IUserService _userService;

        public ServiceBusConsumer(ServiceBusSettings settings, IUserService userService)
        {
            _settings = settings;
            _userService = userService;
            _client = new ServiceBusClient(_settings.ConnectionString);
        }

        public async Task RegisterOnMessageHandlerAndReceiveMessages()
        {
            _processor = _client.CreateProcessor(_settings.QueueName);
            _processor.ProcessMessageAsync += ProcessMessagesAsync;
            _processor.ProcessErrorAsync += ProcessErrorAsync;
            await _processor.StartProcessingAsync().ConfigureAwait(false);
        }

        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            var message = args.Message.Body.ToObjectFromJson<Message>();

            bool isValid = await _userService.ValidateAsync(message.UserId);

            if (isValid)
            {
                await _userService.ActivateAsync(message.UserId);
            }
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            return Task.CompletedTask;
        }


        public async Task CloseQueueAsync()
        {
            await _processor.CloseAsync().ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            if (_processor != null)
            {
                await _processor.DisposeAsync().ConfigureAwait(false);
            }

            if (_client != null)
            {
                await _client.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}
