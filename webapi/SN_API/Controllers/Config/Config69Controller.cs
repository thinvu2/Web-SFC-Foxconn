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
    public class Config69Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig69Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig69Content(Config69Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = "SELECT MODEL_NAME,VERSION_CODE,MO_TYPE FROM SFIS1.C_CUSTSN_RULE_CHECK_T"
                    + " WHERE WAIT_CHECK = 'PQE' GROUP BY MODEL_NAME, VERSION_CODE,MO_TYPE"
                    + " ORDER BY MODEL_NAME, VERSION_CODE, MO_TYPE";
            }
            else
            {
                strGetData = "SELECT MODEL_NAME,VERSION_CODE,MO_TYPE FROM SFIS1.C_CUSTSN_RULE_CHECK_T"
                   + $" WHERE WAIT_CHECK = 'PQE' AND UPPER(MODEL_NAME) = '{model.MODEL_NAME.ToUpper()}' GROUP BY MODEL_NAME, VERSION_CODE,MO_TYPE"
                   + $" ORDER BY MODEL_NAME, VERSION_CODE, MO_TYPE";
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


        [System.Web.Http.Route("GetConfig69Content_Adv")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig69Content_Adv(Config69Element model)
        {
            string strGetData = " SELECT * FROM SFIS1.C_CUSTSN_RULE_CHECK_T "
                + $" WHERE WAIT_CHECK='PQE' AND MODEL_NAME = '{model.MODEL_NAME}'"
                + $" AND  VERSION_CODE = '{model.VERSION_CODE}' AND MO_TYPE = '{model.MO_TYPE}' ORDER BY CUSTSN_CODE";

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


        [System.Web.Http.Route("GetPackSequence")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetPackSequence(Config69Element model)
        {
            string strGetData = " select model_name,version_code,mo_type from sfis1.c_pack_sequence_t "
                + " where (model_name,version_code,mo_type) not in( select model_name,version_code,mo_type from sfis1.c_custsn_rule_check_t )"
                + " group by model_name,version_code,mo_type"
                + " order by model_name,version_code,mo_type";

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


        [System.Web.Http.Route("DeleteConfig69")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig69(Config69Element model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'RULE SETUP_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }

            string strDelete = $" DELETE SFIS1.C_CUSTSN_RULE_CHECK_T WHERE MODEL_NAME = '{model.MODEL_NAME}'"
                + $" AND VERSION_CODE = '{model.VERSION_CODE}' AND CUSTSN_CODE = '{model.CUSTSN_CODE}' AND MO_TYPE = '{model.MO_TYPE}'";
            try
            {
                //insert log
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'Config69 MODEL_NAME: {model.MODEL_NAME};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_KEYPARTS_DESC_T' ");
                sbLog.Append(" ) ");
                string strInsertLog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }


        [System.Web.Http.Route("InsertUpdateConfig69")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig69(Config69Element model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string actionString = " ";

                //check exist
                string strCheckexist = $" SELECT MODEL_NAME FROM SFIS1.C_CUSTSN_RULE_CHECK_T WHERE MODEL_NAME = '{model.MODEL_NAME}'";
                if (DBConnect.GetData(strCheckexist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'RULE SETUP_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //no exist => insert
                    actionString = "INSERT";
                    sb.Append("INSERT into");
                    sb.Append($" SFIS1.C_CUSTSN_RULE_CHECK_T");
                    sb.Append($" (MODEL_NAME, MO_TYPE, VERSION_CODE, CUSTSN_CODE, CUSTSN_PREFIX,");
                    sb.Append($" CUSTSN_POSTFIX, CUSTSN_LENG,CREATE_NAME,WAIT_CHECK )");
                    sb.Append($" VALUES ");
                    sb.Append($" (MODEL_NAME = '{model.MODEL_NAME}', MO_TYPE = '{model.MO_TYPE}', VERSION_CODE = '{model.VERSION_CODE}', CUSTSN_CODE = '{model.CUSTSN_CODE}', CUSTSN_PREFIX = '{model.CUSTSN_PREFIX}',");
                    sb.Append($" (CUSTSN_POSTFIX = '{model.CUSTSN_POSTFIX}', CUSTSN_LENG = '{model.CUSTSN_LENG}', CREATE_NAME = '{model.CREATE_NAME}', WAIT_CHECK = 'PQE'");
                }
                else
                {
                    //check privilege
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'RULE SETUP_EDIT' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //existed => update
                    actionString = "UPDATE";
                    sb.Append("UPDATE");
                    sb.Append($" SFIS1.C_CUSTSN_RULE_CHECK_T");
                    sb.Append($" SET ");
                    sb.Append($" CUSTSN_PREFIX = '{model.CUSTSN_PREFIX}', CUSTSN_POSTFIX = '{model.CUSTSN_POSTFIX}', VERSION_CODE = '{model.VERSION_CODE}', CUSTSN_LENG = '{model.CUSTSN_LENG}',");
                    sb.Append($" MODIFY_NAME = '{model.MODIFY_NAME}',IN_STATION_TIME = SYSDATE");
                    sb.Append($" WHERE WAIT_CHECK='PQE' AND MODEL_NAME = '{model.MODEL_NAME}'");
                }

                //insert log

                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config69 MODEL_NAME: {model.MODEL_NAME};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_KEYPARTS_DESC_T' ");
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