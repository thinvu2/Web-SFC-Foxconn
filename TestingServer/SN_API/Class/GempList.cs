using Newtonsoft.Json;
using SN_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SN_API.Class
{
    public class GempList
    {
        public DataTable EmpList(string JsonData)
        {
            string dbtype = string.Empty;
            string sql = string.Empty;
            string Qemp = string.Empty;
            string db = string.Empty;
            try
            {
                AmsDataIn diin = new AmsDataIn();
                AmsDataOut diout = new AmsDataOut();

                if (String.IsNullOrEmpty(JsonData)) { return null; }

                diin = JsonConvert.DeserializeObject<AmsDataIn>(JsonData);

                dbtype = diin.ApplicationID;

                if (String.IsNullOrEmpty(dbtype))/*rule:if application id is empty,return [null]*/
                {
                    return null;
                }

                foreach (Models.item sitem in diin.EmpList)
                {
                    if (sitem.EMPNO != "")
                    {
                        if (Qemp == "")
                        {
                            Qemp = "('" + sitem.EMPNO + "',";
                        }
                        else
                        {
                            Qemp = Qemp + "'" + sitem.EMPNO + "'" + ",";
                        }
                    }

                    if (sitem.EMPNO == "ALL")
                    {
                        Qemp = string.Empty;
                        goto GempList;
                    }
                }

                Qemp = Qemp.TrimEnd(',') + ")";

                GempList:

                sql = @"select a.empno userid,a.empno empno,a.name name,a.groupname,a.groupremark,
                                a.effecttime  from sfis1.C_AMS_USER_INFOR_T a where a.APPLICATIONID = '" + dbtype + "' and a.status is null ";
                if (!String.IsNullOrEmpty(Qemp))
                {
                    sql = sql + " and a.empno in {0}";
                    sql = String.Format(sql, Qemp);
                }

                if (String.IsNullOrEmpty(sql))
                {
                    return null;
                }

                try
                {
                    return DBConnect.GetData(sql, "CPEI");
                }
                catch
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string EmpType(string JsonData)
        {
            try
            {
                AmsDataIn diin = new AmsDataIn();

                if (String.IsNullOrEmpty(JsonData)) { return null; }

                diin = JsonConvert.DeserializeObject<AmsDataIn>(JsonData);

                return diin.ApplicationID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<AmsDataOut> DataTableToJson(DataTable dt, string db)/*incorrect format*/
        {
            List<SetEmpInfo> user = new List<SetEmpInfo>();
            List<SetEmpInfo> Ap = new List<SetEmpInfo>();
            List<AmsDataOut> amsOut = new List<AmsDataOut>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    user.Add(new SetEmpInfo
                    {
                        USERID = dt.Rows[i]["USERID"].ToString(),
                        EMPNO = dt.Rows[i]["EMPNO"].ToString(),
                        NAME = dt.Rows[i]["NAME"].ToString(),
                        GroupName = dt.Rows[i]["GroupName"].ToString(),
                        GroupRemark = dt.Rows[i]["GroupRemark"].ToString(),
                        EffectTime = dt.Rows[i]["EffectTime"].ToString()
                    });
                }
            }

            amsOut.Add(new AmsDataOut
            {
                ApplicationID = db,
                EmpList = user
            });

            return amsOut;
        }

        public List<SetEmpInfo> DT2Json(DataTable dt)
        {
            List<SetEmpInfo> user = new List<SetEmpInfo>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    user.Add(new SetEmpInfo
                    {
                        USERID = dt.Rows[i]["USERID"].ToString(),
                        EMPNO = dt.Rows[i]["EMPNO"].ToString(),
                        NAME = dt.Rows[i]["NAME"].ToString(),
                        GroupName = dt.Rows[i]["GroupName"].ToString(),
                        GroupRemark = dt.Rows[i]["GroupRemark"].ToString(),
                        EffectTime = dt.Rows[i]["EffectTime"].ToString()
                    });
                }
            }
            return user;
        }
    }
    public class ReturnJsonResult
    {
        public static JsonResult<T> GetJsonResult<T>(string type, T dataList)
        {
            JsonResult<T> jsonResult = new JsonResult<T>();
            List<SetEmpInfo> user = new List<SetEmpInfo>();

            jsonResult.ApplicationID = type;
            jsonResult.EmpList = dataList;
            return jsonResult;
        }
    }

    public class JsonResult<T>
    {
        public string ApplicationID { get; set; }
        public T EmpList { get; set; }
    }
}