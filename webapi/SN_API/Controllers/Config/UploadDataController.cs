using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using SN_API.Services;
using System.Diagnostics;
using SN_API.Models;
using SN_API.Models.Config;
using System.Data;
using Oracle.ManagedDataAccess.Types;

namespace SN_API.Controllers.Config
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UploadController : ApiController
    {
        [Route("QuerySPDM_SN")]
        [HttpPost]
        public async Task<HttpResponseMessage> QuerySPDM_SN(UploadDataElement model)
        {
            string strGetData = $" select * from sfism4.r_spdm_sn where SERIAL_NUMBER = '{model.CHECK_BFIH_DN}' or SHIPPING_SN = '{model.CHECK_BFIH_DN}' or MODEL_NAME = '{model.CHECK_BFIH_DN}' ";
            //  string strGetData = $"select * from table(PKG_RETURN_TABLE.F_GETLISTRESOURCE('{model.emp_no}')) ";

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
        [Route("QueryDnNo")]
        [HttpPost]
        public async Task<HttpResponseMessage> QueryDnNo(UploadDataElement model)
        {
            try
            {
                string checkExistsProcedure = "SFIS1.CHECK_DN_DETAILS";
                string procedureResult = "";
                List<Dictionary<string, object>> cursorData = new List<Dictionary<string, object>>();
                var connectionString = new GetString().Get()[model.database_name];
                using (var connection = new OracleConnection(connectionString))
                {
                    using (var command = new OracleCommand(checkExistsProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("P_EMP_NO", OracleDbType.Varchar2).Value = model.EMP_NO;
                        command.Parameters.Add("P_BFIH_DN", OracleDbType.Varchar2).Value = model.BFIH_DN;
                        command.Parameters.Add("P_FTI_DN", OracleDbType.Varchar2).Value = "";
                        command.Parameters.Add("P_NOTE_DN", OracleDbType.Varchar2).Value = "";
                        command.Parameters.Add("P_FUNCTION", OracleDbType.Varchar2).Value = model.CHECK_BFIH_DN;

                        command.Parameters.Add("o_dt", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        command.Parameters.Add("RES", OracleDbType.Varchar2, 126).Direction = ParameterDirection.Output;

                        connection.Open();
                        await command.ExecuteNonQueryAsync();
                        procedureResult = command.Parameters["RES"].Value.ToString();

                        //try
                        //{
                        //    if (command.Parameters["o_dt"].Value is OracleRefCursor cursor)
                        //        using (var reader = cursor.GetDataReader())
                        //        {
                        //            while (await reader.ReadAsync())
                        //            {
                        //                var row = new Dictionary<string, object>();
                        //                for (int i = 0; i < reader.FieldCount; i++)
                        //                {
                        //                    row[reader.GetName(i)] = reader.GetValue(i);
                        //                }
                        //                cursorData.Add(row);
                        //            }
                        //        }

                        //}
                        //catch
                        //{
                        //    cursorData = new List<Dictionary<string, object>>();
                        //}
                        try
                        {
                            var reader = ((OracleRefCursor)command.Parameters["o_dt"].Value).GetDataReader();

                            while (await reader.ReadAsync())
                            {
                                var row = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row[reader.GetName(i)] = reader.GetValue(i);
                                }
                                cursorData.Add(row);
                            }
                        }
                        catch
                        {
                            cursorData = new List<Dictionary<string, object>>();
                        }

                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { result = procedureResult, data = cursorData });
                //if (procedureResult == "OK")
                //{
                //return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = cursorData });
                //}
                //else
                //{
                //    return Request.CreateResponse(HttpStatusCode.OK, new { result = "Error", data = procedureResult });
                //}
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }
        //Import
        [Route("ImportExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ImportExcel([FromBody] JObject model)
        {
            try
            {
                int error = 0;
                int total = 0;
                int exists = 0;
                int success = 0;
                List<string> errorList = new List<string>();
                string value = model["value"].ToString();
                string emp_no = model["EMP_NO"].ToString();
                string databaseName = model["database_name"].ToString();
                string databaseAp = model["databaseAp"].ToString();
                JArray data = (JArray)model["data"];

                string strPrivilege = $" SELECT emp FROM  SFIS1.C_PRIVILEGE  WHERE PRG_NAME='CONFIG' AND EMP='{emp_no}' AND FUN ='IMPORT_EXCEL'";

                if (DBConnect.GetData(strPrivilege, databaseName).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "This emp has no privilege" });
                }

                foreach (var item in data)
                {
                    try
                    {
                        switch (value)
                        {
                            case "UPDATERTRSN":
                                total++;
                                await HandleRTRSN(item, databaseName);
                                break;
                            case "UPDATERSNDETAIL":
                                total++;
                                await HandleRSNDETAIL(item, databaseName);
                                break;
                            case "WAIBAOTRCODE":
                                total++;
                                await HandleWAIBAOTRCODE(item, databaseAp);
                                break;
                            case "WAIBAOTRPRODUCT":
                                total++;
                                await HandleWAIBAOTRPRODUCT(item, databaseAp);
                                break;
                            case "WAIBAOTRSN":
                                total++;
                                await HandleWAIBAOTRSN(item, databaseAp);
                                break;
                            case "SPDM_SN":
                                total++;
                                await HandleSPDM_SN(item, databaseName);
                                break;
                            default:
                                exists++;
                                continue;
                        }
                        success++;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("EXISTS"))
                        {
                            exists++;
                        }
                        else
                        {
                            error++;
                            if (value == "UPDATERTRSN" || value == "UPDATERSNDETAIL"|| value == "SPDM_SN")
                            {
                                string serialNumber = item["SERIAL_NUMBER"]?.ToString();
                                if (string.IsNullOrEmpty(serialNumber))
                                {
                                    serialNumber += "SN IS NULL";
                                }
                                errorList.Add($"SerialNumber: {serialNumber} - Error: {ex.Message}");
                            }
                            else
                            {
                                errorList.Add($"Error: {ex.Message}");
                            }
                            Debug.WriteLine($"Error: {ex.Message}");
                            Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", total = total, exists = exists, success = success, error = error, errorList = errorList });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { result = ex.Message });
            }
        }
        private async Task HandleRTRSN(JToken item, string databaseName)
        {
            try
            {
                var moNumber = item["MO_NUMBER"]?.ToString();
                var serialNumber = item["SERIAL_NUMBER"]?.ToString();
                var trsn = item["TRSN"]?.ToString();
                var trCode = item["TRCODE"]?.ToString();
                var panelNo = item["PANEL_NO"]?.ToString();
                var linkTime = item["LINK_TIME"]?.ToString();
                var seqNo = item["SEQNO"]?.ToString();
                string checkQuery = "SELECT count(*) FROM SFISM4.R_SN_TRSN_LINK_T WHERE SERIAL_NUMBER = :serialNumber ";
                string query = $@" INSERT INTO SFISM4.R_SN_TRSN_LINK_T (MO_NUMBER, SERIAL_NUMBER, TRSN, TRCODE, PANEL_NO, LINK_TIME, SEQNO) 
                    VALUES (:moNumber, :serialNumber, :trsn, :trCode, :panelNo, TO_DATE(:linkTime, 'DD/MM/YYYY HH24:MI:SS'), :seqNo)";

                var connectionString = new GetString().Get()[databaseName];
                using (var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var checkCommand = new OracleCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.Add(new OracleParameter("serialNumber", serialNumber));
                        var count = (decimal)await checkCommand.ExecuteScalarAsync();
                        if (count > 0)
                        {
                            throw new Exception("EXISTS");
                        }
                    }

                    using (var command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("moNumber", moNumber));
                        command.Parameters.Add(new OracleParameter("serialNumber", serialNumber));
                        command.Parameters.Add(new OracleParameter("trsn", trsn));
                        command.Parameters.Add(new OracleParameter("trCode", trCode));
                        command.Parameters.Add(new OracleParameter("panelNo", panelNo));
                        command.Parameters.Add(new OracleParameter("linkTime", linkTime));
                        command.Parameters.Add(new OracleParameter("seqNo", seqNo));

                        if (string.IsNullOrEmpty(moNumber) || string.IsNullOrEmpty(serialNumber) || string.IsNullOrEmpty(trCode) ||
                            string.IsNullOrEmpty(panelNo) || string.IsNullOrEmpty(seqNo) || string.IsNullOrEmpty(trsn))
                        {
                            throw new ArgumentException("One or more required parameters are null or empty");
                        }
                        await command.ExecuteNonQueryAsync();
                        Debug.WriteLine($"Executed query: {query}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }
        private async Task HandleRSNDETAIL(JToken item, string databaseName)
        {
            try
            {
                var serialNumber = item["SERIAL_NUMBER"]?.ToString();
                var moNumber = item["MO_NUMBER"]?.ToString();
                var modelName = item["MODEL_NAME"]?.ToString();
                var versionCode = item["VERSION_CODE"]?.ToString();
                var lineName = item["LINE_NAME"]?.ToString();
                var groupName = item["GROUP_NAME"]?.ToString();
                var inStationTime = item["IN_STATION_TIME"]?.ToString();
                var inLineTime = item["IN_LINE_TIME"]?.ToString();
                var palletNo = item["PALLET_NO"]?.ToString();
                var cartonNo = item["CARTON_NO"]?.ToString();
                var trayNo = item["TRAY_NO"]?.ToString();
                var wipGroup = item["WIP_GROUP"]?.ToString();
                var shipNo = item["SHIP_NO"]?.ToString();

                string checkQuery = "SELECT count(*) FROM SFISM4.R_SN_DETAIL_T WHERE SERIAL_NUMBER = :serialNumber and GROUP_NAME = :groupName and IN_STATION_TIME = to_date(:inStationTime, 'DD/MM/YYYY HH24:MI:SS') ";
                string query = $@" INSERT INTO SFISM4.R_SN_DETAIL_T (
                    SERIAL_NUMBER, MO_NUMBER, MODEL_NAME, VERSION_CODE, LINE_NAME, GROUP_NAME, IN_STATION_TIME, IN_LINE_TIME, PALLET_NO, CARTON_NO, TRAY_NO, SHIP_NO, WIP_GROUP) 
                    VALUES (:serialNumber, :moNumber, :modelName, :versionCode, :lineName, :groupName, TO_DATE(:inStationTime, 'DD/MM/YYYY HH24:MI:SS'),
                     TO_DATE(:inLineTime, 'DD/MM/YYYY HH24:MI:SS'), :palletNo, :cartonNo, :trayNo, :shipNo, :wipGroup)";

                var connectionString = new GetString().Get()[databaseName];
                using (var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var checkCommand = new OracleCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.Add(new OracleParameter("serialNumber", serialNumber));
                        checkCommand.Parameters.Add(new OracleParameter("groupName", groupName));
                        checkCommand.Parameters.Add(new OracleParameter("inStationTime", inStationTime));
                        var count = (decimal)await checkCommand.ExecuteScalarAsync();
                        if (count > 0)
                        {
                            throw new Exception("EXISTS");
                        }
                    }
                    using (var command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("serialNumber", serialNumber));
                        command.Parameters.Add(new OracleParameter("moNumber", moNumber));
                        command.Parameters.Add(new OracleParameter("modelName", modelName));
                        command.Parameters.Add(new OracleParameter("versionCode", versionCode));
                        command.Parameters.Add(new OracleParameter("lineName", lineName));
                        command.Parameters.Add(new OracleParameter("groupName", groupName));
                        command.Parameters.Add(new OracleParameter("inStationTime", inStationTime));
                        command.Parameters.Add(new OracleParameter("inLineTime", inLineTime));
                        command.Parameters.Add(new OracleParameter("palletNo", palletNo));
                        command.Parameters.Add(new OracleParameter("cartonNo", cartonNo));
                        command.Parameters.Add(new OracleParameter("trayNo", trayNo));
                        command.Parameters.Add(new OracleParameter("shipNo", shipNo));
                        command.Parameters.Add(new OracleParameter("wipGroup", wipGroup));

                        if (string.IsNullOrEmpty(moNumber) || string.IsNullOrEmpty(serialNumber) || string.IsNullOrEmpty(inStationTime) ||
                        string.IsNullOrEmpty(groupName) || string.IsNullOrEmpty(modelName) || string.IsNullOrEmpty(wipGroup))
                        {
                            throw new ArgumentException("One or more required parameters are null or empty");
                        }
                        await command.ExecuteNonQueryAsync();
                        Debug.WriteLine($"Executed query: {query}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }
        //allpart
        private async Task HandleWAIBAOTRCODE(JToken item, string databaseAp)
        {
            try
            {
                var trCode = item["TR_CODE"]?.ToString();
                var station = item["STATION"]?.ToString();
                var stationFlag = item["STATION_FLAG"]?.ToString();
                var wo = item["WO"]?.ToString();
                var pNo = item["P_NO"]?.ToString();
                var slotNo = item["SLOT_NO"]?.ToString();
                var feederNo = item["FEEDER_NO"]?.ToString();
                var trSn = item["TR_SN"]?.ToString();
                var kpNo = item["KP_NO"]?.ToString();
                var mfrKpNo = item["MFR_KP_NO"]?.ToString();
                var mfrCode = item["MFR_CODE"]?.ToString();
                var dateCode = item["DATE_CODE"]?.ToString();
                var lotCode = item["LOT_CODE"]?.ToString();
                var processFlag = item["PROCESS_FLAG"]?.ToString();
                var workTime = item["WORK_TIME"]?.ToString();
                var data1 = item["DATA1"]?.ToString();
                var data2 = item["DATA2"]?.ToString();

                string query = $@" INSERT INTO MES4.R_WAIBAO_TR_CODE_DETAIL (
                    TR_CODE, STATION, STATION_FLAG, WO, P_NO, SLOT_NO, FEEDER_NO, TR_SN, KP_NO, MFR_KP_NO, MFR_CODE, DATE_CODE, LOT_CODE, PROCESS_FLAG, WORK_TIME, DATA1, DATA2) 
                    VALUES (:trCode, :station, :stationFlag, :wo, :pNo, :slotNo, :feederNo, :trSn, :kpNo, :mfrKpNo, :mfrCode, :dateCode, :lotCode, :processFlag, TO_DATE(:workTime, 'DD/MM/YYYY HH24:MI:SS'), :data1, :data2)";

                var connectionString = new GetString().Get()[databaseAp];
                using (var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("trCode", trCode));
                        command.Parameters.Add(new OracleParameter("station", station));
                        command.Parameters.Add(new OracleParameter("stationFlag", stationFlag));
                        command.Parameters.Add(new OracleParameter("wo", wo));
                        command.Parameters.Add(new OracleParameter("pNo", pNo));
                        command.Parameters.Add(new OracleParameter("slotNo", slotNo));
                        command.Parameters.Add(new OracleParameter("feederNo", feederNo));
                        command.Parameters.Add(new OracleParameter("trSn", trSn));
                        command.Parameters.Add(new OracleParameter("kpNo", kpNo));
                        command.Parameters.Add(new OracleParameter("mfrKpNo", mfrKpNo));
                        command.Parameters.Add(new OracleParameter("mfrCode", mfrCode));
                        command.Parameters.Add(new OracleParameter("dateCode", dateCode));
                        command.Parameters.Add(new OracleParameter("lotCode", lotCode));
                        command.Parameters.Add(new OracleParameter("processFlag", processFlag));
                        command.Parameters.Add(new OracleParameter("workTime", workTime));
                        command.Parameters.Add(new OracleParameter("data1", data1));
                        command.Parameters.Add(new OracleParameter("data2", data2));

                        await command.ExecuteNonQueryAsync();
                        Debug.WriteLine($"Executed query: {query}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }
        private async Task HandleWAIBAOTRPRODUCT(JToken item, string databaseAp)
        {
            try
            {
                var wo = item["WO"]?.ToString();
                var pSn = item["P_SN"]?.ToString();
                var trCode = item["TR_CODE"]?.ToString();
                var processFlag = item["PROCESS_FLAG"]?.ToString();
                var workTime = item["WORK_TIME"]?.ToString();
                var station = item["STATION"]?.ToString();
                var data1 = item["DATA1"]?.ToString();
                var data2 = item["DATA2"]?.ToString();
                var listTrsn = item["LIST_TRSN"]?.ToString();

                string query = $@" INSERT INTO MES4.R_WAIBAO_TR_PRODUCT_DETAIL (
                    WO, P_SN, TR_CODE, PROCESS_FLAG, WORK_TIME, STATION, DATA1, DATA2, LIST_TRSN) 
                    VALUES (:wo, :pSn, :trCode, :processFlag, TO_DATE(:workTime, 'DD/MM/YYYY HH24:MI:SS'), :station, :data1, :data2, :listTrsn)";

                var connectionString = new GetString().Get()[databaseAp];
                using (var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("wo", wo));
                        command.Parameters.Add(new OracleParameter("pSn", pSn));
                        command.Parameters.Add(new OracleParameter("trCode", trCode));
                        command.Parameters.Add(new OracleParameter("processFlag", processFlag));
                        command.Parameters.Add(new OracleParameter("workTime", workTime));
                        command.Parameters.Add(new OracleParameter("station", station));
                        command.Parameters.Add(new OracleParameter("data1", data1));
                        command.Parameters.Add(new OracleParameter("data2", data2));
                        command.Parameters.Add(new OracleParameter("listTrsn", listTrsn));

                        await command.ExecuteNonQueryAsync();
                        Debug.WriteLine($"Executed query: {query}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }
        private async Task HandleWAIBAOTRSN(JToken item, string databaseAp)
        {
            try
            {
                var trSn = item["TR_SN"]?.ToString();
                var outsideSn = item["OUTSIDE_SN"]?.ToString();
                var wo = item["WO"]?.ToString();
                var custKpNo = item["CUST_KP_NO"]?.ToString();
                var mfrKpNo = item["MFR_KP_NO"]?.ToString();
                var mfrCode = item["MFR_CODE"]?.ToString();
                var dateCode = item["DATE_CODE"]?.ToString();
                var lotCode = item["LOT_CODE"]?.ToString();
                var qty = item["QTY"]?.ToString();
                //
                var extQty = item["EXT_QTY"]?.ToString();
                var feederNo = item["FEEDER_NO"]?.ToString();
                var slotNo = item["SLOT_NO"]?.ToString();
                var startTime = item["START_TIME"]?.ToString();
                var endTime = item["END_TIME"]?.ToString();
                var station = item["STATION"]?.ToString();
                var data1 = item["DATA1"]?.ToString();
                var data2 = item["DATA2"]?.ToString();

                string query = $@" INSERT INTO MES4.R_WAIBAO_TR_SN_DETAIL (
                    TR_SN, OUTSIDE_SN, WO, CUST_KP_NO, MFR_KP_NO, MFR_CODE, DATE_CODE, LOT_CODE, QTY, EXT_QTY, FEEDER_NO, SLOT_NO, START_TIME, END_TIME, STATION, DATA1, DATA2) 
                    VALUES (:trSn, :outsideSn, :wo, :custKpNo, :mfrKpNo, :mfrCode, :dateCode, :lotCode, :qty, :extQty, :feederNo, :slotNo, TO_DATE(:startTime, 'DD/MM/YYYY HH24:MI:SS'), TO_DATE(:endTime, 'DD/MM/YYYY HH24:MI:SS'), :station, :data1, :data2)";

                var connectionString = new GetString().Get()[databaseAp];
                using (var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("trSn", trSn));
                        command.Parameters.Add(new OracleParameter("outsideSn", outsideSn));
                        command.Parameters.Add(new OracleParameter("wo", wo));
                        command.Parameters.Add(new OracleParameter("custKpNo", custKpNo));
                        command.Parameters.Add(new OracleParameter("mfrKpNo", mfrKpNo));
                        command.Parameters.Add(new OracleParameter("mfrCode", mfrCode));
                        command.Parameters.Add(new OracleParameter("dateCode", dateCode));
                        command.Parameters.Add(new OracleParameter("lotCode", lotCode));
                        command.Parameters.Add(new OracleParameter("qty", qty));
                        //
                        command.Parameters.Add(new OracleParameter("extQty", extQty));
                        command.Parameters.Add(new OracleParameter("feederNo", feederNo));
                        command.Parameters.Add(new OracleParameter("slotNo", slotNo));
                        command.Parameters.Add(new OracleParameter("startTime", startTime));
                        command.Parameters.Add(new OracleParameter("endTime", endTime));
                        command.Parameters.Add(new OracleParameter("station", station));
                        command.Parameters.Add(new OracleParameter("data1", data1));
                        command.Parameters.Add(new OracleParameter("data2", data2));

                        await command.ExecuteNonQueryAsync();
                        Debug.WriteLine($"Executed query: {query}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }
        private async Task HandleSPDM_SN(JToken item, string databaseAp)
        {
            try
            {
                var sn = item["SERIAL_NUMBER"]?.ToString();
                var ssn = item["SHIPPING_SN"]?.ToString();
                var model = item["MODEL_NAME"]?.ToString();
                var asicpart = item["ASIC_PART"]?.ToString();
                var board = item["BOARD"]?.ToString();
                var component = item["COMPONENT_REPLACED"]?.ToString();
                var datareplace = item["DATE_REPLACED"]?.ToString();
                var failed = item["FAILED_LOGFILE"]?.ToString();
                var firsttest = item["FIRST_TEST_FAILURE"]?.ToString();
                var nextstep = item["NEXT_STEP"]?.ToString();
                var serversign = item["SERVER_SIGN_TRANSACTIONID"]?.ToString();
                var factory = item["FACTORY_RESULT"]?.ToString();
                var data1 = item["DATA1"]?.ToString() == null ? "" : item["DATA1"]?.ToString();
                var data2 = item["DATA2"]?.ToString() == null ? "" : item["DATA2"]?.ToString();
                var data3 = item["DATA3"]?.ToString() == null ? "" : item["DATA3"]?.ToString();
                var data4 = item["DATA4"]?.ToString() == null ? "" : item["DATA4"]?.ToString();

                string query = $@" INSERT INTO sfism4.r_spdm_sn (Serial_number,shipping_sn,model_name,asic_part,board,component_replaced,date_replaced,
                    failed_logfile,first_test_failure,next_step,server_sign_transactionid,factory_result,data1,data2,data3,data4) 
                    VALUES (:serial_number, :shipping_sn, :model_name, :asic_part,:board, :component_replaced,TO_DATE(:date_replaced, 'DD/MM/YYYY HH24:MI:SS'),
                    :failed_logfile,:first_test_failure,:next_step,:server_sign_transactionid, :factory_result, :data1, :data2, :data3, :data4)";

                //string qwue1 = $@"INSERT INTO sfism4.r_spdm_sn (Serial_number,shipping_sn,model_name,asic_part,board,component_replaced,date_replaced,
                //    failed_logfile,first_test_failure,next_step,server_sign_transactionid,factory_result,data1,data2,data3,data4) 
                //    VALUES ('{sn}','{ssn}','{model}','{asicpart}','{board}','{component}',TO_DATE('{datareplace}', 'DD/MM/YYYY HH24:MI:SS'),'{failed}','{firsttest}','{nextstep}',
                //    '{serversign}','{factory}','{data1}','{data2}','{data3}','{data4}')";

                var connectionString = new GetString().Get()[databaseAp];
                using (var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("serial_number", sn));
                        command.Parameters.Add(new OracleParameter("shipping_sn", ssn));
                        command.Parameters.Add(new OracleParameter("model_name", model));
                        command.Parameters.Add(new OracleParameter("asic_part", asicpart));
                        command.Parameters.Add(new OracleParameter("board", board));
                        command.Parameters.Add(new OracleParameter("component_replaced", component));
                        command.Parameters.Add(new OracleParameter("date_replaced", datareplace));
                        command.Parameters.Add(new OracleParameter("failed_logfile", failed));
                        command.Parameters.Add(new OracleParameter("first_test_failure", firsttest));
                        command.Parameters.Add(new OracleParameter("next_step", nextstep));
                        command.Parameters.Add(new OracleParameter("server_sign_transactionid", serversign));
                        command.Parameters.Add(new OracleParameter("factory_result", factory));
                        command.Parameters.Add(new OracleParameter("data1", data1));
                        command.Parameters.Add(new OracleParameter("data2", data2));
                        command.Parameters.Add(new OracleParameter("data3", data3));
                        command.Parameters.Add(new OracleParameter("data4", data4));

                        await command.ExecuteNonQueryAsync();
                        Debug.WriteLine($"Executed query: {query}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }
    }
    public class UploadDataElement
    {
        public string database_name { get; set; }
        public string EMP_NO { get; set; }
        public string BFIH_DN { get; set; }
        public string CHECK_BFIH_DN { get; set; }
    }
}
