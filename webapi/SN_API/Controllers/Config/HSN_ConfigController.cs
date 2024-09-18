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
    public class HSN_ConfigController : ApiController
    {
        // GET: ConfigModel_Attr
        [System.Web.Http.Route("getHSN_Config")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> getHSN_Config(HSN_ConfigElement model)
        {
            string strGetData = "";
            if (!string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = String.Format("select  TYPE_VALUE as MODEL_NAME,  ATTRIBUTE_VALUE AS KP_HSN, EMP_NO, INPUT_TIME AS TIME, ROWID ID" +
                    "  from SFIS1.C_MODEL_ATTR_CONFIG_T where ATTRIBUTE_NAME='HSN_CONFIG' " +
                    " and TYPE_VALUE like '%{0}%' ", model.MODEL_NAME.ToUpper());
            }
            else
            {
                strGetData = String.Format("select TYPE_VALUE as MODEL_NAME,  ATTRIBUTE_VALUE AS KP_HSN, EMP_NO, INPUT_TIME AS TIME, ROWID ID" +
                    "  from SFIS1.C_MODEL_ATTR_CONFIG_T where ATTRIBUTE_NAME='HSN_CONFIG' ");
            }

            string strgroup_name = $"select sfis1.z_pkg.get_group_allparts_material() from dual";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            DataTable dtCheck1 = DBConnect.GetData(strgroup_name, model.database_name);

            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail", data1 = dtCheck1 });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, data1 = dtCheck1 });
            }
        }

        [System.Web.Http.Route("InsertUpdateHSN_Config")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateHSN_Config(HSN_ConfigElement model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                string modify = " ";
                //check exist
                string strCheckExist = $"  select * from SFIS1.C_MODEL_ATTR_CONFIG_T where  ROWID = '{model.ID}' ";
                string actionString = " ";
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'HSN_CONFIG' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //not exist => insert
                    actionString = "INSERT";
                    sb.Append("INSERT into");
                    sb.Append($" SFIS1.C_MODEL_ATTR_CONFIG_T(TYPE_NAME,TYPE_VALUE,ATTRIBUTE_NAME,");
                    sb.Append($"ATTRIBUTE_VALUE,EMP_NO,INPUT_TIME)");
                    sb.Append($" VALUES(");
                    sb.Append($" 'MODEL_NAME','{model.MODEL_NAME}','HSN_CONFIG',");
                    sb.Append($" '{model.GROUP_NAME}','{model.EMP}',SYSDATE");
                    sb.Append($" ) ");
                }
                else
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'HSN_CONFIG' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" UPDATE SFIS1.C_MODEL_ATTR_CONFIG_T ");
                    sb.Append(" SET ");
                    sb.Append($" TYPE_VALUE = '{model.MODEL_NAME}' ,"); //MODEL_CODE
                    sb.Append($" ATTRIBUTE_VALUE = '{model.GROUP_NAME}', ");
                    sb.Append($" WHERE ROWID = '{model.ID}' "); //ID

                    modify = " UPDATE: ";
                    string query = $"select ATTRIBUTE_VALUE from SFIS1.C_MODEL_ATTR_CONFIG_T WHERE ROWID = '{model.ID}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);
                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if (row[0].ToString() != model.GROUP_NAME)
                        {
                            modify += $" ATTRIBUTE_VALUE: {row[0].ToString()};";
                        }
                    }

                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'HSN_CONFIG TYPE_VALUE: {model.MODEL_NAME}; {modify}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_ATTR_CONFIG_T' ");
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

        [System.Web.Http.Route("DeleteHSN_Config")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteHSN_Config(HSN_ConfigElement model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'HSN_CONFIG' AND EMP='{model.EMP}'";
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
                sbLog.Append($"  'HSN_CONFIG TYPE_VALUE: {model.MODEL_NAME}; ATTRIBUTE_VALUE: {model.GROUP_NAME}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_ATTR_CONFIG_T' ");
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

        [System.Web.Http.Route("HSN_CONFIGGetAllModel")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> HSN_CONFIGGetAllModel(Config19Element model)
        {
            string strGetData = "select MODEL_NAME from SFIS1.C_MODEL_DESC_T " +
                    " MINUS " +
                    " select TYPE_VALUE MODEL_NAME from SFIS1.C_MODEL_ATTR_CONFIG_T where ATTRIBUTE_NAME='HSN_CONFIG'";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }
    }
}