using Microsoft.AspNetCore.Identity;

namespace Task7.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = null!;
        public ICollection<ApplicationUserChat> ApplicationUserChats { get; set; }
    }
}
