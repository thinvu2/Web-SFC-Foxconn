using Oracle.ManagedDataAccess.Client;
using SN_API.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class DBConnect
    {
        public static string GetConnectionString(string dbName)
        {
            var connectionStrings = new GetString().Get();
            if(connectionStrings.ContainsKey(dbName))
            {
                return connectionStrings[dbName];
            }
            else
            {
                throw new Exception("connection string not found for database: " + dbName);
            }
        }
        public static void ExecuteNoneQuery(string queryString, string dbName)
        {
            var conn = new DbContext().Connect(dbName);           
            using (var cmd = new OracleCommand(queryString, conn))
            {
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
        public static DataTable GetData(string queryString, string dbName)
        {
            var conn = new DbContext().Connect(dbName);
            DataTable dt = new DataTable();         
            using (var cmd = new OracleCommand(queryString, conn))
            {
                try
                {
                    dt.Load(cmd.ExecuteReader());
                }
                catch (Exception ex)
                {
                    string ass = ex.Message;
                }
            }
            conn.Close();
            return dt;
        }
        public static void BuildCommand(OracleCommand comm, params OracleParameter[] parameter)
        {
            if (parameter != null && parameter.Length > 0)
            {
                foreach (OracleParameter p in parameter)
                {
                    comm.Parameters.Add(p);
                }
            }
        }
        public static string RunProcedure(string proName, OracleConnection conn, params OracleParameter[] parameter)
        {
            string result = null;
            OracleCommand cmd = new OracleCommand(proName, conn);
            BuildCommand(cmd, parameter);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            object obj = cmd.Parameters[parameter[parameter.Length - 1].ParameterName].Value;
            if (!(Object.Equals(obj, null)) && !(Object.Equals(obj, System.DBNull.Value)))
            {
                result = obj.ToString();
            }
            cmd.Dispose();

            return cmd.Parameters[parameter[parameter.Length - 1].ParameterName].Value.ToString();
        }
    }
}