using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using SN_API.Models;
using SN_API.Models.Config;
using SN_API.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ShippingDataController : ApiController
    {
        #region SHIPPING FILE BROADCOM
        // GET: ShippingData
        [System.Web.Http.Route("GetCustPN")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetCustPN(ValueShippingData value)
        {
            try
            {
                string _query_string = String.Format($"select distinct CUSTPN as MODEL_NAME from SFISM4.R_SHIPPING_DATA where GROUPCODE = '{value.type.Trim().ToUpper()}' ORDER BY CUSTPN");

                //dynamic dtCheck3 = ExecuteProdecuce("SFIS1.B2B_GET_FILE", value.database_name, value.dn, value.model_name, value.date_from, value.date_to, value.type, value.record_type);


                DataTable dt = DBConnect.GetData(_query_string, value.database_name);
                return Request.CreateResponse(new { message = "ok", data = dt, query = _query_string });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(new { message = "Authorization has been denied for this request." });
            }

        }
        [System.Web.Http.Route("GetModel_type")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetModel_type(ValueShippingData value)
        {
            try
            {
                string _query_string = "select distinct GROUPCODE from SFISM4.R_SHIPPING_DATA order by GROUPCODE asc";

                DataTable dt = DBConnect.GetData(_query_string, value.database_name);
                return Request.CreateResponse(new { message = "ok", data = dt, query = _query_string });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(new { message = "Authorization has been denied for this request." });
            }

        }
        [System.Web.Http.Route("GetShippingData")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetShippingData(ValueShippingData model)
        {
            string strGetData = "";

            if (model.byDN)
            {
                string groupcode = " select distinct GROUPCODE  from SFISM4.R_SHIPPING_DATA where DN = '" + model.dn.Trim() + "' ";
                DataTable dtCheck1 = DBConnect.GetData(groupcode, model.database_name);
                if (dtCheck1.Rows.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                }

                foreach (DataRow row in dtCheck1.Rows)
                {
                    model.type = row[0].ToString();
                }

                if (model.record_type == "WIPC")
                {
                    if (model.type == "NIC")
                    {
                        strGetData = String.Format("select RecordType,CMCode,to_char(CreateDate,'YYYYMMDDHH24MISS')CreateDate,null col4,DN ,CustPN,ChipPN,data1 col8,ChipQty,deptcode," +
                            " null as col11, null as col12, null as col13, null as col14, null as col15, null as col16, null as col17, null as col18, null as col19, null as col20," +
                            " LotNumber, 'V' || substr(ChipPN, 4) as col22, ChipQty as col23, null as col24, null as col25, null as col26, null as col27" +
                            " from SFISM4.R_SHIPPING_DATA where RECORDTYPE = 'WIPC' and DN = '" + model.dn.Trim() + "' ");
                    }
                    else if (model.type == "ECD")
                    {
                        strGetData = String.Format("select RecordType,CMCode,to_char(CreateDate,'YYYYMMDDHH24MISS')CreateDate,null col4,DN ,CustPN,ChipPN,data1 col8,ChipQty,deptcode," +
                            " null as col11, null as col12, null as col13, null as col14, null as col15, null as col16, null as col17, null as col18, null as col19, null as col20," +
                            " LotNumber, 'V' || substr(ChipPN, 4) as col22, ChipQty as col23, null as col24, null as col25, null as col26, null as col27" +
                            " from SFISM4.R_SHIPPING_DATA where RECORDTYPE = 'WIPC' and DN = '" + model.dn.Trim() + "' ");
                    }
                }
                else if (model.record_type == "SHIP")
                {
                    if (model.type == "NIC")
                    {
                        strGetData = String.Format("select * from (" +
                            " select RecordType, CMCode, to_char(CreateDate, 'YYYYMMDDHH24MISS') CreateDate, CONTENTTYPE, null as col5, CUSTPN, data1 as col7, DEPTCODE as col8, to_char(ShipDate, 'YYYYMMDDHH24MISS') as col9, to_char(ShipDate, 'YYYYMMDDHH24MISS') as col10," +
                            " DN, null as col12, null as col13, null as col14, null as col15, null as col16, null as col17, null as col18, null as col19, null as col20," +
                            " null as col21, ShippingQty as col22, ShippingQty as col23, null as col24, DN as col25, null as col26, LotNumber as col27, CartonQty as col28, COO as col29, LicenseID as col30," +
                            " null as col31, null as col32, null as col33, null as col34, CUSTVER as col35, null as col36, null as col37, STDIAG_VER as col38, null as col39, null as col40," +
                            " data3 as col41, null as col42, null as col43, null as col44, null as col45, null as col46, null as col47, null as col48, null as col49, null as col50," +
                            " WEEKCODE as col51, null as col52, LicenseID as col53, null as col54" +
                            " from SFISM4.R_SHIPPING_DATA where CONTENTTYPE = 'HEADER' and groupcode = 'NIC'" +
                            " union" +
                            " select RecordType, CMCode, to_char(CreateDate, 'YYYYMMDDHH24MISS') CreateDate, CONTENTTYPE, null as col5, CUSTPN, data1 as col7, DEPTCODE as col8, null as col9, null as col10," +
                            " DN, null as col12, null as col13, null as col14, null as col15, null as col16, null as col17, null as col18, null as col19, null as col20," +
                            " null as col21, null as col22, null as col23, null as col24, null as col25, null as col26, LotNumber as col27, CartonQty as col28, COO as col29, LicenseID as col30," +
                            " to_char(ShipDate, 'YYYYMMDDHH24MISS') as col31, null as col32, null as col33, null as col34, CUSTVER as col35, null as col36, null as col37, STDIAG_VER as col38, null as col39, null as col40," +
                            " null as col41, null as col42, null as col43, null as col44, null as col45, null as col46, null as col47, to_char(ShipDate, 'YYYYMMDDHH24MISS') as col48, null as col49, null as col50," +
                            " WEEKCODE as col51, null as col52, LicenseID as col53, data3 as col54" +
                            " from SFISM4.R_SHIPPING_DATA where CONTENTTYPE = 'LOT' and groupcode = 'NIC'" +
                            " ) where RecordType = 'SHIP' and dn = '" + model.dn.Trim() + "' order by DN,CONTENTTYPE ");
                    }
                    else if (model.type == "ECD")
                    {
                        strGetData = String.Format("select* from(" +
                            " select RecordType, CMCode, to_char(CreateDate,'YYYYMMDDHH24MISS') CreateDate,CONTENTTYPE,null as col5,CUSTPN,data1 as col7,DEPTCODE as col8,to_char(ShipDate, 'YYYYMMDDHH24MISS') as col9 ,to_char(ShipDate, 'YYYYMMDDHH24MISS') as col10," +
                            " DN,null as col12,null as col13,null as col14,null as col15,null as col16,null as col17,data4 as col18,null as col19,null as col20," +
                            " null as col21,ShippingQty as col22,ShippingQty as col23,null as col24,DN as col25,null as col26,LotNumber as col27,CartonQty as col28,COO as col29,LicenseID as col30," +
                            " null as col31,null as col32,null as col33,null as col34,CUSTVER as col35,null as col36,null as col37,STDIAG_VER as col38,null as col39,null as col40," +
                            " data3 as col41,null as col42,null as col43,null as col44,null as col45,null as col46,null as col47,null as col48,null as col49,null as col50," +
                            " WEEKCODE as col51,null as col52" +
                            " from SFISM4.R_SHIPPING_DATA where CONTENTTYPE = 'HEADER' and groupcode = 'ECD'" +
                            " union" +
                            " select RecordType,CMCode,to_char(CreateDate, 'YYYYMMDDHH24MISS') CreateDate,CONTENTTYPE,null as col5,CUSTPN,data1 as col7,DEPTCODE as col8,null as col9,null as col10," +
                            " DN,null as col12,null as col13,null as col14,null as col15,null as col16,null as col17,null as col18,null as col19,null as col20," +
                            " null as col21,null as col22,null as col23,null as col24,null as col25,null as col26,LotNumber as col27,TRAYQty as col28,COO as col29,trayID as col30," +
                            " to_char(ShipDate, 'YYYYMMDDHH24MISS') as col31,null as col32,null as col33,null as col34,CUSTVER as col35,null as col36,null as col37,null col38,null as col39,null as col40," +
                            " null as col41, null as col42, null as col43,null as col44,null as col45,null as col46,null as col47,to_char(ShipDate, 'YYYYMMDDHH24MISS') as col48 ,null as col49,null as col50," +
                            " WEEKCODE as col51,null as col52" +
                            " from SFISM4.R_SHIPPING_DATA where CONTENTTYPE = 'LOT' and groupcode = 'ECD'" +
                            " ) where RecordType = 'SHIP' AND dn = '" + model.dn.Trim() + "'" +
                            " order by DN,CONTENTTYPE ");
                    }
                    else if (model.type == "SUPERCAP")
                    {
                        strGetData = String.Format("select* from(" +
                            " select RecordType, CMCode, to_char(CreateDate,'YYYYMMDDHH24MISS') CreateDate,CONTENTTYPE,null as col5,CUSTPN,data1 as col7,DEPTCODE as col8,to_char(ShipDate, 'YYYYMMDDHH24MISS') as col9 ,to_char(ShipDate, 'YYYYMMDDHH24MISS') as col10," +
                            " DN,null as col12,null as col13,null as col14,null as col15,null as col16,null as col17,data4 as col18,null as col19,null as col20," +
                            " null as col21,ShippingQty as col22,ShippingQty as col23,null as col24,DN as col25,null as col26,LotNumber as col27,CartonQty as col28,COO as col29,CartonID as col30," +
                            " null as col31,null as col32,null as col33,null as col34,CUSTVER as col35,null as col36,null as col37,STDIAG_VER as col38,null as col39,null as col40," +
                            " data3 as col41,null as col42,null as col43,null as col44,null as col45,null as col46,null as col47,null as col48,null as col49,null as col50," +
                            " WEEKCODE as col51,null as col52" +
                            " from SFISM4.R_SHIPPING_DATA where CONTENTTYPE = 'HEADER' and groupcode = 'SUPERCAP'" +
                            " union" +
                            " select RecordType,CMCode,to_char(CreateDate, 'YYYYMMDDHH24MISS') CreateDate,CONTENTTYPE,null as col5,CUSTPN,data1 as col7,DEPTCODE as col8,null as col9,null as col10," +
                            " DN,null as col12,null as col13,null as col14,null as col15,null as col16,null as col17,null as col18,null as col19,null as col20," +
                            " null as col21,null as col22,null as col23,null as col24,null as col25,null as col26,LotNumber as col27,CartonQty as col28,COO as col29,CartonID as col30," +
                            " to_char(ShipDate, 'YYYYMMDDHH24MISS') as col31,null as col32,null as col33,null as col34,CUSTVER as col35,null as col36,null as col37,null col38,null as col39,null as col40," +
                            " null as col41, null as col42, null as col43,null as col44,null as col45,null as col46,null as col47,to_char(ShipDate, 'YYYYMMDDHH24MISS') as col48 ,null as col49,null as col50," +
                            " WEEKCODE as col51,null as col52" +
                            " from SFISM4.R_SHIPPING_DATA where CONTENTTYPE = 'LOT' and groupcode = 'SUPERCAP'" +
                            " ) where RecordType = 'SHIP' AND dn = '" + model.dn.Trim() + "'" +
                            " order by DN,CONTENTTYPE");

                    }
                }
                else if (model.record_type == "BDSN")
                {
                    if (model.type == "NIC")
                    {
                        strGetData = String.Format("select RecordType,CMCode,SN,SSN,LotNumber,LicenseID, null as col7,CustPN,null as col9,COO," +
                            " null as col11, null as col12, null as col13, null as col14, null as col15, null as col16, null as col17, null as col18, null as col19, null as col20" +
                            " from SFISM4.R_SHIPPING_DATA where RECORDTYPE = 'BDSN' AND groupcode = 'NIC' ");
                        if (model.dn.Trim() != "")
                        {
                            strGetData = strGetData + " and DN = '" + model.dn.Trim() + "' ";
                        }
                        strGetData = strGetData + " order by SN";
                    }
                    else if (model.type == "ECD")
                    {
                        strGetData = String.Format("select RecordType,CMCode,SN,SSN,LotNumber,TrayID, null as col7,CustPN,null as col9,COO," +
                            " null as col11, null as col12, null as col13, null as col14, null as col15, null as col16, null as col17, null as col18, null as col19, null as col20" +
                            " from SFISM4.R_SHIPPING_DATA where RECORDTYPE = 'BDSN' AND groupcode = 'ECD' ");
                        if (model.dn.Trim() != "")
                        {
                            strGetData = strGetData + " and DN = '" + model.dn.Trim() + "' ";
                        }
                        strGetData = strGetData + " order by SN";
                    }
                    else if (model.type == "SUPERCAP")
                    {
                        strGetData = String.Format("select RecordType,CMCode,SN,SSN,LotNumber,CartonID,to_char(CreateDate,'YYYYMMDDHH24MISS') CreateDate,CustPN,null as col9,COO," +
                            " null as col11, null as col12 from SFISM4.R_SHIPPING_DATA where RECORDTYPE = 'BDSN' AND groupcode = 'SUPERCAP'");
                        if (model.dn.Trim() != "")
                        {
                            strGetData = strGetData + " and DN = '" + model.dn.Trim() + "' ";
                        }
                        strGetData = strGetData + " order by SN";
                    }
                }
                else if (model.record_type == "SHPCFM")
                {
                    if (model.type == "NIC")
                    {
                        strGetData = String.Format("select RecordType,CMCode,Data1,Data2,'MAN'||LICENSEID col5,data3 as col6,data4 as col7,CustPN,LotNumber,1 as QTY," +
                            " LICENSEID, SN, SSN, null as col14, null as col15, null as col16, null as col17, null as col18, null as col19, null as col20," +
                            " null as col21, null as col22, null as col23, null as col24, null as col25, null as col26, null as col27, null as col28" +
                            " from SFISM4.R_SHIPPING_DATA where RECORDTYPE = 'SHPCFM' AND groupcode = 'NIC'");
                        if (model.dn.Trim() != "")
                        {
                            strGetData = strGetData + " and DN = '" + model.dn.Trim() + "' ";
                        }
                        strGetData = strGetData + " order by SN";
                    }
                    else if (model.type == "ECD")
                    {
                        strGetData = String.Format("select RecordType,CMCode,Data1,Data2,'MAN'||LICENSEID col5,data3 as col6,data4 as col7,CustPN,LotNumber,1 as QTY," +
                            " LICENSEID, SN, SSN, null as col14, null as col15, null as col16, null as col17, null as col18, null as col19, null as col20," +
                            " null as col21, null as col22, null as col23, null as col24, null as col25, null as col26, null as col27, null as col28" +
                            " from SFISM4.R_SHIPPING_DATA where DN in ('5142030073', '5145004953') and RECORDTYPE = 'SHPCFM' AND groupcode = 'ECD'");
                        if (model.dn.Trim() != "")
                        {
                            strGetData = strGetData + " and DN = '" + model.dn.Trim() + "' ";
                        }
                        strGetData = strGetData + " order by SN";
                    }
                    else if (model.type == "SUPERCAP")
                    {
                        //strGetData = String.Format("");
                    }
                }
            }
            else
            {
                if (model.record_type == "WIPC")
                {
                    if (model.type == "NIC")
                    {
                        strGetData = String.Format("select RecordType,CMCode,to_char(CreateDate,'YYYYMMDDHH24MISS')CreateDate,null col4,DN ,CustPN,ChipPN,data1 col8,ChipQty,deptcode," +
                            " null as col11, null as col12, null as col13, null as col14, null as col15, null as col16, null as col17, null as col18, null as col19, null as col20," +
                            " LotNumber, 'V' || substr(ChipPN, 4) as col22, ChipQty as col23, null as col24, null as col25, null as col26, null as col27" +
                            " from SFISM4.R_SHIPPING_DATA where RECORDTYPE = 'WIPC' AND groupcode = 'NIC' ");
                        if (model.model_name.Trim() != "ALL")
                        {
                            strGetData = strGetData + " and CustPN = '" + model.model_name.Trim() + "' ";
                        }
                        if (model.date_from != null)
                        {
                            strGetData = strGetData + " AND CREATEDATE between TO_DATE('" + model.date_from + "','YYYY/MM/DD HH24:MI') and TO_DATE('" + model.date_to + "','YYYY/MM/DD HH24:MI') ";
                        }
                    }
                    else if (model.type == "ECD")
                    {
                        strGetData = String.Format("select RecordType,CMCode,to_char(CreateDate,'YYYYMMDDHH24MISS')CreateDate,null col4,DN ,CustPN,ChipPN,data1 col8,ChipQty,deptcode," +
                            " null as col11, null as col12, null as col13, null as col14, null as col15, null as col16, null as col17, null as col18, null as col19, null as col20," +
                            " LotNumber, 'V' || substr(ChipPN, 4) as col22, ChipQty as col23, null as col24, null as col25, null as col26, null as col27" +
                            " from SFISM4.R_SHIPPING_DATA where RECORDTYPE = 'WIPC' AND groupcode = 'ECD'");
                        if (model.model_name.Trim() != "ALL")
                        {
                            strGetData = strGetData + " and CustPN = '" + model.model_name.Trim() + "' ";
                        }
                        if (model.date_from != null)
                        {
                            strGetData = strGetData + " AND CREATEDATE between TO_DATE('" + model.date_from + "','YYYY/MM/DD HH24:MI') and TO_DATE('" + model.date_to + "','YYYY/MM/DD HH24:MI') ";
                        }
                    }
                }
                else if (model.record_type == "SHIP")
                {
                    if (model.type == "NIC")
                    {
                        strGetData = String.Format("select * from (" +
                            " select RecordType, CMCode, to_char(CreateDate, 'YYYYMMDDHH24MISS') CreateDate, CONTENTTYPE, null as col5, CUSTPN, data1 as col7, DEPTCODE as col8, to_char(ShipDate, 'YYYYMMDDHH24MISS') as col9, to_char(ShipDate, 'YYYYMMDDHH24MISS') as col10," +
                            " DN, null as col12, null as col13, null as col14, null as col15, null as col16, null as col17, null as col18, null as col19, null as col20," +
                            " null as col21, ShippingQty as col22, ShippingQty as col23, null as col24, DN as col25, null as col26, LotNumber as col27, CartonQty as col28, COO as col29, LicenseID as col30," +
                            " null as col31, null as col32, null as col33, null as col34, CUSTVER as col35, null as col36, null as col37, STDIAG_VER as col38, null as col39, null as col40," +
                            " data3 as col41, null as col42, null as col43, null as col44, null as col45, null as col46, null as col47, null as col48, null as col49, null as col50," +
                            " WEEKCODE as col51, null as col52, LicenseID as col53, null as col54" +
                            " from SFISM4.R_SHIPPING_DATA where CONTENTTYPE = 'HEADER' and groupcode = 'NIC'");
                        if (model.date_from != null)
                        {
                            strGetData = strGetData + " AND CREATEDATE between TO_DATE('" + model.date_from + "','YYYY/MM/DD HH24:MI') and TO_DATE('" + model.date_to + "','YYYY/MM/DD HH24:MI') ";
                        }
                        strGetData = strGetData + " union" +
                            " select RecordType, CMCode, to_char(CreateDate, 'YYYYMMDDHH24MISS') CreateDate, CONTENTTYPE, null as col5, CUSTPN, data1 as col7, DEPTCODE as col8, null as col9, null as col10," +
                            " DN, null as col12, null as col13, null as col14, null as col15, null as col16, null as col17, null as col18, null as col19, null as col20," +
                            " null as col21, null as col22, null as col23, null as col24, null as col25, null as col26, LotNumber as col27, CartonQty as col28, COO as col29, LicenseID as col30," +
                            " to_char(ShipDate, 'YYYYMMDDHH24MISS') as col31, null as col32, null as col33, null as col34, CUSTVER as col35, null as col36, null as col37, STDIAG_VER as col38, null as col39, null as col40," +
                            " null as col41, null as col42, null as col43, null as col44, null as col45, null as col46, null as col47, to_char(ShipDate, 'YYYYMMDDHH24MISS') as col48, null as col49, null as col50," +
                            " WEEKCODE as col51, null as col52, LicenseID as col53, data3 as col54" +
                            " from SFISM4.R_SHIPPING_DATA where CONTENTTYPE = 'LOT' and groupcode = 'NIC'";
                        if (model.date_from != null)
                        {
                            strGetData = strGetData + " AND CREATEDATE between TO_DATE('" + model.date_from + "','YYYY/MM/DD HH24:MI') and TO_DATE('" + model.date_to + "','YYYY/MM/DD HH24:MI') ";
                        }
                        strGetData = strGetData + " ) where RecordType = 'SHIP' ";
                        if (model.model_name.Trim() != "ALL")
                        {
                            strGetData = strGetData + " and CustPN = '" + model.model_name.Trim() + "' ";
                        }
                        strGetData = strGetData + " order by DN,CONTENTTYPE";
                    }
                    else if (model.type == "ECD")
                    {
                        strGetData = String.Format("select* from(" +
                            " select RecordType, CMCode, to_char(CreateDate,'YYYYMMDDHH24MISS') CreateDate,CONTENTTYPE,null as col5,CUSTPN,data1 as col7,DEPTCODE as col8,to_char(ShipDate, 'YYYYMMDDHH24MISS') as col9 ,to_char(ShipDate, 'YYYYMMDDHH24MISS') as col10," +
                            " DN,null as col12,null as col13,null as col14,null as col15,null as col16,null as col17,data4 as col18,null as col19,null as col20," +
                            " null as col21,ShippingQty as col22,ShippingQty as col23,null as col24,DN as col25,null as col26,LotNumber as col27,CartonQty as col28,COO as col29,LicenseID as col30," +
                            " null as col31,null as col32,null as col33,null as col34,CUSTVER as col35,null as col36,null as col37,STDIAG_VER as col38,null as col39,null as col40," +
                            " data3 as col41,null as col42,null as col43,null as col44,null as col45,null as col46,null as col47,null as col48,null as col49,null as col50," +
                            " WEEKCODE as col51,null as col52" +
                            " from SFISM4.R_SHIPPING_DATA where CONTENTTYPE = 'HEADER' and groupcode = 'ECD'");
                        if (model.date_from != null)
                        {
                            strGetData = strGetData + " AND CREATEDATE between TO_DATE('" + model.date_from + "','YYYY/MM/DD HH24:MI') and TO_DATE('" + model.date_to + "','YYYY/MM/DD HH24:MI') ";
                        }
                        strGetData = strGetData + " union" +
                            " select RecordType,CMCode,to_char(CreateDate, 'YYYYMMDDHH24MISS') CreateDate,CONTENTTYPE,null as col5,CUSTPN,data1 as col7,DEPTCODE as col8,null as col9,null as col10," +
                            " DN,null as col12,null as col13,null as col14,null as col15,null as col16,null as col17,null as col18,null as col19,null as col20," +
                            " null as col21,null as col22,null as col23,null as col24,null as col25,null as col26,LotNumber as col27,TRAYQty as col28,COO as col29,trayID as col30," +
                            " to_char(ShipDate, 'YYYYMMDDHH24MISS') as col31,null as col32,null as col33,null as col34,CUSTVER as col35,null as col36,null as col37,null col38,null as col39,null as col40," +
                            " null as col41, null as col42, null as col43,null as col44,null as col45,null as col46,null as col47,to_char(ShipDate, 'YYYYMMDDHH24MISS') as col48 ,null as col49,null as col50," +
                            " WEEKCODE as col51,null as col52" +
                            " from SFISM4.R_SHIPPING_DATA where CONTENTTYPE = 'LOT' and groupcode = 'ECD'";
                        if (model.date_from != null)
                        {
                            strGetData = strGetData + " AND CREATEDATE between TO_DATE('" + model.date_from + "','YYYY/MM/DD HH24:MI') and TO_DATE('" + model.date_to + "','YYYY/MM/DD HH24:MI') ";
                        }
                        strGetData = strGetData + " ) where RecordType = 'SHIP' ";
                        if (model.model_name.Trim() != "ALL")
                        {
                            strGetData = strGetData + " and CustPN = '" + model.model_name.Trim() + "' ";
                        }
                        strGetData = strGetData + " order by DN,CONTENTTYPE";

                    }
                    else if (model.type == "SUPERCAP")
                    {
                        strGetData = String.Format("select* from(" +
                            " select RecordType, CMCode, to_char(CreateDate,'YYYYMMDDHH24MISS') CreateDate,CONTENTTYPE,null as col5,CUSTPN,data1 as col7,DEPTCODE as col8,to_char(ShipDate, 'YYYYMMDDHH24MISS') as col9 ,to_char(ShipDate, 'YYYYMMDDHH24MISS') as col10," +
                            " DN,null as col12,null as col13,null as col14,null as col15,null as col16,null as col17,data4 as col18,null as col19,null as col20," +
                            " null as col21,ShippingQty as col22,ShippingQty as col23,null as col24,DN as col25,null as col26,LotNumber as col27,CartonQty as col28,COO as col29,CartonID as col30," +
                            " null as col31,null as col32,null as col33,null as col34,CUSTVER as col35,null as col36,null as col37,STDIAG_VER as col38,null as col39,null as col40," +
                            " data3 as col41,null as col42,null as col43,null as col44,null as col45,null as col46,null as col47,null as col48,null as col49,null as col50," +
                            " WEEKCODE as col51,null as col52" +
                            " from SFISM4.R_SHIPPING_DATA where CONTENTTYPE = 'HEADER' and groupcode = 'SUPERCAP'");
                        if (model.date_from != null)
                        {
                            strGetData = strGetData + " AND CREATEDATE between TO_DATE('" + model.date_from + "','YYYY/MM/DD HH24:MI') and TO_DATE('" + model.date_to + "','YYYY/MM/DD HH24:MI') ";
                        }
                        strGetData = strGetData + " union" +
                            " select RecordType,CMCode,to_char(CreateDate, 'YYYYMMDDHH24MISS') CreateDate,CONTENTTYPE,null as col5,CUSTPN,data1 as col7,DEPTCODE as col8,null as col9,null as col10," +
                            " DN,null as col12,null as col13,null as col14,null as col15,null as col16,null as col17,null as col18,null as col19,null as col20," +
                            " null as col21,null as col22,null as col23,null as col24,null as col25,null as col26,LotNumber as col27,CartonQty as col28,COO as col29,CartonID as col30," +
                            " to_char(ShipDate, 'YYYYMMDDHH24MISS') as col31,null as col32,null as col33,null as col34,CUSTVER as col35,null as col36,null as col37,null col38,null as col39,null as col40," +
                            " null as col41, null as col42, null as col43,null as col44,null as col45,null as col46,null as col47,to_char(ShipDate, 'YYYYMMDDHH24MISS') as col48 ,null as col49,null as col50," +
                            " WEEKCODE as col51,null as col52" +
                            " from SFISM4.R_SHIPPING_DATA where CONTENTTYPE = 'LOT' and groupcode = 'SUPERCAP'";
                        if (model.date_from != null)
                        {
                            strGetData = strGetData + " AND CREATEDATE between TO_DATE('" + model.date_from + "','YYYY/MM/DD HH24:MI') and TO_DATE('" + model.date_to + "','YYYY/MM/DD HH24:MI') ";
                        }
                        strGetData = strGetData + " ) where RecordType = 'SHIP' ";
                        if (model.model_name.Trim() != "ALL")
                        {
                            strGetData = strGetData + " and CustPN = '" + model.model_name.Trim() + "' ";
                        }
                        strGetData = strGetData + " order by DN,CONTENTTYPE";

                    }
                }
                else if (model.record_type == "BDSN")
                {
                    if (model.type == "NIC")
                    {
                        strGetData = String.Format("select RecordType,CMCode,SN,SSN,LotNumber,LicenseID, null as col7,CustPN,null as col9,COO," +
                            " null as col11, null as col12, null as col13, null as col14, null as col15, null as col16, null as col17, null as col18, null as col19, null as col20" +
                            " from SFISM4.R_SHIPPING_DATA where RECORDTYPE = 'BDSN' AND groupcode = 'NIC' ");
                        if (model.model_name.Trim() != "ALL")
                        {
                            strGetData = strGetData + " and CustPN = '" + model.model_name.Trim() + "' ";
                        }
                        if (model.date_from != null)
                        {
                            strGetData = strGetData + " AND CREATEDATE between TO_DATE('" + model.date_from + "','YYYY/MM/DD HH24:MI') and TO_DATE('" + model.date_to + "','YYYY/MM/DD HH24:MI') ";
                        }
                        strGetData = strGetData + " order by SN";
                    }
                    else if (model.type == "ECD")
                    {
                        strGetData = String.Format("select RecordType,CMCode,SN,SSN,LotNumber,TrayID, null as col7,CustPN,null as col9,COO," +
                            " null as col11, null as col12, null as col13, null as col14, null as col15, null as col16, null as col17, null as col18, null as col19, null as col20" +
                            " from SFISM4.R_SHIPPING_DATA where RECORDTYPE = 'BDSN' AND groupcode = 'ECD' ");
                        if (model.model_name.Trim() != "ALL")
                        {
                            strGetData = strGetData + " and CustPN = '" + model.model_name.Trim() + "' ";
                        }
                        if (model.date_from != null)
                        {
                            strGetData = strGetData + " AND CREATEDATE between TO_DATE('" + model.date_from + "','YYYY/MM/DD HH24:MI') and TO_DATE('" + model.date_to + "','YYYY/MM/DD HH24:MI') ";
                        }
                        strGetData = strGetData + " order by SN";
                    }
                    else if (model.type == "SUPERCAP")
                    {
                        strGetData = String.Format("select RecordType,CMCode,SN,SSN,LotNumber,CartonID,to_char(CreateDate,'YYYYMMDDHH24MISS') CreateDate,CustPN,null as col9,COO," +
                            " null as col11, null as col12 from SFISM4.R_SHIPPING_DATA where RECORDTYPE = 'BDSN' AND groupcode = 'SUPERCAP'");
                        if (model.model_name.Trim() != "ALL")
                        {
                            strGetData = strGetData + " and CustPN = '" + model.model_name.Trim() + "' ";
                        }
                        if (model.date_from != null)
                        {
                            strGetData = strGetData + " AND CREATEDATE between TO_DATE('" + model.date_from + "','YYYY/MM/DD HH24:MI') and TO_DATE('" + model.date_to + "','YYYY/MM/DD HH24:MI') ";
                        }
                        strGetData = strGetData + " order by SN";
                    }
                }
                else if (model.record_type == "SHPCFM")
                {
                    if (model.type == "NIC")
                    {
                        strGetData = String.Format("select RecordType,CMCode,Data1,Data2,'MAN'||LICENSEID col5,data3 as col6,data4 as col7,CustPN,LotNumber,1 as QTY," +
                            " LICENSEID, SN, SSN, null as col14, null as col15, null as col16, null as col17, null as col18, null as col19, null as col20," +
                            " null as col21, null as col22, null as col23, null as col24, null as col25, null as col26, null as col27, null as col28" +
                            " from SFISM4.R_SHIPPING_DATA where RECORDTYPE = 'SHPCFM' AND groupcode = 'NIC'");
                        if (model.model_name.Trim() != "ALL")
                        {
                            strGetData = strGetData + " and CustPN = '" + model.model_name.Trim() + "' ";
                        }
                        if (model.date_from != null)
                        {
                            strGetData = strGetData + " AND CREATEDATE between TO_DATE('" + model.date_from + "','YYYY/MM/DD HH24:MI') and TO_DATE('" + model.date_to + "','YYYY/MM/DD HH24:MI') ";
                        }
                        strGetData = strGetData + " order by SN";
                    }
                    else if (model.type == "ECD")
                    {
                        strGetData = String.Format("select RecordType,CMCode,Data1,Data2,'MAN'||LICENSEID col5,data3 as col6,data4 as col7,CustPN,LotNumber,1 as QTY," +
                            " LICENSEID, SN, SSN, null as col14, null as col15, null as col16, null as col17, null as col18, null as col19, null as col20," +
                            " null as col21, null as col22, null as col23, null as col24, null as col25, null as col26, null as col27, null as col28" +
                            " from SFISM4.R_SHIPPING_DATA where DN in ('5142030073', '5145004953') and RECORDTYPE = 'SHPCFM' AND groupcode = 'ECD'");
                        if (model.model_name.Trim() != "ALL")
                        {
                            strGetData = strGetData + " and CustPN = '" + model.model_name.Trim() + "' ";
                        }
                        if (model.date_from != null)
                        {
                            strGetData = strGetData + " AND CREATEDATE between TO_DATE('" + model.date_from + "','YYYY/MM/DD HH24:MI') and TO_DATE('" + model.date_to + "','YYYY/MM/DD HH24:MI') ";
                        }
                        strGetData = strGetData + " order by SN";
                    }
                    else if (model.type == "SUPERCAP")
                    {
                        //strGetData = String.Format("");
                    }
                }
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

        public static DataTable ExecuteProdecuce(string proName, string dbName, string fiel1, string fiel2, string fiel3, string fiel4, string fiel5, string fiel6)
        {
            var conn = new DbContext().Connect(dbName);

            // string spSql = "BEGIN STORED_PROC_NAME(:IN_PARAM, :OUT_PARAM1, :OUT_PARAM2); END; ";
            DataTable dt = new DataTable();
            DataSet dataset = new DataSet();

            OracleCommand objCmd = new OracleCommand(proName, conn);


            objCmd.CommandText = proName;// "pkg_return_table.ProcReturnDiTable";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("IN_DN", OracleDbType.Varchar2).Value = fiel1;
            objCmd.Parameters.Add("IN_PN", OracleDbType.Varchar2).Value = fiel2;
            objCmd.Parameters.Add("IN_TIME1", OracleDbType.Varchar2).Value = fiel3;
            objCmd.Parameters.Add("IN_TIME2", OracleDbType.Varchar2).Value = fiel4;
            objCmd.Parameters.Add("IN_DATA", OracleDbType.Varchar2).Value = fiel5;
            objCmd.Parameters.Add("IN_TYPE", OracleDbType.Varchar2).Value = fiel6;
            objCmd.Parameters.Add("o_dt", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("res", OracleDbType.Varchar2).Direction = ParameterDirection.Output;
            try
            {



                //objConn.Open();
                objCmd.ExecuteNonQuery();

                var res = objCmd.Parameters["res"].Value.ToString();



                OracleDataAdapter da = new OracleDataAdapter(objCmd);


                da.Fill(dataset);


                return dt = dataset.Tables[0];
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception: {0}", ex.ToString());
                return dt = null;
            }


        }
        #endregion

        #region SHIPPING FILE TELIT
        [System.Web.Http.Route("GetShippingTelit")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetShippingTelit(LinkDnElement model)
        {
            try
            {
                string checkExistsProcedure = "SFIS1.CHECK_DN_DETAILS";
                string procedureResult = "";
                List<Dictionary<string, object>> cursorData = new List<Dictionary<string, object>>();
                var connectionString = new GetString().Get()[model.database_name];
                using (var connection = new OracleConnection(connectionString))
                {
                    using (var command = new OracleCommand(checkExistsProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("P_EMP_NO", OracleDbType.Varchar2).Value = "";
                        command.Parameters.Add("P_BFIH_DN", OracleDbType.Varchar2).Value = "";
                        command.Parameters.Add("P_FTI_DN", OracleDbType.Varchar2).Value = model.FTI_DN;
                        command.Parameters.Add("P_NOTE_DN", OracleDbType.Varchar2).Value = "";
                        command.Parameters.Add("P_FUNCTION", OracleDbType.Varchar2).Value = "CHECK_FTI_DN";

                        command.Parameters.Add("o_dt", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        command.Parameters.Add("RES", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                        connection.Open();
                        await command.ExecuteNonQueryAsync();
                        procedureResult = command.Parameters["RES"].Value.ToString();


                        try
                        {
                            var reader = ((OracleRefCursor)command.Parameters["o_dt"].Value).GetDataReader();

                            while (await reader.ReadAsync())
                            {
                                var row = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row[reader.GetName(i)] = reader.GetValue(i);
                                }
                                cursorData.Add(row);
                            }
                        }
                        catch
                        {
                            cursorData = new List<Dictionary<string, object>>();
                        }
                        //if (command.Parameters["o_dt"].Value is OracleRefCursor cursor)
                        //{
                        //    try
                        //    {
                        //        using (var reader = cursor.GetDataReader())
                        //        {
                        //            while (await reader.ReadAsync())
                        //            {
                        //                var row = new Dictionary<string, object>();
                        //                for (int i = 0; i < reader.FieldCount; i++)
                        //                {
                        //                    row[reader.GetName(i)] = reader.GetValue(i);
                        //                }
                        //                cursorData.Add(row);
                        //            }
                        //        }
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        cursorData = new List<Dictionary<string, object>>();
                        //    }
                        //}
                        //else
                        //{
                        //    cursorData = new List<Dictionary<string, object>>();
                        //}
                    }
                }
                if (procedureResult != "OK")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = cursorData });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = cursorData, data = procedureResult });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }
        #endregion
    }
}