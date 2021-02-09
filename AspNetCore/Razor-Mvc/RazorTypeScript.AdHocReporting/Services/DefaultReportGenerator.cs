using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

using Newtonsoft.Json.Linq;

using EqDemo.Models;

namespace EqDemo.Services
{
    public class DefaultReportGenerator
    {
        private string _dataPath;
        private AppDbContext _dbContext;

        private const string _modelId = "adhoc-reporting";

        public DefaultReportGenerator(IWebHostEnvironment env, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dataPath = Path.Combine(env.ContentRootPath, $"App_Data\\Seed");
        }

        public async Task GenerateAsync(IdentityUser user)
        {
            var reportJsons = GetReportJsons();
            foreach (var json in reportJsons) {
                var jobject = JObject.Parse(json);
                var reportId = Guid.NewGuid().ToString();
                jobject["id"] = reportId;
                var report = new Report
                {
                    Id = reportId,
                    OwnerId = user.Id,
                    Name = jobject["name"]?.ToString(),
                    Description = jobject["desc"]?.ToString(),
                    ModelId = _modelId,
                    QueryJson = jobject.ToString()
                };

                await _dbContext.AddAsync(report);
            }

            await _dbContext.SaveChangesAsync();

        }

        private IEnumerable<string> GetReportJsons()
        {
            var files = Directory.GetFiles(_dataPath, "*.json");
            foreach (var file in files) {
                yield return File.ReadAllText(file);
            }
        }
    }
}
