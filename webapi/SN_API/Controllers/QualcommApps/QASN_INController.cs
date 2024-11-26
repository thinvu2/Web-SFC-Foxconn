using SN_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SN_API.Controllers.QualcommApps
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class QASN_INController : ApiController
    {
        //[System.Web.Http.Route("GetWeightTable")]
        //[System.Web.Http.HttpGet]
        //public async Task<HttpResponseMessage> GetWeightTable(string database_name, string PACKSLIP_NO)
        //{
        //    string strGetData = "";
        //    if (string.IsNullOrEmpty(PACKSLIP_NO))
        //    {
        //        strGetData = $"SELECT PACKSLIP_NO, FLAG, F_ID, SHIP_MCMN_CREATTIME, MSG_SENDER_NAME, MSG_RECEIVER_NAME, PO_NO, POLINE_NO, ITEM_NO,ITEM_SHIPPEDQTY, ITEM_UNITOFMEASURE, ITEM_MPN, ITEM_DESCRIPTION, LOT_NO, RECEIVER_LOCATIONNAME, RECEIVER_NAME FROM SFISM4.R_QASN_IN";
        //    }
        //    else
        //    {
        //        strGetData = $"SELECT PACKSLIP_NO, FLAG, F_ID, SHIP_MCMN_CREATTIME, MSG_SENDER_NAME, MSG_RECEIVER_NAME, PO_NO, POLINE_NO, ITEM_NO,ITEM_SHIPPEDQTY, ITEM_UNITOFMEASURE, ITEM_MPN, ITEM_DESCRIPTION, LOT_NO, RECEIVER_LOCATIONNAME, RECEIVER_NAME FROM SFISM4.R_QASN_IN WHERE PACKSLIP_NO = {PACKSLIP_NO}";
        //    }
        //    DataTable dtCheck = DBConnect.GetData(strGetData, database_name);
        //    if (dtCheck.Rows.Count == 0)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        //    }
        //}
        [System.Web.Http.Route("GetTableLPN")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetTableLPN(string database_name, string PACKSLIP_NO)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(PACKSLIP_NO))
            {
                strGetData = $"SELECT PACKSLIP_NO, FLAG, F_ID, SHIP_MCMN_CREATTIME, MSG_SENDER_NAME, MSG_RECEIVER_NAME, PO_NO, POLINE_NO, ITEM_NO,ITEM_SHIPPEDQTY, ITEM_UNITOFMEASURE, ITEM_MPN, ITEM_DESCRIPTION, LOT_NO, RECEIVER_LOCATIONNAME, RECEIVER_NAME FROM SFISM4.R_QASN_IN";
            }
            else
            {
                strGetData = $"SELECT PACKSLIP_NO, FLAG, F_ID, SHIP_MCMN_CREATTIME, MSG_SENDER_NAME, MSG_RECEIVER_NAME, PO_NO, POLINE_NO, ITEM_NO,ITEM_SHIPPEDQTY, ITEM_UNITOFMEASURE, ITEM_MPN, ITEM_DESCRIPTION, LOT_NO, RECEIVER_LOCATIONNAME, RECEIVER_NAME FROM SFISM4.R_QASN_IN WHERE PACKSLIP_NO = {PACKSLIP_NO}";
            }
            DataTable dtCheck = DBConnect.GetData(strGetData, database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }
        [System.Web.Http.Route("GetQASN_IN")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetQASN_IN(string database_name, string PACKSLIP_NO)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(PACKSLIP_NO))
            {
                strGetData = $"SELECT PACKSLIP_NO, FLAG, F_ID, SHIP_MCMN_CREATTIME, MSG_SENDER_NAME, MSG_RECEIVER_NAME, PO_NO, POLINE_NO, ITEM_NO,ITEM_SHIPPEDQTY, ITEM_UNITOFMEASURE, ITEM_MPN, ITEM_DESCRIPTION, LOT_NO, RECEIVER_LOCATIONNAME, RECEIVER_NAME FROM SFISM4.R_QASN_IN";
            }
            else
            {
                strGetData = $"SELECT PACKSLIP_NO, FLAG, F_ID, SHIP_MCMN_CREATTIME, MSG_SENDER_NAME, MSG_RECEIVER_NAME, PO_NO, POLINE_NO, ITEM_NO,ITEM_SHIPPEDQTY, ITEM_UNITOFMEASURE, ITEM_MPN, ITEM_DESCRIPTION, LOT_NO, RECEIVER_LOCATIONNAME, RECEIVER_NAME FROM SFISM4.R_QASN_IN WHERE PACKSLIP_NO = {PACKSLIP_NO}";
            }
            DataTable dtCheck = DBConnect.GetData(strGetData, database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }

        [System.Web.Http.Route("GetShowDetail")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetShowDetail(string database_name, string PACKSLIP_NO, string FLAG, string F_ID)
        {
            string strGetData = "";
            string strGetDataTableOuterLPN = "";
            string strGetDataTableInnerLPN = "";
            string strGetReceiveAddress = "";
            strGetData = $"select PACKSLIP_NO, SHIP_MCMN_CREATTIME, MSG_SENDER_NAME, MSG_RECEIVER_NAME, PACKSLIP_NO, AIRWAYBILL, PO_NO, COUNTRY_CODE, POSTAL_CODE, SHIP_MCMN_CITY, SHIP_MCMN_STREET1, SHIP_MCMN_STREET2, SHIP_MCMN_STREET3, SHIP_MCMN_LPN, PAL_GROSSWEIGHT, PAL_NETWEIGHT, PAL_WIDTH, PAL_LENGTH, PAL_HEIGHT, ITEM_NO, ITEM_SHIPPEDQTY, ITEM_MPN FROM SFISM4.R_QASN_IN where PACKSLIP_NO = '{PACKSLIP_NO}' and FLAG = '{FLAG}' and F_ID = '{F_ID}' ";
            strGetDataTableOuterLPN = $"select PACKSLIP_NO, flag, OUTERBOX_LPN, OUTERBOX_GROSSWEIGHT, OUTERBOX_NETWEIGHT, OUTERBOX_WEIGHTUNITOFMEASURE, INNERBOX_WEIGHTUNITOFMEASURE, count(*) as OUTERBOX_qty from SFISM4.R_QASN_IN where PACKSLIP_NO = '{PACKSLIP_NO}' and FLAG = '{FLAG}' group by PACKSLIP_NO, flag, OUTERBOX_LPN, OUTERBOX_GROSSWEIGHT, OUTERBOX_NETWEIGHT, OUTERBOX_WEIGHTUNITOFMEASURE, INNERBOX_WEIGHTUNITOFMEASURE";
            strGetDataTableInnerLPN = $"select INNERBOX_LPN, 'LPN' LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT, count(*) as INNERBOX_qty from SFISM4.R_QASN_IN  where PACKSLIP_NO = '{PACKSLIP_NO}' group by INNERBOX_LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT";
            strGetReceiveAddress = $"select SHIP_ADDRESS from SFISM4.R_SAP_DN_DETAIL_T where model_name ='{PACKSLIP_NO}'";
            DataTable dtCheck = DBConnect.GetData(strGetData, database_name);
            DataTable dtCheckTableOuterLPN = DBConnect.GetData(strGetDataTableOuterLPN, database_name);
            DataTable dtCheckTableInnerLPN = DBConnect.GetData(strGetDataTableInnerLPN, database_name);
            DataTable dtCheckReceiveAddress = DBConnect.GetData(strGetReceiveAddress, database_name);
            if (dtCheck.Rows.Count == 0 || dtCheckTableOuterLPN.Rows.Count == 0 || dtCheckTableInnerLPN.Rows.Count == 0 || dtCheckReceiveAddress.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck , dataTableOuterLPN = dtCheckTableOuterLPN, dataTableInnerLPN = dtCheckTableInnerLPN, dataReceiveAddress = dtCheckReceiveAddress});
            }
        }

        [System.Web.Http.Route("InsertQasnIn")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertQasnIn(QASN_IN_Element model)
        {
            try
            {
                //check privilege
                //string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'MODEL_EDIT' AND EMP='{model.EMP_NO}'";
                //if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                //{
                //    return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                //}
                //check exist
                //string strCheckExist = $" select model_name from SFIS1.C_MODEL_DESC_T where model_name = '{model.MODEL_NAME}' ";
                string actionString = "";
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                //if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                //{
                    // not exist => insert
                    sb.Append(" Insert into SFISM4.R_QRECEIPT_OUT ");
                    sb.Append(" (F_ID, MSG_RECEIVER_DUNS, MSG_SENDER_DUNS, MSG_RECEIVER_NAME, MSG_SENDER_NAME, PACKSLIP_NO) ");
                    sb.Append(" values ( ");
                    sb.Append($" '0', ");
                    sb.Append($" '{model.Total_Received_Qty}', ");
                    sb.Append($" '{model.Total_Accepted_Qty}', ");
                    sb.Append($" '{model.Inner_LPN}', "); 
                    sb.Append($" '{model.Box_Received_Qty}', "); 
                    sb.Append($" '{model.Box_Accepted_Qty}'");                
                    sb.Append(" )");
                    actionString = "INSERT";
                //}
                //else
                //{
                //    //existed => update
                //    actionString = "UPDATE";
                //    sb.Append(" update sfis1.c_MODEL_desc_t ");
                //    sb.Append(" SET ");
                //    sb.Append($" MODEL_NAME = '{model.MODEL_NAME}', ");
                //    sb.Append($" MODEL_SERIAL = '{model.MODEL_SERIAL}', ");
                //    sb.Append($" MODEL_TYPE = '{model.MODEL_TYPE}', ");
                //    sb.Append($" BOM_NO = '{model.BOM_NO}', ");
                //    sb.Append($" STANDARD = '{model.STANDARD}', ");
                //}
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP_NO}', ");
                sbLog.Append($" 'QASN_IN', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'QASN_IN: Total_Received_Qty {model.Total_Received_Qty}, Total_Accepted_Qty {model.Total_Accepted_Qty}, IP:{AuthorizationController.UserIP()}' ");
                sbLog.Append(" ) ");

                string strInsertLog = sbLog.ToString();
                string strInserUpdate = sb.ToString();
                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
        public class QASN_IN_Element
        {
            public string database_name { get; set; }
            public string EMP_NO { get; set; }
            public string Total_Received_Qty { get; set; }
            public string Total_Accepted_Qty { get; set; }
            public string Inner_LPN { get; set; }
            public string Box_Received_Qty { get; set; }
            public string Box_Accepted_Qty { get; set; }
        }
    }
}
