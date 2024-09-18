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
using static SN_API.Models.LineElement;

namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class Config1Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig1Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig1Content(ConfigElement model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.fun))
            {
                strGetData = $" select LINE_NAME,LINE_CODE,LINE_DESC,LINE_TYPE from SFIS1.C_LINE_DESC_T order by line_name ";
            }
            else
            {
                strGetData = $" select LINE_NAME,LINE_CODE,LINE_DESC,LINE_TYPE from SFIS1.C_LINE_DESC_T where  upper(LINE_NAME) like '%{model.fun.ToUpper()}%'  order by line_name ";
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
        [System.Web.Http.Route("InsertUpdateConfig1")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig1(LineElement model)
        {
            string strCheckExist = $" select LINE_NAME from SFIS1.C_LINE_DESC_T where line_name = '{model.line_name}' ";
            DataTable dtCheck = DBConnect.GetData(strCheckExist, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                //create
                try
                {
                    DBConnect.ExecuteNoneQuery($" Insert into SFIS1.C_LINE_DESC_T (LINE_NAME, LINE_TYPE, LINE_DESC, LINE_CODE) Values ('{model.line_name}', '{model.line_type}', '{model.line_desc}', '{model.line_code}') ", model.database_name);
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
                }
            }
            else
            {
                //update
                try
                {
                    DBConnect.ExecuteNoneQuery($" update SFIS1.C_LINE_DESC_T set LINE_NAME= '{model.line_name}' , LINE_TYPE = '{model.line_type}' , LINE_DESC = '{model.line_desc}', LINE_CODE = '{model.line_code}' where line_name = '{model.line_name}' ", model.database_name);
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
                }
            }
        }
        public string GetSelectByList(List<LineName> list, string type)
        {
            StringBuilder sb = new StringBuilder();
            if (list.Count > 0)
            {
                var sbContent = new StringBuilder();
                sbContent.Append(" IN (");

                for (int i = 0; i < list.Count; i++)
                {
                    string mo = list[i].LINE_NAME.ToString();

                    sbContent.AppendFormat("'{0}'", mo);
                    if (i < list.Count - 1)
                    {
                        sbContent.Append(",");
                    }
                }
                sbContent.Append(")");
                sb.Append($" AND {type} {sbContent} ");
            }
            return sb.ToString();
        }
        [System.Web.Http.Route("DeleteConfig1")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig1(LineElement model)
        {
            if (model.listLine.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            string strDelete = $" delete SFIS1.C_LINE_DESC_T where 1 = 1 {GetSelectByList(model.listLine, "LINE_NAME")} ";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
    }
}