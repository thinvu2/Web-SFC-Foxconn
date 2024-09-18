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
    public class Config65Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig65Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig65Content(Config65Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = "SELECT ROWID as ID, model_name, group_name, pass_limit, retest_limit,"
                    + " retest_model, repass_model, retest_percent, ddpm, pass_percent, ddpm,"
                    + " temaillist, maillist, pmmaillist, pqemaillist"
                    + " FROM sfis1.c_model_ate_set_t"
                    + " ORDER BY model_name, group_name";
            }
            else
            {
                strGetData = "SELECT ROWID AS ID, model_name, group_name, pass_limit, retest_limit,"
                    + $" retest_model, repass_model, retest_percent, ddpm, pass_percent, ddpm,"
                    + $" temaillist, maillist, pmmaillist, pqemaillist"
                    + $" FROM sfis1.c_model_ate_set_t where upper(model_name) = '{model.MODEL_NAME.ToUpper()}'"
                    + $" ORDER BY model_name, group_name";
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

        // GET: Config
        [System.Web.Http.Route("GetConfigMODEL65Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfigMODEL65Content(Config65Element model)
        {
            string strGetData = "";
            string strGetData1 = "";

            strGetData = "select DISTINCT MODEL_NAME from SFIS1.C_MODEL_DESC_T ";

            strGetData1 = "select distinct GROUP_NAME from SFIS1.C_STATION_CONFIG_T order by GROUP_NAME asc ";


            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            DataTable dtCheck1 = DBConnect.GetData(strGetData1, model.database_name);

            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, data1 = dtCheck1 });

        }


        [System.Web.Http.Route("DeleteConfig65")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig65(Config65Element model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'ATE_CONFIG_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }

            string strDelete = $" DELETE FROM SFIS1.C_MODEL_ATE_SET_T WHERE ROWID= '{model.ID}'";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($" 'Config65 MODEL_NAME: {model.MODEL_NAME};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_ATE_SET_T' ");
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


        [System.Web.Http.Route("InsertUpdateConfig65")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig65(Config65Element model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string actionString = " ";
                string modify = " ";

                string strCheckexist = $" select model_name FROM sfis1.c_model_ate_set_t where model_name = '{model.MODEL_NAME}'";
                if (DBConnect.GetData(strCheckexist, model.database_name).Rows.Count <= 0)
                {
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'ATE_CONFIG_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    actionString = "INSERT";
                    sb.Append("INSERT into");
                    sb.Append($" SFIS1.C_MODEL_ATE_SET_T");
                    sb.Append($" (MODEL_NAME,GROUP_NAME,PASS_LIMIT,RETEST_LIMIT,DDPM,TEMAILLIST,MAILLIST,PMMAILLIST,PQEMAILLIST,RETEST_MODEL,REPASS_MODEL)");
                    sb.Append($"VALUES (");
                    sb.Append($" '{model.MODEL_NAME}', '{model.GROUP_NAME}', '{model.PASS_LIMIT}', '{model.RETEST_LIMIT}', '{model.DDPM}',");
                    sb.Append($" '{model.TEMAILLIST}', '{model.MAILLIST}', '{model.PMMAILLIST}', '{model.PQEMAILLIST}', '{model.RETEST_MODEL}', '{model.REPASS_MODEL}')");
                }
                else
                {
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'ATE_CONFIG_EDIT' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    actionString = "UPDATE";
                    sb.Append("UPDATE ");
                    sb.Append($" SFIS1.C_MODEL_ATE_SET_T");
                    sb.Append($" SET ");
                    sb.Append($" PASS_LIMIT = {model.PASS_LIMIT}, ");
                    sb.Append($" RETEST_LIMIT = {model.RETEST_LIMIT}, ");
                    sb.Append($" RETEST_MODEL = {model.RETEST_MODEL}, ");
                    sb.Append($" REPASS_MODEL = {model.REPASS_MODEL}, ");
                    sb.Append($" RETEST_PERCENT = {model.RETEST_PERCENT}, ");
                    sb.Append($" PASS_PERCENT = {model.PASS_PERCENT}, ");
                    sb.Append($" DDPM = {model.DDPM} ");
                    sb.Append($" WHERE ROWID = '{model.ID}'");


                    modify = " UPDATE: ";
                    string query = $"select * from sfis1.C_MODEL_ATE_SET_T WHERE ROWID = '{model.ID}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);
                }

                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($" 'Config65 MODEL_NAME: {model.MODEL_NAME};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_ATE_SET_T' ");
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