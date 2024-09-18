using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using SN_API.Models;
using System.Threading.Tasks;
using System.Text;
using System.Data;
using SN_API.Models.Config;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using SN_API.Services;

namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LINKDNController : ApiController
    {
        [System.Web.Http.Route("GetLinkDnImport")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetLinkDnImport(LinkDnElement model)
        {
            try
            {
                string checkExistsProcedure = "SFIS1.CHECK_DN_DETAILS";
                string procedureResult = "";
                var connectionString = new GetString().Get()[model.database_name];
                using (var connection = new OracleConnection(connectionString))
                {
                    using(var command = new OracleCommand (checkExistsProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("P_EMP_NO", OracleDbType.Varchar2).Value = model.EMP_NO;
                        command.Parameters.Add("P_BFIH_DN", OracleDbType.Varchar2).Value = model.BFIH_DN;
                        command.Parameters.Add("P_FTI_DN", OracleDbType.Varchar2).Value = model.FTI_DN;
                        command.Parameters.Add("P_NOTE_DN", OracleDbType.Varchar2).Value = model.NOTE_DN;
                        command.Parameters.Add("P_FUNCTION", OracleDbType.Varchar2).Value = model.LINK_DN;

                        command.Parameters.Add("o_dt", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        command.Parameters.Add("RES", OracleDbType.Varchar2, 100).Direction = ParameterDirection.Output;
                        connection.Open();
                        await command.ExecuteNonQueryAsync();
                        procedureResult = command.Parameters["RES"].Value.ToString();
                    }
                }
                if(procedureResult == "OK")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "Error", data = procedureResult });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }

        [System.Web.Http.Route("GetDnContent")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetDnContent(LinkDnElement model)
        {
            try
            {
                string strGetData = "";
                if (string.IsNullOrEmpty(model.BFIH_DN))
                {
                    strGetData = $"SELECT INVOICE as BFIH_DN, SHIP_NO AS FTI_DN,  INV_NO AS QUANTITY, TIME FROM SFISM4.R_DN_SHIP_NO_T  WHERE ROWNUM <20 ORDER BY TIME DESC";
                }
                else
                {
                    strGetData = $"SELECT INVOICE as BFIH_DN, SHIP_NO AS FTI_DN,  INV_NO AS QUANTITY, TIME FROM SFISM4.R_DN_SHIP_NO_T  WHERE INVOICE = '{model.BFIH_DN}' ";
                }
                DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }
    }
    public class LinkDnElement
    {
        public string database_name { get; set; }
        public string EMP_NO { get; set; }
        public string BFIH_DN { get; set; }
        public string FTI_DN { get; set; }
        public string NOTE_DN { get; set; }
        public string LINK_DN { get; set; }
    }
}