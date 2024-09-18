using SN_API.Models;
using SN_API.Models.Config;
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

namespace SN_API.Controllers.Config
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class Config53Controller : ApiController
    {
        [System.Web.Http.Route("UpdateConfig53")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> UpdateConfig53(Config53Element emp)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                //check exist
                string strCheckexist = $" SELECT * FROM SFIS1.C_EMP_DESC_T WHERE EMP_BC = '{emp.OLD_PASS}'  AND  EMP_NO = {emp.EMP_NO}";
                if (DBConnect.GetData(strCheckexist, emp.database_name).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                }
                else
                {
                    string strCheckprivilege = $" SELECT * FROM SFIS1.C_PRIVILEGE WHERE FUN='PASSWORD_EDIT' AND PRG_NAME='CONFIG' AND EMP = {emp.EMP_NO}";
                    if (DBConnect.GetData(strCheckprivilege, emp.database_name).Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }

                    sb.Append("UPDATE");
                    sb.Append($" SFIS1.C_EMP_DESC_T");
                    sb.Append($" SET EMP_BC = '{emp.EMP_BC}', EMP_PASS='{emp.EMP_BC}'  WHERE EMP_NO='{emp.EMP_NO}' ");

                    string str = sb.ToString();
                    DataTable dtCheck = DBConnect.GetData(str, emp.database_name);

                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
    }
}