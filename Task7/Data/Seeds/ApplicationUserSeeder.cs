using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task7.Entities;

namespace Task7.Data.Seeds
{
    internal static class ApplicationUserSeeder
    {
        internal static void SeedApplicationUsers(this ModelBuilder builder)
        {
            var users = new List<ApplicationUser>() {
                new ApplicationUser()
                {
                    Id = "1e445865-a24d-4543-a6c6-9443d048cdb9",
                    UserName = "1",
                    NormalizedUserName = "1".ToUpper(),
                    Name = "Nikita",
                },
                new ApplicationUser()
                {
                    Id = "2e445865-a24d-4543-a6c6-9443d048cdb9",
                    UserName = "2",
                    NormalizedUserName = "2".ToUpper(),
                    Name = "Pavel",
                },
                new ApplicationUser()
                {
                    Id = "3e445865-a24d-4543-a6c6-9443d048cdb9",
                    UserName = "3",
                    NormalizedUserName = "3".ToUpper(),
                    Name = "Egor",
                },
            };

            var hasher = new PasswordHasher<ApplicationUser>();

            users[0].PasswordHash = hasher.HashPassword(users[0], "1");
            users[1].PasswordHash = hasher.HashPassword(users[1], "2");
            users[2].PasswordHash = hasher.HashPassword(users[2], "3");

            builder.Entity<ApplicationUser>().HasData(users);
        }
    }
}
