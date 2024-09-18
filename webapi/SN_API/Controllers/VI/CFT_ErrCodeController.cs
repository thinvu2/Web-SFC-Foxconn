using SN_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace SN_API.Controllers.VI
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CFT_ErrCodeController : ApiController
    {
        [System.Web.Http.Route("getCFT_ErrorCode")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> getCFT_ErrorCode(string condition, string data)
        {

            string strGetData = "";
            if (!string.IsNullOrEmpty(data))
            {
                strGetData = "SELECT /*+ first_rows */ rownum NO,CFT_NAME,MODEL_NAME,A.ERROR_CODE,ERROR_DESC,ERROR_DESC2,EMP_NO,TO_CHAR(CREATE_DATE,'YYYY/MM/DD HH24:MI:SS') AS CREATE_DATE "
                        + "FROM SFIS1.C_CFT_ERROR_CODE_T A  LEFT OUTER JOIN SFIS1.C_ERROR_CODE_T B ON A.ERROR_CODE = B.ERROR_CODE WHERE " + condition + " like '%" + data + "%' ORDER BY NO";
            }
            else
            {
                strGetData = "SELECT /*+ first_rows */ rownum NO,CFT_NAME,MODEL_NAME,A.ERROR_CODE,ERROR_DESC,ERROR_DESC2,EMP_NO,TO_CHAR(CREATE_DATE,'YYYY/MM/DD HH24:MI:SS') AS CREATE_DATE "
                        + "FROM SFIS1.C_CFT_ERROR_CODE_T A  LEFT OUTER JOIN SFIS1.C_ERROR_CODE_T B ON A.ERROR_CODE = B.ERROR_CODE ORDER BY NO";
            }

            DataTable dtCheck = DBConnect.GetData(strGetData,"NIC");
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }

        [System.Web.Http.Route("getErrorCode")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> getErrorCode(string cft_name, string model)
        {

            string strGetData = "";
            if (cft_name != "" && model != "")
            {
                strGetData = "SELECT ERROR_CODE AS ID,ERROR_DESC||' ( '||ERROR_DESC2||' )' AS NAME FROM SFIS1.C_ERROR_CODE_T WHERE ERROR_CODE NOT IN "
                                               + "(SELECT ERROR_CODE FROM  SFIS1.C_ERROR_CODE_T WHERE CFT_NAME ='" + cft_name + "' AND MODEL_NAME ='" + model + "') ORDER BY ERROR_CODE ASC";
            }
            else
            {
                strGetData = "SELECT ERROR_CODE AS ID,ERROR_DESC||' ( '||ERROR_DESC2||' )' AS NAME1 FROM SFIS1.C_ERROR_CODE_T ORDER BY ERROR_CODE ASC ";
            }

            DataTable dtCheck = DBConnect.GetData(strGetData, "NIC");
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