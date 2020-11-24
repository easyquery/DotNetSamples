using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korzh.EasyQuery;
using Korzh.EasyQuery.Db;
using Korzh.EasyQuery.Services;
using Newtonsoft.Json.Linq;

namespace EqAspNetCoreDemo.Services
{
    public class CustomEasyQueryManagerSql : EasyQueryManagerSql
    {

        public CustomEasyQueryManagerSql(IServiceProvider services, EasyQueryOptions options) : base(services, options) {
            //ModelLoader = new FileModelLoader("AppData");
        }


        protected override IEqResultSet ExecuteQueryCore(JObject options = null)
        {
            var filter = options["filter"]?.ToString();
            var stringColumns = Query.Columns.Where(c => c.SystemType == typeof(string));

            if (!string.IsNullOrEmpty(filter) && stringColumns.Any()) {

                var conditions = Query.Root.Conditions.ToList();
                if (conditions.Any()) {
                    Query.Root.Conditions.Clear();

                    var left = Query.Root.AddConditionGroup(Query.Root.Linking);
                    foreach (var condition in conditions)
                        left.Conditions.Add(condition);

                    var right = Query.Root.AddConditionGroup(Condition.LinkType.Any);
                    AddFullTextSearch(right, stringColumns, filter);
                }
                else {
                    AddFullTextSearch(Query.Root, stringColumns, filter);
                }
               

                Query.Root.Linking = Condition.LinkType.All;

                var usedTables = (Query as DbQuery).GetUsedTables();
                var mainSelectedQueryTable = usedTables[0].GetTableName();
            }

            return base.ExecuteQueryCore(options);
        }

        private void AddFullTextSearch(Condition root, IEnumerable<Column> columns, string text)
        {
            foreach (var column in columns) {
                root.AddSimpleCondition(column.Expr.Value, "Contains", text);
            }
        }

        protected override void TuneBuilder(IQueryBuilder builder)
        {
            base.TuneBuilder(builder);
        }

    }
}
 