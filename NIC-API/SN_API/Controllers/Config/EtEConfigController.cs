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
    public class EtEConfigController : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetEtEConfigContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetEtEConfigContent(EtEConfigElement model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $" select A.MODEL_NAME,A.GROUP_NAME,A.TYPE,A.CONDITION,A.CREATE_TIME,A.STATUS,A.CURRENT_DATA,A.ERROR_CODE,A.QTY,A.DATA, ROWIDTOCHAR(ROWID)" 
                + $" from sfis1.c_ete_config_t A where A.model_name in(select model_name from SFIS1.C_MODEL_DESC_T) and rownum < 20 order by A.model_name,A.group_name,A.type";                
            }
            else
            {
                strGetData = $" select A.MODEL_NAME,A.GROUP_NAME,A.TYPE,A.CONDITION,A.CREATE_TIME,A.STATUS,A.CURRENT_DATA,A.ERROR_CODE,A.QTY,A.DATA, ROWIDTOCHAR(ROWID)" 
                + $" from sfis1.c_ete_config_t A where upper(A.model_name)='{ model.MODEL_NAME.ToUpper() }' order by A.model_name,A.group_name,A.type";
            }
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" , data = dtCheck});
            }
        }


        [System.Web.Http.Route("DeleteEtEConfig")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteEteConfig(EtEConfigElement model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'ETE CONFIG_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege,model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }

            string strDelete = $" delete from SFIS1.C_ETE_CONFIG_T where ROWID = '{model.ID}'";

            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);

                //insert log
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($" 'EtEConfig MODEL_NAME: {model.MODEL_NAME};GROUP_NAME: {model.GROUP_NAME};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_ETE_CONFIG_T'");
                sbLog.Append(" ) ");

                string strInsertLog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }


        [System.Web.Http.Route("InsertUpdateEtEConfig")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateEtEConfig(EtEConfigElement model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();

                //check exist
                string strCheckExist = $"  select MODEL_NAME from sfis1.c_ete_config_t where model_name = '{model.MODEL_NAME}' and group_name {model.GROUP_NAME}";
                string actionString = " ";
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    string strPrivilege = $" SELECT * FROM sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG' AND FUN='ETE CONFIG_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //not exist => insert
                    actionString = "INSERT";
                    sb.Append(" INSERT into");
                    sb.Append($" SFIS1.C_ETE_CONFIG_T");
                    sb.Append($" (MODEL_NAME,GROUP_NAME,TYPE,CONDITION,CREATE_TIME,STATUS,CURRENT_DATA,ERROR_CODE,QTY,DATA)");
                    sb.Append($" VALUES(");
                    sb.Append($"'{model.MODEL_NAME}','{model.GROUP_NAME}','{model.TYPE}','{model.CONDITION}','{model.CREATE_TIME}')");
                }
                else
                {
                    //check privilege
                    string strPrivilege = $" SELECT * FROM sfis1.C_PRIVILEGE where PRG_NAME='CONFIG' AND FUN='ETE CONFIG_EDIT' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" UPDATE ");
                    sb.Append($" SFIS1.C_ETE_CONFIG_T");
                    sb.Append($" SET");
                    sb.Append($" CONDITION = '{model.CONDITION}',");
                    sb.Append($" create_time = sysdate");
                    sb.Append($" where");
                    sb.Append($" MODEL_NAME = '{model.MODEL_NAME}'");
                    sb.Append($" AND TYPE = '{model.TYPE}'");
                    sb.Append($" AND GROUP_NAME = '{model.GROUP_NAME}'");
                }

                //insert log
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($" 'EtEConfig MODEL_NAME: {model.MODEL_NAME};GROUP_NAME: {model.GROUP_NAME};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_ETE_CONFIG_T'");
                sbLog.Append(" ) ");

                string strInsertUpdate = sb.ToString();
                string strInsertLog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strInsertUpdate, model.database_name); //Execute insert update EtEconfig
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name); //Execute insert log
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }            
        }


        [System.Web.Http.Route("Lock&Unlock Model")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> LockEtEConfig(EtEConfigElement model)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbLog = new StringBuilder();
            string actionString = "UPDATE";
            string strCheckExist = $" select MODEL_NAME from sfis1.c_ete_config_t where model_name = '{model.MODEL_NAME}' and group_name = {model.GROUP_NAME} and status = 'LOCK'";
            if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
            {
                //check privilege
                string strPrivilege = $" SELECT * FROM sfis1.C_PRIVILEGE where PRG_NAME='CONFIG' AND FUN='ETE CONFIG_LOCK' AND EMP='{model.EMP}'";
                if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                }
                //lock
                sb.Append(" UPDATE ");
                sb.Append($" SFIS1.C_ETE_CONFIG_T");
                sb.Append($" SET");
                sb.Append($" STATUS = 'LOCK'");
                sb.Append($" where");
                sb.Append($" MODEL_NAME = '{model.MODEL_NAME}'");
                sb.Append($" AND TYPE = '{model.TYPE}'");
                sb.Append($" AND GROUP_NAME = '{model.GROUP_NAME}'");
            }
            else
            {
                //check privilege
                string strPrivilege = $" SELECT * FROM sfis1.C_PRIVILEGE where PRG_NAME='CONFIG' AND FUN='ETE CONFIG_UNLOCK' AND EMP='{model.EMP}'";
                if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                }
                //unlock
                sb.Append(" UPDATE ");
                sb.Append($" SFIS1.C_ETE_CONFIG_T");
                sb.Append($" SET");
                sb.Append($" STATUS = ''");
                sb.Append($" where");
                sb.Append($" MODEL_NAME = '{model.MODEL_NAME}'");
                sb.Append($" AND TYPE = '{model.TYPE}'");
                sb.Append($" AND GROUP_NAME = '{model.GROUP_NAME}'");
            }

            //insert log
            sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
            sbLog.Append(" VALUES ( ");
            sbLog.Append($" '{model.EMP}', ");
            sbLog.Append($" 'CONFIG', ");
            sbLog.Append($" '{actionString}', ");
            sbLog.Append($" 'EtEConfig LOCK&UNLOCK MODEL_NAME: {model.MODEL_NAME};GROUP_NAME: {model.GROUP_NAME}; STATUS : {model.STATUS};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_ETE_CONFIG_T'");
            sbLog.Append(" ) ");

            string strLock = sb.ToString();
            string strInsertLog = sbLog.ToString();
            DBConnect.ExecuteNoneQuery(strLock, model.database_name); //Execute lock&unlock model
            DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name); //Execute insert log
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
        }
    }
}