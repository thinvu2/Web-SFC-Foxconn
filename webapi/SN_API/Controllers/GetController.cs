using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SN_API.Handler.JWT;
using SN_API.Models;
using SN_API.Models.Ulist.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
namespace SN_API.Controllers
{
    [Authorize]
    public class GetController : Controller
    {
        [System.Web.Http.HttpGet]

        public string GetData(string SN, string dbName)
        {
            return new Information().GetDataFromProc(SN, dbName);
        }
        [System.Web.Http.HttpGet]
        public async Task<JsonResult> GetRoute(string dynamic)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            //IHttpActionResult
            JObject json = null;
            try
            {
                json = JObject.Parse(dynamic);
                string db = json.GetValue("db").ToString();
                string function_name = json.GetValue("function_name").ToString();
                string route = json.GetValue("route").ToString();
                string group_name = json.GetValue("group_name").ToString();
                // dynamic={"db":"UI","function_name":"SFIS1.SI_YIELD_TEST","route":"8952","group_name":"NOOO"}
                //http://localhost:55829/Get/GetRoute?db=ad&function_name=ASDASD&route=2
                List<Route> list_route = new Route().GetRoute(db, function_name, route, group_name);
                var response = new HttpResponseMessage();
                return Json(new { data = list_route }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        [System.Web.Http.Route("Emp")]
        [System.Web.Http.Authorize]
        [HttpPost]
        public async Task<ActionResult> GetEMP(EmpResponse root)
        {
            List<Employee> listEmp = new List<Employee>();
            string Token = await Task.Run(() => root.Token);
            var ApplicationID = await Task.Run(() => root.ApplicationID);
            var _listEMPNo = await Task.Run(() => root.EmpList);
            int i = 0;
            string queryString = "";

            var response = new HttpResponseMessage();
            //response = Request.CreateResponse(HttpStatusCode.OK, new { data = JwtToken.GetPayLoad(Token) });

            if (Token != "Token") return Json(new { ApplicationID = ApplicationID, EmpList = listEmp }, JsonRequestBehavior.AllowGet);
            foreach (var item in _listEMPNo)
            {
                if (i == 0)
                {
                    queryString += "SELECT * FROM SFIS1.C_AMS_USER_INFOR_T WHERE EMPNO='" + item.EMPNO + "' and APPLICATIONID='" + ApplicationID + "' ";
                }
                else
                {
                    queryString += "UNION SELECT * FROM SFIS1.C_AMS_USER_INFOR_T WHERE EMPNO='" + item.EMPNO + "' and APPLICATIONID='" + ApplicationID + "'";
                }
                i = 1;
            }
            if (queryString == "") return Json(new { ApplicationID = ApplicationID, EmpList = listEmp }, JsonRequestBehavior.AllowGet);

            DataTable dt = Employee.GetData(queryString, "CPEI");

            if (dt.Rows.Count < 0)
            {
                return Json(new { ApplicationID = ApplicationID, EmpList = listEmp }, JsonRequestBehavior.AllowGet);
            }
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                listEmp.Add(new Employee(dt.Rows[j]["USERID"].ToString(), dt.Rows[j]["EMPNO"].ToString(), dt.Rows[j]["NAME"].ToString(), dt.Rows[j]["GROUPNAME"].ToString(), dt.Rows[j]["GROUPREMARK"].ToString(), dt.Rows[j]["EFFECTTIME"].ToString()));
            }

            return Json(new { ApplicationID = ApplicationID, EmpList = listEmp }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public async Task<ActionResult> Queray(JObject obj)
        {
            //string a = (string) jObject["name"];
            return Json(new { ApplicationID = "aa", EmpList = obj }, JsonRequestBehavior.AllowGet);
        }
    }
}
