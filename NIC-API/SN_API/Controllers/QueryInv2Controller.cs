using SN_API.Models;
using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SN_API.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class QueryInv2Controller : ApiController
    {
        [System.Web.Http.Route("OptionQueryInv2")]
        [System.Web.Http.HttpPost]
        public async Task<System.Net.Http.HttpResponseMessage> OptionQueryInv2(ValueOption valueInput)
        {
            string _database = valueInput.database;
            string _option = valueInput.option;
            string value = valueInput.value_input;
            string query_string = "";
            string sub_query = "";
            if (_option == "MO")
            {
                query_string = "SELECT MO_NUMBER,SERIAL_NUMBER,MODEL_NAME,VERSION_CODE,LINE_NAME," +
                                 "SECTION_NAME,GROUP_NAME,STATION_NAME,IN_STATION_TIME,IN_LINE_TIME," +
                                 "SHIPPING_SN,SPECIAL_ROUTE,PALLET_NO,QA_NO,QA_RESULT,SCRAP_FLAG," +
                                 "NEXT_STATION,CUSTOMER_NO,BOM_NO,PO_NO,KEY_PART_NO,CARTON_NO,REWORK_NO," +
                                 "EMP_NO,PO_LINE,PALLET_FULL_FLAG,ATE_STATION_NO,IMEI,MCARTON_NO,SO_NUMBER," +
                                 "SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,WIP_GROUP FROM SFISM4.R_WIP_TRACKING_T " +
                                 " WHERE MO_NUMBER = '" + value + "' OR REWORK_NO = '" + value + "'";
                DataTable dtmo = DBConnect.GetData(query_string, _database);
                if (dtmo.Rows.Count == 0)
                {
                    var query_string1 = "SELECT MO_NUMBER,SERIAL_NUMBER,MODEL_NAME,VERSION_CODE,LINE_NAME," +
                                 "SECTION_NAME,GROUP_NAME,STATION_NAME,IN_STATION_TIME,IN_LINE_TIME," +
                                 "SHIPPING_SN,SPECIAL_ROUTE,PALLET_NO,QA_NO,QA_RESULT,SCRAP_FLAG," +
                                 "NEXT_STATION,CUSTOMER_NO,BOM_NO,PO_NO,KEY_PART_NO,CARTON_NO,REWORK_NO," +
                                 "EMP_NO,PO_LINE,PALLET_FULL_FLAG,ATE_STATION_NO,IMEI,MCARTON_NO,SO_NUMBER," +
                                 "SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,WIP_GROUP FROM SFISM4.H_WIP_TRACKING_T " +
                                 " WHERE MO_NUMBER = '" + value + "'";
                    DataTable dtmo1 = DBConnect.GetData(query_string1, _database);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = dtmo1, query = query_string1, result = "ok" });
                }
            }
            else if (_option == "Serial")
            {
                query_string = "SELECT SERIAL_NUMBER,B.INVOICE,SHIPPING_SN,A.MODEL_NAME,A.VERSION_CODE," +
                               "PALLET_NO,CARTON_NO,IMEI,MCARTON_NO,TRAY_NO,SHIP_NO" +
                              " FROM SFISM4.R_WIP_TRACKING_T A,SFISM4.R_BPCS_INVOICE_T B " +
                              " WHERE Serial_number = '" + value + "' AND A.SHIP_NO <> 'N/A' AND A.SHIP_NO = B.TCOM";
            }
            else if (_option == "MAC")
            {
                query_string = "SELECT Serial_Number,Mo_number,Key_Part_No,Key_Part_SN,Version FROM SFISM4.R_WIP_KEYPARTS_T " +
                               " WHERE (Serial_number = '" + value + "' OR Key_Part_SN = '" + value + "')" + " AND VERSION IS NOT NULL";
            }
            else if (_option == "Box")
            {
                query_string = "SELECT SERIAL_NUMBER,B.INVOICE,SHIPPING_SN,A.MODEL_NAME,A.VERSION_CODE," +
                              "PALLET_NO,CARTON_NO,IMEI,MCARTON_NO,TRAY_NO,SHIP_NO" +
                              " FROM SFISM4.R_WIP_TRACKING_T A,SFISM4.R_BPCS_INVOICE_T B " +
                              " WHERE Shipping_sn = '" + value + "' AND A.SHIP_NO <> 'N/A' AND A.SHIP_NO = B.TCOM";
            }
            else if (_option == "R2S_MAC")
            {

            }
            else if (_option == "Invoice")
            {
                string query_invoice = "SELECT TCOM FROM SFISM4.R_BPCS_INVOICE_T WHERE Invoice = '" + value + "'";
                DataTable dtinvoice = DBConnect.GetData(query_invoice, _database);
                if (dtinvoice.Rows.Count > 0)
                {
                    string ship_no = "";
                    foreach (DataRow row in dtinvoice.Rows)
                    {
                        ship_no = row[0].ToString();
                    }
                    query_string = "SELECT a.*,b.TRACK_NO FROM sfism4.z107 a,sfism4.r107 b " +
                                "WHERE a.SHIP_NO = '" + ship_no + "' AND a.SERIAL_NUMBER = b.SERIAL_NUMBER  " +
                                "UNION " +
                                "SELECT c.*,d.TRACK_NO FROM sfism4.z107@SFCODBH c,sfism4.r107@SFCODBH d " +
                                "WHERE c.SHIP_NO = '" + ship_no + "' AND c.SERIAL_NUMBER = d.SERIAL_NUMBER  ";
                }
            }
            else if (_option == "R2S_Ship")
            {

            }
            else if (_option == "H2S_Ship")
            {

            }
            else if (_option == "Find_MO")
            {
                query_string = "SELECT * FROM SFISM4.R_WIP_TRACKING_T WHERE STOCK_NO<>'N/A' AND SHIP_NO='N/A' " +
                            "AND TRAY_NO='N/A'  and length(SERIAL_NUMBER)='7' AND MODEL_NAME LIKE  '" + value + "%' " +
                            "AND MO_NUMBER NOT IN (SELECT MO_NUMBER FROM SFISM4.Z_WIP_TRACKING_T GROUP BY MO_NUMBER)  " +
                            "AND MO_NUMBER IN (SELECT MO_NUMBER FROM SFISM4.R105 WHERE CLOSE_FLAG='3') AND ROWNUM=1 ";
            }
            else if (_option == "T77")
            {

            }


            DataTable dt = DBConnect.GetData(query_string, _database);
            if (dt.Rows.Count > 0)
            {
                if (_option == "Serial")
                {
                    sub_query = "SELECT * FROM SFISM4.R_WIP_KEYPARTS_T " +
                    "WHERE SERIAL_NUMBER = '" + dt.Rows[0]["SERIAL_NUMBER"] + "' OR KEY_PART_SN = '" + dt.Rows[0]["SERIAL_NUMBER"] + "'";
                }
                else if (_option == "MAC")
                {
                    sub_query = "SELECT SERIAL_NUMBER,SHIPPING_SN,A.MODEL_NAME,A.VERSION_CODE," +
                                  "PALLET_NO,CARTON_NO,IMEI,MCARTON_NO,TRAY_NO,SHIP_NO" +
                                  " FROM SFISM4.R_WIP_TRACKING_T A " +
                                  " WHERE Serial_number = '" + dt.Rows[0]["SERIAL_NUMBER"] + "' AND A.SHIP_NO <> 'N/A'";
                }
                else if (_option == "MO")
                {
                    sub_query = "SELECT * FROM SFISM4.R_WIP_KEYPARTS_T " +
                   "WHERE SERIAL_NUMBER = '" + dt.Rows[0]["SERIAL_NUMBER"] + "' OR KEY_PART_SN = '" + dt.Rows[0]["SERIAL_NUMBER"] + "'";
                }
                else if (_option == "Repair")
                {
                    sub_query = "SELECT * FROM SFISM4.R_REPAIR_T_BAK WHERE SERIAL_NUMBER = '" + dt.Rows[0]["SERIAL_NUMBER"] + "' ";
                }

            }
            else
            {
                if (_option == "Serial")
                {
                    query_string = query_string.Replace("R_WIP_TRACKING_T", "H_WIP_TRACKING_T");
                }
                else if (_option == "MO")
                {
                    query_string = query_string.Replace("R_WIP_TRACKING_T", "H_WIP_TRACKING_T");
                }
                else if (_option == "MAC")
                {
                    query_string = query_string.Replace("R_WIP_TRACKING_T", "H_WIP_TRACKING_T");
                }
                dt = DBConnect.GetData(query_string, _database);
                if (dt.Rows.Count > 0)
                {
                    if (_option == "Serial")
                    {
                        sub_query = sub_query.Replace("R_WIP_KEYPARTS_T", "H_WIP_KEYPARTS_T");
                    }
                    else if (_option == "Keypart")
                    {
                        sub_query = sub_query.Replace("R_WIP_TRACKING_T", "H_WIP_TRACKING_T");
                    }
                    else if (_option == "MO")
                    {
                        sub_query = sub_query.Replace("R_WIP_KEYPARTS_T", "H_WIP_KEYPARTS_T");
                    }
                    else if (_option == "Repair")
                    {
                        sub_query = "SELECT * FROM SFISM4.R_REPAIR_T_BAK WHERE SERIAL_NUMBER = '" + dt.Rows[0]["SERIAL_NUMBER"] + "' ";
                    }

                }
                DataTable dt2 = DBConnect.GetData(sub_query, _database);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = dt, query = query_string, data1 = dt2, result = "ok" });
            }
            DataTable dt1 = DBConnect.GetData(sub_query, _database);
            return Request.CreateResponse(HttpStatusCode.OK, new { data = dt, query = query_string, data1 = dt1, result = "ok" });
        }
    }
}