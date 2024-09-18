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
    public class Config40Controller : ApiController
    {
        #region Config Cust_Model
        // GET: Config
        [System.Web.Http.Route("GetConfig40Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig40Content(Config40Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = String.Format(@"select a.CUST_MODEL_NAME BRCM_NAME,a.MODEL_NAME,a.CUSTMODEL_DESC1 SITE,a.CUSTMODEL_DESC2 DEV,
                a.VERSION_CODE,a.CUSTMODEL_DESC5 BRCM_VER,a.CUSTMODEL_DESC3 VRT,
                a.CUSTMODEL_DESC4 DATA2,a.CUSTMODEL_DESC6 DATA3,a.CUSTMODEL_DESC7 DATA4,a.CUSTMODEL_DESC8 DATA5 
                ,a.CUSTMODEL_DESC9 DATA6, CUSTMODEL_DESC10 STATUS, a.CUSTMODEL_DESC12 TIME_SETUP,a.CUSTMODEL_DESC13 EMP,a.rowid ID
                from sfis1.c_cust_model_t a WHERE 1 = 1 AND ROWNUM < 20 
                order by MODEL_NAME,VERSION_CODE");

                //" select a.* ,a.rowid,b.* from sfis1.c_cust_model_t a,sfis1.c_customer_t b where a.cust_code=b.cust_code";
            }
            else
            {
                strGetData = String.Format(@"select a.CUST_MODEL_NAME BRCM_NAME,a.MODEL_NAME,a.CUSTMODEL_DESC1 SITE,a.CUSTMODEL_DESC2 DEV,
                a.VERSION_CODE,a.CUSTMODEL_DESC5 BRCM_VER,a.CUSTMODEL_DESC3 VRT,
                a.CUSTMODEL_DESC4 DATA2,a.CUSTMODEL_DESC6 DATA3,a.CUSTMODEL_DESC7 DATA4,a.CUSTMODEL_DESC8 DATA5 
                ,a.CUSTMODEL_DESC9 DATA6, CUSTMODEL_DESC10 STATUS, a.CUSTMODEL_DESC12 TIME_SETUP,a.CUSTMODEL_DESC13 EMP,a.rowid ID
                from sfis1.c_cust_model_t a WHERE upper(a.model_name) = '{0}' order by MODEL_NAME,VERSION_CODE", model.MODEL_NAME.ToUpper());

                //strGetData = $" select a.* ,a.rowid,b.* from sfis1.c_cust_model_t a,sfis1.c_customer_t b where a.cust_code=b.cust_code and upper(a.model_name) = '{model.MODEL_NAME.ToUpper()}'";
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


        [System.Web.Http.Route("DeleteConfig40")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig40(Config40Element model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CUST MODEL_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }

            string strDelete = $" delete from SFIS1.C_CUST_MODEL_T where ROWID = '{model.ID}'";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'Config40 MODEL_NAME: {model.MODEL_NAME},VERSION_CODE: {model.VERSION_CODE}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_CUST_MODEL_T' ");
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


        [System.Web.Http.Route("InsertUpdateConfig40")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig40(Config40Element model)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbLog = new StringBuilder();
            string actionString = " ";
            string modify = " ";
            try
            {
                //check exis
                string strCheckexist = $"select * from sfis1.c_cust_model_t where MODEL_NAME = '{model.MODEL_NAME}' and  VERSION_CODE = '{model.VERSION_CODE}' ";
                if (DBConnect.GetData(strCheckexist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CUST MODEL_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //not exist => insert
                    actionString = "INSERT";
                    sb.Append("INSERT into");
                    sb.Append($" SFIS1.C_CUST_MODEL_T(MODEL_NAME,CUST_CODE,CUST_MODEL_NAME,CUSTMODEL_DESC1,");
                    sb.Append($" CUSTMODEL_DESC2,CUSTMODEL_DESC3,CUSTMODEL_DESC4,CUSTMODEL_DESC5,VERSION_CODE,CUSTOMER_NAME,");
                    sb.Append($" CUSTMODEL_DESC6,CUSTMODEL_DESC7,CUSTMODEL_DESC8,CUSTMODEL_DESC9,CUSTMODEL_DESC10,CUSTMODEL_DESC12,CUSTMODEL_DESC13)");
                    sb.Append($" VALUES(");
                    sb.Append($" '{model.MODEL_NAME}','80190','{model.CUST_MODEL_NAME}','{model.CUSTMODEL_DESC1}',");
                    sb.Append($" '{model.CUSTMODEL_DESC2}','{model.CUSTMODEL_DESC3}','{model.CUSTMODEL_DESC4}','{model.CUSTMODEL_DESC5}',");
                    sb.Append($" '{model.VERSION_CODE}','{model.CUSTOMER_NAME}','{model.CUSTMODEL_DESC6}','{model.CUSTMODEL_DESC7}',");
                    sb.Append($" '{model.CUSTMODEL_DESC8}','{model.CUSTMODEL_DESC9}','N','{(DateTime.Now).ToString()}','{model.EMP}'");
                    sb.Append($" ) ");

                }
                else
                {
                    //check privilege
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CUST MODEL_EDIT' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    if (model.ID == "" || model.ID == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "not_exist" });
                    }

                    //existed => update
                    actionString = "UPDATE";
                    sb.Append("UPDATE");
                    sb.Append($" SFIS1.C_CUST_MODEL_T");
                    sb.Append($" SET ");
                    sb.Append($" MODEL_NAME = '{model.MODEL_NAME}', CUST_MODEL_NAME = '{model.CUST_MODEL_NAME}', CUSTMODEL_DESC1 = '{model.CUSTMODEL_DESC1}',");
                    sb.Append($" CUSTMODEL_DESC2 = '{model.CUSTMODEL_DESC2}', CUSTMODEL_DESC3 = '{model.CUSTMODEL_DESC3}', CUSTMODEL_DESC4 = '{model.CUSTMODEL_DESC4}',");
                    sb.Append($" CUSTMODEL_DESC5 = '{model.CUSTMODEL_DESC5}', VERSION_CODE = '{model.VERSION_CODE}', CUSTOMER_NAME = '{model.CUSTOMER_NAME}',");
                    sb.Append($" CUSTMODEL_DESC6 = '{model.CUSTMODEL_DESC6}', CUSTMODEL_DESC7 = '{model.CUSTMODEL_DESC7}', ");
                    sb.Append($" CUSTMODEL_DESC8 = '{model.CUSTMODEL_DESC8}', CUSTMODEL_DESC9 = '{model.CUSTMODEL_DESC9}', ");
                    sb.Append($" CUSTMODEL_DESC10 = 'N', CUSTMODEL_DESC11 = '', ");
                    sb.Append($" CUSTMODEL_DESC12 = '{(DateTime.Now).ToString()}', CUSTMODEL_DESC13 = '{model.EMP}' ");
                    sb.Append($" WHERE ROWID = '{model.ID}'");


                    modify = " UPDATE: ";
                    string query = $"select MODEL_NAME,CUST_MODEL_NAME,CUSTMODEL_DESC1,CUSTMODEL_DESC2,CUSTMODEL_DESC3,CUSTMODEL_DESC4," +
                        $"CUSTMODEL_DESC5,VERSION_CODE,CUSTOMER_NAME,CUSTMODEL_DESC6,CUSTMODEL_DESC7,CUSTMODEL_DESC8,CUSTMODEL_DESC9,CUSTMODEL_DESC12," +
                        $"CUSTMODEL_DESC13 from sfis1.C_CUST_MODEL_T WHERE ROWID = '{model.ID}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);
                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if ((row[0].ToString() == null ? "" : row[0].ToString()) != model.MODEL_NAME.Trim())
                        {
                            modify += $" MODEL_NAME: {row[0].ToString()} => {model.MODEL_NAME};";
                        }
                        if ((row[1].ToString() == null ? "" : row[1].ToString()) != model.CUST_MODEL_NAME.Trim())
                        {
                            modify += $" CUST_MODEL_NAME: {row[1].ToString()} => {model.CUST_MODEL_NAME};";
                        }
                        if ((row[2].ToString() == null ? "" : row[2].ToString()) != model.CUSTMODEL_DESC1.Trim())
                        {
                            modify += $" CUSTMODEL_DESC1: {row[2].ToString()} => {model.CUSTMODEL_DESC1};";
                        }
                        if ((row[3].ToString() == null ? "" : row[3].ToString()) != model.CUSTMODEL_DESC2.Trim())
                        {
                            modify += $" CUSTMODEL_DESC2: {row[3].ToString()} => {model.CUSTMODEL_DESC2};";
                        }
                        if ((row[4].ToString() == null ? "" : row[4].ToString()) != model.CUSTMODEL_DESC3.Trim())
                        {
                            modify += $" CUSTMODEL_DESC3: {row[4].ToString()} => {model.CUSTMODEL_DESC3};";
                        }
                        if ((row[5].ToString() == null ? "" : row[5].ToString()) != model.CUSTMODEL_DESC4.Trim())
                        {
                            modify += $" CUSTMODEL_DESC4: {row[5].ToString()} => {model.CUSTMODEL_DESC4};";
                        }
                        if ((row[6].ToString() == null ? "" : row[6].ToString()) != model.CUSTMODEL_DESC5.Trim())
                        {
                            modify += $" CUSTMODEL_DESC5: {row[6].ToString()} => {model.CUSTMODEL_DESC5};";
                        }
                        if ((row[7].ToString() == null ? "" : row[7].ToString()) != model.VERSION_CODE.Trim())
                        {
                            modify += $" VERSION_CODE: {row[7].ToString()} => {model.VERSION_CODE};";
                        }
                        if ((row[8].ToString() == null ? "" : row[8].ToString()) != model.CUSTOMER_NAME.Trim())
                        {
                            modify += $" CUSTOMER_NAME: {row[8].ToString()} => {model.CUSTOMER_NAME};";
                        }
                        if ((row[9].ToString() == null?"": row[9].ToString()) != model.CUSTMODEL_DESC6.Trim())
                        {
                            modify += $" CUSTMODEL_DESC6: {row[9].ToString()} => {model.CUSTMODEL_DESC6};";
                        }
                        if ((row[10].ToString() == null ? "" : row[10].ToString()) != model.CUSTMODEL_DESC7.Trim())
                        {
                            modify += $" CUSTMODEL_DESC7: {row[10].ToString()} => {model.CUSTMODEL_DESC7};";
                        }
                        if ((row[11].ToString() == null ? "" : row[11].ToString()) != model.CUSTMODEL_DESC8.Trim())
                        {
                            modify += $" CUSTMODEL_DESC8: {row[11].ToString()} => {model.CUSTMODEL_DESC8};";
                        }
                        if ((row[12].ToString() == null ? "" : row[12].ToString()) != model.CUSTMODEL_DESC9.Trim())
                        {
                            modify += $" CUSTMODEL_DESC9: {row[12].ToString()} => {model.CUSTMODEL_DESC9};";
                        }

                    }
                }

                //insert log
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config40 MODEL_NAME: {model.MODEL_NAME},VERSION_CODE: {model.VERSION_CODE}; {modify}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_CUST_MODEL_T' ");
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

        [System.Web.Http.Route("Config40_MODEL")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config40_MODEL(Config40Element model)
        {
            string strGetData = "select DISTINCT MODEL_NAME from SFIS1.C_MODEL_DESC_T";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }
        [System.Web.Http.Route("Config40GetVersion")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config40GetVersion(Config40Element model)
        {
            string strGetData = "select VERSION_CODE from SFIS1.C_CUST_SNRULE_T where MODEL_NAME = '" + model.MODEL_NAME + "' " +
                                "MINUS " +
                                "select VERSION_CODE from SFIS1.c_cust_model_t where MODEL_NAME = '" + model.MODEL_NAME + "'";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }
        #endregion

        [System.Web.Http.Route("GetConfirmConfig40Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfirmConfig40Content(Config40Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = String.Format(@"select a.CUST_MODEL_NAME BRCM_NAME,a.MODEL_NAME,a.CUSTMODEL_DESC1 SITE,a.CUSTMODEL_DESC2 DEV,
                a.VERSION_CODE,a.CUSTMODEL_DESC5 BRCM_VER,a.CUSTMODEL_DESC3 VRT,
                a.CUSTMODEL_DESC4 DATA2,a.CUSTMODEL_DESC6 DATA3,a.CUSTMODEL_DESC7 DATA4,a.CUSTMODEL_DESC8 DATA5 
                ,a.CUSTMODEL_DESC9 DATA6, CUSTMODEL_DESC10 STATUS, a.CUSTMODEL_DESC12 TIME_SETUP,a.CUSTMODEL_DESC13 EMP,a.rowid ID
                from sfis1.c_cust_model_t a WHERE (CUSTMODEL_DESC10 <> 'Y' OR CUSTMODEL_DESC10 IS NULL ) AND ROWNUM < 20 ");

                //" select a.* ,a.rowid,b.* from sfis1.c_cust_model_t a,sfis1.c_customer_t b where a.cust_code=b.cust_code";
            }
            else
            {
                strGetData = String.Format(@"select a.CUST_MODEL_NAME BRCM_NAME,a.MODEL_NAME,a.CUSTMODEL_DESC1 SITE,a.CUSTMODEL_DESC2 DEV,
                a.VERSION_CODE,a.CUSTMODEL_DESC5 BRCM_VER,a.CUSTMODEL_DESC3 VRT,
                a.CUSTMODEL_DESC4 DATA2,a.CUSTMODEL_DESC6 DATA3,a.CUSTMODEL_DESC7 DATA4,a.CUSTMODEL_DESC8 DATA5 
                ,a.CUSTMODEL_DESC9 DATA6, CUSTMODEL_DESC10 STATUS,  a.CUSTMODEL_DESC12 TIME_SETUP,a.CUSTMODEL_DESC13 EMP,a.rowid ID
                from sfis1.c_cust_model_t a WHERE upper(a.model_name) like '{0}%' ", model.MODEL_NAME.ToUpper());

                //strGetData = $" select a.* ,a.rowid,b.* from sfis1.c_cust_model_t a,sfis1.c_customer_t b where a.cust_code=b.cust_code and upper(a.model_name) = '{model.MODEL_NAME.ToUpper()}'";
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

        [System.Web.Http.Route("ConfirmConfig40")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ConfirmConfig40(Config40Element model)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbLog = new StringBuilder();
            string actionString = " ";
            string modify = " ";
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

                //existed => update
                actionString = "UPDATE";
                sb.Append("UPDATE");
                sb.Append($" SFIS1.C_CUST_MODEL_T");
                sb.Append($" SET ");
                sb.Append($" CUSTMODEL_DESC10 = 'Y', CUSTMODEL_DESC11 = '{model.EMP}' ");
                sb.Append($" WHERE ROWID = '{model.ID}'");



                //insert log
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'ConfirmConfig40 MODEL_NAME: {model.MODEL_NAME},VERSION_CODE: {model.VERSION_CODE}; CONFIRM: OK; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_CUST_MODEL_T' ");
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