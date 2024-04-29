using Microsoft.AspNetCore.Identity;
using Task7.ViewModels;

namespace Task7.Interfaces.Services
{
    public interface IApplicationAuthenticateService
    {
        Task LogOutAsync();
        Task<bool> LoginAsync(LoginViewModel model);
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
    }
}
