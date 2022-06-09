using Korzh.EasyQuery.Db;
using Korzh.EasyQuery.Services;
using System.Data.Common;

namespace EqDemo
{ 
    public class CustomEasyQueryManagerSql : EasyQueryManagerSql
    {
        protected override DbCommand PrepareDbCommand(ISqlStatement statement, bool useCountCommand = false)
        {
            var sql = statement.SQL;

            //modify statement.SQL the way you need

            return base.PrepareDbCommand(statement, useCountCommand);
        }
    }
}
