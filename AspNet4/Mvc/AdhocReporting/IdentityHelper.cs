using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using EqDemo.Models;
using EqDemo.Services;
using System.Configuration;

namespace EqDemo
{
    internal static class IdentityHelper
    {

        public static void SeedEqManagerRole()
        {

            const string eqManagerRole = "eq-manager";

            using (var context = ApplicationDbContext.Create())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                if (!roleManager.RoleExists(eqManagerRole)) {
                    roleManager.Create(new IdentityRole { Name = eqManagerRole });
                }
            }
        }

        public static void SeedDefaultUser()
        {
            const string defaultUserEmail = "demo@korzh.com";
            const string defaultUserPassword = "demo";

            using (var dbContext = ApplicationDbContext.Create())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbContext));
                // Configure validation logic for passwords
                userManager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 4,
                    RequireNonLetterOrDigit = false,
                    RequireDigit = false,
                    RequireLowercase = false,
                    RequireUppercase = false,
                };

                var resetDemoUserStr = ConfigurationManager.AppSettings["resetDefaultUser"];
                var resetDemoUser = resetDemoUserStr != null ? bool.Parse(resetDemoUserStr) : true;

                var user = userManager.FindByEmail(defaultUserEmail);

                //remove default user if "resetDefaultUser" option is set to true
                if (resetDemoUser && user != null) {
                    dbContext.Reports.RemoveRange(dbContext.Reports.Where(r => r.OwnerId == user.Id));
                    dbContext.SaveChanges();

                    userManager.DeleteAsync(user).GetAwaiter().GetResult();
                    user = null;
                }

                if (user == null) {
                    user = new ApplicationUser {
                        Email = defaultUserEmail,
                        UserName = defaultUserEmail,
                        EmailConfirmed = true
                    };

                    var result = userManager.Create(user, defaultUserPassword);
                    if (result.Succeeded) {
                        var reportGenerator = new DefaultReportGenerator(dbContext);
                        reportGenerator.Generate(user);
                    }
                }
            }
        }
    }
}