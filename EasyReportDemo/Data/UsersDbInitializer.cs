using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EasyReportDemo.Models;


namespace EasyReportDemo.Data
{
    public class UsersDbInitializer {
        public static async Task RolesInitialize(IServiceProvider serviceProvider) {
            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (await roleManager.FindByNameAsync("admin") == null) { 
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null) {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

        }

        public static async Task UsersInitialize(IServiceProvider serviceProvider) {
            UserManager<ApplicationUser> userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string emailJohn = "john.dow@easyquerybuilder.com";
            string passwordJohn = "john1234";
            string emailAlice = "alice.dow@easyquerybuilder.com";
            string passwordAlice = "alice1234";
            if (await userManager.FindByNameAsync(emailJohn) == null) {
                ApplicationUser admin = new ApplicationUser { Email = emailJohn, UserName = emailJohn };
                IdentityResult result = await userManager.CreateAsync(admin, passwordJohn);
                if (result.Succeeded) {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
            if (await userManager.FindByNameAsync(emailAlice) == null) {
                ApplicationUser user = new ApplicationUser { Email = emailAlice, UserName = emailAlice };
                IdentityResult result = await userManager.CreateAsync(user, passwordAlice);
                if (result.Succeeded) {
                    await userManager.AddToRoleAsync(user, "user");
                }
            }

        }
    }
}
