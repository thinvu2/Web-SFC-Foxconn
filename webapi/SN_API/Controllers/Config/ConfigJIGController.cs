using SN_API.Models;
using SN_API.Models.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;


namespace SN_API.Controllers.Config
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConfigJIGController : ApiController
    {
        [System.Web.Http.Route("GetConfigJIGContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfigJIGContent(ConfigJigElement model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.TYPE_VALUE))
            {
                strGetData = $" select A.TYPE_VALUE ,A.ATTRIBUTE_VALUE,A.ATTRIBUTE_DESC1,A.EMP_NO,A.INPUT_TIME,ROWIDTOCHAR(ROWID) ID from SFIS1.C_MODEL_ATTR_CONFIG_T A WHERE ATTRIBUTE_NAME = 'CHECK_JIG' AND ROWNUM < 20 ";
            }
            else
            {
                strGetData = $" select A.TYPE_VALUE ,A.ATTRIBUTE_VALUE,A.ATTRIBUTE_DESC1,A.EMP_NO,A.INPUT_TIME,ROWIDTOCHAR(ROWID) ID from SFIS1.C_MODEL_ATTR_CONFIG_T A WHERE ATTRIBUTE_NAME = 'CHECK_JIG' AND UPPER(A.TYPE_VALUE) LIKE '%{model.TYPE_VALUE.ToUpper()}%' ";
            }
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }

        [System.Web.Http.Route("ConfigJIGModel")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ConfigJIGModel(ConfigJigElement model)
        {
            string strGetData = "select DISTINCT MODEL_NAME from SFIS1.C_MODEL_DESC_T ";

            string strvalue = "select DISTINCT ATTRIBUTE_VALUE from SFIS1.C_MODEL_ATTR_CONFIG_T WHERE ATTRIBUTE_NAME = 'CHECK_JIG' ";

            string strdesc1 = "select DISTINCT ATTRIBUTE_DESC1 from SFIS1.C_MODEL_ATTR_CONFIG_T WHERE ATTRIBUTE_NAME = 'CHECK_JIG' ";

            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            DataTable dtvalue = DBConnect.GetData(strvalue, model.database_name);
            DataTable dtdesc = DBConnect.GetData(strdesc1, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, dtvalue= dtvalue, dtdesc= dtdesc });
        }

        [System.Web.Http.Route("DeleteJIG")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteJIG(ConfigJigElement model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CONFIG_JIG' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }

            string strDelete = $" delete from SFIS1.C_MODEL_ATTR_CONFIG_T where ROWID = '{model.ID}'";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'JIG TYPE_VALUE: {model.TYPE_VALUE}; ATTRIBUTE_VALUE: {model.ATTRIBUTE_VALUE}; ATTRIBUTE_DESC1: {model.ATTRIBUTE_DESC1}; VERSION: {model.VERSION}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_ATTR_CONFIG_T' ");
                sbLog.Append(" ) ");

                string strInsertLog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }

        [System.Web.Http.Route("InsertUpdateJIG")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateJIG(ConfigJigElement model)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbLog = new StringBuilder();
            string actionString = " ";
            string modify = " ";
            try
            {
                //check exist
                string strCheckexist = $"select a.* from sfis1.C_MODEL_ATTR_CONFIG_T a where upper(a.TYPE_VALUE) = '{model.TYPE_VALUE.ToUpper()}'" +
                    $" AND ATTRIBUTE_VALUE = '{model.ATTRIBUTE_VALUE.ToUpper()}' AND ATTRIBUTE_NAME = 'CHECK_JIG' AND ATTRIBUTE_DESC1 = '{model.ATTRIBUTE_DESC1.ToUpper()}' ";
                if (DBConnect.GetData(strCheckexist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CONFIG_JIG' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //not exist => insert
                    actionString = "INSERT";
                    sb.Append("INSERT into");
                    sb.Append($" SFIS1.C_MODEL_ATTR_CONFIG_T VALUES(");
                    sb.Append($" 'MODEL_NAME','{model.TYPE_VALUE}','','CHECK_JIG','{model.ATTRIBUTE_VALUE}','{model.ATTRIBUTE_DESC1}','','{model.EMP}',");
                    sb.Append($" SYSDATE,'','',''");
                    sb.Append($" ) ");
                }
                else
                {
                    //check privilege
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CONFIG_JIG' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    //check privilege
                    string checkexist = $"select * from sfis1.C_MODEL_ATTR_CONFIG_T where ROWID='{model.ID}'";
                    if (DBConnect.GetData(checkexist, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "notexist" });
                    }

                    //existed => update
                    actionString = "UPDATE";
                    sb.Append("UPDATE");
                    sb.Append($" SFIS1.C_MODEL_ATTR_CONFIG_T");
                    sb.Append($" SET ");
                    sb.Append($" TYPE_VALUE = '{model.TYPE_VALUE}', ATTRIBUTE_DESC1 = '{model.ATTRIBUTE_DESC1}', ATTRIBUTE_VALUE = '{model.ATTRIBUTE_VALUE}'");
                    sb.Append($" WHERE ROWID = '{model.ID}'");


                    modify = " UPDATE: ";
                    string query = $"select TYPE_VALUE,ATTRIBUTE_DESC1,ATTRIBUTE_VALUE from sfis1.C_MODEL_ATTR_CONFIG_T WHERE ROWID = '{model.ID}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);
                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if (row[0].ToString() != model.TYPE_VALUE)
                        {
                            modify += $" TYPE_VALUE: {row[0].ToString()} => {model.TYPE_VALUE};";
                        }
                        if (row[1].ToString() != model.ATTRIBUTE_DESC1)
                        {
                            modify += $" ATTRIBUTE_DESC1: {row[1].ToString()} => {model.ATTRIBUTE_DESC1};";
                        }
                        if (row[2].ToString() != model.ATTRIBUTE_VALUE)
                        {
                            modify += $" ATTRIBUTE_VALUE: {row[2].ToString()} => {model.ATTRIBUTE_VALUE};";
                        }
                    }
                }

                //insert log
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'CHECK_JIG MODEL_NAME: {model.TYPE_VALUE};{modify}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_ATTR_CONFIG_T' ");
                sbLog.Append(" ) ");

                string strInsertUpdate = sb.ToString();
                string strInsertLog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strInsertUpdate, model.database_name);
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
    }
}