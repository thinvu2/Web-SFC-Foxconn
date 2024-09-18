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
    public class Config14Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig14Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig14Content(Config14Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.CUSTOMER))
            {
                strGetData = $" select A.CUSTOMER,A.PASSWD,A.CUST_CODE,ROWIDTOCHAR(ROWID) ID from SFIS1.C_CUSTOMER_T A ";
            }
            else
            {
                strGetData = $"select A.CUSTOMER,A.PASSWD,A.CUST_CODE,ROWIDTOCHAR(ROWID) ID from SFIS1.C_CUSTOMER_T A WHERE UPPER(A.CUSTOMER) LIKE '%{model.CUSTOMER.ToUpper()}%' OR UPPER(A.CUST_CODE) LIKE '%{model.CUSTOMER.ToUpper()}%'";
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
        [System.Web.Http.Route("InsertUpdateConfig14")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig14(Config14Element model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                string modify = " ";
                //check exist
                string strCheckExist = $"  select CUSTOMER from SFIS1.C_CUSTOMER_T where CUST_CODE = '{model.CUST_CODE}' ";
                string actionString = " ";
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CUSTOMER_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    // not exist => insert
                    sb.Append(" INSERT INTO SFIS1.C_CUSTOMER_T ");
                    sb.Append($" ( CUSTOMER,PASSWD,CUST_CODE )  VALUES  ( '{model.CUSTOMER}','{model.PASSWD}','{model.CUST_CODE}' )  ");
                    actionString = "INSERT";
                }
                else
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CUSTOMER_EDIT' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    //check privilege
                    string query = $"select CUSTOMER,PASSWD,CUST_CODE from sfis1.C_CUSTOMER_T WHERE ROWID = '{model.ID}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);
                    if (dtModifly.Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "notexist" });
                    }

                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" UPDATE SFIS1.C_CUSTOMER_T ");
                    sb.Append(" SET ");
                    sb.Append($" CUSTOMER = '{model.CUSTOMER}', "); //CUSTOMER
                    sb.Append($" PASSWD = '{model.PASSWD}', "); //PASSWD
                    sb.Append($" CUST_CODE = '{model.CUST_CODE}' "); //CUST_CODE
                    sb.Append($" WHERE CUST_CODE = '{model.CUST_CODE}' "); //ID


                    modify = " UPDATE: ";
                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if (row[0].ToString() != model.CUSTOMER)
                        {
                            modify += $" CUSTOMER: {row[0].ToString()} => {model.CUSTOMER};";
                        }
                        if (row[1].ToString() != model.PASSWD)
                        {
                            modify += $" PASSWD: {row[1].ToString()} => {model.PASSWD};";
                        }
                        if (row[2].ToString() != model.CUST_CODE)
                        {
                            modify += $" CUST_CODE: {row[2].ToString()} => {model.CUST_CODE};";
                        }
                    }
                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config14 CUSTOMER: {model.CUSTOMER};{modify} ;IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_CUSTOMER_T' ");
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
        [System.Web.Http.Route("DeleteConfig14")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig14(Config14Element model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CUSTOMER_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }
            string strDelete = $" delete SFIS1.C_CUSTOMER_T where  ROWID = '{model.ID}' ";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'Config14 CUSTOMER: {model.CUSTOMER};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_CUSTOMER_T' ");
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