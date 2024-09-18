using Oracle.ManagedDataAccess.Client;
using SN_API.Models;
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
    public class QueryModelController : ApiController
    {
        [System.Web.Http.Route("SpB_QueryClick")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> SpB_QueryClick(ValueQueryModel value)
        {
            var str_sql = "";
            var str_table = "";
            var sLocsql = "";
            var mo_number_list = "";
            int qty = 0;
            DataTable dt = null;


            if (value.table == "R")
                str_table = " sfism4.R_wip_tracking_t ";
            if (value.table == "Z")
                str_table = " sfism4.Z_wip_tracking_t ";
            if (value.table == "H")
                str_table = " sfism4.H_wip_tracking_t ";
            if (value.table == "RHIS")
                str_table = " sfism4.R_wip_tracking_t@SFCODBH ";
            if (value.table == "ZHIS")
                str_table = " sfism4.Z_wip_tracking_t@SFCODBH ";

            if (value.model.Trim() == "" || value.version.Trim() == "" || value.group.Trim() == "")
            {
                if (value.RB_check == "PC")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { message = "Input find Condition!" });
                }
            }

            if (value.location.Trim() == "ALL" || value.location.Trim() == "")
            {
                sLocsql = "";
            }
            else
            {
                sLocsql = "LOCATION='" + value.location.Trim() + "' AND ";
            }

            //------------------------------PCH:2005-12-27---add-------------------------------
            if (value.RB_check != "PC")
            {
                if (value.model.Trim() == "ALL" || value.version.Trim() == "ALL")
                {
                    if (value.RB_check == "STOCKIN")
                    {
                        if (value.period)
                        {
                            str_sql = "SELECT DISTINCT MODEL_NAME,COUNT(SERIAL_NUMBER)TOTAL,VERSION_CODE,MO_NUMBER FROM '+str_table+' WHERE SERIAL_NUMBER IN( SELECT distinct serial_number FROM sfism4.R_SN_DETAIL_T ";
                            if (value.date_to != "ALL")
                            {
                                str_sql = str_sql + " and in_station_time >= TO_DATE('" + value.date_from + "','YYYY/MM/DD HH24:MI')" +
                                    " AND in_station_time <= TO_DATE('" + value.date_to + "','YYYY/MM/DD HH24:MI')";
                            }
                            else
                            {
                                str_sql = str_sql + " and in_station_time >= TO_DATE('" + value.date_from + "','YYYY/MM/DD HH24:MI')" +
                                    " AND in_station_time <= TO_DATE('" + value.date_to + "','YYYY/MM/DD HH24:MI')";
                            }
                            str_sql = str_sql + "and group_name = ''STOCKIN'' AND STOCK_NO IS NOT NULL) and stock_no is not null  group by model_name,VERSION_CODE,MO_NUMBER order by model_name'";
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { message = "Checked Time Stockin!" });
                        }
                    }

                    if (value.RB_check == "CQA")
                    {

                        if (value.model.Trim() == "ALL" && value.version.Trim() == "ALL")
                        {
                            str_sql = "SELECT  MODEL_NAME,COUNT(*) TOTAL_COUNT,VERSION_CODE,MO_NUMBER,TRACK_NO AS TA_VER FROM " + str_table +
                                    "WHERE " + sLocsql + "  wip_group='" + value.group.Trim() + "'";
                        }
                        if (value.model.Trim() == "ALL" && value.version.Trim() != "ALL")
                        {
                            str_sql = "SELECT  MODEL_NAME,COUNT(*) TOTAL_COUNT,VERSION_CODE,MO_NUMBER,TRACK_NO AS TA_VER FROM " + str_table +
                                    "WHERE " + sLocsql + "TRACK_NO='" + value.version + "' AND  wip_group='" + value.group.Trim() + "'";
                        }
                        if (value.model.Trim() != "ALL" && value.version.Trim() == "ALL")
                        {
                            str_sql = "SELECT  MODEL_NAME,COUNT(*) TOTAL_COUNT,VERSION_CODE,MO_NUMBER,TRACK_NO AS TA_VER FROM " + str_table +
                                    "WHERE " + sLocsql + "MODEL_NAME='" + value.model.Trim() + "' AND  wip_group='" + value.group.Trim() + "'";
                        }


                        if (value.period)
                        {

                            if (value.date_to != "ALL")
                            {
                                str_sql = str_sql + " and in_station_time >= TO_DATE('" + value.date_from + "','YYYY/MM/DD HH24:MI')" +
                                    " AND in_station_time <= TO_DATE('" + value.date_to + "','YYYY/MM/DD HH24:MI')";
                            }
                            else
                            {
                                str_sql = str_sql + " and in_station_time >= TO_DATE('" + value.date_from + "','YYYY/MM/DD HH24:MI')" +
                                    " AND in_station_time <= TO_DATE('" + value.date_to + "','YYYY/MM/DD HH24:MI')";
                            }
                        }
                        str_sql = str_sql + " group by MODEL_NAME,VERSION_CODE,MO_NUMBER,TRACK_NO order by MODEL_NAME ";

                    }
                    var wipall = "('FG', 'FGHOLD', 'FG_HOLD', 'FG-HOLD', 'HOLD-FG', 'FG_CHECK', 'FG_CHECK_HOLD','HOLDFG', 'HOLDFG_CHECK','HOLD-FG_CHECK', 'HOLDFG_HOLD')";
                    if (value.RB_check == "WHS")
                    {

                        if (value.model.Trim() == "ALL" && value.version.Trim() == "ALL" && value.group.Trim() != "ALL")
                        {
                            str_sql = "SELECT MODEL_NAME,COUNT(*) TOTAL_COUNT,VERSION_CODE,MO_NUMBER,TRACK_NO AS TA_VER FROM " + str_table +
                                    " WHERE " + sLocsql + " wip_group='" + value.group.Trim() + "'";
                        }
                        if (value.model.Trim() == "ALL" && value.version.Trim() == "ALL" && value.group.Trim() == "ALL")
                        {
                            str_sql = "SELECT MODEL_NAME,COUNT(*) TOTAL_COUNT,VERSION_CODE,MO_NUMBER,TRACK_NO AS TA_VER FROM " + str_table +
                                    " WHERE " + sLocsql + " wip_group in" + wipall;
                        }

                        if (value.model.Trim() == "ALL" && value.version.Trim() != "ALL" && value.group.Trim() != "ALL")
                        {
                            str_sql = "SELECT MODEL_NAME,COUNT(*) TOTAL_COUNT,VERSION_CODE,MO_NUMBER,TRACK_NO AS TA_VER FROM " + str_table +
                                    " WHERE " + sLocsql + "TRACK_NO = '" + value.version.Trim() + "' and wip_group='" + value.group.Trim() + "'";
                        }
                        if (value.model.Trim() == "ALL" && value.version.Trim() != "ALL" && value.group.Trim() == "ALL")
                        {
                            str_sql = "SELECT MODEL_NAME,COUNT(*) TOTAL_COUNT,VERSION_CODE,MO_NUMBER,TRACK_NO AS TA_VER FROM " + str_table +
                                    " WHERE " + sLocsql + "TRACK_NO = '" + value.version.Trim() + "' and wip_group in" + wipall;
                        }

                        if (value.model.Trim() != "ALL" && value.version.Trim() == "ALL" && value.group.Trim() != "ALL")
                        {
                            str_sql = "SELECT MODEL_NAME,COUNT(*) TOTAL_COUNT,VERSION_CODE,MO_NUMBER,TRACK_NO AS TA_VER FROM " + str_table +
                                    " WHERE " + sLocsql + "MODEL_NAME = '" + value.model.Trim() + "' and wip_group='" + value.group.Trim() + "'";
                        }
                        if (value.model.Trim() != "ALL" && value.version.Trim() != "ALL" && value.group.Trim() == "ALL")
                        {
                            str_sql = "SELECT MODEL_NAME,COUNT(*) TOTAL_COUNT,VERSION_CODE,MO_NUMBER,TRACK_NO AS TA_VER FROM " + str_table +
                                    " WHERE " + sLocsql + "MODEL_NAME = '" + value.model.Trim() + "' and wip_group in" + wipall;
                        }
                        if (value.model.Trim() != "ALL" && value.version.Trim() == "ALL" && value.group.Trim() == "ALL")
                        {
                            str_sql = "SELECT MODEL_NAME,COUNT(*) TOTAL_COUNT,VERSION_CODE,MO_NUMBER,TRACK_NO AS TA_VER FROM " + str_table +
                                    " WHERE " + sLocsql + "MODEL_NAME = '" + value.model.Trim() + "' and wip_group in" + wipall;
                        }


                        if (value.period)
                        {

                            if (value.date_to != "ALL")
                            {
                                str_sql = str_sql + " and in_station_time >= TO_DATE('" + value.date_from + "','YYYY/MM/DD HH24:MI')" +
                                    " AND in_station_time <= TO_DATE('" + value.date_to + "','YYYY/MM/DD HH24:MI')";
                            }
                            else
                            {
                                str_sql = str_sql + " and in_station_time >= TO_DATE('" + value.date_from + "','YYYY/MM/DD HH24:MI')" +
                                    " AND in_station_time <= TO_DATE('" + value.date_to + "','YYYY/MM/DD HH24:MI')";
                            }
                        }
                        str_sql = str_sql + " group by MODEL_NAME,VERSION_CODE,MO_NUMBER,TRACK_NO order by MODEL_NAME ";

                    }


                    dt = DBConnect.GetData(str_sql, value.database);
                    foreach (DataRow row in dt.Rows)
                    {
                        qty = qty + int.Parse(row[1].ToString());
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new { data = dt, qty = qty, data1 = "", query = "", result = "ok" });
                }
            }
            //------------------------------PCH:2005-12-27---add-------------------------------

            if (value.RB_check == "CQA")
            {

                if (value.mo_number == null || value.mo_number == "")
                {
                    if (value.group == "ALL")
                    {

                        str_sql = "select SERIAL_NUMBER,IN_STATION_TIME,CARTON_NO,MCARTON_NO,PALLET_NO,IMEI,SHIPPING_SN,SHIPPING_SN2,MO_NUMBER,MODEL_NAME,VERSION_CODE,MO_NUMBER,QA_NO,TRACK_NO,WIP_GROUP " +
                        "FROM" + str_table + "WHERE " + sLocsql + "MODEL_NAME='" + value.model.Trim() + "' and VERSION_CODE ='" +
                        value.version.Trim() + "'";
                    }
                    else
                    {

                        str_sql = "select SERIAL_NUMBER,IN_STATION_TIME,CARTON_NO,MCARTON_NO,PALLET_NO,IMEI,SHIPPING_SN,SHIPPING_SN2,MO_NUMBER,MODEL_NAME,VERSION_CODE,MO_NUMBER,QA_NO,TRACK_NO,WIP_GROUP " +
                        "FROM" + str_table + "WHERE " + sLocsql + "MODEL_NAME='" + value.model.Trim() + "' and VERSION_CODE ='" +
                        value.version.Trim() + "' and wip_group='" + value.group.Trim() + "' ";
                    }
                }
                else
                {
                    if (value.group == "ALL")
                    {
                        str_sql = "select SERIAL_NUMBER,IN_STATION_TIME,CARTON_NO,MCARTON_NO,PALLET_NO,IMEI,SHIPPING_SN,SHIPPING_SN2,MO_NUMBER,MODEL_NAME,VERSION_CODE,MO_NUMBER,QA_NO,TRACK_NO,WIP_GROUP " +
                            "FROM" + str_table + "WHERE " + sLocsql + "MODEL_NAME='" + value.model.Trim() + "' and VERSION_CODE ='" +
                            value.version.Trim() + "' and mo_number = '"+ value.model.Trim() + "'";
                    }
                    else
                    {
                        str_sql = "select SERIAL_NUMBER,IN_STATION_TIME,CARTON_NO,MCARTON_NO,PALLET_NO,IMEI,SHIPPING_SN,SHIPPING_SN2,MO_NUMBER,MODEL_NAME,VERSION_CODE,MO_NUMBER,QA_NO,TRACK_NO,WIP_GROUP " +
                            "FROM" + str_table + "WHERE " + sLocsql + "MODEL_NAME='" + value.model.Trim() + "' and VERSION_CODE ='" +
                            value.version.Trim() + "' and mo_number = '" + value.model.Trim() + "' and wip_group='" + value.group.Trim() + "' ";
                    }
                }

                if (value.period)
                {

                    if (value.date_to != "ALL")
                    {
                        str_sql = str_sql + " and in_station_time >= TO_DATE('" + value.date_from + "','YYYY/MM/DD HH24:MI')" +
                            " AND in_station_time <= TO_DATE('" + value.date_to + "','YYYY/MM/DD HH24:MI')";
                    }
                    else
                    {
                        str_sql = str_sql + " and in_station_time >= TO_DATE('" + value.date_from + "','YYYY/MM/DD HH24:MI')" +
                            " AND in_station_time <= TO_DATE('" + value.date_to + "','YYYY/MM/DD HH24:MI')";
                    }


                }
                str_sql = str_sql + " group by SERIAL_NUMBER,IN_STATION_TIME,CARTON_NO,MCARTON_NO,PALLET_NO,IMEI,SHIPPING_SN,SHIPPING_SN2,MO_NUMBER,MODEL_NAME,VERSION_CODE,MO_NUMBER,TRACK_NO,QA_NO,WIP_GROUP order by SHIPPING_SN";
                dt = DBConnect.GetData(str_sql, value.database);

                return Request.CreateResponse(HttpStatusCode.OK, new { data = dt, qty = 0, data1 = "", query = "", result = "ok" });
            }

            if (value.RB_check == "WHS")
            {
                if (!value.checkbymo)
                {
                    if (value.group == "ALL")
                    {
                        str_sql = $"select distinct model_name, version_code,Track_NO AS TA_VER,wip_group,MAX(IN_STATION_TIME) AS IN_STATION_STOCK,MIN(IN_STATION_TIME) AS IN_STATION_TIME,imei,count(imei||'0') AS COUNT_IMEI " +
                        $"from  {str_table}  where {sLocsql} model_name='{value.model.Trim()}' and version_code='{value.version.Trim()}'";
                    }
                    else                
                    {
                        str_sql = $"select distinct model_name, version_code,Track_NO AS TA_VER,wip_group,MAX(IN_STATION_TIME) AS IN_STATION_STOCK,MIN(IN_STATION_TIME) AS IN_STATION_TIME,imei,count(imei||'0') AS COUNT_IMEI " +
                        $"from  {str_table}  where {sLocsql} model_name='{value.model.Trim()}' and version_code='{value.version.Trim()}' and wip_group='{value.group.Trim()}' ";
                    }

                    if (value.period)
                    {
                        if (value.date_to != "ALL")
                        {
                            str_sql = str_sql + " and in_station_time >= TO_DATE('" + value.date_from + "','YYYY/MM/DD HH24:MI')" +
                                " AND in_station_time <= TO_DATE('" + value.date_to + "','YYYY/MM/DD HH24:MI')";
                        }
                        else
                        {
                            str_sql = str_sql + " and in_station_time >= TO_DATE('" + value.date_from + "','YYYY/MM/DD HH24:MI')" +
                                " AND in_station_time <= TO_DATE('" + value.date_to + "','YYYY/MM/DD HH24:MI')";
                        }
                    }
                    str_sql = str_sql + " group by model_name,VERSION_CODE,MO_NUMBER,Track_No,wip_group,imei order by imei";
                    dt = DBConnect.GetData(str_sql, value.database);
                    foreach (DataRow row in dt.Rows)
                    {
                        qty = qty + int.Parse(row[7].ToString());
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new { data = dt, qty = qty, data1 = "", query = "", result = "ok" });
                }
            }

            if (value.RB_check == "WHS" && value.visible)
            {
                if (value.CSQ == "ALL")
                {
                    str_sql = ("SELECT MO_NUMBER, BILL_NO,IMEI, COUNT(IMEI)  COUNT_IMEI   from " + str_table + " where " + sLocsql + "  " +
                        " model_name='" + value.model.Trim() + "' AND  MO_NUMBER  IN(SELECT MO_NUMBER FROM SFISM4.R_MO_BASE_T  WHERE " +
                        " PO_NO = '" + value.cust_po.Trim() + "') and wip_group = '" + value.group.Trim() + "' " +
                        " GROUP BY  MO_NUMBER, BILL_NO, IMEI  order by MO_NUMBER, IMEI");
                }
                else if (value.CSQ == "CSQ")
                {
                    str_sql = ("SELECT MO_NUMBER, BILL_NO,IMEI, COUNT(IMEI)  COUNT_IMEI   from " + str_table + " where " + sLocsql + "  " +
                        " model_name='" + value.model.Trim() + "' AND  MO_NUMBER  IN(SELECT MO_NUMBER FROM SFISM4.R_MO_BASE_T  WHERE " +
                        " PO_NO = '" + value.cust_po.Trim() + "') and wip_group = '" + value.group.Trim() + "' " +
                        " AND  BILL_NO IS NOT NULL  GROUP BY  MO_NUMBER, IMEI ,BILL_NO order by MO_NUMBER, IMEI");
                }
                else if (value.CSQ == "CSQ")
                {
                    str_sql = ("SELECT MO_NUMBER, BILL_NO,IMEI, COUNT(IMEI)  COUNT_IMEI   from " + str_table + " where " + sLocsql + "  " +
                        " model_name='" + value.model.Trim() + "' AND  MO_NUMBER  IN(SELECT MO_NUMBER FROM SFISM4.R_MO_BASE_T  WHERE " +
                        " PO_NO = '" + value.cust_po.Trim() + "') and wip_group = '" + value.group.Trim() + "' " +
                        " AND  BILL_NO IS  NULL  GROUP BY  MO_NUMBER, BILL_NO, IMEI  order by MO_NUMBER, IMEI");
                }
            }
            else if (value.RB_check == "WHS")
            {
                if (value.checkbymo)
                {
                    if (value.mo_number == null ||value.mo_number == "")
                    {
                        str_sql = "SELECT distinct A.MODEL_NAME,A.MO_NUMBER,A.TRACK_NO AS TA_VER,A.VERSION_CODE,A.WIP_GROUP ,max(b.in_station_time) " +
                            "as in_time_stockin,MIN(A.IN_STATION_TIME) AS IN_STATION_TIME, a.imei,count(a.imei || '0') as count_imei " +
                            "FROM sfism4.z107 a, sfism4.r107 b where a.SERIAL_NUMBER = b.SERIAL_NUMBER AND A.STOCK_NO IS NOT NULL " +
                            "AND " + sLocsql + "  A.model_name = '" + value.model.Trim() + "' and A.version_code = '" + value.version.Trim() + "'";
                    }
                    else
                    {
                        str_sql = "SELECT distinct A.MODEL_NAME,A.MO_NUMBER,A.TRACK_NO AS TA_VER,A.VERSION_CODE,A.WIP_GROUP ,max(b.in_station_time) " +
                            "as in_time_stockin,MIN(A.IN_STATION_TIME) AS IN_STATION_TIME, a.imei,count(a.imei || '0') as count_imei " +
                            "FROM sfism4.z107 a, sfism4.r107 b where a.SERIAL_NUMBER = b.SERIAL_NUMBER AND A.STOCK_NO IS NOT NULL " +
                            "AND " + sLocsql + "  A.model_name = '" + value.model.Trim() + "' and A.version_code = '" + value.version.Trim() + "'" +
                            "AND MO_NUMBER = '" + value.mo_number.Trim() + "'";
                    }
                    if (value.period)
                    {

                        if (value.date_to != "ALL")
                        {
                            str_sql = str_sql + " and in_station_time >= TO_DATE('" + value.date_from + "','YYYY/MM/DD HH24:MI')" +
                                " AND in_station_time <= TO_DATE('" + value.date_to + "','YYYY/MM/DD HH24:MI')";
                        }
                        else
                        {
                            str_sql = str_sql + " and in_station_time >= TO_DATE('" + value.date_from + "','YYYY/MM/DD HH24:MI')" +
                                " AND in_station_time <= TO_DATE('" + value.date_to + "','YYYY/MM/DD HH24:MI')";
                        }
                    }
                    str_sql = str_sql + " group by A.model_name,A.MO_NUMBER,A.TRACK_NO,A.version_code,A.wip_group,A.imei order by A.imei";
                    dt = DBConnect.GetData(str_sql, value.database);
                    foreach (DataRow row in dt.Rows)
                    {
                        qty = qty + int.Parse(row[9].ToString());
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new { data = dt, qty = qty, data1 = "", query = "", result = "ok" });
                }

                if (value.CSQ == "ALL" && value.visible)
                {
                    str_sql = "SELECT COUNT(*)  COUNT_IMEI from " + str_table + " where model_name='" + value.model + "' AND MO_NUMBER IN" +
                        "(SELECT MO_NUMBER FROM SFISM4.R_MO_BASE_T  WHERE  PO_NO||PO_ITEM = '" + value.cust_po + "') AND WIP_GROUP ='" + value.group + "'";
                }
                else if (value.CSQ == "CSQ" && value.visible)
                {
                    str_sql = "SELECT COUNT(*)  COUNT_IMEI from " + str_table + " where model_name='" + value.model + "' AND MO_NUMBER IN" +
                        "(SELECT MO_NUMBER FROM SFISM4.R_MO_BASE_T  WHERE  PO_NO||PO_ITEM = '" + value.cust_po + "') AND WIP_GROUP ='" + value.group + "'  AND  BILL_NO IS NOT NULL ";
                }
                else if (value.CSQ == "NOCSQ" && value.visible)
                {
                    str_sql = "SELECT COUNT(*)  COUNT_IMEI from " + str_table + " where model_name='" + value.model + "' AND MO_NUMBER IN" +
                        "(SELECT MO_NUMBER FROM SFISM4.R_MO_BASE_T  WHERE  PO_NO||PO_ITEM = '" + value.cust_po + "') AND WIP_GROUP ='" + value.group + "'  AND  BILL_NO IS NULL ";
                }
                else if (value.mo_number != null && value.mo_number != "" )
                {
                    if (value.group == "ALL")
                    {
                        str_sql = "SELECT COUNT(IMEI||'0') AS COUNT_IMEI from " + str_table + " where model_name='" + value.model + "'" +
                        "and version_code='" + value.version + "' and  mo_number='" + value.mo_number + "'";
                    }
                    else
                    {
                        str_sql = "SELECT COUNT(IMEI||'0') AS COUNT_IMEI from " + str_table + " where model_name='" + value.model + "'" +
                       "and version_code='" + value.version + "' and  mo_number='" + value.mo_number + "' and wip_group='" + value.group + "'";
                    }
                }
                else
                {
                    if (value.group == "ALL")
                    {
                        str_sql = "SELECT COUNT(IMEI||'0') AS COUNT_IMEI from " + str_table + " where model_name='" + value.model + "'" +
                        "and version_code='" + value.version + "'";
                    }
                    else
                    {
                        str_sql = "SELECT COUNT(IMEI||'0') AS COUNT_IMEI from " + str_table + " where model_name='" + value.model + "'" +
                       "and version_code='" + value.version + "' and wip_group='" + value.group + "'";
                    }
                }

                if (value.period)
                {
                    if (value.date_to != "ALL")
                    {
                        str_sql = str_sql + " and in_station_time >= TO_DATE('" + value.date_from + "','YYYY/MM/DD HH24:MI')" +
                            " AND in_station_time <= TO_DATE('" + value.date_to + "','YYYY/MM/DD HH24:MI')";
                    }
                    else
                    {
                        str_sql = str_sql + " and in_station_time >= TO_DATE('" + value.date_from + "','YYYY/MM/DD HH24:MI')" +
                            " AND in_station_time <= TO_DATE('" + value.date_to + "','YYYY/MM/DD HH24:MI')";
                    }
                }
                str_sql = str_sql + " group by A.model_name,A.MO_NUMBER,A.TRACK_NO,A.version_code,A.wip_group,A.imei order by A.imei";
                dt = DBConnect.GetData(str_sql, value.database);
                foreach (DataRow row in dt.Rows)
                {
                    qty = qty + int.Parse(row[0].ToString());
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { data = dt, qty = qty, data1 = "", query = "", result = "ok" });
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { data = dt, data1 = "", query = "", result = "ok" });
        }


        [System.Web.Http.Route("sp8_QueryByModel")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> sp8_QueryByModel(ValueQueryModel value)
        {
            var str_sql = "";
            var str_table = "";
            var sLocsql = "";
            var table2 = "";
            DataTable dt = null;
            DataTable dt_ver = null;
            DataTable dt_mo = null;
            DataTable dtcust_po = null;


            if (value.table == "R")
            {
                str_table = " sfism4.R_wip_tracking_t ";
                table2 = " sfism4.r_mo_base_t ";
            }
            if (value.table == "Z")
            {
                str_table = " sfism4.Z_wip_tracking_t ";
                table2 = " sfism4.r_mo_base_t ";
            }
            if (value.table == "H")
            {
                str_table = " sfism4.H_wip_tracking_t ";
                table2 = " sfism4.h_mo_base_t ";
            }
            if (value.table == "RHIS")
            {
                str_table = " sfism4.R_wip_tracking_t@SFCODBH ";
                table2 = " sfism4.h_mo_base_t ";
            }
            if (value.table == "ZHIS")
            {
                str_table = " sfism4.Z_wip_tracking_t@SFCODBH ";
                table2 = " sfism4.r_mo_base_t@SFCODBH ";
            }

            string str_ver = $"SELECT VERSION_CODE FROM SFIS1.C_CUST_SNRULE_T where MODEL_NAME = '{value.model}'";
            dt_ver = DBConnect.GetData(str_ver, value.database);

            string str_mo = $"Select distinct MO_NUMBER from  {table2}  where  MODEL_NAME = '{value.model}' ";
            dt_mo = DBConnect.GetData(str_mo, value.database);

            string str_po = $"Select distinct cust_po from sfism4.r_bpcs_invoice_t where  MODEL_NAME = '{value.model}'";
            dtcust_po = DBConnect.GetData(str_po, value.database);

            return Request.CreateResponse(HttpStatusCode.OK, new { data = dt, version = dt_ver, cust_po = dtcust_po, dt_mo = dt_mo, data1 = "", query = "", result = "ok" });
        }

        [System.Web.Http.Route("sp8_AllModel")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> sp8_AllModel(ValueQueryModel value)
        {
            string strGetData = "select DISTINCT MODEL_NAME from SFIS1.C104 where 1 = 1 order by MODEL_NAME";

            DataTable dtCheck = DBConnect.GetData(strGetData, value.database);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }


                [System.Web.Http.Route("GetAllModel_QR")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetAllModel_QR(ValueQueryModel model)
        {
            string strGetData = "";

            strGetData = $" select MODEL_NAME FROM SFIS1.C_MODEL_DESC_T ORDER BY MODEL_NAME";


            DataTable dtCheck = DBConnect.GetData(strGetData, model.database);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }


        [System.Web.Http.Route("GetGroup_byModel")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetGroup_byModel(ValueQueryModel model)
        {
            string strGetData = "";

            strGetData = $" select MODEL_NAME FROM SFIS1.C_MODEL_DESC_T ORDER BY MODEL_NAME";


            DataTable dtCheck = DBConnect.GetData(strGetData, model.database);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }
        [System.Web.Http.Route("GetCustPO_byModel")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetCustPO_byModel(ValueQueryModel model)
        {
            string strGetData = "";

            strGetData = $" SELECT DISTINCT CUST_PO  FROM sfism4.R_BPCS_INVOICE_T  where model_name =  '{model.model_name}' ";


            DataTable dtCheck = DBConnect.GetData(strGetData, model.database);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }


        [System.Web.Http.Route("Query_ModelContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Query_ModelContent(ValueQueryModel model)
        {
            string strGetData = "";
            double mo_qty = 0;
            DataTable dtCheck3 = ExecuteProdecuce("SFIS1.PrReturnQuery_Model_FN", model.database, model.where_fiel1, model.where_fiel2, model.where_fiel3);

            mo_qty = ExecuteProdecuceDb("SFIS1.PrReturnQuery_Model_FN", model.database, model.where_fiel1, model.where_fiel2, model.where_fiel3);

            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck3, mo_QTY = mo_qty.ToString() });

        }


        public static DataTable ExecuteProdecuce(string proName, string dbName, string where_fiel1, string where_fiel2, string where_fiel3)
        {
            var conn = new DbContext().Connect(dbName);

            // string spSql = "BEGIN STORED_PROC_NAME(:IN_PARAM, :OUT_PARAM1, :OUT_PARAM2); END; ";
            DataTable dt = new DataTable();
            DataSet dataset = new DataSet();

            OracleCommand objCmd = new OracleCommand(proName, conn);


            objCmd.CommandText = proName;// "pkg_return_table.ProcReturnDiTable";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("where_condition_row1", OracleDbType.Varchar2).Value = where_fiel1;
            objCmd.Parameters.Add("where_condition_row2", OracleDbType.Varchar2).Value = where_fiel2;
            objCmd.Parameters.Add("where_condition_row3", OracleDbType.Varchar2).Value = where_fiel3;
            objCmd.Parameters.Add("o_remCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("moQty", OracleDbType.Double).Direction = ParameterDirection.Output;


            try
            {
                //  objConn.Open();
                objCmd.ExecuteNonQuery();

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
        public static double ExecuteProdecuceDb(string proName, string dbName, string where_fiel1, string where_fiel2, string where_fiel3)
        {
            var conn = new DbContext().Connect(dbName);

            // string spSql = "BEGIN STORED_PROC_NAME(:IN_PARAM, :OUT_PARAM1, :OUT_PARAM2); END; ";
            DataTable dt = new DataTable();
            DataSet dataset = new DataSet();

            double moQTY = 0;
            OracleCommand objCmd = new OracleCommand(proName, conn);


            objCmd.CommandText = proName;// "pkg_return_table.ProcReturnDiTable";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("where_condition_row1", OracleDbType.Varchar2).Value = where_fiel1;
            objCmd.Parameters.Add("where_condition_row2", OracleDbType.Varchar2).Value = where_fiel2;
            objCmd.Parameters.Add("where_condition_row3", OracleDbType.Varchar2).Value = where_fiel3;
            objCmd.Parameters.Add("o_remCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("moQty", OracleDbType.Double).Direction = ParameterDirection.Output;


            try
            {
                //  objConn.Open();
                objCmd.ExecuteNonQuery();

                string qtymo = objCmd.Parameters["moQty"].Value.ToString();


                moQTY = double.Parse(qtymo);
                /// OracleDataAdapter da = new OracleDataAdapter(objCmd);

                //da.Fill(dataset);

                return moQTY;//= dataset.Tables[0];
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception: {0}", ex.ToString());
                return moQTY = 0;
            }


        }
    }
}