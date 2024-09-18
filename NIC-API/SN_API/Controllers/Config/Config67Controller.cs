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
    public class Config67Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig67Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig67Content(Config67Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = String.Format("SELECT PRG_NAME,VR_NAME MODEL_NAME,VR_VALUE WEIGHT,VR_DESC STANDARD_WEIGHT,ROWID ID FROM SFIS1.C_PARAMETER_INI WHERE PRG_NAME='CHECK_BOX_WEIGHT' " +
                    "AND VR_CLASS='Default' AND VR_ITEM = 'OFFSET' AND VR_NAME IN(SELECT MODEL_NAME FROM SFISM4.R105 WHERE CLOSE_FLAG = 2) order by VR_VALUE asc");
            }
            else
            {
                strGetData = String.Format("SELECT PRG_NAME,VR_NAME MODEL_NAME,VR_VALUE WEIGHT,VR_DESC STANDARD_WEIGHT,ROWID ID FROM SFIS1.C_PARAMETER_INI WHERE PRG_NAME='CHECK_BOX_WEIGHT' " +
                     "AND VR_CLASS='Default' AND VR_ITEM = 'OFFSET' AND VR_NAME LIKE '"+ model.MODEL_NAME+"%' " +
                     "AND VR_NAME IN(SELECT MODEL_NAME FROM SFISM4.R105 WHERE CLOSE_FLAG = 2) order by VR_VALUE asc");
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



        [System.Web.Http.Route("Config67_MODEL")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config67_MODEL(Config67Element model)
        {
            string strGetData = string.Format("select MODEL_NAME from SFIS1.C_MODEL_DESC_T minus " +
                            "SELECT vr_name MODEL_NAME FROM SFIS1.C_PARAMETER_INI WHERE PRG_NAME = 'CHECK_BOX_WEIGHT' AND VR_NAME IN(SELECT MODEL_NAME FROM SFISM4.R105 WHERE CLOSE_FLAG = 2)");

            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }



        [System.Web.Http.Route("DeleteConfig67")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig67(Config67Element model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'WEIGHT_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }

            string strDelete = $" delete from SFIS1.C_PARAMETER_INI where ROWID = '{model.ID}'";
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
                sbLog.Append($"  'Config67 MODEL_NAME: {model.MODEL_NAME};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_PARAMETER_INI' ");
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


        [System.Web.Http.Route("InsertUpdateConfig67")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig67(Config67Element model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string actionString = " ";
                string modify = " ";

                //check exist
                string strCheckexist = $" SELECT * FROM SFIS1.C_PARAMETER_INI WHERE PRG_NAME='CHECK_BOX_WEIGHT' AND VR_NAME = '{model.MODEL_NAME}'";
                if (DBConnect.GetData(strCheckexist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'WEIGHT_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //no exist => insert
                    actionString = "INSERT";
                    sb.Append("INSERT into");
                    sb.Append($" SFIS1.C_PARAMETER_INI");
                    sb.Append($" VALUES ");
                    sb.Append($" ('CHECK_BOX_WEIGHT','Default','OFFSET','{model.MODEL_NAME}','{model.WEIGHT}','{model.STANDARD_WEIGHT}') ");

                }
                else
                {
                    //check privilege
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'WEIGHT_EDIT' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //existed => update
                    actionString = "UPDATE";
                    sb.Append("UPDATE");
                    sb.Append($" SFIS1.C_PARAMETER_INI");
                    sb.Append($" SET ");
                    sb.Append($" VR_VALUE = '{model.WEIGHT}', VR_DESC = '{model.STANDARD_WEIGHT}' ");
                    sb.Append($" WHERE PRG_NAME='CHECK_BOX_WEIGHT' AND VR_NAME = '{model.MODEL_NAME}'");


                    modify = " UPDATE: ";
                    string query = $"select VR_NAME,VR_VALUE,VR_DESC  WHERE PRG_NAME='CHECK_BOX_WEIGHT' AND VR_NAME = '{model.MODEL_NAME}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);

                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if (row[0].ToString() != model.MODEL_NAME)
                        {
                            modify += $" VR_NAME: {row[0].ToString()} => {model.MODEL_NAME};";
                        }
                        if (row[1].ToString() != model.WEIGHT)
                        {
                            modify += $" VR_VALUE: {row[1].ToString()} => {model.WEIGHT};";
                        }
                        if (row[2].ToString() != model.STANDARD_WEIGHT)
                        {
                            modify += $" VR_DESC: {row[2].ToString()} => {model.STANDARD_WEIGHT};";
                        }
                    }
                }

                //insert log

                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config67 MODEL_NAME: {model.MODEL_NAME}; {modify};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_PARAMETER_INI' ");
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