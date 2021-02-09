using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.AspNet.Identity.EntityFramework;

using Newtonsoft.Json.Linq;

using EqDemo.Models;

namespace EqDemo.Services
{
    public class DefaultReportGenerator
    {
        private string _dataPath;
        private ApplicationDbContext _dbContext;

        private const string _modelId = "adhoc-reporting";

        public DefaultReportGenerator(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dataPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data"), "Seed");
        }

        public void Generate(IdentityUser user)
        {
            var reportJsons = GetReportJsons();
            foreach (var json in reportJsons)
            {
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

                _dbContext.Reports.Add(report);
            }

            _dbContext.SaveChanges();

        }

        private IEnumerable<string> GetReportJsons()
        {
            var files = Directory.GetFiles(_dataPath, "*.json");
            foreach (var file in files)
            {
                yield return File.ReadAllText(file);
            }
        }
    }
}