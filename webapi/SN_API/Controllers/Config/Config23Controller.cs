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
    public class Config23Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig23Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig23Content(Config23Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = String.Format(@"select A.*,a.rowid ID from SFIS1.C_MODEL_AQL_T a WHERE 1 = 1 AND ROWNUM < 20 ");

                //" select a.* ,a.rowid,b.* from sfis1.C_MODEL_AQL_T a,sfis1.c_customer_t b where a.cust_code=b.cust_code";
            }
            else
            {
                strGetData = String.Format(@"select A.*,a.rowid ID from SFIS1.C_MODEL_AQL_T a WHERE upper(a.model_name) LIKE '{0}%'", model.MODEL_NAME.ToUpper());

                //strGetData = $" select a.* ,a.rowid,b.* from sfis1.C_MODEL_AQL_T a,sfis1.c_customer_t b where a.cust_code=b.cust_code and upper(a.model_name) = '{model.MODEL_NAME.ToUpper()}'";
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


        [System.Web.Http.Route("DeleteConfig23")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig23(Config23Element model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'AQL_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }

            string strDelete = $" delete from SFIS1.C_MODEL_AQL_T where ROWID = '{model.ID}'";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'Config23 MODEL_NAME: {model.MODEL_NAME}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_AQL_T' ");
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


        [System.Web.Http.Route("InsertUpdateConfig23")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig23(Config23Element model)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbLog = new StringBuilder();
            string actionString = " ";
            string modify = " ";
            try
            {
                //check exist
                string strCheckexist = $"select a.* from sfis1.C_MODEL_AQL_T a where upper(a.model_name) = '{model.MODEL_NAME.ToUpper()}' ";
                if (DBConnect.GetData(strCheckexist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'AQL_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //not exist => insert
                    actionString = "INSERT";
                    sb.Append("INSERT into");
                    sb.Append($" SFIS1.C_MODEL_AQL_T(MODEL_NAME,MO_NUMBER,LOT_SIZE,FQA_TYPE,");
                    sb.Append($" RATE,PILOT_AQL,NORMAL_AQL,EMP_NO,AQL_TIME)");
                    sb.Append($" VALUES(");
                    sb.Append($" '{model.MODEL_NAME}','{model.MO_NUMBER}','{model.LOT_SIZE}','{model.FQA_TYPE}',");
                    sb.Append($" '{model.FQA_RATE}','{model.PILOT_AQL}','{model.NORMAL_AQL}','{model.EMP}',SYSDATE");
                    sb.Append($" ) ");
                }
                else
                {
                    //check privilege
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'AQL_EDIT' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //existed => update
                    actionString = "UPDATE";
                    sb.Append("UPDATE");
                    sb.Append($" SFIS1.C_MODEL_AQL_T");
                    sb.Append($" SET ");
                    sb.Append($" MODEL_NAME = '{model.MODEL_NAME}', MO_NUMBER = '{model.MO_NUMBER}', LOT_SIZE = '{model.LOT_SIZE}',");
                    sb.Append($" FQA_TYPE = '{model.FQA_TYPE}', RATE = '{model.FQA_RATE}', PILOT_AQL = '{model.PILOT_AQL}',");
                    sb.Append($" NORMAL_AQL = '{model.NORMAL_AQL}', AQL_TIME = SYSDATE, EMP_NO = '{model.EMP}' ");
                    sb.Append($" WHERE ROWID = '{model.ID}'");


                    modify = " UPDATE: ";
                    string query = $"select * from sfis1.C_MODEL_AQL_T WHERE ROWID = '{model.ID}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);
                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if (row[0].ToString() != model.MODEL_NAME)
                        {
                            modify += $" MODEL_NAME: {row[0].ToString()} => {model.MODEL_NAME};";
                        }
                        if (row[1].ToString() != model.MO_NUMBER)
                        {
                            modify += $" MO_NUMBER: {row[1].ToString()} => {model.MO_NUMBER};";
                        }
                        if (row[2].ToString() != model.LOT_SIZE)
                        {
                            modify += $" LOT_SIZE: {row[2].ToString()} => {model.LOT_SIZE};";
                        }
                        if (row[3].ToString() != model.FQA_TYPE)
                        {
                            modify += $" FQA_TYPE: {row[3].ToString()} => {model.FQA_TYPE};";
                        }
                        if (row[4].ToString() != model.FQA_RATE)
                        {
                            modify += $" RATE: {row[4].ToString()} => {model.FQA_RATE};";
                        }
                        if (row[5].ToString() != model.PILOT_AQL)
                        {
                            modify += $" PILOT_AQL: {row[5].ToString()} => {model.PILOT_AQL};";
                        }
                        if (row[6].ToString() != model.NORMAL_AQL)
                        {
                            modify += $" NORMAL_AQL: {row[6].ToString()} => {model.NORMAL_AQL};";
                        }
                    }
                }

                //insert log
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config23 MODEL_NAME: {model.MODEL_NAME};{modify}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_AQL_T' ");
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

        [System.Web.Http.Route("Config23_MODEL")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config23_MODEL(Config23Element model)
        {
            string strGetData = "select DISTINCT MODEL_NAME from SFIS1.C_MODEL_DESC_T" +
                " MINUS " +
                "select DISTINCT MODEL_NAME from SFIS1.C_MODEL_AQL_T";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }
        [System.Web.Http.Route("Config23GetMO_number")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config23GetMO_number(Config23Element model)
        {
            string strGetData = "select MO_NUMBER from SFISM4.R105 where MODEL_NAME = '" + model.MODEL_NAME + "' ";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }
    }
}