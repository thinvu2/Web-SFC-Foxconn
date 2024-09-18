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
    public class ConfigWeightSetupController : ApiController
    {
        //CONFIG FIX_WEIGHT
        #region 
        [System.Web.Http.Route("GetConfigWeightSetupContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfigWeightSetupContent(ConfigWeightSetupElement model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $" select MODEL_NAME,STANDARD_WEIGHT, UP_WEIGHT,DOWN_WEIGHT,EMP,SKIP_DATA,CTN_GROSS FIX_WEIGHT,DOWNLOAD_TIME, ROWIDTOCHAR(ROWID) ID from SFIS1.C_DOWNLOAD_WEIGHT_T A WHERE ROWNUM < 20 ";
            }
            else
            {
                strGetData = $" select MODEL_NAME,STANDARD_WEIGHT, UP_WEIGHT,DOWN_WEIGHT,EMP,SKIP_DATA,CTN_GROSS FIX_WEIGHT,DOWNLOAD_TIME, ROWIDTOCHAR(ROWID) ID from SFIS1.C_DOWNLOAD_WEIGHT_T A WHERE ROWNUM < 20 AND UPPER(A.MODEL_NAME) LIKE '%{model.MODEL_NAME.ToUpper()}%' ";
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
        [System.Web.Http.Route("ConfigWeight_MODEL")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ConfigLabel_MODEL(ConfigWeightSetupElement model)
        {
            string strGetData = "select DISTINCT MODEL_NAME from SFIS1.C104 " +
                                "MINUS " +
                                "select DISTINCT MODEL_NAME from SFIS1.C_DOWNLOAD_WEIGHT_T";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }
        [System.Web.Http.Route("InsertUpdateConfigWeight_Setup")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfigWeight_Setup(ConfigWeightSetupElement model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                string modify="";
                //check exist
                string strCheckExist = $"  select EMP from SFIS1.C_DOWNLOAD_WEIGHT_T where MODEL_NAME = '{model.MODEL_NAME}' ";
                string actionString = " ";
                DataTable checkLabel = DBConnect.GetData(strCheckExist, model.database_name);
                if (checkLabel.Rows.Count <= 0)
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'WEIGHT_SETUP_ADD' AND EMP='{model.EMP}'";
                    DataTable checkPri = DBConnect.GetData(strPrivilege, model.database_name);
                    if (checkPri.Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    // not exist => insert
                    sb.Append(" INSERT INTO SFIS1.C_DOWNLOAD_WEIGHT_T ");
                    sb.Append($" ( MODEL_NAME,STANDARD_WEIGHT,UP_WEIGHT,DOWN_WEIGHT,DOWNLOAD_TIME,FIR_MODIFY_TIME,SEC_MODIFY_TIME,EMP,SKIP_DATA,CTN_GROSS ) " +
                        $" VALUES  ( '{model.MODEL_NAME}','{model.STANDARD_WEIGHT}','{MathRound(model.UP_WEIGHT)}','{MathRound(model.DOWN_WEIGHT)}',SYSDATE,SYSDATE,SYSDATE,'{model.EMP}','{model.SKIP_DATA}','{model.CTN_GROSS}' )  ");
                    actionString = "INSERT";
                }
                else
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'WEIGHT_SETUP_EDIT' AND EMP='{model.EMP}'";
                    DataTable checkPri = DBConnect.GetData(strPrivilege, model.database_name);
                    if (checkPri.Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    if (string.IsNullOrEmpty(model.ID))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "notexist" });
                    }
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" UPDATE SFIS1.C_DOWNLOAD_WEIGHT_T ");
                    sb.Append(" SET ");
                    sb.Append($" MODEL_NAME = '{model.MODEL_NAME}', "); //MODEL_NAME
                    sb.Append($" STANDARD_WEIGHT = '{model.STANDARD_WEIGHT}', "); 
                    sb.Append($" UP_WEIGHT = '{MathRound(model.UP_WEIGHT)}', "); 
                    sb.Append($" DOWN_WEIGHT = '{MathRound(model.DOWN_WEIGHT)}', "); 
                    sb.Append($" DOWNLOAD_TIME = SYSDATE, "); //MODIFY_DATE
                    sb.Append($" FIR_MODIFY_TIME = SYSDATE, "); //MODIFY_DATE
                    sb.Append($" SEC_MODIFY_TIME = SYSDATE, "); //MODIFY_DATE
                    sb.Append($" EMP = '{model.EMP}', ");
                    sb.Append($" SKIP_DATA = '{model.SKIP_DATA}', ");
                    sb.Append($" CTN_GROSS = '{model.CTN_GROSS}' ");
                    sb.Append($" WHERE ROWID = '{model.ID}' "); //ID

                    modify = " Change: ";
                    string query = $"  select MODEL_NAME,STANDARD_WEIGHT, UP_WEIGHT,DOWN_WEIGHT,EMP,SKIP_DATA,CTN_GROSS FIX_WEIGHT,DOWNLOAD_TIME from SFIS1.C_DOWNLOAD_WEIGHT_T where ROWID = '{model.ID}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);

                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if(row[0].ToString() != model.MODEL_NAME)
                        {
                            modify += $" MODEL_NAME: {row[0].ToString()} => {model.MODEL_NAME};";
                        }
                        if(row[1].ToString() != model.STANDARD_WEIGHT)
                        {
                            modify += $" STANDARD_WEIGHT: {row[1].ToString()} => {model.STANDARD_WEIGHT};";
                        }
                        if(row[2].ToString() != model.UP_WEIGHT)
                        {
                            modify += $" UP_WEIGHT: {row[2].ToString()} => {model.UP_WEIGHT};";
                        }
                        if(row[3].ToString() != model.DOWN_WEIGHT)
                        {
                            modify += $" DOWN_WEIGHT: {row[3].ToString()} => {model.DOWN_WEIGHT};";
                        }
                        if(row[5].ToString() != model.SKIP_DATA)
                        {
                            modify += $" SKIP_DATA: {row[5].ToString()} => {model.SKIP_DATA};";
                        }
                        if(row[6].ToString() != model.CTN_GROSS)
                        {
                            modify += $" CTN_GROSS: {row[6].ToString()} => {model.CTN_GROSS};";
                        }
                    }
                    

                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'WEIGHT_SETUP MODEL_NAME: {model.MODEL_NAME}; {modify}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_DOWNLOAD_WEIGHT_T' ");
                sbLog.Append(" ) ");
                string strInsertLog = sbLog.ToString();
                string strInserUpdate = sb.ToString();
                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config C_DOWNLOAD_WEIGHT_T
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }

        [System.Web.Http.Route("DeleteConfigWeight_Setup")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfigWeight_Setup(ConfigWeightSetupElement model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'WEIGHT_SETUP_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }
            string strDelete = $" delete SFIS1.C_DOWNLOAD_WEIGHT_T where  ROWID = '{model.ID}' ";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'Config WEIGHT_SETUP MODEL_NAME: {model.MODEL_NAME} IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_DOWNLOAD_WEIGHT_T' ");
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

        public string MathRound(string res)
        {
            var math = float.Parse(res);
            return Math.Round(math, 3).ToString();
        }
        #endregion
    }
}