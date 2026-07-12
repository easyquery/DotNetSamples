using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Korzh.EasyQuery;
using Korzh.EasyQuery.Services;

using EqDemo.Models;

namespace EqDemo.Services
{
    /// <summary>
    /// A query store that saves the queries to the database
    /// and binds each of them to the current user's ID.
    /// </summary>
    public class DbQueryStore : IQueryStore
    {
        private IHttpContextAccessor _httpContextAccessor;
        private AppDbContext _dbContext;

        protected IServiceProvider Services;

        public DbQueryStore(IServiceProvider services)
        {
            Services = services;
            _httpContextAccessor = Services.GetRequiredService<IHttpContextAccessor>();
            _dbContext = Services.GetRequiredService<AppDbContext>();
        }

        private string GetUserId() {
            var user = _httpContextAccessor?.HttpContext?.User;
            if (user == null) {
                throw new NullReferenceException("Can't get HttpContextAccessor or the current user");
            }
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<bool> AddQueryAsync(Query query, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(query.Id)) {
                query.Id = Guid.NewGuid().ToString();
            }

            var userQuery = new UserQuery {
                Id = query.Id,
                Name = query.Name,
                Description = query.Description,
                ModelId = query.Model.Id,
                QueryJson = await query.SaveToJsonStringAsync(),
                OwnerId = GetUserId()
            };

            if (userQuery.OwnerId == null) {
                throw new ArgumentNullException(nameof(userQuery.OwnerId));
            }

            await _dbContext.UserQueries.AddAsync(userQuery, ct);
            await _dbContext.SaveChangesAsync(ct);
            return true;
        }

        public Task<IEnumerable<QueryListItem>> GetAllQueriesAsync(string modelId, CancellationToken ct = default)
        {
            return Task.FromResult(ApplyUserGuard(_dbContext.UserQueries)
                                   .Where(q => q.ModelId == modelId)
                                   .OrderBy(q => q.Name)
                                   .Select(q => new QueryListItem(q.Id, q.ModelId, q.Name, q.Description))
                                   .AsNoTracking()
                                   .AsEnumerable());
        }

        public async Task<bool> LoadQueryAsync(Query query, string queryId, CancellationToken ct = default)
        {
            var userQuery = await ApplyUserGuard(_dbContext.UserQueries).FirstOrDefaultAsync(q => q.Id == queryId, ct);
            if (userQuery != null) {
                await query.LoadFromJsonStringAsync(userQuery.QueryJson);
                query.Id = userQuery.Id;

                return true;
            }

            return false;
        }

        public async Task<bool> RemoveQueryAsync(string modelId, string queryId, CancellationToken ct = default)
        {
            var userQuery = await ApplyUserGuard(_dbContext.UserQueries).FirstOrDefaultAsync(q => q.Id == queryId, ct);
            if (userQuery != null) {
                _dbContext.Remove(userQuery);
                await _dbContext.SaveChangesAsync(ct);

                return true;
            }

            return false;
        }

        public async Task<bool> SaveQueryAsync(Query query, bool createIfNotExists = true, CancellationToken ct = default)
        {
            var userQuery = await ApplyUserGuard(_dbContext.UserQueries).FirstOrDefaultAsync(q => q.Id == query.Id, ct);
            if (userQuery != null) {
                userQuery.Name = query.Name;
                userQuery.Description = query.Description;
                userQuery.ModelId = query.Model.Id;
                userQuery.QueryJson = await query.SaveToJsonStringAsync();

                _dbContext.Update(userQuery);
                await _dbContext.SaveChangesAsync(ct);

                return true;
            }
            else if (createIfNotExists) {
                return await AddQueryAsync(query, ct);
            }

            return false;
        }

        private IQueryable<UserQuery> ApplyUserGuard(IQueryable<UserQuery> queries)
        {
            var userId = GetUserId();
            return queries.Where(q => q.OwnerId == userId);
        }
    }
}
