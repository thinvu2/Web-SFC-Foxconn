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
    public class Config43Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig43Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig43Content(Config43Element model)
        {
            string strGetData = "";
            string strGetData_detail = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $" SELECT A.MODEL_NAME, A.VERSION_CODE, A.MO_TYPE FROM SFIS1.C_PACK_SEQUENCE_T A where rownum<70  "
                    + " GROUP BY MODEL_NAME, VERSION_CODE,MO_TYPE ORDER BY A.MODEL_NAME, A.VERSION_CODE, A.MO_TYPE";
            }
            else
            {
                strGetData = $" SELECT A.MODEL_NAME, A.VERSION_CODE, A.MO_TYPE FROM SFIS1.C_PACK_SEQUENCE_T A WHERE upper(MODEL_NAME) like '{model.MODEL_NAME.ToUpper()}%'";
                if (!string.IsNullOrEmpty(model.VERSION_CODE))
                {
                    strGetData = strGetData + $" AND VERSION_CODE='{model.VERSION_CODE}'";
                }
                if (!string.IsNullOrEmpty(model.MO_TYPE))
                {
                    strGetData = strGetData + $" AND MO_TYPE='{model.MO_TYPE}'";
                }
                strGetData = strGetData + $" GROUP BY MODEL_NAME, VERSION_CODE,MO_TYPE ORDER BY A.MODEL_NAME, A.VERSION_CODE, A.MO_TYPE";
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


        [System.Web.Http.Route("GetConfig43DataDetail")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig43DataDetail(Config43Element model)
        {
            string strGetData_detail = "";
            string strgetdatacustsn = "";
            if (!string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData_detail = $"  SELECT SCAN_SEQ,CUSTSN_NAME FROM SFIS1.C_PACK_SEQUENCE_T  WHERE upper(MODEL_NAME)='{model.MODEL_NAME.ToUpper()}'"
               + $" AND VERSION_CODE='{model.VERSION_CODE}' AND MO_TYPE='{model.MO_TYPE}' ORDER BY SCAN_SEQ ASC";


            }

            DataTable dtdetail = DBConnect.GetData(strGetData_detail, model.database_name);
            if (dtdetail.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                strgetdatacustsn = $"  SELECT CUSTSN_NAME VALUE FROM SFIS1.C_PACK_SEQUENCE_T  WHERE upper(MODEL_NAME)='{model.MODEL_NAME.ToUpper()}'"
               + $" AND VERSION_CODE='{model.VERSION_CODE}' AND MO_TYPE='{model.MO_TYPE}' ORDER BY SCAN_SEQ ASC";
                DataTable dt = DBConnect.GetData(strgetdatacustsn, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtdetail, data1 = dt });
            }
        }
        [System.Web.Http.Route("GetCustSnName")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetCustSnName(Config43Element model)
        {
            string strgetdata = "SELECT DISTINCT CUSTSN_NAME VALUE FROM SFIS1.C_CUSTSN_DESC_T ORDER BY CUSTSN_NAME ASC";
            DataTable dt = DBConnect.GetData(strgetdata, model.database_name);
            if (dt.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dt });
            }

        }

        [System.Web.Http.Route("DeleteConfig43")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig43(Config43Element model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG' AND FUN = 'PACK SCAN STEP_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }

            string strDelete = $" DELETE SFIS1.C_PACK_SEQUENCE_T WHERE MODEL_NAME = '{model.MODEL_NAME}' AND VERSION_CODE = '{model.VERSION_CODE}' AND MO_TYPE = '{model.MO_TYPE}'";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($" 'Config43 MODEL_NAME: {model.MODEL_NAME};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_PACK_SEQUENCE_T' ");
                sbLog.Append(" ) ");

                string strLog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strLog, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
        [System.Web.Http.Route("GetMoTypeR105")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetMoTypeR105(Config43Element model)
        {
            string getdata = $" select distinct MO_TYPE from sfism4.r105 ";
            DataTable dt = DBConnect.GetData(getdata, model.database_name);
            if (dt.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dt });
            }
        }

        [System.Web.Http.Route("InsertUpdateConfig43")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig43(Config43Element model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string actionString = " ";
                string strInsertUpdate = " ";
                model.SCAN_SEQ = 1;
                //check exist
                string strCheckExist = $" SELECT MODEL_NAME FROM SFIS1.C_PACK_SEQUENCE_T WHERE MODEL_NAME = '{model.MODEL_NAME}' and VERSION_CODE='{model.VERSION_CODE}' and MO_TYPE='{model.MO_TYPE}' ";
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    //check privilage
                    string strPrivilege = $" SELECT * FROM sfis1.C_PRIVILEGE where PRG_NAME='CONFIG' AND FUN = 'PACK SCAN STEP_ADD' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    actionString = "INSERT";

                    //not exist => insert
                    if (model.listCUSTSN_NAME != null)
                    {
                        for (int i = 0; i < model.listCUSTSN_NAME.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(model.listCUSTSN_NAME[i].VALUE))
                            {
                                strInsertUpdate = "";
                                sb.Clear();
                                model.CUSTSN_NAME = model.listCUSTSN_NAME[i].VALUE;
                                model.SCAN_SEQ = i + 1;
                                sb.Append($" INSERT INTO ");
                                sb.Append($"  SFIS1.C_PACK_SEQUENCE_T(");
                                sb.Append($" MODEL_NAME, VERSION_CODE, MO_TYPE, SCAN_SEQ, CUSTSN_NAME)");
                                sb.Append($" VALUES (");
                                sb.Append($" '{model.MODEL_NAME}', '{model.VERSION_CODE}', '{model.MO_TYPE}', '{model.SCAN_SEQ}', '{model.CUSTSN_NAME}' ");
                                sb.Append(" ) ");

                                strInsertUpdate = sb.ToString();
                                DBConnect.ExecuteNoneQuery(strInsertUpdate, model.database_name);

                            }
                        }
                    }

                    //insert log
                    sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                    sbLog.Append(" VALUES ( ");
                    sbLog.Append($" '{model.EMP}', ");
                    sbLog.Append($" 'CONFIG', ");
                    sbLog.Append($" '{actionString}', ");
                    sbLog.Append($" 'Config43 MODEL_NAME: {model.MODEL_NAME};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_PACK_SEQUENCE_T' ");
                    sbLog.Append(" ) ");

                    string strLog = sbLog.ToString();
                    DBConnect.ExecuteNoneQuery(strLog, model.database_name);
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
                }
                else
                {
                    //check privilage
                    string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PACK SCAN STEP_EDIT' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    try
                    {
                        //delete
                        string strDelete = $" DELETE  SFIS1.C_PACK_SEQUENCE_T WHERE MODEL_NAME = '{model.MODEL_NAME}' AND VERSION_CODE = '{model.VERSION_CODE}' AND MO_TYPE = '{model.MO_TYPE}'";
                        DBConnect.ExecuteNoneQuery(strDelete, model.database_name);

                        //not exist => insert
                        actionString = "UPDATE";
                        if (model.listCUSTSN_NAME != null)
                        {
                            for (int i = 0; i < model.listCUSTSN_NAME.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(model.listCUSTSN_NAME[i].VALUE))
                                {
                                    strInsertUpdate = "";
                                    sb.Clear();
                                    model.CUSTSN_NAME = model.listCUSTSN_NAME[i].VALUE;
                                    model.SCAN_SEQ = i + 1;
                                    sb.Append($" INSERT INTO ");
                                    sb.Append($"  SFIS1.C_PACK_SEQUENCE_T(");
                                    sb.Append($" MODEL_NAME, VERSION_CODE, MO_TYPE, SCAN_SEQ, CUSTSN_NAME)");
                                    sb.Append($" VALUES (");
                                    sb.Append($" '{model.MODEL_NAME}', '{model.VERSION_CODE}', '{model.MO_TYPE}', '{model.SCAN_SEQ}', '{model.CUSTSN_NAME}' ");
                                    sb.Append(" ) ");

                                    strInsertUpdate = sb.ToString();
                                    DBConnect.ExecuteNoneQuery(strInsertUpdate, model.database_name);
                                }
                            }
                        }



                        //insert log
                        sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                        sbLog.Append(" VALUES ( ");
                        sbLog.Append($" '{model.EMP}', ");
                        sbLog.Append($" 'CONFIG', ");
                        sbLog.Append($" '{actionString}', ");
                        sbLog.Append($" 'Config43 MODEL_NAME: {model.MODEL_NAME};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_PACK_SEQUENCE_T' ");
                        sbLog.Append(" ) ");

                        string strLog = sbLog.ToString();
                        DBConnect.ExecuteNoneQuery(strLog, model.database_name);
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
                    }
                    catch (Exception ex)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
    }
}