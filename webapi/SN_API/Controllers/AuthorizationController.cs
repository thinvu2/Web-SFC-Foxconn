using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SN_API.Class;
using SN_API.Handler.JWT;
using SN_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.IO;
using static SN_API.Models.ValueShipToFile;
using System.Web.Script.Serialization;
using System.Collections;

namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthorizationController : ApiController
    {
        [System.Web.Http.Route("GetIP")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetIP()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = addresses[0] });
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { result = context.Request.ServerVariables["REMOTE_ADDR"] }); ;
        }

        public static string GetMacAddress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            int i;
            string mac = "";
            for (i = 0; i <= macAddresses.Length - 2; i = i + 2)
            {
                if (i == macAddresses.Length - 2)
                {
                    mac = mac + macAddresses.Substring(i, 2);
                }
                else
                    mac = mac + macAddresses.Substring(i, 2) + ':';
            }
            return mac;
        }

        public static string UserIP()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        [System.Web.Http.Route("GetCPU")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetCPU()
        {
            PerformanceCounter PC = new PerformanceCounter();
            PC.CategoryName = "Process";
            PC.CounterName = "Working Set - Private";
            PC.InstanceName = "processNameHere";
            var memsize = Convert.ToInt32(PC.NextValue()) / (int)(1024);
            PC.Close();
            PC.Dispose();
            return Request.CreateResponse(HttpStatusCode.OK, new { result = memsize }); ;
        }

        [System.Web.Http.Route("GenerateToken")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Index(LoginInfo info)
        {
            var response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.OK, new { token = GetToken(info.UserName, info.PassWord) });
            return response;
        }
        protected string GetToken(string username, string password)
        {
            var _token = JwtToken.GenerateToken(username, password);
            return _token;
        }
        protected string CheckToken(string token)
        {
            var _username = JwtToken.ValidateToken(token);
            return _username;
        }
        [System.Web.Http.Route("CheckLogin")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> CheckLogin(LoginInfo privilege)
        {
            string database_name = privilege.DatabaseName;
            string UserName = privilege.UserName;
            string PassWord = privilege.PassWord;
            var watch = new Stopwatch();
            string queryString = "select * from sfis1.C_EMP_DESC_T  where Upper(emp_no) = '" + UserName.ToUpper() + "' and emp_pass='" + PassWord + "'";
            DataTable dtCheck = DBConnect.GetData(queryString, database_name);
            watch.Stop();
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail", time = watch.ElapsedMilliseconds });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", emp_name = dtCheck.Rows[0]["EMP_NAME"].ToString(), time = watch.ElapsedMilliseconds });
            }
        }
        [System.Web.Http.Route("GetFtpAddress")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetFtpAddress()
        {
            string queryString = "SELECT * FROM sfism4.AMS_SERVER WHERE SERVER_ALIAS='MAIN'";

            DataTable dtCheck = DBConnect.GetData(queryString, "CPEII");
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                string ftpAddress = "ftp://" + dtCheck.Rows[0]["SERVER_IP"] + dtCheck.Rows[0]["SERVER_DIR"].ToString().Replace('\\', '/');
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ftpAddress });
            }
        }

        [System.Web.Http.Route("GetAMSVersion")]
        [System.Web.Http.HttpPost]

        public async Task<HttpResponseMessage> GetAMSVersion()
        {
            try
            {
                DataTable dt = DBConnect.GetData("select * from sfis1.C_AMS_PATTERN_T where ap_name = 'SFIS_AMS'", "CPEII");

                return Request.CreateResponse(new { status = "success", version = dt.Rows[0]["AP_VERSION"].ToString() });
            }
            catch
            {
                return Request.CreateResponse(new { status = "fail" });
            }
        }
        [System.Web.Http.Route("GetEmp")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult CheckAuth([FromBody] dynamic obj)
        {
            var datain = Convert.ToString(obj);
            GempList emplist = new GempList();

            var dt = emplist.EmpList(datain);

            var empType = emplist.EmpType(datain);

            var jempType = emplist.DT2Json(dt);
            var _results = ReturnJsonResult.GetJsonResult(empType, jempType);
            return Ok(_results);
        }

        //[System.Web.Http.Route("GetEmp")]
        ////[System.Web.Http.Authorize]
        //[System.Web.Http.HttpPost]
        //public async Task<HttpResponseMessage> CheckAuth(EmpResponse emp)
        //{
        //    List<Employee> listEmp = new List<Employee>();
        //    string Token = emp.Token;
        //    var ApplicationID = emp.ApplicationID;
        //    var _listEMPNo = emp.EmpList;
        //    int i = 0;
        //    string queryString = "";
        //    var response = new HttpResponseMessage();
        //    //var _obj = JwtToken.GetPayLoad(Token);
        //    //var _unique_name = "";
        //    //var _password = "";
        //    //try
        //    //{
        //    //    _unique_name = _obj["username"].ToString();
        //    //    _password = _obj["password"].ToString();
        //    //}
        //    //catch
        //    //{
        //    //    return Request.CreateResponse(new { message = "Authorization has been denied for this request." });
        //    //}
        //    //string _query_string_check = "SELECT * FROM SFIS1.C_PARAMETER_INI WHERE PRG_NAME = 'AMS_API' AND VR_NAME = '" + _unique_name + "' AND VR_VALUE = '" + _password + "' AND ROWNUM =1";
        //    //DataTable dtCheck = Employee.GetData(_query_string_check, "CPEI");
        //    //if (dtCheck.Rows.Count == 0) return Request.CreateResponse(new { message = "Username or password is wrong." });

        //    string temp_condition = "";
        //    foreach (var item in _listEMPNo)
        //    {
        //        if (i == 0)
        //        {
        //            temp_condition += "'" + item.EMPNO + "'";
        //        }
        //        else
        //        {
        //            temp_condition += ",'" + item.EMPNO + "'";
        //        }
        //        i = 1;
        //    }
        //    queryString = "SELECT * FROM SFIS1.C_AMS_USER_INFOR_T WHERE APPLICATIONID='" + ApplicationID + "' AND EMPNO IN (" + temp_condition + ")";

        //    if (temp_condition == "") return Request.CreateResponse(new { ApplicationID = ApplicationID, EmpList = listEmp });


        //    DataTable dt = Employee.GetData(queryString, "CPEI");

        //    if (dt.Rows.Count < 0) return response;

        //    for (int j = 0; j < dt.Rows.Count; j++)
        //    {
        //        listEmp.Add(new Employee(dt.Rows[j]["USERID"].ToString(), dt.Rows[j]["EMPNO"].ToString(), dt.Rows[j]["NAME"].ToString(), dt.Rows[j]["GROUPNAME"].ToString(), dt.Rows[j]["GROUPREMARK"].ToString(), dt.Rows[j]["EFFECTTIME"].ToString()));
        //    }

        //    return Request.CreateResponse(new { ApplicationID = ApplicationID, EmpList = listEmp });

        //}
        [System.Web.Http.Route("GetDataShipToFile")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetDataShipToFile(ValueShipToFile value)
        {
            var database = value.database;
            string serial_value = GetListData(value.list_serial);
            string license_value = GetListData(value.license_no);
            string mcarton_value = GetListData(value.mcarton_no);
            string sql = string.Format(@"select distinct b.mo_number,b.model_name,b.VERSION_CODE,d.CUST_PO, h.BRCM_VER,d.INVOICE, 
                        b.IMEI,d.CUST_NAME, b.serial_number,b.SHIPPING_SN,b.SHIPPING_SN2,i.SEQ, 
                        (SELECT COUNT(DISTINCT MCARTON_NO)fROM SFISM4.Z107 WHERE SHIP_NO = B.SHIP_NO) DN_CTN_QTY, 
                        (SELECT COUNT(SERIAL_NUMBER)fROM SFISM4.Z107 WHERE SHIP_NO = B.SHIP_NO) DN_QTY, 
                       b.wip_group, 
                        b.mcarton_no,c.LICENSE_NO,g.GROSS_WEIGHT, g.QTY,d.FINISH_DATE,f.SHIP_ADDRESS
                        from sfism4.z107 b
                        left join SFISM4.R_SEC_LIC_LINK_T c
                          on b.mcarton_no = c.carton_no
                        left join SFISM4.R_BPCS_INVOICE_T d
                            on b.ship_no = d.tcom
                        left join SFISM4.R_SAP_DN_DETAIL_T f
                            on d.INVOICE = f.DN_NO
                        left join SFISM4.R_CTN_WEIGHT_T g   
                        on b.mcarton_no = g.mcarton_no
                        left join SFIS1.C_MODEL_BRCM_VER_T h
                        on b.mo_number = h.mo_number
                        left join    SFISM4.R_DN_CARTON_LINK_T i
                        on b.mcarton_no = i.MCARTON_NO
                        where 1 = 1 and wip_group = 'SHIPPING' ");


            string r117 = string.Format(@"SELECT * FROM (SELECT SERIAL_NUMBER,MODEL_NAME,MO_NUMBER,VERSION_CODE Version,TYPE,SECTION_FLAG,LINE_NAME,
                        SECTION_NAME,GROUP_NAME,STATION_NAME,LOCATION,STATION_SEQ,ERROR_FLAG,IN_STATION_TIME,
                        IN_LINE_TIME,OUT_LINE_TIME,SHIPPING_SN,WORK_FLAG,FINISH_FLAG,ENC_CNT,SPECIAL_ROUTE,
                        PALLET_NO,CONTAINER_NO,QA_NO,QA_RESULT,SCRAP_FLAG,NEXT_STATION,CUSTOMER_NO,BOM_NO,
                        BILL_NO,TRACK_NO,PO_NO,KEY_PART_NO,CARTON_NO,WARRANTY_DATE,REWORK_NO,REPAIR_CNT,
                        EMP_NO,PO_LINE,PALLET_FULL_FLAG,PMCC,GROUP_NAME_CQC,MO_NUMBER_OLD,ERP_MO,ATE_STATION_NO,
                        MSN,IMEI,JOB,MCARTON_NO,SO_NUMBER,SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,WIP_GROUP,SHIPPING_SN2 FROM SFISM4.R117 where serial_number in({0})
                        union all
                        SELECT SERIAL_NUMBER,MODEL_NAME,MO_NUMBER,VERSION_CODE Version,TYPE,SECTION_FLAG,LINE_NAME,
                        SECTION_NAME,GROUP_NAME,STATION_NAME,LOCATION,STATION_SEQ,ERROR_FLAG,IN_STATION_TIME,
                        IN_LINE_TIME,OUT_LINE_TIME,SHIPPING_SN,WORK_FLAG,FINISH_FLAG,ENC_CNT,SPECIAL_ROUTE,
                        PALLET_NO,CONTAINER_NO,QA_NO,QA_RESULT,SCRAP_FLAG,NEXT_STATION,CUSTOMER_NO,BOM_NO,
                        BILL_NO,TRACK_NO,PO_NO,KEY_PART_NO,CARTON_NO,WARRANTY_DATE,REWORK_NO,REPAIR_CNT,
                        EMP_NO,PO_LINE,PALLET_FULL_FLAG,PMCC,GROUP_NAME_CQC,MO_NUMBER_OLD,ERP_MO,ATE_STATION_NO,
                        MSN,IMEI,JOB,MCARTON_NO,SO_NUMBER,SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,WIP_GROUP,SHIPPING_SN2 FROM SFISM4.R117@sfcodbh where serial_number in({0}))
                        WHERE 1 = 1 ORDER BY IN_STATION_TIME ASC", serial_value);
            if (value.ischeckR117 && serial_value != null)
            {
                if (value.list_serial.Count() > 999)
                    return null;
                DataTable dtCheckr117 = DBConnect.GetData(r117, database);
                if (dtCheckr117.Rows.Count != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheckr117, moQTY = dtCheckr117.Rows.Count, queryTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheckr117, moQTY = dtCheckr117.Rows.Count, queryTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                }

            }

            //search serial_number
            if (serial_value != null)
            {
                sql = sql + " and (b.serial_number in(" + serial_value + ") or b.SHIPPING_SN in(" + serial_value + "))";
            } //search license_no
            if (license_value != null)
            {
                sql = sql + " and c.LICENSE_NO in(" + license_value + ")";
            }
            //seach mcarton
            if (mcarton_value != null)
            {
                sql = sql + " and b.mcarton_no in(" + mcarton_value + ")";
            }
            if (value.DN_NO != null)
            {
                sql = sql + " and d.INVOICE = '" + value.DN_NO + "'";
            }


            DataTable dtCheck = DBConnect.GetData(sql, database);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, moQTY = dtCheck.Rows.Count, queryTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
        }
        private string GetListData(List<List_Input> input)
        {
            string result = "";

            if (input != null)
            {
                for (int i = 0; i < input.Count; i++)
                {
                    if (input[i].input.Trim() != "")
                    {
                        if (i == 0)
                        {
                            result += "'" + input[i].input + "'";
                        }
                        else
                        {
                            result += ",'" + input[i].input + "'";
                        }
                    }
                }

                if (result == "")
                    return null;

                return result;
            }
            else
            {
                return null;
            }

        }
        [System.Web.Http.Route("OptionQuery")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> OptionQuery(ValueOption valueInput)
        {
            string _database = valueInput.database;
            string _option = valueInput.option;
            string value = valueInput.value_input;
            string query_string = "";
            string sub_query = "";
            if (_option == "COC")
            {
                string[] list = value.Split(',');
                StringBuilder myDN = new StringBuilder();
                myDN.Append("(");
                for (int i = 0; i < list.Length; i++)
                {
                    if (i == 0)
                    {
                        myDN.Append("'" + list[i].Trim() + "'");
                    }
                    else
                    {
                        myDN.Append(",'" + list[i].Trim() + "'");
                    }
                }
                myDN.Append(")");
                //cai nay de sau
                string sqlListTestStation = "SELECT DISTINCT C.GROUP_NAME " +
             "  FROM SFISM4.Z107 A, SFISM4.R_BPCS_INVOICE_T B, SFIS1.C_ROUTE_CONTROL_T C " +
             " WHERE     B.TCOM = A.SHIP_NO " +
             "       AND B.INVOICE IN " + myDN.ToString() + " " +
             "       AND A.SPECIAL_ROUTE = C.ROUTE_CODE " +
             "       AND C.GROUP_NAME IN (SELECT GROUP_NAME " +
             "                              FROM SFIS1.C_GROUP_CONFIG_T " +
             "                             WHERE GROUP_DESC = 'TEST') ";
                DataTable dtMain = new DataTable();
                dtMain.Columns.Add("PN");
                dtMain.Columns.Add("DN");
                dtMain.Columns.Add("QTY");
                dtMain.Columns.Add("DATECODE");
                dtMain.Columns.Add("SHIPDATE");
                dtMain.Columns.Add("OBA");
                int lengthTable = 6;
                DataTable dtStation = DBConnect.GetData(sqlListTestStation, _database);
                foreach (DataRow dr in dtStation.Rows)
                {
                    dtMain.Columns.Add(dr["GROUP_NAME"].ToString());
                    lengthTable++;
                }
                string SQL
    = "SELECT A.MODEL_NAME PN,  " +
        "   A.INVOICE DN, " +
        "    A.SHIPPING_QTY QTY,        " +
        "        LISTAGG(B.DATECODE,',') WITHIN GROUP (ORDER BY A.MODEL_NAME) DATECODE , " +
        "         A.FINISH_DATE SHIPDATE  " +
        "    FROM SFISM4.R_BPCS_INVOICE_T A, " +
        "         (SELECT DISTINCT SUBSTR (A.MCARTON_NO, 6, 3) DATECODE, B.INVOICE " +   //sua cho ma carton B04 12 ky tu Tuan 7/7/2022
        "            FROM SFISM4.Z107 A, SFISM4.R_BPCS_INVOICE_T B " +
        "           WHERE B.TCOM = A.SHIP_NO AND B.INVOICE IN " + myDN.ToString() + ") B " +
        "   WHERE A.INVOICE = B.INVOICE " +
        "GROUP BY A.model_nAME, " +
        "         A.INVOICE, " +
        "         A.SHIPPING_QTY,                " +
        "         A.FINISH_DATE ";

                DataTable dtFirst = DBConnect.GetData(SQL, _database);
                string[] totalTest = new string[lengthTable - 6];
                int[] numberTestLeft = new int[lengthTable - 6];
                int[] numberTestRight = new int[lengthTable - 6];
                int obaLeft = 0;
                int obaRight = 0;
                for (int i = 0; i < dtFirst.Rows.Count; i++)
                {
                    string SQLOBA =
        "SELECT    (SELECT SUM (NG_NUM) " +
        "             FROM SFISM4.R_CQC_REC_T " +
        "            WHERE LOT_NO IN (SELECT DISTINCT F.LOT_NO " +
        "                               FROM SFISM4.Z107 D, " +
        "                                    SFISM4.R_BPCS_INVOICE_T E, " +
        "                                    SFISM4.R_QC_SN_T F " +
        "                              WHERE     E.TCOM = D.SHIP_NO " +
        "                                    AND D.SERIAL_NUMBER = F.SERIAL_NUMBER " +
        "                                    AND E.INVOICE = '" + dtFirst.Rows[i]["DN"].ToString() + "')) " +
        "       || '/' " +
        "       || (SELECT SUM (PASS_QTY) OBA " +
        "             FROM SFISM4.R_CQC_REC_T " +
        "            WHERE LOT_NO IN (SELECT DISTINCT F.LOT_NO " +
        "                               FROM SFISM4.Z107 D, " +
        "                                    SFISM4.R_BPCS_INVOICE_T E, " +
        "                                    SFISM4.R_CQC_REC_T F " +
        "                              WHERE     E.TCOM = D.SHIP_NO " +
        "                                    AND D.QA_NO = F.LOT_NO " +
        "                                    AND E.INVOICE = '" + dtFirst.Rows[i]["DN"].ToString() + "')) " +
        "          OBA " +
        "  FROM DUAL ";
                    DataTable dt_oba = DBConnect.GetData(SQLOBA, _database);
                    DataRow dtRow = dtMain.NewRow();
                    dtMain.Rows.Add(dtRow);
                    dtMain.Rows[i][0] = dtFirst.Rows[i]["PN"].ToString();
                    dtMain.Rows[i][1] = dtFirst.Rows[i]["DN"].ToString();
                    dtMain.Rows[i][2] = dtFirst.Rows[i]["QTY"].ToString();
                    dtMain.Rows[i][3] = dtFirst.Rows[i]["DATECODE"].ToString();
                    dtMain.Rows[i][4] = dtFirst.Rows[i]["SHIPDATE"].ToString();
                    dtMain.Rows[i][5] = dt_oba.Rows[0]["OBA"].ToString();
                    int k = 0;
                    foreach (DataRow drr in dtStation.Rows)
                    {
                        string sqlStation = "SELECT    COUNT ( " +
            "             DISTINCT (SELECT DISTINCT SERIAL_NUMBER " +
            "                         FROM SFISM4.R117 " +
            "                        WHERE     GROUP_NAME = '" + drr["GROUP_NAME"].ToString() + "' " +
            "                              AND ERROR_FLAG = '1' " +
            "                              AND SERIAL_NUMBER = A.SERIAL_NUMBER)) " +
            "       || '/' " +
            "       || COUNT ( " +
            "             DISTINCT (SELECT DISTINCT SERIAL_NUMBER " +
            "                         FROM SFISM4.R117 " +
            "                        WHERE     GROUP_NAME = '" + drr["GROUP_NAME"].ToString() + "' " +
            "                              AND ERROR_FLAG = '0' " +
            "                              AND SERIAL_NUMBER = A.SERIAL_NUMBER)) " +
            "          " + drr["GROUP_NAME"].ToString() + " " +
            "  FROM SFISM4.Z107 A, SFISM4.R_BPCS_INVOICE_T B " +
            " WHERE B.TCOM = A.SHIP_NO AND B.INVOICE = '" + dtFirst.Rows[i]["DN"].ToString() + "' ";

                        DataTable dtt = DBConnect.GetData(sqlStation, _database);
                        if (dtt.Rows.Count > 0)
                        {
                            dtMain.Rows[i][drr["GROUP_NAME"].ToString()] = dtt.Rows[0][0].ToString();
                        }
                        else
                        {
                            dtMain.Rows[i][drr["GROUP_NAME"].ToString()] = "0/0";
                        }
                        numberTestLeft[k] += int.Parse(dtt.Rows[0][0].ToString().Split('/')[0]);
                        numberTestRight[k] += int.Parse(dtt.Rows[0][0].ToString().Split('/')[1]);
                        k++;
                    }

                    string[] obaRate = dt_oba.Rows[0]["OBA"].ToString().Split('/');
                    obaLeft += int.Parse(obaRate[0]);
                    obaRight += int.Parse(obaRate[1]);
                    if (i == dtFirst.Rows.Count - 1)
                    {

                        DataRow dtRow1 = dtMain.NewRow();
                        DataRow dtRow2 = dtMain.NewRow();
                        dtMain.Rows.Add(dtRow1);
                        dtMain.Rows.Add(dtRow2);
                        dtMain.Rows[i + 1][0] = "Total QTY";
                        dtMain.Rows[i + 1][5] = obaLeft + "/" + obaRight;

                        for (int j = 0; j < totalTest.Length; j++)
                        {
                            dtMain.Rows[i + 1][6 + j] = numberTestLeft[j] + "/" + numberTestRight[j];

                            dtMain.Rows[i + 2][6 + j] = Math.Round((100 - (double)int.Parse(dtMain.Rows[i + 1][6 + j].ToString().Split('/')[0]) / int.Parse(dtMain.Rows[i + 1][6 + j].ToString().Split('/')[1].ToString()) * 100), 2) + "%";
                        }

                        dtMain.Rows[i + 2][0] = "Total YieldRate";
                        dtMain.Rows[i + 2][5] = Math.Round(100 - (double)obaLeft / obaRight * 100, 2) + "%";
                    }
                }
                DataTable dtNull = new DataTable();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = dtMain, query = query_string, data1 = dtNull, result = "ok" });
            }
            else if (_option == "NOTLASER")
            {
                query_string = $"select*from SFISM4.R_NETG_PRIN_ALL_T where mo_number='{value}' AND MACID1 <>'Y'";
            }
            else if (_option == "Serial")
            {
                query_string = "SELECT a.SERIAL_NUMBER,a.SHIPPING_SN,a.PALLET_NO,a.CARTON_NO,a.MCARTON_NO,a.IMEI,a.MO_NUMBER," +
                            "a.MODEL_NAME,a.VERSION_CODE,a.SECTION_NAME,a.GROUP_NAME,a.STATION_NAME,a.LOCATION,a.ERROR_FLAG,a.track_no,a.IN_STATION_TIME," +
                             "a.IN_LINE_TIME,a.SPECIAL_ROUTE,a.QA_NO,a.QA_RESULT,a.SCRAP_FLAG,a.LINE_NAME," +
                            "a.NEXT_STATION,a.CUSTOMER_NO,a.BOM_NO,a.PO_NO,a.KEY_PART_NO,a.REWORK_NO," +
                             "a.EMP_NO,a.PO_LINE,a.PALLET_FULL_FLAG,a.ATE_STATION_NO,a.GROUP_NAME_CQC,a.MSN,a.SO_NUMBER,b.LICENSE_NO PMCC," +
                             "a.SO_LINE,a.STOCK_NO,a.TRAY_NO,a.SHIP_NO,a.WIP_GROUP FROM SFISM4.R_WIP_TRACKING_T a left join SFISM4.R_SEC_LIC_LINK_T b on a.mcarton_no = b.carton_no or  a.carton_no = b.carton_no" +
                             " WHERE a.Serial_Number like '" + value + "%'";
            }
            else if (_option == "GetMoTlinklevel")
            {
                query_string = "SELECT SERIAL_NUMBER,PALLET_NO,CARTON_NO,MCARTON_NO,TRAY_NO, " +
        "SHIPPING_SN,MO_NUMBER,MODEL_NAME,VERSION_CODE,LINE_NAME,SECTION_NAME, " +
        "GROUP_NAME,STATION_NAME,LOCATION,IN_STATION_TIME,IN_LINE_TIME, " +
        "SPECIAL_ROUTE,QA_NO,QA_RESULT,SCRAP_FLAG, " +
        "NEXT_STATION,CUSTOMER_NO,BOM_NO,PO_NO,KEY_PART_NO,REWORK_NO, " +
        "EMP_NO,PO_LINE,PALLET_FULL_FLAG,ATE_STATION_NO,GROUP_NAME_CQC,MSN,IMEI,SO_NUMBER, " +
        "SO_LINE,STOCK_NO,SHIP_NO,WIP_GROUP  " +
        "FROM SFISM4.R107 WHERE SERIAL_NUMBER IN( " +
        "SELECT SERIAL_NUMBER FROM SFISM4.R107 A  " +
        "WHERE MO_NUMBER='" + value + "' AND NOT EXISTS ( " +
        "SELECT 1 FROM  SFISM4.R108 B WHERE A.SERIAL_NUMBER =B.KEY_PART_SN) ) ";
            }
            else if (_option == "CHECK MO")
            {
                query_string = "select * from sfism4.R105 where mo_number ='" + value + "' ";
            }
            else if (_option == "Repallet")
            {
                query_string = "SELECT CARTON_NO,MCARTON_NO,SERIAL_NUMBER,PALLET_NO,IMEI, " +
        "SHIPPING_SN,MO_NUMBER,MODEL_NAME,VERSION_CODE,LINE_NAME, " +
        "SECTION_NAME,GROUP_NAME,STATION_NAME,LOCATION,ERROR_FLAG,track_no,IN_STATION_TIME,IN_LINE_TIME, " +
        "SPECIAL_ROUTE,QA_NO,QA_RESULT,SCRAP_FLAG, " +
        "NEXT_STATION,CUSTOMER_NO,BOM_NO,PO_NO,KEY_PART_NO,REWORK_NO, " +
        "EMP_NO,PO_LINE,PALLET_FULL_FLAG,ATE_STATION_NO,GROUP_NAME_CQC,MSN,SO_NUMBER, " +
        "SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,WIP_GROUP FROM SFISM4.R_WIP_TRACKING_T  " +
        " WHERE IMEI= '" + value + "' OR PALLET_NO = '" + value + "' ";
            }
            else if (_option == "Keypart")
            {
                query_string = "SELECT SERIAL_NUMBER,PALLET_NO,CARTON_NO,MCARTON_NO,TRAY_NO," +
           "SHIPPING_SN,MO_NUMBER,MODEL_NAME,VERSION_CODE,LINE_NAME,SECTION_NAME," +
           "GROUP_NAME,STATION_NAME,LOCATION,ERROR_FLAG,IN_STATION_TIME,IN_LINE_TIME," +
           "SPECIAL_ROUTE,QA_NO,QA_RESULT,SCRAP_FLAG," +
           "NEXT_STATION,CUSTOMER_NO,BOM_NO,PO_NO,KEY_PART_NO,REWORK_NO," +
           "EMP_NO,PO_LINE,PALLET_FULL_FLAG,ATE_STATION_NO,GROUP_NAME_CQC,MSN,IMEI,SO_NUMBER," +
           "SO_LINE,STOCK_NO,SHIP_NO,WIP_GROUP FROM SFISM4.R_WIP_TRACKING_T " +
           " WHERE SERIAL_NUMBER IN(SELECT SERIAL_NUMBER FROM SFISM4.R_WIP_KEYPARTS_T " +
           " WHERE SERIAL_NUMBER = '" + value + "' OR KEY_PART_SN = '" + value + "')";
            }
            else if (_option == "Landis_DATA")
            {
                query_string = $" SELECT * FROM SFISM4.R_landis_data_T  WHERE  serial_number = '{value}' OR DATA2='{value}' order by in_station_time";
            }
            else if (_option == "HistoryKPSN")
            {
                query_string = " SELECT SERIAL_NUMBER,PALLET_NO,CARTON_NO,MCARTON_NO,TRAY_NO, " +
        "SHIPPING_SN,MO_NUMBER,MODEL_NAME,VERSION_CODE,LINE_NAME,SECTION_NAME, " +
        "GROUP_NAME,STATION_NAME,LOCATION,IN_STATION_TIME,IN_LINE_TIME, " +
        "SPECIAL_ROUTE,QA_NO,QA_RESULT,SCRAP_FLAG, " +
        "NEXT_STATION,CUSTOMER_NO,BOM_NO,PO_NO,KEY_PART_NO,REWORK_NO, " +
        "EMP_NO,PO_LINE,PALLET_FULL_FLAG,ATE_STATION_NO,GROUP_NAME_CQC,MSN,IMEI,SO_NUMBER, " +
        "SO_LINE,STOCK_NO,SHIP_NO,WIP_GROUP FROM SFISM4.R_WIP_TRACKING_T  " +
        " WHERE SERIAL_NUMBER IN(SELECT SERIAL_NUMBER FROM SFISM4.R_WIP_KEYPARTS_T  " +
        " WHERE SERIAL_NUMBER = '" + value + "' OR KEY_PART_SN = '" + value + "')";
            }
            else if (_option == "MO")
            {
                query_string = "SELECT MO_NUMBER,SERIAL_NUMBER,MODEL_NAME,VERSION_CODE,LINE_NAME," +
          "SECTION_NAME,GROUP_NAME,STATION_NAME,LOCATION,track_no,IN_STATION_TIME,IN_LINE_TIME," +
          "SHIPPING_SN,SPECIAL_ROUTE,PALLET_NO,QA_NO,QA_RESULT,SCRAP_FLAG," +
          "NEXT_STATION,CUSTOMER_NO,BOM_NO,PO_NO,KEY_PART_NO,CARTON_NO,REWORK_NO," +
          "EMP_NO,PO_LINE,PALLET_FULL_FLAG,ATE_STATION_NO,GROUP_NAME_CQC,MSN,IMEI,MCARTON_NO,SO_NUMBER," +
          "SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,WIP_GROUP ,SHIPPING_SN2 FROM SFISM4.R_WIP_TRACKING_T" +
          " WHERE MO_NUMBER = '" + value.Trim() + "'";
            }
            else if (_option == "Tray")
            {
                query_string = "SELECT TRAY_NO,SERIAL_NUMBER,PALLET_NO,CARTON_NO,MCARTON_NO," +
              "SHIPPING_SN,MO_NUMBER,MODEL_NAME,VERSION_CODE,LINE_NAME,SECTION_NAME," +
              "GROUP_NAME,STATION_NAME,IN_STATION_TIME,LOCATION,ERROR_FLAG,IN_LINE_TIME," +
              "SPECIAL_ROUTE,QA_NO,QA_RESULT,SCRAP_FLAG," +
              "NEXT_STATION,CUSTOMER_NO,BOM_NO,PO_NO,KEY_PART_NO,REWORK_NO," +
              "EMP_NO,PO_LINE,PALLET_FULL_FLAG,ATE_STATION_NO,GROUP_NAME_CQC,MSN,IMEI,SO_NUMBER," +
              "SO_LINE,STOCK_NO,SHIP_NO,WIP_GROUP FROM SFISM4.R_WIP_TRACKING_T " +
              "WHERE Tray_NO = '" + value + "'";
            }
            else if (_option == "Route")
            {
                query_string = "SELECT ROUTE_CODE, ROUTE_NAME, ROUTE_DESC " +
        "  FROM SFIS1.C_ROUTE_NAME_T " +
        " WHERE    TO_CHAR(ROUTE_CODE) = '" + value.ToUpper().Trim() + "' " +
        "       OR ROUTE_NAME LIKE '%" + value.ToUpper().Trim() + "%' " +
        "       OR ROUTE_CODE = (SELECT SPECIAL_ROUTE " +
        "                          FROM SFISM4.R107 " +
        "                         WHERE SERIAL_NUMBER = '" + value.Trim() + "') " +
        "       OR ROUTE_CODE = (SELECT ROUTE_CODE " +
        "                          FROM SFISM4.R105 " +
        "                         WHERE MO_NUMBER = '" + value.Trim() + "') ";
            }
            else if (_option == "BOX_SN")
            {
                query_string = "SELECT SHIPPING_SN,SERIAL_NUMBER,MO_NUMBER,MODEL_NAME,VERSION_CODE,LINE_NAME," +
          "SECTION_NAME,GROUP_NAME,STATION_NAME,LOCATION,track_no,IN_STATION_TIME,IN_LINE_TIME," +
          "SPECIAL_ROUTE,PALLET_NO,QA_NO,QA_RESULT,SCRAP_FLAG," +
          "NEXT_STATION,CUSTOMER_NO,BOM_NO,PO_NO,KEY_PART_NO,CARTON_NO,REWORK_NO," +
          "EMP_NO,PO_LINE,PALLET_FULL_FLAG,ATE_STATION_NO,GROUP_NAME_CQC,MSN,IMEI,MCARTON_NO,SO_NUMBER," +
          "SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,WIP_GROUP FROM SFISM4.R_WIP_TRACKING_T " +
          " WHERE Shipping_sn = '" + value + "'";
            }
            else if (_option == "Carton")
            {
                query_string = "SELECT a.CARTON_NO,a.MCARTON_NO,a.SERIAL_NUMBER,a.PALLET_NO,a.IMEI," +
          "a.SHIPPING_SN,a.MO_NUMBER,a.MODEL_NAME,a.VERSION_CODE,a.LINE_NAME," +
          "a.SECTION_NAME,a.GROUP_NAME,a.STATION_NAME,a.LOCATION,a.ERROR_FLAG,a.track_no,a.IN_STATION_TIME,a.IN_LINE_TIME," +
          "a.SPECIAL_ROUTE,a.QA_NO,a.QA_RESULT,a.SCRAP_FLAG," +
          "a.NEXT_STATION,a.CUSTOMER_NO,a.BOM_NO,a.PO_NO,a.KEY_PART_NO,a.REWORK_NO," +
          "a.EMP_NO,a.PO_LINE,a.PALLET_FULL_FLAG,a.ATE_STATION_NO,a.GROUP_NAME_CQC,a.MSN,a.SO_NUMBER,b.LICENSE_NO PMCC," +
          "a.SO_LINE,a.STOCK_NO,a.TRAY_NO,a.SHIP_NO,a.WIP_GROUP FROM SFISM4.R_WIP_TRACKING_T a left join SFISM4.R_SEC_LIC_LINK_T b on a.mcarton_no = b.carton_no or  a.carton_no = b.carton_no" +
          " WHERE a.Carton_NO = '" + value + "' OR a.MCarton_NO = '" + value + "'";
            }
            else if (_option == "IMEI")
            {
                query_string = "SELECT CARTON_NO,MCARTON_NO,SERIAL_NUMBER,PALLET_NO,IMEI," +
          "SHIPPING_SN,MO_NUMBER,MODEL_NAME,VERSION_CODE,LINE_NAME," +
          "SECTION_NAME,GROUP_NAME,STATION_NAME,LOCATION,ERROR_FLAG,track_no,IN_STATION_TIME,IN_LINE_TIME," +
          "SPECIAL_ROUTE,QA_NO,QA_RESULT,SCRAP_FLAG," +
          "NEXT_STATION,CUSTOMER_NO,BOM_NO,PO_NO,KEY_PART_NO,REWORK_NO," +
          "EMP_NO,PO_LINE,PALLET_FULL_FLAG,ATE_STATION_NO,GROUP_NAME_CQC,MSN,SO_NUMBER," +
          "SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,WIP_GROUP FROM SFISM4.R_WIP_TRACKING_T " +
          " WHERE IMEI= '" + value + "' OR PALLET_NO = '" + value + "'";
            }
            else if (_option == "License")
            {
                query_string = "SELECT * FROM SFISM4.R_SEC_LIC_LINK_T WHERE CARTON_NO = '" + value + "' OR  LICENSE_NO = '" + value + "' ";
            }
            else if (_option == "Cpanel")
            {
                query_string = "SELECT * FROM SFISM4.R_SN_TRSN_LINK_T WHERE PANEL_NO = '" + value + "'" +
                " or SERIAL_NUMBER ='" + value + "'  or  trsn = '" + value + "' ";
            }
            else if (_option == "FailAte")
            {
                query_string = $"SELECT * FROM SFISM4.R_FAIL_ATEDATA_T WHERE STATION_NAME = '{value}' or SERIAL_NUMBER ='{value}' ";
            }
            else if (_option == "IP")
            {
                query_string = "SELECT H.HOST_NAME, " +
        "         H.HOSTID, " +
        "         IP SMO_IP, " +
        "         H.SMO_DESC SERVER_DESC, " +
        "         F.STATION_NUMBER, " +
        "         F.GROUP_NAME, " +
        "         Q.PORT_ID, " +
        "         S.STATION_TYPE MHV, " +
        "         F.STATION_NAME, " +
        "         F.LINE_NAME LINE, " +
        "         Q.NODE_IP STATION_IP " +
        "    FROM SFIS1.C_QWAY_CONFIG_T Q, " +
        "         SFIS1.C_HOST_NAME_T H, " +
        "         SFIS1.C_STATION_TYPE S, " +
        "         SFIS1.C_STATION_CONFIG_T F " +
        "   WHERE     Q.HOSTID = H.HOSTID " +
        "         AND S.STATION_NUMBER = F.STATION_NUMBER " +
        "         AND S.STATION_NUMBER = Q.STATION_NUMBER " +
        "         AND Q.NODE_IP LIKE '%" + value + "%' " +
        "ORDER BY GROUP_NAME, IP, PORT_ID ASC ";
            }
            else if (_option == "MO_RANGE")
            {
                query_string = "SELECT * FROM SFISM4.R_MO_EXT2_T WHERE MO_NUMBER = '" + value + "'";
            }
            else if (_option == "MAC/SSN RANGER")
            {
                query_string = "SELECT * FROM SFISM4.R_MO_EXT4_T WHERE  '" + value + "'  BETWEEN  ITEM_1  AND  ITEM_2 AND length(ITEM_1)=length('" + value + "')";
                if (DBConnect.GetData(query_string, _database).Rows.Count <= 0)
                {
                    query_string = "SELECT * FROM SFISM4.R_MO_EXT4_T@sfcodbh WHERE  '" + value + "'  BETWEEN  ITEM_1  AND  ITEM_2 AND length(ITEM_1)=length('" + value + "')";
                    if (DBConnect.GetData(query_string, _database).Rows.Count <= 0)
                    {
                        query_string = "SELECT * FROM SFISM4.R_MO_EXT3_T WHERE  '" + value + "'  BETWEEN  ITEM_1  AND  ITEM_2 AND length(ITEM_1)=length('" + value + "')";
                        if (DBConnect.GetData(query_string, _database).Rows.Count <= 0)
                        {
                            query_string = "SELECT * FROM SFISM4.R_MO_EXT3_T@sfcodbh WHERE  '" + value + "'  BETWEEN  ITEM_1  AND  ITEM_2 AND length(ITEM_1)=length('" + value + "')";
                        }
                    }
                }
            }
            else if (_option == "RE IN/OUT")
            {
                query_string = $"select serial_number,mo_number,model_name,line_name,station_name,user_emp,doc_no,reason,out_time,status from SFISM4.R_SCRAP_IN_OUT_T where serial_number='{value}'";
            }
            else if (_option == "DATA INPUT")
            {
                string tempQuery = $"select * from sfism4.r105 where model_name like '830%' and mo_number ='{value}'";
                DataTable dtCheck = DBConnect.GetData(tempQuery, _database);
                if (dtCheck.Rows.Count == 0)
                {
                    query_string = "Select * from sfism4.r_data_input_t where mo_number =  '" + value + "'";
                }
                else
                {
                    query_string = $"select* from sfism4.r_netg_prin_all_t where macid1='Y' and mo_number= '{value}'";
                }
            }
            else if (_option == "CUST SSN")
            {
                query_string = "select * from sfism4.R_CUSTSN_T where serial_number = '" + value + "' " +
    "or ssn1 = '" + value + "'  OR SSN2 = 'E" + value + "' OR SSN3 = '" + value + "' " +
    " OR SSN4 = '" + value + "' OR SSN5 = '" + value + "' OR MO_NUMBER = '" + value + "' " +
    " OR MAC1 = '" + value + "' OR MAC2 = '" + value + "' OR MAC3 = '" + value + "' " +
    " OR MAC4 = '" + value + "' OR MAC5 = '" + value + "' OR SSN6 = '" + value + "' " +
    " OR SSN7 = '" + value + "' OR SSN8 = '" + value + "' OR SSN9 = '" + value + "' " +
    " OR SSN10 = '" + value + "' OR SSN11 = '" + value + "' OR SSN12 = '" + value + "' ";
            }
            else if (_option == "NOT INPUT")
            {
                if (_database == "UI" || _database == "B06")
                {
                    query_string = " select * from sfism4.R_WPAKEY_WPSPIN_T where serial_number not in (select serial_number from sfism4.R107 where mo_number= '" + value + "') and mo_number= '" + value + "'";
                }
                else if (_database == "CPEI" || _database == "NIC")
                {
                    query_string = " select * from sfism4.R_PRINT_INPUT_T where serial_number not in (select serial_number from sfism4.R107 where mo_number= '" + value + "') and mo_number= '" + value + "'";
                }
                else if (_database == "ROKU")
                {
                    query_string = " select * from sfism4.R_PRINT_INPUT_T where SSN1 not in (select serial_number from sfism4.R107 where mo_number= '" + value + "') and mo_number= '" + value + "'";
                }
                else if (_database == "CPEII")
                {
                    string tempQuery = $"select * from sfism4.r105 where model_name like '830%' and mo_number ='{value}'";
                    DataTable dtCheck = DBConnect.GetData(tempQuery, _database);
                    if (dtCheck.Rows.Count != 0)
                    {
                        query_string = $"SELECT*FROM ((SELECT SHIPPING_SN FROM SFISM4.R_NETG_PRIN_ALL_T WHERE MO_NUMBER= '{value}' " +
        " MINUS  " +
        $"SELECT PCB_SERIAL_NUMBER FROM SFISM4.R_SN_LINK_T WHERE PCB_MO_NUMBER='{value}' ) " +
        " MINUS  " +
        $"SELECT SERIAL_NUMBER FROM SFISM4.R107 WHERE SERIAL_NUMBER IN(SELECT SHIPPING_SN FROM SFISM4.R_NETG_PRIN_ALL_T WHERE MO_NUMBER='{value}'  " +
        " MINUS  " +
        $"SELECT PCB_SERIAL_NUMBER FROM SFISM4.R_SN_LINK_T WHERE PCB_MO_NUMBER='{value}')) ";

                    }
                    else
                    {
                        query_string = "SELECT A.MO_NUMBER,A.SERIAL_NUMBER,B.MAC1,A.MODEL_NAME,A.VERSION_CODE,A.LINE_NAME, " +
        "A.SECTION_NAME,A.GROUP_NAME,A.STATION_NAME,A.LOCATION,A.track_no,A.IN_STATION_TIME,A.IN_LINE_TIME, " +
        "A.SHIPPING_SN,A.SPECIAL_ROUTE,A.PALLET_NO,A.QA_NO,A.QA_RESULT,A.SCRAP_FLAG, " +
        "A.NEXT_STATION,A.CUSTOMER_NO,A.BOM_NO,A.PO_NO,A.KEY_PART_NO,A.CARTON_NO,A.REWORK_NO, " +
        "A.EMP_NO,A.PO_LINE,A.PALLET_FULL_FLAG,A.ATE_STATION_NO,A.GROUP_NAME_CQC,A.MSN,A.IMEI,A.MCARTON_NO,A.SO_NUMBER, " +
        "A.SO_LINE,A.STOCK_NO,A.TRAY_NO,A.SHIP_NO,A.WIP_GROUP ,A.SHIPPING_SN2 FROM SFISM4.R_WIP_UNDO_T A, SFISM4.R_DATA_INPUT_T B " +
        $" WHERE A.MODEL_NAME LIKE 'I01X%.%' AND A.MO_NUMBER = '{value}'  " +
        " AND A.SERIAL_NUMBER=B.SSN1  " +
        " AND A.SERIAL_NUMBER NOT IN (SELECT SERIAL_NUMBER FROM SFISM4.R107 WHERE MODEL_NAME LIKE 'I01X%.%') ";

                    }

                }
            }
            else if (_option == "REWORK NO")
            {
                query_string = "SELECT * from sfism4.r_wip_tracking_t where Rework_no = '" + value + "'";
            }
            else if (_option == "HOLD REASON")
            {
                query_string = "select * from SFISM4.R_SYSTEM_HOLD_T a LEFT join SFISM4.R_SYSTEM_LOG_T b on A.HOLD_TIME = b.time where A.SERIAL_NUMBER = '" + value + "'";
            }
            else if (_option == "LINK PANEL")
            {
                query_string = "SELECT * FROM SFISM4.R_SN_LINK_T WHERE SERIAL_NUMBER = '" + value + "' OR PCB_SERIAL_NUMBER = '" + value + "'";
            }
            else if (_option == "LINK CARRIER")
            {
                query_string = "SELECT H1DVNO CARRIER_SN, " +
        "       H1CPO ANTENNA_SN, " +
        "       TIMESTAMP ANTENNA_NO, " +
        "       CREATE_DATE LINK_TIME, " +
        "       FROMTYPE GROUP_NAME " +
        "  FROM SFISM4.R_EDI_AA940H " +
        " WHERE H1DVNO = '" + value + "' OR H1CPO = '" + value + "'";

            }
            else if (_option.ToUpper() == "REPAIR")
            {
                string _queryy = "SELECT * FROM SFISM4.R107 WHERE (SERIAL_NUMBER = '" + value + "' or PO_NO ='" + value + "' OR SHIPPING_SN = '" + value + "' OR SHIPPING_SN2 ='" + value + "')  AND ( WIP_GROUP like 'R_%'  OR WIP_GROUP IN('BC8M','BC9M'))";
                DataTable dt_temp = DBConnect.GetData(_queryy, _database);
                if (dt_temp.Rows.Count == 0)
                {
                    query_string = "";
                }
                else
                {
                    string serial_number = dt_temp.Rows[0]["SERIAL_NUMBER"].ToString();
                    query_string = $"SELECT * FROM SFISM4.R109 WHERE SERIAL_NUMBER = '" + serial_number + "'";
                    dt_temp = DBConnect.GetData(query_string, _database);
                    if (dt_temp.Rows.Count == 0)
                    {
                        query_string = $"SELECT * FROM SFISM4.R_REPAIR_T_BAK WHERE SERIAL_NUMBER = '" + serial_number + "' ";
                    }
                }
            }
            else if (_option == "HistoryByDN")
            {
                query_string = "SELECT d.INVOICE,  " +
        "       d.SERIAL_NUMBER,  " +
        "       d.GROUP_NAME,  " +
        "       TO_CHAR (d.IN_STATION_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_STATION_TIME, " +
        "       e.DATA2 as LANID " +
        "  FROM (SELECT C.INVOICE,  " +
        "               A.SERIAL_NUMBER,  " +
        "               A.GROUP_NAME,  " +
        "               A.IN_STATION_TIME,  " +
        "               RANK ()  " +
        "               OVER (PARTITION BY A.SERIAL_NUMBER, A.GROUP_NAME  " +
        "                     ORDER BY A.IN_STATION_TIME DESC)  " +
        "                  ranker  " +
        "          FROM sfism4.R117 A, sfism4.Z107 B, SFISM4.R_BPCS_INVOICE_T C  " +
        "         WHERE     A.SERIAl_NUMBER = B.SERIAL_NUMBER  " +
        "               AND B.SHIP_NO = C.TCOM  " +
        $"               AND C.INVOICE = '{value}') d inner join SFISM4.R_LANDIS_DATA_T  e  " +
        "on d.serial_number = e.serial_number " +
        "where d.ranker = 1 and e.PRINTER_FLAG = 'Y' and e.WORK_FLAG = 'O' AND DATA1='UUT_PASS' ";

            }
            else if (_option == "INVOICE OR TO")
            {
                query_string = $" select * from SFISM4.R_EDI_AA940H2 where H2DVNO = '{value}'";
            }
            else if (_option == "RepairByDN")
            {
                query_string = "SELECT C.* " +
        "  FROM SFISM4.R_BPCS_INVOICE_T A, sfism4.z107 B, sfism4.r109 C " +
        $" WHERE     A.INVOICE = '{value}' " +
        "       AND A.TCOM = B.SHIP_NO " +
        "       AND B.SERIAL_NUMBER = C.SERIAL_NUMBER ";


            }

            DataTable dt = DBConnect.GetData(query_string, _database);
            if (dt.Rows.Count > 0)
            {
                if (_option == "Serial")
                {
                    sub_query = "SELECT * FROM SFISM4.R_WIP_KEYPARTS_T " +
                    "WHERE SERIAL_NUMBER = '" + dt.Rows[0]["SERIAL_NUMBER"] + "' OR KEY_PART_SN = '" + dt.Rows[0]["SERIAL_NUMBER"] + "'";
                }
                else if (_option == "Keypart")
                {
                    sub_query = "SELECT * FROM SFISM4.R_WIP_KEYPARTS_T " +
                   " WHERE SERIAL_NUMBER = '" + dt.Rows[0]["SERIAL_NUMBER"] + "' OR KEY_PART_SN = '" + dt.Rows[0]["SERIAL_NUMBER"] + "'";
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
                else if (_option == "Tray")
                {
                    sub_query = "SELECT * FROM SFISM4.R_WIP_KEYPARTS_T " +
                   " WHERE SERIAL_NUMBER = '" + dt.Rows[0]["SERIAL_NUMBER"] + "' OR KEY_PART_SN = '" + dt.Rows[0]["SERIAL_NUMBER"] + "'";
                }
                else if (_option == "Route")
                {
                    sub_query = "SELECT * FROM SFIS1.C_ROUTE_CONTROL_T WHERE ROUTE_CODE = '" + value + "' ORDER BY STEP_SEQUENCE";
                }
                else if (_option == "SN")
                {
                    sub_query = "SELECT * FROM SFISM4.R_WIP_KEYPARTS_T " +
                    "WHERE SERIAL_NUMBER = '" + dt.Rows[0]["SERIAL_NUMBER"] + "' OR KEY_PART_SN = '" + dt.Rows[0]["SERIAL_NUMBER"] + "'";
                }
                else if (_option == "Carton")
                {
                    sub_query = "SELECT * FROM SFISM4.R_WIP_KEYPARTS_T " +
                   " WHERE SERIAL_NUMBER = '" + dt.Rows[0]["SERIAL_NUMBER"] + "' OR KEY_PART_SN = '" + dt.Rows[0]["SERIAL_NUMBER"] + "'";
                }
                else if (_option == "IMEI")
                {
                    sub_query = "SELECT * FROM SFISM4.R_WIP_KEYPARTS_T " +
                   " WHERE SERIAL_NUMBER = '" + dt.Rows[0]["SERIAL_NUMBER"] + "' OR KEY_PART_SN = '" + dt.Rows[0]["SERIAL_NUMBER"] + "'";
                }
                else if (_option == "Cpanel")
                {
                    sub_query = "";
                }
                else if (_option == "IP")
                {
                    sub_query = "";
                }
                else if (_option == "MO_RANGE")
                {
                    sub_query = "SELECT * FROM SFISM4.R_MO_EXT_T WHERE MO_NUMBER = '" + value + "'";
                }
                else if (_option == "MAC/SSN RANGER")
                {
                    sub_query = "";
                }
                else if (_option == "RE IN/OUT")
                {
                    sub_query = "";
                }
                else if (_option == "DATA INPUT")
                {
                    sub_query = "";
                }
                else if (_option == "CUST SSN")
                {
                    sub_query = "";
                }
                else if (_option == "NOT INPUT")
                {
                    sub_query = "";
                }
                else if (_option == "REWORK NO")
                {
                    sub_query = "";
                }
            }
            DataTable dt1 = DBConnect.GetData(sub_query, _database);
            return Request.CreateResponse(HttpStatusCode.OK, new { data = dt, query = query_string, data1 = dt1, result = "ok" });
        }
        [System.Web.Http.Route("QueryListShip")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> QueryListShip(ValueInputQuery6 valueInput)
        {
            string _database = valueInput.database;
            List<ValueInputQuery6.ListInput> listInput = valueInput.list_input;
            string inputvalue = "";

            for (int i = 0; i < listInput.Count; i++)
            {
                if (i == 0)
                {
                    inputvalue += "'" + listInput[i].input + "'";
                }
                else
                {
                    inputvalue += ",'" + listInput[i].input + "'";
                }
            }
            if (listInput.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
            string sql = string.Format(@"select a.MODEL_NAME,a.MO_NUMBER,a.VERSION_CODE,b.IMEI,a.MCARTON_NO,b.SHIPPING_SN,a.SERIAL_NUMBER ,f.LICENSE_NO,d.SHIP_ADDRESS,c.FINISH_DATE,b.SHIP_NO,d.DN_NO from sfism4.r107 a
                                        left join sfism4.z107 b
                                        on a.serial_number = b.serial_number
                                        left join SFISM4.R_BPCS_INVOICE_T c
                                        on b.SHIP_NO = c.tcom
                                        left join SFISM4.R_SAP_DN_DETAIL_T d
                                        on c.INVOICE = d.DN_NO
                                        left join SFISM4.R_SEC_LIC_LINK_T f
                                        on a.mcarton_no = f.carton_no
                                         where a.serial_number in '{0}'
                                         order by a.mo_number", inputvalue);
            DataTable dt = DBConnect.GetData(sql, valueInput.database);
            return Request.CreateResponse(HttpStatusCode.OK, new { data = dt, query = sql, result = "ok" });
        }
        [System.Web.Http.Route("QueryList")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> QueryList(ValueInputQuery6 valueInput)
        {
            string _database = valueInput.database;
            List<ValueInputQuery6.ListInput> listInput = valueInput.list_input;
            string sql = "";
            string _table = valueInput.table;
            string inputvalue = "";
            for (int i = 0; i < listInput.Count; i++)
            {
                if (i == 0)
                {
                    inputvalue += "'" + listInput[i].input + "'";
                }
                else
                {
                    inputvalue += ",'" + listInput[i].input + "'";
                }
            }
            if (listInput.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
            switch (_table)
            {
                case "WIP":
                    sql = " select * FROM SFISM4.R107 where serial_number in (" + inputvalue + ") or shipping_sn in (" + inputvalue + ") or shipping_sn2 in (" + inputvalue + ") or po_no in (" + inputvalue + ") ";
                    break;
                case "DETAIL":
                    if (valueInput.database == "CPEII" || valueInput.database == "CPEI" || valueInput.database == "NIC")
                    {

                        sql = "select * from (select * from sfism4.R117 where SERIAL_NUMBER in (" + inputvalue + ")  UNION ALL select * from sfism4.R117@sfcodbh where SERIAL_NUMBER in (" + inputvalue + ") UNION ALL select * from sfism4.R_SN_DETAIL_T_20200701@sfcodbh where SERIAL_NUMBER in (" + inputvalue + ")) order by serial_number,in_station_time asc ";

                    }
                    else
                    {
                        sql = "select * from (select * from sfism4.R117 where SERIAL_NUMBER in (" + inputvalue + ") UNION ALL select * from sfism4.R117@sfcodbh where SERIAL_NUMBER in (" + inputvalue + ")) order by serial_number,in_station_time asc";
                    }
                    //sql = " select * FROM SFISM4.R_SN_DETAIL_T where SERIAL_NUMBER in (" + inputvalue + ")  ";
                    break;
                case "DETAILSSN":
                    sql = "select * from (select * from sfism4.R117 where SHIPPING_SN in (" + inputvalue + ")  )order by serial_number,in_station_time asc ";
                    //sql = " select * FROM SFISM4.R_SN_DETAIL_T where SERIAL_NUMBER in (" + inputvalue + ")  ";
                    break;
                case "CUSTSN_DATA":
                    sql = " select * FROM SFISM4.R_custsn_t where SERIAL_NUMBER in (" + inputvalue + ") or ssn1 in (" + inputvalue + ") or mac1 in (" + inputvalue + ")";
                    break;
                case "DATA_INPUT":
                    sql = " select * FROM SFISM4.R_data_input_t where SSN1 in (" + inputvalue + ")";
                    break;
                case "REPAIR_DATA":
                    sql = " select SERIAL_NUMBER, MO_NUMBER, MODEL_NAME, TEST_TIME, TEST_CODE, TEST_STATION, TEST_GROUP, TEST_SECTION, TEST_LINE, TESTER, REPAIRER, REPAIR_TIME, REASON_CODE, REPAIR_STATION, REPAIR_GROUP, REPAIR_SECTION, REPAIR_STATUS, DUTY_STATION, DUTY_TYPE, ERROR_ITEM_CODE, ITEM_DESC, RECORD_TYPE, SECTION_FLAG, MACHINE, SUPPLIER, SUPPLIER_MODEL, DATE_CODE, MEMO, SOLDER_COUNT, DATECODE as  LOT_CODE, DESCRIP, TEST_VALUE, LOCATION_CODE, ATE_STATION_NO, T_WORK_SECTION, T_CLASS, T_CLASS_DATE, R_WORK_SECTION, R_CLASS, R_CLASS_DATE, EC_EXT, MOVE_FLAG FROM SFISM4.R_REPAIR_T where SERIAL_NUMBER  in (" + inputvalue + ") ";
                    break;
                case "LINK_DATA(SN)":
                    sql = " select * FROM SFISM4.R_WIP_KEYPARTS_T where serial_number in (" + inputvalue + ") ";
                    break;
                case "HISTORY_LINK_DATA(SN)":
                    sql = " select * FROM SFISM4.P_WIP_KEYPARTS_T where serial_number in (" + inputvalue + ") ";
                    break;
                case "HISTORY_LINK_DATA(KP)":
                    sql = " select * FROM SFISM4.P_WIP_KEYPARTS_T where key_part_sn in (" + inputvalue + ") ";
                    break;
                case "LINK_DATA(KP)":
                    sql = " select * FROM SFISM4.R_WIP_KEYPARTS_T where key_part_sn in (" + inputvalue + ") ";
                    break;
                case "LINK_H(SN)":
                    sql = " select * FROM SFISM4.R_WIP_KEYPARTS_T_BAK where serial_number in (" + inputvalue + ") or key_part_sn in (" + inputvalue + ") ";
                    break;
                case "LINK_H(KP)":
                    sql = " select * FROM SFISM4.R_WIP_KEYPARTS_T_BAK where KEY_PART_SN in (" + inputvalue + ") ";
                    break;
                case "FG(SERIAL_NUMBER)":
                    sql = " select * FROM SFISM4.Z107 where SERIAL_NUMBER in (" + inputvalue + ") or shipping_sn in (" + inputvalue + ") or shipping_sn2 in (" + inputvalue + ")  ";
                    break;
                case "DN(SHIP_NO)":
                    sql = " select * FROM SFISM4.R_BPCS_INVOICE_T where TCOM  in (" + inputvalue + ") OR INVOICE  in (" + inputvalue + ")  OR MODEL_NAME  in (" + inputvalue + ") ";
                    break;
                case "FG(SHIPPING_SN)":
                    sql = " select * FROM SFISM4.Z107 where shipping_sn   in (" + inputvalue + ")";
                    break;
                case "Labelroom_Data":

                    StringBuilder sb = new StringBuilder();
                    List<string> listObj = new List<string>();
                    foreach (var item in listInput)
                    {
                        if (item.input.Length >= 53)
                        {
                            listObj.Add(item.input.Substring(36, 17).Trim().Replace(":", ""));
                            continue;
                        }
                        listObj.Add(item.input);

                    }
                    inputvalue = string.Join(",", listObj.Select(p => $"'{p}'"));
                    sql = "SELECT * FROM SFISM4.R_NETG_PRIN_ALL_T WHERE SHIPPING_SN in (" + inputvalue + ") OR MACID in (" + inputvalue + ")";
                    break;
                case "Tru_PGI":
                    sql = "select serial_number, mo_number,model_name,test_time,test_code,test_group,test_section,test_line,tester from sfism4.r_repair_t where serial_number in ( " +
" select distinct serial_number from sfism4.r107 where wip_group='BC9M') AND model_name in (" + inputvalue + ")  " +
" UNION  " +
" select serial_number, mo_number,model_name,test_time,test_code,test_group,test_section,test_line,tester from sfism4.r_repair_t_bak where serial_number in ( " +
" select distinct serial_number from sfism4.r107 where wip_group='BC9M') AND model_name in (" + inputvalue + ") ";
                    break;

                case "BC9M":
                    sql = " select serial_number, mo_number,model_name,test_time,test_code,test_group,test_section,test_line,tester from sfism4.r_repair_t where serial_number in ( " +
" select distinct serial_number from sfism4.r107 where wip_group='BC9M') AND model_name in (" + inputvalue + ") " +
" UNION " +
" select serial_number, mo_number,model_name,test_time,test_code,test_group,test_section,test_line,tester from sfism4.r_repair_t_bak where serial_number in (  " +
" select distinct serial_number from sfism4.r107 where wip_group='BC9M') AND model_name in (" + inputvalue + ") ";
                    break;
                case "Vendor_Shipping_File":
                    sql = "select * from sfism4.r_camera_t where to_char(MANUAFACTURE_TIME,'YYYY-MM-DD') in( " +
                          " select to_char(MANUAFACTURE_TIME,'YYYY-MM-DD') from sfism4.r_camera_t where serial_number in (" + inputvalue + "))";
                    break;
                case "R_SYSTEM_HOLD_T":
                    sql = "select * from SFISM4.R_SYSTEM_HOLD_T a LEFT join SFISM4.R_SYSTEM_LOG_T b on A.HOLD_TIME = b.time where a.serial_number in (" + inputvalue + ")";
                    break;
                case "LINKED_WIP_SI":
                    sql = " SELECT serial_number,mo_number,model_name,in_station_time,group_name,wip_group,special_route FROM SFISM4.R107 WHERE  model_name in (" + inputvalue + ") and wip_group not in ('FG') and ship_no='N/A'  ";
                    break;
                case "RFID_DETAIL":
                    sql = " SELECT * FROM SFISM4.R_SEC_LIC_LINK_T WHERE  RFID in (" + inputvalue + ")  ";
                    break;
                case "FAIL_HISTORY":

                    sql = "select AA.*,'' REFRESH_TIME, '' DIFF_TIME from ( " +
        "select distinct P.*,Q.TEST_GROUP from (select * from (select A.*, rank() over (partition by serial_number order by in_station_time asc) NO from (SELECT emp_no, " +
        "       mo_number, " +
        "       model_name, " +
        "       serial_number, " +
        "       line_name, " +
        "       section_name, " +
        "       group_name, " +
        "       station_name, " +
        "       ERROR_CODE,      " +
        "       in_station_time, " +
        "       ate_station_no " +
        "  FROM SFISM4.R_FAIL_ATEDATA_T " +
        " WHERE serial_number IN (" + inputvalue + ")) A " +
        "order by serial_number,NO asc)) P " +
        "LEFT JOIN (select serial_number,test_time,TEST_GROUP,ATE_STATION_NO from SFISM4.R_REPAIR_T union all select serial_number,test_time,TEST_GROUP,ATE_STATION_NO from SFISM4.H_REPAIR_T union all select serial_number,test_time,TEST_GROUP,ATE_STATION_NO from sfism4.R109@sfcodbh) Q " +
        "ON P.SERIAL_NUMBER = Q.SERIAL_NUMBER and TO_CHAR(P.IN_STATION_TIME, 'MM/DD/YYYY HH24:MI') = TO_CHAR(Q.TEST_TIME,'MM/DD/YYYY HH24:MI') AND P.ATE_STATION_NO = Q.ATE_STATION_NO " +
        ") AA order by serial_number,no ";

                    DataTable dtbl = new DataTable();
                    dtbl = DBConnect.GetData(sql, valueInput.database);
                    bool IsRefresh = true;

                    DateTime lastTime = new DateTime();
                    DataTable mytable = new DataTable();
                    mytable.Columns.Add("emp_no");
                    mytable.Columns.Add("mo_number");
                    mytable.Columns.Add("model_name");
                    mytable.Columns.Add("serial_number");
                    mytable.Columns.Add("line_name");
                    mytable.Columns.Add("section_name");
                    mytable.Columns.Add("group_name");
                    mytable.Columns.Add("station_name");
                    mytable.Columns.Add("ERROR_CODE");
                    mytable.Columns.Add("in_station_time");
                    mytable.Columns.Add("ATE_STATION_NO");
                    mytable.Columns.Add("NO");
                    mytable.Columns.Add("REPAIR_STATION");
                    mytable.Columns.Add("REFRESH_TIME");
                    mytable.Columns.Add("DIFF_TIME");
                    string lastStation = "";
                    int startIndex = 0;
                    for (int i = 0; i < dtbl.Rows.Count; i++)
                    {
                        DataRow dtRow = mytable.NewRow();
                        for (int j = 0; j < dtbl.Columns.Count; j++)
                        {
                            dtRow[j] = dtbl.Rows[i][j].ToString();
                        }



                        mytable.Rows.Add(dtRow);
                        startIndex++;

                        if (i == 0)
                        {
                            lastTime = DateTime.Parse(mytable.Rows[i]["IN_STATION_TIME"].ToString());
                            mytable.Rows[i]["REFRESH_TIME"] = "1";
                            lastStation = mytable.Rows[i]["GROUP_NAME"].ToString();
                            startIndex = 1;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(mytable.Rows[i - 1]["REPAIR_STATION"].ToString()))
                            {
                                mytable.Rows[i]["REFRESH_TIME"] = "1";
                                lastStation = mytable.Rows[i]["GROUP_NAME"].ToString();
                                startIndex = 1;
                            }
                            else
                            {
                                if (mytable.Rows[i]["GROUP_NAME"] != lastStation)
                                {
                                    mytable.Rows[i]["REFRESH_TIME"] = "1";
                                    lastStation = mytable.Rows[i]["GROUP_NAME"].ToString();
                                    startIndex = 1;
                                }
                                else
                                {
                                    mytable.Rows[i]["REFRESH_TIME"] = startIndex.ToString();
                                }
                            }

                            if (mytable.Rows[i]["REFRESH_TIME"].ToString() == "1")
                            {
                                lastTime = DateTime.Parse(mytable.Rows[i]["IN_STATION_TIME"].ToString());
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(mytable.Rows[i]["REPAIR_STATION"].ToString()))
                                {
                                    mytable.Rows[i]["DIFF_TIME"] = (DateTime.Parse(mytable.Rows[i]["IN_STATION_TIME"].ToString()) - lastTime).Days + " Days";
                                }
                            }


                        }

                    }
                    mytable.Columns.RemoveAt(11);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = mytable, query = sql, result = "ok" });
                case "SI_PRODUCTION_IN_LINE":
                    sql = "SELECT serial_number, " +
        "       mo_number, " +
        "       model_name, " +
        "       in_station_time, " +
        "       group_name, " +
        "       wip_group, " +
        "       special_route " +
        "  FROM SFISM4.R107 " +
        " WHERE     MODEL_NAME IN (SELECT BOM_NO " +
        "                            FROM SFIS1.Cmodel_KEYPART_T " +
        "                           WHERE key_part_no IN (" + inputvalue + ")) " +
        "       AND MODEL_NAME LIKE '%FXN%' " +
        "       AND WIP_GROUP NOT IN ('FG', " +
        "                             'LINKED', " +
        "                             'STOCKIN', " +
        "                             'HOLD-LINKED', " +
        "                             'Linked') " +
        "UNION " +
        "SELECT serial_number, " +
        "       mo_number, " +
        "       model_name, " +
        "       in_station_time, " +
        "       group_name, " +
        "       wip_group, " +
        "       special_route " +
        "  FROM SFISM4.R107@SFCODBH " +
        " WHERE     MODEL_NAME IN (SELECT BOM_NO " +
        "                            FROM SFIS1.Cmodel_KEYPART_T " +
        "                           WHERE key_part_no IN (" + inputvalue + ")) " +
        "       AND MODEL_NAME LIKE '%FXN%' " +
        "       AND WIP_GROUP NOT IN ('FG', " +
        "                             'LINKED', " +
        "                             'STOCKIN', " +
        "                             'HOLD-LINKED', " +
        "                             'Linked') ";

                    break;
                case "VSC1000":
                    sql = "SELECT DISTINCT A.IMEI AS \"Pallet ID\", " +
                            "                A.Mcarton_no AS \"Master Carton ID\", " +
                            "                B.CUST_MODEL_DESC AS \"Item Number\", " +
                            "                A.SHIPPING_SN AS \"Serial Number\", " +
                            "                A.SHIPPING_SN AS \"Top Serial Number\", " +
                            "                c.KEY_PART_SN AS \"MAC ID\", " +
                            "                'N/A' AS \"ASN Number\", " +
                            "                'N/A' AS \"Invoice Number\", " +
                            "                'N/A' AS \"Packing Slip Number\", " +
                            "                '110004724' AS \"PO Number\", " +
                            "                TO_CHAR (SYSDATE, 'YYYY/MM/DD') AS \"XFACTORY DATE\", " +
                            "                'FOXCONN' AS \"MANUFACTURER NAME\", " +
                            "                TO_CHAR (SYSDATE, 'YYYY/MM/DD') AS \"DATE OF MANUFACTURE\", " +
                            "                'VIETNAM' AS \"Country of Origin\", " +
                            "                'N/A' AS \"Org Code\", " +
                            "                H.SSN24 AS \"FA Number Level Rev\", " +
                            "                H.SSN23 AS \"FA Number\", " +
                            "                'V1' AS \"Item Number Level Rev\", " +
                            "                H.SSN9 AS \"Hardware Version\", " +
                            "                A.MO_NUMBER_OLD AS \"Firmware Version\", " +
                            "                H.SSN6 AS \"Software Version\", " +
                            "                SUBSTR (h.ssn2, 1, INSTR (H.SSN2, 'R') - 1) AS \"TA#\", " +
                            "                H.MAC4 AS \"VERISURE NODEID\", " +
                            "                (    SELECT REGEXP_SUBSTR (A.ERP_MO, " +
                            "                                           '[^,]+', " +
                            "                                           1, " +
                            "                                           LEVEL) " +
                            "                       FROM DUAL " +
                            "                      WHERE LEVEL = 4 " +
                            "                 CONNECT BY REGEXP_SUBSTR (A.ERP_MO, " +
                            "                                           '[^,]+', " +
                            "                                           1, " +
                            "                                           LEVEL) " +
                            "                               IS NOT NULL) " +
                            "                   AS \"VERISURE ITEM NUMBER\", " +
                            "                (    SELECT REGEXP_SUBSTR (A.ERP_MO, " +
                            "                                           '[^,]+', " +
                            "                                           1, " +
                            "                                           LEVEL) " +
                            "                       FROM DUAL " +
                            "                      WHERE LEVEL = 5 " +
                            "                 CONNECT BY REGEXP_SUBSTR (A.ERP_MO, " +
                            "                                           '[^,]+', " +
                            "                                           1, " +
                            "                                           LEVEL) " +
                            "                               IS NOT NULL) " +
                            "                   AS \"VERISURE ITEM REVISION\", " +
                            "                H.MAC2 AS \"VERISURE LABEL\", " +
                            "                H.MAC8 AS \"VERISURE UART KEY\", " +
                            "                h.mac11 AS \"ASSEMBLY VARIANT\", " +
                            "                h.mac5 AS \"NODETALK KEY\", " +
                            "                H.SSN5 AS \"EFR HW REVISION\", " +
                            "                'VSC1000A' AS \"MODEL ID\" " +
                            "  FROM SFISM4.R_WIP_TRACKING_T A, " +
                            "       SFISM4.R117 K, " +
                            "       SFIS1.C_CUST_SNRULE_T B, " +
                            "       SFISM4.R_WIP_KEYPARTS_T C, " +
                            "       SFISM4.R_WIP_KEYPARTS_T F, " +
                            "       SFISM4.R_CUSTSN_T H " +
                            " WHERE     C.KEY_PART_NO = 'MACID' " +
                            "       AND A.MODEL_NAME = B.MODEL_NAME " +
                            "       AND A.VERSION_CODE = B.VERSION_CODE " +
                            "       AND A.SERIAL_NUMBER = C.SERIAL_NUMBER " +
                            "       AND A.SERIAL_NUMBER = F.SERIAL_NUMBER " +
                            "       AND A.SERIAL_NUMBER = H.SERIAL_NUMBER " +
                            "       AND A.MODEL_NAME LIKE 'VSC1000%' " +
                            "       AND A.SERIAL_NUMBER = K.SERIAL_NUMBER " +
                            "       AND (   H.MAC2 IN (" + inputvalue + ") " +
                            "            OR A.SERIAL_NUMBER IN (" + inputvalue + ") " +
                            "            OR A.shipping_sn IN (" + inputvalue + ") " +
                            "            OR A.shipping_sn2 IN (" + inputvalue + ")) ";

                    break;
                case "VSC2000":
                    sql = "SELECT DISTINCT A.IMEI AS \"Pallet ID\"," +
                "A.Mcarton_no AS \"Master Carton ID\"," +
                "B.CUST_MODEL_DESC AS \"Item Number\"," +
                "A.SHIPPING_SN AS \"Serial Number\"," +
                "A.SHIPPING_SN AS \"Top Serial Number\"," +
                "c.KEY_PART_SN AS \"MAC ID\"," +
                "'N/A' AS \"ASN Number\"," +
                "'N/A' AS \"Invoice Number\"," +
                "'N/A' AS \"Packing Slip Number\"," +
                "'N/A' AS \"Container Number\"," +
                "'N/A' AS \"PO Number\"," +
                "'N/A' AS \"PO Line Number\"," +
                "TO_CHAR(SYSDATE, 'MM/DD/YYYY') AS \"Xfactory_Date\"," +
                 "'FOXCONN' AS \"Manufacturer_Name\"," +
                   "TO_CHAR(SYSDATE, 'MM/DD/YYYY')  AS \"Date of Manufacture\"," +
                 " 'VIETNAM' AS \"Country of Origin\"," +
                  "'N/A' AS \"Org Code\"," +
                  "'N/A' AS \"IMEI Number\"," +
                  "'N/A' AS \"MasterLock Number\"," +
                  "'N/A' AS \"NetworkLock Number\"," +
                  "'N/A' AS \"ServiceLock Number\"," +
                  "H.SSN24 AS \"FA Number Level Rev\"," +
                 " H.SSN23 AS \"FA Number\"," +
                  "'V1' AS \"Item Number Level Rev\"," +
                  "'N/A' AS \"WEP Key\"," +
                  "'N/A' AS \"WIFI ID\"," +
                  "'N/A' AS \"Access Code\"," +
                  "'N/A' AS \"Primary SSID\"," +
                  "'N/A' AS \"WPA Key\"," +
                  "'N/A' AS \"MAC ID Cable\"," +
                  "'N/A' AS \"MAC ID EMTA\"," +
                 " H.SSN9 AS \"Hardware Version\"," +
                 " A.MO_NUMBER_OLD AS \"Firmware Version\"," +
                  "'N/A' AS \"EAN Code\"," +
                "'N/A' AS \"Software Version\"," +
                "'N/A' AS \"SRM Password\"," +
                "'N/A' AS \"RF Mac ID\"," +
                "'N/A' AS \"MAC ID MTA\"," +
                "'N/A' AS \"MTA MAN/router MAC\"," +
                "'N/A' AS \"MTAdata MAC\"," +
                "'N/A' AS \"Ethernet MAC\"," +
                "'N/A' AS \"USB MAC\"," +
                "'N/A' AS \"PrimarySSID PassPhrase\"," +
                "'N/A' AS \"CMCI MAC\"," +
                "'N/A' AS \"LAN MAC\"," +
                "'N/A' AS \"WAN MAC\"," +
                "'N/A' AS \"DEVICE MAC\"," +
                "'N/A' AS \"Wireless MAC\"," +
                "'N/A' AS \"WiFi MAC SSID1\"," +
                "'N/A' AS \"SSID1\"," +
                "'N/A' AS \"SSID1 Passphrase\"," +
                "'N/A' AS \"WPA Passphrase\"," +
                "'N/A' AS \"WPS PIN Code\"," +
                "'N/A' AS \"PPPoA Username\"," +
                "'N/A' AS \"PPPoA Passphrase\"," +
                "'N/A' AS \"TR-069 Unique Key 64 Bit\"," +
                "'N/A' AS \"FON KEY\"," +
                "SUBSTR(h.ssn2, 1, INSTR(H.SSN2, 'R') - 1) AS \"TA#\"," +
              "'N/A' AS \"Power Adapter SN\"," +
              "'N/A' AS \"Power Adapter SKU\"," +
              "'N/A' AS \"Optus SAP_NO (PO)\"," +
              "'N/A' AS \"5G_MAC_ID\"," +
              "'N/A' AS \"ACS_PASSWORD\"," +
              "'N/A' AS \"MEID_HEX\"," +
              "'N/A' AS \"MEID_DEC\"," +
              "'N/A' AS \"PRL\"," +
              "'N/A' AS \"iControl Software Version\"," +
              "'N/A' AS \"ICCID\"," +
              "'N/A' AS \"DEVICE_ID\"," +
              "'N/A' AS \"UNIQUE_ID\"," +
              "'N/A' AS \"OUI\"," +
              "'N/A' AS \"2G_MAC_ID\"," +
              "H.MAC4 AS \"Node ID\"," +
              "H.MAC6 AS \"CRYPT_KEY\"," +
              "H.MACID7 AS \"HASH_KEY\"," +
              "H.MAC2 AS \"LABEL\"," +
              "H.MAC8 AS \"UART_KEY\"," +
              "h.mac11 AS \"Assembly Variant\"," +
              "h.mac5 AS \"Nodetalk Key\"," +
              "'N/A' AS \"EFR HW revision\"," +
                 "'N/A' AS \"Attribute9\"," +
                "'N/A' AS \"Attribute10\"," +
                "'N/A' AS \"Attribute11\"," +
                "'N/A' AS \"Attribute12\"," +
                "'N/A' AS \"Attribute13\"," +
                "'N/A' AS \"Attribute14\"," +
                "'N/A' AS \"Attribute15\"," +
                "'N/A' AS \"Attribute16\"," +
                "'N/A' AS \"SERIAL_NUMBER_01\"," +
                "'N/A' AS \"SERIAL_NUMBER_02\"," +
                "'N/A' AS \"SERIAL_NUMBER_03\"," +
                "'N/A' AS \"SERIAL_NUMBER_04\"," +
                "'N/A' AS \"SERIAL_NUMBER_05\"," +
                "'N/A' AS \"SERIAL_NUMBER_06\"," +
                "'N/A' AS \"SERIAL_NUMBER_07\"," +
                "'N/A' AS \"SERIAL_NUMBER_08\"," +
                "'N/A' AS \"SERIAL_NUMBER_09\"," +
                "'N/A' AS \"BT_MAC_ADDRESS\"," +
               "H.MAC3 AS \"CER_ID\"," +
                "'N/A' AS \"BT_BRIDGE_SECURE_KEY\"," +
                "'N/A' AS \"APPLE_HOME_KEY\"," +
                "'N/A' AS \"LIGHTS_AUTHENTIC_CODE\"," +
                "'N/A' AS \"LIGHTS_UUID\"," +
                "'N/A' AS \"BATTERY_SN\"," +
                "'N/A' AS \"INTERFAC_VERSION\"," +
                "'VSC2000A' AS \"MODEL_ID\"" +
    " FROM SFISM4.R_WIP_TRACKING_T A," +
       " SFISM4.R117 K," +
       " SFIS1.C_CUST_SNRULE_T B," +
       "SFISM4.R_WIP_KEYPARTS_T C," +
       "SFISM4.R_WIP_KEYPARTS_T F," +
       "SFISM4.R_CUSTSN_T H " +
    " WHERE C.KEY_PART_NO = 'MACID' " +
       " AND A.MODEL_NAME = B.MODEL_NAME " +
       " AND A.VERSION_CODE = B.VERSION_CODE " +
       " AND A.SERIAL_NUMBER = C.SERIAL_NUMBER " +
       " AND A.SERIAL_NUMBER = F.SERIAL_NUMBER " +
       " AND A.SERIAL_NUMBER = H.SERIAL_NUMBER " +
       " AND A.MODEL_NAME LIKE 'VSC2000%' " +
       " AND A.SERIAL_NUMBER = K.SERIAL_NUMBER " +
      " AND  ( H.MAC2 IN(" + inputvalue + ") or A.SERIAL_NUMBER IN (" + inputvalue + ") " +
               "or A.shipping_sn in (" + inputvalue + ") or A.shipping_sn2 in (" + inputvalue + ") )";
                    break;
                default:
                    sql = " select * FROM SFISM4.R107 where serial_number in (" + inputvalue + ")";
                    break;
                // hau add export rate smt error
                case "RATE_SMT_ER":

                    sql = "select * from   TABLE(PKG_RETURN_TABLE.F_GET_SMT_RATE_ER('" + listInput[0].input + "')) ";
                    break;

                case "Not_Link_SI":

                    sql = " select * from  table(PKG_RETURN_TABLE.F_GET_SMT_ER_NOT_LINK('" + listInput[0].input + "')) ";
                    break;
            }
            DataTable dt = DBConnect.GetData(sql, valueInput.database);
            return Request.CreateResponse(HttpStatusCode.OK, new { data = dt, query = sql, result = "ok" });
        }

        //[System.Web.Http.Route("SendMessage")]
        //[System.Web.Http.HttpPost]
        //public async Task<HttpResponseMessage> SendMessage(Message message)
        //{
        //    string key = "xFUfgkitlozH";
        //    using (var client = new HttpClient())
        //    {
        //        string URL = "http://10.224.81.136:8015/api/SendMessage";

        //        string urlParameters = "?key=" + key + "&group=" + message.GroupName + "&message=" + message.MessageContent + " ";

        //        client.BaseAddress = new Uri(URL);

        //        // Add an Accept header for JSON format.
        //        client.DefaultRequestHeaders.Accept.Add(
        //        new MediaTypeWithQualityHeaderValue("application/json"));

        //        // List data response.
        //        HttpResponseMessage response = client.GetAsync(urlParameters).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, new { status = "ok" });
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, new { status = "fail" });
        //        }
        //    }
        //}

        [System.Web.Http.Route("Query")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Query(ValueInputQuery6 valueInput)
        {
            //Request.Content.Headers.Add("Access-Control-Allow-Origin", "*");
            //Response.Headers.Add("Access-Control-Allow-Origin", "*");
            string _database = valueInput.database;
            DataTable dt = null;
            DataTable dt2 = null;
            List<ValueInputQuery6.ListInput> listInput = valueInput.list_input;
            List<ValueInputQuery6.ListGroupName> listGroupName = valueInput.list_group_name;
            string _queryString = "";
            string inputvalue = "";
            string inputGroupName = "";
            for (int i = 0; i < listInput.Count; i++)
            {
                if (i == 0)
                {
                    inputvalue += "'" + listInput[i].input + "'";
                }
                else
                {
                    inputvalue += ",'" + listInput[i].input + "'";
                }
            }
            try
            {
                for (int i = 0; i < listGroupName.Count; i++)
                {
                    if (i == 0)
                    {
                        inputGroupName += "'" + listGroupName[i].input + "'";
                    }
                    else
                    {
                        inputGroupName += ",'" + listGroupName[i].input + "'";
                    }
                }
            }
            catch
            {

            }
            if (listInput.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });

            string _group_name_condition = "";

            if (inputGroupName != "") _group_name_condition = "AND GROUP_NAME IN (" + inputGroupName + ") ";

            if (valueInput.table == "R117")
            {
                if (valueInput.date_from != null)
                {
                    _queryString = "select  SERIAL_NUMBER,SECTION_FLAG,MO_NUMBER,MODEL_NAME,TYPE,VERSION_CODE,LINE_NAME,SECTION_NAME,GROUP_NAME,WIP_GROUP,STATION_NAME,LOCATION,STATION_SEQ,ERROR_FLAG,TO_CHAR (IN_STATION_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_STATION_TIME,TO_CHAR (IN_LINE_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_LINE_TIME,OUT_LINE_TIME,SHIPPING_SN,WORK_FLAG,FINISH_FLAG,ENC_CNT,SPECIAL_ROUTE,PALLET_NO,CONTAINER_NO,QA_NO,QA_RESULT,SCRAP_FLAG,NEXT_STATION,CUSTOMER_NO,BOM_NO,BILL_NO,TRACK_NO,PO_NO,KEY_PART_NO,CARTON_NO,WARRANTY_DATE,REWORK_NO,REPAIR_CNT,EMP_NO,PO_LINE,PALLET_FULL_FLAG,PMCC,GROUP_NAME_CQC,MO_NUMBER_OLD,ERP_MO,ATE_STATION_NO,MSN,IMEI,JOB,MCARTON_NO,SO_NUMBER,SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,SHIPPING_SN2 from sfism4.R117 where " + valueInput.field + " like ('" + listInput[0].input + "%') " + _group_name_condition + " AND in_station_time between TO_DATE('" + valueInput.date_from + "','YYYY/MM/DD HH24:MI') and TO_DATE('" + valueInput.date_to + "','YYYY/MM/DD HH24:MI') ";
                }
                else
                {
                    _queryString = "select  SERIAL_NUMBER,SECTION_FLAG,MO_NUMBER,MODEL_NAME,TYPE,VERSION_CODE,LINE_NAME,SECTION_NAME,GROUP_NAME,WIP_GROUP,STATION_NAME,LOCATION,STATION_SEQ,ERROR_FLAG,TO_CHAR (IN_STATION_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_STATION_TIME,TO_CHAR (IN_LINE_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_LINE_TIME,OUT_LINE_TIME,SHIPPING_SN,WORK_FLAG,FINISH_FLAG,ENC_CNT,SPECIAL_ROUTE,PALLET_NO,CONTAINER_NO,QA_NO,QA_RESULT,SCRAP_FLAG,NEXT_STATION,CUSTOMER_NO,BOM_NO,BILL_NO,TRACK_NO,PO_NO,KEY_PART_NO,CARTON_NO,WARRANTY_DATE,REWORK_NO,REPAIR_CNT,EMP_NO,PO_LINE,PALLET_FULL_FLAG,PMCC,GROUP_NAME_CQC,MO_NUMBER_OLD,ERP_MO,ATE_STATION_NO,MSN,IMEI,JOB,MCARTON_NO,SO_NUMBER,SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,SHIPPING_SN2 from sfism4.R117 where " + valueInput.field + " like ('" + listInput[0].input + "%') " + _group_name_condition + "  ";
                }
                //}
            }
            else if (valueInput.table == "ATEDATA")
            {

               _queryString = "select * from SFISM4.R_TMP_ATEDATA_T_BAK where " + valueInput.field + " like ('" + listInput[0].input + "%') ";

            }
            else if (valueInput.table == "WipSMT")
            {
                _queryString = "SELECT C.MO_NUMBER, " +
        "       E.MO_START_DATE IN_LINE_TIME, " +
        "       C.SMT_QTY, " +
        "(SELECT COUNT (*) " +
        "          FROM SFISM4.R_WIP_TRACKING_T t " +
        "         WHERE     MO_NUMBER = C.MO_NUMBER AND WIP_GROUP ='STOCKIN' " +
        "               AND serial_number  NOT IN (SELECT key_part_sn " +
        "                                           FROM sfism4.r108 " +
        "                                          WHERE key_part_sn = t.serial_number " +
        "                                         UNION " +
        "                                         SELECT key_part_sn FROM sfism4.h108 WHERE  key_part_sn = t.serial_number)) NOTLINK_QTY, " +
                "       NVL (REPAIR_QTY, '0') REPAIR_QTY, " +
        "          RTRIM ( " +
        "             TO_CHAR (NVL (ROUND (REPAIR_QTY / SMT_QTY * 100, 2), '0'), " +
        "                      'FM999999999999990.99'), " +
        "             '.') " +
        "       || ' %' " +
        "          RATE, " +
        "       NVL (G.RE_QTY, '0') RE_QTY, " +
        "       NVL (F.QTY_REFAIL, '0') QTY_REFAIL, " +
        "          RTRIM ( " +
        "             TO_CHAR (NVL (ROUND (QTY_REFAIL / RE_QTY * 100, 2), '0'), " +
        "                      'FM999999999999990.99'), " +
        "             '.') " +
        "       || ' %' " +
        "          DFR " +
        "  FROM (  SELECT MO_NUMBER, COUNT (*) SMT_QTY " +
        "            FROM SFISM4.R107 " +
        "           WHERE SERIAL_NUMBER IN (SELECT SUBSTR (SERIAL_NUMBER, 2, 7) " +
        "                                     FROM SFISM4.R117 " +
        "                                    WHERE     (IN_STATION_TIME BETWEEN TO_DATE ( " +
        "                                                                          '" + valueInput.date_from + "', " +
        "                                                                          'YYYY/MM/DD HH24:MI:SS') " +
        "                                                                   AND TO_DATE ( " +
        "                                                                          '" + valueInput.date_to + "', " +
        "                                                                          'YYYY/MM/DD HH24:MI:SS')) " +
        "                                          AND MODEL_NAME = '" + listInput[0].input + "' " +
        "                                          AND GROUP_NAME = '" + listGroupName[0].input + "') " +
        "        GROUP BY MO_NUMBER) C, " +
        "       (  SELECT MO_NUMBER, COUNT (*) REPAIR_QTY " +
        "            FROM (SELECT B.MO_NUMBER, " +
        "                         A.SERIAL_NUMBER, " +
        "                         RANK () " +
        "                         OVER (PARTITION BY B.MO_NUMBER, A.SERIAL_NUMBER " +
        "                               ORDER BY A.TEST_TIME ASC) " +
        "                            RANKER " +
        "                    FROM SFISM4.R109 A, SFISM4.R107 B " +
        "                   WHERE     A.SERIAL_NUMBER IN (SELECT DISTINCT SERIAL_NUMBER " +
        "                                                   FROM SFISM4.R117 " +
        "                                                  WHERE     (IN_STATION_TIME BETWEEN TO_DATE ( " +
        "                                                                                        '" + valueInput.date_from + "', " +
        "                                                                                        'YYYY/MM/DD HH24:MI:SS') " +
        "                                                                                 AND TO_DATE ( " +
        "                                                                                        '" + valueInput.date_to + "', " +
        "                                                                                        'YYYY/MM/DD HH24:MI:SS')) " +
        "                                                        AND MODEL_NAME = " +
        "                                                               '" + listInput[0].input + "' " +
        "                                                        AND GROUP_NAME = '" + listGroupName[0].input + "') " +
        "                         AND A.TEST_GROUP = '" + listGroupName[0].input + "' " +
        "                         AND SUBSTR (A.SERIAL_NUMBER, 2, 7) = B.SERIAL_NUMBER " +
        "                         AND A.TEST_TIME BETWEEN TO_DATE ( " +
        "                                                    '" + valueInput.date_from + "', " +
        "                                                    'YYYY/MM/DD HH24:MI:SS') " +
        "                                             AND TO_DATE ( " +
        "                                                    '" + valueInput.date_to + "', " +
        "                                                    'YYYY/MM/DD HH24:MI:SS') " +
        "                                                    UNION ALL " +
        "                                                    SELECT B.MO_NUMBER, " +
        "                         A.SERIAL_NUMBER, " +
        "                         RANK () " +
        "                         OVER (PARTITION BY B.MO_NUMBER, A.SERIAL_NUMBER " +
        "                               ORDER BY A.TEST_TIME ASC) " +
        "                            RANKER " +
        "                    FROM SFISM4.R_REPAIR_T_BAK A, SFISM4.R107 B " +
        "                   WHERE     A.SERIAL_NUMBER IN (SELECT DISTINCT SERIAL_NUMBER " +
        "                                                   FROM SFISM4.R117 " +
        "                                                  WHERE     (IN_STATION_TIME BETWEEN TO_DATE ( " +
        "                                                                                        '" + valueInput.date_from + "', " +
        "                                                                                        'YYYY/MM/DD HH24:MI:SS') " +
        "                                                                                 AND TO_DATE ( " +
        "                                                                                        '" + valueInput.date_to + "', " +
        "                                                                                        'YYYY/MM/DD HH24:MI:SS')) " +
        "                                                        AND MODEL_NAME = " +
        "                                                               '" + listInput[0].input + "' " +
        "                                                        AND GROUP_NAME = '" + listGroupName[0].input + "') " +
        "                         AND A.TEST_GROUP = '" + listGroupName[0].input + "' " +
        "                         AND SUBSTR (A.SERIAL_NUMBER, 2, 7) = B.SERIAL_NUMBER " +
        "                         AND A.TEST_TIME BETWEEN TO_DATE ( " +
        "                                                    '" + valueInput.date_from + "', " +
        "                                                    'YYYY/MM/DD HH24:MI:SS') " +
        "                                             AND TO_DATE ( " +
        "                                                    '" + valueInput.date_to + "', " +
        "                                                    'YYYY/MM/DD HH24:MI:SS') " +
        "                                                    ) " +
        "           WHERE RANKER = 1 " +
        "        GROUP BY MO_NUMBER) D, " +
        "       SFISM4.R105 E, " +
        "       (  SELECT MO_NUMBER, COUNT (*) QTY_REFAIL " +
        "            FROM (SELECT DISTINCT MO_NUMBER, SERIAL_NUMBER " +
        "                    FROM (SELECT * " +
        "                            FROM (SELECT DISTINCT MO_NUMBER, " +
        "                                                  SERIAL_NUMBER, " +
        "                                                  TEST_TIME, " +
        "                                                  ranker " +
        "                                    FROM (SELECT B.mo_number, " +
        "                                                 a.serial_number, " +
        "                                                 A.TEST_TIME, " +
        "                                                 RANK () " +
        "                                                 OVER ( " +
        "                                                    PARTITION BY B.mo_number, " +
        "                                                                 a.serial_number " +
        "                                                    ORDER BY A.test_time ASC) " +
        "                                                    ranker " +
        "                                            FROM SFISM4.R109 A, sfism4.R107 B " +
        "                                           WHERE     A.serial_number IN (SELECT DISTINCT " +
        "                                                                                serial_number " +
        "                                                                           FROM sfism4.R117 " +
        "                                                                          WHERE     (in_station_time BETWEEN TO_DATE ( " +
        "                                                                                                                '" + valueInput.date_from + "', " +
        "                                                                                                                'YYYY/MM/DD HH24:MI:SS') " +
        "                                                                                                         AND TO_DATE ( " +
        "                                                                                                                '" + valueInput.date_to + "', " +
        "                                                                                                                'YYYY/MM/DD HH24:MI:SS')) " +
        "                                                                                AND model_name = " +
        "                                                                                       '" + listInput[0].input + "' " +
        "                                                                                AND group_name = " +
        "                                                                                       '" + listGroupName[0].input + "') " +
        "                                                 AND A.test_group = '" + listGroupName[0].input + "' " +
        "                                                 AND SUBSTR (A.serial_number, " +
        "                                                             2, " +
        "                                                             7) = " +
        "                                                        B.serial_number)) " +
        "                          UNION " +
        "                          SELECT * " +
        "                            FROM (SELECT DISTINCT MO_NUMBER, " +
        "                                                  SERIAL_NUMBER, " +
        "                                                  TEST_TIME, " +
        "                                                  ranker " +
        "                                    FROM (SELECT B.mo_number, " +
        "                                                 a.serial_number, " +
        "                                                 A.TEST_TIME, " +
        "                                                 RANK () " +
        "                                                 OVER ( " +
        "                                                    PARTITION BY B.mo_number, " +
        "                                                                 a.serial_number " +
        "                                                    ORDER BY A.test_time ASC) " +
        "                                                    ranker " +
        "                                            FROM SFISM4.R_REPAIR_T_BAK A, " +
        "                                                 sfism4.R107 B " +
        "                                           WHERE     A.serial_number IN (SELECT DISTINCT " +
        "                                                                                serial_number " +
        "                                                                           FROM sfism4.R117 " +
        "                                                                          WHERE     (in_station_time BETWEEN TO_DATE ( " +
        "                                                                                                                '" + valueInput.date_from + "', " +
        "                                                                                                                'YYYY/MM/DD HH24:MI:SS') " +
        "                                                                                                         AND TO_DATE ( " +
        "                                                                                                                '" + valueInput.date_to + "', " +
        "                                                                                                                'YYYY/MM/DD HH24:MI:SS')) " +
        "                                                                                AND model_name = " +
        "                                                                                       '" + listInput[0].input + "' " +
        "                                                                                AND group_name = " +
        "                                                                                       '" + listGroupName[0].input + "') " +
        "                                                 AND A.test_group = '" + listGroupName[0].input + "' " +
        "                                                 AND SUBSTR (A.serial_number, " +
        "                                                             2, " +
        "                                                             7) = " +
        "                                                        B.serial_number))) " +
        "                   WHERE     RANKER > 1 " +
        "                         AND TEST_TIME BETWEEN TO_DATE ( " +
        "                                                  '" + valueInput.date_from + "', " +
        "                                                  'YYYY/MM/DD HH24:MI:SS') " +
        "                                           AND TO_DATE ( " +
        "                                                  '" + valueInput.date_to + "', " +
        "                                                  'YYYY/MM/DD HH24:MI:SS')) " +
        "        GROUP BY MO_NUMBER) F, " +
        "       (  SELECT MO_NUMBER, COUNT (*) RE_QTY " +
        "            FROM (SELECT B.mo_number, " +
        "                         a.serial_number, " +
        "                         RANK () " +
        "                         OVER (PARTITION BY B.mo_number, a.serial_number " +
        "                               ORDER BY A.test_time ASC) " +
        "                            ranker " +
        "                    FROM SFISM4.R109 A, sfism4.R107 B " +
        "                   WHERE     A.serial_number IN (SELECT DISTINCT serial_number " +
        "                                                   FROM sfism4.R117 " +
        "                                                  WHERE     (in_station_time BETWEEN TO_DATE ( " +
        "                                                                                        '" + valueInput.date_from + "', " +
        "                                                                                        'YYYY/MM/DD HH24:MI:SS') " +
        "                                                                                 AND TO_DATE ( " +
        "                                                                                        '" + valueInput.date_to + "', " +
        "                                                                                        'YYYY/MM/DD HH24:MI:SS')) " +
        "                                                        AND model_name = " +
        "                                                               '" + listInput[0].input + "' " +
        "                                                        AND group_name = '" + listGroupName[0].input + "') " +
        "                         AND A.test_group = '" + listGroupName[0].input + "' " +
        "                         AND SUBSTR (A.serial_number, 2, 7) = B.serial_number) " +
        "        GROUP BY MO_NUMBER) G " +
        " WHERE     C.MO_NUMBER = D.MO_NUMBER(+) " +
        "       AND C.MO_NUMBER = E.MO_NUMBER " +
        "       AND C.MO_NUMBER = F.MO_NUMBER(+) " +
        "       AND C.MO_NUMBER = G.MO_NUMBER(+) ";

            }
            else if (valueInput.table == "Antena")
            {
                if (valueInput.field == "LINE_NAME")
                {
                    if (valueInput.date_from == null)
                    {
                        _queryString = "select distinct SERIAL_NUMBER from SFISM4.R108 WHERE GROUP_NAME in (" + inputvalue + ")  ";
                    }
                    else
                    {
                        _queryString = "select distinct SERIAL_NUMBER from SFISM4.R108 WHERE GROUP_NAME in (" + inputvalue + ")  " +
                        " AND WORK_TIME between  TO_DATE('" + valueInput.date_from + "','YYYY / MM / DD HH24: MI: SS') and TO_DATE('" + valueInput.date_to + "','YYYY/MM/DD HH24:MI:SS') ";
                    }
                }
                else
                {
                    _queryString = "select distinct h1dvno from SFISM4.R_EDI_AA940H where create_date between  TO_DATE('" + valueInput.date_from + "','YYYY/MM/DD HH24:MI:SS') and TO_DATE('" + valueInput.date_to + "','YYYY/MM/DD HH24:MI:SS')  " +
                    "union  " +
                    "select distinct SERIAL_NUMBER from SFISM4.R108 WHERE KEY_PART_NO IN( " +
                    "SELECT KEY_PART_NO FROM SFIS1.Cmodel_KEYPART_T WHERE BOM_NO LIKE'ANTENNA_'||" + inputvalue + ") " +
                    "AND WORK_TIME between  TO_DATE('" + valueInput.date_from + "','YYYY / MM / DD HH24: MI: SS') and TO_DATE('" + valueInput.date_to + "','YYYY/MM/DD HH24:MI:SS') ";
                }

            }
            else if (valueInput.table == "R117_")
            {
                string _queryString_ = "select distinct SERIAL_NUMBER,SECTION_FLAG,MO_NUMBER,MODEL_NAME,TYPE,VERSION_CODE,LINE_NAME,SECTION_NAME,GROUP_NAME,WIP_GROUP,STATION_NAME,LOCATION,STATION_SEQ,ERROR_FLAG,TO_CHAR (IN_STATION_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_STATION_TIME,TO_CHAR (IN_LINE_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_LINE_TIME,OUT_LINE_TIME,SHIPPING_SN,WORK_FLAG,FINISH_FLAG,ENC_CNT,SPECIAL_ROUTE,PALLET_NO,CONTAINER_NO,QA_NO,QA_RESULT,SCRAP_FLAG,NEXT_STATION,CUSTOMER_NO,BOM_NO,BILL_NO,TRACK_NO,PO_NO,KEY_PART_NO,CARTON_NO,WARRANTY_DATE,REWORK_NO,REPAIR_CNT,EMP_NO,PO_LINE,PALLET_FULL_FLAG,PMCC,GROUP_NAME_CQC,MO_NUMBER_OLD,ERP_MO,ATE_STATION_NO,MSN,IMEI,JOB,MCARTON_NO,SO_NUMBER,SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,SHIPPING_SN2 from sfism4.R107 where " + valueInput.field + " in (" + inputvalue + ") ";
                dt2 = DBConnect.GetData(_queryString_, valueInput.database);
                string _queryString1 = "select distinct SERIAL_NUMBER,SECTION_FLAG,MO_NUMBER,MODEL_NAME,TYPE,VERSION_CODE,LINE_NAME,SECTION_NAME,GROUP_NAME,WIP_GROUP,STATION_NAME,LOCATION,STATION_SEQ,ERROR_FLAG,TO_CHAR (IN_STATION_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_STATION_TIME,TO_CHAR (IN_LINE_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_LINE_TIME,OUT_LINE_TIME,SHIPPING_SN,WORK_FLAG,FINISH_FLAG,ENC_CNT,SPECIAL_ROUTE,PALLET_NO,CONTAINER_NO,QA_NO,QA_RESULT,SCRAP_FLAG,NEXT_STATION,CUSTOMER_NO,BOM_NO,BILL_NO,TRACK_NO,PO_NO,KEY_PART_NO,CARTON_NO,WARRANTY_DATE,REWORK_NO,REPAIR_CNT,EMP_NO,PO_LINE,PALLET_FULL_FLAG,PMCC,GROUP_NAME_CQC,MO_NUMBER_OLD,ERP_MO,ATE_STATION_NO,MSN,IMEI,JOB,MCARTON_NO,SO_NUMBER,SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,SHIPPING_SN2 from sfism4.R117 where " + valueInput.field + " in (" + inputvalue + ") " + _group_name_condition + "  order by in_station_time asc  ";
                dt = DBConnect.GetData(_queryString1, valueInput.database);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = dt, data1 = dt2, query = _queryString1, result = "ok" });
            }
            else if (valueInput.table == "R109")

            {
                string _queryString_ = "select A.SERIAL_NUMBER,A.MO_NUMBER,A.MODEL_NAME,A.TEST_TIME,C.WIP_GROUP,A.TEST_CODE,B.ERROR_DESC,B.ERROR_DESC2,A.ERROR_ITEM_CODE,A.REASON_CODE,A.REPAIRER,A.REPAIR_TIME,A.REPAIR_STATION,A.TEST_GROUP,A.LOCATION_CODE,A.TEST_STATION,A.TEST_SECTION,A.TEST_LINE,A.TESTER,A.REPAIR_GROUP,A.REPAIR_SECTION,A.REPAIR_STATUS,A.DUTY_STATION,A.DUTY_TYPE,A.ITEM_DESC,A.RECORD_TYPE,A.SECTION_FLAG,A.MACHINE,A.SUPPLIER,A.SUPPLIER_MODEL,A.DATE_CODE,A.MEMO,A.SOLDER_COUNT,A.DATECODE,A.DESCRIP,A.TEST_VALUE,A.ATE_STATION_NO,A.T_WORK_SECTION,A.T_CLASS,A.T_CLASS_DATE,A.R_WORK_SECTION,A.R_CLASS,A.R_CLASS_DATE,A.EC_EXT,A.MOVE_FLAG from  (select * from sfism4.R109 " +
                   " union " +
                   " select * from SFISM4.R_REPAIR_T_BAK ) A, SFIS1.C_ERROR_CODE_T B ,SFISM4.R107 C WHERE";
                string _queryString1 = "";
                if (valueInput.field == "SERIAL_NUMBER")
                {
                    _queryString_ = _queryString_ + " (C.SERIAL_NUMBER in  (" + inputvalue + ") OR C.SHIPPING_SN in  (" + inputvalue + ") ) AND A.TEST_CODE = B.ERROR_CODE(+) AND A.SERIAL_NUMBER = C.SERIAL_NUMBER";
                    _queryString1 = "select distinct SERIAL_NUMBER,SECTION_FLAG,MO_NUMBER,MODEL_NAME,TYPE,VERSION_CODE,LINE_NAME,SECTION_NAME,GROUP_NAME,WIP_GROUP,STATION_NAME,LOCATION,STATION_SEQ,ERROR_FLAG,TO_CHAR (IN_STATION_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_STATION_TIME,TO_CHAR (IN_LINE_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_LINE_TIME,OUT_LINE_TIME,SHIPPING_SN,WORK_FLAG,FINISH_FLAG,ENC_CNT,SPECIAL_ROUTE,PALLET_NO,CONTAINER_NO,QA_NO,QA_RESULT,SCRAP_FLAG,NEXT_STATION,CUSTOMER_NO,BOM_NO,BILL_NO,TRACK_NO,PO_NO,KEY_PART_NO,CARTON_NO,WARRANTY_DATE,REWORK_NO,REPAIR_CNT,EMP_NO,PO_LINE,PALLET_FULL_FLAG,PMCC,GROUP_NAME_CQC,MO_NUMBER_OLD,ERP_MO,ATE_STATION_NO,MSN,IMEI,JOB,MCARTON_NO,SO_NUMBER,SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,SHIPPING_SN2 from sfism4.R117 where " + valueInput.field + " in (" + inputvalue + ") " + _group_name_condition + "  order by in_station_time asc  ";
                }
                if (valueInput.field == "MO_NUMBER")
                {
                    _queryString_ = _queryString_ + " C.MO_NUMBER in (" + inputvalue + ")  AND A.TEST_CODE = B.ERROR_CODE(+) AND A.SERIAL_NUMBER = C.SERIAL_NUMBER";
                }
                if (valueInput.field == "MODEL_NAME")
                {
                    _queryString_ = _queryString_ + " C.MODEL_NAME in  (" + inputvalue + ") AND A.TEST_CODE = B.ERROR_CODE(+) AND A.SERIAL_NUMBER = C.SERIAL_NUMBER";
                }

                if (valueInput.date_from != null && valueInput.date_to != null)
                    _queryString_ = _queryString_ + " AND A.TEST_TIME between TO_DATE('" + valueInput.date_from + "','YYYY / MM / DD HH24: MI: SS') and TO_DATE('" + valueInput.date_to + "','YYYY/MM/DD HH24:MI:SS')";

                dt2 = DBConnect.GetData(_queryString_, valueInput.database);
                dt = DBConnect.GetData(_queryString1, valueInput.database);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = dt, data1 = dt2, query = _queryString1, result = "ok" });

            }
            else
            {
                if (valueInput.date_from != null)
                {
                    _queryString = "select distinct SERIAL_NUMBER,SECTION_FLAG,MO_NUMBER,MODEL_NAME,TYPE,VERSION_CODE,LINE_NAME,SECTION_NAME,GROUP_NAME,WIP_GROUP,STATION_NAME,LOCATION,STATION_SEQ,ERROR_FLAG,TO_CHAR (IN_STATION_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_STATION_TIME,TO_CHAR (IN_LINE_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_LINE_TIME,OUT_LINE_TIME,SHIPPING_SN,WORK_FLAG,FINISH_FLAG,ENC_CNT,SPECIAL_ROUTE,PALLET_NO,CONTAINER_NO,QA_NO,QA_RESULT,SCRAP_FLAG,NEXT_STATION,CUSTOMER_NO,BOM_NO,BILL_NO,TRACK_NO,PO_NO,KEY_PART_NO,CARTON_NO,WARRANTY_DATE,REWORK_NO,REPAIR_CNT,EMP_NO,PO_LINE,PALLET_FULL_FLAG,PMCC,GROUP_NAME_CQC,MO_NUMBER_OLD,ERP_MO,ATE_STATION_NO,MSN,IMEI,JOB,MCARTON_NO,SO_NUMBER,SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,SHIPPING_SN2 from sfism4." + valueInput.table + " where ( " + valueInput.field + " in (" + inputvalue + ") or serial_number in (" + inputvalue + ") or shipping_sn  in (" + inputvalue + ") or shipping_sn2 in (" + inputvalue + ") or po_no in (" + inputvalue + ") )  and in_station_time between TO_DATE('" + valueInput.date_from + "','YYYY/MM/DD HH24:MI') and TO_DATE('" + valueInput.date_to + "','YYYY/MM/DD HH24:MI') " + _group_name_condition + "";
                }
                else
                {
                    if (valueInput.table == "R107" && valueInput.field == "SHIPPING_SN2" && valueInput.database == "CPEII")
                    {
                        _queryString = "SELECT distinct SERIAL_NUMBER,SECTION_FLAG,MO_NUMBER,MODEL_NAME,TYPE,VERSION_CODE,LINE_NAME,SECTION_NAME,GROUP_NAME,WIP_GROUP,STATION_NAME,LOCATION,STATION_SEQ,ERROR_FLAG,TO_CHAR (IN_STATION_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_STATION_TIME,TO_CHAR (IN_LINE_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_LINE_TIME,OUT_LINE_TIME,SHIPPING_SN,WORK_FLAG,FINISH_FLAG,ENC_CNT,SPECIAL_ROUTE,PALLET_NO,CONTAINER_NO,QA_NO,QA_RESULT,SCRAP_FLAG,NEXT_STATION,CUSTOMER_NO,BOM_NO,BILL_NO,TRACK_NO,PO_NO,KEY_PART_NO,CARTON_NO,WARRANTY_DATE,REWORK_NO,REPAIR_CNT,EMP_NO,PO_LINE,PALLET_FULL_FLAG,PMCC,GROUP_NAME_CQC,MO_NUMBER_OLD,ERP_MO,ATE_STATION_NO,MSN,IMEI,JOB,MCARTON_NO,SO_NUMBER,SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,SHIPPING_SN2 " +
                       "  FROM sfism4.r_wip_tracking_t " +
                       " WHERE serial_number IN (SELECT serial_number " +
                       "                           FROM sfism4.r117 " +
                       "                          WHERE     shipping_sn2 in (" + inputvalue + ") " +
                       "                                AND in_station_time IN (SELECT MAX ( " +
                       "                                                                  in_station_time) " +
                       "                                                          FROM SFISM4.r117 " +
                       "                                                         WHERE shipping_sn2 in (" + inputvalue + "))) ";

                    }
                    else if (valueInput.table == "R107" || valueInput.table == "R117" || valueInput.table == "Z107")
                    {
                        _queryString = "select distinct SERIAL_NUMBER,SECTION_FLAG,MO_NUMBER,MODEL_NAME,TYPE,VERSION_CODE,LINE_NAME,SECTION_NAME,GROUP_NAME,WIP_GROUP,STATION_NAME,LOCATION,STATION_SEQ,ERROR_FLAG,TO_CHAR (IN_STATION_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_STATION_TIME,TO_CHAR (IN_LINE_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_LINE_TIME,OUT_LINE_TIME,SHIPPING_SN,WORK_FLAG,FINISH_FLAG,ENC_CNT,SPECIAL_ROUTE,PALLET_NO,CONTAINER_NO,QA_NO,QA_RESULT,SCRAP_FLAG,NEXT_STATION,CUSTOMER_NO,BOM_NO,BILL_NO,TRACK_NO,PO_NO,KEY_PART_NO,CARTON_NO,WARRANTY_DATE,REWORK_NO,REPAIR_CNT,EMP_NO,PO_LINE,PALLET_FULL_FLAG,PMCC,GROUP_NAME_CQC,MO_NUMBER_OLD,ERP_MO,ATE_STATION_NO,MSN,IMEI,JOB,MCARTON_NO,SO_NUMBER,SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,SHIPPING_SN2 from sfism4." + valueInput.table + " where ( " + valueInput.field + " in (" + inputvalue + ") or serial_number in (" + inputvalue + ") or shipping_sn  in (" + inputvalue + ") or shipping_sn2 in (" + inputvalue + ") or po_no in (" + inputvalue + ") ) " + _group_name_condition + " ";
                    }
                    else if (valueInput.table == "R108")
                    {
                        _queryString = "select * from sfism4." + valueInput.table + " where " + valueInput.field + " in (" + inputvalue + ") ";
                    }
                    else if (valueInput.table == "R105")
                    {
                        _queryString = "select * from sfism4." + valueInput.table + " where " + valueInput.field + " in (" + inputvalue + ")";
                    }
                    else
                    {
                        _queryString = "select * from sfism4." + valueInput.table + " where " + valueInput.field + " in (" + inputvalue + ") ";
                    }

                }
            }

            dt = DBConnect.GetData(_queryString, valueInput.database);
            dt2 = null;
            if (valueInput.table == "R117")
            {
                goto lastResult;
            }
            if (dt.Rows.Count > 0)
            {
                if (valueInput.table == "R107" || valueInput.table == "R108" || valueInput.table == "Z107" || valueInput.table == "R117_")
                {
                    string _queryString1 = "select * from sfism4.R108 where serial_number = '" + dt.Rows[0]["SERIAL_NUMBER"] + "' order by work_time asc";
                    dt2 = DBConnect.GetData(_queryString1, valueInput.database);
                }
                else if (valueInput.table == "R117")
                {
                    string _queryString1 = "";
                    if (valueInput.date_from != null)
                    {
                        _queryString1 = "select * from sfism4.R117 where " + valueInput.field + " in (" + inputvalue + ") " + _group_name_condition + " AND in_station_time between TO_DATE('" + valueInput.date_from + "','YYYY/MM/DD HH24:MI') and TO_DATE('" + valueInput.date_to + "','YYYY/MM/DD HH24:MI') order by in_station_time asc ";
                    }
                    else
                    {
                        _queryString1 = "select * from sfism4.R117 where " + valueInput.field + " in (" + inputvalue + ") " + _group_name_condition + " order by in_station_time asc  ";
                    }
                    dt2 = DBConnect.GetData(_queryString1, valueInput.database);
                }

            }
            else
            {
                if (valueInput.table == "R117")
                {
                    string _queryString1 = "";
                    if (valueInput.date_from != null)
                    {
                        _queryString1 = "select * from sfism4.R117 where " + valueInput.field + " in (" + inputvalue + ") " + _group_name_condition + " AND in_station_time between TO_DATE('" + valueInput.date_from + "','YYYY/MM/DD HH24:MI') and TO_DATE('" + valueInput.date_to + "','YYYY/MM/DD HH24:MI') order by in_station_time asc ";
                    }
                    else
                    {
                        _queryString1 = "select * from sfism4.R117 where " + valueInput.field + " in (" + inputvalue + ") " + _group_name_condition + " order by in_station_time asc ";
                    }
                    dt2 = DBConnect.GetData(_queryString1, valueInput.database);
                    if (dt2.Rows.Count != 0)
                    {
                        _queryString = "select * from sfism4.R107 where serial_number = '" + dt2.Rows[0]["SERIAL_NUMBER"].ToString() + "' ";
                        dt = DBConnect.GetData(_queryString, valueInput.database);
                    }
                }
            }
        lastResult:
            return Request.CreateResponse(HttpStatusCode.OK, new { data = dt, data1 = dt2, query = _queryString, result = "ok" });
        }

        [System.Web.Http.Route("DeleteLockInfo")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteLockInfo(ETEConfigInfo eteConfig)
        {
            string database_name = eteConfig.database_name;
            string model_name = eteConfig.model_name;
            string group_name = eteConfig.group_name;
            string condition = eteConfig.condition;
            string emp_pass = eteConfig.emp_pass;
            string ip_address = UserIP();

            string queryString = "select * from SFIS1.C_PRIVILEGE where emp in (select distinct emp_no from sfis1.C_EMP_DESC_T where emp_pass= '" + emp_pass + "' and rownum =1) and fun = 'ETE CONFIG_DELETE' and prg_name = 'CONFIG'";
            DataTable dtCheck1 = DBConnect.GetData(queryString, database_name);
            if (dtCheck1.Rows.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });


            string formatDelete = "Delete sfis1.c_ete_config_t where type='{0}' and model_name='{1}' and group_name= '{2}'";
            string queryDelete = string.Format(formatDelete, condition, model_name, group_name);

            DBConnect.ExecuteNoneQuery(queryDelete, database_name);

            string queryInserLog = "Insert into SFISM4.R_SYSTEM_LOG_T(EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC,TIME) Values (''||(SELECT EMP_NO FROM SFIS1.C_EMP_DESC_T where EMP_PASS = '" + emp_pass + "' and rownum =1)||'','CONFIG','DELETE','ETE Config - MODEL NAME : " + model_name + "; GROUP NAME : " + group_name + "; " + condition + ";MAC ADDRESS﹕null;IP:" + ip_address + "',sysdate)";

            DBConnect.ExecuteNoneQuery(queryInserLog, database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
        }


        [System.Web.Http.Route("SendMessage")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> SendMessage(Message message)
        {
            string api_link = message.ApiLink;    /// "http://10.224.81.136:1234/apizalo/SendMessageKey";
            string groupKey = message.GroupKey;          //// "6iECeNKmk+j3eRti9wWIdql+0gNm2vH1sanXqr5GzR7k9EMs+63t/hG2fJ8Z0XykgvhtjfmYtu3LSn9S6LzLI/D6pQodhNNWARyTemGF8KOg+4y4hDFs2XmGIIkCObE+";
            var httpWebRequestZalo = (HttpWebRequest)WebRequest.Create(api_link);
            httpWebRequestZalo.ContentType = "application/json";
            httpWebRequestZalo.Method = "POST";
            string ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
            var obj = new
            {
                KEY = @"" + groupKey.ToString(),
                CONTENTS = message.HostIP + ": " + message.MessageContent,  /// "10.224.44.58: DFASDFAS",         /// GetLocalIPAddress() + ": " + messageContent.Text,
                TYPE = "TEXT"
            };

            string parsedContent = JsonConvert.SerializeObject(obj);
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(parsedContent);

            Stream newStream = httpWebRequestZalo.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();

            var response = httpWebRequestZalo.GetResponse();
            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://10.224.81.136:8888/api/sendData");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string contentSend = message.dbKey + message.HostIP + ": " + message.MessageContent;  /// "CPEII10.224.44.58: DFASDFAS" ;        // "[" + Login.dbKey + "]-" + Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString() + ": \n" + messageContent.Text;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"ip_address\":\"" + message.HostIP + "\"," +
                                "\"building\" : \"" + message.dbKey + "\"," +
                                "\"team\" : \"SFIS\"," +
                                "\"emp_user\" : \"" + message.dbKey + "\"," +
                                "\"description\" : \"" + message.MessageContent + "\"}";
                streamWriter.Write(json);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                dynamic objer = JsonConvert.DeserializeObject(result);
                if (result.Contains("Id_ats"))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "ok" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "NG" });
                }
            }

        }

        [System.Web.Http.Route("ConfirmUnLockConfigLockInfo")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ConfirmUnLockConfigLockInfo(ETEConfigInfo eteConfig)
        {
            string database_name = eteConfig.database_name;
            string model_name = eteConfig.model_name;
            string group_name = eteConfig.group_name;
            string condition = eteConfig.condition;
            string condition_value = eteConfig.condition_value;
            string emp_pass = eteConfig.emp_pass;
            string reason = eteConfig.reason;
            string solution = eteConfig.solution;
            string ip_address = UserIP();

            string queryString = "select * from SFIS1.C_PRIVILEGE where emp in (select distinct emp_no from sfis1.C_EMP_DESC_T where emp_pass= '" + emp_pass + "' and rownum =1) and fun = 'ETE CONFIG_UNLOCK_CONFIRM' and prg_name = 'CONFIG'";
            DataTable dtCheck1 = DBConnect.GetData(queryString, database_name);
            if (dtCheck1.Rows.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });


            string formatUpdate = "update sfis1.c_ete_config_t set status='',DATA='',create_time=sysdate where model_name= '{1}'  and type='{2}' and group_name = '{3}'";
            string queryUpdate = string.Format(formatUpdate, condition_value, model_name, condition, group_name);
            DBConnect.ExecuteNoneQuery(queryUpdate, database_name);

            string queryInserLog = "Insert into SFISM4.R_SYSTEM_LOG_T(EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC,TIME) Values (''||(SELECT EMP_NO FROM SFIS1.C_EMP_DESC_T where EMP_PASS = '" + emp_pass + "' and rownum =1)||'','CONFIG','UNLOCK','ETE Config - MODEL NAME : " + model_name + "; GROUP NAME : " + group_name + ";MAC ADDRESS﹕null;IP:" + UserIP() + "',sysdate)";

            DBConnect.ExecuteNoneQuery(queryInserLog, database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
        }

        [System.Web.Http.Route("UnLockConfigLockInfo")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> UnLockConfigLockInfo(ETEConfigInfo eteConfig)
        {
            string database_name = eteConfig.database_name;
            string model_name = eteConfig.model_name;
            string group_name = eteConfig.group_name;
            string condition = eteConfig.condition;
            string condition_value = eteConfig.condition_value;
            string emp_pass = eteConfig.emp_pass;
            string reason = eteConfig.reason;
            string solution = eteConfig.solution;
            string ip_address = UserIP();

            string queryString = "select * from SFIS1.C_PRIVILEGE where emp in (select distinct emp_no from sfis1.C_EMP_DESC_T where emp_pass= '" + emp_pass + "' and rownum =1) and fun = 'ETE CONFIG_UNLOCK' and prg_name = 'CONFIG'";
            DataTable dtCheck1 = DBConnect.GetData(queryString, database_name);
            if (dtCheck1.Rows.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });


            string formatUpdate = "update sfis1.c_ete_config_t set status='' , create_time=sysdate where model_name= '{1}'  and type='{2}' and group_name = '{3}'";
            string queryUpdate = string.Format(formatUpdate, condition_value, model_name, condition, group_name);
            DBConnect.ExecuteNoneQuery(queryUpdate, database_name);

            string queryInserLog = "Insert into SFISM4.R_SYSTEM_LOG_T(EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC,TIME) Values (''||(SELECT EMP_NO FROM SFIS1.C_EMP_DESC_T where EMP_PASS = '" + emp_pass + "' and rownum =1)||'','CONFIG','UNLOCK','ETE Config - MODEL NAME : " + model_name + "; GROUP NAME : " + group_name + "; " + solution + ";MAC ADDRESS﹕null;IP:" + ip_address + ";" + reason + ";',sysdate)";

            DBConnect.ExecuteNoneQuery(queryInserLog, database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
        }

        [System.Web.Http.Route("CheckPrivilege")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> CheckPrivilege(PrivilegeInfo privilege)
        {
            string database_name = privilege.database_name;
            string emp_no = privilege.emp_no;
            string fun = privilege.fun;
            string prg_name = privilege.prg_name;

            string queryString = "select * from SFIS1.C_PRIVILEGE where emp = '" + emp_no + "' and fun = '" + fun + "' and prg_name = '" + prg_name + "'";
            DataTable dtCheck = DBConnect.GetData(queryString, database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
        }

        [System.Web.Http.Route("LockConfigLockInfo")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> LockConfigLockInfo(ETEConfigInfo eteConfig)
        {
            string database_name = eteConfig.database_name;
            string model_name = eteConfig.model_name;
            string group_name = eteConfig.group_name;
            string condition = eteConfig.condition;
            string condition_value = eteConfig.condition_value;
            string emp_pass = eteConfig.emp_pass;
            string reason = eteConfig.reason;
            string ip_address = UserIP();
            string queryString = "select * from SFIS1.C_PRIVILEGE where emp in (select distinct emp_no from sfis1.C_EMP_DESC_T where emp_pass= '" + emp_pass + "' and rownum =1) and fun = 'ETE CONFIG_LOCK' and prg_name = 'CONFIG'";
            DataTable dtCheck = DBConnect.GetData(queryString, database_name);
            if (dtCheck.Rows.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });

            string formatUpdate = "update sfis1.c_ete_config_t set status='LOCK',create_time=sysdate where model_name= '{1}'  and type='{2}' and group_name = '{3}'";
            string queryUpdate = string.Format(formatUpdate, condition_value, model_name, condition, group_name);
            DBConnect.ExecuteNoneQuery(queryUpdate, database_name);

            string queryInserLog = "Insert into SFISM4.R_SYSTEM_LOG_T(EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC,TIME) Values (''||(SELECT EMP_NO FROM SFIS1.C_EMP_DESC_T where EMP_PASS = '" + emp_pass + "' and rownum =1)||'','CONFIG','LOCK','ETE Config - MODEL NAME : " + model_name + "; GROUP NAME : " + group_name + "; " + reason + ";MAC ADDRESS﹕null;IP:" + ip_address + "',sysdate)";

            DBConnect.ExecuteNoneQuery(queryInserLog, database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
        }



        [System.Web.Http.Route("UpdateLockInfo")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> UpdateLockInfo(ETEConfigInfo eteConfig)
        {
            string database_name = eteConfig.database_name;
            string model_name = eteConfig.model_name;
            string group_name = eteConfig.group_name;
            string condition = eteConfig.condition;
            string condition_value = eteConfig.condition_value;
            string emp_pass = eteConfig.emp_pass;
            string ip_address = UserIP();

            string queryString = "select * from SFIS1.C_PRIVILEGE where emp in (select distinct emp_no from sfis1.C_EMP_DESC_T where emp_pass= '" + emp_pass + "' and rownum =1) and fun = 'ETE CONFIG_EDIT' and prg_name = 'CONFIG'";
            DataTable dtCheck = DBConnect.GetData(queryString, database_name);
            if (dtCheck.Rows.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });


            string formatUpdate = "update sfis1.c_ete_config_t set condition='{0}',create_time=sysdate where model_name= '{1}'  and type='{2}' and group_name = '{3}'";
            string queryUpdate = string.Format(formatUpdate, condition_value, model_name, condition, group_name);
            DBConnect.ExecuteNoneQuery(queryUpdate, database_name);

            string queryInserLog = "Insert into SFISM4.R_SYSTEM_LOG_T(EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC,TIME) Values (''||(SELECT EMP_NO FROM SFIS1.C_EMP_DESC_T where EMP_PASS = '" + emp_pass + "' and rownum =1)||'','CONFIG','UPDATE','ETE Config - MODEL NAME : " + model_name + "; GROUP NAME : " + group_name + "; " + condition + ":" + condition_value + ";MAC ADDRESS﹕null;IP:" + ip_address + "',sysdate)";

            DBConnect.ExecuteNoneQuery(queryInserLog, database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
        }


        [System.Web.Http.Route("InsertLockInfo")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertLockInfo(ETEConfigInfo eteConfig)
        {
            string database_name = eteConfig.database_name;
            string model_name = eteConfig.model_name;
            string group_name = eteConfig.group_name;
            string condition = eteConfig.condition;
            string condition_value = eteConfig.condition_value;
            string emp_pass = eteConfig.emp_pass;
            string ip_address = UserIP();

            string queryString = "select * from SFIS1.C_PRIVILEGE where emp in (select distinct emp_no from sfis1.C_EMP_DESC_T where emp_pass= '" + emp_pass + "' and rownum =1) and fun = 'ETE CONFIG_ADD' and prg_name = 'CONFIG'";
            DataTable dtCheck1 = DBConnect.GetData(queryString, database_name);
            if (dtCheck1.Rows.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });


            string queryCheck = "select * from sfis1.c_ete_config_t where type='" + condition + "' and model_name= '" + model_name + "' and group_name= '" + group_name + "'";
            DataTable dtCheck = new DataTable();
            dtCheck = DBConnect.GetData(queryCheck, database_name);
            if (dtCheck.Rows.Count != 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "exist" });
            }
            string queryInsertFormat = "Insert into sfis1.c_ete_config_t(model_name,group_name,type,condition,create_time) VALUES('{0}','{1}','{2}','{3}',sysdate)";

            string queryInsert = string.Format(queryInsertFormat, model_name, group_name, condition, condition_value);

            DBConnect.ExecuteNoneQuery(queryInsert, database_name);

            string queryInserLog = "Insert into SFISM4.R_SYSTEM_LOG_T(EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC,TIME) Values (''||(SELECT EMP_NO FROM SFIS1.C_EMP_DESC_T where EMP_PASS = '" + emp_pass + "' and rownum =1)||'','CONFIG','INSERT','ETE Config - MODEL NAME : " + model_name + "; GROUP NAME : " + group_name + "; " + condition + ":" + condition_value + ";MAC ADDRESS﹕null;IP:" + ip_address + "',sysdate)";

            DBConnect.ExecuteNoneQuery(queryInserLog, database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
        }

        [System.Web.Http.Route("GetLockInfo")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetLockInfo(LockInformation lockInformation)
        {
            string database_name = lockInformation.database_name;
            string model_name = lockInformation.model_name;
            string group_name = lockInformation.group_name;
            string action_type = lockInformation.action_type;
            DataTable dt = new DataTable();

            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(model_name))
            {
                sb.AppendFormat(" and model_name = '{0}'", model_name);
            }
            if (!string.IsNullOrEmpty(group_name))
            {
                sb.AppendFormat(" and group_name = '{0}'", group_name);
            }
            if (action_type == "NORMAL")
            {
                sb.Append(" and status is null");
            }
            else if (action_type == "LOCK")
            {
                sb.Append(" and status = 'LOCK' and DATA IS NULL ");
            }
            else if (action_type == "CONFIRM_UNLOCK")
            {
                sb.Append(" and status = 'LOCK' and DATA = 'CONFIRM'");
            }
            string subQuery = "select * from SFIS1.C_ETE_CONFIG_T where (TO_CHAR(ERROR_CODE) IS NULL OR TO_CHAR(ERROR_CODE) NOT IN ('AQL')) and 1 = 1 ";
            sb.Insert(0, subQuery);
            string queryString = sb.ToString();
            dt = DBConnect.GetData(queryString, database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { data = dt });

        }


        [System.Web.Http.Route("GetLogHistory")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetLogHistory(HistoryLog historyLog)
        {
            string database_name = historyLog.database_name;
            string model_name = historyLog.model_name;
            string group_name = historyLog.group_name;
            string action_type = historyLog.action_type;
            string start_time = historyLog.start_time;
            string end_time = historyLog.end_time;
            DataTable dt = new DataTable();

            var sb = new StringBuilder();
            sb.AppendFormat("and action_desc like 'ETE Config%{0}%{1}%'", model_name, group_name);

            if (!string.IsNullOrEmpty(action_type) && action_type != "ALL")
            {
                sb.AppendFormat(" AND action_type ='{0}'", action_type);
            }
            if (!string.IsNullOrEmpty(start_time) && !string.IsNullOrEmpty(end_time))
            {
                sb.AppendFormat(" AND (TIME >=TO_DATE('{0}','YYYY/MM/DD HH24:MI') AND TIME <= TO_DATE('{1}','YYYY/MM/DD HH24:MI'))", start_time, end_time);
            }
            if (sb.Length > 0)
            {
                var selectCause = "";
                if (action_type == "CONFIRM_UNLOCK")
                {
                    selectCause = "SELECT A.EMP_NO, " +
       "       B.EMP_NAME, " +
       "       A.ACTION_TYPE, " +
       "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                          FROM DUAL " +
       "                                         WHERE LEVEL = 1 " +
       "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                                  IS NOT NULL), " +
       "                                  '[^:]+', " +
       "                                  1, " +
       "                                  LEVEL) " +
       "              FROM DUAL " +
       "             WHERE LEVEL = 2 " +
       "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                          FROM DUAL " +
       "                                         WHERE LEVEL = 1 " +
       "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                                  IS NOT NULL), " +
       "                                  '[^:]+', " +
       "                                  1, " +
       "                                  LEVEL) " +
       "                      IS NOT NULL) " +
       "          MODEL_NAME, " +
       "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                          FROM DUAL " +
       "                                         WHERE LEVEL = 2 " +
       "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                                  IS NOT NULL), " +
       "                                  '[^:]+', " +
       "                                  1, " +
       "                                  LEVEL) " +
       "              FROM DUAL " +
       "             WHERE LEVEL = 2 " +
       "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                          FROM DUAL " +
       "                                         WHERE LEVEL = 1 " +
       "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                                  IS NOT NULL), " +
       "                                  '[^:]+', " +
       "                                  1, " +
       "                                  LEVEL) " +
       "                      IS NOT NULL) " +
       "          GROUP_NAME, " +
       "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                          FROM DUAL " +
       "                                         WHERE LEVEL = 5 " +
       "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                                  IS NOT NULL), " +
       "                                  '[^:]+', " +
       "                                  1, " +
       "                                  LEVEL) " +
       "              FROM DUAL " +
       "             WHERE LEVEL = 2 " +
       "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                          FROM DUAL " +
       "                                         WHERE LEVEL = 1 " +
       "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
       "                                                              '[^;]+', " +
       "                                                              1, " +
       "                                                              LEVEL) " +
       "                                                  IS NOT NULL), " +
       "                                  '[^:]+', " +
       "                                  1, " +
       "                                  LEVEL) " +
       "                      IS NOT NULL) " +
       "          IP, " +
       "       A.TIME " +
       "  FROM sfism4.R_SYSTEM_LOG_T A, SFIS1.C_EMP_DESC_T B " +
       " WHERE     1 = 1 " +
       "       AND A.EMP_NO = B.EMP_NO ";
                }
                else if (action_type == "UNLOCK")
                {
                    selectCause = "  SELECT A.EMP_NO, " +
         "         B.EMP_NAME, " +
         "         A.ACTION_TYPE, " +
         "         (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                            FROM DUAL " +
         "                                           WHERE LEVEL = 1 " +
         "                                      CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                                    IS NOT NULL), " +
         "                                    '[^:]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                FROM DUAL " +
         "               WHERE LEVEL = 2 " +
         "          CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                            FROM DUAL " +
         "                                           WHERE LEVEL = 1 " +
         "                                      CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                                    IS NOT NULL), " +
         "                                    '[^:]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                        IS NOT NULL) " +
         "            MODEL_NAME, " +
         "         (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                            FROM DUAL " +
         "                                           WHERE LEVEL = 2 " +
         "                                      CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                                    IS NOT NULL), " +
         "                                    '[^:]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                FROM DUAL " +
         "               WHERE LEVEL = 2 " +
         "          CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                            FROM DUAL " +
         "                                           WHERE LEVEL = 1 " +
         "                                      CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                                    IS NOT NULL), " +
         "                                    '[^:]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                        IS NOT NULL) " +
         "            GROUP_NAME, " +
         "         (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                    '[^;]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                FROM DUAL " +
         "               WHERE LEVEL = 3 " +
         "          CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                    '[^;]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                        IS NOT NULL) " +
         "            SOLUTION, " +
         "         (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                    '[^;]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                FROM DUAL " +
         "               WHERE LEVEL = 6 " +
         "          CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                    '[^;]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                        IS NOT NULL) " +
         "            REASON, " +
         "         (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                            FROM DUAL " +
         "                                           WHERE LEVEL = 5 " +
         "                                      CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                                    IS NOT NULL), " +
         "                                    '[^:]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                FROM DUAL " +
         "               WHERE LEVEL = 2 " +
         "          CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                            FROM DUAL " +
         "                                           WHERE LEVEL = 1 " +
         "                                      CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
         "                                                                '[^;]+', " +
         "                                                                1, " +
         "                                                                LEVEL) " +
         "                                                    IS NOT NULL), " +
         "                                    '[^:]+', " +
         "                                    1, " +
         "                                    LEVEL) " +
         "                        IS NOT NULL) " +
         "            IP, " +
         "         A.TIME " +
         "    FROM sfism4.R_SYSTEM_LOG_T A, SFIS1.C_EMP_DESC_T B " +
         "   WHERE     1 = 1 " +
         "         AND A.EMP_NO = B.EMP_NO ";
                }
                else if (action_type == "LOCK")
                {
                    selectCause = "SELECT A.EMP_NO, " +
        "       B.EMP_NAME, " +
        "       A.ACTION_TYPE, " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          MODEL_NAME, " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 2 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          GROUP_NAME, " +
        "       (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                  '[^;]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 3 " +
        "        CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                  '[^;]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          REASON, " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 5 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          IP, " +
        "       A.TIME " +
        "  FROM sfism4.R_SYSTEM_LOG_T A, SFIS1.C_EMP_DESC_T B " +
        " WHERE     1 = 1 " +
        "       AND A.EMP_NO = B.EMP_NO ";

                }
                else if (action_type == "DELETE")
                {
                    selectCause = "SELECT A.EMP_NO, " +
        "       B.EMP_NAME, " +
        "       A.ACTION_TYPE, " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          MODEL_NAME, " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 2 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          GROUP_NAME, " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 5 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          IP, " +
        "       A.TIME " +
        "  FROM sfism4.R_SYSTEM_LOG_T A, SFIS1.C_EMP_DESC_T B " +
        " WHERE     1 = 1 " +
        "       AND A.EMP_NO = B.EMP_NO ";

                }
                else
                {
                    //INSERT , UPDATE
                    selectCause = "SELECT A.EMP_NO, " +
        "       B.EMP_NAME, " +
        "       A.ACTION_TYPE, " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          MODEL_NAME, " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 2 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 2 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          GROUP_NAME,           " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 3 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 1 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 2 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          CONDITION, " +
        "          (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 3 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 2 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          VALUE " +
        "          ,        " +
        "       (    SELECT REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 5 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "              FROM DUAL " +
        "             WHERE LEVEL = 2 " +
        "        CONNECT BY REGEXP_SUBSTR ( (    SELECT REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                          FROM DUAL " +
        "                                         WHERE LEVEL = 1 " +
        "                                    CONNECT BY REGEXP_SUBSTR (A.ACTION_DESC, " +
        "                                                              '[^;]+', " +
        "                                                              1, " +
        "                                                              LEVEL) " +
        "                                                  IS NOT NULL), " +
        "                                  '[^:]+', " +
        "                                  1, " +
        "                                  LEVEL) " +
        "                      IS NOT NULL) " +
        "          IP, " +
        "       A.TIME " +
        "  FROM sfism4.R_SYSTEM_LOG_T A, SFIS1.C_EMP_DESC_T B " +
        " WHERE     1 = 1 " +
        "       AND A.EMP_NO = B.EMP_NO ";
                }


                sb.Insert(0, selectCause);
                var queryString = sb.ToString() + " order by A.TIME DESC";
                dt = DBConnect.GetData(queryString, database_name);


            }
            return Request.CreateResponse(HttpStatusCode.OK, new { data = dt });
        }

        [System.Web.Http.Route("GetStation")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetStation(ValueInputQuery6 valueInput)
        {
            string _database = valueInput.database;
            string table = valueInput.table;
            string query_string = "";
            string field = valueInput.field;
            List<ValueInputQuery6.ListInput> listInput = valueInput.list_input;
            string inputvalue = "";
            for (int i = 0; i < listInput.Count; i++)
            {
                if (i == 0)
                {
                    inputvalue += "'" + listInput[i].input + "'";
                }
                else
                {
                    inputvalue += ",'" + listInput[i].input + "'";
                }
            }
            query_string = " SELECT DISTINCT GROUP_NEXT " +
       "    FROM SFIS1.C_ROUTE_CONTROL_T " +
       "   WHERE ROUTE_CODE IN (SELECT DISTINCT SPECIAL_ROUTE " +
       "                          FROM SFISM4." + table + " " +
       "                         WHERE " + field + " IN (" + inputvalue + ")) " +
       "ORDER BY GROUP_NEXT ASC ";

            DataTable dt = DBConnect.GetData(query_string, _database);
            return Request.CreateResponse(HttpStatusCode.OK, new { data = dt });
        }
        //login

        [System.Web.Http.Route("LoginQuery")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> LoginQuery(JObject obj)
        {
            var JsonObj = JsonConvert.SerializeObject(obj, Formatting.Indented);
            dynamic data = JObject.Parse(JsonObj);
            //List<Employee> listEmp = new List<Employee>();
            //string Token = await Task.Run(() => data.Token);
            //var ApplicationID = await Task.Run(() => data.ApplicationID);
            //var _listEMPNo = await Task.Run(() => data.EmpList);
            //int i = 0;
            //string queryString = "";
            //var response = new HttpResponseMessage();
            //var _obj = JwtToken.GetPayLoad(Token);
            var _database = "";
            var _unique_name = "";
            var _password = "";
            try
            {
                _database = data["database"].ToString();
                _unique_name = data["username"].ToString();
                _password = data["password"].ToString();
            }
            catch
            {
                return Request.CreateResponse(new { message = "Authorization has been denied for this request." });
            }
            string _query_string = "SELECT * FROM SFIS1.C_EMP_DESC_T WHERE EMP_NO = '" + _unique_name + "' AND EMP_PASS = '" + _password + "'";

            DataTable dt = DBConnect.GetData(_query_string, _database);
            if (dt.Rows.Count > 0)
            {
                return Request.CreateResponse(new { message = "ok", emp_name = dt.Rows[0]["EMP_NAME"] });
            }
            else
            {
                return Request.CreateResponse(new { message = "Authorization has been denied for this request." });
            }

        }
        [System.Web.Http.Route("GetAllDistinct")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetAllDistinct(JObject obj)
        {
            var JsonObj = JsonConvert.SerializeObject(obj, Formatting.Indented);
            dynamic data = JObject.Parse(JsonObj);
            var _database = "";
            var _unique_name = "";
            var _password = "";
            var _option_distinct = "";
            var _option_order = "";
            var _table = "";
            try
            {
                _database = data["database"].ToString();
                _option_distinct = data["option_distinct"];
                _table = data["table"].ToString();
                _option_order = data["orderby"].ToString();
            }
            catch
            {
                return Request.CreateResponse(new { message = "Authorization has been denied for this request." });
            }
            string _query_string = "SELECT DISTINCT " + _option_distinct + " FROM " + _table + " ORDER BY " + _option_order + " ASC";

            DataTable dt = DBConnect.GetData(_query_string, _database);
            if (dt.Rows.Count > 0)
            {
                return Request.CreateResponse(new { message = "ok", data = dt, query = _query_string });
            }
            else
            {
                return Request.CreateResponse(new { message = "Authorization has been denied for this request." });
            }

        }
        [System.Web.Http.Route("GetAllBake")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetAllBake(ValueBake value)
        {
            try
            {
                string _sql = "";
                if (value.model_serial == "SONY")
                {
                    _sql = string.Format(@"SELECT DISTINCT sub,LOCATION,MODEL_NAME,MAX(TIME) TIME,SUM(qty)  qty,SUM(tray) tray,CONTROL_TIME,time_out,color
FROM   (SELECT SUBLOCATION SUB,b.*,
               CASE
               WHEN CONTROL_TIME - ( ( SYSDATE - TIME ) * 24 ) > 0 THEN Round(CONTROL_TIME - ( ( SYSDATE - TIME ) * 24 ), 1)
               WHEN CONTROL_TIME - ( ( SYSDATE - TIME ) * 24 ) <= 0 THEN 0
               END         AS time_out,
               CASE
               WHEN( SYSDATE - TIME ) * 24 > CONTROL_TIME THEN 'GREEN'
               WHEN( SYSDATE - TIME ) * 24 BETWEEN CONTROL_TIME * 0.9 AND CONTROL_TIME THEN 'YELLOW'
               WHEN( SYSDATE - TIME ) * 24 < CONTROL_TIME * 0.9 THEN 'RED'
               END         AS COLOR
               FROM  (SELECT SUBLOCATION1
                      ||SUBLOCATION AS SUBLOCATION
               FROM  (SELECT '01' sublocation
                      FROM   dual
                      UNION ALL
                      SELECT '02' sublocation
                      FROM   dual
                      UNION ALL
                      SELECT '03' sublocation
                      FROM   dual
                      UNION ALL
                      SELECT '04' sublocation
                      FROM   dual
                      UNION ALL
                      SELECT '05' sublocation
                      FROM   dual
                      UNION ALL
                      SELECT '06' sublocation
                      FROM   dual
                      UNION ALL
                      SELECT '07' sublocation
                      FROM   dual
                      UNION ALL
                      SELECT '08' sublocation
                      FROM   dual
                      UNION ALL
                      SELECT '09' sublocation
                      FROM   dual) a,
                     (SELECT 'A' sublocation1
                      FROM   dual
                      UNION ALL
                      SELECT 'B' sublocation1
                      FROM   dual
                      UNION ALL
                      SELECT 'C' sublocation1
                      FROM   dual
                      UNION ALL
                      SELECT 'D' sublocation1
                      FROM   dual
                      UNION ALL
                      SELECT 'E' sublocation1
                      FROM   dual
                      UNION ALL
                      SELECT 'F' sublocation1
                      FROM   dual
                      UNION ALL
                      SELECT 'G' sublocation1
                      FROM   dual
                      UNION ALL
                      SELECT 'H' sublocation1
                      FROM   dual
                      UNION ALL
                      SELECT 'I' sublocation1
                      FROM   dual
                      UNION ALL
                      SELECT 'j' sublocation1
                      FROM   dual
                      UNION ALL
                      SELECT 'K' sublocation1
                      FROM   dual
                      UNION ALL
                      SELECT 'L' sublocation1
                      FROM   dual
                      UNION ALL
                      SELECT 'M' sublocation1
                      FROM   dual
                      UNION ALL
                      SELECT 'N' sublocation1
                      FROM   dual
                      UNION ALL
                      SELECT 'O' sublocation1
                      FROM   dual
                      UNION ALL
                      SELECT 'P' sublocation1
                      FROM   dual)) a
              left join (SELECT DISTINCT a.LOCATION,a.MODEL_NAME,MAX(a.IN_STATION_TIME) TIME,SUM(a.qty)  qty,COUNT(*) TRAY,CONTROL_TIME
              FROM   SFISM4.R_ROIN_T a,(SELECT * FROM   SFIS1.C_ROAST_TIME_CONTROL_T WHERE  DEFAULT_GROUP = 'ROAST_IN' AND end_group = 'ROAST_OUT') b
              WHERE  a.model_name = b.model_name(+) AND WORK_FLAG = 0 AND LOCATION LIKE 'BAKE%' GROUP  BY LOCATION, a.MODEL_NAME, TRAY_NO, CONTROL_TIME) b
              ON Substr(LOCATION, Length(LOCATION) - 2, Length(LOCATION)) = sublocation
              ORDER  BY sub)GROUP  BY sub,LOCATION,MODEL_NAME,qty,tray,CONTROL_TIME,time_out,color ORDER  BY sub  ");
                }
                else
                {

                    _sql = string.Format(@"select SUBLOCATION SUB,b.*,
               CASE
               WHEN CONTROL_TIME - ( ( SYSDATE - TIME ) * 24 ) > 0 THEN Round(CONTROL_TIME - ( ( SYSDATE - TIME ) * 24 ), 1)
               WHEN CONTROL_TIME - ( ( SYSDATE - TIME ) * 24 ) <= 0 THEN 0
               END         AS time_out,
                case   
                when(sysdate - TIME) * 24 > CONTROL_TIME  then 'GREEN'  
                when(sysdate - TIME) * 24 between CONTROL_TIME*0.9 and CONTROL_TIME  then 'YELLOW'  
                when(sysdate - TIME) * 24 < CONTROL_TIME*0.9 then 'RED' 
               end as COLOR
               from(  select SUBLOCATION1||SUBLOCATION  as SUBLOCATION 
               From( select '01' sublocation from dual
               union all
               select '02' sublocation from dual
               union all
               select '03' sublocation from dual
               union all
               select '04' sublocation from dual
               union all
               select '05' sublocation from dual
               union all
               select '06' sublocation from dual
               union all
               select '07' sublocation from dual
               union all
               select '08' sublocation from dual
               union all
               select '09' sublocation from dual) a,
               ( select 'A' sublocation1 from dual
               union all
               select 'B' sublocation1 from dual
               union all
               select 'C' sublocation1 from dual
               union all
               select 'D' sublocation1 from dual
               union all
               select 'E' sublocation1 from dual
               union all
               select 'F' sublocation1 from dual
               union all
               select 'G' sublocation1 from dual
               union all
               select 'H' sublocation1 from dual
               union all
               select 'I' sublocation1 from dual
               union all
               select 'J' sublocation1 from dual
               union all
               select 'K' sublocation1 from dual
               union all
               select 'L' sublocation1 from dual
               union all
               select 'M' sublocation1 from dual 
               union all
               select 'N' sublocation1 from dual
               union all
               select 'O' sublocation1 from dual
               union all
               select 'P' sublocation1 from dual))  a
               left join
              ( select distinct a.LOCATION,a.MODEL_NAME, max(a.IN_STATION_TIME) time,CONTROL_TIME, sum(a.qty) qty, a.TRAY_NO TRAY 
              from SFIS1.R_ROIN_T a,(select * from SFIS1.C_ROAST_TIME_CONTROL_T where DEFAULT_GROUP='ROAST_IN' and end_group='ROAST_OUT') b where a.model_name=b.model_name(+) and
              WORK_FLAG = 0 and LOCATION like 'BAKE%' group by  LOCATION,a.MODEL_NAME,TRAY_NO ,CONTROL_TIME  ) b
              on substr(LOCATION,length(LOCATION) -2, length(LOCATION)) = sublocation ORDER BY sub");
                }

                DataTable dt = DBConnect.GetData(_sql, value.database);
                if (dt.Rows.Count > 0)
                {
                    return Request.CreateResponse(new { message = "ok", data = dt, query = _sql });
                }
                else
                {
                    return Request.CreateResponse(new { message = "ERROR" });
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(new { message = e.Message });
            }

        }

        [System.Web.Http.Route("GetTrayinBake")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetTrayinBake(ValueBake value)
        {
            try
            {
                string _query_string = "";
                if(value.model_serial == "SONY")
                {
                    _query_string = $"              SELECT b.LOCATION,a.TRAY_NO,a.SERIAL_NUMBER,a.MO_NUMBER,a.MODEL_NAME,a.VERSION_CODE,a.IN_STATION_TIME,a.WIP_GROUP " +
                        $"              FROM SFISM4.R107 a," +
                        $"              SFISM4.R_ROIN_T b" +
                        $"              WHERE a.TRAY_NO = b.TRAY_NO and b.LOCATION = '{value.BakeNo}' and b.WORK_FLAG = 0";
                }
                else
                {
                    _query_string = $"              SELECT b.LOCATION,a.TRAY_NO,a.SERIAL_NUMBER,a.MO_NUMBER,a.MODEL_NAME,a.VERSION_CODE,a.IN_STATION_TIME,a.WIP_GROUP " +
                        $"              FROM SFISM4.R107 a," +
                        $"              SFIS1.R_ROIN_T b" +
                        $"              WHERE a.TRAY_NO = b.TRAY_NO and b.LOCATION = '{value.BakeNo}' and b.WORK_FLAG = 0";
                }

                DataTable dt = DBConnect.GetData(_query_string, value.database);
                return Request.CreateResponse(new { message = "ok", data = dt, query = _query_string });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(new { message = "Authorization has been denied for this request." });
            }

        }

        [System.Web.Http.Route("GetValueQR")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetValueQR(ValueInputQuery6 value)
        {
            try
            {
                List<ValueInputQuery6.ListInput> listInput = value.list_input;
                string _query_string = "";
                string inputvalue = "";
                string field = "";
                for (int i = 0; i < listInput.Count; i++)
                {
                    if (i == 0)
                    {
                        inputvalue += listInput[i].input;
                    }
                    else
                    {
                        inputvalue += listInput[i].input;
                    }
                }
                if (value.field == "SERIAL_NUMBER")
                {
                    field = "SN";
                }

                if (value.field == "SHIPPING_SN")
                {
                    field = "SSN";
                }


                _query_string = "select SFIS1.F_GETQR('" + field + "','" + inputvalue + "') as QR from dual";

                DataTable dt = DBConnect.GetData(_query_string, value.database);
                return Request.CreateResponse(new { message = "ok", data = dt, query = _query_string });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(new { message = "Authorization has been denied for this request." });
            }

        }
    }
}
