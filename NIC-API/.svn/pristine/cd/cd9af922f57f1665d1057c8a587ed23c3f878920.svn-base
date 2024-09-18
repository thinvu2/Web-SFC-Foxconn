using SN_API.Models;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ModelDestT2Controller : ApiController
    {
        [System.Web.Http.Route("getDataModelDestT2")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> getDataModelDestT2(ModelDestT2Model model)
        {
            string strGetData = "";
            if(!string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = String.Format("select * from( select A.Model_name ,B.MODEL_SERIAL, A.VERSION_CODE,A.PRODUCT_NAME LABEL_TYPE, A.CUST_KP MATERIAL, " +
                    " A.QTY , A.MODEL_TYPE MAC_ON_SFIS,A.CUSTOMER MAC_STEP,A.STANDARD PREFIX, A.MODEL_RANGE2 LENGHT, A.END_GROUP POSTFIX, ROWIDTOCHAR(A.ROWID) ID" +
                    " from SFIS1.C_MODEL_DESC_T2 A," +
                    " SFIS1.C_MODEL_DESC_T B" +
                    " WHERE A.MODEL_NAME = B.MODEL_NAME(+) and A.MODEL_NAME like '{0}%') order by MODEL_NAME,VERSION_CODE ", model.MODEL_NAME.ToUpper());
            }
            else
            {
                strGetData = String.Format("select * from(select A.Model_name ,B.MODEL_SERIAL, A.VERSION_CODE,A.PRODUCT_NAME LABEL_TYPE, A.CUST_KP MATERIAL, " +
                    " A.QTY , A.MODEL_TYPE MAC_ON_SFIS,A.CUSTOMER MAC_STEP,A.STANDARD PREFIX, A.MODEL_RANGE2 LENGHT, A.END_GROUP POSTFIX, ROWIDTOCHAR(A.ROWID) ID" +
                    " from SFIS1.C_MODEL_DESC_T2 A," +
                    " SFIS1.C_MODEL_DESC_T B" +
                    " WHERE A.MODEL_NAME = B.MODEL_NAME(+)) order by MODEL_NAME,VERSION_CODE");
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
        [System.Web.Http.Route("getDataModelDest")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> getDataModelDest(ModelDestT2Model model)
        {
            string strGetData = "";
            strGetData = $" select MODEL_NAME from SFIS1.C_MODEL_DESC_T ";

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
        [System.Web.Http.Route("InsertUpdateDataModelDestT2")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateDataModelDestT2(ModelDestT2Model model)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string strPrivilege = "";
                string actionString = " ";
                if (model.ID == "")
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'MODELDESTT2' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    // not exist => insert
                    sb.Append(" INSERT INTO SFIS1.C_MODEL_DESC_T2 ");
                    sb.Append($" ( MODEL_NAME,MODEL_SERIAL,VERSION_CODE,VERSION_DIFFERENCE,CUSTOMER,MODEL_TYPE,STANDARD,PRODUCT_NAME,CUST_KP,MODEL_RANGE2,QTY,END_GROUP )  VALUES  ( '{model.MODEL_NAME}','LANDIS_GYR','{model.VERSION_CODE}','{model.VERSION_DIFFERENCE}','{model.CUSTOMER}',1,'{model.STANDARD}' ,'{model.PRODUCT_NAME}','{model.CUST_KP}','{model.MODEL_RANGE2}','{model.QTY}','{model.END_GROUP}')  ");
                    actionString = "INSERT";
                    sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                    sbLog.Append(" VALUES ( ");
                    sbLog.Append($" '{model.EMP}', ");
                    sbLog.Append($" 'CONFIG', ");
                    sbLog.Append($" 'INSERT', ");
                    sbLog.Append($" 'TABLE: SFIS1.C_MODEL_DESC_T2' ");
                    sbLog.Append(" ) ");
                    string strInsertLog = sbLog.ToString();
                    DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);
                }
                else
                {
                    //check privilege
                    strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'MODELDESTT2' AND EMP='{model.EMP}'";
                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" UPDATE SFIS1.C_MODEL_DESC_T2 ");
                    sb.Append(" SET ");
                    sb.Append($" CUST_KP = '{model.CUST_KP}', "); //CUST_KP
                    sb.Append($" STANDARD = '{model.STANDARD}', "); //STANDARD                    
                    sb.Append($" VERSION_CODE = '{model.VERSION_CODE}', "); //VERSION_CODE
                    //sb.Append($" MODEL_TYPE = 1, "); //MODEL_TYPE
                    sb.Append($" CUSTOMER = '{model.CUSTOMER}', "); //STANDARD               
                    sb.Append($" MODEL_RANGE2 = '{model.MODEL_RANGE2}', "); //MODEL_RANGE2
                    sb.Append($" PRODUCT_NAME = '{model.PRODUCT_NAME}', "); //PRODUCT_NAME
                    sb.Append($" QTY = '{model.QTY}', "); //MODEL_RANGE2
                    sb.Append($" END_GROUP = '{model.END_GROUP}', "); //END_GROUP
                    sb.Append($" VERSION_DIFFERENCE = '{model.VERSION_DIFFERENCE}' "); //VERSION_DIFFERENCE
                    sb.Append($" WHERE ROWID = '{model.ID}' "); //ID
                    
                    string data = string.Format(@"select 'MODEL_NAME:'||MODEL_NAME||',VERSION_CODE:'||VERSION_CODE||',VERSION_DIFFERENCE:'||VERSION_DIFFERENCE||',CUSTOMER:'||CUSTOMER||'MODEL_TYPE:'||MODEL_TYPE||',STANDARD:'||STANDARD||',PRODUCT_NAME:'||PRODUCT_NAME||',CUST_KP:'||CUST_KP||',MODEL_RANGE2:'||MODEL_RANGE2||',END_GROUP:'||END_GROUP AS LOG from SFIS1.C_MODEL_DESC_T2  WHERE ROWID = '{0}'", model.ID);
                    DataTable dtCheck = DBConnect.GetData(data, model.database_name);
                    
                    sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                    sbLog.Append(" VALUES ( ");
                    sbLog.Append($" '{model.EMP}', ");
                    sbLog.Append($" 'CONFIG', ");
                    sbLog.Append($" 'UPDATE', ");
                    sbLog.Append($" '{dtCheck.Rows[0]["log"].ToString()};  TABLE: SFIS1.C_MODEL_DESC_T2' ");
                    sbLog.Append(" ) ");
                    string strInsertLog = sbLog.ToString();
                    DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);
                }
                string strInserUpdate = sb.ToString();
                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
        [System.Web.Http.Route("DeleteDataModelDestT2")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteDataModelDestT2(ModelDestT2Model model)
        {
            //check privilege
            StringBuilder sbLog = new StringBuilder();
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'MODELDESTT2' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }
            string strDelete = $" delete SFIS1.C_MODEL_DESC_T2 where  ROWID = '{model.ID}' ";
            try
            {
                string data = string.Format(@"select 'MODEL_NAME:'||MODEL_NAME||',VERSION_CODE:'||VERSION_CODE||',VERSION_CODE:'||VERSION_CODE||',VERSION_DIFFERENCE:'||VERSION_DIFFERENCE||',CUSTOMER:'||CUSTOMER||'MODEL_TYPE:'||MODEL_TYPE||',STANDARD:'||STANDARD||',PRODUCT_NAME:'||PRODUCT_NAME||',CUST_KP:'||CUST_KP||',MODEL_RANGE2:'||MODEL_RANGE2||',END_GROUP:'||END_GROUP AS LOG from SFIS1.C_MODEL_DESC_T2  WHERE ROWID = '{0}'", model.ID);
                DataTable dtCheck = DBConnect.GetData(data, model.database_name);
                
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($" '{dtCheck.Rows[0]["log"].ToString()};  TABLE: SFIS1.C_MODEL_DESC_T2' ");
                sbLog.Append(" ) ");
                string strInsertLog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
    }
}