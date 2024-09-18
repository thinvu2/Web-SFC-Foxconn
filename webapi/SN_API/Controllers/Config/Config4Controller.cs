using SN_API.Models;
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
using static SN_API.Models.Config4Element;
using static SN_API.Models.LineElement;

namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class Config4Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig4Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig4Content(Config4Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.LINE_NAME))
            {
                strGetData = $" SELECT A.LINE_NAME,A.SECTION_NAME,A.MAP_WORK_SECTION,A.START_TIME,A.END_TIME,A.CLASS,A.SERIAL,A.DAY_DISTINCT,A.CREATE_DATE,ROWIDTOCHAR(ROWID) ID FROM SFIS1.C_CLASS_WORK_T A WHERE ROWNUM < 50 ORDER BY LINE_NAME,SERIAL ";
            }
            else
            {
                strGetData = $" SELECT A.LINE_NAME,A.SECTION_NAME,A.MAP_WORK_SECTION,A.START_TIME,A.END_TIME,A.CLASS,A.SERIAL,A.DAY_DISTINCT,A.CREATE_DATE,ROWIDTOCHAR(ROWID) ID FROM SFIS1.C_CLASS_WORK_T A WHERE UPPER(A.LINE_NAME) LIKE '%{model.LINE_NAME}%' AND ROWNUM < 50 ORDER BY LINE_NAME,SERIAL ";
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


        [System.Web.Http.Route("CoppyTemplateSection")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> CoppyTemplateSection(Config4Element model)
        {
            string strGetData = $" SELECT distinct '{model.NEW_LINE_NAME}' LINE_NAME,A.SECTION_NAME,A.MAP_WORK_SECTION,A.START_TIME,A.END_TIME,A.CLASS,A.SERIAL,A.DAY_DISTINCT FROM SFIS1.C_CLASS_WORK_T A where A.LINE_NAME = '{model.OLD_LINE_NAME}' order by A.CLASS,A.DAY_DISTINCT,A.SERIAL ";

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

        [System.Web.Http.Route("GetTemplateSection")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetTemplateSection(Config4Element model)
        {
          
            string strGetData = $" SELECT  A.LINE_NAME FROM SFIS1.C_CLASS_WORK_T A where A.LINE_NAME = '{model.OLD_LINE_NAME}' order by A.CLASS,A.DAY_DISTINCT,A.SERIAL ";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                string strInsert = " insert into SFIS1.C_CLASS_WORK_T" +
$"SELECT distinct '{model.NEW_LINE_NAME}' LINE_NAME,A.SECTION_NAME,A.MAP_WORK_SECTION,A.START_TIME,A.END_TIME,A.CLASS,A.SERIAL,A.DAY_DISTINCT,sysdate CREATE_DATE FROM SFIS1.C_CLASS_WORK_T A where A.LINE_NAME = '{model.OLD_LINE_NAME}' order by A.CLASS,A.DAY_DISTINCT,A.SERIAL";
                DBConnect.ExecuteNoneQuery(strInsert, model.database_name);


                //string strInsert = " insert into SFIS1.C_CLASS_WORK_T" +
//$"SELECT distinct '{model.NEW_LINE_NAME}' LINE_NAME,A.SECTION_NAME,A.MAP_WORK_SECTION,A.START_TIME,A.END_TIME,A.CLASS,A.SERIAL,A.DAY_DISTINCT,sysdate CREATE_DATE FROM SFIS1.C_CLASS_WORK_T A where A.LINE_NAME = '{model.OLD_LINE_NAME}' order by A.CLASS,A.DAY_DISTINCT,A.SERIAL";
               // DBConnect.ExecuteNoneQuery(strInsert, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }

        [System.Web.Http.Route("Config4GetAllModel")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config4GetAllModel(Config4Element model)
        {
            string strGetData = "select LINE_NAME MODEL_NAME from SFIS1.C_LINE_DESC_T order by LINE_NAME asc";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }

        [System.Web.Http.Route("Config4GetCustomer")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config4GetCustomer(Config4Element model)
        {
            string strGetData = "select customer,cust_code from SFIS1.C_CUSTOMER_T where customer not like '10.%' and customer not like '%?%' order by customer ";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }
        [System.Web.Http.Route("InsertUpdateConfig4")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig1(Config4Element model)
        {
            try
            {
                //check privilege
                string strPrivilege = $" select substr(FUN,13,7) SubFun ,PRIVILEGE from SFIS1.C_PRIVILEGE where EMP = '{model.EMP}' AND FUN like 'CUST SNRULE_%' AND PRG_NAME = 'CONFIG' AND (TRIM(substr(FUN,13,7)) = 'EDIT' OR  TRIM(substr(FUN,13,7)) = 'DELETE') ORDER BY PRIVILEGE ";
                StringBuilder sb = new StringBuilder();

                StringBuilder sbLog = new StringBuilder();

                if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                }
                //check exist
                string strCheckExist = $" select model_name from sfis1.c_cust_snrule_t where model_name = '{model.LINE_NAME}' and version_code ='{model.LINE_NAME}' ";
                string actionString = " ";
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    // not exist => insert
                    sb.Append(" Insert into sfis1.c_cust_snrule_t ");
                    sb.Append(" (CUST_CODE,MODEL_NAME,VERSION_CODE,CUST_MODEL_NAME,CUST_VERSION_CODE,CUST_MODEL_DESC,CARTON_LAB_NAME,UPCEANDATA, ");
                    sb.Append(" CUST_SN_PREFIX,CUST_VENDER_CODE,CUST_SN_POSTFIX,CUST_SN_LENG,CUST_SN_STR, ");
                    sb.Append(" CUST_CARTON_PREFIX,CUST_CARTON_POSTFIX,CUST_CARTON_LENG,CUST_CARTON_STR,CUST_PALLET_PREFIX,CUST_PALLET_POSTFIX, ");
                    sb.Append(" CUST_PALLET_LENG,CUST_PALLET_STR,PALLET_LAB_NAME,EMP_NO,IN_STATION_TIME,MACID_PREFIX, D1,CUST_BOX_PREFIX,CUST_BOX_LENG,CUST_BOX_STR,FINISH_GOOD) ");
                    sb.Append(" values ( ");
                    //sb.Append($" '{model.CUST_CODE}', "); //CUST_CODE,
                    //sb.Append($" '{model.MODEL_NAME}', "); //MODEL_NAME,
                    //sb.Append($" '{model.VERSION_CODE}', "); //VERSION_CODE,
                    //sb.Append($" '{model.CUST_MODEL_NAME}', "); //CUST_MODEL_NAME,
                    //sb.Append($" '{model.VERSION_CODE}', "); //CUST_VERSION_CODE
                    //sb.Append($" '{model.CUST_MODEL_DESC}', "); //,CUST_MODEL_DESC,
                    //sb.Append($" '{model.CARTON_LAB_NAME}', "); //CARTON_LAB_NAME,
                    //sb.Append($" '{model.UPCEANDATA}', "); //UPCEANDATA,
                    //sb.Append($" '{model.CUST_SN_PREFIX}', "); //CUST_SN_PREFIX,
                    //sb.Append($" '{model.CUST_VENDER_CODE}', "); //CUST_VENDER_CODE,
                    //sb.Append($" '{model.CUST_SN_POSTFIX}', "); //CUST_SN_POSTFIX,
                    //sb.Append($" '{model.CUST_SN_LENG}', "); //CUST_SN_LENG,
                    //sb.Append($" '{model.CUST_SN_STR}', "); //CUST_SN_STR,
                    //sb.Append($" '{model.CUST_CARTON_PREFIX}', "); //CUST_CARTON_PREFIX,
                    //sb.Append($" '{model.CUST_CARTON_POSTFIX}', "); //CUST_CARTON_POSTFIX,
                    //sb.Append($" '{model.CUST_CARTON_LENG}', "); //CUST_CARTON_LENG,
                    //sb.Append($" '{model.CUST_CARTON_STR}', "); //CUST_CARTON_STR,
                    //sb.Append($" '{model.CUST_PALLET_PREFIX}', "); //CUST_PALLET_PREFIX,
                    //sb.Append($" '{model.CUST_PALLET_POSTFIX}', "); //CUST_PALLET_POSTFIX,
                    //sb.Append($" '{model.CUST_PALLET_LENG}', "); //CUST_PALLET_LENG,
                    //sb.Append($" '{model.CUST_PALLET_STR}', "); //CUST_PALLET_STR,
                    //sb.Append($" '{model.PALLET_LAB_NAME}', "); //PALLET_LAB_NAME,
                    //sb.Append($" '{model.EMP}', "); //EMP_NO,
                    //sb.Append($" sysdate, "); //IN_STATION_TIME,
                    //sb.Append($" '{model.MACID_PREFIX}', "); //MACID_PREFIX,
                    //sb.Append($" '{model.D1}', "); //D1,
                    //sb.Append($" '{model.CUST_BOX_PREFIX}', "); //CUST_BOX_PREFIX,
                    //sb.Append($" '{model.CUST_BOX_LENG}', "); //CUST_BOX_LENG,
                    //sb.Append($" '{model.CUST_BOX_STR}', "); //CUST_BOX_STR,
                    //sb.Append($" '{model.FINISH_GOOD}' "); //FINISH_GOOD
                    sb.Append(" )");
                    actionString = "INSERT";
                }
                else
                {
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" Update sfis1.c_cust_snrule_t ");
                    sb.Append(" SET ");
                    //sb.Append($" VERSION_CODE = '{model.VERSION_CODE}', ");//VERSION_CODE
                    //sb.Append($" CUST_MODEL_NAME = '{model.CUST_MODEL_NAME}', ");//CUST_MODEL_NAME
                    //sb.Append($" CUST_VERSION_CODE = '{model.VERSION_CODE}', ");//CUST_VERSION_CODE
                    //sb.Append($" CUST_MODEL_DESC = '{model.CUST_MODEL_DESC}', ");//CUST_MODEL_DESC
                    //sb.Append($" CARTON_LAB_NAME = '{model.CARTON_LAB_NAME}', ");//CARTON_LAB_NAME
                    //sb.Append($" UPCEANDATA = '{model.UPCEANDATA}', ");//UPCEANDATA
                    //sb.Append($" CUST_SN_PREFIX = '{model.CUST_SN_PREFIX}', ");//CUST_SN_PREFIX
                    //sb.Append($" CUST_VENDER_CODE = '{model.CUST_VENDER_CODE}', ");//CUST_VENDER_CODE
                    //sb.Append($" CUST_SN_POSTFIX = '{model.CUST_SN_POSTFIX}', ");//CUST_SN_POSTFIX
                    //sb.Append($" CUST_SN_LENG = '{model.CUST_SN_LENG}', ");//CUST_SN_LENG
                    //sb.Append($" CUST_SN_STR = '{model.CUST_SN_STR}', ");//CUST_SN_STR
                    //sb.Append($" CUST_CARTON_PREFIX = '{model.CUST_CARTON_PREFIX}', ");//CUST_CARTON_PREFIX
                    //sb.Append($" CUST_CARTON_POSTFIX = '{model.CUST_CARTON_POSTFIX}', ");//CUST_CARTON_POSTFIX
                    //sb.Append($" CUST_CARTON_LENG = '{model.CUST_CARTON_LENG}', ");//CUST_CARTON_LENG
                    //sb.Append($" CUST_CARTON_STR = '{model.CUST_CARTON_STR}', ");//CUST_CARTON_STR
                    //sb.Append($" CUST_PALLET_PREFIX = '{model.CUST_PALLET_PREFIX}', ");//CUST_PALLET_PREFIX
                    //sb.Append($" CUST_PALLET_POSTFIX = '{model.CUST_PALLET_POSTFIX}', ");//CUST_PALLET_POSTFIX
                    //sb.Append($" CUST_PALLET_LENG = '{model.CUST_PALLET_LENG}', ");//CUST_PALLET_LENG
                    //sb.Append($" CUST_PALLET_STR = '{model.CUST_PALLET_STR}' ,");//CUST_PALLET_STR
                    //sb.Append($" PALLET_LAB_NAME = '{model.PALLET_LAB_NAME}', ");//PALLET_LAB_NAME
                    //sb.Append($" EMP_NO = '{model.EMP_NO}', ");//EMP_NO
                    //sb.Append($" IN_STATION_TIME = sysdate, ");  //time
                    //sb.Append($" MACID_PREFIX = '{model.MACID_PREFIX}', "); //MACID_PREFIX
                    //sb.Append($" D1 = '{model.D1}', "); //D1
                    //sb.Append($" CUST_BOX_PREFIX = '{model.CUST_BOX_PREFIX}', "); //CUST_BOX_PREFIX
                    //sb.Append($" CUST_BOX_LENG = '{model.CUST_BOX_LENG}', ");  //CUST_BOX_LENG
                    //sb.Append($" CUST_BOX_STR = '{model.CUST_BOX_STR}', "); //CUST_BOX_STR
                    //sb.Append($" FINISH_GOOD = '{model.FINISH_GOOD}' "); //FINISH_GOOD
                    sb.Append($" WHERE ROWID = '{model.ID}' ");
                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                // sbLog.Append($"  'Config4 {model.MODEL_NAME};{model.VERSION_CODE};IP:{AuthorizationController.UserIP()}' ");
                sbLog.Append(" ) ");

                string strInsertLog = sbLog.ToString();
                string strInserUpdate = sb.ToString();
                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config 19
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }

        [System.Web.Http.Route("DeleteConfig4")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig4(Config4Element model)
        {
            string strDelete = $" delete SFIS1.C_CUST_SNRULE_T where  ROWID = '{model.ID}' ";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'Config4 {model.LINE_NAME};{model.LINE_NAME};IP:{AuthorizationController.UserIP()}' ");
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