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

namespace SN_API.Controllers.Config
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class Config60Controller : ApiController
    {

        // get content 

        [System.Web.Http.Route("GetConfig60Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig60Content(Config60Element model)
        {
            // check GWCPEII_CONFIG 

            string strGetData = "";
            if (string.IsNullOrEmpty(model.SHIP_INDEX))
            {
                strGetData = $"  select SHIP_INDEX, SHIP_ADDRESS, SHIP_CODE,EMP_NO, EDIT_TIME  from sfis1.C_SHIP_ADDR_T where rownum <=20 ";
            }
            else
            {
                strGetData = $" select SHIP_INDEX, SHIP_ADDRESS, SHIP_CODE,EMP_NO, EDIT_TIME  from sfis1.C_SHIP_ADDR_T WHERE  SHIP_INDEX LIKE '%{model.SHIP_INDEX.ToUpper()}%' or SHIP_ADDRESS LIKE '%{model.SHIP_INDEX.ToUpper()}%'  or SHIP_code LIKE '%{model.SHIP_INDEX.ToUpper()}%' ";
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

        //ineert udate config45

        [System.Web.Http.Route("InsertUpdate60")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdate60(Config60Element model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                string actionString = " ";
                //check exist

                if (model.ACTION_TYPE == "INSERT")
                {
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'SHIP_ADDR_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    sb.Append($" Begin  SFIS1.P_SHIP_ADDR_CF60('{model.SHIP_INDEX}', '{model.SHIP_ADDRESS}','{model.SHIP_CODE}','{model.EMP}','INSERT'); end;");

                    actionString = "INSERT";
                }

                else
                {
                    if (model.ACTION_TYPE == "UPDATE")
                    {
                        //check privilege
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'SHIP_ADDR_EDIT' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege_c" });
                        }

                        //existed => update
                        actionString = "UPDATE";

                        sb.Append($" Begin  SFIS1.P_SHIP_ADDR_CF60( '{model.SHIP_INDEX}','{model.SHIP_ADDRESS}','{model.SHIP_CODE}','{model.EMP}','UPDATE'); end;");
                    }
                    else
                    {
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'SHIP_ADDR_DELETE' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        //existed => update
                        actionString = "DELETE";

                        sb.Append($" Begin  SFIS1.P_SHIP_ADDR_CF60( '{model.SHIP_INDEX}','{model.SHIP_ADDRESS}','{model.SHIP_CODE}','{model.EMP}','DELETE'); end;");
                    }


                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config60  SHIP_INDEX:  {model.SHIP_INDEX},SHI_ADDR: {model.SHIP_ADDRESS}, SHIP_CODE:{model.SHIP_CODE} ; IP:{AuthorizationController.UserIP()} ;' ");
                sbLog.Append(" ) ");
                string strInsertLog = sbLog.ToString();
                string strInserUpdate = sb.ToString();
                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config 60
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);                                                            // DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }



        }

    }
}

