using Task7.Enums;

namespace Task7.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Body { get; set; } = string.Empty;
        public MessageStatus Status { get; set; }
        public MessageType MessageType { get; set; }
        public string Sender { get; set; } = null!;
        public DateTime DateTime { get; set; }

        public Chat Chat { get; set; } = null!;
    }
}
