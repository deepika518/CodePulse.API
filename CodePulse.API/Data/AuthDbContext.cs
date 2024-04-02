using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        // seeding the roles using entity framework core
        // 1. reader role for public
        // 2. read-write role for admin

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "1ad5b4a4-4017-4851-bb0b-1a8a8e3ec706";
            var writerRoleId = "a660d6a0-cac4-4dd5-a86c-d708569a1c69";


            //Create reader and writer roles
            var roles = new List<IdentityRole>
            {
                //Reader role
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                // Writer Role
                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };

            //Seed the roles

            builder.Entity<IdentityRole>().HasData(roles);

            //Create a default Admin user
            var adminUserId = "efc5c094-f86d-4230-8160-e82dcd72ac53";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@codepulse.com",
                Email = "admin@codepulse.com",
                NormalizedEmail = "admin@codepulse.com".ToUpper(),
                NormalizedUserName = "admin@codepulse.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            //seed the admin inside DB

            builder.Entity<IdentityUser>().HasData(admin);

            //Give Roles to Admin (Both Reader and Writer)

            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId
                }
            };

            //seeding the roles
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

        }

    }
}
