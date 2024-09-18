using SN_API.Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using SN_API.Models;
using System.Data;
using System.Text;

namespace SN_API.Controllers.Config
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CheckRouteController : ApiController
    {
        [System.Web.Http.Route("GetRoute_NameContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetRoute_NameContent(CheckRouteElement model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.ROUTE_NAME))
            {
                strGetData = $" select A.*,ROWIDTOCHAR(ROWID) ID from SFIS1.C_ROUTE_NAME_T A WHERE ROWNUM < 30 ";
            }
            else
            {
                strGetData = $" select A.*,ROWIDTOCHAR(ROWID) ID from SFIS1.C_ROUTE_NAME_T A WHERE UPPER(A.ROUTE_NAME) LIKE '%{model.ROUTE_NAME.ToUpper()}%' ";
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
        [System.Web.Http.Route("GetRouteContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetRouteContent(CheckRouteElement model)
        {
            string strGetData = "";
            string strGetData1 = "";

            strGetData = $" SELECT mo_number,model_name,mo_type,route_code,route_name  FROM table(SFIS1.Z_PKG.get_route('{model.MO_NUMBER}')) order by route_index ";

            strGetData1 = $" SELECT *  FROM table(SFIS1.Z_PKG.get_bomno('{model.MO_NUMBER}')) order by BOM_INDEX ";

            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            DataTable dtCheck1 = DBConnect.GetData(strGetData1, model.database_name);

            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, new { result = "ok", data = dtCheck, data1 = dtCheck1 });
            }
        }
        [System.Web.Http.Route("InsertUpdateRoute_Description")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateRoute_Description(CheckRouteElement model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                string modify = " ";
                //check exist
                string strCheckExist = $"  select * from SFIS1.C_ROUTE_NAME_T where ROUTE_CODE = '{model.ROUTE_CODE}' AND ROUTE_NAME = '{model.ROUTE_NAME}' ";
                string actionString = " ";
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "" });
                }
                else
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'ROUTE CONTROL' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" UPDATE SFIS1.C_ROUTE_NAME_T ");
                    sb.Append(" SET ");
                    sb.Append($" ROUTE_DESC = '{model.ROUTE_DESC}' "); //MODEL_CODE
                    sb.Append($" WHERE ROWID = '{model.ID}' "); //ID

                    modify = " UPDATE: ";
                    string query = $"select ROUTE_DESC from SFIS1.C_ROUTE_NAME_T WHERE ROWID = '{model.ID}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);
                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if (row[0].ToString() != model.ROUTE_DESC)
                        {
                            modify += $" ROUTE_DESC: {row[0].ToString()};";
                        }
                    }

                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CHECKROUTE', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'CHECKROUTE ROUTE_NAME: {model.ROUTE_NAME}; ROUTE_CODE: {model.ROUTE_CODE};{modify}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_ROUTE_NAME_T' ");
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
    }
}