using System;
using System.Security.Principal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

using Microsoft.AspNet.Identity;

using Korzh.EasyQuery;
using Korzh.EasyQuery.Services;

using EqDemo.Models;
using System.Threading;

namespace EqDemo.Services
{
    public class ReportStore : IQueryStore
    {

        protected IPrincipal User;

        protected ApplicationDbContext DbContext;

        public ReportStore(ApplicationDbContext dbContext, IPrincipal user)
        {

            User = user;
            DbContext = dbContext;
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
                OwnerId = User?.Identity.GetUserId()
            };


            if (report.OwnerId == null) {
                throw new ArgumentNullException(nameof(report.OwnerId));
            }

            DbContext.Reports.Add(report);
            await DbContext.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IEnumerable<QueryListItem>> GetAllQueriesAsync(string modelId, CancellationToken ct = default)
        {
            var reports = await ApplyUserGuard(DbContext.Reports)
                            .Where(r => r.ModelId == modelId)
                            .ToListAsync(ct);

            return reports.Select(r => new QueryListItem(r.Id, r.ModelId, r.Name, r.Description)).ToList();
        }


        public async Task<bool> LoadQueryAsync(Query query, string queryId, CancellationToken ct = default)
        {
            var report = await ApplyUserGuard(DbContext.Reports).FirstOrDefaultAsync(r => r.Id == queryId, ct);
            if (report != null)
            {
                await query.LoadFromJsonStringAsync(report.QueryJson, ct);
                query.Id = report.Id;

                return true;
            }

            return false;
        }

        public async Task<bool> RemoveQueryAsync(string modelId, string queryId, CancellationToken ct = default)
        {
            var report = await ApplyUserGuard(DbContext.Reports).FirstOrDefaultAsync(r => r.Id == queryId, ct);
            if (report != null) {
                DbContext.Reports.Remove(report);
                await DbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> SaveQueryAsync(Query query, bool createIfNotExist = true, CancellationToken ct = default)
        {
            var report = await ApplyUserGuard(DbContext.Reports).FirstOrDefaultAsync(r => r.Id == query.Id, ct);
            if (report != null) {
                report.Name = query.Name;
                report.Description = query.Description;
                report.ModelId = query.Model.Id;
                report.QueryJson = await query.SaveToJsonStringAsync();

                await DbContext.SaveChangesAsync();

                return true;
            }
            else if (createIfNotExist)
            {
                return await AddQueryAsync(query, ct);
            }

            return false;
        }

        private IQueryable<Report> ApplyUserGuard(IQueryable<Report> filter)
        {
            var userId = User?.Identity.GetUserId();
            return filter.Where(r => r.OwnerId == userId);
        }
    }
}