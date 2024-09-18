
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
    public class Config12Controller : ApiController
    {
        // GET : Config
        [System.Web.Http.Route("GetConfig12Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig12Content(Config12Element model)
        {
            string strGetData = "";
            string strGetBom = "";
            if (string.IsNullOrEmpty(model.BOM_NO))
            {
                strGetData = $"select DISTINCT BOM_NO FROM SFIS1.C_BOM_KEYPART_T";
            }
            else
            {
                strGetData = $"select A.bom_no,A.key_part_no,A.kp_relation,A.kp_count,A.work_date,A.group_name,A.version_code,A.type,ROWIDTOCHAR(ROWID) ID "
                        + $" FROM SFIS1.C_BOM_KEYPART_T A where upper(A.BOM_NO) = '{model.BOM_NO.ToUpper()}'";

            }

            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                if (!string.IsNullOrEmpty(model.BOM_NO))
                {
                    strGetBom = $"select DISTINCT BOM_NO FROM SFIS1.C_BOM_KEYPART_T  where upper(BOM_NO) = '{model.BOM_NO.ToUpper()}' ";
                    DataTable dtBom = DBConnect.GetData(strGetBom, model.database_name);
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtBom, data1 = dtCheck });
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }

        [System.Web.Http.Route("GetKeypartContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetKeypartContent(Config11Element _keypart)
        {
            string strGetData = "";
            strGetData = "SELECT DISTINCT KEY_PART_NO FROM SFIS1.C_KEYPARTS_DESC_T ";

            DataTable dtCheck = DBConnect.GetData(strGetData, _keypart.database_name);
            if (dtCheck.Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }

        [System.Web.Http.Route("GetOptionLink")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetOptionLink(Config11Element model)
        {
            string strGetData = "";
            strGetData = "select DISTINCT GROUP_NAME from sfis1.C_GROUP_CONFIG_T";

            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            if (dtCheck.Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }
        [System.Web.Http.Route("DeleteConfig12")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig12(Config12Element model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'BOM_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }

            string strDelete = $"delete FROM SFIS1.C_BOM_KEYPART_T where ROWID='{model.ID}'";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'Config12 BOM_NO: {model.BOM_NO}, KEY_PART_NO: {model.KEY_PART_NO}, KP_RELATION: {model.KP_RELATION}, KP_COUNT: {model.KP_COUNT}, GROUP_NAME: {model.GROUP_NAME}, VERSION_CODE: {model.OPTION_LINK}, IP: {AuthorizationController.UserIP()}' ");
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


        [System.Web.Http.Route("DeleteKeypart")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteKeypart(Config12Element model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'KEYPART NO_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }
            string strDelete = $" delete SFIS1.C_KEYPARTS_DESC_T where  KEY_PART_NO = '{model.KEY_PART_NO}' and BOM_NO= '{model.BOM_NO}'";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'Config12 BOM_NO: {model.BOM_NO}, KEY_PART_NO: {model.KEY_PART_NO}, KP_RELATION: {model.KP_RELATION}, KP_COUNT: {model.KP_COUNT}, GROUP_NAME: {model.GROUP_NAME}, VERSION_CODE: {model.OPTION_LINK}, IP: {AuthorizationController.UserIP()}'");
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


        [System.Web.Http.Route("InsertUpdateConfig12")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig12(Config12Element model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                string checkBomExixt = "";
                int kp_count = 1;

                //check exist
                string strCheckExist = $" select BOM_NO from SFIS1.C_BOM_KEYPART_T where bom_no = '{model.BOM_NO}'";
                string actionString = " ";
                if (model.type == "add")
                {
                    //check privilege
                    strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'BOM_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });

                    }
                    checkBomExixt = $" SELECT * FROM SFIS1.C_BOM_KEYPART_T WHERE BOM_NO = '{model.BOM_NAME}'";
                    if (DBConnect.GetData(checkBomExixt, model.database_name).Rows.Count > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "BOM ALREADY EXISTS" });

                    }

                    actionString = "INSERT NEW BOM";

                    if (string.IsNullOrEmpty(model.BOM_COPY))
                    {
                        sb.Append(" INSERT into  SFIS1.C_BOM_KEYPART_T");
                        sb.Append($" (BOM_NO,KEY_PART_NO,KP_RELATION,KP_COUNT,WORK_DATE,GROUP_NAME,VERSION_CODE,TYPE)");
                        sb.Append($" Values(");
                        sb.Append($"'{model.BOM_NAME}','{model.KEY_PART_NO}','{model.KP_RELATION}','{model.KP_COUNT}','{model.WORK_DATE}','{model.GROUP_NAME}','{model.VERSION_CODE}','{model.TYPE}')");
                    }
                    else
                    {
                        sb.Append("INSERT INTO SFIS1.C_BOM_KEYPART_T ");
                        sb.Append("(BOM_NO,KEY_PART_NO,KP_RELATION,KP_COUNT,GROUP_NAME,VERSION_CODE) ");
                        sb.Append($"SELECT '{model.BOM_NAME}',KEY_PART_NO,KP_RELATION,KP_COUNT,GROUP_NAME,VERSION_CODE ");
                        sb.Append($" FROM SFIS1.C_BOM_KEYPART_T ");
                        sb.Append($"WHERE BOM_NO='{model.BOM_COPY}' ");
                    }

                    sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                    sbLog.Append(" VALUES ( ");
                    sbLog.Append($" '{model.EMP}', ");
                    sbLog.Append($" 'CONFIG', ");
                    sbLog.Append($" '{actionString}', ");
                    sbLog.Append($" 'Config12 BOM_NO: {model.BOM_NAME}, BOM_COPY: {model.BOM_COPY}, IP: {AuthorizationController.UserIP()}'");
                    sbLog.Append(" ) ");

                }
                else
                {
                    strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'BOM_EDIT' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    string check_bom = $"SELECT KP_RELATION FROM SFIS1.C_BOM_KEYPART_T WHERE BOM_NO='{model.BOM_NO}' and KEY_PART_NO='{model.KEY_PART_NO}' order by KP_RELATION asc";

                    DataTable dt = DBConnect.GetData(check_bom, model.database_name);

                    if (dt.Rows.Count > 0)
                    {
                        model.KP_RELATION = Int32.Parse(dt.Rows[0]["KP_RELATION"].ToString());// + kp_count;
                    }
                    else
                    {
                        string getMaxKpQuery = $"SELECT MAX(KP_RELATION) FROM SFIS1.C_BOM_KEYPART_T WHERE BOM_NO='{model.BOM_NO}'";
                        DataTable dtMaxKp = DBConnect.GetData(getMaxKpQuery, model.database_name);
                        if (dtMaxKp.Rows.Count > 0 && dtMaxKp.Rows[0][0] != DBNull.Value)
                        {
                            model.KP_RELATION = Convert.ToInt32(dtMaxKp.Rows[0][0]) + 1;
                        }
                        else
                        {
                            model.KP_RELATION = kp_count;

                        }
                    }

                    actionString = "INSERT";
                    if (string.IsNullOrEmpty(model.ID))
                    {
                        if (string.IsNullOrEmpty(model.BOM_NO))
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "PLEASE CHOOSE BOM" });
                        }

                        string check_dup = $"SELECT * FROM SFIS1.C_BOM_KEYPART_T WHERE BOM_NO='{model.BOM_NO}' and KEY_PART_NO='{model.KEY_PART_NO}' ";
                        DataTable dt_check_dup = DBConnect.GetData(check_dup, model.database_name);
                        if (dt_check_dup.Rows.Count > 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = $"Ðã t?n t?i key_part_no  {model.KEY_PART_NO} trong Bom {model.BOM_NO}\nKey_part_no already exists" });
                        }
                        if (!string.IsNullOrWhiteSpace(model.GROUP_NAME))
                        {
                            check_dup = $"SELECT * FROM SFIS1.C_BOM_KEYPART_T WHERE BOM_NO='{model.BOM_NO}' and GROUP_NAME='{model.GROUP_NAME}' and KEY_PART_NO='{model.KEY_PART_NO}'";
                            dt_check_dup = DBConnect.GetData(check_dup, model.database_name);
                            if (dt_check_dup.Rows.Count > 0)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, new { result = $"Ðã t?n t?i GROUP_NAME  {model.GROUP_NAME} trong Bom {model.BOM_NO}\nGROUP_NAME already exists" });
                            }
                        }

                        sb.Append("INSERT INTO SFIS1.C_BOM_KEYPART_T ");
                        sb.Append($"(BOM_NO,KEY_PART_NO,KP_RELATION,KP_COUNT,GROUP_NAME,VERSION_CODE,TYPE)");
                        sb.Append($" VALUES ('{model.BOM_NO}','{model.KEY_PART_NO}',{model.KP_RELATION},{model.KP_COUNT},'{model.GROUP_NAME}','{model.OPTION_LINK}','')");

                        sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                        sbLog.Append(" VALUES ( ");
                        sbLog.Append($" '{model.EMP}', ");
                        sbLog.Append($" 'CONFIG', ");
                        sbLog.Append($" '{actionString}', ");
                        sbLog.Append($" 'Config12 BOM_NO: {model.BOM_NO}, KEY_PART_NO: {model.KEY_PART_NO}, KP_RELATION: {model.KP_RELATION}, KP_COUNT: {model.KP_COUNT}, GROUP_NAME: {model.GROUP_NAME}, VERSION_CODE: {model.OPTION_LINK}, IP: {AuthorizationController.UserIP()}'");
                        sbLog.Append(" ) ");
                    }

                    else if (model.type == "edit")
                    {

                        string getOldValueQuery = $"SELECT KEY_PART_NO,group_name,version_code FROM SFIS1.C_BOM_KEYPART_T WHERE BOM_NO = '{model.BOM_NO}' AND ROWID = '{model.ID}'";
                        DataTable dtOldValue = DBConnect.GetData(getOldValueQuery, model.database_name);
                        string oldKeyPartNo = (dtOldValue.Rows.Count > 0) ? dtOldValue.Rows[0]["KEY_PART_NO"].ToString() : "";
                        string oldGroupName = (dtOldValue.Rows.Count > 0) ? dtOldValue.Rows[0]["GROUP_NAME"].ToString() : "";
                        string oldVersionCode = (dtOldValue.Rows.Count > 0) ? dtOldValue.Rows[0]["VERSION_CODE"].ToString() : "";


                        actionString = "UPDATE TO";
                        if (model.GROUP_NAME == "CPS")
                        {
                            if (string.IsNullOrEmpty(model.OPTION_LINK))
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, new { result = "VERSION_CODE is not empty" });
                            }
                            else
                            {
                                sb.Append(" UPDATE SFIS1.C_BOM_KEYPART_T");
                                sb.Append($" SET ");
                                sb.Append($" KEY_PART_NO = '{model.KEY_PART_NO}',");
                                sb.Append($" KP_RELATION = {model.KP_RELATION}, ");
                                sb.Append($" KP_COUNT = {model.KP_COUNT},");
                                sb.Append($" GROUP_NAME = '{model.GROUP_NAME}',");
                                sb.Append($" VERSION_CODE = '{model.OPTION_LINK}'");
                                sb.Append($" WHERE BOM_NO = '{model.BOM_NO}' AND KEY_PART_NO = '{oldKeyPartNo}'");

                            }
                            sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                            sbLog.Append(" VALUES ( ");
                            sbLog.Append($" '{model.EMP}', ");
                            sbLog.Append($" 'CONFIG', ");
                            sbLog.Append($" '{actionString}', ");
                            sbLog.Append($" 'Config12 BOM_NO: {model.BOM_NO}, KEY_PART_NO_NEW: {model.KEY_PART_NO}, KEY_PART_NO_OLD: {oldKeyPartNo}, KP_RELATION: {model.KP_RELATION}, KP_COUNT: {model.KP_COUNT}, GROUP_NAME_NEW: {model.GROUP_NAME}, GROUP_NAME_OLD: {oldGroupName}, VERSION_CODE_NEW: {model.OPTION_LINK} ,VERSION_CODE_OLD: {oldVersionCode}, IP: {AuthorizationController.UserIP()}'");
                            sbLog.Append(" ) ");
                        }
                        else
                        {
                            sb.Append(" UPDATE SFIS1.C_BOM_KEYPART_T");
                            sb.Append($" SET ");
                            sb.Append($" KEY_PART_NO = '{model.KEY_PART_NO}',");
                            sb.Append($" KP_RELATION = {model.KP_RELATION}, ");
                            sb.Append($" KP_COUNT = {model.KP_COUNT},");
                            sb.Append($" GROUP_NAME = '{model.GROUP_NAME}',");
                            sb.Append($" VERSION_CODE = ''");
                            sb.Append($" WHERE BOM_NO = '{model.BOM_NO}' AND KEY_PART_NO = '{oldKeyPartNo}'");

                            sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                            sbLog.Append(" VALUES ( ");
                            sbLog.Append($" '{model.EMP}', ");
                            sbLog.Append($" 'CONFIG', ");
                            sbLog.Append($" '{actionString}', ");
                            sbLog.Append($" 'Config12 BOM_NO: {model.BOM_NO}, KEY_PART_NO_NEW: {model.KEY_PART_NO}, KEY_PART_NO_OLD: {oldKeyPartNo}, KP_RELATION: {model.KP_RELATION}, KP_COUNT: {model.KP_COUNT}, GROUP_NAME_NEW: {model.GROUP_NAME}, GROUP_NAME_OLD: {oldGroupName}, VERSION_CODE_NEW: ,VERSION_CODE_OLD: {oldVersionCode}, IP: {AuthorizationController.UserIP()}" +
                                $"'");
                            sbLog.Append(" ) ");
                        }
                    }
                }
                string strInsertLog = sbLog.ToString();
                string strInserUpdate = sb.ToString();
                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config 12
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
