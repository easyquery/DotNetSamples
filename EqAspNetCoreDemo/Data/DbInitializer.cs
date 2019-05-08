using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using System.Data.SqlClient;

namespace EqAspNetCoreDemo.Data
{
    public class DbInitializer
    {
        private string _connectionString;
        private SqlConnection _connection;
        private string _scriptFilePath;

        public DbInitializer(IConfiguration config, string dbName, string scriptFilePath)
        {
            _connectionString = config.GetConnectionString(dbName);
            _connection = new SqlConnection(_connectionString);
            _scriptFilePath = scriptFilePath;
        }

        public void AddTestData()
        {
            try
            {
                _connection.Open();

                if (IsEmptyDb()) {
                    FillDb();
                }

                _connection.Close();
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
          
        }

        private bool IsEmptyDb()
        {
            string script = "SELECT * FROM dbo.Employees";

            var fillDbCommand = _connection.CreateCommand();

            fillDbCommand.CommandText = script;

            var rows = fillDbCommand.ExecuteNonQuery();

            return rows < 1;
        }

        private void FillDb()
        {
            string script = System.IO.File.ReadAllText(_scriptFilePath);

            var fillDbCommand = _connection.CreateCommand();

            fillDbCommand.CommandText = script;

            fillDbCommand.ExecuteNonQuery();
        }
    }
}
