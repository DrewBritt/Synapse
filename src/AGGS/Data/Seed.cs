using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AGGS.Data
{
    public static class Seed
    {
        /// <summary>
        /// Ran on program initialization. Creates roles in context of the program.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            //Define Roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string[] roleNames = { "Admin", "Teacher", "Student" };
            IdentityResult roleResult;

            foreach (string roleName in roleNames)
            {
                //Seed roles to database if they don't exist
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Add superuser to "drewbritt02@gmail.com" account
            var superuser = await UserManager.FindByEmailAsync("drewbritt02@gmail.com");

            await UserManager.AddToRoleAsync(superuser, "Admin");
        }
    }
}
