using MassTransit;
using Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClientA
{
    public class SpamService : IHostedService
    {
        int maxMessages = 10000;
        ILogger logger;
        IBus bus;

        public SpamService(IBus bus, ILogger<SpamService> logger)
        {
            this.logger = logger;
            this.bus = bus;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            IRequestClient<Message> spammer = bus.CreateRequestClient<Message>();
            for (int i = 0; i < maxMessages; i++)
            {
                try
                {
                    Response<MessageResponse> response = await spammer.GetResponse<MessageResponse>(new Message { MessageContent = $"Spam, spam spam, spammity spam, spam, spam!" });
                    logger.LogInformation($"Recieved message: {response.Message.Response}");
                    Task.Delay(2000).Wait();
                }
                catch(Exception ex)
                {
                    logger.LogError(ex, "Error sending spam");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
