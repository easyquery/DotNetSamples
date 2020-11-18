using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Korzh.EasyQuery.Services;

namespace EqAspNetCoreDemo.Services
{
    public class CustomEasyQueryManagerWithSavingSql: EasyQueryManagerSql
    {
        public CustomEasyQueryManagerWithSavingSql(IServiceProvider services, EasyQueryOptions options)
           : base(services, options)
        {
           
        }

        public override Task<bool> SaveQueryToStoreAsync(bool createIfNotExist = true)
        {
            var result = BuildQuery();
            Query.ExtraData.Sql = result.Statement;
            return base.SaveQueryToStoreAsync(createIfNotExist);
        }
    }
}
