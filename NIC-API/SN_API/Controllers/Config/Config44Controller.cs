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
    public class Config44Controller : ApiController
    {
        public DataTable dt_new = new DataTable();
        // GET: Config
        [System.Web.Http.Route("GetConfig44Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig44Content(Config44Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = " SELECT MODEL_NAME,VERSION_CODE,MO_TYPE FROM SFIS1.C_CUSTSN_RULE_T where rownum<100 "
                    + $" GROUP BY MODEL_NAME, VERSION_CODE,MO_TYPE"
                    + " ORDER BY MODEL_NAME, VERSION_CODE,MO_TYPE";
            }
            else
            {
                strGetData = $" SELECT A.MODEL_NAME,A.VERSION_CODE,A.MO_TYPE FROM SFIS1.C_CUSTSN_RULE_T A WHERE  UPPER(MODEL_NAME) like  '{model.MODEL_NAME.ToUpper()}%' ";
                if (!string.IsNullOrEmpty(model.MO_TYPE))
                {
                    strGetData = strGetData + $" and MO_TYPE='{model.MO_TYPE}' ";
                }
                if (!string.IsNullOrEmpty(model.VERSION_CODE))
                {
                    strGetData = strGetData + $" and VERSION_CODE='{model.VERSION_CODE}' ";
                }
                strGetData = strGetData + $" GROUP BY MODEL_NAME, VERSION_CODE,MO_TYPE"
                    + " ORDER BY MODEL_NAME, VERSION_CODE,MO_TYPE";

            }
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                //if(!string.IsNullOrEmpty(model.MODEL_NAME) && !string.IsNullOrEmpty(model.VERSION_CODE) && !string.IsNullOrEmpty(model.MO_TYPE))
                //{
                //    string get_detail_custsn = $" SELECT * FROM SFIS1.C_CUSTSN_RULE_T WHERE MODEL_NAME='{model.MODEL_NAME}' AND VERSION_CODE='{model.VERSION_CODE}' AND MO_TYPE='{model.MO_TYPE}' ";
                //    DataTable dt = DBConnect.GetData(get_detail_custsn, model.database_name);
                //    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, data1=dt });
                //}
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }


        [System.Web.Http.Route("GetConfig44Aside")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig44Aside(Config44Element model)
        {
            string strGetData = "";
            if (!string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $" SELECT ROWIDTOCHAR(ROWID) ID,A.* FROM SFIS1.C_CUSTSN_RULE_T A WHERE A.MODEL_NAME = '{model.MODEL_NAME}' AND A.VERSION_CODE = '{model.VERSION_CODE}' AND A.MO_TYPE = '{model.MO_TYPE}'";
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
        [System.Web.Http.Route("GetInsertCustSNCode")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetInsertCustSNCode(Config44Element model)
        {
            StringBuilder get_CUSTSN_DESC_T = new StringBuilder();
            StringBuilder get_C_CUSTSN_RULE_T = new StringBuilder();


            get_CUSTSN_DESC_T.Append("SELECT CUSTSN_CODE FROM SFIS1.C_CUSTSN_DESC_T ");
            get_CUSTSN_DESC_T.Append(" WHERE CUSTSN_NAME IN ( ");
            get_CUSTSN_DESC_T.Append("   SELECT CUSTSN_NAME FROM SFIS1.C_PACK_SEQUENCE_T ");
            get_CUSTSN_DESC_T.Append($" WHERE MODEL_NAME ='{model.MODEL_NAME}' ");
            get_CUSTSN_DESC_T.Append($"  AND VERSION_CODE = '{model.VERSION_CODE}' AND MO_TYPE ='{model.MO_TYPE}')");
            get_CUSTSN_DESC_T.Append(" GROUP BY CUSTSN_CODE");
            string select_cust_desc = get_CUSTSN_DESC_T.ToString();

            get_C_CUSTSN_RULE_T.Append("SELECT * FROM SFIS1.C_CUSTSN_RULE_T ");
            get_C_CUSTSN_RULE_T.Append($" WHERE MODEL_NAME ='{model.MODEL_NAME}' ");
            get_C_CUSTSN_RULE_T.Append($" AND  VERSION_CODE = '{model.VERSION_CODE}' AND MO_TYPE='{model.MO_TYPE}' ");
            string select_cust_rule = get_C_CUSTSN_RULE_T.ToString();

            DataTable dt_cust_desc = DBConnect.GetData(select_cust_desc, model.database_name);
            DataTable dt_cust_rule = DBConnect.GetData(select_cust_rule, model.database_name);


            if (dt_cust_desc.Rows.Count > 0)
            {
                bool check = false;
                foreach (DataRow row in dt_cust_desc.Rows)
                {
                    check = false;
                    if (dt_cust_rule.Rows.Count > 0)
                    {
                        foreach (DataRow row_cust_rule in dt_cust_rule.Rows)
                        {
                            if (row["CUSTSN_CODE"].ToString() == row_cust_rule["CUSTSN_CODE"].ToString())
                            {
                                check = true;
                                break;
                            }
                        }
                    }
                    if (!check)
                    {
                        AddParams(row["CUSTSN_CODE"].ToString());
                    }
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "CHUA THIET LAP CONFIG 43" });
            }
            int a = dt_new.Rows.Count;
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dt_new ,data1 = dt_cust_desc });
        }

        [System.Web.Http.Route("Get_Rull_Name")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Get_Rull_Name(Config44Element model)
        {
            string strgetdata = "";
            DataTable dt = new DataTable();           
            string strgetdata1 = "";
            DataTable dt1 = new DataTable();


            if (model.CUSTSN_CODE.Contains("SSN"))
            {
                strgetdata1 = $"SELECT RULL_NAME FROM SFIS1.C_SSN_RULE_NAME_T WHERE RULE_TYPE ='SSN_RULE'";
                strgetdata = "SELECT DISTINCT CHECK_RULE_NAME FROM SFIS1.C_CUSTSN_RULE_T where CUSTSN_CODE like 'MAC%'";

            }

            if (model.CUSTSN_CODE.Contains("MAC"))
            {
                strgetdata1 = "SELECT RULL_NAME FROM SFIS1.C_SSN_RULE_NAME_T WHERE RULE_TYPE ='MAC_RULE'";
                strgetdata = "SELECT DISTINCT CHECK_RULE_NAME FROM SFIS1.C_CUSTSN_RULE_T where CUSTSN_CODE like 'MAC%'";
            }

            dt = DBConnect.GetData(strgetdata, model.database_name);
            dt1 = DBConnect.GetData(strgetdata1, model.database_name);


            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dt , data1 = dt1 });
        }

        [System.Web.Http.Route("GetModel_Config43")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetModel_Config43(Config44Element model)
        {
            string srt_get = $"SELECT distinct MODEL_NAME FROM SFIS1.C_PACK_SEQUENCE_T ";
            DataTable dt = DBConnect.GetData(srt_get, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dt });
        }

        [System.Web.Http.Route("GetMoTypeAdd")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetMoTypeAdd(Config44Element model)
        {
            string str_sql = $"SELECT DISTINCT MO_TYPE FROM SFIS1.C_PACK_SEQUENCE_T WHERE MODEL_NAME='{model.MODEL_NAME_ADD}'  ";
            DataTable dt = DBConnect.GetData(str_sql, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dt });
        }

        [System.Web.Http.Route("GetMoTypeCopy")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetMoTypeCopy(Config44Element model)
        {
            string str_sql = $"SELECT distinct MO_TYPE FROM SFIS1.C_CUSTSN_RULE_T  where model_name= '{model.MODEL_NAME_COPY}'  ";
            DataTable dt = DBConnect.GetData(str_sql, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dt });
        }


        [System.Web.Http.Route("GetModelCopy")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetModelCopy(Config44Element model)
        {
            string str_sql = "SELECT distinct MODEL_NAME FROM SFIS1.C_CUSTSN_RULE_T ";
            DataTable dt = DBConnect.GetData(str_sql, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dt });
        }

        [System.Web.Http.Route("DeleteConfig44")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig44(Config44Element model)
        {
            //check privilege
            string strDelete = "";
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'STEP RULE_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }
            if (model.TYPE_CHECK == "MODEL")
            {
                strDelete = $"DELETE SFIS1.C_CUSTSN_RULE_T WHERE MODEL_NAME='{model.MODEL_NAME}' and VERSION_CODE='{model.VERSION_CODE}' and MO_TYPE='{model.MO_TYPE}' ";
            }
            else
            {
                strDelete = $" DELETE  SFIS1.C_CUSTSN_RULE_T WHERE ROWID = '{model.ID}'";
            }

            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($" 'Config44 MODEL_NAME: '{model.MODEL_NAME}';IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_CUSTSN_RULE_T' ");
                sbLog.Append(" ) ");

                string strInsertlog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strInsertlog, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }



        [System.Web.Http.Route("InsertUpdateConfig44")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig44(Config44Element model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string actionString = " ";
                string message_error = "";
                //check exist 
                if (model.TYPE_CHECK == "add")
                {
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'STEP RULE_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    string check_model_copy = $"SELECT * FROM SFIS1.C_CUSTSN_RULE_T WHERE "
                                                + $" MODEL_NAME='{model.MODEL_NAME_COPY}' and VERSION_CODE='{model.VERSION_CODE_COPY}' "
                                                + $" AND MO_TYPE='{model.MO_TYPE_COPY}' ";

                    DataTable dt_check = DBConnect.GetData(check_model_copy, model.database_name);
                    if (dt_check.Rows.Count <= 0)
                    {
                        message_error = $"KHONG TON TAI MODEL:{model.MODEL_NAME_COPY} CO VERSION: {model.VERSION_CODE_COPY} va MO_TYPE: {model.MO_TYPE_COPY}";
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = message_error });
                    }
                    string check_model_add = $"SELECT * FROM SFIS1.C_CUSTSN_RULE_T WHERE "
                                                + $" MODEL_NAME='{model.MODEL_NAME_ADD}' and VERSION_CODE='{model.VERSION_CODE_ADD}' "
                                                + $" AND MO_TYPE='{model.MO_TYPE_ADD}' ";
                    dt_check = DBConnect.GetData(check_model_add, model.database_name);
                    if (dt_check.Rows.Count > 0)
                    {
                        message_error = $"TON TAI MODEL: {model.MODEL_NAME_ADD} CO VERSION: {model.VERSION_CODE_ADD} va MO Type:{model.MO_TYPE_ADD}";
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = message_error });
                    }

                    actionString = "ADD NEW";
                    sb.Append($"INSERT INTO SFIS1.C_CUSTSN_RULE_T ");
                    sb.Append($" (MODEL_NAME,VERSION_CODE, MO_TYPE, CUSTSN_CODE, CUSTSN_PREFIX, ");
                    sb.Append($"  CUSTSN_POSTFIX, CUSTSN_LENG, CUSTSN_STR, CHECK_SSN, ");
                    sb.Append("CHECK_RANGE, CHECK_RULE_NAME, SHIPPINGSN_CODE, COMPARE_SN,");
                    sb.Append("COMPARE_SN_START, COMPARE_SN_END, CUSTSN_START, CUSTSN_END,");
                    sb.Append("CREATE_NAME,SHIPPING_CODE) ");
                    sb.Append($"SELECT '{model.MODEL_NAME_ADD}' MODEL_NAME,'{model.VERSION_CODE_ADD}' VERSION_CODE ,'{model.MO_TYPE_ADD}' MO_TYPE, ");
                    sb.Append("CUSTSN_CODE, CUSTSN_PREFIX, CUSTSN_POSTFIX, CUSTSN_LENG,");
                    sb.Append("CUSTSN_STR, CHECK_SSN, CHECK_RANGE, CHECK_RULE_NAME,");
                    sb.Append("SHIPPINGSN_CODE, COMPARE_SN, COMPARE_SN_START, COMPARE_SN_END,");
                    sb.Append($"CUSTSN_START, CUSTSN_END, '{model.EMP}' CREATE_NAME, SHIPPING_CODE");
                    sb.Append(" FROM  SFIS1.C_CUSTSN_RULE_T ");
                    sb.Append($" WHERE MODEL_NAME='{model.MODEL_NAME_COPY}' AND ");
                    sb.Append($" VERSION_CODE='{model.VERSION_CODE_COPY}' AND MO_TYPE='{model.MO_TYPE_COPY}' ");
                }
                else
                {
                    string strCheckexist = $" SELECT * FROM SFIS1.C_CUSTSN_RULE_T WHERE MODEL_NAME = '{model.MODEL_NAME}' "
                                        + $"and MO_TYPE = '{model.MO_TYPE}' and VERSION_CODE = '{model.VERSION_CODE}' "
                                        + $" and CUSTSN_CODE = '{model.CUSTSN_CODE}'";
                    if (DBConnect.GetData(strCheckexist, model.database_name).Rows.Count <= 0)
                    {
                        //check privilege
                        string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'STEP RULE_ADD' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        //not exist => insert
                        actionString = "INSERT";
                        sb.Append("INSERT into");
                        sb.Append($"  SFIS1.C_CUSTSN_RULE_T");
                        sb.Append($" (MODEL_NAME, MO_TYPE, VERSION_CODE, CUSTSN_CODE, CUSTSN_PREFIX,");
                        sb.Append($" CUSTSN_POSTFIX, CUSTSN_LENG, CUSTSN_STR, CHECK_SSN,");
                        sb.Append($" CHECK_RANGE, CHECK_RULE_NAME, SHIPPINGSN_CODE, COMPARE_SN,");
                        sb.Append($" COMPARE_SN_START, COMPARE_SN_END, CUSTSN_START, CUSTSN_END,");
                        sb.Append($" CREATE_NAME,SHIPPING_CODE)");
                        sb.Append($" VALUES (");
                        sb.Append($" '{model.MODEL_NAME}',");
                        sb.Append($" '{model.MO_TYPE}',");
                        sb.Append($" '{model.VERSION_CODE}',");
                        sb.Append($" '{model.CUSTSN_CODE}',");
                        sb.Append($" '{model.CUSTSN_PREFIX}',");
                        sb.Append($" '{model.CUSTSN_POSTFIX}',");
                        sb.Append($" '{model.CUSTSN_LENG}',");
                        sb.Append($" '{model.CUSTSN_STR}',");
                        sb.Append($" '{model.CHECK_SSN}',");
                        sb.Append($" '{model.CHECK_RANGE}',");
                        sb.Append($" '{model.CHECK_RULE_NAME}',");
                        sb.Append($" '{model.SHIPPINGSN_CODE}',");
                        sb.Append($" '{model.COMPARE_SN}',");
                        sb.Append($" '{model.COMPARE_SN_START}',");
                        sb.Append($" '{model.COMPARE_SN_END}',");
                        sb.Append($" '{model.CUSTSN_START}',");
                        sb.Append($" '{model.CUSTSN_END}',");
                        sb.Append($" '{model.CREATE_NAME}',");
                        sb.Append($" '{model.SHIPPING_CODE}')");

                    }
                    else
                    {
                        //check privilege
                        string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'STEP RULE_EDIT' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        //existed => update
                        sb.Clear();
                        actionString = "UPDATE";
                        sb.Append("UPDATE");
                        sb.Append(" SFIS1.C_CUSTSN_RULE_T");
                        sb.Append(" SET ");
                        sb.Append($" MODEL_NAME = '{model.MODEL_NAME}',");
                        sb.Append($" MO_TYPE = '{model.MO_TYPE}',");
                        sb.Append($" VERSION_CODE = '{model.VERSION_CODE}',");
                        sb.Append($" CUSTSN_CODE = '{model.CUSTSN_CODE}',");
                        sb.Append($" CUSTSN_PREFIX = '{model.CUSTSN_PREFIX}',");
                        sb.Append($" CUSTSN_POSTFIX = '{model.CUSTSN_POSTFIX}',");
                        sb.Append($" CUSTSN_LENG = '{model.CUSTSN_LENG}',");
                        sb.Append($" CUSTSN_STR = '{model.CUSTSN_STR}',");
                        sb.Append($" CHECK_SSN = '{model.CHECK_SSN}',");
                        sb.Append($" CHECK_RANGE = '{model.CHECK_RANGE}',");
                        sb.Append($" CHECK_RULE_NAME = '{model.CHECK_RULE_NAME}',");
                        sb.Append($" SHIPPINGSN_CODE = '{model.SHIPPINGSN_CODE}',");
                        sb.Append($" COMPARE_SN = '{model.COMPARE_SN}',");
                        sb.Append($" COMPARE_SN_START = '{model.COMPARE_SN_START}',");
                        sb.Append($" COMPARE_SN_END = '{model.COMPARE_SN_END}',");
                        sb.Append($" CUSTSN_START = '{model.CUSTSN_START}',");
                        sb.Append($" CUSTSN_END = '{model.CUSTSN_END}',");
                        sb.Append($" CREATE_NAME = '{model.CREATE_NAME}',");
                        sb.Append($" SHIPPING_CODE = '{model.SHIPPING_CODE}' ");
                        sb.Append($" WHERE MODEL_NAME = '{model.MODEL_NAME}'");
                        sb.Append($" AND VERSION_CODE = '{model.VERSION_CODE}'");
                        sb.Append($" AND MO_TYPE = '{model.MO_TYPE}'");
                        sb.Append($" AND CUSTSN_CODE = '{model.CUSTSN_CODE}'");
                    }
                }

                //insert log
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                if (model.TYPE_CHECK == "add")
                {
                    sbLog.Append($" 'Config44 MODEL_NAME: {model.MODEL_NAME_ADD}, Version: {model.VERSION_CODE_ADD}, Mo type: {model.MO_TYPE_ADD} ;IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_CUSTSN_RULE_T' ");
                }
                else
                {
                    sbLog.Append($" 'Config44 MODEL_NAME: {model.MODEL_NAME};");
                    sbLog.Append($" Version: {model.VERSION_CODE}; ");
                    sbLog.Append($" Mo type:{model.MO_TYPE}; ");
                    sbLog.Append($" CUSTSN_CODE: {model.CUSTSN_CODE} ");
                    sbLog.Append($" CUSTSN_PREFIX: {model.CUSTSN_PREFIX}");
                    sbLog.Append($"IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_CUSTSN_RULE_T' ");
                }

                sbLog.Append(" ) ");

                string strInsertUpdate = sb.ToString();
                string strInsertlog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strInsertUpdate, model.database_name);
                DBConnect.ExecuteNoneQuery(strInsertlog, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
        private void AddParams(string _name)
        {
            if (dt_new.Columns.Count == 0)
            {
                dt_new.Columns.Add("CUSTSN_CODE");

            }
            DataRow dr = dt_new.NewRow(); // have new row on each iteration
            dr["CUSTSN_CODE"] = _name.ToString();

            dt_new.Rows.Add(dr);
        }
    }
}