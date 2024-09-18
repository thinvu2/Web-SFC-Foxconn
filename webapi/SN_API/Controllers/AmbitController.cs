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
    public class AmbitController : ApiController
    {
        [System.Web.Http.Route("GetDetailAmbit")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetDetailAmbit(AmbitElement model)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builder1 = new StringBuilder();
            bool checkNotLink = model.isNotlink;

            if (model.group_value == "ORT Wip")
            {
                string SQL = "SELECT SERIAL_NUMBER,SECTION_FLAG,MO_NUMBER,MODEL_NAME,TYPE,VERSION_CODE,LINE_NAME,SECTION_NAME,GROUP_NAME,STATION_NAME,LOCATION,STATION_SEQ,ERROR_FLAG,TO_CHAR (IN_STATION_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_STATION_TIME,TO_CHAR (IN_LINE_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_LINE_TIME,OUT_LINE_TIME,SHIPPING_SN,WORK_FLAG,FINISH_FLAG,ENC_CNT,SPECIAL_ROUTE,PALLET_NO,CONTAINER_NO,QA_NO,QA_RESULT,SCRAP_FLAG,NEXT_STATION,CUSTOMER_NO,BOM_NO,BILL_NO,TRACK_NO,PO_NO,KEY_PART_NO,CARTON_NO,WARRANTY_DATE,REWORK_NO,REPAIR_CNT,EMP_NO,PO_LINE,PALLET_FULL_FLAG,PMCC,GROUP_NAME_CQC,MO_NUMBER_OLD,ERP_MO,ATE_STATION_NO,MSN,IMEI,JOB,MCARTON_NO,SO_NUMBER,SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,WIP_GROUP,SHIPPING_SN2"
                   + " FROM   SFISM4.R_WIP_TRACKING_T t " +
        "WHERE  MO_NUMBER = '" + model.mo_number + "' and wip_group ='STOCKIN'  AND serial_number IN(SELECT serial_number " +
        "                                FROM   sfism4.R_ORT_WIP " +
        "                                WHERE  out_group is null) ";
                builder.Append(SQL);

                goto getToDataTable;
            }

            if (model.line_name != "")
            {
                builder1.Append($" AND LINE_NAME = '{model.line_name}' ");
            }
            var tableName = "SFISM4.R107";
            if (model.isFG)
            {
                tableName = "SFISM4.Z107";
            }
            if (checkNotLink)
            {
                string SQL = "SELECT SERIAL_NUMBER,SECTION_FLAG,MO_NUMBER,MODEL_NAME,TYPE,VERSION_CODE,LINE_NAME,SECTION_NAME,GROUP_NAME,STATION_NAME,LOCATION,STATION_SEQ,ERROR_FLAG,TO_CHAR (IN_STATION_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_STATION_TIME,TO_CHAR (IN_LINE_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_LINE_TIME,OUT_LINE_TIME,SHIPPING_SN,WORK_FLAG,FINISH_FLAG,ENC_CNT,SPECIAL_ROUTE,PALLET_NO,CONTAINER_NO,QA_NO,QA_RESULT,SCRAP_FLAG,NEXT_STATION,CUSTOMER_NO,BOM_NO,BILL_NO,TRACK_NO,PO_NO,KEY_PART_NO,CARTON_NO,WARRANTY_DATE,REWORK_NO,REPAIR_CNT,EMP_NO,PO_LINE,PALLET_FULL_FLAG,PMCC,GROUP_NAME_CQC,MO_NUMBER_OLD,ERP_MO,ATE_STATION_NO,MSN,IMEI,JOB,MCARTON_NO,SO_NUMBER,SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,WIP_GROUP,SHIPPING_SN2"
                   + " FROM   SFISM4.R_WIP_TRACKING_T t " +
        "WHERE  MO_NUMBER = '" + model.mo_number + "' AND MO_NUMBER NOT LIKE'00%' AND WIP_GROUP IN('STOCKIN','Linked')  AND serial_number NOT IN(SELECT key_part_sn " +
        "                                FROM   sfism4.r108 " +
        "                                WHERE  key_part_sn = t.serial_number " +
        "                                UNION " +
        "                                SELECT key_part_sn " +
        "                                FROM   sfism4.h108 " +
        "                                WHERE  key_part_sn = t.serial_number) ";
                builder.Append(SQL);
            }
            else
            {
                string SQL = "SELECT SERIAL_NUMBER,MODEL_NAME,MO_NUMBER, " +
        "       VERSION_CODE, " +
        "       PALLET_NO, " +
        "       QA_NO, " +
        "       CARTON_NO, " +
        "          TO_CHAR (IN_STATION_TIME, 'yyyy/mm/dd hh24:mi:ss  ') IN_STATION_TIME," +
        " ROUND(TO_NUMBER(SYSDATE - in_STATION_TIME)*24,1) \"HOUR\"," +
        "       MCARTON_NO, " +
        "       IMEI, " +
        "       SHIPPING_SN, " +
        "       SHIPPING_SN2, " +
        "       ERROR_FLAG, " +
        "       MO_NUMBER_OLD " +
        $"  FROM {tableName} " +
        " WHERE     MO_NUMBER = '" + model.mo_number + "' " +
        "       AND WIP_GROUP = '" + model.group_value + "' ";

                builder.Append(SQL);
                builder.Append(builder1);
            }

        getToDataTable:

            string queryString = builder.ToString();

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
        public string GetSelectByList(List<Type> list, string type)
        {
            if (list == null) return "";
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
        [System.Web.Http.Route("GetAmbitElement")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetAmbitElement(AmbitElement model)
        {
            StringBuilder builder = new StringBuilder();

            if (model.type == "model")
            {
                builder.Append($"select distinct MODEL_NAME VALUE from sfism4.R105 where 1=1 ");
                if (model.motype_value != "ALL")
                {
                    builder.Append($" and upper(mo_type) = '{model.motype_value.ToUpper()}'");
                }
                if (model.mo_value != null)
                {
                    builder.Append(GetSelectByList(model.mo_value, "MO_NUMBER"));
                }
            }
            else if (model.type == "mo")
            {
                builder.Append($"select  MO_NUMBER VALUE from sfism4.R105 where 1=1 ");
                if (model.mo_number != "")
                {
                    builder.Append($" and mo_number like '%{model.mo_number}%'");
                }
                if (model.motype_value != "ALL")
                {
                    builder.Append($" and upper(mo_type) = '{model.motype_value.ToUpper()}'");
                }
                if (model.model_value != null)
                {
                    builder.Append(GetSelectByList(model.model_value, "MODEL_NAME"));
                }
            }
            else if (model.type == "section")
            {
                builder.Append($"SELECT distinct SECTION_NAME VALUE FROM SFIS1.C_SECTION_CONFIG_T where upper(section_name) like '%{model.section_value.ToUpper()}%' GROUP BY SECTION_NAME  ORDER BY SECTION_NAME");
            }
            else if (model.type == "wip")
            {
                string sectionString = "";

                string modelString = "";
                string moString = "";
                string moState = "";
                var tableName = "SFISM4.R107";
                if (model.isFG)
                {
                    tableName = "SFISM4.Z107";
                }

                if (model.mo_state.ToUpper() == "ALL")
                {
                    moState = " 1 = 1 ";
                }
                else if (model.mo_state == "online")
                {
                    moState = " CLOSE_FLAG in (2) ";
                }
                else
                {
                    moState = " CLOSE_FLAG in (3) ";
                }
                if (model.section_value.Trim().ToUpper() != "ALL")
                {
                    sectionString = $" AND SECTION_NAME = '{model.section_value.Trim().ToUpper()}' ";
                }
                if (model.model_value != null)
                {
                    modelString = GetSelectByList(model.model_value, "MODEL_NAME");
                    if (model.mo_value.Count != 0)
                    {
                        moString = GetSelectByList(model.mo_value, "MO_NUMBER");
                    }
                    else
                    {
                        if (model.motype_value.Trim().ToUpper() != "ALL")
                        {
                            moString = $" AND MO_NUMBER IN (select MO_NUMBER from sfism4.r105 where 1=1 " + GetSelectByList(model.model_value, "MODEL_NAME") + $" and upper(mo_type) = '{model.motype_value.Trim().ToUpper()}') ";
                        }
                        else
                        {
                            moString = $" AND MO_NUMBER IN (select MO_NUMBER from sfism4.r105 where 1=1 " + GetSelectByList(model.model_value, "MODEL_NAME") + ") ";
                        }
                    }
                }
                else
                {
                    moString = GetSelectByList(model.mo_value, "MO_NUMBER");
                }
                string queryGroupName = "";

                string moWip = "";
                if (model.mo_value != null)
                {
                    moWip = GetSelectByList(model.mo_value, "MO_NUMBER");
                }

                string sectionWip = "";
                if (model.section_value.ToUpper().Trim() != "ALL")
                {
                    sectionWip = $" AND SECTION_NAME = '{model.section_value.ToUpper().Trim()}'";
                }

                string modelWip = "";
                if (model.model_value != null)
                {
                    modelWip = GetSelectByList(model.model_value, "MODEL_NAME");
                }

                string modelTypeWip = "";
                if (model.motype_value.ToUpper().Trim() != "ALL")
                {
                    modelTypeWip = $" AND UPPER(MO_TYPE) = '{model.motype_value.ToUpper().Trim()}'";
                }



                queryGroupName = $"Select  DISTINCT A.WIP_GROUP from {tableName} A WHERE 1=1 {moWip} AND MO_NUMBER IN (SELECT DISTINCT MO_NUMBER FROM SFISM4.R105 WHERE 1=1  {modelWip} {modelTypeWip})    {sectionString}   AND WIP_GROUP IS NOT NULL AND WIP_GROUP <>'N/A' ";

                DataTable dtGroupName = DBConnect.GetData(queryGroupName, model.database_name);
                if (dtGroupName.Rows.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "no record" });
                }
                else
                {
                    int countOBA = 0;
                    string SQL = "";
                    string c_line = "";
                    if (model.show_line)
                    {
                        c_line = "A.LINE_NAME,";
                    }
                    SQL += "SELECT A.MO_NUMBER,NVL(A.WIP_GROUP,'N/A') WIP_GROUP, COUNT(A.WIP_GROUP) TOTAL "
            + $"       , {c_line} A.MODEL_NAME,B.VERSION_CODE,B.TA,B.TARGET_QTY,B.DAY ";

                    if (model.isNotlink)
                    {
                        if (model.database_name.ToUpper() == "SFO") //sua thanh SFO
                        {
                            SQL += " , (SELECT COUNT(*) " +
                            "FROM   SFISM4.R_WIP_TRACKING_T t " +
                            "WHERE  MO_NUMBER=A.MO_NUMBER AND MO_NUMBER NOT LIKE'00%' AND WIP_GROUP IN ('STOCKIN','Linked') AND serial_number NOT IN(SELECT key_part_sn " +
                            "                                FROM   sfism4.r108 " +
                            "                                WHERE  key_part_sn = t.serial_number " +
                            "                                UNION " +
                            "                                SELECT key_part_sn " +
                            "                                FROM   SFISM4.H_WIP_KEYPARTS_T1 " +
                            "                                WHERE  key_part_sn = t.serial_number" +
                            "                                UNION " +
                            "                                SELECT key_part_sn " +
                            "                                FROM   sfism4.h108 " +
                            "                                WHERE  key_part_sn = t.serial_number)) NOTLINK_QTY ";
                        }
                        else
                        {
                            SQL += " , (SELECT COUNT(*) " +
                            "FROM   SFISM4.R_WIP_TRACKING_T t " +
                            "WHERE  MO_NUMBER=A.MO_NUMBER AND MO_NUMBER NOT LIKE'00%' AND WIP_GROUP IN ('STOCKIN','Linked') AND serial_number NOT IN(SELECT key_part_sn " +
                            "                                FROM   sfism4.r108 " +
                            "                                WHERE  key_part_sn = t.serial_number " +
                            "                                UNION " +
                            "                                SELECT key_part_sn " +
                            "                                FROM   sfism4.h108 " +
                            "                                WHERE  key_part_sn = t.serial_number)) NOTLINK_QTY ";
                        }
                    }
                    if (model.database_name.ToUpper() == "SFO") //sua thanh SFO
                    {
                        if (model.isORTWip)
                        {
                            SQL += ",(SELECT COUNT (*) " +
       "            FROM SFISM4.R_ORT_WIP " +
       "           WHERE SERIAL_NUMBER IN (SELECT SERIAL_NUMBER FROM SFISM4.R107 WHERE WIP_GROUP ='STOCKIN' AND MO_NUMBER = A.MO_NUMBER) AND OUT_GROUP IS NULL) ORT_WIP ";
                        }

                    }

                    SQL += $"FROM ((select MO_NUMBER,WIP_GROUP,LINE_NAME,MODEL_NAME from {tableName} "
            + $"      WHERE 1 = 1 {sectionString} {modelString} {moString} "
            //+ sLineSQL
            + "  )) A "
            + "     ,(select TARGET_QTY,MO_NUMBER,VERSION_CODE,VENDER_PART_NO TA,CLOSE_FLAG,ROUND(SYSDATE-MO_START_DATE) DAY from SFISM4.R_MO_BASE_T "
            + $"       WHERE {moState} {modelString} {moString}"
            + ") B "
            + "WHERE A.MO_NUMBER = B.MO_NUMBER "
            + $"GROUP BY A.MO_NUMBER, A.WIP_GROUP ,  {c_line}"
            + "      A.MODEL_NAME,"
            + " B.VERSION_CODE,B.TA,B.TARGET_QTY,B.day ";
                    if (model.model_value != null)
                    {
                        string queryMO = $"SELECT * FROM SFISM4.R105 WHERE 1=1 {GetSelectByList(model.mo_value, "MO_NUMBER")} ";
                        DataTable dtMO = DBConnect.GetData(queryMO, model.database_name);
                        if (dtMO.Rows[0]["MO_TYPE"].ToString() == "Normal")
                        {
                            string sql3 = " UNION ALL SELECT A.MO_NUMBER, NVL ('OBA', 'N/A') WIP_GROUP,COUNT(*) TOTAL, "
             + $" {c_line}"
              + " A.MODEL_NAME,B.VERSION_CODE,B.VENDER_PART_NO TA, B.TARGET_QTY ,ROUND (SYSDATE - B.MO_START_DATE) \"DAY\" ";

                            if (model.isNotlink)
                            {
                                if (model.database_name.ToUpper() == "SFO") //sua thanh SFO
                                {
                                    sql3 += " , (SELECT COUNT(*) " +
                                    "FROM   SFISM4.R_WIP_TRACKING_T t " +
                                    "WHERE  MO_NUMBER=A.MO_NUMBER AND MO_NUMBER NOT LIKE'00%' AND WIP_GROUP IN ('STOCKIN','Linked') AND serial_number NOT IN(SELECT key_part_sn " +
                                    "                                FROM   sfism4.r108 " +
                                    "                                WHERE  key_part_sn = t.serial_number " +
                                    "                                UNION " +
                                    "                                SELECT key_part_sn " +
                                    "                                FROM   SFISM4.H_WIP_KEYPARTS_T1 " +
                                    "                                WHERE  key_part_sn = t.serial_number" +
                                    "                                UNION " +
                                    "                                SELECT key_part_sn " +
                                    "                                FROM   sfism4.h108 " +
                                    "                                WHERE  key_part_sn = t.serial_number)) NOTLINK_QTY ";
                                }
                                else
                                {
                                    sql3 += " , (SELECT COUNT(*) " +
                                    "FROM   SFISM4.R_WIP_TRACKING_T t " +
                                    "WHERE  MO_NUMBER=A.MO_NUMBER AND MO_NUMBER NOT LIKE'00%' AND WIP_GROUP IN ('STOCKIN','Linked') AND serial_number NOT IN(SELECT key_part_sn " +
                                    "                                FROM   sfism4.r108 " +
                                    "                                WHERE  key_part_sn = t.serial_number " +
                                    "                                UNION " +
                                    "                                SELECT key_part_sn " +
                                    "                                FROM   sfism4.h108 " +
                                    "                                WHERE  key_part_sn = t.serial_number)) NOTLINK_QTY ";
                                }
                                // sql3 += " , (SELECT COUNT(*) " +
                                //"FROM   SFISM4.R_WIP_TRACKING_T t " +
                                //"WHERE MO_NUMBER=A.MO_NUMBER AND MO_NUMBER NOT LIKE'00%' AND WIP_GROUP IN ('STOCKIN','Linked') AND serial_number NOT IN(SELECT key_part_sn " +
                                //"                                FROM   sfism4.r108 " +
                                //"                                WHERE  key_part_sn = t.serial_number " +
                                //"                                UNION " +
                                //"                                SELECT key_part_sn " +
                                //"                                FROM   sfism4.h108 " +
                                //"                                WHERE  key_part_sn = t.serial_number)) NOTLINK_QTY ";
                            }
                            if (model.database_name.ToUpper() == "SFO")
                            {
                                if (model.isORTWip)
                                {
                                    sql3 += " , (SELECT COUNT (*) " +
                "            FROM SFISM4.R_ORT_WIP " +
                "           WHERE SERIAL_NUMBER IN (SELECT SERIAL_NUMBER FROM SFISM4.R107 WHERE WIP_GROUP ='STOCKIN' AND MO_NUMBER = A.MO_NUMBER) AND OUT_GROUP IS NULL) ORT_WIP ";
                                }
                            }
                            sql3 += $" FROM {tableName} A, SFISM4.R105 B WHERE A.MO_NUMBER=B.MO_NUMBER {GetSelectByList(model.mo_value, "A.MO_NUMBER")}"
                     + " AND A.WIP_GROUP = 'VI' "
                        + " AND A.GROUP_NAME NOT LIKE '%OBA%' "
                         + " AND A.SERIAL_NUMBER IN (SELECT SERIAL_NUMBER "
                                                 + "FROM SFISM4.R109 "
                                                + $" WHERE 1=1 {moString}) "
                                                   + $" GROUP BY A.MO_NUMBER,A.WIP_GROUP, {c_line}  ROUND(SYSDATE - B.MO_START_DATE), "
                                                   + " "
                                                   + " A.MODEL_NAME,B.TARGET_QTY,B.VERSION_CODE,B.VENDER_PART_NO  ";
                            SQL = SQL + sql3;
                            string stringR109 = "SELECT COUNT(*) TOTAL FROM SFISM4.R109 WHERE  (SERIAL_NUMBER,ROWID) IN ( "
               + "SELECT SERIAL_NUMBER,MAX(ROWID) FROM SFISM4.R109 WHERE  SERIAL_NUMBER IN ( "
               + $"SELECT SERIAL_NUMBER FROM {tableName} WHERE MO_NUMBER='{model.mo_value}' AND WIP_GROUP='VI' AND GROUP_NAME NOT LIKE '%OBA%')GROUP BY SERIAL_NUMBER)";
                            countOBA = int.Parse(DBConnect.GetData(stringR109, model.database_name).Rows[0]["TOTAL"].ToString());
                        }
                        else
                        {
                            string sql3 = " UNION "
                        + "SELECT A.MO_NUMBER,"
                      + "NVL ('OBA', 'N/A') WIP_GROUP,"
                       + "COUNT (A.WIP_GROUP) TOTAL,"
                      + c_line
                      + "A.MODEL_NAME, B.VERSION_CODE,B.TA,"
                      + "B.TARGET_QTY, "
                      + "B.DAY ";
                            if (model.isNotlink)
                            {
                                if (model.database_name.ToUpper() == "SFO") //sua thanh SFO
                                {
                                    sql3 += " , (SELECT COUNT(*) " +
                                    "FROM   SFISM4.R_WIP_TRACKING_T t " +
                                    "WHERE  MO_NUMBER=A.MO_NUMBER AND MO_NUMBER NOT LIKE'00%' AND WIP_GROUP IN ('STOCKIN','Linked') AND serial_number NOT IN(SELECT key_part_sn " +
                                    "                                FROM   sfism4.r108 " +
                                    "                                WHERE  key_part_sn = t.serial_number " +
                                    "                                UNION " +
                                    "                                SELECT key_part_sn " +
                                    "                                FROM   SFISM4.H_WIP_KEYPARTS_T1 " +
                                    "                                WHERE  key_part_sn = t.serial_number" +
                                    "                                UNION " +
                                    "                                SELECT key_part_sn " +
                                    "                                FROM   sfism4.h108 " +
                                    "                                WHERE  key_part_sn = t.serial_number)) NOTLINK_QTY ";
                                }
                                else
                                {
                                    sql3 += " , (SELECT COUNT(*) " +
                                    "FROM   SFISM4.R_WIP_TRACKING_T t " +
                                    "WHERE  MO_NUMBER=A.MO_NUMBER AND MO_NUMBER NOT LIKE'00%' AND WIP_GROUP IN ('STOCKIN','Linked') AND serial_number NOT IN(SELECT key_part_sn " +
                                    "                                FROM   sfism4.r108 " +
                                    "                                WHERE  key_part_sn = t.serial_number " +
                                    "                                UNION " +
                                    "                                SELECT key_part_sn " +
                                    "                                FROM   sfism4.h108 " +
                                    "                                WHERE  key_part_sn = t.serial_number)) NOTLINK_QTY ";
                                }
                                //sql3 += " ,  (SELECT COUNT(*) " +
                                //"FROM   SFISM4.R_WIP_TRACKING_T t " +
                                //"WHERE MO_NUMBER=A.MO_NUMBER AND MO_NUMBER NOT LIKE'00%' AND WIP_GROUP IN ('STOCKIN','Linked') AND  serial_number NOT IN(SELECT key_part_sn " +
                                //"                                FROM   sfism4.r108 " +
                                //"                                WHERE  key_part_sn = t.serial_number " +
                                //"                                UNION " +
                                //"                                SELECT key_part_sn " +
                                //"                                FROM   sfism4.h108 " +
                                //"                                WHERE  key_part_sn = t.serial_number)) NOTLINK_QTY ";
                            }
                            if (model.database_name.ToUpper() == "SFO")
                            {
                                if (model.isORTWip)
                                {
                                    sql3 += " , (SELECT COUNT (*) " +
               "            FROM SFISM4.R_ORT_WIP " +
               "           WHERE SERIAL_NUMBER IN (SELECT SERIAL_NUMBER FROM SFISM4.R107 WHERE WIP_GROUP ='STOCKIN' AND MO_NUMBER = A.MO_NUMBER) AND OUT_GROUP IS NULL) ORT_WIP ";
                                }
                            }
                            sql3 += "FROM ((SELECT MO_NUMBER,"
                            + " WIP_GROUP,"
                             + " LINE_NAME,"
                             + " MODEL_NAME,"
                             + " SERIAL_NUMBER"
                         + $" FROM {tableName} "
                        + " WHERE     1 = 1"
                        + $"  {modelString} {moString}"
                             + " AND WIP_GROUP='VI'"
                                + "AND GROUP_NAME NOT LIKE '%OBA%')) A ,"
                                + "  (SELECT MO_NUMBER,TARGET_QTY, VERSION_CODE,VENDER_PART_NO TA,"
                                + "ROUND(SYSDATE - MO_START_DATE) \"DAY\"  "
                          + " FROM SFISM4.R_MO_BASE_T "
                          + $"WHERE  {moState}  "
                      + $"  {modelString} {moString}"
                            + ") B"
                   + " WHERE A.MO_NUMBER = B.MO_NUMBER"
                   + " GROUP BY A.MO_NUMBER,"
                       + "A.WIP_GROUP,"
                       + "A.LINE_NAME,"
                       + " A.MODEL_NAME,B.VERSION_CODE,B.TA,"
                       + "B.TARGET_QTY,"
                       + "B.DAY";
                            SQL = SQL + sql3;
                            string stringR109 = $"SELECT COUNT(*) TOTAL FROM {tableName} WHERE 1=1  {GetSelectByList(model.mo_value, "MO_NUMBER")}  AND WIP_gROUP='VI' AND GROUP_NAME NOT LIKE '%OBA%'";
                            countOBA = int.Parse(DBConnect.GetData(stringR109, model.database_name).Rows[0]["TOTAL"].ToString());
                        }
                    }
                    else
                    {
                        string sql3 = " UNION ALL "
              + "SELECT A.MO_NUMBER, "
                     + "NVL (A.WIP_GROUP, 'N/A') WIP_GROUP, "
                      + "COUNT (A.WIP_GROUP) TOTAL, "
                      + c_line
                      + "A.MODEL_NAME, B.VERSION_CODE,B.TA, "
                      + "B.TARGET_QTY, "
                      + "B.DAY ";
                        if (model.isNotlink)
                        {
                            if (model.database_name.ToUpper() == "SFO") //sua thanh SFO
                            {
                                sql3 += " , (SELECT COUNT(*) " +
                                "FROM   SFISM4.R_WIP_TRACKING_T t " +
                                "WHERE  MO_NUMBER=A.MO_NUMBER AND MO_NUMBER NOT LIKE'00%' AND WIP_GROUP IN ('STOCKIN','Linked') AND serial_number NOT IN(SELECT key_part_sn " +
                                "                                FROM   sfism4.r108 " +
                                "                                WHERE  key_part_sn = t.serial_number " +
                                "                                UNION " +
                                "                                SELECT key_part_sn " +
                                "                                FROM   SFISM4.H_WIP_KEYPARTS_T1 " +
                                "                                WHERE  key_part_sn = t.serial_number" +
                                "                                UNION " +
                                "                                SELECT key_part_sn " +
                                "                                FROM   sfism4.h108 " +
                                "                                WHERE  key_part_sn = t.serial_number)) NOTLINK_QTY ";
                            }
                            else
                            {
                                sql3 += " , (SELECT COUNT(*) " +
                                "FROM   SFISM4.R_WIP_TRACKING_T t " +
                                "WHERE  MO_NUMBER=A.MO_NUMBER AND MO_NUMBER NOT LIKE'00%' AND WIP_GROUP IN ('STOCKIN','Linked') AND serial_number NOT IN(SELECT key_part_sn " +
                                "                                FROM   sfism4.r108 " +
                                "                                WHERE  key_part_sn = t.serial_number " +
                                "                                UNION " +
                                "                                SELECT key_part_sn " +
                                "                                FROM   sfism4.h108 " +
                                "                                WHERE  key_part_sn = t.serial_number)) NOTLINK_QTY ";
                            }
                            //sql3 += " , (SELECT COUNT(*) " +
                            //"FROM   SFISM4.R_WIP_TRACKING_T t " +
                            //"WHERE  MO_NUMBER=A.MO_NUMBER AND MO_NUMBER NOT LIKE'00%' AND WIP_GROUP IN ('STOCKIN','Linked') AND serial_number NOT IN(SELECT key_part_sn " +
                            //"                                FROM   sfism4.r108 " +
                            //"                                WHERE  key_part_sn = t.serial_number " +
                            //"                                UNION " +
                            //"                                SELECT key_part_sn " +
                            //"                                FROM   sfism4.h108 " +
                            //"                                WHERE  key_part_sn = t.serial_number)) NOTLINK_QTY ";
                        }
                        if (model.database_name.ToUpper() == "SFO")
                        {
                            if (model.isORTWip)
                            {
                                sql3 += " , (SELECT COUNT (*) " +
            "            FROM SFISM4.R_ORT_WIP " +
            "           WHERE SERIAL_NUMBER IN (SELECT SERIAL_NUMBER FROM SFISM4.R107 WHERE WIP_GROUP ='STOCKIN' AND MO_NUMBER = A.MO_NUMBER) AND OUT_GROUP IS NULL) ORT_WIP ";
                            }
                        }
                        sql3 += " FROM ((SELECT MO_NUMBER, WIP_GROUP,LINE_NAME, MODEL_NAME,GROUP_NAME,SERIAL_NUMBER "
                        + $"  FROM {tableName} "
                        + $" WHERE 1 = 1 {GetSelectByList(model.model_value, "MODEL_NAME")} AND MO_NUMBER <> '1')) A, "
                        + " (SELECT TARGET_QTY, VERSION_CODE ,VENDER_PART_NO TA, "
                                + " MO_NUMBER, "
                                + " CLOSE_FLAG, "
                                + " MO_TYPE, "
                                + " ROUND (SYSDATE - MO_START_DATE) DAY "
                            + "FROM SFISM4.R_MO_BASE_T "
                          + $" WHERE     {moState} "
                                + $" {GetSelectByList(model.model_value, "MODEL_NAME")} "
                                   + " AND MO_NUMBER <> '1') B "
                       + "   WHERE A.MO_NUMBER = B.MO_NUMBER AND B.MO_TYPE='Normal' AND A.WIP_GROUP='VI' AND A.GROUP_NAME NOT LIKE '%OBA%' AND A.SERIAL_NUMBER IN ( "
                          + $" SELECT SERIAL_NUMBER FROM SFISM4.R109 WHERE 1=1 {GetSelectByList(model.model_value, "MODEL_NAME")}) "
                          + "GROUP BY A.MO_NUMBER, "
                                  + " A.WIP_GROUP, "
                                  + c_line
                                  + " A.MODEL_NAME, B.VERSION_CODE,B.TA,"
                                  + " B.TARGET_QTY, "
                                   + "B.day "
                          + "UNION ALL "
                           + "SELECT A.MO_NUMBER, "
                                   + "NVL (A.WIP_GROUP, 'N/A') WIP_GROUP, "
                                    + "COUNT (A.WIP_GROUP) TOTAL, "
                                    + c_line
                                    + "A.MODEL_NAME,B.VERSION_CODE,B.TA, "
                                    + "B.TARGET_QTY, "
                                    + "B.DAY "
                               + "FROM ((SELECT MO_NUMBER, WIP_GROUP,LINE_NAME, MODEL_NAME,GROUP_NAME,SERIAL_NUMBER "
                                       + $" FROM {tableName} "
                                       + $" WHERE 1 = 1 AND {GetSelectByList(model.model_value, "MODEL_NAME")} AND MO_NUMBER <> '1')) A, "
                                       + "(SELECT TARGET_QTY, VERSION_CODE ,VENDER_PART_NO TA,"
                                              + " MO_NUMBER, "
                                               + "CLOSE_FLAG, "
                                               + "MO_TYPE, "
                                               + "ROUND (SYSDATE - MO_START_DATE) DAY "
                                          + "FROM SFISM4.R_MO_BASE_T "
                                         + $"WHERE 1=1     {moState} "
                                              + $" {GetSelectByList(model.model_value, "MODEL_NAME")} "
                                                 + " AND MO_NUMBER <> '1') B "
                                     + "WHERE A.MO_NUMBER = B.MO_NUMBER AND B.MO_TYPE <>'Normal' AND A.WIP_GROUP='VI' AND A.GROUP_NAME NOT LIKE '%OBA%' "
                                     + " GROUP BY A.MO_NUMBER, "
                                              + "A.WIP_GROUP, "
                                              + c_line
                                              + "A.MODEL_NAME, B.VERSION_CODE,B.TA,"
                                              + "B.TARGET_QTY, "
                                              + "B.day         ";
                        SQL = SQL + sql3;
                    }
                    string column1 = "";
                    string column2 = "";
                    if (model.sort_by == "mo")
                    {
                        SQL = SQL + " ORDER BY MO_NUMBER,MODEL_NAME ";
                        column1 = "MO";
                        column2 = "MODEL";
                    }
                    else
                    {
                        SQL = SQL + " ORDER BY MODEL_NAME, MO_NUMBER ";
                        column1 = "MODEL";
                        column2 = "MO";
                    }
                    DataTable dtR107 = DBConnect.GetData(SQL, model.database_name);

                    DataTable mytable = new DataTable();
                    mytable.Columns.Add(column1);
                    mytable.Columns.Add(column2);
                    mytable.Columns.Add("MO QTY");
                    mytable.Columns.Add("VERSION");
                    mytable.Columns.Add("TA");
                    if (model.show_line)
                    {
                        mytable.Columns.Add("LINE");
                    }
                    mytable.Columns.Add("ON_LINE");
                    for (int i = 0; i < dtGroupName.Rows.Count; i++)
                    {
                        mytable.Columns.Add(dtGroupName.Rows[i]["WIP_GROUP"].ToString());
                    }
                    mytable.Columns.Add("Total");
                    mytable.Columns.Add("SubTotal");
                    mytable.Columns.Add("Not input QTY");
                    if (model.isNotlink)
                    {
                        mytable.Columns.Add("Not link QTY");
                    }
                    if (model.isORTWip)
                    {
                        mytable.Columns.Add("ORT Wip");
                    }


                    int[] groupList = new int[dtGroupName.Rows.Count];
                    int totalSum = 0;
                    string lastMemory = "";
                    int? totalLastMemory = 0;
                    int? subTotalAll = 0;
                    int? notInputAll = 0;
                    for (int i = 0; i < dtR107.Rows.Count; i++)
                    {

                        DataRow dtRow = mytable.NewRow();
                        mytable.Rows.Add(dtRow);
                        if (model.sort_by == "mo")
                        {
                            mytable.Rows[i][0] = dtR107.Rows[i]["MO_NUMBER"].ToString();
                            mytable.Rows[i][1] = dtR107.Rows[i]["MODEL_NAME"].ToString();
                        }
                        else
                        {
                            mytable.Rows[i][0] = dtR107.Rows[i]["MODEL_NAME"].ToString();
                            mytable.Rows[i][1] = dtR107.Rows[i]["MO_NUMBER"].ToString();
                        }
                        mytable.Rows[i][2] = dtR107.Rows[i]["TARGET_QTY"].ToString();
                        mytable.Rows[i][3] = dtR107.Rows[i]["VERSION_CODE"].ToString();
                        mytable.Rows[i][4] = dtR107.Rows[i]["TA"].ToString();
                        if (model.show_line)
                        {
                            mytable.Rows[i][5] = dtR107.Rows[i]["LINE_NAME"].ToString();
                            mytable.Rows[i][6] = dtR107.Rows[i]["DAY"].ToString();
                        }
                        else
                        {
                            mytable.Rows[i][5] = dtR107.Rows[i]["DAY"].ToString();
                        }
                        int number = 0;
                        if (model.show_line)
                        {
                            number = 7;
                        }
                        else
                        {
                            number = 6;
                        }

                        int totalQTY = 0;


                        for (int j = 0; j < dtGroupName.Rows.Count; j++)
                        {
                            if (dtR107.Rows[i]["WIP_GROUP"].ToString() == mytable.Columns[number + j].ColumnName)
                            {
                                groupList[j] += int.Parse(dtR107.Rows[i]["TOTAL"].ToString());
                                //totalQTY += groupList[j];
                                mytable.Rows[i][number + j] = int.Parse(dtR107.Rows[i]["TOTAL"].ToString());
                            }
                            else
                            {
                                mytable.Rows[i][number + j] = 0;
                            }
                            try
                            {
                                totalQTY += int.Parse(mytable.Rows[i][number + j].ToString());
                            }
                            catch
                            {

                            }
                        }

                        if (i == 0)
                        {
                            lastMemory = mytable.Rows[i][0].ToString();
                        }
                        else
                        {
                            if (lastMemory != mytable.Rows[i][0].ToString())
                            {
                                mytable.Rows[i - 1]["SubTotal"] = totalLastMemory;
                                subTotalAll += totalLastMemory;

                                //mytable.Rows[i - 1]["Not link QTY"] = 1997;
                                mytable.Rows[i - 1]["Not input QTY"] = ToNumber.ToNullableInt(mytable.Rows[i - 1]["MO QTY"].ToString()) - totalLastMemory;
                                notInputAll += ToNumber.ToNullableInt(mytable.Rows[i - 1]["MO QTY"].ToString()) - totalLastMemory;
                                lastMemory = mytable.Rows[i][0].ToString();
                                totalLastMemory = 0;
                            }
                        }
                        mytable.Rows[i]["Total"] = totalQTY;
                        totalLastMemory += totalQTY;

                        totalSum += totalQTY;
                        if (i == dtR107.Rows.Count - 1)
                        {
                            DataRow dtRowLast = mytable.NewRow();
                            mytable.Rows.Add(dtRowLast);
                            for (int j = 0; j < dtGroupName.Rows.Count; j++)
                            {
                                mytable.Rows[i + 1][number + j] = groupList[j];
                            }
                            mytable.Rows[i + 1][0] = "Total";

                            mytable.Rows[i]["SubTotal"] = totalLastMemory;
                            subTotalAll += totalLastMemory;


                            mytable.Rows[i]["Not input QTY"] = ToNumber.ToNullableInt(mytable.Rows[i]["MO QTY"].ToString()) - totalLastMemory;
                            notInputAll += ToNumber.ToNullableInt(mytable.Rows[i]["MO QTY"].ToString()) - totalLastMemory;

                            mytable.Rows[i + 1]["Total"] = totalSum;

                            mytable.Rows[i + 1]["SubTotal"] = subTotalAll;

                            mytable.Rows[i + 1]["Not input QTY"] = notInputAll;
                        }
                        mytable.Rows[i]["Total"] = totalQTY;

                    }
                    if (!model.show_line)
                    {
                        mytable = mytable.AsEnumerable()
                .GroupBy(r => new { MO = r["MO"], MODEL = r["MODEL"], MO_QTY = r["MO QTY"], VERSION = r["VERSION"], TA = r["TA"], ONLINE = r["ON_LINE"] })
                .Select(g =>
                {
                    var row = mytable.NewRow();
                    if (model.sort_by == "mo")
                    {
                        row["MO"] = g.Key.MO;
                        row["MODEL"] = g.Key.MODEL;
                    }
                    else
                    {
                        row["MODEL"] = g.Key.MODEL;
                        row["MO"] = g.Key.MO;
                    }
                    row["MO QTY"] = g.Key.MO_QTY;
                    row["VERSION"] = g.Key.VERSION;
                    row["TA"] = g.Key.TA;
                    row["ON_LINE"] = g.Key.ONLINE;
                    for (int i = 0; i < dtGroupName.Rows.Count; i++)
                    {
                        row[dtGroupName.Rows[i]["WIP_GROUP"].ToString()] = g.Sum(r => ToNumber.ToNullableInt(r.Field<String>(dtGroupName.Rows[i]["WIP_GROUP"].ToString())));
                    }
                    return row;
                }).CopyToDataTable();
                        int? totalNotInput = 0;
                        int? totalNotLink = 0;
                        int? totalORT = 0;
                        for (int i = 0; i < mytable.Rows.Count; i++)
                        {
                            int? sum = 0;
                            int? notInput = 0;
                            for (int j = 0; j < dtGroupName.Rows.Count; j++)
                            {
                                sum += ToNumber.ToNullableInt(mytable.Rows[i][6 + j].ToString());
                            }
                            mytable.Rows[i]["Total"] = sum;
                            mytable.Rows[i]["SubTotal"] = sum;
                            if (i == mytable.Rows.Count - 1)
                            {
                                mytable.Rows[i]["Not input QTY"] = totalNotInput;
                            }
                            else
                            {
                                notInput = ToNumber.ToNullableInt(mytable.Rows[i]["MO QTY"].ToString()) - sum;
                                totalNotInput += notInput;
                                mytable.Rows[i]["Not input QTY"] = notInput;
                            }

                            if (model.isNotlink)
                            {
                                string expression = $"MO_NUMBER = '{mytable.Rows[i]["MO"]}'"; //Add not link QTY for mr Quy 
                                if (dtR107.Select(expression).Count() != 0)
                                {
                                    DataRow ass = dtR107.Select(expression)[0];
                                    mytable.Rows[i]["Not link QTY"] = ass["NOTLINK_QTY"];
                                    totalNotLink += ToNumber.ToNullableInt(ass["NOTLINK_QTY"].ToString());
                                }
                            }
                            if (model.isORTWip)
                            {
                                string expression = $"MO_NUMBER = '{mytable.Rows[i]["MO"]}'"; //Ort wip for mr Quy 
                                if (dtR107.Select(expression).Count() != 0)
                                {
                                    DataRow ass = dtR107.Select(expression)[0];
                                    mytable.Rows[i]["ORT Wip"] = ass["ORT_WIP"];
                                    totalORT += ToNumber.ToNullableInt(ass["ORT_WIP"].ToString());
                                }
                            }
                        }
                        if (model.isNotlink)
                        {
                            mytable.Rows[mytable.Rows.Count - 1]["Not link QTY"] = totalNotLink;
                        }
                        if (model.isORTWip)
                        {
                            mytable.Rows[mytable.Rows.Count - 1]["ORT Wip"] = totalORT;
                        }
                    }
                    else
                    {
                        mytable = mytable.AsEnumerable()
                .GroupBy(r => new { MO = r["MO"], MODEL = r["MODEL"], MO_QTY = r["MO QTY"], VERSION = r["VERSION"], TA = r["TA"], LINE = r["LINE"], ONLINE = r["ON_LINE"] })
                .Select(g =>
                {
                    var row = mytable.NewRow();
                    if (model.sort_by == "mo")
                    {
                        row["MO"] = g.Key.MO;
                        row["MODEL"] = g.Key.MODEL;
                    }
                    else
                    {
                        row["MODEL"] = g.Key.MODEL;
                        row["MO"] = g.Key.MO;
                    }
                    row["MO QTY"] = g.Key.MO_QTY;
                    row["VERSION"] = g.Key.VERSION;
                    row["TA"] = g.Key.TA;
                    row["LINE"] = g.Key.LINE;
                    row["ON_LINE"] = g.Key.ONLINE;
                    for (int i = 0; i < dtGroupName.Rows.Count; i++)
                    {
                        row[dtGroupName.Rows[i]["WIP_GROUP"].ToString()] = g.Sum(r => ToNumber.ToNullableInt(r.Field<String>(dtGroupName.Rows[i]["WIP_GROUP"].ToString())));
                    }
                    return row;
                }).CopyToDataTable();
                        int totalQTY = 0;
                        totalLastMemory = 0;
                        subTotalAll = 0;
                        notInputAll = 0;
                        for (int i = 0; i < mytable.Rows.Count; i++)
                        {
                            int number = 7;
                            int? totalOfRow = 0;
                            for (int j = 0; j < dtGroupName.Rows.Count; j++)
                            {
                                totalOfRow += ToNumber.ToNullableInt(mytable.Rows[i][number + j].ToString());
                            }
                            mytable.Rows[i]["Total"] = totalOfRow;


                            if (i == 0)
                            {
                                lastMemory = mytable.Rows[i][0].ToString();
                            }
                            else
                            {
                                if (lastMemory != mytable.Rows[i][0].ToString())
                                {
                                    mytable.Rows[i - 1]["SubTotal"] = totalLastMemory;
                                    subTotalAll += totalLastMemory;
                                    mytable.Rows[i - 1]["Not input QTY"] = ToNumber.ToNullableInt(mytable.Rows[i - 1]["MO QTY"].ToString()) - totalLastMemory; ;
                                    notInputAll += ToNumber.ToNullableInt(mytable.Rows[i - 1]["MO QTY"].ToString()) - totalLastMemory;
                                    lastMemory = mytable.Rows[i][0].ToString();
                                    totalLastMemory = 0;
                                }
                            }
                            //mytable.Rows[i]["Total"] = totalQTY;
                            totalLastMemory += totalOfRow;

                            totalSum += totalQTY;

                            if (i == mytable.Rows.Count - 1)
                            {
                                mytable.Rows[i]["SubTotal"] = subTotalAll;
                                mytable.Rows[i]["Not input QTY"] = notInputAll;
                            }
                        }
                    }
                    int totalMO = mytable.AsEnumerable().Select(r => r.Field<string>("MO")).Distinct().Count() - 1;
                    int totalModel = mytable.AsEnumerable().Select(r => r.Field<string>("MODEL")).Distinct().Count() - 1;
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = mytable, moQTY = totalMO, modelQTY = totalModel, queryTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") });
                }
            }
            builder.Append(" ");
            string queryString = $" SELECT * FROM ( {builder} ) ";

            if (model.type == "mo")
            {
                queryString = $"  {builder} and rownum < 50 order by mo_first_pcs_date desc  ";
            }
            DataTable dtCheck = DBConnect.GetData(queryString, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }
    }
}