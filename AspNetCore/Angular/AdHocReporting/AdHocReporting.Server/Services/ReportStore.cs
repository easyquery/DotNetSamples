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

using EqDemo.Models;
using System.Threading;
using EqDemo.Services;
using Microsoft.AspNetCore.Identity;

namespace EqDemo
{
    public class ReportStore : IQueryStore
    {
        private IHttpContextAccessor _httpContextAccessor;
        private AppDbContext _dbContext;
        private IConfiguration _config;

        protected IServiceProvider Services;

        public ReportStore(IServiceProvider services, IConfiguration config)
        {
            Services = services;
            _httpContextAccessor = Services.GetRequiredService<IHttpContextAccessor>();
            _dbContext = Services.GetRequiredService<AppDbContext>();
            _config = config;
        }

        private string GetUserId() {
            var userClaim = _httpContextAccessor?.HttpContext?.User;

            if (userClaim != null) {
                var userId = userClaim.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
                if (userId != null) { 
                    return userId;
                }
            }

            if (_config.GetValue<bool>("needAuth")) {
                throw new Exception("User is not authenticated");
            }

            var userManager = Services.GetRequiredService<UserManager<IdentityUser>>();
            var user = userManager.FindByEmailAsync(DbInitializeExtensions.defaultUserEmail).Result;
            if (user != null) {
                return user.Id;
            }
            else {
                throw new Exception("Can't get default user");
            }
        }

        public async Task<bool> AddQueryAsync(Query query, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(query.Id)) {
                query.Id = Guid.NewGuid().ToString();
            }

            var report = new Report {
                Id = query.Id,
                Name = query.Name,
                Description = query.Description,
                ModelId = query.Model.Id,
                QueryJson = await query.SaveToJsonStringAsync(),
                OwnerId = GetUserId()
            };


            if (report.OwnerId == null) {
                throw new ArgumentNullException(nameof(report.OwnerId));
            }

            await _dbContext.Reports.AddAsync(report, ct);
            await _dbContext.SaveChangesAsync(ct);
            return true;

        }

        public Task<IEnumerable<QueryListItem>> GetAllQueriesAsync(string modelId, CancellationToken ct = default)
        {
            return Task.FromResult(ApplyUserGuard(_dbContext.Reports)
                                   .Where(r => r.ModelId == modelId)
                                   .OrderBy(r => r.Name)
                                   .Select(r => new QueryListItem(r.Id, r.ModelId, r.Name, r.Description))
                                   .AsNoTracking()
                                   .AsEnumerable());
        }


        public async Task<bool> LoadQueryAsync(Query query, string queryId, CancellationToken ct = default)
        {
            var report = await ApplyUserGuard(_dbContext.Reports).FirstOrDefaultAsync(r => r.Id == queryId, ct);
            if (report != null)
            {
                await query.LoadFromJsonStringAsync(report.QueryJson);
                query.Id = report.Id;

                return true;
            }

            return false;
        }

        public async Task<bool> RemoveQueryAsync(string modelId, string queryId, CancellationToken ct = default)
        {
            var report = await ApplyUserGuard(_dbContext.Reports).FirstOrDefaultAsync(r => r.Id == queryId, ct);
            if (report != null)
            {
                _dbContext.Remove(report);
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> SaveQueryAsync(Query query, bool createIfNotExists = true, CancellationToken ct = default)
        {
            var report = await ApplyUserGuard(_dbContext.Reports).FirstOrDefaultAsync(r => r.Id == query.Id, ct);
            if (report != null) {
                report.Name = query.Name;
                report.Description = query.Description;
                report.ModelId = query.Model.Id;
                report.QueryJson = await query.SaveToJsonStringAsync();

                _dbContext.Update(report);
                await _dbContext.SaveChangesAsync(ct);

                return true;
            }
            else if (createIfNotExists) {
                return await AddQueryAsync(query, ct);
            }

            return false;
        }

        private IQueryable<Report> ApplyUserGuard(IQueryable<Report> filter)
        {
            var userId = GetUserId();
            return filter.Where(r => r.OwnerId == userId);
        }
    }
}
