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
    public class Config9Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig9Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig9Content(Config9Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.REASON_CODE))
            {
                strGetData = $" select A.REASON_CODE,A.REASON_TYPE,A.REASON_DESC,A.REASON_DESC2,ROWIDTOCHAR(ROWID) ID from SFIS1.C_REASON_CODE_T A WHERE ROWNUM < 20 ORDER BY REASON_CODE ";
            }
            else
            {
                strGetData = $" select A.REASON_CODE,A.REASON_TYPE,A.REASON_DESC,A.REASON_DESC2,ROWIDTOCHAR(ROWID) ID from SFIS1.C_REASON_CODE_T A WHERE UPPER(A.REASON_CODE) LIKE '%{model.REASON_CODE.ToUpper()}%' AND ROWNUM < 20 ORDER BY REASON_CODE ";
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
        [System.Web.Http.Route("InsertUpdateConfig9")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig9(Config9Element model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                string modify = "";
                //check exist
                string strCheckExist = $"  select REASON_CODE from SFIS1.C_REASON_CODE_T where REASON_CODE = '{model.REASON_CODE}' ";
                string actionString = " ";
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'REASON CODE_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    // not exist => insert
                    sb.Append(" INSERT INTO SFIS1.C_REASON_CODE_T ");
                    sb.Append($" ( REASON_CODE,REASON_TYPE,REASON_DESC,REASON_DESC2,DUTY_STATION,STATISTIC_FLAG )  VALUES  ( '{model.REASON_CODE}','{model.REASON_TYPE}','{model.REASON_DESC}','{model.REASON_DESC2}','{model.DUTY_STATION}','{model.STATISTIC_FLAG}' )  ");
                    actionString = "INSERT";
                }
                else
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'REASON CODE_EDIT' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" UPDATE SFIS1.C_REASON_CODE_T ");
                    sb.Append(" SET ");
                    sb.Append($" REASON_CODE = '{model.REASON_CODE}', "); //REASON_CODE
                    sb.Append($" REASON_TYPE = '{model.REASON_TYPE}', "); //REASON_TYPE
                    sb.Append($" REASON_DESC = '{model.REASON_DESC}', "); //REASON_DESC
                    sb.Append($" REASON_DESC2 = '{model.REASON_DESC2}', "); //REASON_DESC2
                    sb.Append($" DUTY_STATION = '{model.DUTY_STATION}', "); //DUTY_STATION
                    sb.Append($" STATISTIC_FLAG = '{model.STATISTIC_FLAG}' "); //STATISTIC_FLAG
                    sb.Append($" WHERE ROWID = '{model.ID}' "); //ID

                    modify = " Change: ";
                    string query = $"   select REASON_CODE,REASON_TYPE,REASON_DESC,REASON_DESC2,DUTY_STATION,STATISTIC_FLAG from SFIS1.C_REASON_CODE_T  WHERE ROWID = '{model.ID}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);

                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if (row[0].ToString() != model.REASON_CODE)
                        {
                            modify += $" REASON_CODE: {row[0].ToString()} => {model.REASON_CODE};";
                        }
                        if (row[1].ToString() != model.REASON_TYPE)
                        {
                            modify += $" REASON_TYPE: {row[1].ToString()} => {model.REASON_TYPE};";
                        }
                        if (row[2].ToString() != model.REASON_DESC)
                        {
                            modify += $" REASON_DESC: {row[2].ToString()} => {model.REASON_DESC};";
                        }
                        if (row[3].ToString() != model.REASON_DESC2)
                        {
                            modify += $" REASON_DESC2: {row[3].ToString()} => {model.REASON_DESC2};";
                        }
                        if (row[4].ToString() != model.DUTY_STATION)
                        {
                            modify += $" DUTY_STATION: {row[4].ToString()} => {model.DUTY_STATION};";
                        }
                        if (row[5].ToString() != model.STATISTIC_FLAG)
                        {
                            modify += $" STATISTIC_FLAG: {row[5].ToString()} => {model.STATISTIC_FLAG};";
                        }
                    }
                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config9 REASON_CODE: {model.REASON_CODE};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_REASON_CODE_T' ");
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
        [System.Web.Http.Route("DeleteConfig9")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig9(Config9Element model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'REASON CODE_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }
            string strDelete = $" delete SFIS1.C_REASON_CODE_T where  ROWID = '{model.ID}' ";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'Config9 REASON_CODE: {model.REASON_CODE};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_REASON_CODE_T' ");
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