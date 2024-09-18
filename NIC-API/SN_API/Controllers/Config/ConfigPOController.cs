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
    public class ConfigPOController : ApiController
    {
        // GET: ConfigPO
        [System.Web.Http.Route("GetConfigPOContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfigPOContent(ConfigPOElement model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.PO_NO))
            {
                strGetData = $" select a.MODEL_NAME INVOICE,a.TYPE,a.PO_NO,a.TIME,a.rowid ID from SFIS1.C_PO_CONFIG_T a WHERE 1 = 1 AND ROWNUM < 20 ";
            }
            else
            {
                strGetData = $" select a.MODEL_NAME INVOICE,a.TYPE,a.PO_NO,a.TIME,a.rowid ID from SFIS1.C_PO_CONFIG_T a WHERE UPPER(A.PO_NO) LIKE '%{model.PO_NO.ToUpper()}%' OR UPPER(A.MODEL_NAME) LIKE '%{model.PO_NO.ToUpper()}%' ";
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

        [System.Web.Http.Route("ConfigPOModel")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ConfigPOModel(ConfigJigElement model)
        {
            string strGetData = "select DISTINCT MODEL_NAME from SFIS1.C_MODEL_DESC_T ";

            string strvalue = "select '10' as type from dual union select '20' as type from dual union select '30' as type from dual";

            string strdesc1 = "select DISTINCT ATTRIBUTE_DESC1 from SFIS1.C_MODEL_ATTR_CONFIG_T WHERE ATTRIBUTE_NAME = 'CHECK_JIG' ";

            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            DataTable dtvalue = DBConnect.GetData(strvalue, model.database_name);
            DataTable dtdesc = DBConnect.GetData(strdesc1, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, dtvalue = dtvalue, dtdesc = dtdesc });
        }

        [System.Web.Http.Route("DeletePO")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeletePO(ConfigPOElement model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'INVOICE_ITEM' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }

            string strDelete = $" delete from SFIS1.C_PO_CONFIG_T where ROWID = '{model.ID}'";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'INVOICE_ITEM INVOICE: {model.MODEL_NAME}; PO_NO: {model.PO_NO}; TYPE: {model.TYPE}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_PO_CONFIG_T' ");
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

        [System.Web.Http.Route("InsertUpdatePO")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdatePO(ConfigPOElement model)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbLog = new StringBuilder();
            string actionString = " ";
            string modify = " ";
            try
            {
                //check exist
                string strCheckexist = $"select a.* from sfis1.C_PO_CONFIG_T a where upper(a.MODEL_NAME) = '{model.MODEL_NAME.ToUpper()}'";
                if (DBConnect.GetData(strCheckexist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'INVOICE_ITEM' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //not exist => insert
                    actionString = "INSERT";
                    sb.Append("INSERT into");
                    sb.Append($" SFIS1.C_PO_CONFIG_T (MODEL_NAME,TYPE,PO_NO,TIME) VALUES(");
                    sb.Append($" '{model.MODEL_NAME}','{model.TYPE}','{model.PO_NO}',SYSDATE");
                    sb.Append($" ) ");
                }
                else
                {
                    //check privilege
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'INVOICE_ITEM' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    //check privilege
                    string checkexist = $"select * from sfis1.C_PO_CONFIG_T where ROWID='{model.ID}'";
                    if (DBConnect.GetData(checkexist, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "notexist" });
                    }

                    //existed => update
                    actionString = "UPDATE";
                    sb.Append("UPDATE");
                    sb.Append($" SFIS1.C_PO_CONFIG_T");
                    sb.Append($" SET ");
                    sb.Append($" TYPE = '{model.TYPE}', PO_NO = '{model.PO_NO}'");
                    sb.Append($" WHERE ROWID = '{model.ID}'");


                    modify = " UPDATE: ";
                    string query = $"select TYPE,PO_NO from sfis1.C_PO_CONFIG_T WHERE ROWID = '{model.ID}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);
                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if (row[0].ToString() != model.TYPE)
                        {
                            modify += $" TYPE: {row[0].ToString()} => {model.TYPE};";
                        }
                        if (row[1].ToString() != model.PO_NO)
                        {
                            modify += $" PO_NO: {row[1].ToString()} => {model.PO_NO};";
                        }
                    }
                }

                //insert log
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'INVOICE_ITEM: INVOICE: {model.MODEL_NAME};PO_NO: {model.PO_NO};{modify}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_PO_CONFIG_T' ");
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