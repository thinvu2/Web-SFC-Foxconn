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
    public class Config10Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig10Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig10Content(Config10Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $" select A.MODEL_NAME,A.ITEM_CODE,A.ITEM_NAME,A.SIDE,A.ITEM_TYPE,A.MFR_CODE,A.MFR_NAME,ROWIDTOCHAR(ROWID) ID from SFIS1.C_ITEM_DESC_T A WHERE ROWNUM < 20 ";
            }
            else
            {
                strGetData = $" select A.MODEL_NAME,A.ITEM_CODE,A.ITEM_NAME,A.SIDE,A.ITEM_TYPE,A.MFR_CODE,A.MFR_NAME,ROWIDTOCHAR(ROWID) ID from SFIS1.C_ITEM_DESC_T A WHERE ROWNUM < 20 AND UPPER(A.MODEL_NAME) LIKE '%{model.MODEL_NAME.ToUpper()}%' ";
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
        [System.Web.Http.Route("InsertUpdateConfig10")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig10(Config10Element model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                string modify = " ";
                //check exist
                string strCheckExist = $"  select MODEL_NAME from SFIS1.C_ITEM_DESC_T where MODEL_NAME = '{model.MODEL_NAME}' AND ITEM_CODE = '{model.ITEM_CODE}' ";
                string actionString = " ";
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'LOCATION_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    // not exist => insert
                    sb.Append(" INSERT INTO SFIS1.C_ITEM_DESC_T ");
                    sb.Append($" ( MODEL_CODE,MODEL_NAME,ITEM_CODE,ITEM_SERIAL,ITEM_NAME,ITEM_TYPE,SIDE,MFR_CODE,MFR_NAME )  VALUES  ( '{model.MODEL_CODE}','{model.MODEL_NAME}','{model.ITEM_CODE}','{model.ITEM_SERIAL}','{model.ITEM_NAME}','{model.ITEM_TYPE}','{model.SIDE}','{model.MFR_CODE}','{model.MFR_NAME}' )  ");
                    actionString = "INSERT";
                }
                else
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'LOCATION_EDIT' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" UPDATE SFIS1.C_ITEM_DESC_T ");
                    sb.Append(" SET ");
                    sb.Append($" MODEL_CODE = '{model.MODEL_CODE}', "); //MODEL_CODE
                    sb.Append($" MODEL_NAME = '{model.MODEL_NAME}', "); //MODEL_NAME
                    sb.Append($" ITEM_CODE = '{model.ITEM_CODE}', "); //ITEM_CODE
                    sb.Append($" ITEM_SERIAL = '{model.ITEM_SERIAL}', "); //ITEM_SERIAL
                    sb.Append($" ITEM_NAME = '{model.ITEM_NAME}', "); //ITEM_NAME
                    sb.Append($" ITEM_TYPE = '{model.ITEM_TYPE}', "); //ITEM_TYPE
                    sb.Append($" SIDE = '{model.SIDE}', "); //SIDE
                    sb.Append($" MFR_CODE = '{model.MFR_CODE}', "); //MFR_CODE
                    sb.Append($" MFR_NAME = '{model.MFR_NAME}' "); //MFR_NAME
                    sb.Append($" WHERE ROWID = '{model.ID}' "); //ID

                    modify = " UPDATE: ";
                    string query = $"select MODEL_NAME,ITEM_CODE,ITEM_NAME,SIDE,ITEM_TYPE,MFR_CODE,MFR_NAME " +
                        $"from SFIS1.C_ITEM_DESC_T WHERE ROWID = '{model.ID}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);
                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if (row[0].ToString() != model.MODEL_NAME)
                        {
                            modify += $" MODEL_NAME: {row[0].ToString()};";
                        }
                        if (row[1].ToString() != model.ITEM_CODE)
                        {
                            modify += $" ITEM_CODE: {row[1].ToString()};";
                        }
                        if (row[2].ToString() != model.ITEM_NAME)
                        {
                            modify += $" ITEM_NAME: {row[2].ToString()} => {model.ITEM_NAME};";
                        }
                        if (row[3].ToString() != model.SIDE)
                        {
                            modify += $" SIDE: {row[3].ToString()} => {model.SIDE};";
                        }
                        if (row[4].ToString() != model.ITEM_TYPE)
                        {
                            modify += $" ITEM_TYPE: {row[4].ToString()} => {model.ITEM_TYPE};";
                        }
                        if (row[5].ToString() != model.MFR_CODE)
                        {
                            modify += $" MFR_CODE: {row[5].ToString()} => {model.MFR_CODE};";
                        }
                        if (row[6].ToString() != model.MFR_NAME)
                        {
                            modify += $" MFR_NAME: {row[6].ToString()}  => {model.MFR_NAME};";
                        }
                    }

                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config10 MODEL_NAME: {model.MODEL_NAME}; ITEM_CODE: {model.ITEM_CODE};{modify}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_ITEM_DESC_T' ");
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
        [System.Web.Http.Route("DeleteConfig10")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig10(Config10Element model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'LOCATION_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }
            string strDelete = $" delete SFIS1.C_ITEM_DESC_T where  ROWID = '{model.ID}' ";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'Config10 MODEL_NAME: {model.MODEL_NAME}; ITEM_CODE: {model.ITEM_CODE} IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_ITEM_DESC_T' ");
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