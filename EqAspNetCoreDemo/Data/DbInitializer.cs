using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;

using Korzh.EasyQuery.Services;

using EqAspNetCoreDemo.Services;

namespace EqAspNetCoreDemo.Data
{
    public class DbInitializer
    {
        const string defaultUserEmail = "demo@korzh.com";
        const string defaultUserPassword = "demo";

        private IServiceProvider _services;
        private string _connectionString;
        private SqlConnection _connection;
        private string _scriptFilePath;


        public DbInitializer(IServiceProvider services, IConfiguration config, string dbName, string scriptFilePath)
        {
            _services = services;
            _connectionString = config.GetConnectionString(dbName);
            _connection = new SqlConnection(_connectionString);
            _scriptFilePath = scriptFilePath;
        }

        public void AddTestData()
        {
            try
            {
                _connection.Open();

                if (IsEmptyDb())
                {
                    FillDb();
                }

                _connection.Close();

                CheckAddDefaultUser();
                CheckAddManagerRole();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        private bool IsEmptyDb()
        {
            var fillDbCommand = _connection.CreateCommand();
            fillDbCommand.CommandText = "SELECT TOP(1) CategoryID FROM dbo.Categories";

            Object someId = fillDbCommand.ExecuteScalar();

            return someId == null;
        }

        private void FillDb()
        {
            var fillDbCommand = _connection.CreateCommand();

            fillDbCommand.CommandText = System.IO.File.ReadAllText(_scriptFilePath);
            fillDbCommand.CommandTimeout = 300;

            fillDbCommand.ExecuteNonQuery();
        }

        private async void CheckAddDefaultUser()
        {
            using (var scope = _services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;

                var manager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                try
                {
                    IdentityUser user = await manager.FindByEmailAsync(defaultUserEmail);
                    if (user == null)
                    {
                        user = new IdentityUser()
                        {
                            UserName = defaultUserEmail,
                            Email = defaultUserEmail,
                            EmailConfirmed = true
                        };
                        var result = await manager.CreateAsync(user, defaultUserPassword);
                        if (result.Succeeded)
                        {
                            var defaultReportsGenerator = scope.ServiceProvider.GetRequiredService<DefaultReportGenerator>();
                            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                            await defaultReportsGenerator.GenerateAsync(user);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private async void CheckAddManagerRole()
        {
            using (var scope = _services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;

                var manager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                try
                {
                    IdentityRole role = await manager.FindByNameAsync(DefaultEqAuthProvider.EqManagerRole);
                    if (role == null)
                    {
                        role = new IdentityRole(DefaultEqAuthProvider.EqManagerRole);
                        var result = await manager.CreateAsync(role);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}