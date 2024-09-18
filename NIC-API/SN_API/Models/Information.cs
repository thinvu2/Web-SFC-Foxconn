using Oracle.ManagedDataAccess.Client;
using SN_API.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class Information
    {
        public string SN { set; get; }
        public string STATION { set; get; }
        public string MODEL { set; get; }
        public string COUNTRY
        {
            set; get;
        }
       
        public Information()
        {

        }
        public Information(string sN, string sTATION, string mODEL, string cOUNTRY)
        {
            SN = sN;
            STATION = sTATION;
            MODEL = mODEL;
            COUNTRY = cOUNTRY;
        }
        public List<Information> GetInformData(string queryString, string databaseName)
        {
            OracleConnection conn = new DbContext().Connect(databaseName);
            List<Information> list = new List<Information>();
            using (var cmd = new OracleCommand(queryString, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Information info = new Information();
                        info.SN = reader["SERIAL_NUMBER"].ToString();
                        info.STATION = reader["GROUP_NAME"].ToString();
                        info.MODEL = reader["MODEL_NAME"].ToString();
                        info.COUNTRY = reader["MODEL_NAME"].ToString();
                        list.Add(info);
                    }
                }
            }
            conn.Close();
            return list;
        }
        public string GetDataFromProc(String SN, string databaseName)
        {
            OracleConnection conn = new DbContext().Connect(databaseName);
            if (conn.State.ToString() == "CLOSED")
            {
                conn.Open();
            }
            OracleParameter[] parameter = new OracleParameter[2] {
                new OracleParameter("DATA",OracleDbType.Varchar2,4000),
                new OracleParameter("RES",OracleDbType.Varchar2,4000)
            };
            parameter[0].Value = SN;
            parameter[0].Direction = ParameterDirection.Input;
            parameter[1].Direction = ParameterDirection.Output;
            string result = DBConnect.RunProcedure("LUAT_GET_SN_TO_API", conn, parameter);
            conn.Close();
            return result;
        }
    }
}