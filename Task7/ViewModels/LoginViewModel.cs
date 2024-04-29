using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Task7.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; } = null!;

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;
    }
}
