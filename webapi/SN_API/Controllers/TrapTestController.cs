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
    public class TrapTestController : ApiController
    {
        // GET: CheckSN
        [System.Web.Http.Route("GetTrapTestContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetTrapTestContent(ValueTrapTest model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = String.Format("SELECT SERIAL_NUMBER,MO_NUMBER,MODEL_NAME,HOLD_FLAG ACTION,HOLD_EMP_NO CHECK_EMP," +
                    "HOLD_TIME CHECK_TIME,UNHOLD_EMP_NO CONFIRM_EMP,UNHOLD_TIME CONFIRM_TIME,ROWID ID FROM SFISM4.R_SN_EXCEPT_HOLD_T");
            }
            else
            {
                strGetData = String.Format("SELECT SERIAL_NUMBER,MO_NUMBER,MODEL_NAME,HOLD_FLAG ACTION,HOLD_EMP_NO CHECK_EMP," +
                    "HOLD_TIME CHECK_TIME,UNHOLD_EMP_NO CONFIRM_EMP,UNHOLD_TIME CONFIRM_TIME,ROWID ID FROM SFISM4.R_SN_EXCEPT_HOLD_T WHERE " +
                    "(MO_NUMBER like '%" + model.MODEL_NAME + "%' OR MODEL_NAME like '%" + model.MODEL_NAME + "%' OR SERIAL_NUMBER = '" + model.MODEL_NAME + "')");
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

        [System.Web.Http.Route("CheckSNTrapTest")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> CheckSNTrapTest(ValueTrapTest model)
        {
            try
            {
                string strGetData = "";
                string strGetData1 = "";
                string strGetData2 = "";
                string strGetData3 = "";
                strGetData = String.Format("SELECT * FROM SFISM4.R107 WHERE SERIAL_NUMBER = '" + model.SERIAL_NUMBER + "' AND (MCARTON_NO IS NOT NULL AND MCARTON_NO <> 'N/A')");
                strGetData1 = String.Format("SELECT * FROM SFISM4.R_SN_EXCEPT_HOLD_T WHERE SERIAL_NUMBER = '" + model.SERIAL_NUMBER + "' AND HOLD_FLAG = 'Y'");
                strGetData2 = String.Format("SELECT MO_NUMBER,MODEL_NAME FROM SFISM4.R107 WHERE SERIAL_NUMBER = '" + model.SERIAL_NUMBER + "'");
                strGetData3 = String.Format("SELECT * FROM SFISM4.R_SN_EXCEPT_HOLD_T WHERE SERIAL_NUMBER = '" + model.SERIAL_NUMBER + "' AND HOLD_FLAG = 'N'");


                DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
                DataTable dtCheck1 = DBConnect.GetData(strGetData1, model.database_name);
                DataTable dtCheck2 = DBConnect.GetData(strGetData2, model.database_name);
                DataTable dtCheck3 = DBConnect.GetData(strGetData3, model.database_name);
                if (dtCheck.Rows.Count == 0 && dtCheck1.Rows.Count == 0)
                {
                    if (dtCheck2.Rows.Count == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                    }
                    else
                    {
                        if (dtCheck3.Rows.Count == 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", action = true, data = dtCheck2 });
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", action = false, data = dtCheck2 });
                        }
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "exist" });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
        }
        [System.Web.Http.Route("InsertUpdateTrapTest")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateTrapTest(ValueTrapTest model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string actionString = " ";

                //check exist
                string strCheckexist = $"SELECT * FROM SFISM4.R_SN_EXCEPT_HOLD_T WHERE SERIAL_NUMBER = '" + model.SERIAL_NUMBER + "'AND HOLD_FLAG = 'N'";
                if (DBConnect.GetData(strCheckexist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where FUN = 'SN_CHECK' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //no exist => insert
                    actionString = "INSERT";
                    sb.Append("INSERT into");
                    sb.Append($" SFISM4.R_SN_EXCEPT_HOLD_T(");
                    sb.Append($" SERIAL_NUMBER,MO_NUMBER,MODEL_NAME,HOLD_FLAG,HOLD_EMP_NO,HOLD_TIME,UNHOLD_EMP_NO,UNHOLD_TIME)");
                    sb.Append($" VALUES ");
                    sb.Append($" ('{model.SERIAL_NUMBER}','{model.MO_NUMBER}','{model.MODEL_NAME}','{model.ACTION}','{model.EMP}',SYSDATE,'','') ");

                }
                else
                {
                    //check privilege
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'SN_CHECK' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //existed => update
                    actionString = "UPDATE";
                    sb.Append("UPDATE");
                    sb.Append($" SFISM4.R_SN_EXCEPT_HOLD_T");
                    sb.Append($" SET ");
                    sb.Append($" HOLD_FLAG = '{model.ACTION}', UNHOLD_EMP_NO = '{model.EMP}', UNHOLD_TIME = SYSDATE ");
                    sb.Append($" WHERE SERIAL_NUMBER = '" + model.SERIAL_NUMBER + "'");
                }

                //insert log

                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config67 MODEL_NAME: {model.MODEL_NAME};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_PARAMETER_INI' ");
                sbLog.Append(" ) ");

                string strInsertUpdate = sb.ToString();
                string strInsertLog = sbLog.ToString();

                DBConnect.ExecuteNoneQuery(strInsertUpdate, model.database_name);
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
        }
    }
}