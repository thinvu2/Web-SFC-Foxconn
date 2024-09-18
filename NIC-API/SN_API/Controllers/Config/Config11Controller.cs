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
using static SN_API.Models.LineElement;

namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class Config11Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig11Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig11Content(Config11Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.KEY_PART_NO))
            {
                strGetData = $" select A.KEY_PART_NO,A.KP_NAME,A.KP_DESC, ROWIDTOCHAR(ROWID) ID from SFIS1.C_KEYPARTS_DESC_T A WHERE ROWNUM < 20 ";
            }
            else
            {
                strGetData = $" select A.KEY_PART_NO,A.KP_NAME,A.KP_DESC, ROWIDTOCHAR(ROWID) ID from SFIS1.C_KEYPARTS_DESC_T A WHERE UPPER(A.KEY_PART_NO) LIKE '%{model.KEY_PART_NO.ToUpper()}%' AND ROWNUM < 20 ";
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
        [System.Web.Http.Route("InsertUpdateConfig11")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig11(Config11Element model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                string modify = " ";
                //check exist
                string strCheckExist = $"  select KEY_PART_NO from SFIS1.C_KEYPARTS_DESC_T where KEY_PART_NO = '{model.KEY_PART_NO}' ";
                string actionString = " ";
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'KEYPART NO_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    // not exist => insert
                    sb.Append(" INSERT INTO SFIS1.C_KEYPARTS_DESC_T ");
                    sb.Append($" ( KEY_PART_NO,KP_NAME,KP_DESC )  VALUES  ( '{model.KEY_PART_NO}','{model.KP_NAME}','{model.KP_DESC}' )  ");
                    actionString = "INSERT";
                }
                else
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'KEYPART NO_EDIT' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" UPDATE SFIS1.C_KEYPARTS_DESC_T ");
                    sb.Append(" SET ");
                    sb.Append($" KEY_PART_NO = '{model.KEY_PART_NO}', "); //KEY_PART_NO
                    sb.Append($" KP_NAME = '{model.KP_NAME}', "); //KP_NAME
                    sb.Append($" KP_DESC = '{model.KP_DESC}' "); //KP_DESC
                    sb.Append($" WHERE KEY_PART_NO = '{model.KEY_PART_NO}' "); //ID

                    modify = " UPDATE: ";
                    string query = $"select A.KEY_PART_NO,A.KP_NAME,A.KP_DESC " +
                        $"from SFIS1.C_KEYPARTS_DESC_T A WHERE ROWID = '{model.ID}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);
                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if (row[0].ToString() != model.KEY_PART_NO)
                        {
                            modify += $" KEY_PART_NO: {row[0].ToString()} => {model.KEY_PART_NO};";
                        }
                        if (row[1].ToString() != model.KP_NAME)
                        {
                            modify += $" KP_NAME: {row[1].ToString()} => {model.KP_NAME};";
                        }
                        if (row[2].ToString() != model.KP_DESC)
                        {
                            modify += $" KP_DESC: {row[2].ToString()} => {model.KP_DESC};";
                        }
                    }
                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config11 KEY_PART_NO: {model.KEY_PART_NO}; {modify};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_KEYPARTS_DESC_T' ");
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

        

        [System.Web.Http.Route("Config11GetKeyPartName")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config11GetKeyPartName(Config19Element model)
        {
            string strGetData = " select distinct KP_NAME MODEL_NAME from SFIS1.C_KEYPARTS_DESC_T order by KP_NAME";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }


        [System.Web.Http.Route("DeleteConfig11")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig11(Config11Element model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'KEYPART NO_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }
            string strDelete = $" delete SFIS1.C_KEYPARTS_DESC_T where  ROWID = '{model.ID}' ";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'Config11 KEY_PART_NO: {model.KEY_PART_NO};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_KEYPARTS_DESC_T' ");
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