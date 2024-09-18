using SN_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SN_API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Redirect("~/Swagger");           
        }
        [HttpGet]
        public string GetAPI(string sn)
        {
            string[] arrSN = sn.Split(',');
            string tmp = "";
            List<Information> listInfo = new List<Information>();
            for (int i = 0; i < 10; i++)
            {
                listInfo.Add(new Information("1" + i, "1" + i, "1" + i, "1" + i));
            }
            return "cdffc";
        }

    }
}