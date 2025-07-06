using Animal_Health_System.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.DAL.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "Owner", "Veterinarian", "FarmStaff", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. دالة مساعدة لإنشاء المستخدم لو مش موجود
            async Task CreateUserIfNotExists(string userName, string email, string role, string password)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    var newUser = new ApplicationUser
                    {
                        UserName = userName,
                        Email = email,
                        EmailConfirmed = true,
                        Role = role
                    };
                    var result = await userManager.CreateAsync(newUser, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newUser, role);
                    }
                    else
                    {
                        // يمكنك هنا تسجيل الأخطاء إذا أحببت
                        throw new Exception($"Failed to create user {userName}: {string.Join(", ", result.Errors)}");
                    }
                }
            }

            await CreateUserIfNotExists("Admin", "admin@animal.com", "Admin", "Sohaib@18");
            await CreateUserIfNotExists("Owner", "owner@animal.com", "Owner", "Sohaib@18");
            await CreateUserIfNotExists("Veterinarian", "veterinarian@animal.com", "Veterinarian", "Sohaib@18");
            await CreateUserIfNotExists("farmstaff", "farmstaff@animal.com", "FarmStaff", "Sohaib@18");
            await CreateUserIfNotExists("User", "user@animal.com", "User", "Sohaib@18");
        }
    }
}
