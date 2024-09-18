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
using static SN_API.Models.LineElement;

namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class Config47Controller : ApiController
    {
        [System.Web.Http.Route("GetConfig47Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig47Content(Config47Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $" select A.MODEL_NAME,A.MODEL_SERIAL,A.VERSION_CODE,A.VERSION_DIFFERENCE, ROWIDTOCHAR(ROWID) ID from SFIS1.C_MODEL_DESC_T2 A WHERE ROWNUM < 20 ";
            }
            else
            {
                strGetData = $" select A.MODEL_NAME,A.MODEL_SERIAL,A.VERSION_CODE,A.VERSION_DIFFERENCE, ROWIDTOCHAR(ROWID) ID from SFIS1.C_MODEL_DESC_T2 A WHERE ROWNUM < 20 AND UPPER(A.MODEL_NAME) LIKE '%{model.MODEL_NAME.ToUpper()}%' ";
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
        [System.Web.Http.Route("InsertUpdateConfig47")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig47(Config47Element model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                //check exist
                string strCheckExist = $"  select MODEL_NAME from SFIS1.C_MODEL_DESC_T2 where MODEL_NAME = '{model.MODEL_NAME}' AND VERSION_CODE = '{model.VERSION_CODE}' ";
                string actionString = " ";
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'REPAIR_RULE_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    // not exist => insert
                    sb.Append(" INSERT INTO SFIS1.C_MODEL_DESC_T2 ");
                    sb.Append($" ( MODEL_NAME,MODEL_SERIAL,VERSION_CODE,VERSION_DIFFERENCE )  VALUES  ( '{model.MODEL_NAME}','{model.MODEL_SERIAL}','{model.VERSION_CODE}','{model.VERSION_DIFFERENCE}' )  ");
                    actionString = "INSERT";
                }
                else
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'REPAIR_RULE_EDIT' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" UPDATE SFIS1.C_MODEL_DESC_T2 ");
                    sb.Append(" SET ");
                    sb.Append($" MODEL_NAME = '{model.MODEL_NAME}', "); //MODEL_NAME
                    sb.Append($" MODEL_SERIAL = '{model.MODEL_SERIAL}', "); //MODEL_SERIAL                    
                    sb.Append($" VERSION_CODE = '{model.VERSION_CODE}', "); //VERSION_CODE
                    sb.Append($" VERSION_DIFFERENCE = '{model.VERSION_DIFFERENCE}' "); //VERSION_DIFFERENCE
                    sb.Append($" WHERE ROWID = '{model.ID}' "); //ID

                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config47 MODEL_NAME: {model.MODEL_NAME}; VERSION_CODE: {model.VERSION_CODE}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_DESC_T2' ");
                sbLog.Append(" ) ");
                string strInsertLog = sbLog.ToString();
                string strInserUpdate = sb.ToString();
                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config 6
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
        [System.Web.Http.Route("DeleteConfig47")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig47(Config47Element model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'REPAIR_RULE_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }
            string strDelete = $" delete SFIS1.C_MODEL_DESC_T2 where  ROWID = '{model.ID}' ";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'Config47 MODEL_NAME: {model.MODEL_NAME}; VERSION_CODE: {model.VERSION_CODE} IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_DESC_T2' ");
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
    }
}