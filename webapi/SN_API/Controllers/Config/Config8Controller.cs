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
    public class Config8Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig8Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig8Content(Config8Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.ERROR_CODE))
            {
                strGetData = $" select ERROR_CODE,ERROR_DESC,ERROR_DESC2,ROWIDTOCHAR(ROWID) ID from SFIS1.C_ERROR_CODE_T A WHERE ROWNUM < 20 ORDER BY ERROR_CODE ";
            }
            else
            {
                strGetData = $" select ERROR_CODE,ERROR_DESC,ERROR_DESC2,ROWIDTOCHAR(ROWID) ID from SFIS1.C_ERROR_CODE_T A WHERE UPPER(A.ERROR_CODE) LIKE '%{model.ERROR_CODE.ToUpper()}%' AND ROWNUM < 20 ORDER BY ERROR_CODE ";
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
        [System.Web.Http.Route("InsertUpdateConfig8")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig8(Config8Element model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                string modify = "";
                //check exist
                string strCheckExist = $"  select ERROR_CODE from SFIS1.C_ERROR_CODE_T where ERROR_CODE = '{model.ERROR_CODE}' ";
                string actionString = " ";
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'ERROR CODE_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    // not exist => insert
                    sb.Append(" INSERT INTO SFIS1.C_ERROR_CODE_T ");
                    sb.Append($" ( ERROR_CODE,ERROR_DEGREE,ERROR_TYPE,ERROR_DESC,ERROR_DESC2 )  VALUES  ( '{model.ERROR_CODE}','{model.ERROR_DEGREE}','{model.ERROR_TYPE}','{model.ERROR_DESC}','{model.ERROR_DESC2}' )  ");
                    actionString = "INSERT";
                }
                else
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'ERROR CODE_EDIT' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" UPDATE SFIS1.C_ERROR_CODE_T ");
                    sb.Append(" SET ");
                    sb.Append($" ERROR_CODE = '{model.ERROR_CODE}', "); //ERROR_CODE
                    sb.Append($" ERROR_DEGREE = '{model.ERROR_DEGREE}', "); //ERROR_DEGREE
                    sb.Append($" ERROR_TYPE = '{model.ERROR_TYPE}', "); //ERROR_TYPE
                    sb.Append($" ERROR_DESC = '{model.ERROR_DESC}', "); //ERROR_DESC
                    sb.Append($" ERROR_DESC2 = '{model.ERROR_DESC2}' "); //ERROR_DESC2
                    sb.Append($" WHERE ROWID = '{model.ID}' "); //ID

                    modify = " Change: ";
                    string query = $"  select ERROR_CODE,ERROR_DEGREE,ERROR_TYPE, ERROR_DESC,ERROR_DESC2 from  SFIS1.C_ERROR_CODE_T where ERROR_CODE = '{model.ERROR_CODE}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);

                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if (row[0].ToString() != model.ERROR_CODE)
                        {
                            modify += $" ERROR_CODE: {row[0].ToString()} => {model.ERROR_CODE};";
                        }
                        if (row[1].ToString() != model.ERROR_DEGREE)
                        {
                            modify += $" ERROR_DEGREE: {row[1].ToString()} => {model.ERROR_DEGREE};";
                        }
                        if (row[2].ToString() != model.ERROR_TYPE)
                        {
                            modify += $" ERROR_TYPE: {row[2].ToString()} => {model.ERROR_TYPE};";
                        }
                        if (row[3].ToString() != model.ERROR_DESC)
                        {
                            modify += $" ERROR_DESC: {row[3].ToString()} => {model.ERROR_DESC};";
                        }
                        if (row[4].ToString() != model.ERROR_DESC2)
                        {
                            modify += $" ERROR_DES2: {row[4].ToString()} => {model.ERROR_DESC2};";
                        }
                    }
                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config8 ERROR_CODE: {model.ERROR_CODE}; {modify}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_ERROR_CODE_T' ");
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
        [System.Web.Http.Route("DeleteConfig8")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig8(Config8Element model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'ERROR CODE_DELETE' AND EMP='{model.EMP}'";           
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }
            string strDelete = $" delete SFIS1.C_ERROR_CODE_T where  ROWID = '{model.ID}' ";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'Config8 ERROR_CODE: {model.ERROR_CODE};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_ERROR_CODE_T' ");
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