namespace Messages
{
    public class Message : IMessage
    {
        public string MessageContent { get; set; }
    }

    public interface IMessage
    {

    }

    public class MessageResponse
    {
        public string Response { get; set; }
    }
}
