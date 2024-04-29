using Task7.Enums;

namespace Task7.Dto
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Body { get; set; } = string.Empty;
        public MessageStatus Status { get; set; }
        public MessageType MessageType { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsMyMessage { get; set; } 
    }
}
