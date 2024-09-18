using SN_API.Models;
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
    public class LockController : ApiController
    {
        public string GetIP()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        [System.Web.Http.Route("LockUpdate")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> LockUpdate(ETEConfigInfo eteConfig)
        {
            string database_name = eteConfig.database_name;
            string model_name = eteConfig.model_name;
            string group_name = eteConfig.group_name;
            string condition = eteConfig.condition;
            string condition_value = eteConfig.condition_value;
            string emp_pass = eteConfig.emp_pass;
            string ip_address = GetIP();

            string queryString = "select * from SFIS1.C_PRIVILEGE where emp in (select distinct emp_no from sfis1.C_EMP_DESC_T where emp_pass= '" + emp_pass + "' and rownum =1) and fun = 'ETE CONFIG_EDIT' and prg_name = 'CONFIG'";
            DataTable dtCheck = DBConnect.GetData(queryString, database_name);
            if (dtCheck.Rows.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });


            string formatUpdate = "update sfis1.c_ete_config_t set condition='{0}',create_time=sysdate where model_name= '{1}'  and type='{2}' and group_name = '{3}'";
            string queryUpdate = string.Format(formatUpdate, condition_value, model_name, condition, group_name);
            DBConnect.ExecuteNoneQuery(queryUpdate, database_name);

            string queryInserLog = "Insert into SFISM4.R_SYSTEM_LOG_T(EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC,TIME) Values (''||(SELECT EMP_NO FROM SFIS1.C_EMP_DESC_T where EMP_PASS = '" + emp_pass + "' and rownum =1)||'','CONFIG','UPDATE','ETE Config - MODEL NAME : " + model_name + "; GROUP NAME : " + group_name + "; " + condition + ":" + condition_value + ";MAC ADDRESS﹕null;IP:" + ip_address + "',sysdate)";

            DBConnect.ExecuteNoneQuery(queryInserLog, database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
        }

        [System.Web.Http.Route("LockStation")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> LockStation(ETEConfigInfo eteConfig)
        {
            string database_name = eteConfig.database_name;
            string model_name = eteConfig.model_name;
            string group_name = eteConfig.group_name;
            string condition = eteConfig.condition;
            string condition_value = eteConfig.condition_value;
            string emp_pass = eteConfig.emp_pass;
            string reason = eteConfig.reason;
            string ip_address = GetIP();
            string queryString = "select * from SFIS1.C_PRIVILEGE where emp in (select distinct emp_no from sfis1.C_EMP_DESC_T where emp_pass= '" + emp_pass + "' and rownum =1) and fun = 'ETE CONFIG_LOCK' and prg_name = 'CONFIG'";
            DataTable dtCheck = DBConnect.GetData(queryString, database_name);
            if (dtCheck.Rows.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });

            string formatUpdate = "update sfis1.c_ete_config_t set status='LOCK',create_time=sysdate where model_name= '{1}'  and type='{2}' and group_name = '{3}'";
            string queryUpdate = string.Format(formatUpdate, condition_value, model_name, condition, group_name);
            DBConnect.ExecuteNoneQuery(queryUpdate, database_name);

            string queryInserLog = "Insert into SFISM4.R_SYSTEM_LOG_T(EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC,TIME) Values (''||(SELECT EMP_NO FROM SFIS1.C_EMP_DESC_T where EMP_PASS = '" + emp_pass + "' and rownum =1)||'','CONFIG','LOCK','ETE Config - MODEL NAME : " + model_name + "; GROUP NAME : " + group_name + "; " + reason + ";MAC ADDRESS﹕null;IP:" + ip_address + "',sysdate)";

            DBConnect.ExecuteNoneQuery(queryInserLog, database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
        }

        [System.Web.Http.Route("UnLockStation")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> UnLockStation(ETEConfigInfo eteConfig)
        {
            string database_name = eteConfig.database_name;
            string model_name = eteConfig.model_name;
            string group_name = eteConfig.group_name;
            string condition = eteConfig.condition;
            string condition_value = eteConfig.condition_value;
            string emp_pass = eteConfig.emp_pass;
            string reason = eteConfig.reason;
            string solution = eteConfig.solution;
            string ip_address = GetIP();

            string queryString = "select * from SFIS1.C_PRIVILEGE where emp in (select distinct emp_no from sfis1.C_EMP_DESC_T where emp_pass= '" + emp_pass + "' and rownum =1) and fun = 'ETE CONFIG_UNLOCK' and prg_name = 'CONFIG'";
            DataTable dtCheck1 = DBConnect.GetData(queryString, database_name);
            if (dtCheck1.Rows.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });


            string formatUpdate = "update sfis1.c_ete_config_t set status='' , create_time=sysdate where model_name= '{1}'  and type='{2}' and group_name = '{3}'";
            string queryUpdate = string.Format(formatUpdate, condition_value, model_name, condition, group_name);
            DBConnect.ExecuteNoneQuery(queryUpdate, database_name);

            string queryInserLog = "Insert into SFISM4.R_SYSTEM_LOG_T(EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC,TIME) Values (''||(SELECT EMP_NO FROM SFIS1.C_EMP_DESC_T where EMP_PASS = '" + emp_pass + "' and rownum =1)||'','CONFIG','UNLOCK','ETE Config - MODEL NAME : " + model_name + "; GROUP NAME : " + group_name + "; " + solution + ";MAC ADDRESS﹕null;IP:" + ip_address + ";" + reason + ";',sysdate)";

            DBConnect.ExecuteNoneQuery(queryInserLog, database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
        }

        [System.Web.Http.Route("LockDelete")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> LockDelete(ETEConfigInfo eteConfig)
        {
            string database_name = eteConfig.database_name;
            string model_name = eteConfig.model_name;
            string group_name = eteConfig.group_name;
            string condition = eteConfig.condition;
            string emp_pass = eteConfig.emp_pass;
            string ip_address = GetIP();

            string queryString = "select * from SFIS1.C_PRIVILEGE where emp in (select distinct emp_no from sfis1.C_EMP_DESC_T where emp_pass= '" + emp_pass + "' and rownum =1) and fun = 'ETE CONFIG_DELETE' and prg_name = 'CONFIG'";
            DataTable dtCheck1 = DBConnect.GetData(queryString, database_name);
            if (dtCheck1.Rows.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });


            string formatDelete = "Delete sfis1.c_ete_config_t where type='{0}' and model_name='{1}' and group_name= '{2}'";
            string queryDelete = string.Format(formatDelete, condition, model_name, group_name);

            DBConnect.ExecuteNoneQuery(queryDelete, database_name);

            string queryInserLog = "Insert into SFISM4.R_SYSTEM_LOG_T(EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC,TIME) Values (''||(SELECT EMP_NO FROM SFIS1.C_EMP_DESC_T where EMP_PASS = '" + emp_pass + "' and rownum =1)||'','CONFIG','DELETE','ETE Config - MODEL NAME : " + model_name + "; GROUP NAME : " + group_name + "; " + condition + ";MAC ADDRESS﹕null;IP:" + ip_address + "',sysdate)";

            DBConnect.ExecuteNoneQuery(queryInserLog, database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
        }


        [System.Web.Http.Route("LockInsert")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> LockInsert(ETEConfigInfo eteConfig)
        {
            string database_name = eteConfig.database_name;
            string model_name = eteConfig.model_name;
            string group_name = eteConfig.group_name;
            string condition = eteConfig.condition;
            string condition_value = eteConfig.condition_value;
            string emp_pass = eteConfig.emp_pass;
            string ip_address = GetIP();

            string queryString = "select * from SFIS1.C_PRIVILEGE where emp in (select distinct emp_no from sfis1.C_EMP_DESC_T where emp_pass= '" + emp_pass + "' and rownum =1) and fun = 'ETE CONFIG_ADD' and prg_name = 'CONFIG'";
            DataTable dtCheck1 = DBConnect.GetData(queryString, database_name);
            if (dtCheck1.Rows.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });


            string queryCheck = "select * from sfis1.c_ete_config_t where type='" + condition + "' and model_name= '" + model_name + "' and group_name= '" + group_name + "'";
            DataTable dtCheck = new DataTable();
            dtCheck = DBConnect.GetData(queryCheck, database_name);
            if (dtCheck.Rows.Count != 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "exist" });
            }
            string queryInsertFormat = "Insert into sfis1.c_ete_config_t(model_name,group_name,type,condition,create_time) VALUES('{0}','{1}','{2}','{3}',sysdate)";

            string queryInsert = string.Format(queryInsertFormat, model_name, group_name, condition, condition_value);

            DBConnect.ExecuteNoneQuery(queryInsert, database_name);

            string queryInserLog = "Insert into SFISM4.R_SYSTEM_LOG_T(EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC,TIME) Values (''||(SELECT EMP_NO FROM SFIS1.C_EMP_DESC_T where EMP_PASS = '" + emp_pass + "' and rownum =1)||'','CONFIG','INSERT','ETE Config - MODEL NAME : " + model_name + "; GROUP NAME : " + group_name + "; " + condition + ":" + condition_value + ";MAC ADDRESS﹕null;IP:" + ip_address + "',sysdate)";

            DBConnect.ExecuteNoneQuery(queryInserLog, database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
        }

        [System.Web.Http.Route("GetLockModel")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetLockModel(LockElement model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"select distinct MODEL_NAME VALUE from SFIS1.C_MODEL_DESC_T WHERE MODEL_NAME LIKE '%{model.value.Trim().ToUpper()}%'");
            string queryString = sb.ToString();
            DataTable dtCheck = DBConnect.GetData(queryString, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }


        [System.Web.Http.Route("GetLockElement")]
        [System.Web.Http.HttpPost]
        // GET: Lock
        public async Task<HttpResponseMessage> GetLockElement(LockElement model)
        {
            StringBuilder builder = new StringBuilder();

            if (model.type == "model")
            {
                StringBuilder modelSb = new StringBuilder();
                modelSb.Append("SELECT DISTINCT A.MODEL_NAME VALUE FROM SFIS1.C_ETE_CONFIG_T A WHERE 1=1 ");

                if (model.customer == "netgear")
                {
                    modelSb.Append(" AND A.MODEL_NAME IN (select distinct model_name B from SFIS1.C_MODEL_DESC_T B where B.model_SERIAL in ('Netgear','NETGEAR')) ");
                }
                else if (model.customer == "arlo")
                {
                    modelSb.Append(" AND A.MODEL_NAME IN (select distinct model_name B from SFIS1.C_MODEL_DESC_T B where B.model_SERIAL in ('ARLO')) ");
                }
                else if (model.customer == "eero")
                {
                    modelSb.Append(" AND A.MODEL_NAME IN (select distinct model_name B from SFIS1.C_MODEL_DESC_T B where B.MODEL_NAME LIKE'810%' OR B.MODEL_NAME LIKE'830%') ");
                }
                else if (model.customer == "PPLES")
                {
                    modelSb.Append(" AND  A.type ='PPLES' ");
                }

                if (model.listGroup.Count > 0)
                {
                    StringBuilder groupBuilder = new StringBuilder();
                    StringBuilder contentBuilder = new StringBuilder();
                    for (int i = 0; i < model.listGroup.Count; i++)
                    {
                        contentBuilder.AppendFormat("'{0}'", model.listGroup[i].VALUE);
                        if (i < model.listGroup.Count - 1)
                        {
                            contentBuilder.Append(",");
                        }
                    }
                    groupBuilder.Append("AND A.GROUP_NAME IN (");
                    groupBuilder.Append(contentBuilder.ToString());
                    groupBuilder.Append(")");
                    modelSb.Append(groupBuilder);
                }
                builder.Append(modelSb);
                builder.Append(" ORDER BY A.MODEL_NAME");
            }
            else if (model.type == "group")
            {
                StringBuilder groupSb = new StringBuilder();
                groupSb.Append("SELECT DISTINCT A.GROUP_NAME VALUE FROM SFIS1.C_ETE_CONFIG_T A WHERE 1=1 ");
                if (model.customer == "netgear")
                {
                    groupSb.Append(" AND A.MODEL_NAME IN (select distinct model_name B from SFIS1.C_MODEL_DESC_T B where B.model_SERIAL in ('Netgear','NETGEAR')) ");
                }
                else if (model.customer == "arlo")
                {
                    groupSb.Append(" AND A.MODEL_NAME IN (select distinct model_name B from SFIS1.C_MODEL_DESC_T B where B.model_SERIAL in ('ARLO')) ");
                }
                else if(model.customer == "eero")
                {
                    groupSb.Append(" AND A.MODEL_NAME IN (select distinct model_name B from SFIS1.C_MODEL_DESC_T B where B.MODEL_NAME LIKE'810%' OR B.MODEL_NAME LIKE'830%') ");
                }
                else if (model.customer == "PPLES")
                {
                    groupSb.Append(" AND  A.type ='PPLES' ");
                }
                if (model.listModel.Count > 0)
                {
                    StringBuilder groupBuilder = new StringBuilder();
                    StringBuilder contentBuilder = new StringBuilder();
                    for (int i = 0; i < model.listModel.Count; i++)
                    {
                        contentBuilder.AppendFormat("'{0}'", model.listModel[i].VALUE);
                        if (i < model.listModel.Count - 1)
                        {
                            contentBuilder.Append(",");
                        }
                    }
                    groupBuilder.Append("AND A.MODEL_NAME IN (");
                    groupBuilder.Append(contentBuilder.ToString());
                    groupBuilder.Append(")");
                    groupSb.Append(groupBuilder);
                }
                builder.Append(groupSb);
                builder.Append(" ORDER BY A.GROUP_NAME");
            }

            string queryString = builder.ToString();
            DataTable dtCheck = DBConnect.GetData(queryString, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }
        [System.Web.Http.Route("GetLockDetail")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetLockDetail(LockElement model)
        {

            var sb = new StringBuilder();

            if (model.listGroup.Count > 0)
            {
                StringBuilder groupBuilder = new StringBuilder();
                StringBuilder contentBuilder = new StringBuilder();
                for (int i = 0; i < model.listGroup.Count; i++)
                {
                    contentBuilder.AppendFormat("'{0}'", model.listGroup[i].VALUE);
                    if (i < model.listGroup.Count - 1)
                    {
                        contentBuilder.Append(",");
                    }
                }
                groupBuilder.Append("AND GROUP_NAME IN (");
                groupBuilder.Append(contentBuilder.ToString());
                groupBuilder.Append(")");
                sb.Append(groupBuilder);
            }
            if (model.listModel.Count > 0)
            {
                StringBuilder groupBuilder = new StringBuilder();
                StringBuilder contentBuilder = new StringBuilder();
                for (int i = 0; i < model.listModel.Count; i++)
                {
                    contentBuilder.AppendFormat("'{0}'", model.listModel[i].VALUE);
                    if (i < model.listModel.Count - 1)
                    {
                        contentBuilder.Append(",");
                    }
                }
                groupBuilder.Append("AND MODEL_NAME IN (");
                groupBuilder.Append(contentBuilder.ToString());
                groupBuilder.Append(")");
                sb.Append(groupBuilder);
            }
            if (model.type == "LOCK")
            {
                sb.Append(" and TO_CHAR(status) = 'LOCK'");
            }
            else
            {
                sb.Append(" and status is null ");
            }
            
            StringBuilder subBuild = new StringBuilder();
            string subQuery = "select A.MODEL_NAME,A.GROUP_NAME,A.TYPE,A.CONDITION,TO_CHAR(A.CURRENT_DATA) CURRENT_DATA,TO_CHAR(A.ERROR_CODE) ERROR_CODE,TO_CHAR(A.STATUS) STATUS from SFIS1.C_ETE_CONFIG_T A WHERE 1=1 ";

            subBuild.Append(subQuery);
            if (model.customer == "netgear")
            {
                subBuild.Append(" AND A.MODEL_NAME IN (select distinct model_name B from SFIS1.C_MODEL_DESC_T B where B.model_SERIAL in ('Netgear','NETGEAR')) ");
            }
            else if (model.customer == "arlo")
            {
                subBuild.Append(" AND A.MODEL_NAME IN (select distinct model_name B from SFIS1.C_MODEL_DESC_T B where B.model_SERIAL in ('ARLO')) ");
            }
            else if (model.customer == "PPLES")
            {
                subBuild.Append(" AND  A.type ='PPLES' ");
            }
            else if(model.customer == "eero")
            {
                subBuild.Append(" AND A.MODEL_NAME IN (select distinct model_name B from SFIS1.C_MODEL_DESC_T B where B.MODEL_NAME LIKE'810%' OR B.MODEL_NAME LIKE'830%') ");
            }
            
            sb.Insert(0, subBuild.ToString());
            string queryString = sb.ToString();
            DataTable dtCheck = DBConnect.GetData(queryString, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }

        }
        [System.Web.Http.Route("LockHistory")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> LockHistory(HistoryLog historyLog)
        {
            string database_name = historyLog.database_name;
            string model_name = historyLog.model_name;
            string group_name = historyLog.group_name;
            string action_type = historyLog.action_type;
            string start_time = historyLog.start_time;
            string end_time = historyLog.end_time;
            DataTable dt = new DataTable();

            var sb = new StringBuilder();
            sb.AppendFormat("and action_desc like 'ETE Config%{0}%{1}%'", model_name, group_name);

            if (!string.IsNullOrEmpty(action_type) && action_type != "ALL")
            {
                sb.AppendFormat(" AND action_type ='{0}'", action_type);
            }
            if (!string.IsNullOrEmpty(start_time) && !string.IsNullOrEmpty(end_time))
            {
                sb.AppendFormat(" AND (TIME >=TO_DATE('{0}','YYYY/MM/DD HH24:MI') AND TIME <= TO_DATE('{1}','YYYY/MM/DD HH24:MI'))", start_time, end_time);
            }
            if (sb.Length > 0)
            {
                var selectCause = "";
                if (action_type == "CONFIRM_UNLOCK")
                {
                    selectCause = "SELECT A.EMP_NO, " +
       "       B.EMP_NAME, " +
       "       A.ACTION_TYPE, " +
       "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                          FROM DUAL " +
       "                                         WHERE LEVEL = 1 " +
       "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                                  IS NOT NULL), " +
       "                                  '[^:]+', " +
       "                                  1, " +
       "                                  LEVEL) " +
       "              FROM DUAL " +
       "             WHERE LEVEL = 2 " +
       "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                          FROM DUAL " +
       "                                         WHERE LEVEL = 1 " +
       "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                                  IS NOT NULL), " +
       "                                  '[^:]+', " +
       "                                  1, " +
       "                                  LEVEL) " +
       "                      IS NOT NULL) " +
       "          MODEL_NAME, " +
       "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                          FROM DUAL " +
       "                                         WHERE LEVEL = 2 " +
       "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                                  IS NOT NULL), " +
       "                                  '[^:]+', " +
       "                                  1, " +
       "                                  LEVEL) " +
       "              FROM DUAL " +
       "             WHERE LEVEL = 2 " +
       "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                          FROM DUAL " +
       "                                         WHERE LEVEL = 1 " +
       "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                                  IS NOT NULL), " +
       "                                  '[^:]+', " +
       "                                  1, " +
       "                                  LEVEL) " +
       "                      IS NOT NULL) " +
       "          GROUP_NAME, " +
       "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                          FROM DUAL " +
       "                                         WHERE LEVEL = 5 " +
       "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                                  IS NOT NULL), " +
       "                                  '[^:]+', " +
       "                                  1, " +
       "                                  LEVEL) " +
       "              FROM DUAL " +
       "             WHERE LEVEL = 2 " +
       "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                          FROM DUAL " +
       "                                         WHERE LEVEL = 1 " +
       "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                                  IS NOT NULL), " +
       "                                  '[^:]+', " +
       "                                  1, " +
       "                                  LEVEL) " +
       "                      IS NOT NULL) " +
       "          IP, " +
       "       A.TIME " +
       "  FROM sfism4.R_SYSTEM_LOG_T A, SFIS1.C_EMP_DESC_T B " +
       " WHERE     1 = 1 " +
       "       AND A.EMP_NO = B.EMP_NO ";
                }
                else if (action_type == "UNLOCK")
                {
                    selectCause = "  SELECT A.EMP_NO, " +
         "         B.EMP_NAME, " +
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
         "    FROM sfism4.R_SYSTEM_LOG_T A, SFIS1.C_EMP_DESC_T B " +
         "   WHERE     1 = 1 " +
         "         AND A.EMP_NO = B.EMP_NO ";
                }
                else if (action_type == "LOCK")
                {
                    selectCause = "SELECT A.EMP_NO, " +
        "       B.EMP_NAME, " +
        "       A.ACTION_TYPE, " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          MODEL_NAME, " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 2 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          GROUP_NAME, " +
        "       (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                  '[^;]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 3 " +
        "        CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                  '[^;]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          REASON, " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 5 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          IP, " +
        "       A.TIME " +
        "  FROM sfism4.R_SYSTEM_LOG_T A, SFIS1.C_EMP_DESC_T B " +
        " WHERE     1 = 1 " +
        "       AND A.EMP_NO = B.EMP_NO ";

                }
                else if (action_type == "DELETE")
                {
                    selectCause = "SELECT A.EMP_NO, " +
        "       B.EMP_NAME, " +
        "       A.ACTION_TYPE, " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          MODEL_NAME, " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 2 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          GROUP_NAME, " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 5 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          IP, " +
        "       A.TIME " +
        "  FROM sfism4.R_SYSTEM_LOG_T A, SFIS1.C_EMP_DESC_T B " +
        " WHERE     1 = 1 " +
        "       AND A.EMP_NO = B.EMP_NO ";

                }
                else
                {
                    //INSERT , UPDATE
                    selectCause = "SELECT A.EMP_NO, " +
        "       B.EMP_NAME, " +
        "       A.ACTION_TYPE, " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          MODEL_NAME, " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 2 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 2 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          GROUP_NAME,           " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 3 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 1 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 2 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          CONDITION, " +
        "          (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 3 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 2 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          VALUE " +
        "          ,        " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 5 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          IP, " +
        "       A.TIME " +
        "  FROM sfism4.R_SYSTEM_LOG_T A, SFIS1.C_EMP_DESC_T B " +
        " WHERE     1 = 1 " +
        "       AND A.EMP_NO = B.EMP_NO ";
                }


                sb.Insert(0, selectCause);
                var queryString = sb.ToString() + " order by A.TIME DESC";
                dt = DBConnect.GetData(queryString, database_name);


            }
            return Request.CreateResponse(HttpStatusCode.OK, new { data = dt });
        }
    }
}