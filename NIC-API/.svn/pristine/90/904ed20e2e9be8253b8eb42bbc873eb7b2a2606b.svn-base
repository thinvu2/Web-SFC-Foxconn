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
    public class Config64Controller : ApiController
    {
        [System.Web.Http.Route("GetConfig64Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig64Content(Config64Element model)
        {
            // check GWCPEII_CONFIG 

            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $"  select *  from SFIS1.TMM_REPORT_CONTENT where rownum <=20 ";
            }
            else
            {
                strGetData = $" select *  from SFIS1.TMM_REPORT_CONTENT WHERE   UPPER(MODEL_NAME) LIKE '%{model.MODEL_NAME.ToUpper()}%' ";
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
        [System.Web.Http.Route("GetallModel_CF64")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetallModel_CF64(Config45Element model)
        {
            // check GWCPEII_CONFIG 

            string strGetData = "";

            strGetData = $" select model_name from sfis1.c_model_desc_t  ";



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



        [System.Web.Http.Route("InsertUpdateTmm_report")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateTmm_report(Config64Element model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                string actionString = " ";
                string TYPE_NAME = "";
                string TYPE_NAME_FN = "";
                string strexist = "";




                TYPE_NAME = model.TYPE_NAME;
                char[] spearator = { '-' };
                Int32 count = 2;

                // Using the Method
                String[] cust_col = TYPE_NAME.Split(spearator,
                       count, StringSplitOptions.RemoveEmptyEntries);

                if (!TYPE_NAME.Contains("-"))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "Invalid Format TYPE_NAME, TYPE_NAME MUST include '-' " });
                }
                // TYPE_NAME_FN = $"{cust_col[0]} ; '{cust_col[1]}'";

                TYPE_NAME_FN = $"{cust_col[0]};{cust_col[1]}";

                string sfc_c = "";
                // String[] cust_col = TYPE_NAME.ToString().Split("-");

                //string type_col = $'cust_col[0]','{cust_col[1]}';
                //check exist

                if (cust_col[1].Substring(1, 1) == "'")
                {
                    sfc_c = $"'{cust_col[1]}'";
                }
                else
                {
                    sfc_c = cust_col[1];
                }





                if (model.ACTION_TYPE == "INSERT")
                {




                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'TMM_REPORT_ADD' and PRIVILEGE='2' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    strexist = $" SELECT * FROM SFIS1.c_tmm_report_t  WHERE  MODEL_NAME = '{model.MODEL_NAME}'  and  ( customer_name= '{cust_col[0]}' or default_location ='{model.DATA_LOCATION}' )   ";

                    if (DBConnect.GetData(strexist, model.database_name).Rows.Count > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "exists" });
                    }

                    /*sb.Append(" INSERT INTO SFIS1.C_TMM_REPORT_T");
                    sb.Append($" ( MODEL_NAME,CUSTOMER_NAME,SFC_COLUMN,DEFAULT_PREFIX,DEFAULT_POSTFIX,"+
                                        "DEFAULT_STR, DEFAULT_LENGTH, DEFAULT_LOCATION, IS_USE, DEFAULT_TEMP1"+
                                        ", DEFAULT_TEMP2, DEFAULT_TEMP3, ADD_CUST, QTY, USE_TOKEN)  VALUES " +
                                        $" ( '{model.MODEL_NAME}','{cust_col[0]}','{cust_col[1].Replace("'","''")}','{model.DEFAULT_PREFIX}','{model.DEFAULT_POSTFIX}','{model.DEFAULT_STRING}','{model.DEFAULT_LENGHT}','{model.DATA_LOCATION}','N','{model.EMP}',sysdate,'{model.CHECK_UNIQUE}','{model.UPLOAD_TO}','{model.CAM_QTY}','{model.USE_TOKEN}' )  ");
                    actionString = "INSERT";*/

                    sb.Append($" Begin  SFIS1.P_CRUD_TMM_REPORT_1( '{model.MODEL_NAME}','{cust_col[0]}','{cust_col[1].Replace("'", "''")}','{model.DEFAULT_PREFIX}','{model.DEFAULT_POSTFIX}','{model.DEFAULT_STRING}','{model.DEFAULT_LENGHT}','{model.DATA_LOCATION}','{model.EMP}','{model.CHECK_UNIQUE}','{model.UPLOAD_TO}','{model.CAM_QTY}','{model.USE_TOKEN}','{AuthorizationController.UserIP()}','INSERT','{model.ID}'); end;");

                    actionString = "INSERT";
                }

                else
                {
                    if (model.ACTION_TYPE == "UPDATE")
                    {


                        //check privilege
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'TMM_REPORT_EDIT' and PRIVILEGE='2' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        //existed => update
                        actionString = "UPDATE";

                        sb.Append($" Begin  SFIS1.P_CRUD_TMM_REPORT_1( '{model.MODEL_NAME}','{cust_col[0]}','{cust_col[1].Replace("'", "''")}','{model.DEFAULT_PREFIX}','{model.DEFAULT_POSTFIX}','{model.DEFAULT_STRING}','{model.DEFAULT_LENGHT}','{model.DATA_LOCATION}','{model.EMP}','{model.CHECK_UNIQUE}','{model.UPLOAD_TO}','{model.CAM_QTY}','{model.USE_TOKEN}','{AuthorizationController.UserIP()}','UPDATE','{model.ID}'); end;");
                    }
                    else
                    {

                        if (model.ACTION_TYPE == "DELETE")
                        {
                            strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'TMM_REPORT_DELETE' and PRIVILEGE='2' AND EMP='{model.EMP}'";
                            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                            }

                            //existed => update
                            actionString = "DELETE";

                            sb.Append($" Begin  SFIS1.P_CRUD_TMM_REPORT_1( '{model.MODEL_NAME}','{cust_col[0]}','{cust_col[1].Replace("'", "''")}','{model.DEFAULT_PREFIX}','{model.DEFAULT_POSTFIX}','{model.DEFAULT_STRING}','{model.DEFAULT_LENGHT}','{model.DATA_LOCATION}','{model.EMP}','{model.CHECK_UNIQUE}','{model.UPLOAD_TO}','{model.CAM_QTY}','{model.USE_TOKEN}','{AuthorizationController.UserIP()}','DELETE','{model.ID}'); end;");
                        }
                        //check privilege

                        if (model.ACTION_TYPE == "CONFIRM")
                        {
                            strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'TMM_REPORT_CONFIRM' AND EMP='{model.EMP}'";
                            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                            }

                            //existed => update
                            actionString = "CONFIRM";

                            sb.Append($" Begin  SFIS1.P_CRUD_TMM_REPORT_1( '{model.MODEL_NAME}','{cust_col[0]}','{cust_col[1].Replace("'", "''")}','{model.DEFAULT_PREFIX}','{model.DEFAULT_POSTFIX}','{model.DEFAULT_STRING}','{model.DEFAULT_LENGHT}','{model.DATA_LOCATION}','{model.EMP}','{model.CHECK_UNIQUE}','{model.UPLOAD_TO}','{model.CAM_QTY}','{model.USE_TOKEN}','{AuthorizationController.UserIP()}','CONFIRM','{model.ID}'); end;");
                            //   sb.Append($" Begin  SFIS1.P_CRUD_TMM_REPORT_1( '{model.MODEL_NAME}',CUSTOMER_NAME: {cust_col[0]}, SFC_COLUMN: {cust_col[1]}, DEFAULT_PREFIX: {model.DEFAULT_PREFIX}, DEFAULT_POSTFIX:  {model.DEFAULT_POSTFIX}, DEFAULT_STRING: {model.DEFAULT_STRING},DEFAULT_LENGHT: {model.DEFAULT_LENGHT} , DATA_LOCATION: {model.DATA_LOCATION} ,DEFAUTL_TEMP1: {model.EMP} , DEFAULT_TEMP3: {model.CHECK_UNIQUE} ,ADD_TO: {model.UPLOAD_TO}, QTY:{model.CAM_QTY}. TOKEN: {model.USE_TOKEN}','{AuthorizationController.UserIP()}','CONFIRM','{model.ID}'); end;");

                        }

                    }


                }

                //  string strInserUpdate = sb.ToString();
                /*   sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                   sbLog.Append(" VALUES ( ");
                   sbLog.Append($" '{model.EMP}', ");
                   sbLog.Append($" 'CONFIG', ");
                   sbLog.Append($" '{actionString}', ");
                   sbLog.Append($"  'Config64 MODEL_NAME: {model.MODEL_NAME} ,CUSTOMER_NAME: {cust_col[0]}, SFC_COLUMN: {cust_col[1].Replace("'", "''")}, DEFAULT_PREFIX: {model.DEFAULT_PREFIX}, DEFAULT_POSTFIX:  {model.DEFAULT_POSTFIX}, DEFAULT_STRING: {model.DEFAULT_STRING},DEFAULT_LENGHT: {model.DEFAULT_LENGHT} , DATA_LOCATION: {model.DATA_LOCATION} ,DEFAUTL_TEMP1: {model.EMP} , DEFAULT_TEMP3: {model.CHECK_UNIQUE} ,ADD_TO: {model.UPLOAD_TO}, QTY:{model.CAM_QTY}. TOKEN: {model.USE_TOKEN}, IP: {AuthorizationController.UserIP()} TABLE: FIS1.C_TMM_REPORT_T' ");
                   sbLog.Append(" ) ");
                   string strInsertLog = sbLog.ToString();
                   

                   //DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log*/
                string strInserUpdate = sb.ToString();
                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);
                //Execute insert update config 6
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }



        }

        // Copy data from old_model=> new_model

        [System.Web.Http.Route("CopyModel_TmmReport")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> CopyModel_TmmReport(CopyModel_TmmReportt model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                string actionString = " ";
                string strexist = "";

                //check exist
                strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'TMM_REPORT_ADD' AND EMP='{model.EMP}'";

                if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                }

                strexist = $" SELECT * FROM SFIS1.c_tmm_report_t  WHERE  MODEL_NAME = '{model.MODEL_NAME}' ";

                if (DBConnect.GetData(strexist, model.database_name).Rows.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "exists" });
                }


                sb.Append($" Begin  SFIS1.P_COPY_MODEL_TMMREPORT_T( '{model.MODEL_NAME_OLD}','{model.MODEL_NAME}','{model.EMP}'); end;");

                actionString = "COPY";


                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config64  MODEL_NAME_OLD:  {model.MODEL_NAME_OLD},  MODEL_NAME_NEW: {model.MODEL_NAME}, IP:{AuthorizationController.UserIP()} ;' ");
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
