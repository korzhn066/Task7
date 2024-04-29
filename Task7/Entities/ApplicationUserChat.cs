namespace Task7.Entities
{
    public class ApplicationUserChat
    {
        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public int ChatId { get; set; }
        public Chat Chat { get; set; } = null!;
    }
}
