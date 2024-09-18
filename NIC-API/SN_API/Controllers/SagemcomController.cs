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
    public class SagemcomController : ApiController
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
        [System.Web.Http.Route("GetSagemcomElement")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetSagemcomElement(SagemcomElement model)
        {
            string timeFrom = TimeToWorkSection(model.timeFrom);
            string timeTo = TimeToWorkSection(model.timeTo);
            StringBuilder sb = new StringBuilder();
            if(model.type == "customermodel")
            {
                sb.Append($" select distinct H1CPO value from  SFISM4.R_EDI_AE940H where TIMESTAMP ='SAGEMCOM' and H1DVNO in ( select distinct model_name value from SFISM4.R_STATION_ATE_T where WORK_DATE || trim(to_char(Work_Section, '00')) >= REPLACE('{model.dateFrom}', '/','')||'{timeFrom}' and WORK_DATE || trim(to_char(Work_Section, '00')) < REPLACE('{model.dateTo}', '/','')||'{timeTo}' )");
            }
            else
            {
                sb.Append($" select distinct model_name value from SFISM4.R_STATION_ATE_T where  WORK_DATE || trim(to_char(Work_Section, '00')) >= REPLACE('{model.dateFrom}', '/','')||'{timeFrom}' and WORK_DATE || trim(to_char(Work_Section, '00')) < REPLACE('{model.dateTo}', '/','')||'{timeTo}' ORDER BY model_name ASC");
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
        public string TimeToWorkSection(string time)
        {
            if (time == "24:00")
            {
                return "25";
            }
            else
            {
                var workSection = time.Replace(':', '.');
                return Math.Ceiling(Double.Parse(workSection)).ToString().PadLeft(2, '0');
            }
        }
        [System.Web.Http.Route("GetSagemcom")]                                                                                                                                                                                                 
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetSagemcom(SagemcomObject obj)
        {
           
            string _string_customer_model= GetSelectByList(obj.group_model, "H1CPO");
            string _string_model_modelc = GetSelectByList(obj.group_model, "c.model_name");
            string timeFrom = TimeToWorkSection(obj.timeFrom);
            string timeTo = TimeToWorkSection(obj.timeTo);
            string sql = string.Empty;
            string _string_fox_model = string.Empty;
            DataTable dt;
            //if (_string_model_model == "")
            //{
            //    dt = null;
            //    return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail", data = dt});
            //}

            string sqlfox = " select H1DVNO from  SFISM4.R_EDI_AE940H where TIMESTAMP ='SAGEMCOM'" + _string_customer_model + "";
            dt = DBConnect.GetData(sqlfox, obj.database);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _string_fox_model = _string_fox_model+ "'"+dt.Rows[i]["H1DVNO"].ToString()+"'";
                if (i < dt.Rows.Count - 1)
                {
                    _string_fox_model = _string_fox_model + ",";
                }
            }

            if (obj.option == "SagemcomQuery")
            {
                sql = "SELECT ( select distinct H1CPO value from  SFISM4.R_EDI_AE940H where TIMESTAMP ='SAGEMCOM' and H1DVNO = model_name AND ROWNUM =1) AS model_name,group_name,total_qty,pass_qty,repass_qty,fail_qty,refail_qty,FPY_Rate,Yield_Rate," +
      " To_char((repass_qty / total_qty) * 100, '990.99') || '%' retestok_rate," +
      "  To_char(((fail_qty + refail_qty) / total_qty) * 100, '990.99') || '%' fail_rate" +
      "  FROM(" +
      "  SELECT r.model_name," +
      "         r.GROUP_NAME," +
      "         SUM(r.pass_qty)                   pass_qty," +
      "        SUM(r.fail_qty)                   fail_qty," +
      "        SUM(Nvl(R.FIRST_FAIL_QTY, 0))     FIRST_FAIL_QTY," +
      "        DECODE(SUM(R.PASS_QTY) + SUM(R.FAIL_QTY), 0, SUM(R.rePASS_QTY) + SUM(R.reFAIL_QTY), SUM(R.PASS_QTY) + SUM(R.FAIL_QTY)) Total_qty," +
      "        To_number(To_char(100 * (SUM(R.PASS_QTY) + SUM(R.FAIL_QTY) - SUM(Nvl(R.FIRST_FAIL_QTY, 0))) / Decode((SUM(R.PASS_QTY)), 0, 1, (SUM(R.PASS_QTY) + SUM(R.FAIL_QTY))), '9900.00')) || '%'                            FPY_Rate," +
      "        To_char(100 * SUM(R.PASS_QTY) / Decode((SUM(R.PASS_QTY) + SUM(R.FAIL_QTY)), 0, 1, (SUM(R.PASS_QTY) + SUM(R.FAIL_QTY))), '9900.00') || '%'  Yield_Rate," +
      "        SUM(Nvl(r.repass_qty, 0))         repass_qty, SUM(Nvl(r.refail_qty, 0))         refail_qty, t.num" +
      " FROM(SELECT  WORK_SECTION, MO_NUMBER, WORK_DATE, MODEL_NAME, LINE_NAME, SECTION_NAME, GROUP_NAME, REPASS_QTY, REFAIL_QTY, PASS_QTY, FIRST_FAIL_QTY, FAIL_QTY" +
      "        FROM   SFISM4.H_STATION_REC_T" +
      "        UNION ALL" +
      "        SELECT  WORK_SECTION, MO_NUMBER, WORK_DATE, MODEL_NAME, LINE_NAME, SECTION_NAME, GROUP_NAME, REPASS_QTY, REFAIL_QTY, PASS_QTY, FIRST_FAIL_QTY, FAIL_QTY" +
      "        FROM   SFISM4.R_STATION_REC_T) R," +
      "       SFIS1.C_GROUP_CONFIG_T C," +
      "       (SELECT group_next," +
      "               MIN(step_sequence) num" +
      "        FROM   sfis1.c_route_control_t" +
      "        WHERE  route_code IN(SELECT DISTINCT route_code" +
      "                             FROM   sfism4.r105" +
      "                             WHERE  mo_number IN(SELECT DISTINCT mo_number" +
      "                                                 FROM   sfism4.r_station_rec_t" +
      "                                                 WHERE  Replace(WORK_DATE || To_char(Work_Section, '00'), ' ', '') BETWEEN(REPLACE('" + obj.date_from + "', '/','')||'" + timeFrom + "' " + ") AND(REPLACE('" + obj.date_to + "', '/','')||'" + timeTo + "' " + ")" +
      "                                                 AND model_name IN(" + _string_fox_model + ")" +
      "                                                        ))" +
      "        GROUP  BY group_next" +
      "        ORDER  BY MIN(step_sequence))T" +
      " WHERE  Replace(R.WORK_DATE" +
      " || To_char(Work_Section, '00'), ' ', '') BETWEEN(REPLACE('" + obj.date_from + "', '/','')||'" + timeFrom + "' " + ") AND(REPLACE('" + obj.date_to + "', '/','')||'" + timeTo + "' " + ")" +
      " AND model_name IN(" + _string_fox_model + ")" +
      "        AND R.GROUP_NAME = C.GROUP_NAME" +
      "        AND r.section_Name = c.section_name" +
      "        AND C.GROUP_NAME NOT IN(SELECT GROUP_NAME" +
      "                                FROM   SFIS1.C_GROUP_NOROUTE_T)" +
      "        AND R.GROUP_name = t.group_next" +
      "  AND substr(R.GROUP_name, 1, 2) not in('PA', 'CH', 'FQ', 'OB', 'AS', 'LA') " +
      "        AND r.SECTION_NAME = 'TEST'" +
      "        and r.line_name <> 'REPAIR'" +
      " GROUP BY R.model_name," +
      "           r.group_name," +
      "           t.num" +
      "           ORDER  BY t.num)";
            }
            else
            {
                sql = string.Format(@"WITH R_STATION as
 (SELECT  WORK_SECTION,
                       MO_NUMBER,
                       WORK_DATE,
                       MODEL_NAME,
                       LINE_NAME,
                       SECTION_NAME,
                       GROUP_NAME,
                       REPASS_QTY,
                       REFAIL_QTY,
                       PASS_QTY,
                       FIRST_FAIL_QTY,
                       FAIL_QTY
              FROM   SFISM4.H_STATION_REC_T
              UNION ALL
              SELECT  WORK_SECTION,
                       MO_NUMBER,
                       WORK_DATE,
                       MODEL_NAME,
                       LINE_NAME,
                       SECTION_NAME,
                       GROUP_NAME,
                       REPASS_QTY,
                       REFAIL_QTY,
                       PASS_QTY,
                       FIRST_FAIL_QTY,
                       FAIL_QTY
                       FROM   SFISM4.R_STATION_REC_T),
 C_GROUP as
    (select* from   SFIS1.C_GROUP_CONFIG_T  ),
 group_nex as
    (SELECT group_next,
                     MIN(step_sequence)num
              FROM sfis1.c_route_control_t
            WHERE  route_code IN(SELECT DISTINCT route_code
                                 FROM   sfism4.r105
                                 WHERE  mo_number IN(SELECT DISTINCT mo_number

                                                     FROM sfism4.r_station_rec_t
                                                     WHERE  Replace(WORK_DATE
                                                                      || To_char(Work_Section, '00'), ' ', '') BETWEEN(Replace('{0}', '/', '')
                                                                        || '{2}') AND(Replace('{1}', '/', '')
                                                                                      || '{3}')
              and model_name in( {4}) ))
              GROUP BY group_next
             ORDER  BY MIN(step_sequence))
SELECT ( select distinct H1CPO value from  SFISM4.R_EDI_AE940H where TIMESTAMP ='SAGEMCOM' and H1DVNO = model_name AND ROWNUM =1) AS model_name,
       group_name,
       total_qty,
       pass_qty,
       repass_qty,
       fail_qty,
       refail_qty,
       FPY_Rate,
       Yield_Rate,
       To_char((repass_qty / total_qty) * 100, '990.99')
       || '%' retestok_rate,
       To_char(((fail_qty + refail_qty) / total_qty) * 100, '990.99')
       || '%' fail_rate
       FROM(
              select r.model_name,
              r.GROUP_NAME,
              SUM(r.pass_qty)                   pass_qty,
              SUM(r.fail_qty)                   fail_qty,
              SUM(Nvl(R.FIRST_FAIL_QTY, 0))     FIRST_FAIL_QTY,
              DECODE(SUM(R.PASS_QTY) + SUM(R.FAIL_QTY), 0, SUM(R.rePASS_QTY) + SUM(R.reFAIL_QTY), SUM(R.PASS_QTY) + SUM(R.FAIL_QTY)) Total_qty,
              To_number(To_char(100 * (SUM(R.PASS_QTY) + SUM(R.FAIL_QTY) - SUM(Nvl(R.FIRST_FAIL_QTY, 0))) / Decode((SUM(R.PASS_QTY)), 0, 1,
                                                                                                                                          (SUM(R.PASS_QTY) + SUM(R.FAIL_QTY))), '9900.00'))
              || '%'                            FPY_Rate,
              To_char(100 * SUM(R.PASS_QTY) / Decode((SUM(R.PASS_QTY) + SUM(R.FAIL_QTY)), 0, 1,
                                                                                            (SUM(R.PASS_QTY) + SUM(R.FAIL_QTY))), '9900.00')
              || '%'                            Yield_Rate,
              SUM(Nvl(r.repass_qty, 0))         repass_qty,
              SUM(Nvl(r.refail_qty, 0))         refail_qty,
              t.num
from R_STATION R, C_GROUP C, group_nex T
where
    Replace(R.WORK_DATE
                      || To_char(Work_Section, '00'), ' ', '') BETWEEN(Replace('{0}', '/', '')
                                                                        || '{2}') AND(Replace('{1}', '/', '')
                                                                                      || '{3}')
              and model_name in( {4})
              AND R.GROUP_NAME = C.GROUP_NAME
              AND r.section_Name = c.section_name
              AND C.GROUP_NAME NOT IN(SELECT GROUP_NAME
                                      FROM   SFIS1.C_GROUP_NOROUTE_T)
              AND R.GROUP_name = t.group_next
              AND substr(R.GROUP_name, 1, 2) not in('PA', 'CH', 'FQ', 'OB', 'AS', 'LA') 
              AND r.SECTION_NAME = 'TEST'
              AND r.line_name <> 'REPAIR'
       GROUP  BY R.model_name,
                 r.group_name,
                 t.num
                 ORDER  BY t.num)", obj.date_from, obj.date_to, timeFrom, timeTo, _string_fox_model);
            }

            dt = DBConnect.GetData(sql, obj.database);
            if (dt.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(new { message = "ok", data = dt, query = sql });
            }
        }
        [System.Web.Http.Route("GetFoxSagemcom")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetFoxSagemcom(SagemcomObject obj)
        {

            string _string_customer_model = GetSelectByList(obj.group_model, "H1CPO");
            string _string_model_model = GetSelectByList(obj.group_model, "model_name");
            string _string_model_modelr = GetSelectByList(obj.group_model, "r.model_name");
            string timeFrom = TimeToWorkSection(obj.timeFrom);
            string timeTo = TimeToWorkSection(obj.timeTo);
            string sql = string.Empty;
            DataTable dt;


            if (obj.option == "SagemcomQuery")
            {
                sql = "SELECT model_name,group_name,total_qty,pass_qty,repass_qty,fail_qty,refail_qty,FPY_Rate,Yield_Rate," +
      " To_char((repass_qty / total_qty) * 100, '990.99') || '%' retestok_rate," +
      "  To_char(((fail_qty + refail_qty) / total_qty) * 100, '990.99') || '%' fail_rate" +
      "  FROM(" +
      "  SELECT r.model_name," +
      "         r.GROUP_NAME," +
      "         SUM(r.pass_qty)                   pass_qty," +
      "        SUM(r.fail_qty)                   fail_qty," +
      "        SUM(Nvl(R.FIRST_FAIL_QTY, 0))     FIRST_FAIL_QTY," +
      "        DECODE(SUM(R.PASS_QTY) + SUM(R.FAIL_QTY), 0, SUM(R.rePASS_QTY) + SUM(R.reFAIL_QTY), SUM(R.PASS_QTY) + SUM(R.FAIL_QTY)) Total_qty," +
      "        To_number(To_char(100 * (SUM(R.PASS_QTY) + SUM(R.FAIL_QTY) - SUM(Nvl(R.FIRST_FAIL_QTY, 0))) / Decode((SUM(R.PASS_QTY)), 0, 1, (SUM(R.PASS_QTY) + SUM(R.FAIL_QTY))), '9900.00')) || '%'                            FPY_Rate," +
      "        To_char(100 * SUM(R.PASS_QTY) / Decode((SUM(R.PASS_QTY) + SUM(R.FAIL_QTY)), 0, 1, (SUM(R.PASS_QTY) + SUM(R.FAIL_QTY))), '9900.00') || '%'  Yield_Rate," +
      "        SUM(Nvl(r.repass_qty, 0))         repass_qty, SUM(Nvl(r.refail_qty, 0))         refail_qty, t.num" +
      " FROM(SELECT  WORK_SECTION, MO_NUMBER, WORK_DATE, MODEL_NAME, LINE_NAME, SECTION_NAME, GROUP_NAME, REPASS_QTY, REFAIL_QTY, PASS_QTY, FIRST_FAIL_QTY, FAIL_QTY" +
      "        FROM   SFISM4.H_STATION_REC_T" +
      "        UNION ALL" +
      "        SELECT  WORK_SECTION, MO_NUMBER, WORK_DATE, MODEL_NAME, LINE_NAME, SECTION_NAME, GROUP_NAME, REPASS_QTY, REFAIL_QTY, PASS_QTY, FIRST_FAIL_QTY, FAIL_QTY" +
      "        FROM   SFISM4.R_STATION_REC_T) R," +
      "       SFIS1.C_GROUP_CONFIG_T C," +
      "       (SELECT group_next," +
      "               MIN(step_sequence) num" +
      "        FROM   sfis1.c_route_control_t" +
      "        WHERE  route_code IN(SELECT DISTINCT route_code" +
      "                             FROM   sfism4.r105" +
      "                             WHERE  mo_number IN(SELECT DISTINCT mo_number" +
      "                                                 FROM   sfism4.r_station_rec_t" +
      "                                                 WHERE  Replace(WORK_DATE || To_char(Work_Section, '00'), ' ', '') BETWEEN(REPLACE('" + obj.date_from + "', '/','')||'" + timeFrom + "' " + ") AND(REPLACE('" + obj.date_to + "', '/','')||'" + timeTo + "' " + ")" +
      "                                                " + _string_model_model + "" +
      "                                                        ))" +
      "        GROUP  BY group_next" +
      "        ORDER  BY MIN(step_sequence))T" +
      " WHERE  Replace(R.WORK_DATE" +
      " || To_char(Work_Section, '00'), ' ', '') BETWEEN(REPLACE('" + obj.date_from + "', '/','')||'" + timeFrom + "' " + ") AND(REPLACE('" + obj.date_to + "', '/','')||'" + timeTo + "' " + ")" +
      " " + _string_model_modelr + "" +
      "        AND R.GROUP_NAME = C.GROUP_NAME" +
      "        AND r.section_Name = c.section_name" +
      "        AND C.GROUP_NAME NOT IN(SELECT GROUP_NAME" +
      "                                FROM   SFIS1.C_GROUP_NOROUTE_T)" +
      "        AND R.GROUP_name = t.group_next" +
      "        AND r.SECTION_NAME = 'TEST'" +
      "        AND substr(R.GROUP_name, 1, 2) not in('PA', 'CH', 'FQ', 'OB', 'AS', 'LA') " +
      "        and r.line_name <> 'REPAIR'" +
      " GROUP BY R.model_name," +
      "           r.group_name," +
      "           t.num" +
      "           ORDER  BY t.num)";
            }
            else
            {
                sql =string.Format(@"WITH R_STATION as
 (SELECT  WORK_SECTION,
                       MO_NUMBER,
                       WORK_DATE,
                       MODEL_NAME,
                       LINE_NAME,
                       SECTION_NAME,
                       GROUP_NAME,
                       REPASS_QTY,
                       REFAIL_QTY,
                       PASS_QTY,
                       FIRST_FAIL_QTY,
                       FAIL_QTY
              FROM   SFISM4.H_STATION_REC_T
              UNION ALL
              SELECT  WORK_SECTION,
                       MO_NUMBER,
                       WORK_DATE,
                       MODEL_NAME,
                       LINE_NAME,
                       SECTION_NAME,
                       GROUP_NAME,
                       REPASS_QTY,
                       REFAIL_QTY,
                       PASS_QTY,
                       FIRST_FAIL_QTY,
                       FAIL_QTY
                       FROM   SFISM4.R_STATION_REC_T),
 C_GROUP as
    (select* from   SFIS1.C_GROUP_CONFIG_T  ),
 group_nex as
    (SELECT group_next,
                     MIN(step_sequence)num
              FROM sfis1.c_route_control_t
            WHERE  route_code IN(SELECT DISTINCT route_code
                                 FROM   sfism4.r105
                                 WHERE  mo_number IN(SELECT DISTINCT mo_number

                                                     FROM sfism4.r_station_rec_t
                                                     WHERE  Replace(WORK_DATE
                                                                      || To_char(Work_Section, '00'), ' ', '') BETWEEN(Replace('{0}', '/', '')
                                                                        || '{2}') AND(Replace('{1}', '/', '')
                                                                                      || '{3}')
                    {4} ))
              GROUP BY group_next
             ORDER  BY MIN(step_sequence))
SELECT model_name,
       group_name,
       total_qty,
       pass_qty,
       repass_qty,
       fail_qty,
       refail_qty,
       FPY_Rate,
       Yield_Rate,
       To_char((repass_qty / total_qty) * 100, '990.99')
       || '%' retestok_rate,
       To_char(((fail_qty + refail_qty) / total_qty) * 100, '990.99')
       || '%' fail_rate
       FROM(
              select r.model_name,
              r.GROUP_NAME,
              SUM(r.pass_qty)                   pass_qty,
              SUM(r.fail_qty)                   fail_qty,
              SUM(Nvl(R.FIRST_FAIL_QTY, 0))     FIRST_FAIL_QTY,
              DECODE(SUM(R.PASS_QTY) + SUM(R.FAIL_QTY), 0, SUM(R.rePASS_QTY) + SUM(R.reFAIL_QTY), SUM(R.PASS_QTY) + SUM(R.FAIL_QTY)) Total_qty,
              To_number(To_char(100 * (SUM(R.PASS_QTY) + SUM(R.FAIL_QTY) - SUM(Nvl(R.FIRST_FAIL_QTY, 0))) / Decode((SUM(R.PASS_QTY)), 0, 1,
                                                                                                                                          (SUM(R.PASS_QTY) + SUM(R.FAIL_QTY))), '9900.00'))
              || '%'                            FPY_Rate,
              To_char(100 * SUM(R.PASS_QTY) / Decode((SUM(R.PASS_QTY) + SUM(R.FAIL_QTY)), 0, 1,
                                                                                            (SUM(R.PASS_QTY) + SUM(R.FAIL_QTY))), '9900.00')
              || '%'                            Yield_Rate,
              SUM(Nvl(r.repass_qty, 0))         repass_qty,
              SUM(Nvl(r.refail_qty, 0))         refail_qty,
              t.num
from R_STATION R, C_GROUP C, group_nex T
where
    Replace(R.WORK_DATE
                      || To_char(Work_Section, '00'), ' ', '') BETWEEN(Replace('{0}', '/', '')
                                                                        || '{2}') AND(Replace('{1}', '/', '')
                                                                                      || '{3}')
              {4}
              AND R.GROUP_NAME = C.GROUP_NAME
              AND r.section_Name = c.section_name
              AND C.GROUP_NAME NOT IN(SELECT GROUP_NAME
                                      FROM   SFIS1.C_GROUP_NOROUTE_T)
              AND R.GROUP_name = t.group_next
             AND substr(R.GROUP_name, 1, 2) not in('PA', 'CH', 'FQ', 'OB', 'AS', 'LA') 
              AND r.SECTION_NAME = 'TEST'
              AND r.line_name <> 'REPAIR'
       GROUP  BY R.model_name,
                 r.group_name,
                 t.num
                 ORDER  BY t.num)", obj.date_from, obj.date_to, timeFrom, timeTo, _string_model_model);
            }

            dt = DBConnect.GetData(sql, obj.database);
            if (dt.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(new { message = "ok", data = dt, query = sql });
            }
        }
    }
}
