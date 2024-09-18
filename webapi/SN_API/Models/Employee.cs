using Oracle.ManagedDataAccess.Client;
using SN_API.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class Employee
    {
        public string USERID { set; get; }
        public string EMPNO { set; get; }
        public string NAME { set; get; }
        public string GroupName { set; get; }
        public string GroupRemark { set; get; }
        public string EffectTime { set; get; }

        public Employee()
        {

        }

        public Employee(string uSERID, string eMPNO, string nAME, string groupName, string groupRemark, string effectTime)
        {
            USERID = uSERID;
            EMPNO = eMPNO;
            NAME = nAME;
            GroupName = groupName;
            GroupRemark = groupRemark;
            EffectTime = effectTime;
        }

        public static OracleConnection conn = new OracleConnection();
        public static DataTable GetData(string queryString, string dbName)
        {
            conn = new DbContext().Connect(dbName);
            DataTable dt = new DataTable();            
            using (var cmd = new OracleCommand(queryString, conn))
            {
                try
                {
                    dt.Load(cmd.ExecuteReader());
                }
                catch { }
            }
            conn.Close();
            return dt;
        }
    }
}