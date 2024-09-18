using SN_API.Models;
using SN_API.Models.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    public class GeneralConfigController : ApiController
    {

        #region Make_Weight ConfirmMSN
        [System.Web.Http.Route("GetConfigMake_WeightContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfigMake_WeightContent(ParameterConfig model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $" select A.PRG_NAME,A.VR_CLASS,A.VR_NAME,A.VR_VALUE,A.VR_DESC,ROWIDTOCHAR(ROWID) ID from SFIS1.C_PARAMETER_INI A WHERE PRG_NAME = 'MakeWeight' and VR_CLASS = 'CONFIRMSN' and ROWNUM < 20 ";
            }
            else
            {
                strGetData = $" select A.PRG_NAME,A.VR_CLASS,A.VR_NAME,A.VR_VALUE,A.VR_DESC,ROWIDTOCHAR(ROWID) ID from SFIS1.C_PARAMETER_INI A WHERE PRG_NAME = 'MakeWeight' and VR_CLASS = 'CONFIRMSN' and  UPPER(A.VR_VALUE) LIKE '%{model.MODEL_NAME.ToUpper()}%' ";
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
        [System.Web.Http.Route("ConfigMake_WeightModel")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ConfigMake_WeightModel(ParameterConfig model)
        {
            string strGetData = "select DISTINCT MODEL_NAME from SFIS1.C_MODEL_DESC_T" +
                " MINUS " +
                "select DISTINCT VR_VALUE as MODEL_NAME from SFIS1.C_PARAMETER_INI WHERE PRG_NAME = 'MakeWeight' and VR_CLASS = 'CONFIRMSN'";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }

        [System.Web.Http.Route("DeleteMake_weightConfig")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteMake_weightConfig(ParameterConfig model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PQE_NIC' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }

            string strDelete = $" delete from SFIS1.C_PARAMETER_INI where ROWID = '{model.ID}'";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'MAKE_WEIGHT--CONFIRMSN MODEL_NAME: {model.MODEL_NAME}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_PARAMETER_INI' ");
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

        [System.Web.Http.Route("InsertUpdateMake_weightConfig")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateMake_weightConfig(ParameterConfig model)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbLog = new StringBuilder();
            string actionString = " ";
            string modify = " ";
            try
            {
                //check exist
                string strCheckexist = $"select * from sfis1.C_PARAMETER_INI  where PRG_NAME = 'MakeWeight' and VR_CLASS = 'CONFIRMSN' and upper(VR_VALUE) = '{model.MODEL_NAME.ToUpper()}' ";
                if (DBConnect.GetData(strCheckexist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PQE_NIC' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //not exist => insert
                    actionString = "INSERT";
                    sb.Append("INSERT into");
                    sb.Append($" SFIS1.C_PARAMETER_INI VALUES(");
                    sb.Append($" 'MakeWeight','CONFIRMSN','SI','{model.MODEL_NAME}','{model.MODEL_NAME}','{model.EMP}--'||SYSDATE||'--{AuthorizationController.UserIP()}'");
                    sb.Append($" ) ");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                }

                //insert log
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'MAKE_WEIGHT--CONFIRMSN MODEL_NAME: {model.MODEL_NAME}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_PARAMETER_INI' ");
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
        #endregion

        #region MODEL_ATTRIBUTE
        [System.Web.Http.Route("GetATT")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetATT(ParameterConfig model)
        {
            string strGetData = "";
            List<string> listoutput = new List<string>();
            if (string.IsNullOrEmpty(model.EMP))
            {
                strGetData = $" SELECT TRIM (REGEXP_SUBSTR (txt,'[^|]+',1,LEVEL)) output FROM (select sfis1.z_pkg.get_data('','GET_ATT') txt from dual) CONNECT BY INSTR (txt,'|',1,LEVEL - 1) > 0  ";
            }
            else
            {
                strGetData = $" SELECT TRIM (REGEXP_SUBSTR (txt,'[^|]+',1,LEVEL)) output FROM (select sfis1.z_pkg.get_data('{model.EMP}','GET_ATT') txt from dual) CONNECT BY INSTR (txt,'|',1,LEVEL - 1) > 0 ";
            }
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            foreach (DataRow row in dtCheck.Rows)
            {
                listoutput.Add(row[0].ToString());
            }
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, output = listoutput[0] });
            }
        }
        [System.Web.Http.Route("GetConfigPACK_CTN_ISBRACKETContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfigPACK_CTN_ISBRACKETContent(ParameterConfig model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $" select A.ATTRIBUTE_NAME AS VR_NAME,A.TYPE_VALUE as MODEL_NAME,A.VERSION,A.ATTRIBUTE_VALUE AS VR_VALUE,A.ATTRIBUTE_DESC1 AS DATA1,A.ATTRIBUTE_DESC2 AS DATA2,A.EMP_NO,A.INPUT_TIME,ROWIDTOCHAR(ROWID) ID from SFIS1.C_MODEL_ATTR_CONFIG_T A WHERE ATTRIBUTE_NAME = '{model.VR_NAME}' and ROWNUM < 20 ";
                if (string.IsNullOrEmpty(model.VR_NAME))
                {
                    strGetData = $" select A.ATTRIBUTE_NAME AS VR_NAME,A.TYPE_VALUE as MODEL_NAME,A.VERSION,A.ATTRIBUTE_VALUE AS VR_VALUE,A.ATTRIBUTE_DESC1 AS DATA1,A.ATTRIBUTE_DESC2 AS DATA2,A.EMP_NO,A.INPUT_TIME,ROWIDTOCHAR(ROWID) ID from SFIS1.C_MODEL_ATTR_CONFIG_T A WHERE ATTRIBUTE_NAME IN (" +
                        $"SELECT OUTPUT FROM (SELECT TRIM (REGEXP_SUBSTR (txt,'[^|]+',1,LEVEL)) output FROM (select sfis1.z_pkg.get_data('V1047047','GET_ATT') txt from dual) CONNECT BY INSTR (txt,'|',1,LEVEL - 1) > 0 ORDER BY output ASC) WHERE ROWNUM = 1) and ROWNUM < 20";
                }
            }
            else
            {
                strGetData = $"select A.ATTRIBUTE_NAME AS VR_NAME,A.TYPE_VALUE as MODEL_NAME,A.VERSION,A.ATTRIBUTE_VALUE AS VR_VALUE,A.ATTRIBUTE_DESC1 AS DATA1,A.ATTRIBUTE_DESC2 AS DATA2,A.EMP_NO,A.INPUT_TIME,ROWIDTOCHAR(ROWID) ID from SFIS1.C_MODEL_ATTR_CONFIG_T A WHERE ATTRIBUTE_NAME = '{model.VR_NAME}' and  " +
                    $" (Upper(A.TYPE_VALUE) LIKE '%{model.MODEL_NAME.ToUpper()}%' or Upper(A.ATTRIBUTE_VALUE) LIKE '%{model.MODEL_NAME.ToUpper()}%')  AND ROWNUM < 20 ";
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
        [System.Web.Http.Route("ConfigPACK_CTN_ISBRACKETModel")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ConfigPACK_CTN_ISBRACKETModel(ParameterConfig model)
        {
            string strGetData = "select DISTINCT MODEL_NAME from SFIS1.C_MODEL_DESC_T";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }

        [System.Web.Http.Route("DeletePACK_CTN_ISBRACKET")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeletePACK_CTN_ISBRACKET(ParameterConfig model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'ATTRIBUTE_CONFIG_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }

            string strDelete = $" delete from SFIS1.C_MODEL_ATTR_CONFIG_T where ROWID = '{model.ID}'";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'C_MODEL_ATTR_CONFIG_T ATTRIBUTE_VALUE: {model.VR_NAME}; MODEL_NAME: {model.MODEL_NAME}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_ATTR_CONFIG_T' ");
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

        [System.Web.Http.Route("InsertUpdatePACK_CTN_ISBRACKET")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdatePACK_CTN_ISBRACKET(ParameterConfig model)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbLog = new StringBuilder();
            string actionString = " ";
            string modify = " ";
            try
            {
                if (model.VR_NAME.Trim() == "LABEL_QTY")
                {
                    model.CONFIRM = "WAITING_ME_CONFIRM";
                }
                if (model.VR_NAME.Trim() == "MSL_LABEL_CONFIG")
                {
                    model.CONFIRM = "WAITING_LABEL_CONFIRM";
                }
                //check exist
                string strCheckexist = $"select * from sfis1.C_MODEL_ATTR_CONFIG_T  where ATTRIBUTE_NAME = '{model.VR_NAME.ToUpper()}' AND ";
                if (string.IsNullOrEmpty(model.VERSION))
                {
                    strCheckexist = strCheckexist + "VERSION is null";
                }
                else
                {
                    strCheckexist = strCheckexist + $"VERSION = '{model.VERSION}'";
                }
                strCheckexist = strCheckexist + $"  and upper(TYPE_VALUE) = '{model.MODEL_NAME.ToUpper()}' ";
                if (DBConnect.GetData(strCheckexist, model.database_name).Rows.Count <= 0)
                {

                    if (!string.IsNullOrEmpty(model.ID))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "notupdate" });
                    }
                    //not exist => insert
                    actionString = "INSERT";
                    sb.Append("INSERT into");
                    sb.Append($" SFIS1.C_MODEL_ATTR_CONFIG_T(TYPE_NAME,TYPE_VALUE,VERSION,ATTRIBUTE_NAME,ATTRIBUTE_VALUE,ATTRIBUTE_DESC1,ATTRIBUTE_DESC2,EMP_NO,INPUT_TIME,MODEL_NAME) VALUES(");
                    sb.Append($" 'MODEL_NAME','{model.MODEL_NAME}','{model.VERSION}','{model.VR_NAME}','{model.VR_VALUE}','{model.DATA1}','{model.DATA2}','{model.EMP}',SYSDATE,'{model.CONFIRM}'");
                    sb.Append($" ) ");
                }
                else
                {
                    if (string.IsNullOrEmpty(model.ID))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                    }
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" UPDATE SFIS1.C_MODEL_ATTR_CONFIG_T ");
                    sb.Append(" SET ");
                    sb.Append($" TYPE_VALUE = '{model.MODEL_NAME}' ,"); //MODEL_CODE
                    sb.Append($" VERSION = '{model.VERSION}', ");
                    sb.Append($" ATTRIBUTE_VALUE = '{model.VR_VALUE}', ");
                    sb.Append($" ATTRIBUTE_DESC1 = '{model.DATA1}', ");
                    sb.Append($" ATTRIBUTE_DESC2 = '{model.DATA2}', ");
                    sb.Append($" EMP_NO = '{model.EMP}', ");
                    sb.Append($" MODEL_NAME = '{model.CONFIRM}', ");
                    sb.Append($" INPUT_TIME = SYSDATE ");
                    sb.Append($" WHERE ROWID = '{model.ID}' "); //ID

                    modify = " UPDATE: ";
                    string query = $"select TYPE_VALUE,VERSION,ATTRIBUTE_VALUE,ATTRIBUTE_DESC1,ATTRIBUTE_DESC2 from SFIS1.C_MODEL_ATTR_CONFIG_T WHERE ROWID = '{model.ID}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);
                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if (row[0].ToString() != model.MODEL_NAME)
                        {
                            modify += $" TYPE_VALUE: {row[0].ToString()};";
                        }
                        if (row[0].ToString() != model.VERSION)
                        {
                            modify += $" VERSION: {row[0].ToString()};";
                        }
                        if (row[0].ToString() != model.VR_VALUE)
                        {
                            modify += $" ATTRIBUTE_VALUE: {row[0].ToString()};";
                        }
                        if (row[0].ToString() != model.DATA1)
                        {
                            modify += $" ATTRIBUTE_DESC1: {row[0].ToString()};";
                        }
                        if (row[0].ToString() != model.DATA2)
                        {
                            modify += $" ATTRIBUTE_DESC2: {row[0].ToString()};";
                        }
                    }

                }

                //insert log
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'C_MODEL_ATTR_CONFIG_T--{model.VR_NAME} MODEL_NAME: {model.MODEL_NAME}; {modify}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_ATTR_CONFIG_T' ");
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
        #endregion

        #region ROAST_OUT--ROAST_IN MODEL TELIT
        [System.Web.Http.Route("GetConfigRoast_timeContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfigRoast_timeContent(Roast_timeElement model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $" select A.MODEL_NAME,A.DEFAULT_GROUP,A.END_GROUP,A.CONTROL_TIME,A.EDIT_TIME,A.EDIT_EMP,ROWIDTOCHAR(ROWID) ID from SFIS1.C_ROAST_TIME_CONTROL_T A WHERE ROWNUM < 20 ";
            }
            else
            {
                strGetData = $" select A.MODEL_NAME,A.DEFAULT_GROUP,A.END_GROUP,A.CONTROL_TIME,A.EDIT_TIME,A.EDIT_EMP,ROWIDTOCHAR(ROWID) ID from SFIS1.C_ROAST_TIME_CONTROL_T A WHERE UPPER(A.MODEL_NAME) = '{model.MODEL_NAME.ToUpper()}' ";
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

        [System.Web.Http.Route("ConfigRoast_timeModel")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ConfigRoast_timeModel(Roast_timeElement model)
        {
            string strGetData = "select DISTINCT MODEL_NAME from SFIS1.C_MODEL_DESC_T ";

            string strDefault = $" select DISTINCT DEFAULT_GROUP from SFIS1.C_ROAST_TIME_CONTROL_T";

            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            DataTable dtDefault = DBConnect.GetData(strDefault, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, dtDefault = dtDefault });
        }
        [System.Web.Http.Route("GetEndGroupModel")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetEndGroupModel(Roast_timeElement model)
        {
            string strEnd = $" select DISTINCT END_GROUP from SFIS1.C_ROAST_TIME_CONTROL_T where DEFAULT_GROUP = '{model.DEFAULT_GROUP.ToUpper()}'";
            DataTable dtEnd = DBConnect.GetData(strEnd, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", dtEnd = dtEnd });
        }


        [System.Web.Http.Route("InsertUpdateRoast_time")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateRoast_time(Roast_timeElement model)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbLog = new StringBuilder();
            string actionString = " ";
            string modify = " ";
            try
            {

                //check privilege
                string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PQE_TELIT' AND EMP='{model.EMP}'";
                if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                }
                //check exist
                string strCheckexist = $"select a.* from sfis1.C_ROAST_TIME_CONTROL_T a where upper(a.model_name) = '{model.MODEL_NAME.ToUpper()}'" +
                    $" AND DEFAULT_GROUP = '{model.DEFAULT_GROUP.ToUpper()}' AND END_GROUP = '{model.END_GROUP.ToUpper()}' ";
                if (DBConnect.GetData(strCheckexist, model.database_name).Rows.Count <= 0)
                {
                    //not exist => insert
                    actionString = "INSERT";
                    sb.Append("INSERT into");
                    sb.Append($" SFIS1.C_ROAST_TIME_CONTROL_T VALUES(");
                    sb.Append($" '{model.MODEL_NAME}','{model.DEFAULT_GROUP}','{model.END_GROUP}','{model.CONTROL_TIME}',");
                    sb.Append($" SYSDATE,'{model.EMP}','','','','',''");
                    sb.Append($" ) ");

                }
                else
                {
                    //check privilege
                    string checkexist = $"select * from sfis1.C_ROAST_TIME_CONTROL_T where ROWID='{model.ID}'";
                    if (DBConnect.GetData(checkexist, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "notexist" });
                    }

                    //existed => update
                    actionString = "UPDATE";
                    sb.Append("UPDATE");
                    sb.Append($" SFIS1.C_ROAST_TIME_CONTROL_T");
                    sb.Append($" SET ");
                    sb.Append($" MODEL_NAME = '{model.MODEL_NAME}', DEFAULT_GROUP = '{model.DEFAULT_GROUP}', END_GROUP = '{model.END_GROUP}',");
                    sb.Append($" CONTROL_TIME = '{model.CONTROL_TIME}', EDIT_TIME = SYSDATE, EDIT_EMP = '{model.EMP}'");
                    sb.Append($" WHERE ROWID = '{model.ID}'");


                    modify = " UPDATE: ";
                    string query = $"select MODEL_NAME,DEFAULT_GROUP,END_GROUP,CONTROL_TIME from sfis1.C_ROAST_TIME_CONTROL_T WHERE ROWID = '{model.ID}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);
                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if (row[0].ToString() != model.MODEL_NAME)
                        {
                            modify += $" MODEL_NAME: {row[0].ToString()};";
                        }
                        if (row[1].ToString() != model.DEFAULT_GROUP)
                        {
                            modify += $" DEFAULT_GROUP: {row[1].ToString()};";
                        }
                        if (row[2].ToString() != model.END_GROUP)
                        {
                            modify += $" END_GROUP: {row[2].ToString()};";
                        }
                        if (row[3].ToString() != model.CONTROL_TIME)
                        {
                            modify += $" CONTROL_TIME: {row[3].ToString()};";
                        }
                    }

                }

                //insert log
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'ROAST_TIME MODEL_NAME: {model.MODEL_NAME};{modify}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_ROAST_TIME_CONTROL_T' ");
                sbLog.Append(" ) ");

                string strInsertUpdate = sb.ToString();
                string strInsertLog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strInsertUpdate, model.database_name);
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);
                string strSite = $"select * from sfis1.C_MODEL_SITE_T a where upper(a.model_name) = '{model.MODEL_NAME.ToUpper()}'";
                if (DBConnect.GetData(strSite, model.database_name).Rows.Count <= 0)
                {
                    string insertSite = $"insert into sfis1.C_MODEL_SITE_T values ('{model.MODEL_NAME.ToUpper()}','Thales',SYSDATE,'{model.EMP}') ";
                    DBConnect.ExecuteNoneQuery(insertSite, model.database_name);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }

        #endregion

        #region Route_BomController
        // GET: Route_Bom
        [System.Web.Http.Route("Setup_Route_Bom")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Setup_Route_Bom(Mofix model)
        {
            string strGetData = "";
            if (!string.IsNullOrEmpty(model.MO_NUMBER))
            {
                strGetData = String.Format("select  TYPE_VALUE as MO_NUMBER,  ATTRIBUTE_DESC1 AS ROUTE_NAME,ATTRIBUTE_DESC2 AS BOM_NO, EMP_NO, INPUT_TIME AS TIME, ROWID ID" +
                    "  from SFIS1.C_MODEL_ATTR_CONFIG_T where ATTRIBUTE_NAME='SETUP_ROUTE_BOM' and TYPE_VALUE like '%{0}%' ", model.MO_NUMBER.ToUpper());
            }
            else
            {
                strGetData = String.Format("select  TYPE_VALUE as MO_NUMBER,  ATTRIBUTE_DESC1 AS ROUTE_NAME,ATTRIBUTE_DESC2 AS BOM_NO, EMP_NO, INPUT_TIME AS TIME, ROWID ID" +
                    "  from SFIS1.C_MODEL_ATTR_CONFIG_T where ATTRIBUTE_NAME='SETUP_ROUTE_BOM' ");
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

        [System.Web.Http.Route("UpdateRoute_Bom")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> UpdateRoute_Bom(Mofix model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                string modify = " ";
                //check exist
                string strCheckExist = $"  select * from SFIS1.C_MODEL_ATTR_CONFIG_T where ATTRIBUTE_NAME='SETUP_ROUTE_BOM' and TYPE_VALUE = '{model.MO_NUMBER.Trim()}' ";
                string actionString = " ";
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'SETUP_ROUTE_BOM' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //not exist => insert
                    actionString = "INSERT";
                    sb.Append("INSERT into");
                    sb.Append($" SFIS1.C_MODEL_ATTR_CONFIG_T(TYPE_NAME,TYPE_VALUE,ATTRIBUTE_NAME,");
                    sb.Append($"ATTRIBUTE_DESC1,ATTRIBUTE_DESC2,EMP_NO,INPUT_TIME)");
                    sb.Append($" VALUES(");
                    sb.Append($" 'MO_NUMBER','{model.MO_NUMBER}','SETUP_ROUTE_BOM',");
                    sb.Append($" '{model.ROUTE_NAME}','{model.BOM_NO}','{model.EMP}',SYSDATE");
                    sb.Append($" ) ");
                }
                else
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'SETUP_ROUTE_BOM' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" UPDATE SFIS1.C_MODEL_ATTR_CONFIG_T ");
                    sb.Append(" SET ");
                    sb.Append($" TYPE_VALUE = '{model.MO_NUMBER}' ,"); //MODEL_CODE
                    sb.Append($" ATTRIBUTE_DESC1 = '{model.ROUTE_NAME}', ");
                    sb.Append($" ATTRIBUTE_DESC2 = '{model.BOM_NO}' ");
                    sb.Append($" WHERE ROWID = '{model.ID}' "); //ID

                    modify = " UPDATE: ";
                    string query = $"select ATTRIBUTE_DESC1,ATTRIBUTE_DESC2 from SFIS1.C_MODEL_ATTR_CONFIG_T WHERE ROWID = '{model.ID}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);
                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if (row[0].ToString() != model.ROUTE_NAME)
                        {
                            modify += $" ROUTE_NAME: {row[0].ToString()};";
                        }
                        if (row[1].ToString() != model.BOM_NO)
                        {
                            modify += $" BOM_NO: {row[1].ToString()};";
                        }
                    }

                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'SETUP_ROUTE_BOM MO_NUMBER: {model.MO_NUMBER}; {modify}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_ATTR_CONFIG_T' ");
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

        [System.Web.Http.Route("DeleteRoute_Bom")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteRoute_Bom(Mofix model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'SETUP_ROUTE_BOM' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }

            string strDelete = $" delete from SFIS1.C_MODEL_ATTR_CONFIG_T where ROWID = '{model.ID}'";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'SETUP_ROUTE_BOM MO_NUMBER: {model.MO_NUMBER}; ATTRIBUTE_DESC1: {model.ROUTE_NAME}; ATTRIBUTE_DESC2: {model.BOM_NO}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_ATTR_CONFIG_T' ");
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

        [System.Web.Http.Route("GetAllRoute_Bom")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetAllRoute_Bom(Mofix model)
        {
            string strGetData = $" SELECT ROUTE_NAME  FROM table(SFIS1.Z_PKG.get_route('{model.MO_NUMBER}')) order by route_index ";
            string strGetData1 = $" SELECT BOM_NO FROM table(SFIS1.Z_PKG.get_bomno('{model.MO_NUMBER}')) order by BOM_INDEX ";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            DataTable dtCheck1 = DBConnect.GetData(strGetData1, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, data1 = dtCheck1 });
        }
        #endregion

        #region Confirm LABEL_QTY

        [System.Web.Http.Route("GetLABEL_QTY")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetLABEL_QTY(ParameterConfig model)
        {
            string strGetData = "";
            if (!string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = String.Format("select a.Type_name,a.TYPE_VALUE,a.VERSION,a.ATTRIBUTE_NAME,a.ATTRIBUTE_VALUE,a.ATTRIBUTE_DESC1,a.ATTRIBUTE_DESC2," +
                    " a.EMP_NO,a.INPUT_TIME,a.MODEL_NAME as IS_CONFIRM,a.QTY AS EMP_CONFIRM,a.SET_TIME,A.ROWID AS ID from SFIS1.C_MODEL_ATTR_CONFIG_T A " +
                    " where A.ATTRIBUTE_NAME = 'LABEL_QTY' AND UPPER(A.TYPE_VALUE) = '{0}' ", model.MODEL_NAME.ToUpper());
            }
            else
            {
                strGetData = String.Format("select a.Type_name,a.TYPE_VALUE,a.VERSION,a.ATTRIBUTE_NAME,a.ATTRIBUTE_VALUE,a.ATTRIBUTE_DESC1,a.ATTRIBUTE_DESC2," +
                    " a.EMP_NO,a.INPUT_TIME,a.MODEL_NAME as IS_CONFIRM,a.QTY AS EMP_CONFIRM,a.SET_TIME,A.ROWID AS ID from SFIS1.C_MODEL_ATTR_CONFIG_T A " +
                    " where A.ATTRIBUTE_NAME = 'LABEL_QTY' AND ROWNUM < 20 ");
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

        [System.Web.Http.Route("ConfirmLabelqty")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ConfirmLabelqty(ParameterConfig model)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbLog = new StringBuilder();
            string actionString = " ";
            //string modify = " ";
            try
            {
                //check privilege
                string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PQE_NIC' AND EMP='{model.EMP}'";
                if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                }
                if (model.ID == "" || model.ID == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "not_exist" });
                }
                //getPrivilegePassw
                string getPrivelege = $"select PASSW from sfis1.C_PRIVILEGE where emp ='{model.EMP}' and prg_name ='CONFIG' and fun ='CONFIRM_LABEL_QTY' and rownum =1";
                DataTable getPri = DBConnect.GetData(getPrivelege, model.database_name);
                string getPassw = (getPri.Rows.Count > 0) ? getPri.Rows[0]["PASSW"].ToString() : "";
                //getMODEL_NAME
                string getOldLabel = $"SELECT MODEL_NAME FROM SFIS1.C_MODEL_ATTR_CONFIG_T  WHERE ATTRIBUTE_NAME = 'LABEL_QTY' and TYPE_NAME ='MODEL_NAME' and VERSION ='{model.VERSION_CODE}' and TYPE_VALUE = '{model.MODEL_NAME}'";
                DataTable dtOldValue = DBConnect.GetData(getOldLabel, model.database_name);
                string oldLabel = (dtOldValue.Rows.Count > 0) ? dtOldValue.Rows[0]["MODEL_NAME"].ToString() : "";

                if (model.CONFIRM == "")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "CONFIRM_LABEL not null" });
                }
                if (oldLabel == "WAITING_ME_CONFIRM")
                {
                    string chkPrivilege = $"SELECT * FROM SFIS1.C_PRIVILEGE WHERE EMP = '{model.EMP}' AND PRG_NAME = 'CONFIG' AND FUN = 'CONFIRM_LABEL_QTY' AND PASSW = 'ME' AND ROWNUM = 1";
                    if (DBConnect.GetData(chkPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "You don't have ME privilege" });
                    }
                }
                if (oldLabel == "WAITING_PQE_CONFIRM")
                {
                    string chkPrivilege = $"SELECT * FROM SFIS1.C_PRIVILEGE WHERE EMP = '{model.EMP}' AND PRG_NAME = 'CONFIG' AND FUN = 'CONFIRM_LABEL_QTY' AND PASSW = 'PQE' AND ROWNUM = 1";
                    if (DBConnect.GetData(chkPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "You don't have PQE privilege" });
                    }
                }
                if (model.CONFIRM == "CONFIRM" && oldLabel == "WAITING_ME_CONFIRM")
                {
                    model.CONFIRM = "WAITING_PQE_CONFIRM";
                }
                //existed => update
                actionString = "UPDATE";
                sb.Append("UPDATE");
                sb.Append($" SFIS1.C_MODEL_ATTR_CONFIG_T");
                sb.Append($" SET ");
                sb.Append($" MODEL_NAME = '{model.CONFIRM}', EMP_NO = '{model.EMP}', SET_TIME = to_char(sysdate,'YYYY/DD/MM HH24:MI:SS') ");
                sb.Append($" WHERE ROWID = '{model.ID}'");

                //insert log
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'CONFIRM LABEL_QTY MODEL_NAME: {model.MODEL_NAME}; Version: {model.VERSION_CODE}; Confirm Status: {model.CONFIRM}; Department: {getPassw}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_ATTR_CONFIG_T' ");
                sbLog.Append(" ) ");

                string strInsertUpdate = sb.ToString();
                string strInsertLog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strInsertUpdate, model.database_name);
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (SqlException sqlEx)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "SQL error: " + sqlEx.Message });
            }
            catch (HttpRequestException httpEx)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "HTTP request error: " + httpEx.Message });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "An error occurred: " + ex.Message });
            }
        }
        #endregion

        #region Invoice_ship
        [System.Web.Http.Route("GetPoShip")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetPoShip(ParameterConfig model)
        {
            try
            {
                string strGetData = "";
                if (string.IsNullOrEmpty(model.PO_NO))
                {
                    strGetData = $"select SHIP_INDEX as DELIVERY_NO, REGEXP_SUBSTR ((REGEXP_SUBSTR (SHIP_ADDRESS,'[^|]+',1,1)),'[^:]+',1,2) as AddrTo1," +
                        $"REGEXP_SUBSTR ((REGEXP_SUBSTR (SHIP_ADDRESS,'[^|]+',1,2)),'[^:]+',1,2) as AddrTo2,REGEXP_SUBSTR ((REGEXP_SUBSTR (SHIP_ADDRESS,'[^|]+',1,3)),'[^:]+',1,2) as AddrTo3," +
                        $"REGEXP_SUBSTR ((REGEXP_SUBSTR (SHIP_ADDRESS,'[^|]+',1,4)),'[^:]+',1,2) as AddrTo4,REGEXP_SUBSTR ((REGEXP_SUBSTR (SHIP_ADDRESS,'[^|]+',1,5)),'[^:]+',1,2) as AddrTo5," +
                        $"REGEXP_SUBSTR ((REGEXP_SUBSTR (SHIP_ADDRESS,'[^|]+',1,6)),'[^:]+',1,2) as AddrTo6, REGEXP_SUBSTR ((REGEXP_SUBSTR (SHIP_ADDRESS,'[^|]+',1,7)),'[^:]+',1,2) as AddrTo7," +
                        $"Regexp_substr((Regexp_substr(SHIP_ADDRESS, '[^|]+', 1, 8)), '[^:]+', 1, 2) AS DN_NO," +
                        $"Regexp_substr ((Regexp_substr(SHIP_ADDRESS, '[^|]+', 1, 9)), '[^:]+', 1, 2) AS DATA1," +
                        $"Regexp_substr ((Regexp_substr(SHIP_ADDRESS, '[^|]+', 1, 10)), '[^:]+', 1, 2) AS DATA2," +
                        $" EMP_NO, EDIT_TIME  from SFIS1.C_SHIP_ADDR_T where SHIP_INDEX not in ('98', '99', '123') and ROWNUM < 20 order by EDIT_TIME desc ";
                }
                else
                {
                    strGetData = $"select SHIP_INDEX as DELIVERY_NO, REGEXP_SUBSTR ((REGEXP_SUBSTR (SHIP_ADDRESS,'[^|]+',1,1)),'[^:]+',1,2) as AddrTo1," +
                        $"REGEXP_SUBSTR ((REGEXP_SUBSTR (SHIP_ADDRESS,'[^|]+',1,2)),'[^:]+',1,2) as AddrTo2,REGEXP_SUBSTR ((REGEXP_SUBSTR (SHIP_ADDRESS,'[^|]+',1,3)),'[^:]+',1,2) as AddrTo3," +
                        $"REGEXP_SUBSTR ((REGEXP_SUBSTR (SHIP_ADDRESS,'[^|]+',1,4)),'[^:]+',1,2) as AddrTo4,REGEXP_SUBSTR ((REGEXP_SUBSTR (SHIP_ADDRESS,'[^|]+',1,5)),'[^:]+',1,2) as AddrTo5," +
                        $"REGEXP_SUBSTR ((REGEXP_SUBSTR (SHIP_ADDRESS,'[^|]+',1,6)),'[^:]+',1,2) as AddrTo6,REGEXP_SUBSTR ((REGEXP_SUBSTR (SHIP_ADDRESS,'[^|]+',1,7)),'[^:]+',1,2) as AddrTo7," +
                        $"Regexp_substr((Regexp_substr(SHIP_ADDRESS, '[^|]+', 1, 8)), '[^:]+', 1, 2) AS DN_NO," +
                        $"Regexp_substr ((Regexp_substr(SHIP_ADDRESS, '[^|]+', 1, 9)), '[^:]+', 1, 2) AS DATA1," +
                        $"Regexp_substr ((Regexp_substr(SHIP_ADDRESS, '[^|]+', 1, 10)), '[^:]+', 1, 2) AS DATA2," +
                        $"EMP_NO, EDIT_TIME  from SFIS1.C_SHIP_ADDR_T where  SHIP_INDEX = '{model.PO_NO}' and SHIP_INDEX not in ('98', '99', '123') ";
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
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { result = ex.Message });
            }

        }
        [System.Web.Http.Route("InsertUpdatePoShip")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdatePoShip(ParameterConfig model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sblog = new StringBuilder();

                string strCheckExit = ($" select SHIP_INDEX from SFIS1.C_SHIP_ADDR_T where SHIP_INDEX ='{model.PO_NO}'");
                string actionString = " ";
                //getShipAddressOld
                string getOldValueQuery = $"SELECT SHIP_ADDRESS FROM SFIS1.C_SHIP_ADDR_T WHERE SHIP_INDEX ='{model.PO_NO}'";
                DataTable dtOldValue = DBConnect.GetData(getOldValueQuery, model.database_name);
                string oldShipAddress = (dtOldValue.Rows.Count > 0) ? dtOldValue.Rows[0]["SHIP_ADDRESS"].ToString() : "";

                if (DBConnect.GetData(strCheckExit, model.database_name).Rows.Count <= 0)
                {
                    sb.Append("INSERT INTO SFIS1.C_SHIP_ADDR_T (SHIP_INDEX, SHIP_ADDRESS, EMP_NO, EDIT_TIME) ");
                    sb.Append($" values ('{model.PO_NO}', '{model.SHIP_ADDRESS}', '{model.EMP}', sysdate)");
                    actionString = "INSERT";
                }
                else
                {
                    sb.Append($"UPDATE SFIS1.C_SHIP_ADDR_T SET SHIP_INDEX ='{model.PO_NO}', SHIP_ADDRESS ='{model.SHIP_ADDRESS}', EMP_NO ='{model.EMP}', EDIT_TIME = sysdate WHERE SHIP_INDEX ='{model.PO_NO}'");
                    actionString = "UPDATE";
                }

                sblog.Append(" INSERT INTO SFISM4.R_SYSTEM_LOG_T (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sblog.Append(" VALUES ( ");
                sblog.Append($" '{model.EMP}', ");
                sblog.Append($" 'CONFIG', ");
                sblog.Append($" '{actionString}', ");
                sblog.Append($"  'TO_NOSHIP: {model.PO_NO}; SHIP_ADDRESS_OLD: {oldShipAddress}; SHIP_ADDRESS_NEW: {model.SHIP_ADDRESS}; Emp: {model.EMP} ; IP:{AuthorizationController.UserIP()}'");
                sblog.Append(" ) ");

                string queryString = sb.ToString();
                string queryStringLog = sblog.ToString();
                DataTable result = DBConnect.GetData(queryString, model.database_name);
                DataTable resultLog = DBConnect.GetData(queryStringLog, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (SqlException ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Database error", message = ex.Message });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });

            }
        }
        [System.Web.Http.Route("DeletePoShip")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeletePoShip(ParameterConfig model)
        {
            string strDelete = $" DELETE SFIS1.C_SHIP_ADDR_T where SHIP_INDEX = '{model.PO_NO}'";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'TO_NOSHIP: {model.PO_NO}; SHIP_ADDRESS: {model.SHIP_ADDRESS}; Emp: {model.EMP} ; IP:{AuthorizationController.UserIP()}' ");
                sbLog.Append(" ) ");

                string strInsertLog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { result = ex.Message });
            }
        }
        [System.Web.Http.Route("GetDN_NO")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetDN_NO(ParameterConfig model)
        {
            string strGetData = $"select DISTINCT DN_no as DN from SFISM4.R_SAP_DN_DETAIL_T where CREATE_DATE > sysdate -60";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }
        #endregion

        #region Confirm LABEL_MSL

        [System.Web.Http.Route("GetLABEL_MSL")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetLABEL_MSL(ParameterConfig model)
        {
            string strGetData = "";
            if (!string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = String.Format("select a.Type_name,a.TYPE_VALUE,a.VERSION,a.ATTRIBUTE_NAME,a.ATTRIBUTE_VALUE,a.MODEL_NAME as IS_CONFIRM," +
                    " a.EMP_NO,a.INPUT_TIME,a.QTY AS EMP_CONFIRM,a.SET_TIME,a.ATTRIBUTE_DESC1,a.ATTRIBUTE_DESC2,A.ROWID AS ID from SFIS1.C_MODEL_ATTR_CONFIG_T A " +
                    " where A.ATTRIBUTE_NAME = 'MSL_LABEL_CONFIG' AND UPPER(A.TYPE_VALUE) = '{0}' ", model.MODEL_NAME.ToUpper());
            }
            else
            {
                strGetData = String.Format("select a.Type_name,a.TYPE_VALUE,a.VERSION,a.ATTRIBUTE_NAME,a.ATTRIBUTE_VALUE,a.MODEL_NAME as IS_CONFIRM," +
                    " a.EMP_NO,a.INPUT_TIME,a.QTY AS EMP_CONFIRM,a.SET_TIME,a.ATTRIBUTE_DESC1,a.ATTRIBUTE_DESC2,A.ROWID AS ID from SFIS1.C_MODEL_ATTR_CONFIG_T A " +
                    " where A.ATTRIBUTE_NAME = 'MSL_LABEL_CONFIG' AND ROWNUM < 20 ");
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

        [System.Web.Http.Route("ConfirmLabelMSL")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ConfirmLabelMSL(ParameterConfig model)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbLog = new StringBuilder();
            string actionString = " ";
            //string modify = " ";
            try
            {
                //check privilege
                string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PQE_NIC' AND EMP='{model.EMP}'";
                if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                }
                if (model.ID == "" || model.ID == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "not_exist" });
                }
                //getPrivilegePassw
                string getPrivelege = $"select PASSW from sfis1.C_PRIVILEGE where emp ='{model.EMP}' and prg_name ='CONFIG' and fun ='CONFIRM_MSL_LABEL' and rownum =1";
                DataTable getPri = DBConnect.GetData(getPrivelege, model.database_name);
                string getPassw = (getPri.Rows.Count > 0) ? getPri.Rows[0]["PASSW"].ToString() : "";
                //getMODEL_NAME
                string getOldLabel = $"SELECT MODEL_NAME FROM SFIS1.C_MODEL_ATTR_CONFIG_T  WHERE ATTRIBUTE_NAME = 'MSL_LABEL_CONFIG' and TYPE_NAME ='MODEL_NAME' and TYPE_VALUE = '{model.MODEL_NAME}'";
                DataTable dtOldValue = DBConnect.GetData(getOldLabel, model.database_name);
                string oldLabel = (dtOldValue.Rows.Count > 0) ? dtOldValue.Rows[0]["MODEL_NAME"].ToString() : "";

                if (model.CONFIRM == "")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "CONFIRM_LABEL not null" });
                }
                if (oldLabel == "WAITING_LABEL_CONFIRM")
                {
                    string chkPrivilege = $"SELECT * FROM SFIS1.C_PRIVILEGE WHERE EMP = '{model.EMP}' AND PRG_NAME = 'CONFIG' AND FUN = 'CONFIRM_MSL_LABEL' AND PASSW = 'LABEL' AND ROWNUM = 1";
                    if (DBConnect.GetData(chkPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "You don't have ME privilege" });
                    }
                }
                if (oldLabel == "WAITING_PQE_CONFIRM")
                {
                    string chkPrivilege = $"SELECT * FROM SFIS1.C_PRIVILEGE WHERE EMP = '{model.EMP}' AND PRG_NAME = 'CONFIG' AND FUN = 'CONFIRM_MSL_LABEL' AND PASSW = 'PQE' AND ROWNUM = 1";
                    if (DBConnect.GetData(chkPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "You don't have PQE privilege" });
                    }
                }
                if (model.CONFIRM == "CONFIRM" && oldLabel == "WAITING_LABEL_CONFIRM")
                {
                    model.CONFIRM = "WAITING_PQE_CONFIRM";
                }
                //existed => update
                actionString = "UPDATE";
                sb.Append("UPDATE");
                sb.Append($" SFIS1.C_MODEL_ATTR_CONFIG_T");
                sb.Append($" SET ");
                sb.Append($" MODEL_NAME = '{model.CONFIRM}', EMP_NO = '{model.EMP}', SET_TIME = to_char(sysdate,'YYYY/DD/MM HH24:MI:SS') ");
                sb.Append($" WHERE ROWID = '{model.ID}'");

                //insert log
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'CONFIRM LABEL_QTY MODEL_NAME: {model.MODEL_NAME}; Version: {model.VERSION_CODE}; Confirm Status: {model.CONFIRM}; Department: {getPassw}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_ATTR_CONFIG_T' ");
                sbLog.Append(" ) ");

                string strInsertUpdate = sb.ToString();
                string strInsertLog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strInsertUpdate, model.database_name);
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (SqlException sqlEx)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "SQL error: " + sqlEx.Message });
            }
            catch (HttpRequestException httpEx)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "HTTP request error: " + httpEx.Message });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "An error occurred: " + ex.Message });
            }
        }
        #endregion

        #region QueryID
        [System.Web.Http.Route("Type_Query_ID")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Type_Query_ID(ParameterConfig model)
        {
            string strGetData = "";
            strGetData = String.Format(" SELECT TRIM (REGEXP_SUBSTR (txt,'[^|]+',1,LEVEL)) output FROM (select sfis1.z_pkg.get_data('','GET_QUERY_TYPE') txt from dual) CONNECT BY INSTR (txt,'|',1,LEVEL - 1) > 0 ");
                
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
        [System.Web.Http.Route("Query_ID")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Query_ID(ParameterConfig model)
        {
            try
            {
                string strGetData1 = "";
                var type = "GET_SQL_BY_" + model.VR_NAME.Trim();
                strGetData1 = $"select sfis1.z_pkg.get_data('{model.VR_VALUE}','{type}') txt from dual";
                //GET_SQL_BY_MAILID
                DataTable dtCheck1 = DBConnect.GetData(strGetData1, model.database_name);
                if (dtCheck1.Rows[0]["txt"].ToString().Contains("not found"))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                }   

                DataTable dtCheck2 = DBConnect.GetData(dtCheck1.Rows[0]["txt"].ToString(), model.database_name);

                if (dtCheck2.Rows.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck2 , queryTime = DateTime.Now.ToString() });
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex });
            }
        }
        #endregion
    }
}