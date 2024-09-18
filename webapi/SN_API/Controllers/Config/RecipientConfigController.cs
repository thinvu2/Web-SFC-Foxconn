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
    public class RecipientConfigController : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetRecipientConfigContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetRecipientConfigContent(RecipientConfigElement mo)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(mo.MO_NUMBER))
            {
                strGetData = $" SELECT A.MO_NUMBER MO,A.SPID  as SHIPPING_ID,A.LOCATION,A.DATA1 CONFIG,A.RECIPIENT_NAME RECIPIENT, A.QTY REQ_QTY, "
                    + $" A.EXT_QTY ACT_QTY, C.TARGET_QTY  MO_QTY, D.QTY SHIPPING_ID_QTY, B.EDIT_TIME CREATE_TIME,A.DATA2 DDR,A.DATA3 SOC "
                    + $" FROM SFIS1.C_RECIPIENT_T A, SFIS1.C_RECIPIENT_SPID_T B, SFISM4.R_BPCS_MOPLAN_T C, (SELECT SUM(TO_NUMBER(QTY)) QTY, DATA1 MO_NUMBER FROM SFIS1.C_RECIPIENT_SPID_T GROUP BY DATA1) D"
                    + $" WHERE A.SPID=B.SPID AND  C.MO_NUMBER=B.DATA1 AND B.DATA1=D.MO_NUMBER  order by A.SPID";
            }
            else
            {
                strGetData = $" SELECT A.MO_NUMBER MO,A.SPID SHIPPING_ID,A.LOCATION,A.DATA1 CONFIG,A.RECIPIENT_NAME RECIPIENT, A.QTY REQ_QTY, A.EXT_QTY ACT_QTY, C.TARGET_QTY  MO_QTY, D.QTY SHIPPING_ID_QTY, B.EDIT_TIME CREATE_TIME "
                    + $" FROM SFIS1.C_RECIPIENT_T A, SFIS1.C_RECIPIENT_SPID_T B, SFISM4.R_BPCS_MOPLAN_T C, (SELECT SUM(TO_NUMBER(QTY)) QTY, DATA1 MO_NUMBER "
                    + $" FROM SFIS1.C_RECIPIENT_SPID_T GROUP BY DATA1) D WHERE A.SPID=B.SPID AND  C.MO_NUMBER=B.DATA1 AND B.DATA1=D.MO_NUMBER AND B.DATA1= '{mo.MO_NUMBER.ToUpper()}'  order by A.SPID";
            }

            DataTable dtCheck = DBConnect.GetData(strGetData, mo.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }


        [System.Web.Http.Route("GetRecipientConfig_QTY")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetRecipientConfig_QTY (RecipientConfigElement mo)
        {
            string strGetData = " SELECT A.MO_NUMBER,QTY_SPID,TARGET_QTY from (SELECT SUM(TO_NUMBER(QTY))QTY_SPID,DATA1 MO_NUMBER  FROM SFIS1.C_RECIPIENT_SPID_T GROUP BY DATA1) A, "
                + $" SFISM4.R_BPCS_MOPLAN_T B WHERE A.MO_NUMBER =B.MO_NUMBER AND B.MO_NUMBER='{mo.MO_NUMBER.ToUpper()}'";

            DataTable dtCheck = DBConnect.GetData(strGetData, mo.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }


        [System.Web.Http.Route("GetRecipientConfigControl")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetRecipientConfigControl(RecipientConfigElement mo)
        {
            string strGetData = " SELECT A.SPID,DATA1 MO_NUMBER, LOCATION, (CASE WHEN FLAG='1' AND QTY > EXT_QTY THEN 'PENDING' WHEN FLAG='1' AND QTY=EXT_QTY THEN 'CLOSED' WHEN FLAG='0' THEN 'OPEN' END) AS STATUS ,QTY,ext_qty, DATA2 LINE "
                + $" FROM SFIS1.C_RECIPIENT_SPID_t A, (select SPID, sum(to_number(ext_qty)) EXT_QTY from SFIS1.C_RECIPIENT_T group by SPID) B where a.spid=b.spid ORDER BY DATA1";

            DataTable dtCheck = DBConnect.GetData(strGetData, mo.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }

        [System.Web.Http.Route("DeleteRecipientConfig")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteRecipientConfig(RecipientConfigElement mo)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG' AND FUN = 'RECIPIENT CONFIG_DELETE' AND EMP='{mo.EMP}'";
            if (DBConnect.GetData(strPrivilege, mo.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }

            string strDelete = $" delete SFIS1.C_RECIPIENT_SPID_T  where spid = '{mo.SPID}'";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, mo.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{mo.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($" 'RecipientConfig MO_NUMBER: {mo.MO_NUMBER};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_RECIPIENT_SPID_T' ");
                sbLog.Append(" ) ");

                string strInsertLog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strInsertLog, mo.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }


        [System.Web.Http.Route("InsertUpdateRecipientConfig")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateRecipientConfig(RecipientConfigElement mo)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string actionString = " ";

                //check exist
                string strCheckexist = $" SELECT * FROM SFIS1.C_RECIPIENT_SPID_T  where spid = '{mo.SPID}'";
                if (DBConnect.GetData(strCheckexist, mo.SPID).Rows.Count <= 0)
                {
                    //check privilage
                    string strPrivilege = $"SELECT * FROM sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG' AND FUN = 'RECIPIENT CONFIG_ADD' AND EMP='{mo.EMP}'";

                    //not exist => insert
                    actionString = "INSERT";
                    sb.Append("INSERT into");
                    sb.Append($" FROM SFIS1.C_RECIPIENT_NAME_T");
                    sb.Append($" (LOCATION, FLAG, recipient_name");
                    sb.Append($" VALUES ");
                    //sb.Append($" ({mo.LOCATION}, {mo.FLAG}, {mo.}")
                   
                }
                else
                {
                    //check privilage
                    string strPrivilege = $"SELECT * FROM sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG' AND FUN = 'RECIPIENT CONFIG_EDIT' AND EMP='{mo.EMP}'";

                }
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            } 
        }
    }
}