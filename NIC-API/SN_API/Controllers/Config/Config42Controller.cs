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
    public class Config42Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig42Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig42Content(Config42Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $"select A.*, ROWIDTOCHAR(ROWID) ID from SFIS1.C_GROUP_SAP_MAPPING_T A where rownum<=20 order by A.MODEL_NAME";
            }
            else
            {
                strGetData = $"SELECT A.*, ROWIDTOCHAR(ROWID) ID FROM SFIS1.C_GROUP_SAP_MAPPING_T A WHERE UPPER(MODEL_NAME) LIKE '%{model.MODEL_NAME.ToUpper()}%'";
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

        //get all model
        [System.Web.Http.Route("GetallModel_CF42")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetallModel_CF42(Config42Element model)
        {
            string strGetData = "";

            strGetData = $"select model_name from sfis1.c_model_desc_t";

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

        //get all group_name
        [System.Web.Http.Route("GetallGroupCF42")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetallGroupCF42(Config42Element model)
        {
            string strGetData = "";

            strGetData = $"select group_name from sfis1.c_group_config_t order by group_name";


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

        //get all route type
        [System.Web.Http.Route("GetallRouteTypeCF42")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetallRouteTypeCF42(Config42Element model)
        {
            string strGetData = "";

            strGetData = $"select MO_TYPE ROUTE_TYPE from SFISM4.R_MO_BASE_T GROUP BY MO_TYPE order by MO_TYPE";


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

        [System.Web.Http.Route("InsertUpdateConfig42")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig42(Config42Element model)
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
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'SAP GROUP MAPPING_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    sb.Append($" Begin SFIS1.P_CRUD_SAP_GROUP_MAPPING( '{model.EMP}','{model.MODEL_NAME}','{model.GROUP_NAME}','{model.SEQUENCE_NO}' ,'{model.ROUTE_TYPE}','{model.ID}','{AuthorizationController.UserIP()}','INSERT'); end;");
                }

                else
                {
                    if (model.ACTION_TYPE == "UPDATE")
                    {
                        //check privilege
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'SAP GROUP MAPPING_EDIT' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        sb.Append($" Begin SFIS1.P_CRUD_SAP_GROUP_MAPPING( '{model.EMP}','{model.MODEL_NAME}','{model.GROUP_NAME}','{model.SEQUENCE_NO}' ,'{model.ROUTE_TYPE}','{model.ID}','{AuthorizationController.UserIP()}','UPDATE'); end;");
                    }
                    else
                    {
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'SAP GROUP MAPPING_DELETE' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        sb.Append($" Begin  SFIS1.P_CRUD_SAP_GROUP_MAPPING( '{model.EMP}','{model.MODEL_NAME}','{model.GROUP_NAME}','{model.SEQUENCE_NO}' ,'{model.ROUTE_TYPE}','{model.ID}','{AuthorizationController.UserIP()}','DELETE'); end;");
                    }
                }
                string strInserUpdate = sb.ToString();
                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config                                                       
                //DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }

        }
    }
}