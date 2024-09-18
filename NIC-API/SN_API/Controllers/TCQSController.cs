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
    public class TCQSController : ApiController
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


        [System.Web.Http.Route("TCQS-Defect")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetTCQSDefect(TCQSElement model)
        {
            string workSectionFrom = TimeToWorkSection(model.timeFrom);
            string workSectionTo = TimeToWorkSection(model.timeTo);
            StringBuilder sb = new StringBuilder();
            StringBuilder groupBy = new StringBuilder();
            sb.Append(" select ");
            sb.Append(" R.MODEL_NAME,  ");
            sb.Append(" R.GROUP_NAME, ");
            groupBy.Append(" GROUP BY ");
            if (model.iStationName)
            {
                sb.Append(" r.STATION_NAME, ");
                groupBy.Append(" r.STATION_NAME, ");
            }
            sb.Append(" B.WIP_QTY, ");
            sb.Append(" B.FIRST_FAIL_QTY, ");
            sb.Append(" B.SECOND_FAIL_QTY, DECODE(SUM(B.wip_qty),0,0, ");
            sb.Append(" to_number(to_char(sum(B.PASS_QTY)/SUM(B.WIP_QTY)*100,'990.99')))||'%' ETEYR, DECODE(SUM(B.wip_qty),0,0, ");
            sb.Append(" to_number(to_char((sum(B.FIRST_FAIL_QTY)-sum(B.REPAIR_QTY))/SUM(B.WIP_QTY)*100,'990.99')))||'%'  RetestYR,  ");
            sb.Append(" R.ERROR_CODE, ");
            sb.Append(" SUM(R.TEST_FAIL_QTY) TEST_FAIL_QTY, ");
            sb.Append(" to_number(to_char(100*SUM(R.TEST_FAIL_QTY)/{0},'990.99'))||'%' Rate1, ");
            sb.Append(" SUM(R.FAIL_QTY) FAIL_QTY, ");
            sb.Append(" DECODE(SUM(B.wip_qty),0,0, ");
            sb.Append(" ROUND(SUM(R.fail_qty)/B.WIP_QTY*1000000)) DPPM  ");
            sb.Append(" from ");
            sb.Append(" SFISM4.R_ATE_ERRCODE_T R, ");
            sb.Append(" (SELECT ");
            sb.Append(" SUM(C.wip_qty) wip_qty, ");
            sb.Append(" sum(C.PASS_QTY) PASS_QTY , ");
            sb.Append(" sum(C.REPAIR_QTY) REPAIR_QTY, ");
            sb.Append(" SUM(C.first_fail_qty) first_fail_qty, ");
            sb.Append(" SUM(C.REPAIR_QTY)+SUM(C.FAIL_QTY) second_fail_qty , ");
            sb.Append(" C.GROUP_NAME, ");
            sb.Append(" C.MODEL_NAME  ");
            sb.Append(" FROM sfism4.r_station_ate_t  C ");
            sb.Append(" WHERE ");
            sb.Append($" C.WORK_DATE || TRIM (TO_CHAR (C.Work_Section, '00')) >=('{model.dateFrom}' || '{workSectionFrom}') AND ");
            sb.Append($" C.WORK_DATE || TRIM (TO_CHAR (C.Work_Section, '00')) < ('{model.dateTo}' || '{workSectionTo}') ");
            sb.Append(GetSelectByList(model.listModel, "c.model_name"));
            sb.Append(GetSelectByList(model.listLine, "c.line_name"));
            sb.Append(GetSelectByList(model.listMo, "c.MO_Number"));
            sb.Append(GetSelectByList(model.listGroup, "C.Group_NAME"));
            sb.Append(" GROUP BY ");
            sb.Append(" C.model_name, ");
            sb.Append(" C.group_name) B  ");
            sb.Append(" WHERE ");
            sb.Append(" B.GROUP_NAME=R.GROUP_NAME  ");
            sb.Append(" AND B.MODEL_NAME=R.MODEL_NAME  ");
            sb.Append($" AND R.WORK_DATE || TRIM (TO_CHAR (work_Section, '00')) >=('{model.dateFrom}' || '{workSectionFrom}') ");
            sb.Append($" AND R.WORK_DATE || TRIM (TO_CHAR (work_Section, '00')) < ('{model.dateTo}' || '{workSectionTo}') ");            
            sb.Append(GetSelectByList(model.listModel, "r.model_name"));
            sb.Append(GetSelectByList(model.listLine, "r.line_name"));
            sb.Append(GetSelectByList(model.listMo, "r.MO_Number"));
            sb.Append(GetSelectByList(model.listGroup, "r.Group_NAME"));
            sb.Append(groupBy.ToString());
            sb.Append(" R.ERROR_CODE,R.GROUP_NAME,R.MODEL_NAME,B.WIP_QTY,B.FIRST_FAIL_QTY,B.SECOND_FAIL_QTY ");
            sb.Append(" ORDER BY R.MODEL_NAME,R.GROUP_NAME, TEST_FAIL_QTY DESC ");
            string queryString = string.Format(sb.ToString(), 1000000);
            DataTable dtCheck = new DataTable();
            try
            {
                dtCheck = DBConnect.GetData(" SELECT SUM (TEST_FAIL_QTY) QTY FROM( "+queryString+" ) ", model.database_name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                string numberTotal = dtCheck.Rows[0]["QTY"].ToString();
                int totalQTY = 0;
                if (string.IsNullOrEmpty(numberTotal))
                {
                    totalQTY = 0;
                }
                else
                {
                    totalQTY = int.Parse(numberTotal);                    
                }
                queryString = string.Format(sb.ToString(), totalQTY);
                dtCheck = DBConnect.GetData(queryString, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }
        // GET: TCQS      
        [System.Web.Http.Route("TCQS-YieldRate")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetTCQSYieldRate(TCQSElement model)
        {
            string workSectionFrom = TimeToWorkSection(model.timeFrom);
            string workSectionTo = TimeToWorkSection(model.timeTo);            
            StringBuilder sb = new StringBuilder();
            StringBuilder groupBy = new StringBuilder();
            StringBuilder orderBy = new StringBuilder();
            sb.Append(" select ");
            groupBy.Append(" group by ");
            orderBy.Append(" order by ");
            if (model.iDate)
            {
                sb.Append(" r.work_date, ");
                groupBy.Append(" r.work_date, ");
            }


            if (model.iModelName)
            {
                sb.Append(" r.Model_name, ");
                groupBy.Append(" r.Model_name, ");
                orderBy.Append(" r.Model_name, ");
            }

            if (model.iVersionCode)
            {
                sb.Append(" R.VERSION_CODE, ");
                groupBy.Append(" R.VERSION_CODE, ");
            }
            if (model.iLineName)
            {
                sb.Append(" line_name, ");
                groupBy.Append(" line_name, ");
            }
            if (model.iMoNumber)
            {
                sb.Append(" r.mo_number, ");
                groupBy.Append(" r.mo_number, ");
            }
            if (1 == 1)
            {
                sb.Append(" r.GROUP_NAME, ");
                groupBy.Append(" r.GROUP_NAME ");
                orderBy.Append(" r.GROUP_NAME ");
            }
            if (model.iStationName)
            {
                sb.Append(" station_name,");
                groupBy.Append(" ,station_name ");
            }
            sb.Append(" SUM (R.WIP_QTY) WIP_QTY, ");
            sb.Append(" SUM (R.FIRST_FAIL_QTY) FIRST_Fail_qty, ");
            sb.Append(" SUM (R.REPAIR_QTY) REPAIR_QTY, ");
            sb.Append(" SUM (R.PASS_QTY) Pass_qty, ");
            sb.Append(" SUM (R.FAIL_QTY) FAIL_QTY, ");
            sb.Append(" DECODE (SUM (r.wip_qty),0, 0,TO_NUMBER (TO_CHAR ((  (SUM (R.WIP_QTY) - SUM (R.FIRST_FAIL_QTY))/ SUM (R.WIP_QTY))* 100,'9999.99')))|| '%' FPRATE, ");
            sb.Append(" DECODE (SUM (r.wip_qty),0, 0,TO_NUMBER (TO_CHAR (SUM (R.PASS_QTY) / SUM (R.WIP_QTY) * 100, '9999.99')))|| '%' PWRATE, ");
            sb.Append(" DECODE (SUM (r.wip_qty),0, 0,TO_NUMBER (TO_CHAR ( (1 - SUM (R.PASS_QTY) / SUM (R.WIP_QTY)) * 100,'9999.99')))|| '%'FWRATE, ");
            sb.Append(" DECODE (SUM (r.wip_qty),0, 0,TO_NUMBER (TO_CHAR ((SUM (R.FIRST_FAIL_QTY) - SUM (REPAIR_QTY))/ SUM (R.WIP_QTY)* 100,'9999.99')))|| '%'FRWRATE ");
            sb.Append(" FROM SFISM4.R_STATION_ATE_T R ");
            sb.Append(" WHERE  ");
            sb.Append($" R.WORK_DATE || TRIM (TO_CHAR (Work_Section, '00')) >=('{model.dateFrom}' || '{workSectionFrom}') ");
            sb.Append($" AND R.WORK_DATE || TRIM (TO_CHAR (work_Section, '00')) < ('{model.dateTo}' || '{workSectionTo}') ");
            sb.Append(GetSelectByList(model.listModel, "r.model_name"));
            sb.Append(GetSelectByList(model.listLine, "r.line_name"));
            sb.Append(GetSelectByList(model.listMo, "r.MO_Number"));
            sb.Append(GetSelectByList(model.listGroup, "r.Group_NAME"));

            sb.Append(groupBy.ToString());
            sb.Append(orderBy.ToString());
            
            Console.WriteLine(sb.ToString());
            string queryString = sb.ToString();
            string ass = "";
            DataTable dtCheck = new DataTable();
            try
            {
                dtCheck = DBConnect.GetData(queryString, model.database_name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }
    }
}