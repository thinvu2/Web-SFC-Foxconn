using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Services
{
    public interface IDbContext
    {
        /// <summary>
        /// Get oracle connection string
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        string GetConnectionString(string databaseName);
        //Connect to database
        OracleConnection Connect(string databaseName);
    }
}