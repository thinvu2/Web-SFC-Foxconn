using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Services
{
    public class DbContext : IDbContext
    {
        public OracleConnection Connect(string databaseName)
        {
            OracleConnection conn = new OracleConnection(GetConnectionString(databaseName));
            try
            {
                conn.Open();
            }
            catch
            {
                return null;
            }
            return conn;
        }

        public string GetConnectionString(string databaseName)
        {
            return new GetString().Get()[databaseName];
        }
    }
}