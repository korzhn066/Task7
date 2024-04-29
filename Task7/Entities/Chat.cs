namespace Task7.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public ICollection<ApplicationUserChat> ApplicationUserChats { get; set; } 
        public ICollection<Message> Messages { get; set; }
    }
}
