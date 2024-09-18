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
    public class Config6Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig6Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig6Content(Config6Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $" select A.*,B.ROUTE_NAME,ROWIDTOCHAR(A.ROWID) ID FROM SFIS1.C_MODEL_DESC_T  A , SFIS1.C_ROUTE_NAME_T B WHERE A.ROUTE_CODE = B.ROUTE_CODE (+) AND ROWNUM < 20 order by model_name ";
            }
            else
            {
                strGetData = $" select A.*,B.ROUTE_NAME,ROWIDTOCHAR(A.ROWID) ID FROM SFIS1.C_MODEL_DESC_T  A , SFIS1.C_ROUTE_NAME_T B WHERE upper(A.MODEL_NAME) LIKE '%{model.MODEL_NAME.ToUpper()}%' AND A.ROUTE_CODE = B.ROUTE_CODE (+) AND ROWNUM < 20 order by model_name ";
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
        [System.Web.Http.Route("Config6GetAllBom")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config6GetAllBom(Config6Element model)
        {
            string strGetData = "select distinct bom_no from SFIS1.Cmodel_KEYPART_T order by bom_no asc";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }

        [System.Web.Http.Route("Config6GetAllModelSerial")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config6GetAllModelSerial(Config6Element model)
        {
            string strGetData = " select distinct model_serial from SFIS1.C_MODEL_DESC_T where checkflag != 'SFC' order by model_serial ";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }
        [System.Web.Http.Route("Config6GetAllRoute")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config6GetAllRoute(Config6Element model)
        {
            string strGetData = " select A.ROUTE_NAME,A.ROUTE_CODE,(SELECT GROUP_NEXT FROM SFIS1.C_ROUTE_CONTROL_T WHERE ROUTE_CODE =  A.ROUTE_CODE AND GROUP_NAME = '0') DEFAULT_GROUP,(SELECT GROUP_NEXT FROM SFIS1.C_ROUTE_CONTROL_T B WHERE ROUTE_CODE = A.ROUTE_CODE  AND STATE_FLAG = 0 AND GROUP_NEXT NOT IN (SELECT GROUP_NAME FROM SFIS1.C_ROUTE_CONTROL_T  WHERE ROUTE_CODE = B.ROUTE_CODE  AND STATE_FLAG = 0) AND ROWNUM =1) END_GROUP FROM SFIS1.C_ROUTE_NAME_T A ";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }
        

        [System.Web.Http.Route("Config6GetAllModel")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config6GetAllModel(Config6Element model)
        {
            string strGetData = "select model_name from SFIS1.C_MODEL_DESC_T order by model_name asc";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }



        [System.Web.Http.Route("Config6GetCustomer")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config6GetCustomer(Config6Element model)
        {
            string strGetData = "select customer,cust_code from SFIS1.C_CUSTOMER_T where customer not like '10.%' and customer not like '%?%' order by customer ";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }
        [System.Web.Http.Route("InsertUpdateConfig6")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig1(Config6Element model)
        {
            try
            {
                //check privilege
                string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'MODEL_EDIT' AND EMP='{model.EMP}'";
                StringBuilder sb = new StringBuilder();

                StringBuilder sbLog = new StringBuilder();

                if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                }
                //check exist
                string strCheckExist = $" select model_name from SFIS1.C_MODEL_DESC_T where model_name = '{model.MODEL_NAME}' ";
                string actionString = " ";
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    // not exist => insert
                    sb.Append(" Insert into sfis1.c_model_desc_t ");
                    sb.Append(" (MODEL_NAME,MODEL_SERIAL,MODEL_TYPE,BOM_NO,MODEL_RANGE1,MODEL_RANGE2,CUSTOMER,STANDARD,ROUTE_CODE,DEFAULT_GROUP,END_GROUP,PRODUCT_NAME,LEAD_FREE) ");                    
                    sb.Append(" values ( ");
                    sb.Append($" '{model.MODEL_NAME}', "); //MODEL_NAME,
                    sb.Append($" '{model.MODEL_SERIAL}', "); //MODEL_SERIAL,
                    sb.Append($" '{model.MODEL_TYPE}', "); //MODEL_TYPE,
                    sb.Append($" '{model.BOM_NO}', "); //BOM_NO,
                    sb.Append($" '{model.MODEL_RANGE1}', "); //MODEL_RANGE1
                    sb.Append($" '{model.MODEL_RANGE2}', "); //,MODEL_RANGE2,
                    sb.Append($" '{model.CUSTOMER}', "); //CUSTOMER,
                    sb.Append($" '{model.STANDARD}', "); //STANDARD,
                    sb.Append($" '{model.ROUTE_CODE}', "); //ROUTE_CODE,
                    sb.Append($" '{model.DEFAULT_GROUP}', "); //DEFAULT_GROUP,
                    sb.Append($" '{model.END_GROUP}', "); //END_GROUP,
                    sb.Append($" '{model.PRODUCT_NAME}', "); //PRODUCT_NAME,
                    sb.Append($" '{model.LEAD_FREE}' "); //LEAD_FREE,                    
                    sb.Append(" )");
                    actionString = "INSERT";
                }
                else
                {
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" update sfis1.c_MODEL_desc_t ");
                    sb.Append(" SET ");
                    sb.Append($" MODEL_NAME = '{model.MODEL_NAME}', ");//MODEL_NAME
                    sb.Append($" MODEL_SERIAL = '{model.MODEL_SERIAL}', ");//MODEL_SERIAL
                    sb.Append($" MODEL_TYPE = '{model.MODEL_TYPE}', ");//MODEL_TYPE
                    sb.Append($" BOM_NO = '{model.BOM_NO}', ");//BOM_NO
                    sb.Append($" STANDARD = '{model.STANDARD}', ");//STANDARD
                    sb.Append($" CUSTOMER = '{model.CUSTOMER}', ");//CUSTOMER
                    sb.Append($" MODEL_RANGE1 = '{model.MODEL_RANGE1}', ");//MODEL_RANGE1
                    sb.Append($" MODEL_RANGE2 = '{model.MODEL_RANGE2}', ");//MODEL_RANGE2
                    sb.Append($" ROUTE_CODE = '{model.ROUTE_CODE}', ");//ROUTE_CODE
                    sb.Append($" DEFAULT_GROUP = '{model.DEFAULT_GROUP}', ");//DEFAULT_GROUP
                    sb.Append($" END_GROUP = '{model.END_GROUP}', ");//END_GROUP
                    sb.Append($" LEAD_FREE = '{model.LEAD_FREE}', ");//LEAD_FREE
                    sb.Append($" PRODUCT_NAME = '{model.PRODUCT_NAME}' ");//PRODUCT_NAME                    
                    sb.Append($" WHERE MODEL_NAME = '{model.MODEL_NAME}' ");
                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'CONFIG6 {model.MODEL_NAME};{model.VERSION_CODE};IP:{AuthorizationController.UserIP()}' ");
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
        [System.Web.Http.Route("DeleteConfig6")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig6(Config6Element model)
        {            
            string strDelete = $" delete SFIS1.C_MODEL_DESC_T where  ROWID = '{model.ID}' ";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'CONFIG6 {model.MODEL_NAME};{model.VERSION_CODE};IP:{AuthorizationController.UserIP()}' ");
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