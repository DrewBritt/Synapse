﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Synapse.Data
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
            var drewsuperuser = await UserManager.FindByEmailAsync("drewbritt02@gmail.com");
            var lyonsuperuser = await UserManager.FindByEmailAsync("lyonjenkins@gmail.com");
            var teacheraccount = await UserManager.FindByEmailAsync("teacheraccount@gmail.com");

            await UserManager.AddToRoleAsync(drewsuperuser, "Admin");
            await UserManager.AddToRoleAsync(lyonsuperuser, "Admin");
            await UserManager.AddToRoleAsync(teacheraccount, "Teacher");
        }
    }
}
