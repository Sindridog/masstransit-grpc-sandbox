using MassTransit;
using Messages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ClientB
{
    public class MessageConsumer : IConsumer<Message>
    {
        ILogger logger;

        public MessageConsumer(ILogger<MessageConsumer> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<Message> context)
        {
            logger.LogInformation($"Recieved message: {context.Message.MessageContent}");
            return context.RespondAsync(new MessageResponse { Response = "Shut up!" });
        }
    }
}
