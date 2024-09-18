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
    public class Allparts_StationController : ApiController
    {
        // GET: ConfigModel_Attr
        [System.Web.Http.Route("getConfigAllparts_Station")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> getConfigAllparts_Station(Allparts_StationElement model)
        {
            string strGetData = "";
            if (!string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = String.Format("select  TYPE_VALUE as MODEL_NAME,  ATTRIBUTE_VALUE AS GROUP_NAME, ATTRIBUTE_DESC1 AS DATA1, ATTRIBUTE_DESC2 AS DATA2, EMP_NO, INPUT_TIME AS TIME, ROWID ID" +
                    "  from SFIS1.C_MODEL_ATTR_CONFIG_T where ATTRIBUTE_NAME='NIC_CHECK_ALLPARTS' " +
                    " and TYPE_VALUE = '{0}' ", model.MODEL_NAME.ToUpper());
            }
            else
            {
                strGetData = String.Format("select  TYPE_VALUE as MODEL_NAME,   ATTRIBUTE_VALUE AS GROUP_NAME, ATTRIBUTE_DESC1 AS DATA1, ATTRIBUTE_DESC2 AS DATA2, EMP_NO, INPUT_TIME AS TIME, ROWID ID" +
                    "  from SFIS1.C_MODEL_ATTR_CONFIG_T where ATTRIBUTE_NAME='NIC_CHECK_ALLPARTS' ");
            }

            //string strgroup_name = $"select sfis1.z_pkg.get_group_allparts_material() from dual";
            string strgroup_name = "SELECT TRIM (REGEXP_SUBSTR (VALUE,'[^|]+',1,LEVEL)) VALUE FROM (select sfis1.z_pkg.get_group_allparts_material() as VALUE from dual) T CONNECT BY INSTR (VALUE,'|',1,LEVEL - 1) > 0 ORDER BY VALUE";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            DataTable dtCheck1 = DBConnect.GetData(strgroup_name, model.database_name);

            List<string> listGroupModel = new List<string>();
            string res1 = "";
            char separator1 = ',';
            if (dtCheck.Rows.Count == 1)
            {
                foreach (DataRow row in dtCheck.Rows)
                {
                    res1 = row[1].ToString();
                }
            }
            listGroupModel = res1.Split(separator1).ToList();
            string strGroup = $"select distinct GROUP_NAME as VALUE from SFIS1.C_EMP_2_GROUP_T where GROUP_NAME in (";
            foreach(var res in listGroupModel)
            {
                strGroup = strGroup + $"'{res.Trim()}',";
            }
            strGroup = strGroup.Substring(0, strGroup.Length - 1);
            strGroup = strGroup + ")";
            DataTable dtCheck2 = DBConnect.GetData(strGroup, model.database_name);

            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail", data1 = dtCheck1 });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck,data1 = dtCheck1,list = dtCheck1, list1 = dtCheck2 });
            }
        }

        [System.Web.Http.Route("ConfigAllpartsGetAllModel")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ConfigAllpartsGetAllModel(Config19Element model)
        {
            string strGetData = "select MODEL_NAME from SFIS1.C_MODEL_DESC_T " +
                    " MINUS " +
                    "select TYPE_VALUE MODEL_NAME from SFIS1.C_MODEL_ATTR_CONFIG_T where ATTRIBUTE_NAME='NIC_CHECK_ALLPARTS'";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }

        [System.Web.Http.Route("InsertUpdateConfigAllparts_Station")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfigAllparts_Station(Allparts_StationElement model)
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
                if (model.listGroup.Count() > 0)
                {
                    if (model.listGroup.Count > 0)
                    {
                        for (int i = 0; i < model.listGroup.Count; i++)
                        {
                            model.GROUP_NAME = model.GROUP_NAME + "," + model.listGroup[i].VALUE;
                        }
                    }
                    model.GROUP_NAME.Trim();
                    model.GROUP_NAME = model.GROUP_NAME.Substring(1);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "group" });
                }
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PQE_NIC' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //not exist => insert
                    actionString = "INSERT";
                    sb.Append("INSERT into");
                    sb.Append($" SFIS1.C_MODEL_ATTR_CONFIG_T(TYPE_NAME,TYPE_VALUE,ATTRIBUTE_NAME,");
                    sb.Append($"ATTRIBUTE_VALUE,ATTRIBUTE_DESC1,ATTRIBUTE_DESC2,EMP_NO,INPUT_TIME)");
                    sb.Append($" VALUES(");
                    sb.Append($" 'MODEL_NAME','{model.MODEL_NAME}','NIC_CHECK_ALLPARTS',");
                    sb.Append($" '{model.GROUP_NAME}','{model.DATA1}','{model.DATA2}','{model.EMP}',SYSDATE");
                    sb.Append($" ) ");
                }
                else
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PQE_NIC' AND EMP='{model.EMP}'";
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
                    sb.Append($" ATTRIBUTE_DESC1 = '{model.DATA1}', ");
                    sb.Append($" ATTRIBUTE_DESC2 = '{model.DATA2}' ");
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
                            //modify += $" VERSION: {row[0].ToString()};";
                        }
                        if (row[0].ToString() != model.GROUP_NAME)
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
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'NIC_CHECK_ALLPARTS TYPE_VALUE: {model.MODEL_NAME}; {modify}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_ATTR_CONFIG_T' ");
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

        [System.Web.Http.Route("DeleteAllparts_Station")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteAllparts_Station(Allparts_StationElement model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PQE_NIC' AND EMP='{model.EMP}'";
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
                sbLog.Append($"  'NIC_CHECK_ALLPARTS TYPE_VALUE: {model.MODEL_NAME}; ATTRIBUTE_VALUE: {model.GROUP_NAME}; ATTRIBUTE_DESC1: {model.DATA1};ATTRIBUTE_DESC2: {model.DATA2}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_ATTR_CONFIG_T' ");
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
    }
}