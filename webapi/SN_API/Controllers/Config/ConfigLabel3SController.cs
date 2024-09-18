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
    public class ConfigLabel3SController : ApiController
    {
        //CONFIGLABEL3S
        #region 
        [System.Web.Http.Route("GetConfigLabel3SContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfigLabel3SContent(ConfigLabel3SElement model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $" select A.MODEL_NAME,A.SFC_NAME VERSION_CODE,A.TEMP5 STATUS,A.TEMP1 EMPLOYEE_SETUP, ROWIDTOCHAR(ROWID) ID from SFIS1.C_TMM_CONFIG_T A WHERE CUSTOMER_NAME = 'LABEL3S' AND ROWNUM < 20 ";
            }
            else
            {
                strGetData = $" select A.MODEL_NAME,A.SFC_NAME VERSION_CODE,A.TEMP5 STATUS,A.TEMP1 EMPLOYEE_SETUP, ROWIDTOCHAR(ROWID) ID from SFIS1.C_TMM_CONFIG_T A WHERE CUSTOMER_NAME = 'LABEL3S' AND ROWNUM < 20 AND UPPER(A.MODEL_NAME) LIKE '%{model.MODEL_NAME.ToUpper()}%' ";
            }
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                foreach (DataRow row in dtCheck.Rows)
                {
                    row[2] = row[2].ToString() == "0" ? "Confirm to 3S" : row[2].ToString() == "1" ? "OK 3S" :
                         row[2].ToString() == "2" ? "Confirm to Not use" : row[2].ToString() == "3" ? "Not use 3S" : "";
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }
        [System.Web.Http.Route("ConfigLabel3sGetVersion")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ConfigLabel3sGetVersion(ConfigLabel3SElement model)
        {
            string strGetData = "select VERSION_CODE from SFIS1.C_CUST_SNRULE_T where MODEL_NAME = '" + model.MODEL_NAME + "' " +
                                "MINUS " +
                                "select SFC_NAME VERSION_CODE from SFIS1.C_TMM_CONFIG_T where CUSTOMER_NAME = 'LABEL3S' AND MODEL_NAME = '" + model.MODEL_NAME + "'";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }

        [System.Web.Http.Route("InsertUpdateConfigLabel3s")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfigLabel3s(ConfigLabel3SElement model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                //check exist
                string strCheckExist = $"  select TEMP1 from SFIS1.C_TMM_CONFIG_T where CUSTOMER_NAME = 'LABEL3S' AND MODEL_NAME = '{model.MODEL_NAME}' AND SFC_NAME = '{model.VERSION_CODE}' ";
                string actionString = " ";
                DataTable checkLabel = DBConnect.GetData(strCheckExist, model.database_name);
                if (checkLabel.Rows.Count <= 0)
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CONFIG_NIC3S' AND EMP='{model.EMP}'";
                    DataTable checkPri = DBConnect.GetData(strPrivilege, model.database_name);
                    if (checkPri.Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    // not exist => insert
                    sb.Append(" INSERT INTO SFIS1.C_TMM_CONFIG_T ");
                    sb.Append($" ( MODEL_NAME,CUSTOMER_NAME,SFC_NAME,TEMP1,TEMP5 )  VALUES  ( '{model.MODEL_NAME}','LABEL3S','{model.VERSION_CODE}','{model.EMP}','{CheckStatus(model.STATUS)}' )  ");
                    actionString = "INSERT";
                }
                else
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CONFIRM_NIC3S' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    var emp = "";
                    foreach (DataRow row in checkLabel.Rows)
                    {
                        emp = row[0].ToString();
                    }
                    if (emp == model.EMP)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "notpass" });
                    }
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" UPDATE SFIS1.C_TMM_CONFIG_T ");
                    sb.Append(" SET ");
                    sb.Append($" MODEL_NAME = '{model.MODEL_NAME}', "); //MODEL_NAME
                    sb.Append($" SFC_NAME = '{model.VERSION_CODE}', "); //VERSION_CODE
                    sb.Append($" TEMP2 = '{model.EMP}', "); //EMP_NO
                    sb.Append($" TEMP5 = '{ CheckStatus(model.STATUS)}' "); //STATUS
                    sb.Append($" WHERE ROWID = '{model.ID}' "); //ID

                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Label3S MODEL_NAME: {model.MODEL_NAME}; SFC_NAME: {model.VERSION_CODE}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_TMM_CONFIG_T' ");
                sbLog.Append(" ) ");
                string strInsertLog = sbLog.ToString();
                string strInserUpdate = sb.ToString();
                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config label3s
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
        private static string CheckStatus(string status)
        {
            if (status == "Confirm to 3S")
            {
                return "0";
            }
            else if (status == "OK 3S")
            {
                return "1";
            }
            else if (status == "Confirm to Remove")
            {
                return "2";
            }
            else if (status == "Not use")
            {
                return "3";
            }
            return null;
        }
        #endregion


        //CONFIGLABEL_LSSC
        #region 
        [System.Web.Http.Route("GetConfigLabel_lsscContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfigLabel_lsscContent(ConfigLabel3SElement model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $" select A.MODEL_NAME ,A.TEMP5 STATUS,A.TEMP1 EMP_SETUP,A.TEMP2 EMP_UPDATE,A.TEMP3 SETUP_DATE,A.TEMP4 MODIFY_DATE, ROWIDTOCHAR(ROWID) ID from SFIS1.C_TMM_CONFIG_T A WHERE CUSTOMER_NAME = 'LABEL LSSC' AND ROWNUM < 20 ";
            }
            else
            {
                strGetData = $" select A.MODEL_NAME ,A.TEMP5 STATUS,A.TEMP1 EMP_SETUP,A.TEMP2 EMP_UPDATE,A.TEMP3 SETUP_DATE,A.TEMP4 MODIFY_DATE, ROWIDTOCHAR(ROWID) ID from SFIS1.C_TMM_CONFIG_T A WHERE CUSTOMER_NAME = 'LABEL LSSC' AND ROWNUM < 20 AND UPPER(A.MODEL_NAME) LIKE '%{model.MODEL_NAME.ToUpper()}%' ";
            }
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                foreach (DataRow row in dtCheck.Rows)
                {
                    row[1] = row[1].ToString() == "0" ? "NOT USE" : row[1].ToString() == "1" ? "USE" : "";
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }
        [System.Web.Http.Route("ConfigLabel_lsscGetVersion")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ConfigLabel_lsscGetVersion(ConfigLabel3SElement model)
        {
            string strGetData = "select VERSION_CODE from SFIS1.C_CUST_SNRULE_T where MODEL_NAME = '" + model.MODEL_NAME + "' " +
                                "MINUS " +
                                "select SFC_NAME VERSION_CODE from SFIS1.C_TMM_CONFIG_T where CUSTOMER_NAME = 'LABEL LSSC' AND MODEL_NAME = '" + model.MODEL_NAME + "'";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }

        [System.Web.Http.Route("ConfigLabel_MODEL")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ConfigLabel_MODEL(ConfigLabel3SElement model)
        {
            string strGetData = "select DISTINCT MODEL_NAME from SFIS1.c104 " +
                                "MINUS " +
                                "select DISTINCT MODEL_NAME from SFIS1.C_TMM_CONFIG_T where CUSTOMER_NAME = 'LABEL LSSC'";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }

        [System.Web.Http.Route("InsertUpdateConfigLabel_lssc")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfigLabel_lssc(ConfigLabel3SElement model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                //check exist
                string strCheckExist = $"  select TEMP1 from SFIS1.C_TMM_CONFIG_T where CUSTOMER_NAME = 'LABEL LSSC' AND MODEL_NAME = '{model.MODEL_NAME}' ";
                string actionString = " ";
                DataTable checkLabel = DBConnect.GetData(strCheckExist, model.database_name);
                if (checkLabel.Rows.Count <= 0)
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CONFIG_NICLSSC' AND EMP='{model.EMP}'";
                    DataTable checkPri = DBConnect.GetData(strPrivilege, model.database_name);
                    if (checkPri.Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    // not exist => insert
                    sb.Append(" INSERT INTO SFIS1.C_TMM_CONFIG_T ");
                    sb.Append($" ( MODEL_NAME,CUSTOMER_NAME,TEMP1,TEMP5,TEMP3 )  VALUES  ( '{model.MODEL_NAME}','LABEL LSSC','{model.EMP}','{CheckStatusLSSC(model.STATUS)}',TO_CHAR(SYSDATE,'YYYY/MM/DD HH24:MI:SS') )  ");
                    actionString = "INSERT";
                }
                else
                {
                    if (string.IsNullOrEmpty(model.ID))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "notexist" });
                    }
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" UPDATE SFIS1.C_TMM_CONFIG_T ");
                    sb.Append(" SET ");
                    sb.Append($" MODEL_NAME = '{model.MODEL_NAME}', "); //MODEL_NAME
                    //sb.Append($" SFC_NAME = '{model.VERSION_CODE}', "); VERSION_CODE
                    sb.Append($" TEMP2 = '{model.EMP}', "); //EMP_NO
                    sb.Append($" TEMP4 = TO_CHar(SYSDATE,'YYYY/MM/DD HH24:MI:SS'), "); //MODIFY_DATE
                    sb.Append($" TEMP5 = '{ CheckStatusLSSC(model.STATUS)}' "); //STATUS
                    sb.Append($" WHERE ROWID = '{model.ID}' "); //ID

                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Label LSSC MODEL_NAME: {model.MODEL_NAME}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_TMM_CONFIG_T' ");
                sbLog.Append(" ) ");
                string strInsertLog = sbLog.ToString();
                string strInserUpdate = sb.ToString();
                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config label lssc
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
        [System.Web.Http.Route("InsertAllConfigLabel_lssc")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertAllConfigLabel_lssc(ConfigLabel3SElement model)
        {
            try
            {
                if (model.LIST_VERSION_CODE != null)
                {
                    for (int i = 0; i < model.LIST_VERSION_CODE.Count; i++)
                    {
                        var versioncode = model.LIST_VERSION_CODE[i].input;
                        if (model.LIST_VERSION_CODE[i].input.Trim() != "")
                        {
                            StringBuilder sb = new StringBuilder();
                            StringBuilder sbLog = new StringBuilder();
                            string strPrivilege = "";
                            //check exist
                            string strCheckExist = $"  select TEMP1 from SFIS1.C_TMM_CONFIG_T where CUSTOMER_NAME = 'LABEL LSSC' AND MODEL_NAME = '{model.MODEL_NAME}' AND SFC_NAME = '{versioncode}' ";
                            string actionString = " ";
                            DataTable checkLabel = DBConnect.GetData(strCheckExist, model.database_name);
                            if (checkLabel.Rows.Count <= 0)
                            {
                                //check privilege
                                strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CONFIG_NIC3S' AND EMP='{model.EMP}'";
                                DataTable checkPri = DBConnect.GetData(strPrivilege, model.database_name);
                                if (checkPri.Rows.Count <= 0)
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                                }
                                // not exist => insert
                                sb.Append(" INSERT INTO SFIS1.C_TMM_CONFIG_T ");
                                sb.Append($" ( MODEL_NAME,CUSTOMER_NAME,SFC_NAME,TEMP1,TEMP5 )  VALUES  ( '{model.MODEL_NAME}','LABEL LSSC','{versioncode}','{model.EMP}','{CheckStatusLSSC(model.STATUS)}' )  ");
                                actionString = "INSERT";

                                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                                sbLog.Append(" VALUES ( ");
                                sbLog.Append($" '{model.EMP}', ");
                                sbLog.Append($" 'CONFIG', ");
                                sbLog.Append($" '{actionString}', ");
                                sbLog.Append($"  'Label LSSC MODEL_NAME: {model.MODEL_NAME}; SFC_NAME: {versioncode}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_TMM_CONFIG_T' ");
                                sbLog.Append(" ) ");
                                string strInsertLog = sbLog.ToString();
                                string strInserUpdate = sb.ToString();
                                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert config label lssc
                                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log                    
                            }
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "NotVercode" });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
        private static string CheckStatusLSSC(string status)
        {
            if (status.ToUpper() == "USE")
            {
                return "1";
            }
            else if (status.ToUpper() == "NOT USE")
            {
                return "0";
            }
            return null;
        }
        #endregion
    }
}