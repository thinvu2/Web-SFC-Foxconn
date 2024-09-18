using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SN_API.Models;
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
using Type = SN_API.Models.Type;

namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RepairSearchController : ApiController
    {
        public string GetSelectByList(List<Type> list, string type)
        {
            StringBuilder sb = new StringBuilder();
            if (list.Count > 0)
            {
                var sbmodel_name = new StringBuilder();
                sbmodel_name.Append(" IN (");

                for (int i = 0; i < list.Count; i++)
                {
                    string mo = list[i].VALUE.ToString();

                    sbmodel_name.AppendFormat("'{0}'", mo);
                    if (i < list.Count - 1)
                    {
                        sbmodel_name.Append(",");
                    }
                }
                sbmodel_name.Append(")");
                sb.Append($" AND {type} {sbmodel_name} ");
            }
            return sb.ToString();
        }
        // GET: RepairSearch
        [System.Web.Http.Route("GetRepairElement")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetRepairElement(RepairElement model)
        {
            //if (model.listModel.Count == 0 && model.listMo.Count == 0 && model.listGroup.Count == 0 && model.listMo.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            StringBuilder sb = new StringBuilder();

            if (model.type == "model")
            {
                sb.Append($" select distinct model_name value from sfism4.R109 where test_time between TO_DATE('{model.dateFrom} {model.timeFrom}', 'YYYY/MM/DD HH24:MI') and TO_DATE('{model.dateTo} {model.timeTo}', 'YYYY/MM/DD HH24:MI') ORDER BY MODEL_NAME ASC");
            }
            else
            {
                sb.Append($" select distinct TEST_GROUP value from sfism4.R109 where test_time between TO_DATE('{model.dateFrom} {model.timeFrom}', 'YYYY/MM/DD HH24:MI') and TO_DATE('{model.dateTo} {model.timeTo}', 'YYYY/MM/DD HH24:MI') ORDER BY TEST_GROUP ASC");
            }

            string queryString = sb.ToString();
            DataTable dtCheck = DBConnect.GetData(queryString, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }
        [System.Web.Http.Route("GetRepair")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetRepair(RepairObject obj)
        {           
            string _string_group_list = GetSelectByList(obj.group_list, " TEST_GROUP");            
            string _string_model_list = GetSelectByList(obj.model_list, " a.MODEL_NAME");
            string po_list = GetSelectByList(obj.model_list, "P_NO");
            string _query_string = "";

            string _sub_query_string = "";

            if (obj.option == "Query not repair")
            {

            }

            if (obj.option == "ALL")
            {
                //tat ca
                _query_string = "SELECT A.SERIAL_NUMBER, " +
       "         C.PANEL_NO, " +
       "         A.MO_NUMBER, " +
       "         A.MODEL_NAME, " +
       "         A.TEST_TIME, " +
       "         A.TEST_CODE, " +
       "         A.TEST_STATION, " +
       "         A.TEST_GROUP, " +
       "         A.TEST_LINE, " +
       "         A.REPAIRER, " +
       "         A.REPAIR_TIME, " +
       "         A.REASON_CODE, " +
       "         A.ERROR_ITEM_CODE, " +
       "         A.LOCATION_CODE, " +
       "         A.ATE_STATION_NO, A.SUPPLIER, A.DATE_CODE, A.DATECODE,B.ERROR_DESC,B.ERROR_DESC2 " +
       "    FROM SFISM4.R109 A, SFIS1.C_ERROR_CODE_T B,SFISM4.R_SN_TRSN_LINK_T C WHERE A.TEST_CODE = B.ERROR_CODE (+) AND A.SERIAL_NUMBER = C.SERIAL_NUMBER (+) AND TO_CHAR (A.REPAIR_TIME, 'YYYY/MM/DD HH24:MI:SS') BETWEEN '" + obj.date_from + "' " +
       "                                                                          AND '" + obj.date_to + "' " +
       "                                                                          " + _string_group_list + "  " +
       "                   " + _string_model_list + " " +
       "                   ORDER BY A.MODEL_NAME,A.SERIAL_NUMBER,A.REASON_CODE ASC  ";
                _sub_query_string = "SELECT MODEL_NAME, COUNT (*) TOTAL " +
        "    FROM SFISM4.R109 " +
        "   WHERE TO_CHAR (TEST_TIME, 'YYYY/MM/DD HH24:MI:SS') BETWEEN '" + obj.date_from + "' " +
        "                                                          AND '" + obj.date_to + "' " + _string_group_list + " " +
         " " + _string_model_list + " " + "  GROUP BY MODEL_NAME order by total desc ";

            }else if (obj.option == "Querynotrepair")
            {
                _query_string = "select A.serial_number, A.mo_number,A.model_name,A.test_time,A.test_code,A.test_group,A.repairer,B.wip_group from sfism4.r_repair_t A, sfism4.r107 B " +
                 "where A.serial_number = B.serial_number and A.repairer is null and B.WIP_GROUP <> 'BC8M' AND B.WIP_GROUP <> 'BC9M' " +
                 "  and TO_CHAR(TEST_TIME,'YYYY/MM/DD HH24:MI:SS') between '" + obj.date_from + "'  AND  '" + obj.date_to + "' " +
                  "                                                                          " + _string_group_list + "  " +
                  "     " + _string_model_list ;
            }
            else if (obj.option == "Query_R")
            {
                _query_string = "SELECT A.*, DECODE (ERROR_FLAG, 1, 'Check in', 7, 'Repair', 8, 'Check out', 0, 'OK') STATUS FROM SFISM4.R_WIP_TRACKING_T A " +
                " WHERE MODEL_NAME IN (SELECT MODEL_NAME FROM SFIS1.C_MODEL_DESC_T WHERE MODEL_SERIAL = 'NIC') AND ERROR_FLAG IN ('1', '7', '8') " +
                "  and TO_CHAR(IN_STATION_TIME,'YYYY/MM/DD HH24:MI:SS') between '" + obj.date_from + "'  AND  '" + obj.date_to + "' " +
                 "     " + _string_model_list;
            }
            else if (obj.option == "QueryBonepile")
            {
                _query_string = "SELECT SN as serial_number,REPORT_DATE,BU,SERIES ,SN_TYPE,CHECKIN_DATE,CHECKOUT_DATE,FAILTIME,FIRST_FAILTIME,EDITDT,P_NO,FINAL_STATUS,ROUND(SN_AGILE,22) SN_AGILE  FROM SFISM4.R_REPAIR_DAILYREPORT_SN a " +
                " WHERE  replace( REPORT_DATE,'-','/') between substr('" + obj.date_from + "',0,10)  AND  substr( '" + obj.date_to + "',0,10) " +
                 "     " + po_list;
            }
            else
            {
                //fail
                _query_string
    = "SELECT * " +
        "  FROM (  SELECT A.SERIAL_NUMBER, " +
        "                 A.MO_NUMBER, " +
        "                 A.MODEL_NAME, " +
        "                 A.TEST_TIME, " +
        "                 A.TEST_CODE, " +
        "                 A.TEST_STATION, " +
        "                 A.TEST_GROUP, " +
        "                 A.TEST_SECTION, " +
        "                 A.TESTER, " +
        "                 A.TEST_LINE, " +
        "                 A.REPAIRER, " +
        "                 A.REPAIR_TIME, " +
        "                 A.REASON_CODE, " +
        "                 A.ERROR_ITEM_CODE,A.SUPPLIER,A.DATE_CODE,A.DATECODE,B.ERROR_DESC,B.ERROR_DESC2," +
        "                 ROW_NUMBER () " +
        "                 OVER (PARTITION BY A.SERIAL_NUMBER, A.TEST_CODE " +
        "                       ORDER BY A.TEST_TIME) " +
        "                    NUM " +
        "            FROM SFISM4.R109 A, SFIS1.C_ERROR_CODE_T B WHERE A.TEST_CODE = B.ERROR_CODE (+) AND (A.SERIAL_NUMBER, A.TEST_CODE) IN (  SELECT DISTINCT " +
        "                                                         SERIAL_NUMBER, TEST_CODE " +
        "                                                    FROM SFISM4.R109 " +
        "                                                   WHERE (SERIAL_NUMBER, " +
        "                                                          TEST_CODE) IN (SELECT DISTINCT " +
        "                                                                                SERIAL_NUMBER, " +
        "                                                                                TEST_CODE " +
        "                                                                           FROM SFISM4.r109 " +
        "                                                                          WHERE TO_CHAR ( " +
        "                                                                                   TEST_TIME, " +
        "                                                                                   'YYYY/MM/DD HH24:MI:SS') BETWEEN '" + obj.date_from + "' " +
        "                                                                                                                AND '" + obj.date_to + "') " +
        "                   " + _string_group_list + " " +
         "                   " + _string_model_list + " " +
        "                                                  HAVING COUNT (*) > 1 " +
        "                                                GROUP BY SERIAL_NUMBER, " +
        "                                                         TEST_CODE) " +
        "        ORDER BY A.SERIAL_NUMBER, NUM) " +
        " WHERE NUM = 1 ";
                _sub_query_string
     = "SELECT MODEL_NAME, COUNT (*) TOTAL " +
         "    FROM (  SELECT A.SERIAL_NUMBER, " +
         "                   A.MO_NUMBER, " +
         "                   A.MODEL_NAME, " +
         "                   A.TEST_TIME, " +
         "                   A.TEST_CODE, " +
         "                   A.TEST_STATION, " +
         "                   A.TEST_GROUP, " +
         "                   A.TEST_SECTION, " +
         "                   A.TESTER, " +
         "                   A.TEST_LINE, " +
         "                   A.REPAIRER, " +
         "                   A.REPAIR_TIME, " +
         "                   A.REASON_CODE, " +
         "                   A.ERROR_ITEM_CODE,A.SUPPLIER,A.DATE_CODE,A.DATECODE, " +
         "                   ROW_NUMBER () " +
         "                   OVER (PARTITION BY A.SERIAL_NUMBER, A.TEST_CODE " +
         "                         ORDER BY TEST_TIME) " +
         "                      NUM " +
         "              FROM SFISM4.R109 A., SFIS1.C_ERROR_CODE_T B WHERE A.TEST_CODE = B.ERROR_CODE (+) AND (A.SERIAL_NUMBER, A.TEST_CODE) IN (  SELECT DISTINCT " +
         "                                                           SERIAL_NUMBER, TEST_CODE " +
         "                                                      FROM SFISM4.R109 " +
         "                                                     WHERE (SERIAL_NUMBER, " +
         "                                                            TEST_CODE) IN (SELECT DISTINCT " +
         "                                                                                  SERIAL_NUMBER, " +
         "                                                                                  TEST_CODE " +
         "                                                                             FROM SFISM4.r109 " +
         "                                                                            WHERE TO_CHAR ( " +
         "                                                                                     TEST_TIME, " +
         "                                                                                     'YYYY/MM/DD HH24:MI:SS') BETWEEN '" + obj.date_from + "' " +
         "                                                                                                                  AND '" + obj.date_to + "') " +
         "                   " + _string_group_list + " " +
          "                   " + _string_model_list + " " +
         "                                                    HAVING COUNT (*) > 1 " +
         "                                                  GROUP BY SERIAL_NUMBER, " +
         "                                                           TEST_CODE) " +
         "          ORDER BY A.SERIAL_NUMBER, NUM) " +
         "   WHERE NUM = 1 " +
         "GROUP BY A.MODEL_NAME ";

            }

            DataTable dt = DBConnect.GetData(_query_string, obj.database);

            DataTable dt1 = DBConnect.GetData(_sub_query_string, obj.database);

            return Request.CreateResponse(new { message = "ok", data = dt, query = _query_string, data1 = dt1, query1 = _sub_query_string });

        }
    }
}