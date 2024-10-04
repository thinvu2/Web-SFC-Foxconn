using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

using System.Net;
using System.Net.Http;

using Newtonsoft.Json.Linq;

using SN_API.Services;
using System.Diagnostics;
using Oracle.ManagedDataAccess.Types;

[EnableCors(origins: "*", headers: "*", methods: "*")]
public class GeneralController : ApiController
{
    private static string PROCEDURE_NAME = "SFIS1.TE_CALL_SFC";

    [HttpPost]
    [Route("api/general/TestingServer")]
    public async Task<IHttpActionResult> TestingServer([FromBody] Request request)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null.");
        }

        try
        {
            List<OracleParameter> parameters = new List<OracleParameter>
            {
                new OracleParameter("TESTER", OracleDbType.Varchar2, 15, request.EmpNo, ParameterDirection.Input),
                new OracleParameter("TESTLINE", OracleDbType.Varchar2, 15, request.LineName.Trim().ToUpper(), ParameterDirection.Input),
                new OracleParameter("TESTGROUP", OracleDbType.Varchar2, 15, request.WorkStation.Trim().ToUpper(), ParameterDirection.Input),
                new OracleParameter("TESTMODEL", OracleDbType.Varchar2, 25, request.ModelName.Trim().ToUpper(), ParameterDirection.Input),
                new OracleParameter("TESTPCNAME", OracleDbType.Varchar2, 25, request.PCName.Trim().ToUpper(), ParameterDirection.Input),
                new OracleParameter("TESTFORMATNAME", OracleDbType.Varchar2, 30, request.TestFormatName.Trim().ToUpper(), ParameterDirection.Input),
                new OracleParameter("BARCODESN", OracleDbType.Varchar2, 80, request.SN.Trim().ToUpper(), ParameterDirection.Input),
                new OracleParameter("ACTIONNAME", OracleDbType.Varchar2, 25, request.Action.Trim().ToUpper(), ParameterDirection.Input),

                new OracleParameter("TESTDATA", OracleDbType.Varchar2, 500, request.SendData, ParameterDirection.Input),
                new OracleParameter("TESTRESULT", OracleDbType.Varchar2, 30, request.TestResult, ParameterDirection.Input),

                new OracleParameter("MESSAGE", OracleDbType.Varchar2, 500, "", ParameterDirection.Output),
                new OracleParameter("RETURNDATA", OracleDbType.Varchar2, 500, "", ParameterDirection.Output)
            };

            var dicResult = await ExecuteProcedureAsync(PROCEDURE_NAME, parameters);

            if (dicResult != null && dicResult.Count > 0)
            {
                return Ok(new Response
                {
                    Action = request.Action,
                    Success = dicResult["MESSAGE"].ToString() == "OK",
                    Message = dicResult["MESSAGE"].ToString(),
                    Data = dicResult["RETURNDATA"].ToString()
                });
            }

            return BadRequest("Can't get result from SFIS1.TE_CALL_SFC.");
        }
        catch (Exception e)
        {
            return InternalServerError(new Exception($"Exception occurred: {e.Message}", e));
        }
    }

    private async Task<Dictionary<string, object>> ExecuteProcedureAsync(string procedureName, List<OracleParameter> parameters)
    {
        var result = new Dictionary<string, object>();
        var connectionString = new GetString().Get()["NIC"];

        using (var connection = new OracleConnection(connectionString))
        {
            await connection.OpenAsync();
            using (var command = new OracleCommand(procedureName, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;

                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                await command.ExecuteNonQueryAsync();

                result["MESSAGE"] = command.Parameters["MESSAGE"].Value.ToString();
                result["RETURNDATA"] = command.Parameters["RETURNDATA"].Value.ToString();
            }
        }

        return result;
    }

    public class Request
    {
        public string EmpNo { get; set; }
        public string LineName { get; set; }
        public string WorkStation { get; set; }
        public string ModelName { get; set; }
        public string PCName { get; set; }
        public string TestFormatName { get; set; }
        public string SN { get; set; }
        public string Action { get; set; }
        public string SendData { get; set; }
        public string TestResult { get; set; }
    }

    public class Response
    {
        public string Action { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }
}
