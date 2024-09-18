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

namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class QueryRepairController : ApiController
    {
        [System.Web.Http.Route("GetModelRepair")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetModelRepair(ValueQueryRepair value)
        {
            try
            {
                string _query_string = "select distinct MODEL_NAME from sfism4.r_mo_base_t where 1 = 1 ORDER BY MODEL_NAME";

                DataTable dt = DBConnect.GetData(_query_string, value.database);
                return Request.CreateResponse(new { message = "ok", data = dt, query = _query_string });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(new { message = "Authorization has been denied for this request." });
            }

        }
        [System.Web.Http.Route("GetMoRepair")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetMoRepair(ValueQueryRepair value)
        {
            try
            {
                string _query_string = "select MO_NUMBER from sfism4.r_mo_base_t where model_name='" + value.model_name + "' ORDER BY MO_NUMBER";

                DataTable dt = DBConnect.GetData(_query_string, value.database);
                return Request.CreateResponse(new { message = "ok", data = dt, query = _query_string });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(new { message = "Authorization has been denied for this request." });
            }

        }
        [System.Web.Http.Route("QueryRepair")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> QueryRepair(ValueQueryRepair value)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string _query_string = "";
                string repair_rate = "";

                sb.Append("select * from sfism4.r_repair_t WHERE 1 = 1 ");

                if (value.model_name != "" && value.model_name != "ALL")
                {
                    sb.Append($" AND MODEL_NAME = '{value.model_name}'");
                }

                if (value.mo_number != "" && value.mo_number != "ALL")
                {
                    sb.Append($" AND MO_NUMBER = '{value.mo_number}'");
                }

                if (value.field == "REPAIRER")
                {
                    sb.Append($" AND REPAIRER = '{value.input}'");

                    if (value.model_name != "" && value.model_name != "ALL")
                    {
                        repair_rate = string.Format(@"select trunc(((SELECT COUNT(*) FROM   (SELECT COUNT(SERIAL_NUMBER) c,SERIAL_NUMBER 
                        FROM sfism4.r_repair_t where model_name='{0}' and repair_status is not null and 
                        repairer='{1}' group by serial_number) where c=1)/(SELECT COUNT(DISTINCT SERIAL_NUMBER) S 
                        FROM  sfism4.r_repair_t where model_name='{0}'  and repairer='{1}'))*100,2) 
                        as rate from dual", value.model_name, value.input);
                    }

                    if (value.mo_number != "" && value.mo_number != "ALL")
                    {
                        repair_rate = "";
                        repair_rate = string.Format(@"select trunc(((SELECT COUNT(*) FROM   (SELECT COUNT(SERIAL_NUMBER) c,SERIAL_NUMBER 
                        FROM sfism4.r_repair_t where model_name='{0}' and  mo_number='{1}'  and repair_status is not null and 
                        repairer='{2}' group by serial_number) where c=1)/(SELECT COUNT(DISTINCT SERIAL_NUMBER) S 
                        FROM  sfism4.r_repair_t where model_name='{0}' and  mo_number='{1}' and repairer='{2}'))*100,2) 
                        as rate from dual", value.model_name,value.mo_number, value.input);
                    }
                }
                else if (value.field == "REPAIR_SN")
                {
                    sb.Append($" AND SERIAL_NUMBER = '{value.input}'");
                }
                else if (value.field == "LOCATION")
                {
                    sb.Append($" and repair_status is not null AND error_item_code = '{value.input}'");
                }
                else if (value.field == "REPAIR_DAY")
                {

                }
                else if (value.field == "REPAIR_MO")
                {
                    sb.Append($" AND MO_NUMBER = '{value.input}'");
                }
                else if (value.field == "REPAIR_WIP")
                {

                }
                else
                {

                }

                if (value.date_from != null && value.date_to != null)
                {
                    sb.Append($" AND test_time between TO_DATE('" + value.date_from + "','YYYY / MM / DD HH24: MI: SS') and TO_DATE('" + value.date_to + "','YYYY/MM/DD HH24:MI:SS')");
                }

                _query_string = sb.ToString();

                DataTable dt = DBConnect.GetData(_query_string, value.database);
                DataTable rate = DBConnect.GetData(repair_rate, value.database);
                //return Request.CreateResponse(new { message = "ok", data = dt, query = _query_string });
                return Request.CreateResponse(HttpStatusCode.OK, new { data = dt, data1 = "",rate = rate, query = _query_string, result = "ok" });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(new { message = "Authorization has been denied for this request." });
            }

        }
    }
}