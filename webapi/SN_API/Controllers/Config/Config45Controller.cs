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

    public class Config45Controller : ApiController
    {

        // get content 

        [System.Web.Http.Route("GetConfig45Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig45Content(Config45Element model)
        {
            // check GWCPEII_CONFIG 

            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $"  select *  from sfis1.c_buffer_trigger_t where rownum <=20 ";
            }
            else
            {
                strGetData = $" select *  from sfis1.c_buffer_trigger_t WHERE   UPPER(MODEL_NAME) LIKE '%{model.MODEL_NAME.ToUpper()}%' ";
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
        // get all MODEL
        [System.Web.Http.Route("GetallModel_CF45")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetallModel_CF45(Config45Element model)
        {
            // check GWCPEII_CONFIG 

            string strGetData = "";

            strGetData = $" SELECT DISTINCT MODEL_NAME FROM SFIS1.C_MODEL_DESC_T  where  model_name not in (select model_name from sfis1.c_buffer_trigger_t) ";



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

        // get all PREGROUP
        [System.Web.Http.Route("GetallPreGroupCF45")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetallPreGroupCF45(Config45Element model)
        {
            // check GWCPEII_CONFIG 

            string strGetData = "";

            strGetData = $" SELECT DISTINCT PRE_GROUP FROM SFIS1.C_BUFFER_TRIGGER_T order by pre_group ";


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

        // get all NEXTGROUP
        [System.Web.Http.Route("GetallNextGroupCF45")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetallNextGroupCF45(Config45Element model)

        {
            // check GWCPEII_CONFIG 

            string strGetData = "";

            strGetData = $" SELECT DISTINCT NEXT_GROUP FROM SFIS1.C_BUFFER_TRIGGER_T order by NEXT_group ";

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
        // ineert udate config45

        [System.Web.Http.Route("InsertUpdateBufger_trigger")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateBufger_trigger(Config45Element model)
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
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'BUFFER_TRIGGER_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    sb.Append($" Begin  SFIS1.P_CRUD_C_BUFFER_TRIGGER( '{model.MODEL_NAME}','{model.VERSION_GROUP}','{model.PRE_GROUP}' ,'{model.NEXT_GROUP}','{model.BUFFER_QTY}','INSERT'); end;");

                    actionString = "INSERT";
                }

                else
                {
                    if (model.ACTION_TYPE == "UPDATE")
                    {
                        //check privilege
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'BUFFER_TRIGGER_EDIT' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege_c" });
                        }

                        //existed => update
                        actionString = "UPDATE";

                        sb.Append($" Begin  SFIS1.P_CRUD_C_BUFFER_TRIGGER(  '{model.MODEL_NAME}','{model.VERSION_GROUP}','{model.PRE_GROUP}' ,'{model.NEXT_GROUP}','{model.BUFFER_QTY}','UPDATE'); end;");
                    }
                    else
                    {
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'BUFFER_TRIGGER_DELETE' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        //existed => update
                        actionString = "DELETE";

                        sb.Append($" Begin  SFIS1.P_CRUD_C_BUFFER_TRIGGER(  '{model.MODEL_NAME}','{model.VERSION_GROUP}','{model.PRE_GROUP}' ,'{model.NEXT_GROUP}','{model.BUFFER_QTY}','DELETE'); end;");
                    }


                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config45  MODEL_NAME:  {model.MODEL_NAME},VERSIOn_GROUP: {model.VERSION_GROUP}, PRE_GROUP:{model.PRE_GROUP} ,NEXT_GROUP: {model.NEXT_GROUP},BUFER_QTY: {model.BUFFER_QTY}; IP:{AuthorizationController.UserIP()} ;' ");
                sbLog.Append(" ) ");
                string strInsertLog = sbLog.ToString();
                string strInserUpdate = sb.ToString();
                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config 6
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
