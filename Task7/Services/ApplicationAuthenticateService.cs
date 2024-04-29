using Microsoft.AspNetCore.Identity;
using Task7.Entities;
using Task7.Interfaces.Services;
using Task7.ViewModels;

namespace Task7.Services
{
    public class ApplicationAuthenticateService : IApplicationAuthenticateService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ApplicationAuthenticateService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user is null)
                return false;

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (result)

                await _signInManager.SignInAsync(user, false);

            return result;
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Name = model.Name,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
            }

            return result;
        }
    }
}
