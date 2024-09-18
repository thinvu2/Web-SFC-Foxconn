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
    public class BonePileController : ApiController
    {
        // GET: BonePile
        [System.Web.Http.Route("GetBonePileElement")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetBonePileElement(BonePileElement model)
        {
            StringBuilder builder = new StringBuilder();

            string SQL = $" SELECT DISTINCT MODEL_NAME VALUE FROM  SFISM4.R_REPAIR_DAILYREPORT_SN A, SFISM4.R107 B WHERE    A.SN = B.SERIAL_NUMBER AND A.REPORT_DATE BETWEEN '{model.dateFrom}' AND '{model.dateTo}' ORDER BY MODEL_NAME ";

            builder.Append(SQL);

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

        [System.Web.Http.Route("GetBonePileData")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetBonePileData(BonePileElement model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT A.REPORT_DATE,A.BU,A.SN SERIAL_NUMBER,A.SN_TYPE,A.CHECKIN_DATE,A.CHECKOUT_DATE,B.MODEL_NAME,B.GROUP_NAME,B.WIP_GROUP FROM SFISM4.R_REPAIR_DAILYREPORT_SN A, SFISM4.R107 B ");
            builder.Append(" WHERE A.SN = B.SERIAL_NUMBER ");
            builder.Append(GetSelectByList(model.listModel, "B.MODEL_NAME"));
            builder.Append($" AND A.REPORT_DATE BETWEEN '{model.dateFrom}' AND '{model.dateTo}' ");
            builder.Append(" ORDER BY A.REPORT_DATE desc,A.CHECKIN_DATE, A.CHECKOUT_DATE  ");

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
    }
}