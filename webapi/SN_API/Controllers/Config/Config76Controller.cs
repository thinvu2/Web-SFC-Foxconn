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
    public class Config76Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig76Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig76Content(Config76Element model)
        {
            // check GWCPEII_CONFIG 
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MO_NUMBER))
            {
                strGetData = $" select a.model_type MO_NUMBER,a.TYPE_FLAG GROUP_NAME,a.TYPE_DESC STATUS,a.EMP_NO,a.CREATE_DATE,a.rowid ID from sfis1.C_MODEL_CONFIRM_T a where a.type_name='NPIMOCONFIG' and rownum <=20 ";
            }
            else
            {
                strGetData = $"  select a.model_type MO_NUMBER,a.TYPE_FLAG GROUP_NAME,a.TYPE_DESC STATUS,a.EMP_NO,a.CREATE_DATE,a.rowid ID from sfis1.C_MODEL_CONFIRM_T a where a.type_name='NPIMOCONFIG' and   UPPER(a.model_type) LIKE '%{model.MO_NUMBER.ToUpper()}%' ";
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

        [System.Web.Http.Route("Config76GetAllMonumber")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config76GetAllMonumber(Config76Element model)
        {
            string strGetData = "select mo_number from sfism4.r105 where 1 = 1 and MO_CREATE_DATE > to_date('2024','YYYY') order by mo_create_date desc";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }
        /// <summary>
        /// CONFIG FA 
        /// </summary>


        // GET: Config
        [System.Web.Http.Route("GetConfigFAContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfigFAContent(ConfigFAElement model)
        {
            // check GWCPEII_CONFIG 


            string strGetData = "";
            if (string.IsNullOrEmpty(model.MO_NUMBER))
            {
                strGetData = $"  select DISTINCT GROUP_NAME VALUE from SFIS1.C_GROUP_CONFIG_T where SUBSTR(group_name,1,2) NOT IN ('R_') order by GROUP_NAME ";
            }
            else
            {
                strGetData = $@" select DISTINCT GROUP_NAME VALUE from SFIS1.C_GROUP_CONFIG_T where SUBSTR(group_name,1,2) NOT IN ('R_')  
                            minis
                            select TYPE_FLAG value from sfis1.C_MODEL_CONFIRM_T where type_name='NPIMOCONFIG' AND MO_NUMBER = '{model.MO_NUMBER.ToUpper()}' ";
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

        [System.Web.Http.Route("Config76GetTA_VerByMO")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config76GetTA_VerByMO(Config76Element model)
        {

            string strGetData = $" select VENDER_PART_NO vn from sfism4.r105  where mo_number ='{model.MO_NUMBER}'";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });


        }

        [System.Web.Http.Route("Config76GetFA_byMO")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Config76GetFA_byMO(Config76Element model)
        {

            string strGetData = $" select * from table(PKG_RETURN_TABLE.F_GetFAINFOR('{model.MO_NUMBER}')) ";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });


        }

        [System.Web.Http.Route("ConfiFAGetModelNamebyMonumber")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ConfiFAGetModelNamebyMonumber(ConfigFAElement model)
        {

            string strGetData = $" select MODEL_NAME  from SFISM4.r_bpcs_moplan_t where mo_number ='{model.MO_NUMBER}'";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });


        }


        [System.Web.Http.Route("InsertUpdateConfigFA")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfigFA(Config76Element model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                string modify = " ";
                //check exist
                string strCheckExist = $"select * from sfis1.C_MODEL_CONFIRM_T where type_name='NPIMOCONFIG' and model_type = '{model.MO_NUMBER}' and upper(TYPE_FLAG) = '{model.GROUP_NAME.ToUpper()}' ";
                string actionString = " ";
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'NPI MO CONFIG_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    // not exist => insert

                    sb.Append($@" INSERT INTO SFIS1.C_MODEL_CONFIRM_T (MODEL_TYPE,TYPE_NAME,TYPE_FLAG,TYPE_DESC,EMP_NO,CREATE_DATE) VALUES 
                                ('{model.MO_NUMBER}','NPIMOCONFIG','{model.GROUP_NAME}','{model.STATUS}','{model.EMP}',SYSDATE)");


                    actionString = "INSERT";
                }
                else
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'NPI MO CONFIG_EDIT' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //existed => update
                    actionString = "UPDATE";

                    sb.Append($@" UPDATE SFIS1.C_MODEL_CONFIRM_T SET TYPE_DESC = '{model.STATUS}' where type_name='NPIMOCONFIG' and model_type = '{model.MO_NUMBER}' and upper(TYPE_FLAG) = '{model.GROUP_NAME.ToUpper()}' ");

                    modify = " UPDATE: ";
                    string query = $"select TYPE_DESC from SFIS1.C_MODEL_CONFIRM_T WHERE where type_name='NPIMOCONFIG' and model_type = '{model.MO_NUMBER}' and upper(TYPE_FLAG) = '{model.GROUP_NAME.ToUpper()}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);
                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if (row[0].ToString() != model.STATUS)
                        {
                            modify += $" TYPE_DESC: {row[0].ToString()};";
                        }
                    }

                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'NPI MO CONFIG : {model.MO_NUMBER}; {modify}; IP:{AuthorizationController.UserIP()} ; TABLE: SFIS1.C_MODEL_CONFIRM_T' ");
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



        [System.Web.Http.Route("DeleteConfigFA")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfigFA(Config76Element model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'NPI MO CONFIG_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }
            string strDelete = $" delete  from sfis1.c_config_fa_t where mo_number='{model.ID}' ";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'NPI MO CONFIG delete: {model.MO_NUMBER};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_MODEL_CONFIRM_T' ");
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


        [System.Web.Http.Route("InsertUpdateConfirmTA_")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfirmTA_(Config76Element model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                //check exist
                string strCheckExist = $"  select model_type from sfis1.C_MODEL_CONFIRM_T where model_type = '{model.MO_NUMBER}' and type_name='NPIMOCONFIG' ";
                string actionString = " ";
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'NPI MO CONFIG_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    // not exist => insert

                    sb.Append($" Begin  SFIS1.P_CONFIRM_TA_FA_T( '{model.MO_NUMBER}','{model.GROUP_NAME}','{model.STATUS}','{model.EMP}','INSERT'); end;");

                    actionString = "INSERT";
                }
                else
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'NPI MO CONFIG_EDIT' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    //existed => update
                    actionString = "UPDATE";

                    sb.Append($" Begin  SFIS1.P_CONFIRM_TA_FA_T( '{model.MO_NUMBER}','{model.GROUP_NAME}','{model.STATUS}','{model.EMP}','UPDATE'); end;");

                }
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config76  : {model.MO_NUMBER}; IP:{AuthorizationController.UserIP()} ; TABLE: SFIS1.C_ERROR_CODE_T' ");
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

        [System.Web.Http.Route("DeleteConfigTA_")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfigTA_(Config76Element model)
        {
            StringBuilder sb = new StringBuilder();


            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'NPI MO CONFIG_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }
            string actionString = "UPDATE";


            sb.Append($" Begin  SFIS1.P_CONFIRM_TA_FA_T( '{model.MO_NUMBER}','{model.GROUP_NAME}','{model.STATUS}','{model.EMP}','DELETE'); end;");
            string strDelete = sb.ToString();
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'ConfigFA delete: {model.MO_NUMBER};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_ERROR_CODE_T' ");
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

        //[System.Web.Http.Route("InsertUpdateConfig76")]
        //[System.Web.Http.HttpPost]
        //public async Task<HttpResponseMessage> InsertUpdateConfig76(Config76Element model)
        //{
        //    try
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        StringBuilder sbLog = new StringBuilder();
        //        string strPrivilege = "";
        //        //check exist
        //        string strCheckExist = $"  select MODEL_NAME from SFIS1.C_ITEM_DESC_T where MODEL_NAME = '{model.MODEL_NAME}' AND ITEM_CODE = '{model.ITEM_CODE}' ";
        //        string actionString = " ";
        //        if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
        //        {
        //            //check privilege
        //            strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'LOCATION_ADD' AND EMP='{model.EMP}'";
        //            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
        //            }
        //            // not exist => insert
        //            sb.Append(" INSERT INTO SFIS1.C_ITEM_DESC_T ");
        //            sb.Append($" ( MODEL_CODE,MODEL_NAME,ITEM_CODE,ITEM_SERIAL,ITEM_NAME,ITEM_TYPE,SIDE,MFR_CODE,MFR_NAME )  VALUES  ( '{model.MODEL_CODE}','{model.MODEL_NAME}','{model.ITEM_CODE}','{model.ITEM_SERIAL}','{model.ITEM_NAME}','{model.ITEM_TYPE}','{model.SIDE}','{model.MFR_CODE}','{model.MFR_NAME}' )  ");
        //            actionString = "INSERT";
        //        }
        //        else
        //        {
        //            //check privilege
        //            strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'LOCATION_EDIT' AND EMP='{model.EMP}'";
        //            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
        //            }
        //            //existed => update
        //            actionString = "UPDATE";
        //            sb.Append(" UPDATE SFIS1.C_ITEM_DESC_T ");
        //            sb.Append(" SET ");
        //            sb.Append($" MODEL_CODE = '{model.MODEL_CODE}', "); //MODEL_CODE
        //            sb.Append($" MODEL_NAME = '{model.MODEL_NAME}', "); //MODEL_NAME
        //            sb.Append($" ITEM_CODE = '{model.ITEM_CODE}', "); //ITEM_CODE
        //            sb.Append($" ITEM_SERIAL = '{model.ITEM_SERIAL}', "); //ITEM_SERIAL
        //            sb.Append($" ITEM_NAME = '{model.ITEM_NAME}', "); //ITEM_NAME
        //            sb.Append($" ITEM_TYPE = '{model.ITEM_TYPE}', "); //ITEM_TYPE
        //            sb.Append($" SIDE = '{model.SIDE}', "); //SIDE
        //            sb.Append($" MFR_CODE = '{model.MFR_CODE}', "); //MFR_CODE
        //            sb.Append($" MFR_NAME = '{model.MFR_NAME}' "); //MFR_NAME
        //            sb.Append($" WHERE ROWID = '{model.ID}' "); //ID

        //        }
        //        sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
        //        sbLog.Append(" VALUES ( ");
        //        sbLog.Append($" '{model.EMP}', ");
        //        sbLog.Append($" 'CONFIG', ");
        //        sbLog.Append($" '{actionString}', ");
        //        sbLog.Append($"  'Config76 MODEL_NAME: {model.MODEL_NAME}; ITEM_CODE: {model.ITEM_CODE}; IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_ITEM_DESC_T' ");
        //        sbLog.Append(" ) ");
        //        string strInsertLog = sbLog.ToString();
        //        string strInserUpdate = sb.ToString();
        //        DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config 6
        //        DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
        //        return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
        //    }
        //}
        //[System.Web.Http.Route("DeleteConfig76")]
        //[System.Web.Http.HttpPost]
        //public async Task<HttpResponseMessage> DeleteConfig76(Config76Element model)
        //{
        //    //check privilege
        //    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'LOCATION_DELETE' AND EMP='{model.EMP}'";
        //    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
        //    }
        //    string strDelete = $" delete SFIS1.C_ITEM_DESC_T where  ROWID = '{model.ID}' ";
        //    try
        //    {
        //        DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
        //        StringBuilder sbLog = new StringBuilder();
        //        sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
        //        sbLog.Append(" VALUES ( ");
        //        sbLog.Append($" '{model.EMP}', ");
        //        sbLog.Append($" 'CONFIG', ");
        //        sbLog.Append($" 'DELETE', ");
        //        sbLog.Append($"  'Config76 MODEL_NAME: {model.MODEL_NAME}; ITEM_CODE: {model.ITEM_CODE} IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_ITEM_DESC_T' ");
        //        sbLog.Append(" ) ");

        //        string strInsertLog = sbLog.ToString();
        //        DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);
        //        return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
        //    }
        //}
    }
}