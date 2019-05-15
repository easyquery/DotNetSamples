using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Korzh.EasyQuery;
using Korzh.EasyQuery.Services;

using EqAspNetCoreDemo.Models;

namespace EqAspNetCoreDemo.Services
{
    public class ReportStore : IQueryStore
    {

        protected IServiceProvider Services;
        protected ClaimsPrincipal User;

        private AppDbContext _dbContext;

        public ReportStore(IServiceProvider services)
        {
            Services = services;
            var httpContextAccessor = Services.GetRequiredService<IHttpContextAccessor>();
            User = httpContextAccessor?.HttpContext?.User;
            if (User == null)
            {
                throw new NullReferenceException("Can't get HttpContextAccessor or the current user");
            }
            _dbContext = Services.GetRequiredService<AppDbContext>();
        }

        public async Task<bool> AddQueryAsync(Query query)
        {
            if (string.IsNullOrEmpty(query.ID))
            {
                query.ID = Guid.NewGuid().ToString();
            }

            var report = new Report
            {
                Id = query.ID,
                Name = query.Name,
                Description = query.Description,
                ModelId = query.Model.ID,
                QueryJson = await query.SaveToJsonStringAsync(),
                OwnerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            };


            if (report.OwnerId == null)
            {
                throw new ArgumentNullException(nameof(report.OwnerId));
            }

            await _dbContext.Reports.AddAsync(report);
            await _dbContext.SaveChangesAsync();
            return true;

        }

        public Task<IEnumerable<QueryListItem>> GetAllQueriesAsync(string modelId)
            => Task.FromResult(ApplyUserGuard(_dbContext.Reports)
                               .Where(r => r.ModelId == modelId)
                               .Select(r => new QueryListItem(r.Id, r.Name, r.Description))
                               .AsEnumerable());


        public async Task<bool> LoadQueryAsync(Query query, string queryId)
        {
            var report = await ApplyUserGuard(_dbContext.Reports).FirstOrDefaultAsync(r => r.Id == queryId);
            if (report != null)
            {
                await query.LoadFromJsonStringAsync(report.QueryJson);
                query.ID = report.Id;

                return true;
            }

            return false;
        }

        public async Task<bool> RemoveQueryAsync(string modelId, string queryId)
        {
            var report = await ApplyUserGuard(_dbContext.Reports).FirstOrDefaultAsync(r => r.Id == queryId);
            if (report != null)
            {
                _dbContext.Remove(report);
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> SaveQueryAsync(Query query)
        {
            var report = await ApplyUserGuard(_dbContext.Reports).FirstOrDefaultAsync(r => r.Id == query.ID);
            if (report != null)
            {
                report.Name = query.Name;
                report.Description = query.Description;
                report.ModelId = query.Model.ID;
                report.QueryJson = await query.SaveToJsonStringAsync();

                _dbContext.Update(report);
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        private IQueryable<Report> ApplyUserGuard(IQueryable<Report> filter)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return filter.Where(r => r.OwnerId == userId);
        }

    }
}
