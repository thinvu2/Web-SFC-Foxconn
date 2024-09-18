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



namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class Config52Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig52Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig52Content(Config52Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $" select A.model_name,A.group_name,A.port_type,A.mo_type,A.status,A.edit_emp,A.verify_by,A.edit_time, ROWIDTOCHAR(ROWID) ID from SFIS1.C_SMO_TYPE_T A WHERE ROWNUM < 20 ";
            }
            else
            {
                strGetData = $" select A.model_name,A.group_name,A.port_type,A.mo_type,A.status,A.edit_emp,A.verify_by,A.edit_time, ROWIDTOCHAR(ROWID) ID from SFIS1.C_SMO_TYPE_T A WHERE UPPER(A.MODEL_NAME) LIKE '%{model.MODEL_NAME.ToUpper()}%'";
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

        [System.Web.Http.Route("DeleteConfig52")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig52(Config52Element model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'SMO TYPE_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }

            string strDelete = $" delete SFIS1.C_SMO_TYPE_T where  ROWID = '{model.ID}' ";

            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($" 'Config52 MODEL_NAME: {model.MODEL_NAME};GROUP_NAME: {model.GROUP_NAME};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_SMO_TYPE_T' ");
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

        [System.Web.Http.Route("InsertUpdateConfig52")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig52(Config52Element model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                //check exist
                string strCheckExist = $" select MODEL_NAME from SFIS1.C_SMO_TYPE_T where MODEL_NAME = '{model.MODEL_NAME}' ";
                string actionString = " ";
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'SMO TYPE_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    // not exist => insert
                    sb.Append(" INSERT INTO SFIS1.C_SMO_TYPE_T ");
                    sb.Append($" ( MODEL_NAME,GROUP_NAME,STATION_TYPE,PORT_TYPE,MO_TYPE,STATUS,EDIT_EMP,VERIFY_BY,EDIT_TIME )  VALUES ");
                    sb.Append($" ('{model.MODEL_NAME}','{model.GROUP_NAME}','{model.STATION_TYPE}','{model.PORT_TYPE}','{model.MO_TYPE}','{model.STATUS}','{model.EDIT_EMP}','{model.VERIFY_BY}',sysdate)");
                    actionString = "INSERT";
                }
                else
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'SMO TYPE_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" UPDATE SFIS1.C_SMO_TYPE_T ");
                    sb.Append(" SET ");
                    sb.Append($" MODEL_NAME = '{model.MODEL_NAME}', "); //MODEL_NAME
                    sb.Append($" GROUP_NAME = '{model.GROUP_NAME}', "); //GROUP_NAME
                    sb.Append($" STATION_TYPE = '{model.STATION_TYPE}', "); //STATION_TYPE
                    sb.Append($" PORT_TYPE = '{model.PORT_TYPE}', "); //PORT_TYPE
                    sb.Append($" MO_TYPE = '{model.MO_TYPE}', "); //MO_TYPE
                    sb.Append($" STATUS = '{model.STATUS}', "); //STATUS
                    sb.Append($" EDIT_EMP = '{model.EDIT_EMP}', "); //EDIT_EMP
                    sb.Append($" EDIT_TIME = sysdate"); //EDIT_TIME
                    sb.Append($" WHERE ROWID = '{model.ID}' "); // ID
                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config52 MODEL_NAME: {model.MODEL_NAME};GROUP_NAME: {model.GROUP_NAME};STATION_TYPE: {model.STATION_TYPE};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_SMO_TYPE_T' ");
                sbLog.Append(" ) ");
                string strInsertLog = sbLog.ToString();
                string strInserUpdate = sb.ToString();
                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config 52
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
    }
}