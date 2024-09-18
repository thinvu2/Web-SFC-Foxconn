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
    public class Config54Controller : ApiController
    {
        // get content 

        [System.Web.Http.Route("GetConfig54Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig54Content(Config54Element model)
        {
            // check GWCPEII_CONFIG 

            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $"  SELECT * FROM SFIS1.C_U54_WEIGHT_T  where   rownum <=20   ORDER BY MODEL_NAME,VERSION_CODE";
            }
            else
            {
                strGetData = $"  SELECT * FROM SFIS1.C_U54_WEIGHT_T   WHERE   UPPER(MODEL_NAME) LIKE '%{model.MODEL_NAME.ToUpper()}%' ";
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

        // get all VERSION CODE
        [System.Web.Http.Route("GetallVersionCodeCF54")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetallVersionCodeCF54(Config54Element model)

        {
            // check GWCPEII_CONFIG 

            string strGetData = "";

            strGetData = $" SELECT DISTINCT VERSION_CODE from SFIS1.C_CUST_SNRULE_T ORDER BY VERSION_CODE ";

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
        [System.Web.Http.Route("GetallModel_CF54")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetallModel_CF54(Config54Element model)
        {
            // check GWCPEII_CONFIG 

            string strGetData = "";

            strGetData = $" SELECT DISTINCT MODEL_NAME FROM SFIS1.C_MODEL_DESC_T  where  model_name not in (select model_name from sfis1.c_u54_weight_t) ";



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

        // ineert udate config54

        [System.Web.Http.Route("InsertUpdateCarton_prefix")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateCarton_prefix(Config54Element model)
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
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CARTON PREFIX_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    sb.Append($" Begin  SFIS1.P_CRUD_C_CARTON_PREFIX( '{model.MODEL_NAME}','{model.VERSION_CODE}','{model.CARTON_QTY}' ,'{model.CARTON_LABEL_WEIGHT}','{model.PRODUCT_DESC}','INSERT'); end;");

                    actionString = "INSERT";
                }

                else
                {
                    if (model.ACTION_TYPE == "UPDATE")
                    {
                        //check privilege
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CARTON PREFIX_EDIT' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        //existed => update
                        actionString = "UPDATE";

                        sb.Append($" Begin  SFIS1.P_CRUD_C_CARTON_PREFIX(  '{model.MODEL_NAME}','{model.VERSION_CODE}','{model.CARTON_QTY}' ,'{model.CARTON_LABEL_WEIGHT}','{model.PRODUCT_DESC}','UPDATE'); end;");
                    }
                    else
                    {
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CARTON PREFIX_DELETE' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        //existed => update
                        actionString = "DELETE";

                        sb.Append($" Begin  SFIS1.P_CRUD_C_CARTON_PREFIX( '{model.MODEL_NAME}','{model.VERSION_CODE}','{model.CARTON_QTY}' ,'{model.CARTON_LABEL_WEIGHT}','{model.PRODUCT_DESC}','DELETE'); end;");
                    }


                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config54  MODEL_NAME:  {model.MODEL_NAME},VERSIOn_GROUP: {model.VERSION_CODE}, CARTON QTY:{model.CARTON_QTY} ,CARTON_LABEL_WEIGHT: {model.CARTON_LABEL_WEIGHT},PRODUCT_DESC: {model.PRODUCT_DESC}; IP:{AuthorizationController.UserIP()} ;' ");
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
        // SET MODEL REWORK 

        // get all CONTENT
        [System.Web.Http.Route("GetallModelReworkContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetallModelReworkContent(SetupModelRework model)
        {
            // check GWCPEII_CONFIG 

            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $"   SELECT ROWID AS ID, MODEL_NAME, MODEL_DESC, GROUP_NAME,NEXT_GROUP, IN_STATION_TIME FROM SFIS1.C_MODEL_NOMMS_T  where model_desc like '%RETURN%' ";
            }
            else
            {
                strGetData = $" SELECT ROWID AS ID, MODEL_NAME, MODEL_DESC, GROUP_NAME,NEXT_GROUP, IN_STATION_TIME FROM SFIS1.C_MODEL_NOMMS_T  where   model_desc like '%RETURN%' and model_name like '%{model.MODEL_NAME.ToUpper()}%' ";
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
        [System.Web.Http.Route("ListModel_Rework")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ListModel_Rework(SetupModelRework model)
        {
            // check GWCPEII_CONFIG 

            string strGetData = "";

            strGetData = $" SELECT DISTINCT MODEL_NAME FROM SFIS1.C_MODEL_DESC_T  ";



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

        // get all GROUP
        [System.Web.Http.Route("GetallGroup_RW")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetallGroup_RW(SetupModelRework model)
        {
            // check GWCPEII_CONFIG 

            string strGetData = "";

            strGetData = $" SELECT DISTINCT GROUP_NAME FROM SFIS1.C_GROUP_CONFIG_T   ";



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

        // get MODEL_DESC 

        [System.Web.Http.Route("GetallModel_Desc_RW")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetallModel_Desc_RW(SetupModelRework model)
        {
            // check GWCPEII_CONFIG 

            string strGetData = "";

            strGetData = $" SELECT DISTINCT MODEL_DESC  FROM SFIS1.c_model_nomms_t where model_desc is not null   ";



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


        // ineert udate config54

        [System.Web.Http.Route("InsertUpdateModelRework")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateModelRework(SetupModelRework model)
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

                    string strCheckexist = $" select *  from SFIS1.c_model_nomms_t a where  upper(a.model_name) = '{model.MODEL_NAME.ToUpper()}' and upper(a.model_desc) = '{model.MODEL_DESC.ToUpper()}' and upper(a.GROUP_NAME) = '{model.FROM_GROUP.ToUpper()}'  and upper(a.NEXT_GROUP) = '{model.TO_GROUP.ToUpper()}'  ";
                    if (DBConnect.GetData(strCheckexist, model.database_name).Rows.Count > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "exists" });
                    }

                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CARTON PREFIX_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    sb.Append($" Begin  SFIS1.P_IUD_MODEL_REWORK( '{model.MODEL_NAME}','{model.MODEL_DESC}','{model.FROM_GROUP}' ,'{model.TO_GROUP}','{model.ID}','INSERT'); end;");

                    actionString = "INSERT";
                }

                else
                {
                    if (model.ACTION_TYPE == "UPDATE")
                    {
                        string strCheckexist = $" select *  from SFIS1.c_model_nomms_t a where  upper(a.model_name) = '{model.MODEL_NAME.ToUpper()}' and upper(a.model_desc) = '{model.MODEL_DESC.ToUpper()}' and upper(a.GROUP_NAME) = '{model.FROM_GROUP.ToUpper()}'  and upper(a.NEXT_GROUP) = '{model.TO_GROUP.ToUpper()}'  ";
                        if (DBConnect.GetData(strCheckexist, model.database_name).Rows.Count > 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "exists" });
                        }

                        //check privilege
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CARTON PREFIX_EDIT' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        //existed => update
                        actionString = "UPDATE";

                        sb.Append($" Begin  SFIS1.P_IUD_MODEL_REWORK( '{model.MODEL_NAME}','{model.MODEL_DESC}','{model.FROM_GROUP}' ,'{model.TO_GROUP}','{model.ID}','UPDATE'); end;");
                    }
                    else
                    {
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'CARTON PREFIX_DELETE' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        //existed => update
                        actionString = "DELETE";

                        sb.Append($" Begin  SFIS1.P_IUD_MODEL_REWORK( '{model.MODEL_NAME}','{model.MODEL_DESC}','{model.FROM_GROUP}' ,'{model.TO_GROUP}','{model.ID}','DELETE'); end;");
                    }


                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Setup Model Rework  MODEL_NAME:  {model.MODEL_NAME}, MODEL_DESC : {model.MODEL_DESC}, FROM_GROUP:{model.FROM_GROUP} , TO_GROUP: {model.TO_GROUP}, ID: {model.ID}; IP:{AuthorizationController.UserIP()} ; TABLE SFIS1.C_MODEL_NOMMS_T' ");
                sbLog.Append(" ) ");
                string strInsertLog = sbLog.ToString();
                string strInserUpdate = sb.ToString();
                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config 6
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);       // DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }



        }
    }
}
