using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection.Emit;
using Task7.Data.Seeds;
using Task7.Entities;

namespace Task7.Data
{
    public class DBContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DBContext(DbContextOptions options) : base(options)
        {
            var dbCreater = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (dbCreater != null)
            {
                // Create Database 
                if (!dbCreater.CanConnect())
                {
                    dbCreater.Create();
                }

                // Create Tables
                if (!dbCreater.HasTables())
                {
                    dbCreater.CreateTables();
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<ApplicationUserChat>()

                .HasKey(ac => new { ac.ChatId, ac.ApplicationUserId })
                ;

            builder.Entity<ApplicationUserChat>()
                .HasOne(ac => ac.ApplicationUser)
                .WithMany(au => au.ApplicationUserChats)
                .HasForeignKey(ac => ac.ApplicationUserId);

            builder.Entity<ApplicationUserChat>()
                .HasOne(ac => ac.Chat)
                .WithMany(c => c.ApplicationUserChats)
                .HasForeignKey(ac => ac.ChatId);

            builder.SeedApplicationUsers();

            base.OnModelCreating(builder);
        }
    }
}
