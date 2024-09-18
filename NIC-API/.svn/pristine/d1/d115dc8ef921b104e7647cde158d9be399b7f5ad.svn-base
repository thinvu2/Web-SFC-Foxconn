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
using static SN_API.Models.Config19Element;
using static SN_API.Models.LineElement;

namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class Config19Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig19Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig19Content(Config19Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $" select A.*,B.CUSTOMER, ROWIDTOCHAR(A.rowid) ID from SFIS1.C_CUST_SNRULE_T A LEFT JOIN  SFIS1.C_CUSTOMER_T B ON A.CUST_CODE = B.CUST_CODE WHERE ROWNUM< 20 ORDER BY A.MODEL_NAME,VERSION_CODE ";
            }
            else
            {
                strGetData = $" select A.*,B.CUSTOMER, ROWIDTOCHAR(A.rowid) ID from SFIS1.C_CUST_SNRULE_T A LEFT JOIN  SFIS1.C_CUSTOMER_T B ON A.CUST_CODE = B.CUST_CODE WHERE upper(A.MODEL_NAME) LIKE '%{model.MODEL_NAME.ToUpper()}%' ORDER BY A.MODEL_NAME,VERSION_CODE ";
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
        [System.Web.Http.Route("Config19GetAllModel")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config19GetAllModel(Config19Element model)
        {
            string strGetData = "select model_name from SFIS1.C_MODEL_DESC_T order by model_name asc";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }

        [System.Web.Http.Route("Config19GetCustomer")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config19GetCustomer(Config19Element model)
        {
            string strGetData = "select customer,cust_code from SFIS1.C_CUSTOMER_T where customer not like '10.%' and customer not like '%?%' order by customer ";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }
        [System.Web.Http.Route("InsertUpdateConfig19")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig1(Config19Element model)
        {
            try
            {
                //check privilege
                string strPrivilege = $" select substr(FUN,13,7) SubFun ,PRIVILEGE from SFIS1.C_PRIVILEGE where EMP = '{model.EMP}' AND FUN like 'CUST SNRULE_%' AND PRG_NAME = 'CONFIG' AND (TRIM(substr(FUN,13,7)) = 'EDIT' OR  TRIM(substr(FUN,13,7)) = 'DELETE') ORDER BY PRIVILEGE ";
                StringBuilder sb = new StringBuilder();

                StringBuilder sbLog = new StringBuilder();
                string modify = " ";

                if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                }
                //check exist
                string strCheckExist = $" select model_name from sfis1.c_cust_snrule_t where model_name = '{model.MODEL_NAME}' and version_code ='{model.VERSION_CODE}' ";
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
                    sb.Append($" '{model.CUST_CODE}', "); //CUST_CODE,
                    sb.Append($" '{model.MODEL_NAME}', "); //MODEL_NAME,
                    sb.Append($" '{model.VERSION_CODE}', "); //VERSION_CODE,
                    sb.Append($" '{model.CUST_MODEL_NAME}', "); //CUST_MODEL_NAME,
                    sb.Append($" '{model.VERSION_CODE}', "); //CUST_VERSION_CODE
                    sb.Append($" '{model.CUST_MODEL_DESC}', "); //,CUST_MODEL_DESC,
                    sb.Append($" '{model.CARTON_LAB_NAME}', "); //CARTON_LAB_NAME,
                    sb.Append($" '{model.UPCEANDATA}', "); //UPCEANDATA,
                    sb.Append($" '{model.CUST_SN_PREFIX}', "); //CUST_SN_PREFIX,
                    sb.Append($" '{model.CUST_VENDER_CODE}', "); //CUST_VENDER_CODE,
                    sb.Append($" '{model.CUST_SN_POSTFIX}', "); //CUST_SN_POSTFIX,
                    sb.Append($" '{model.CUST_SN_LENG}', "); //CUST_SN_LENG,
                    sb.Append($" '{model.CUST_SN_STR}', "); //CUST_SN_STR,
                    sb.Append($" '{model.CUST_CARTON_PREFIX}', "); //CUST_CARTON_PREFIX,
                    sb.Append($" '{model.CUST_CARTON_POSTFIX}', "); //CUST_CARTON_POSTFIX,
                    sb.Append($" '{model.CUST_CARTON_LENG}', "); //CUST_CARTON_LENG,
                    sb.Append($" '{model.CUST_CARTON_STR}', "); //CUST_CARTON_STR,
                    sb.Append($" '{model.CUST_PALLET_PREFIX}', "); //CUST_PALLET_PREFIX,
                    sb.Append($" '{model.CUST_PALLET_POSTFIX}', "); //CUST_PALLET_POSTFIX,
                    sb.Append($" '{model.CUST_PALLET_LENG}', "); //CUST_PALLET_LENG,
                    sb.Append($" '{model.CUST_PALLET_STR}', "); //CUST_PALLET_STR,
                    sb.Append($" '{model.PALLET_LAB_NAME}', "); //PALLET_LAB_NAME,
                    sb.Append($" '{model.EMP}', "); //EMP_NO,
                    sb.Append($" sysdate, "); //IN_STATION_TIME,
                    sb.Append($" '{model.MACID_PREFIX}', "); //MACID_PREFIX,
                    sb.Append($" '{model.D1}', "); //D1,
                    sb.Append($" '{model.CUST_BOX_PREFIX}', "); //CUST_BOX_PREFIX,
                    sb.Append($" '{model.CUST_BOX_LENG}', "); //CUST_BOX_LENG,
                    sb.Append($" '{model.CUST_BOX_STR}', "); //CUST_BOX_STR,
                    sb.Append($" '{model.FINISH_GOOD}' "); //FINISH_GOOD
                    sb.Append(" )");
                    actionString = "INSERT";
                }
                else
                {
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" Update sfis1.c_cust_snrule_t ");
                    sb.Append(" SET ");
                    sb.Append($" VERSION_CODE = '{model.VERSION_CODE}', ");//VERSION_CODE
                    sb.Append($" CUST_MODEL_NAME = '{model.CUST_MODEL_NAME}', ");//CUST_MODEL_NAME
                    sb.Append($" CUST_VERSION_CODE = '{model.VERSION_CODE}', ");//CUST_VERSION_CODE
                    sb.Append($" CUST_MODEL_DESC = '{model.CUST_MODEL_DESC}', ");//CUST_MODEL_DESC
                    sb.Append($" CARTON_LAB_NAME = '{model.CARTON_LAB_NAME}', ");//CARTON_LAB_NAME
                    sb.Append($" UPCEANDATA = '{model.UPCEANDATA}', ");//UPCEANDATA
                    sb.Append($" CUST_SN_PREFIX = '{model.CUST_SN_PREFIX}', ");//CUST_SN_PREFIX
                    sb.Append($" CUST_VENDER_CODE = '{model.CUST_VENDER_CODE}', ");//CUST_VENDER_CODE
                    sb.Append($" CUST_SN_POSTFIX = '{model.CUST_SN_POSTFIX}', ");//CUST_SN_POSTFIX
                    sb.Append($" CUST_SN_LENG = '{model.CUST_SN_LENG}', ");//CUST_SN_LENG
                    sb.Append($" CUST_SN_STR = '{model.CUST_SN_STR}', ");//CUST_SN_STR
                    sb.Append($" CUST_CARTON_PREFIX = '{model.CUST_CARTON_PREFIX}', ");//CUST_CARTON_PREFIX
                    sb.Append($" CUST_CARTON_POSTFIX = '{model.CUST_CARTON_POSTFIX}', ");//CUST_CARTON_POSTFIX
                    sb.Append($" CUST_CARTON_LENG = '{model.CUST_CARTON_LENG}', ");//CUST_CARTON_LENG
                    sb.Append($" CUST_CARTON_STR = '{model.CUST_CARTON_STR}', ");//CUST_CARTON_STR
                    sb.Append($" CUST_PALLET_PREFIX = '{model.CUST_PALLET_PREFIX}', ");//CUST_PALLET_PREFIX
                    sb.Append($" CUST_PALLET_POSTFIX = '{model.CUST_PALLET_POSTFIX}', ");//CUST_PALLET_POSTFIX
                    sb.Append($" CUST_PALLET_LENG = '{model.CUST_PALLET_LENG}', ");//CUST_PALLET_LENG
                    sb.Append($" CUST_PALLET_STR = '{model.CUST_PALLET_STR}' ,");//CUST_PALLET_STR
                    sb.Append($" PALLET_LAB_NAME = '{model.PALLET_LAB_NAME}', ");//PALLET_LAB_NAME
                    sb.Append($" EMP_NO = '{model.EMP_NO}', ");//EMP_NO
                    sb.Append($" IN_STATION_TIME = sysdate, ");  //time
                    sb.Append($" MACID_PREFIX = '{model.MACID_PREFIX}', "); //MACID_PREFIX
                    sb.Append($" D1 = '{model.D1}', "); //D1
                    sb.Append($" CUST_BOX_PREFIX = '{model.CUST_BOX_PREFIX}', "); //CUST_BOX_PREFIX
                    sb.Append($" CUST_BOX_LENG = '{model.CUST_BOX_LENG}', ");  //CUST_BOX_LENG
                    sb.Append($" CUST_BOX_STR = '{model.CUST_BOX_STR}', "); //CUST_BOX_STR
                    sb.Append($" FINISH_GOOD = '{model.FINISH_GOOD}' "); //FINISH_GOOD
                    sb.Append($" WHERE ROWID = '{model.ID}' ");

                    #region history change data to update
                    modify = " UPDATE: ";
                    string query = $"select  VERSION_CODE,CUST_MODEL_NAME,CUST_VERSION_CODE,CUST_MODEL_DESC,CARTON_LAB_NAME,UPCEANDATA,CUST_SN_PREFIX," +
                        $"CUST_VENDER_CODE,CUST_SN_POSTFIX, CUST_SN_LENG,CUST_SN_STR,CUST_CARTON_PREFIX,CUST_CARTON_POSTFIX,CUST_CARTON_LENG," +
                        $"CUST_CARTON_STR,CUST_PALLET_PREFIX,CUST_PALLET_POSTFIX, CUST_PALLET_LENG,CUST_PALLET_STR,PALLET_LAB_NAME,MACID_PREFIX,D1," +
                        $"CUST_BOX_PREFIX,CUST_BOX_LENG,CUST_BOX_STR,FINISH_GOOD from sfis1.c_cust_snrule_t  WHERE ROWID = '{model.ID}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);
                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if (row[0].ToString() != model.VERSION_CODE)
                        {
                            modify += $" VERSION_CODE: {row[0].ToString()} => {model.VERSION_CODE};";
                        }
                        if (row[1].ToString() != model.CUST_MODEL_NAME)
                        {
                            modify += $" CUST_MODEL_NAME: {row[1].ToString()} => {model.CUST_MODEL_NAME};";
                        }
                        if (row[2].ToString() != model.VERSION_CODE)
                        {
                            modify += $" CUST_VERSION_CODE: {row[2].ToString()} => {model.VERSION_CODE};";
                        }
                        if (row[3].ToString() != model.CUST_MODEL_DESC)
                        {
                            modify += $" CUST_MODEL_DESC: {row[3].ToString()} => {model.CUST_MODEL_DESC};";
                        }
                        if (row[4].ToString() != model.CARTON_LAB_NAME)
                        {
                            modify += $" CARTON_LAB_NAME: {row[4].ToString()} => {model.CARTON_LAB_NAME};";
                        }
                        if (row[5].ToString() != model.UPCEANDATA)
                        {
                            modify += $" UPCEANDATA: {row[5].ToString()} => {model.UPCEANDATA};";
                        }
                        if (row[6].ToString() != model.CUST_SN_PREFIX)
                        {
                            modify += $" CUST_SN_PREFIX: {row[6].ToString()} => {model.CUST_SN_PREFIX};";
                        }
                        if (row[7].ToString() != model.CUST_VENDER_CODE)
                        {
                            modify += $" CUST_VENDER_CODE: {row[7].ToString()} => {model.CUST_VENDER_CODE};";
                        }
                        if (row[8].ToString() != model.CUST_SN_POSTFIX)
                        {
                            modify += $" CUST_SN_POSTFIX: {row[8].ToString()} => {model.CUST_SN_POSTFIX};";
                        }
                        if (row[9].ToString() != model.CUST_SN_LENG)
                        {
                            modify += $" CUST_SN_LENG: {row[9].ToString()} => {model.CUST_SN_LENG};";
                        }
                        if (row[10].ToString() != model.CUST_SN_STR)
                        {
                            modify += $" CUST_SN_STR: {row[10].ToString()} => {model.CUST_SN_STR};";
                        }
                        if (row[11].ToString() != model.CUST_CARTON_PREFIX)
                        {
                            modify += $" CUST_CARTON_PREFIX: {row[11].ToString()} => {model.CUST_CARTON_PREFIX};";
                        }
                        if (row[12].ToString() != model.CUST_CARTON_POSTFIX)
                        {
                            modify += $" CUST_CARTON_POSTFIX: {row[12].ToString()} => {model.CUST_CARTON_POSTFIX};";
                        }
                        if (row[13].ToString() != model.CUST_CARTON_LENG)
                        {
                            modify += $" CUST_CARTON_LENG: {row[13].ToString()} => {model.CUST_CARTON_LENG};";
                        }
                        if (row[14].ToString() != model.CUST_CARTON_STR)
                        {
                            modify += $" CUST_CARTON_STR: {row[14].ToString()} => {model.CUST_CARTON_STR};";
                        }
                        if (row[15].ToString() != model.CUST_PALLET_PREFIX)
                        {
                            modify += $" CUST_PALLET_PREFIX: {row[15].ToString()} => {model.CUST_PALLET_PREFIX};";
                        }
                        if (row[16].ToString() != model.CUST_PALLET_POSTFIX)
                        {
                            modify += $" CUST_PALLET_POSTFIX: {row[16].ToString()} => {model.CUST_PALLET_POSTFIX};";
                        }
                        if (row[17].ToString() != model.CUST_PALLET_LENG)
                        {
                            modify += $" CUST_PALLET_LENG: {row[17].ToString()} => {model.CUST_PALLET_LENG};";
                        }
                        if (row[18].ToString() != model.CUST_PALLET_STR)
                        {
                            modify += $" CUST_PALLET_STR: {row[18].ToString()} => {model.CUST_PALLET_STR};";
                        }
                        if (row[19].ToString() != model.PALLET_LAB_NAME)
                        {
                            modify += $" PALLET_LAB_NAME: {row[19].ToString()} => {model.PALLET_LAB_NAME};";
                        }
                        if (row[20].ToString() != model.MACID_PREFIX)
                        {
                            modify += $" MACID_PREFIX: {row[20].ToString()} => {model.MACID_PREFIX};";
                        }
                        if (row[21].ToString() != model.D1)
                        {
                            modify += $" D1: {row[21].ToString()} => {model.D1};";
                        }
                        if (row[22].ToString() != model.CUST_BOX_PREFIX)
                        {
                            modify += $" CUST_BOX_PREFIX: {row[22].ToString()} => {model.CUST_BOX_PREFIX};";
                        }
                        if (row[23].ToString() != model.CUST_BOX_LENG)
                        {
                            modify += $" CUST_BOX_LENG: {row[23].ToString()} => {model.CUST_BOX_LENG};";
                        }
                        if (row[24].ToString() != model.CUST_BOX_STR)
                        {
                            modify += $" CUST_BOX_STR: {row[24].ToString()} => {model.CUST_BOX_STR};";
                        }
                        if (row[25].ToString() != model.FINISH_GOOD)
                        {
                            modify += $" FINISH_GOOD: {row[25].ToString()} => {model.FINISH_GOOD};";
                        }
                        #endregion
                    }
                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'CONFIG19 {model.MODEL_NAME};{model.VERSION_CODE};{modify};IP:{AuthorizationController.UserIP()}' ");
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
        public string GetSelectByList(List<ConfigIds> list, string type)
        {
            StringBuilder sb = new StringBuilder();
            if (list.Count > 0)
            {
                var sbContent = new StringBuilder();
                sbContent.Append(" IN (");

                for (int i = 0; i < list.Count; i++)
                {
                    string mo = list[i].ID.ToString();

                    sbContent.AppendFormat("'{0}'", mo);
                    if (i < list.Count - 1)
                    {
                        sbContent.Append(",");
                    }
                }
                sbContent.Append(")");
                sb.Append($" {type} {sbContent} ");
            }
            return sb.ToString();
        }
        [System.Web.Http.Route("DeleteConfig19")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig19(Config19Element model)
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
                sbLog.Append($"  'CONFIG19 {model.MODEL_NAME};{model.VERSION_CODE};IP:{AuthorizationController.UserIP()}' ");
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