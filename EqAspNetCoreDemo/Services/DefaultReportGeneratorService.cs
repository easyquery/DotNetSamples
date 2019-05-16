﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

using Newtonsoft.Json.Linq;

using EqAspNetCoreDemo.Models;

namespace EqAspNetCoreDemo.Services
{
    public class DefaultReportGeneratorService
    {
        private string _dataPath;
        private AppDbContext _dbContext;

        private const string _modelId = "adhoc-reporting";

        public DefaultReportGeneratorService(IHostingEnvironment env, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dataPath = Path.Combine(env.ContentRootPath, $"App_Data\\dm-{_modelId}\\queries");
        }

        public async Task GenerateAsync(IdentityUser user)
        {
            var reportJsons = GetReportJsons();
            foreach (var json in reportJsons) {
                var jobject = JObject.Parse(json);
                var report = new Report
                {
                    Id = Guid.NewGuid().ToString(),
                    OwnerId = user.Id,
                    Name = jobject["name"]?.ToString(),
                    Description = jobject["desc"]?.ToString(),
                    ModelId = _modelId,
                    QueryJson = json
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