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
using System.Web.Http;
using System.Web.Http.Cors;

namespace SN_API.Controllers.Config
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class Config74Controller : ApiController
    {
        [System.Web.Http.Route("GetConfig74Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig74Content(Config74Element model)
        {
            string getdataemp = string.Empty;
            string detaildata = "SELECT A.EMP_NO, " +
         "         A.ACTION_TYPE, " +
         "         (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                            FROM DUAL " +
         "                                           WHERE LEVEL = 1 " +
         "                                      CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                                    IS NOT NULL), " +
         "                                    '[^:]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                FROM DUAL " +
         "               WHERE LEVEL = 2 " +
         "          CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                            FROM DUAL " +
         "                                           WHERE LEVEL = 1 " +
         "                                      CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                                    IS NOT NULL), " +
         "                                    '[^:]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                        IS NOT NULL) " +
         "            MODEL_NAME, " +
         "         (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                            FROM DUAL " +
         "                                           WHERE LEVEL = 2 " +
         "                                      CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                                    IS NOT NULL), " +
         "                                    '[^:]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                FROM DUAL " +
         "               WHERE LEVEL = 2 " +
         "          CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                            FROM DUAL " +
         "                                           WHERE LEVEL = 1 " +
         "                                      CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                                    IS NOT NULL), " +
         "                                    '[^:]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                        IS NOT NULL) " +
         "            GROUP_NAME, " +
         "         (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                    '[^;]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                FROM DUAL " +
         "               WHERE LEVEL = 3 " +
         "          CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                    '[^;]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                        IS NOT NULL) " +
         "            REASON, " +
         "         (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                    '[^;]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                FROM DUAL " +
         "               WHERE LEVEL = 6 " +
         "          CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                    '[^;]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                        IS NOT NULL) " +
         "            SOLUTION, " +
         "         (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                            FROM DUAL " +
         "                                           WHERE LEVEL = 5 " +
         "                                      CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                                    IS NOT NULL), " +
         "                                    '[^:]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                FROM DUAL " +
         "               WHERE LEVEL = 2 " +
         "          CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                            FROM DUAL " +
         "                                           WHERE LEVEL = 1 " +
         "                                      CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                                    IS NOT NULL), " +
         "                                    '[^:]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                        IS NOT NULL) " +
         "            IP, " +
         "         A.TIME " +
         "    FROM sfism4.R_SYSTEM_LOG_T A " +
         "   WHERE     1 = 1  " +
         "AND action_desc LIKE 'ETE Config%%%' " +
         "       AND action_type IN('UNLOCK', 'LOCK') " +
         "       and EMP_NO not like 'AUTO%' ";

            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                getdataemp = $"select ROWIDTOCHAR(ROWID) ID,A.MODEL_NAME,A.GROUP_NAME,A.TYPE,A.CONDITION,TO_CHAR(A.CURRENT_DATA) CURRENT_DATA ,A.STATUS,ERROR_CODE from sfis1.c_ete_config_t A where 1 = 1  order by A.model_name,A.group_name,A.type";
                detaildata = $"select * from ({detaildata}) where rownum < 30";
            }
            else
            {
                getdataemp = $"select ROWIDTOCHAR(ROWID) ID,A.MODEL_NAME,A.GROUP_NAME,A.TYPE,A.CONDITION ,TO_CHAR(A.CURRENT_DATA) CURRENT_DATA,A.STATUS,ERROR_CODE from sfis1.c_ete_config_t A where A.model_name LIKE UPPER('{model.MODEL_NAME}%')  order by A.model_name,A.group_name,A.type";
                detaildata = detaildata + $" model_name = '{model.MODEL_NAME}'" +
                             $" ORDER  BY A.TIME DESC ";
            }
            dt = DBConnect.GetData(getdataemp, model.database_name);
            dt1 = DBConnect.GetData(detaildata, model.database_name);
            if (dt.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dt, data1 = dt1 });
            }
        }
        [System.Web.Http.Route("InsertOrUpdateConfig74")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertOrUpdateConfig74(Config74Element model)
        {
            string sqldata = string.Empty;
            string sql_log = string.Empty;
            string strPrivilege = string.Empty;
            string sql_check = string.Empty;
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            StringBuilder sbLog = new StringBuilder();
            StringBuilder sb_pri = new StringBuilder();

            var fun = model.ACTION_TYPE == "INSERT" ? "ADD" : model.ACTION_TYPE == "UPDATE" ? "EDIT" : model.ACTION_TYPE;
            fun = "ETE CONFIG_" + fun;

            strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = '{fun}' AND EMP='{model.EMP}'";
            try
            {
                sb_pri.Append($" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG' AND EMP='{model.EMP}' ");
                if (model.ACTION_TYPE == "INSERT")
                {
                    sb_pri.Append(" AND FUN='ETE CONFIG_ADD' ");
                }
                else if (model.ACTION_TYPE == "UPDATE")
                {
                    sb_pri.Append(" AND FUN='ETE CONFIG_EDIT' ");
                }
                else if (model.ACTION_TYPE == "DELETE")
                {
                    sb_pri.Append(" AND FUN='ETE CONFIG_DELETE' ");
                }
                else if (model.ACTION_TYPE == "LOCK")
                {
                    sb_pri.Append(" AND FUN='ETE CONFIG_LOCK' ");
                }
                else if (model.ACTION_TYPE == "UNLOCK")
                {
                    sb_pri.Append(" AND FUN='ETE CONFIG_UNLOCK' ");
                }
                strPrivilege = sb_pri.ToString();

                if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                }

                if (model.ACTION_TYPE == "INSERT")
                {
                    sql_check = $" SELECT * FROM SFIS1.C_ETE_CONFIG_T WHERE MODEL_NAME='{model.MODEL_NAME}' AND GROUP_NAME='{model.GROUP_NAME}' AND TYPE='{model.TYPE}' ";
                    if (DBConnect.GetData(sql_check, model.database_name).Rows.Count > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = $"DA TON TAI MODEL_NAME: {model.MODEL_NAME} and group_name:{model.GROUP_NAME} and type:{model.TYPE} " });
                    }
                    sb.Append(" INSERT INTO SFIS1.C_ETE_CONFIG_T ");
                    sb.Append("(MODEL_NAME,GROUP_NAME,TYPE,CONDITION,CREATE_TIME,STATUS,CURRENT_DATA,ERROR_CODE) ");
                    sb.Append(" VALUES ");
                    sb.Append($" ('{model.MODEL_NAME}','{model.GROUP_NAME}','{model.TYPE}','{model.CONDITION}',sysdate,'','','') ");


                }
                else if (model.ACTION_TYPE == "UPDATE")
                {
                    if (!CHECK_GROUP_NAME(model.GROUP_NAME, model.database_name))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = $"GROUP NAME: {model.GROUP_NAME} khong ton tai trong he thong" });
                    }
                    sb.Append($"UPDATE SFIS1.C_ETE_CONFIG_T SET  ");
                    sb.Append($" CONDITION='{model.CONDITION}' ");
                    sb.Append("  WHERE ");
                    sb.Append($" MODEL_NAME='{model.MODEL_NAME}' and GROUP_NAME='{model.GROUP_NAME}' AND TYPE='{model.TYPE}' ");

                }
                else if (model.ACTION_TYPE == "DELETE")
                {
                    sb.Append($" DELETE SFIS1.C_ETE_CONFIG_T WHERE  ");
                    sb.Append($" MODEL_NAME='{model.MODEL_NAME}' and GROUP_NAME='{model.GROUP_NAME}' AND TYPE='{model.TYPE}' ");
                }
                else
                {
                    sql_check = $" SELECT MODEL_NAME FROM SFIS1.C_ETE_CONFIG_T WHERE MODEL_NAME='{model.MODEL_NAME}' AND GROUP_NAME='{model.GROUP_NAME}' AND TYPE='{model.TYPE}' ";
                    if (DBConnect.GetData(sql_check, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = $"KHÔNG TỒN TẠI MODEL_NAME: {model.MODEL_NAME} and group_name:{model.GROUP_NAME} and type:{model.TYPE} " });
                    }
                    sb.Append($" UPDATE SFIS1.C_ETE_CONFIG_T SET ");
                    if (model.ACTION_TYPE == "LOCK")
                    {
                        sb.Append($" STATUS='LOCK' ");
                    }
                    else if (model.ACTION_TYPE == "UNLOCK")
                    {
                        sql_check = $"select * from sfism4.r105 where mo_number = '{model.MODEL_NAME}'";
                        if (DBConnect.GetData(sql_check, model.database_name).Rows.Count > 0)
                        {
                            sb.Append($" CREATE_TIME= sysdate, ");
                            sb.Append($" CURRENT_DATA= '0', ");
                            sb.Append($" ERROR_CODE= '0', ");
                        }
                        sb.Append(" STATUS='' ");
                    }
                    sb.Append(" WHERE ");
                    sb.Append($" MODEL_NAME='{model.MODEL_NAME}' and GROUP_NAME='{model.GROUP_NAME}' AND TYPE='{model.TYPE}'");
                }

                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{model.ACTION_TYPE}', ");
                if (model.ACTION_TYPE == "LOCK" || model.ACTION_TYPE == "UNLOCK")
                {
                    sbLog.Append($"  'ETE Config - MODEL NAME : {model.MODEL_NAME};GROUP_NAME : {model.GROUP_NAME};{model.REASON}; {AuthorizationController.GetMacAddress()} ;IP:{AuthorizationController.UserIP()};{model.SOLUTION} ;' ");
                }
                else
                {
                    sbLog.Append($"  'ETE Config - MODEL NAME : {model.MODEL_NAME};GROUP_NAME : {model.GROUP_NAME},TYPE: {model.TYPE}; CONDITION={model.CONDITION}; REASON={model.REASON} IP:{AuthorizationController.UserIP()} ;' ");
                }
                sbLog.Append(" ) ");

                sqldata = sb.ToString();
                sql_log = sbLog.ToString();


                DBConnect.ExecuteNoneQuery(sqldata, model.database_name);
                DBConnect.ExecuteNoneQuery(sql_log, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
        [System.Web.Http.Route("GetTypeConfig74")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetTypeConfig74(Config74Element model)
        {
            string getdatatype = string.Empty;
            DataTable dt = new DataTable();
            getdatatype = $"  select distinct type from SFIS1.C_ETE_CONFIG_T   where length(type) > 4  ";
            dt = DBConnect.GetData(getdatatype, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dt });
        }

        public bool CHECK_GROUP_NAME(string groupname, string db)
        {
            string sql = $"SELECT * FROM SFIS1.C_GROUP_CONFIG_T WHERE GROUP_NAME='{groupname}' ";
            if (DBConnect.GetData(sql, db).Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
