using System;
using System.Collections.Generic;
using System.Configuration;


namespace EqDemo
{
    public static class ConfigurationManagerWrapper
    {
        public static string GetConnectionString(string connectionStringId)
        {
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["DefaultConnection"];
            if (mySetting == null || string.IsNullOrEmpty(mySetting.ConnectionString))
                throw new Exception("Fatal error: missing connecting string in web.config file");

            return mySetting.ConnectionString;
        }
    }
}