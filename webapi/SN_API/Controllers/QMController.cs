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
    public class QMController : ApiController
    {

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
        public string TimeToWorkSection(string time)
        {
            if (time == "24:00")
            {
                return "25";
            }
            else
            {
                var workSection = time.Replace(':', '.');
                return Math.Ceiling(Double.Parse(workSection)).ToString().PadLeft(2, '0');
            }
        }
        [System.Web.Http.Route("QCAnalysis")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> QCAnalysis(QMElement model)
        {
            if (model.listModel.Count == 0 && model.listMo.Count == 0 && model.listGroup.Count == 0 && model.listMo.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            StringBuilder sb = new StringBuilder();

            sb.Append(" select ");

            if (model.iLotNo)
            {
                sb.Append(" lot_no, ");
            }
            if (model.iModelName)
            {
                sb.Append(" model_name, ");
            }

            sb.Append(" po_number,tester,A.EMP_NAME tester_name, ");
            sb.Append(" decode(qa_result,'1','REJECT','0','PASS',qa_result) qa_result, sum(lot_size) lot_size, sum(pass_qty) pass_qty, sum(fail_qty) ");
            sb.Append(" fail_qty, sum(pass_qty+fail_qty) total_qty, sum(ng_num) \"REJECT_TIME\" ");
            sb.Append(" from sfism4.R_cqc_rec_t rr,SFIS1.C_EMP_DESC_T A ");
            sb.Append(" WHERE A.EMP_NO =  rr.tester (+) AND ");
            sb.Append($" to_char(start_time,'yyyymmddhh24:miss') >= '{model.dateFrom}' || '{model.timeFrom}' ");
            sb.Append($" AND to_char(start_time,'yyyymmddhh24:miss') < '{model.dateTo}' || '{model.timeTo}' ");
            sb.Append(GetSelectByList(model.listModel, "rr.model_name"));
            sb.Append(GetSelectByList(model.listMo, "rr.SSN"));
            sb.Append(" group by ");
            sb.Append("qa_result ");
            if (model.iLotNo)
            {
                sb.Append(" ,lot_no ");
            }
            if (model.iModelName)
            {
                sb.Append(" ,model_name ");
            }
            sb.Append(" ,lot_no,po_number,tester,A.EMP_NAME  ");
            sb.Append(" union all ");
            sb.Append(" select ");
            if (model.iLotNo)
            {
                sb.Append(" lot_no, ");
            }
            if (model.iModelName)
            {
                sb.Append(" model_name, ");
            }
            sb.Append(" po_number,tester,A.EMP_NAME tester_name, ");
            sb.Append(" decode(qa_result,'1','REJECT','0','PASS',qa_result) qa_result, sum(lot_size) lot_size, sum(pass_qty) pass_qty, sum(fail_qty) ");
            sb.Append(" fail_qty, sum(pass_qty+fail_qty) total_qty, sum(ng_num) \"REJECT_TIME\" ");
            sb.Append(" from sfism4.H_cqc_rec_t rr,SFIS1.C_EMP_DESC_T A ");
            sb.Append(" WHERE A.EMP_NO =  rr.tester(+) AND ");
            sb.Append($" to_char(start_time,'yyyymmddhh24:miss') >= '{model.dateFrom}' || '{model.timeFrom}' ");
            sb.Append($" AND to_char(start_time,'yyyymmddhh24:miss') < '{model.dateTo}' || '{model.timeTo}' ");
            sb.Append(GetSelectByList(model.listModel, "rr.model_name"));
            sb.Append(GetSelectByList(model.listMo, "rr.SSN"));
            sb.Append(GetSelectByList(model.listModel, "rr.model_name"));
            sb.Append(GetSelectByList(model.listMo, "rr.SSN"));
            sb.Append(" group by qa_result ");
            if (model.iLotNo)
            {
                sb.Append(" ,lot_no ");
            }
            if (model.iModelName)
            {
                sb.Append(" ,model_name ");
            }
            sb.Append(" ,po_number,tester,A.EMP_NAME ");
            sb.Append(" order by ");
            if (model.iLotNo)
            {
                sb.Append(" lot_no, ");
            }
            if (model.iModelName)
            {
                sb.Append(" model_name, ");
            }
            sb.Append(" PO_NUMBER,TESTER ");

            string queryString = sb.ToString();
            DataTable dtCheck = DBConnect.GetData(queryString, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                DataTable dtListSerialNumber = new DataTable();
                DataTable dtListSerialNumberAll = new DataTable();
                string SQL = "select serial_number, sum(decode(error_flag,1,1,0)) fail, sum(decode(error_flag,0,1,0)) pass, count(*) total " +
        "from sfism4.R_qc_sn_t s, " +
        "sfism4.R_cqc_rec_t c " +
        "where s.lot_no = c.lot_no " +
        $" and to_char(start_time,'yyyymmddhh24:miss') >= '{model.dateFrom}' || '{model.timeFrom}' " +
        $" AND to_char(start_time,'yyyymmddhh24:miss') < '{model.dateTo}' || '{model.timeTo}' ";

                SQL += GetSelectByList(model.listModel, "c.model_name");
                SQL += "group by serial_number " +
                 "union all " +
                 "select serial_number, sum(decode(error_flag,1,1,0)) fail, sum(decode(error_flag,0,1,0)) pass, count(*) total " +
                 "from sfism4.H_qc_sn_t s, " +
                 "sfism4.H_cqc_rec_t c " +
                 "where s.lot_no = c.lot_no " +
                 $"and to_char(start_time,'yyyymmddhh24:miss') >= '{model.dateFrom}' || '{model.timeFrom}' " +
                 $"AND to_char(start_time,'yyyymmddhh24:miss') < '{model.dateTo}' || '{model.timeTo}' ";
                SQL += GetSelectByList(model.listModel, "c.model_name");
                SQL += "group by serial_number " +
                 "order by Serial_Number ";
                dtListSerialNumberAll = DBConnect.GetData(SQL, model.database_name);

                string sqlAllSerial = "";
                if (model.qcSample == "lastSample")
                {
                    sqlAllSerial = "select serial_number, decode(error_flag,1,'FAIL','PASS') Status, Test_time,Tester " +
        "from sfism4.R_qc_sn_t r " +
        $"where lot_no = '{dtCheck.Rows[0]["LOT_NO"]}' and counter = 0 " +
        "union all " +
        "select serial_number, decode(error_flag,1,'FAIL','PASS') Status, Test_time,Tester " +
        "from sfism4.H_qc_sn_t r " +
        $"where lot_no = '{dtCheck.Rows[0]["LOT_NO"]}' and counter = 0 " +
        "Order by Serial_Number ";
                }
                else
                {
                    sqlAllSerial = "select serial_number, sum(decode(error_flag,1,1,0)) fail, sum(decode(error_flag,0,1,0)) pass, count(*) total " +
        "from sfism4.R_qc_sn_t r " +
        $" where lot_no = '{dtCheck.Rows[0]["LOT_NO"]}' " +
        "group by serial_number " +
        "union all " +
        "select serial_number, sum(decode(error_flag,1,1,0)) fail, sum(decode(error_flag,0,1,0)) pass, count(*) total " +
        "from sfism4.H_qc_sn_t r " +
        $" where lot_no = '{dtCheck.Rows[0]["LOT_NO"]}' and counter = 0 " +
        "group by serial_number " +
        "Order by Serial_Number ";

                }


                dtListSerialNumber = DBConnect.GetData(sqlAllSerial, model.database_name);

                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, data1 = dtListSerialNumber, data2 = dtListSerialNumberAll });
            }
        }
        [System.Web.Http.Route("GetDetailLocation")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetDetailLocation(QMElement model)
        {
            string reasonString = "";
            string SQL = "SELECT B.KP_NO,B.WO,B.STATION, D.MFR_NAME as VENDOR,B.DATE_CODE,B.LOT_CODE,COUNT(1) TOTAL,B.WORK_TIME ONLINE_TIME,B.LOCATION DESCR " +
        "  FROM (SELECT A.KP_NO,A.P_NO, " +
        "               DATE_CODE, " +
        "               STATION, " +
        "               TR_CODE, " +
        "               LOT_CODE, " +
        "               MFR_CODE, " +
        "               MFR_KP_NO, " +
        "               TR_SN,location, " +
        "               A.WO,a.work_time " +
        "          FROM MES4.R_TR_CODE_DETAIL@VNCPEAP A, MES1.C_SMT_AP_LOCATION@VNCPEAP B " +
        "         WHERE     1 = 1 " +
        "               AND A.SMT_CODE = B.SMT_CODE(+) " +
        "               AND A.KP_NO = B.KP_NO(+) " +
        "               AND A.STATION_FLAG = '1' " +
        "               AND A.P_NO IN (SELECT P_NO " +
        "                                FROM MES1.C_PRODUCT_CONFIG@VNCPEAP " +
        "                               WHERE 1 = 1)) B, " +
        "       MES4.R_TR_PRODUCT_DETAIL@VNCPEAP C, MES1.C_MFR_CONFIG@VNCPEAP D  " +
        " WHERE     C.TR_CODE = B.TR_CODE AND B.MFR_CODE = D.MFR_CODE  " +
        $"       AND C.WO = B.WO and b.location='{model.lastLotNo}'  " +
          GetSelectByList(model.listModel, "B.P_NO") +
        "       GROUP BY B.KP_NO,B.WO,B.STATION, D.MFR_NAME,B.DATE_CODE,B.LOT_CODE,B.WORK_TIME,B.LOCATION ";

            DataTable dtCheck = DBConnect.GetData(SQL, model.database_name);
            //if (dtCheck.Rows.Count != 0)
            //{
            //    reasonString = dtCheck.Rows[0]["REJECT_REASON"].ToString();
            //}
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }
        [System.Web.Http.Route("GetReasonQC")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetReasonQC(QMElement model)
        {
            string reasonString = "";
            string SQL = $" SELECT * FROM SFISM4.R_CQC_REC_T WHERE  LOT_NO = '{model.lastLotNo}' ";
            DataTable dtCheck = DBConnect.GetData(SQL, model.database_name);
            if (dtCheck.Rows.Count != 0)
            {
                reasonString = dtCheck.Rows[0]["REJECT_REASON"].ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", reason = reasonString });
        }
        [System.Web.Http.Route("ListQCDetailSN")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ListQCDetailSN(QMElement model)
        {
            string sqlAllSerial = "";
            if (model.qcSample == "lastSample")
            {
                sqlAllSerial = "select serial_number, decode(error_flag,1,'FAIL','PASS') Status, Test_time,Tester " +
    "from sfism4.R_qc_sn_t r " +
    $"where lot_no = '{model.lastLotNo}' and counter = 0 " +
    "union all " +
    "select serial_number, decode(error_flag,1,'FAIL','PASS') Status, Test_time,Tester " +
    "from sfism4.H_qc_sn_t r " +
    $"where lot_no = '{model.lastLotNo}' and counter = 0 " +
    "Order by Serial_Number ";
            }
            else
            {
                sqlAllSerial = "select serial_number, sum(decode(error_flag,1,1,0)) fail, sum(decode(error_flag,0,1,0)) pass, count(*) total " +
    "from sfism4.R_qc_sn_t r " +
    $" where lot_no = '{model.lastLotNo}' " +
    "group by serial_number " +
    "union all " +
    "select serial_number, sum(decode(error_flag,1,1,0)) fail, sum(decode(error_flag,0,1,0)) pass, count(*) total " +
    "from sfism4.H_qc_sn_t r " +
    $" where lot_no = '{model.lastLotNo}' and counter = 0 " +
    "group by serial_number " +
    "Order by Serial_Number ";

            }
            DataTable dtCheck = DBConnect.GetData(sqlAllSerial, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }
        [System.Web.Http.Route("ListSNFirstFail")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ListSNFirstFail(QMElement model)
        {
            if (model.listModel.Count == 0 && model.listMo.Count == 0 && model.listGroup.Count == 0 && model.listMo.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  MO_NUMBER, MODEL_NAME, SERIAL_NUMBER,LINE_NAME, GROUP_NAME, STATION_NAME, ERROR_CODE, RETEST_COUNT, IN_STATION_TIME  FROM SFISM4.R_FAIL_ATEDATA_T WHERE RETEST_COUNT = 1 ");
            sb.Append($" AND to_char(IN_STATION_TIME,'yyyymmddhh24:miss') >= '{model.dateFrom}' || '{model.timeFrom}' ");
            sb.Append($"  AND to_char(IN_STATION_TIME,'yyyymmddhh24:miss') < '{model.dateTo}' || '{model.timeTo}' ");
            sb.Append(GetSelectByList(model.listModel, " MODEL_NAME"));
            sb.Append(GetSelectByList(model.listMo, " MO_NUMBER "));
            sb.Append(GetSelectByList(model.listLine, " LINE_NAME "));
            sb.Append(GetSelectByList(model.listGroup, " GROUP_NAME "));
            sb.Append(" AND GROUP_NAME not in  (select group_name from sfis1.c_group_noroute_t) ");
            string queryString = sb.ToString();
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

        [System.Web.Http.Route("ListSNFirstFailQM")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ListSNFirstFailQM(QMElement model)
        {
            if (model.listModel.Count == 0 && model.listMo.Count == 0 && model.listGroup.Count == 0 && model.listMo.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  MO_NUMBER, MODEL_NAME, SERIAL_NUMBER,LINE_NAME, GROUP_NAME, STATION_NAME, ERROR_CODE, RETEST_COUNT, IN_STATION_TIME  FROM SFISM4.R_FIRST_FAIL_ATEDATA_T WHERE RETEST_COUNT = 1 ");
            sb.Append($" AND to_char(IN_STATION_TIME,'yyyymmddhh24:miss') >= '{model.dateFrom}' || '{model.timeFrom}' ");
            sb.Append($"  AND to_char(IN_STATION_TIME,'yyyymmddhh24:miss') < '{model.dateTo}' || '{model.timeTo}' ");
            sb.Append(GetSelectByList(model.listModel, " MODEL_NAME"));
            sb.Append(GetSelectByList(model.listMo, " MO_NUMBER "));
            sb.Append(GetSelectByList(model.listLine, " LINE_NAME "));
            sb.Append(GetSelectByList(model.listGroup, " GROUP_NAME "));
            sb.Append(" AND GROUP_NAME not in  (select group_name from sfis1.c_group_noroute_t) ");
            string queryString = sb.ToString();
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
        [System.Web.Http.Route("ListSNSecondFailQM")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ListSNSecondFailQM(QMElement model)
        {
            if (model.listModel.Count == 0 && model.listMo.Count == 0 && model.listGroup.Count == 0 && model.listMo.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  MO_NUMBER, MODEL_NAME, SERIAL_NUMBER,LINE_NAME, GROUP_NAME, STATION_NAME, ERROR_CODE, RETEST_COUNT, IN_STATION_TIME  FROM SFISM4.R_FIRST_FAIL_ATEDATA_T WHERE RETEST_COUNT = 2 ");
            sb.Append($" AND to_char(IN_STATION_TIME,'yyyymmddhh24:miss') >= '{model.dateFrom}' || '{model.timeFrom}' ");
            sb.Append($"  AND to_char(IN_STATION_TIME,'yyyymmddhh24:miss') < '{model.dateTo}' || '{model.timeTo}' ");
            sb.Append(GetSelectByList(model.listModel, " MODEL_NAME"));
            sb.Append(GetSelectByList(model.listMo, " MO_NUMBER "));
            sb.Append(GetSelectByList(model.listLine, " LINE_NAME "));
            sb.Append(GetSelectByList(model.listGroup, " GROUP_NAME "));
            sb.Append(" AND GROUP_NAME not in  (select group_name from sfis1.c_group_noroute_t) ");
            string queryString = sb.ToString();
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
        [System.Web.Http.Route("ListSNThirdFailQM")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ListSNThirdFailQM(QMElement model)
        {
            if (model.listModel.Count == 0 && model.listMo.Count == 0 && model.listGroup.Count == 0 && model.listMo.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  MO_NUMBER, MODEL_NAME, SERIAL_NUMBER,LINE_NAME, GROUP_NAME, STATION_NAME, ERROR_CODE, RETEST_COUNT, IN_STATION_TIME  FROM SFISM4.R_FIRST_FAIL_ATEDATA_T WHERE RETEST_COUNT = 3 ");
            sb.Append($" AND to_char(IN_STATION_TIME,'yyyymmddhh24:miss') >= '{model.dateFrom}' || '{model.timeFrom}' ");
            sb.Append($"  AND to_char(IN_STATION_TIME,'yyyymmddhh24:miss') < '{model.dateTo}' || '{model.timeTo}' ");
            sb.Append(GetSelectByList(model.listModel, " MODEL_NAME"));
            sb.Append(GetSelectByList(model.listMo, " MO_NUMBER "));
            sb.Append(GetSelectByList(model.listLine, " LINE_NAME "));
            sb.Append(GetSelectByList(model.listGroup, " GROUP_NAME "));
            sb.Append(" AND GROUP_NAME not in  (select group_name from sfis1.c_group_noroute_t) ");
            string queryString = sb.ToString();
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
        [System.Web.Http.Route("ListSNSecondFail")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ListSNSecondFail(QMElement model)
        {
            if (model.listModel.Count == 0 && model.listMo.Count == 0 && model.listGroup.Count == 0 && model.listMo.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  MO_NUMBER, MODEL_NAME, SERIAL_NUMBER,LINE_NAME, GROUP_NAME, STATION_NAME, ERROR_CODE, RETEST_COUNT, IN_STATION_TIME  FROM SFISM4.R_FAIL_ATEDATA_T WHERE RETEST_COUNT = 2 ");
            sb.Append($" AND to_char(IN_STATION_TIME,'yyyymmddhh24:miss') >= '{model.dateFrom}' || '{model.timeFrom}' ");
            sb.Append($"  AND to_char(IN_STATION_TIME,'yyyymmddhh24:miss') < '{model.dateTo}' || '{model.timeTo}' ");
            sb.Append(GetSelectByList(model.listModel, " MODEL_NAME"));
            sb.Append(GetSelectByList(model.listMo, " MO_NUMBER "));
            sb.Append(GetSelectByList(model.listLine, " LINE_NAME "));
            sb.Append(GetSelectByList(model.listGroup, " GROUP_NAME "));
            sb.Append(" AND GROUP_NAME not in  (select group_name from sfis1.c_group_noroute_t) ");
            string queryString = sb.ToString();
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


        [System.Web.Http.Route("RepairAnalysis")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> RepairAnalysis(QMElement model)
        {
            if (model.listModel.Count == 0 && model.listMo.Count == 0 && model.listGroup.Count == 0 && model.listMo.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            var sb = new StringBuilder();
            StringBuilder sbGroupBy = new StringBuilder();
            sbGroupBy.Append(" GROUP BY ");
            sb.Append(" SELECT ");
            if (model.iModelSerial)
            {
                sb.Append(" cm.model_serial, ");
            }
            if (model.iDate)
            {
                sb.Append(" to_char(test_time,'yyyymmdd') work_date, ");
                sbGroupBy.Append(" to_char(test_time,'yyyymmdd'), ");
            }
            if (model.iModelName)
            {
                sbGroupBy.Append(" rr.model_name, ");
                sb.Append(" rr.model_name, ");
            }
            if (model.iMoNumber)
            {
                sbGroupBy.Append(" rr.mo_number, ");
                sb.Append(" rr.mo_number, ");
            }
            if (model.iLine)
            {
                sbGroupBy.Append(" rr.test_line, ");
                sb.Append(" rr.test_line, ");
            }
            if (model.iGroup)
            {
                sbGroupBy.Append(" rr.TEST_GROUP, ");
                sb.Append(" rr.TEST_GROUP, ");
            }
            sb.Append(" UPPER (RR.TEST_CODE) test_code, ");
            sb.Append(" RR.EC_EXT, ");
            sb.Append(" CE.ERROR_DESC, ");
            sb.Append(" CR.REASON_DESC, ");
            sb.Append(" RR.ERROR_ITEM_CODE \"DESCR\", ");
            sb.Append(" CD.DUTY_DESC, ");
            sb.Append(" COUNT (*) QTY, ");
            sb.Append(" UPPER (RR.REASON_CODE) reason_code, ");
            sb.Append(" RR.ERROR_ITEM_CODE Location, ");
            sb.Append(" RR.DUTY_TYPE ");
            sb.Append(" FROM (SELECT * FROM SFISM4.R_REPAIR_T ");
            sb.Append(" UNION ALL ");
            sb.Append(" SELECT * FROM SFISM4.H_REPAIR_T) RR, ");
            sb.Append(" SFIS1.C_REASON_CODE_T CR, ");
            sb.Append(" SFIS1.C_ERROR_CODE_T CE, ");
            sb.Append(" SFIS1.C_DUTY_T CD ");
            if (model.iModelSerial)
            {
                sb.Append(" , sfis1.c_model_desc_t CM ");
            }
            sb.Append($" WHERE TO_CHAR (RR.Test_Time, 'yyyymmddhh24:miss') >= '{model.dateFrom}' || '{model.timeFrom}' ");
            sb.Append($" AND TO_CHAR (RR.Test_Time, 'yyyymmddhh24:miss') < '{model.dateTo}' || '{model.timeTo}' ");
            if (model.iModelSerial)
            {
                sb.Append(" AND CM.MODEL_NAME = RR.MODEL_NAME ");
            }
            sb.Append(GetSelectByList(model.listModel, "rr.model_name"));
            sb.Append(GetSelectByList(model.listMo, " rr.mo_number "));
            sb.Append(GetSelectByList(model.listLine, " rr.test_line "));
            sb.Append(GetSelectByList(model.listGroup, " rr.test_group "));


            sb.Append(" AND rr.Test_Group NOT IN (SELECT group_name FROM sfis1.c_group_noroute_t) ");
            sb.Append(" AND UPPER (RR.Duty_Type) = CD.Duty_Type(+) ");
            sb.Append(" AND UPPER (RR.TEST_CODE) = UPPER (CE.ERROR_CODE) ");
            sb.Append(" AND UPPER (RR.REASON_CODE) = UPPER (CR.REASON_CODE) ");

            sbGroupBy.Append(" RR.TEST_CODE, ");
            sbGroupBy.Append(" rr.EC_EXT, ");
            sbGroupBy.Append(" CE.ERROR_DESC, ");
            sbGroupBy.Append(" RR.REASON_CODE, ");
            sbGroupBy.Append(" CR.REASON_DESC, ");
            sbGroupBy.Append(" RR.ERROR_ITEM_CODE, ");
            sbGroupBy.Append(" RR.DUTY_TYPE, ");
            sbGroupBy.Append(" CD.DUTY_DESC ");
            if (model.iModelSerial)
            {
                sbGroupBy.Append(" , cm.model_serial ");
            }
            sb.Append(sbGroupBy.ToString());
            sb.Append(" ORDER BY rr.Test_Code, qty DESC ");

            var query = sb.ToString();

            DataTable dtCheck = DBConnect.GetData(query, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                DataTable dtListSerialNumber = new DataTable();
                DataTable dtListSerialNumberAll = new DataTable();
                int totalErrors = 0;
                string SQL = "SELECT RR.SERIAL_NUMBER, count(RR.SERIAL_NUMBER) qty " +
        "FROM (select * from SFISM4.R_REPAIR_T union all select * from SFISM4.H_REPAIR_T) RR " +
        $"WHERE to_char(RR.TEST_TIME,'yyyymmddhh24:miss') >= '{model.dateFrom}' || '{model.timeFrom}' " +
        $"AND to_char(RR.TEST_TIME,'yyyymmddhh24:miss') < '{model.dateTo}' || '{model.timeTo}' ";
                SQL += GetSelectByList(model.listModel, "rr.model_name");
                SQL += GetSelectByList(model.listMo, "rr.MO_NUMBER");
                SQL += GetSelectByList(model.listLine, "rr.TEST_LINE");
                SQL += GetSelectByList(model.listGroup, "rr.TEST_GROUP");

                SQL += $"AND upper(RR.TEST_CODE) = upper('{dtCheck.Rows[0]["TEST_CODE"]}') " +
        $" AND upper(RR.REASON_CODE) = upper('{dtCheck.Rows[0]["REASON_CODE"]}') " +
        $" AND (RR.ERROR_ITEM_CODE = '{dtCheck.Rows[0]["DESCR"]}' or rr.error_item_code is null) " +
        $" AND RR.DUTY_TYPE = '{dtCheck.Rows[0]["DUTY_TYPE"]}' " +
        " GROUP BY RR.SERIAL_NUMBER " +
        " ORDER BY RR.SERIAL_NUMBER ";
                dtListSerialNumber = DBConnect.GetData(SQL, model.database_name);

                string sqlAllSerial = " SELECT RR.SERIAL_NUMBER, count(RR.SERIAL_NUMBER) qty FROM (select * from SFISM4.R_REPAIR_T union all select * from SFISM4.H_REPAIR_T) RR " +
        $" WHERE to_char(RR.TEST_TIME,'yyyymmddhh24:miss') >= '{model.dateFrom}' || '{model.timeFrom}' " +
        $" AND to_char(RR.TEST_TIME,'yyyymmddhh24:miss') < '{model.dateTo}' || '{model.timeTo}' ";
                sqlAllSerial += GetSelectByList(model.listModel, "rr.model_name");
                sqlAllSerial += GetSelectByList(model.listMo, "rr.MO_NUMBER");
                sqlAllSerial += GetSelectByList(model.listLine, "rr.TEST_LINE");
                sqlAllSerial += GetSelectByList(model.listGroup, "rr.TEST_GROUP");
                sqlAllSerial += " And rr.Reason_Code is not null GROUP BY RR.SERIAL_NUMBER " +
        " ORDER BY qty desc ";
                dtListSerialNumberAll = DBConnect.GetData(sqlAllSerial, model.database_name);

                DataTable dtAllpart = new DataTable();
                DataTable dtLast = new DataTable();
                DataTable dtFinal = new DataTable();

                DataTable dtLeft = new DataTable();
                DataTable dtRight = new DataTable();
                DataTable resultLast = new DataTable();

                if (model.iShowAllpart)
                {
                    //neu chon Allpart
                    string sqlGetModel = "SELECT rr.model_name, " +
                "         RR.ERROR_ITEM_CODE, " +
                "         UPPER (RR.REASON_CODE) reason_code,COUNT(*) TOTAL " +
                "    FROM (SELECT * FROM SFISM4.R_REPAIR_T " +
                "          UNION ALL " +
                "          SELECT * FROM SFISM4.H_REPAIR_T) RR, " +
                "         SFIS1.C_REASON_CODE_T CR, " +
                "         SFIS1.C_ERROR_CODE_T CE, " +
                "         SFIS1.C_DUTY_T CD " +
                $"   WHERE     TO_CHAR (RR.Test_Time, 'yyyymmddhh24:miss') >='{model.dateFrom}' || '{model.timeFrom}' " +
                $"         AND TO_CHAR (RR.Test_Time, 'yyyymmddhh24:miss') < '{model.dateTo}' || '{model.timeTo}' " +
                $"         {GetSelectByList(model.listModel, "rr.model_name")} " +
                "         AND rr.Test_Group NOT IN (SELECT group_name " +
                "                                     FROM sfis1.c_group_noroute_t) " +
                "         AND UPPER (RR.Duty_Type) = CD.Duty_Type(+) " +
                "         AND UPPER (RR.TEST_CODE) = UPPER (CE.ERROR_CODE) " +
                "         AND UPPER (RR.REASON_CODE) = UPPER (CR.REASON_CODE)    " +
                "         AND UPPER (RR.REASON_CODE) = 'B000'          " +
                "GROUP BY rr.model_name, " +
                "         RR.REASON_CODE, " +
                "         RR.ERROR_ITEM_CODE " +
                "          ";
                    DataTable dtModel = DBConnect.GetData(sqlGetModel, model.database_name);

                    if (dtModel.Rows.Count != 0)
                    {
                        dtLeft.Columns.Add("MODEL_NAME", typeof(string));
                        dtLeft.Columns.Add("DESCR", typeof(string));
                        dtLeft.Columns.Add("TOTAL1", typeof(int));

                        foreach (DataRow dtRow in dtModel.Rows)
                        {
                            DataRow row = dtLeft.NewRow();
                            row["MODEL_NAME"] = dtRow["MODEL_NAME"];
                            row["DESCR"] = dtRow["ERROR_ITEM_CODE"];
                            row["TOTAL1"] = dtRow["TOTAL"];
                            dtLeft.Rows.Add(row);
                        }


                        dtRight.Columns.Add("VENDOR", typeof(string));
                        dtRight.Columns.Add("DESCR", typeof(string));
                        dtRight.Columns.Add("TOTAL", typeof(int));



                        string sqlGetDescr = "SELECT  distinct " +
        "         RR.ERROR_ITEM_CODE, " +
        "         UPPER (RR.REASON_CODE) " +
        "    FROM (SELECT * FROM SFISM4.R_REPAIR_T " +
        "          UNION ALL " +
        "          SELECT * FROM SFISM4.H_REPAIR_T) RR, " +
        "         SFIS1.C_REASON_CODE_T CR, " +
        "         SFIS1.C_ERROR_CODE_T CE, " +
        "         SFIS1.C_DUTY_T CD " +
        $"   WHERE     TO_CHAR (RR.Test_Time, 'yyyymmddhh24:miss') >='{model.dateFrom}' || '{model.timeFrom}' " +
        $"         AND TO_CHAR (RR.Test_Time, 'yyyymmddhh24:miss') < '{model.dateTo}' || '{model.timeTo}' " +
        $"         {GetSelectByList(model.listModel, "rr.model_name")} " +
        "         AND rr.Test_Group NOT IN (SELECT group_name " +
        "                                     FROM sfis1.c_group_noroute_t) " +
        "         AND UPPER (RR.Duty_Type) = CD.Duty_Type(+) " +
        "         AND UPPER (RR.TEST_CODE) = UPPER (CE.ERROR_CODE) " +
        "         AND UPPER (RR.REASON_CODE) = UPPER (CR.REASON_CODE)    " +
        "         AND UPPER (RR.REASON_CODE) = 'B000' ";
                        DataTable dtDescr = DBConnect.GetData(sqlGetDescr, model.database_name);

                        StringBuilder sbDescr = new StringBuilder();
                        sbDescr.Append(" (");

                        for (int i = 0; i < dtDescr.Rows.Count; i++)
                        {
                            string valueData = dtDescr.Rows[i]["ERROR_ITEM_CODE"].ToString();
                            sbDescr.AppendFormat("'{0}'", valueData);
                            if (i < dtDescr.Rows.Count - 1)
                            {
                                sbDescr.Append(",");
                            }
                        }
                        sbDescr.Append(")");

                        string strGetFromAllpart = "SELECT D.MFR_NAME AS VENDOR,B.LOCATION DESCR,COUNT(1) TOTAL " +
         "  FROM (SELECT A.KP_NO,A.P_NO, " +
         "               DATE_CODE, " +
         "               STATION, " +
         "               TR_CODE, " +
         "               LOT_CODE, " +
         "               MFR_CODE, " +
         "               MFR_KP_NO, " +
         "               TR_SN,location, " +
         "               A.WO,a.work_time " +
         "          FROM MES4.R_TR_CODE_DETAIL@VNCPEAP A, MES1.C_SMT_AP_LOCATION@VNCPEAP B " +
         "         WHERE     1 = 1 " +
         "               AND A.SMT_CODE = B.SMT_CODE(+) " +
         "               AND A.KP_NO = B.KP_NO(+) " +
         "               AND A.STATION_FLAG = '1' " +
         "               AND A.P_NO IN (SELECT P_NO " +
         "                                FROM MES1.C_PRODUCT_CONFIG@VNCPEAP " +
         "                               WHERE 1 = 1)) B, " +
         "       MES4.R_TR_PRODUCT_DETAIL@VNCPEAP C, MES1.C_MFR_CONFIG@VNCPEAP D " +
         " WHERE     C.TR_CODE = B.TR_CODE AND B.MFR_CODE = D.MFR_CODE " +
         "       AND C.WO = B.WO    " +
          $"         {GetSelectByList(model.listModel, "B.P_NO")} " +
         $"       and B.LOCATION in {sbDescr} " +
         "       GROUP BY  D.MFR_NAME,B.LOCATION ";
                        dtLast = DBConnect.GetData(strGetFromAllpart, model.database_name);

                        foreach (DataRow dtRow in dtLast.Rows)
                        {
                            DataRow row = dtRight.NewRow();
                            row["VENDOR"] = dtRow["VENDOR"];
                            row["DESCR"] = dtRow["DESCR"];
                            row["TOTAL"] = dtRow["TOTAL"];
                            dtRight.Rows.Add(row);
                        }

                        // var resultLast =  dtLeft.AsEnumerable().Join
                        // resultLast = JoinDataTables(dtLeft, dtRight, (row1, row2) => row1.Field<string>("DESCR") == row1.Field<string>("DESCR"));
                        resultLast = JoinTwoDataTablesOnOneColumn(dtLeft, dtRight, "DESCR", JoinType.Inner);
                        //DataTable dtt = resultLast;
                        DataTable dtLastResult = new DataTable();

                        dtLastResult.Columns.Add("MODEL_NAME", typeof(string));
                        dtLastResult.Columns.Add("DESCR", typeof(string));
                        dtLastResult.Columns.Add("VENDOR", typeof(string));
                        dtLastResult.Columns.Add("FAIL_QTY", typeof(string));
                        dtLastResult.Columns.Add("INPUT_QTY", typeof(string));
                        dtLastResult.Columns.Add("DPPM", typeof(string));

                        foreach (DataRow dtRow in resultLast.Rows)
                        {
                            DataRow row = dtLastResult.NewRow();
                            row["MODEL_NAME"] = dtRow["MODEL_NAME"];
                            row["DESCR"] = dtRow["DESCR"];
                            row["VENDOR"] = dtRow["VENDOR"];
                            row["FAIL_QTY"] = dtRow["TOTAL1"];
                            row["INPUT_QTY"] = dtRow["TOTAL"];
                            row["DPPM"] = ((float.Parse(dtRow["TOTAL1"].ToString()) / float.Parse(dtRow["TOTAL"].ToString())) * 1000000) + "";
                            dtLastResult.Rows.Add(row);
                        }
                        dtLastResult.DefaultView.Sort = "MODEL_NAME,DESCR ASC";

                        dtCheck = dtLastResult.DefaultView.ToTable();
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, data1 = dtListSerialNumber, data2 = dtListSerialNumberAll, totalError = totalErrors });
            }
        }

        public enum JoinType
        {
            Inner = 0,
            Left = 1
        }


        public static DataTable JoinTwoDataTablesOnOneColumn(DataTable dtblLeft, DataTable dtblRight, string colToJoinOn, JoinType joinType)
        {
            //Change column name to a temp name so the LINQ for getting row data will work properly.
            string strTempColName = colToJoinOn + "_2";
            if (dtblRight.Columns.Contains(colToJoinOn))
                dtblRight.Columns[colToJoinOn].ColumnName = strTempColName;

            //Get columns from dtblLeft
            DataTable dtblResult = dtblLeft.Clone();

            //Get columns from dtblRight
            var dt2Columns = dtblRight.Columns.OfType<DataColumn>().Select(dc => new DataColumn(dc.ColumnName, dc.DataType, dc.Expression, dc.ColumnMapping));

            //Get columns from dtblRight that are not in dtblLeft
            var dt2FinalColumns = from dc in dt2Columns.AsEnumerable()
                                  where !dtblResult.Columns.Contains(dc.ColumnName)
                                  select dc;

            //Add the rest of the columns to dtblResult
            dtblResult.Columns.AddRange(dt2FinalColumns.ToArray());

            //No reason to continue if the colToJoinOn does not exist in both DataTables.
            if (!dtblLeft.Columns.Contains(colToJoinOn) || (!dtblRight.Columns.Contains(colToJoinOn) && !dtblRight.Columns.Contains(strTempColName)))
            {
                if (!dtblResult.Columns.Contains(colToJoinOn))
                    dtblResult.Columns.Add(colToJoinOn);
                return dtblResult;
            }

            switch (joinType)
            {

                default:
                case JoinType.Inner:
                    #region Inner
                    //get row data
                    //To use the DataTable.AsEnumerable() extension method you need to add a reference to the System.Data.DataSetExtension assembly in your project. 
                    var rowDataLeftInner = from rowLeft in dtblLeft.AsEnumerable()
                                           join rowRight in dtblRight.AsEnumerable() on rowLeft[colToJoinOn] equals rowRight[strTempColName]
                                           select rowLeft.ItemArray.Concat(rowRight.ItemArray).ToArray();


                    //Add row data to dtblResult
                    foreach (object[] values in rowDataLeftInner)
                        dtblResult.Rows.Add(values);

                    #endregion
                    break;
                case JoinType.Left:
                    #region Left
                    var rowDataLeftOuter = from rowLeft in dtblLeft.AsEnumerable()
                                           join rowRight in dtblRight.AsEnumerable() on rowLeft[colToJoinOn] equals rowRight[strTempColName] into gj
                                           from subRight in gj.DefaultIfEmpty()
                                           select rowLeft.ItemArray.Concat((subRight == null) ? (dtblRight.NewRow().ItemArray) : subRight.ItemArray).ToArray();


                    //Add row data to dtblResult
                    foreach (object[] values in rowDataLeftOuter)
                        dtblResult.Rows.Add(values);

                    #endregion
                    break;
            }

            //Change column name back to original
            dtblRight.Columns[strTempColName].ColumnName = colToJoinOn;

            //Remove extra column from result
            dtblResult.Columns.Remove(strTempColName);

            return dtblResult;
        }

        private DataTable JoinDataTables(DataTable t1, DataTable t2, params Func<DataRow, DataRow, bool>[] joinOn)
        {
            DataTable result = new DataTable();
            foreach (DataColumn col in t1.Columns)
            {
                if (result.Columns[col.ColumnName] == null)
                    result.Columns.Add(col.ColumnName, col.DataType);
            }
            foreach (DataColumn col in t2.Columns)
            {
                if (result.Columns[col.ColumnName] == null)
                    result.Columns.Add(col.ColumnName, col.DataType);
            }
            foreach (DataRow row1 in t1.Rows)
            {
                var joinRows = t2.AsEnumerable().Where(row2 =>
                {
                    foreach (var parameter in joinOn)
                    {
                        if (!parameter(row1, row2)) return false;
                    }
                    return true;
                });
                foreach (DataRow fromRow in joinRows)
                {
                    DataRow insertRow = result.NewRow();
                    foreach (DataColumn col1 in t1.Columns)
                    {
                        insertRow[col1.ColumnName] = row1[col1.ColumnName];
                    }
                    foreach (DataColumn col2 in t2.Columns)
                    {
                        insertRow[col2.ColumnName] = fromRow[col2.ColumnName];
                    }
                    result.Rows.Add(insertRow);
                }
            }
            return result;
        }

        [System.Web.Http.Route("DefectAnalysis")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DefectAnalysis(QMElement model)
        {
            if (model.listModel.Count == 0 && model.listMo.Count == 0 && model.listGroup.Count == 0 && model.listMo.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            StringBuilder sb = new StringBuilder();

            StringBuilder sbGroupBy = new StringBuilder();
            StringBuilder sbOrderBy = new StringBuilder();

            sbGroupBy.Append(" GROUP BY  ");

            if (model.iTestCode || model.defectType == 1)
            {
                sbGroupBy.Append(" rr.Test_Code ");
            }

            sbOrderBy.Append(" ORDER BY qty desc ");

            sb.Append(" select ");

            if (model.iDate)
            {
                sb.Append(" to_char(test_time,'yyyymmdd') work_date, ");
                sbGroupBy.Append(" ,to_char(test_time,'yyyymmdd') ");
                sbOrderBy.Append(" ,work_date ");
            }
            if (model.iModelSerial)
            {
                sb.Append(" cm.MODEL_SERIAL, ");
                sbOrderBy.Append(" ,cm.MODEL_SERIAL  ");
                sbGroupBy.Append(" ,cm.MODEL_SERIAL  ");
            }
            if (model.iLine)
            {
                sb.Append(" rr.test_line, ");
                sbOrderBy.Append(" ,test_line  ");
                sbGroupBy.Append(" ,rr.test_line  ");
            }
            if (model.iSerialNumber)
            {
                sb.Append(" rr.serial_number, ");
                sbOrderBy.Append("  ");
                sbGroupBy.Append(" ,rr.serial_number  ");
            }
            if (model.iModelName)
            {
                sb.Append(" rr.model_name,  ");
                sbOrderBy.Append(",rr.model_name  ");
                sbGroupBy.Append(" ,rr.model_name  ");
            }
            if (model.iMoNumber)
            {
                sb.Append(" rr.mo_number,  ");
                sbOrderBy.Append(" ,rr.mo_number ");
                sbGroupBy.Append(" ,rr.mo_number  ");
            }
            if (model.iLocationCode)
            {
                sb.Append(" rr.ERROR_ITEM_CODE, ");
                sbGroupBy.Append(" ,rr.ERROR_ITEM_CODE ");
            }

            sbOrderBy.Append(" ,qty desc ");

            if (model.iSection)
            {
                sb.Append(" rr.Test_section section_name, ");
                sbOrderBy.Append("  ");
                sbGroupBy.Append(" ,rr.Test_section  ");
            }
            if (model.iGroup)
            {
                sb.Append(" rr.Test_group group_name,  ");
                sbOrderBy.Append("  ");
                sbGroupBy.Append(" ,rr.Test_group  ");
            }

            if (model.defectType == 4)
            {
                sb.Append(" RR.Reason_Code, c.Reason_Desc,  ");
                sbOrderBy.Append(" , rr.Reason_Code ");
                if (model.iTestCode)
                {
                    sbGroupBy.Append(",");
                }
                sbGroupBy.Append(" rr.Reason_Code,c.Reason_Desc ");
            }
            else if (model.defectType == 5)
            {
                sb.Append(" RR.Duty_Type, c.Duty_Desc,  ");
                sbGroupBy.Append(" ,rr.Duty_Type, c.Duty_Desc ");
                sbOrderBy.Append(" , rr.Duty_Type ");
            }
            else if (model.defectType == 6)
            {
                sb.Append(" c.Item_Name,  ");
                sbOrderBy.Append(" ,c.Item_Name ");
            }
            else
            {
                if (model.iTestCode)
                {
                    sb.Append(" RR.Test_Code,c.ERROR_DESC,  ");
                    sbOrderBy.Append(" ,rr.Test_Code ");
                    sbGroupBy.Append(" ,c.ERROR_DESC ");
                }
            }

            string tableSub = " SFIS1.C_ERROR_CODE_T ";

            if (model.defectType == 4)
            {
                tableSub = " Sfis1.c_Reason_Code_t ";
            }
            else if (model.defectType == 5)
            {
                tableSub = " Sfis1.c_Duty_t ";
            }
            else if (model.defectType == 6)
            {
                tableSub = " Sfis1.c_Item_Desc_t ";
            }
            sb.Append(" count(RR.SERIAL_NUMBER) QTY");
            sb.Append($" FROM (select * from SFISM4.R_REPAIR_T union all select * from SFISM4.H_REPAIR_T union all select * from SFISM4.R_REPAIR_T_BAK ) RR, {tableSub} C ");

            if (model.iModelName || model.iModelSerial)
            {
                sb.Append(" ,sfis1.C_MODEL_DESC_T cm ");
            }
            sb.Append($" WHERE to_char(RR.TEST_TIME,'yyyymmddhh24mi') >= '{model.dateFrom}' || '{model.timeFrom}' ");
            sb.Append($" AND to_char(RR.TEST_TIME,'yyyymmddhh24mi') < '{model.dateTo}' || '{model.timeTo}' ");
            if (model.iModelName || model.iModelSerial)
            {
                sb.Append(" AND RR.MODEL_NAME = cm.model_name ");
            }

            sb.Append(GetSelectByList(model.listModel, "rr.model_name"));
            sb.Append(GetSelectByList(model.listMo, "rr.MO_NUMBER"));
            sb.Append(GetSelectByList(model.listLine, "rr.TEST_LINE"));
            sb.Append(GetSelectByList(model.listGroup, "rr.TEST_GROUP"));

            if (model.defectType == 1 || model.defectType == 2 || model.defectType == 3)
            {
                sb.Append(" AND RR.Test_Code= c.ERROR_CODE(+) ");
            }
            else if (model.defectType == 4)
            {
                sb.Append(" AND RR.Reason_Code= c.Reason_Code(+) ");
            }
            else if (model.defectType == 5)
            {
                sb.Append(" AND RR.Duty_Type= c.Duty_type(+) ");
                sbGroupBy.Append(" ,rr.Duty_Type , c.Duty_Desc ");
            }
            else if (model.defectType == 6)
            {
                sb.Append(" AND RR.Error_Item_Code= c.Item_Code(+) AND (RR.MODEL_NAME = C.MODEL_NAME OR RR.MODEL_NAME = 'unknown' OR upper(C.MODEL_NAME) = 'DEFAULT') ");
            }

            sb.Append(" AND rr.Test_Group not in  (select group_name from sfis1.c_group_noroute_t) ");
            if (model.defectType == 2)
            {
                sb.Append(" and rr.Reason_Code is not null ");
            }
            else if (model.defectType == 3)
            {
                sb.Append(" and rr.Reason_Code is null ");
            }


            sb.Append(sbGroupBy);
            sb.Append(sbOrderBy);

            var query = sb.ToString();

            DataTable dtCheck = DBConnect.GetData(query, model.database_name);

            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            int total = 0;
            for (int i = 0; i < dtCheck.Rows.Count; i++)
            {
                total += int.Parse(dtCheck.Rows[i]["QTY"].ToString());
            }
            sb.Clear();
            sb.Append(" select ");

            if (model.iDate)
            {
                sb.Append(" to_char(test_time,'yyyymmdd') work_date, ");
            }
            if (model.iLine)
            {
                sb.Append(" rr.test_line, ");
            }
            if (model.iModelSerial)
            {
                sb.Append(" cm.model_serial, ");
            }
            if (model.iSerialNumber)
            {
                sb.Append(" rr.serial_number, ");
            }
            if (model.iModelName)
            {
                sb.Append(" rr.model_name,  ");
            }
            if (model.iMoNumber)
            {
                sb.Append(" rr.mo_number,  ");
            }
            if (model.iLocationCode)
            {
                sb.Append(" rr.ERROR_ITEM_CODE Location, ");
                //sbGroupBy.Append(" ,rr.ERROR_ITEM_CODE ");
            }
            //sb.Append(" rr.ERROR_ITEM_CODE, ");            

            if (model.iSection)
            {
                sb.Append(" rr.Test_section section_name, ");
            }
            if (model.iGroup)
            {
                sb.Append(" rr.Test_group group_name,  ");
            }
            if (model.defectType == 4)
            {
                sb.Append(" RR.Reason_Code, c.Reason_Desc,  ");
            }
            else if (model.defectType == 5)
            {
                sb.Append(" RR.Duty_Type, c.Duty_Desc,  ");
                sbGroupBy.Append(" ,rr.Duty_Type, c.Duty_Desc ");
                sbOrderBy.Append(" , rr.Duty_Type ");
            }
            else if (model.defectType == 6)
            {
                if (model.iTestCode)
                {
                    sb.Append(" RR.Test_Code,  ");
                    sbGroupBy.Append(" rr.test_code, ");
                }
                sb.Append(" ,c.Item_Name  ");
                sbOrderBy.Append(" ,c.Item_Name ");
            }
            else
            {
                if (model.iTestCode)
                {
                    sb.Append(" RR.Test_Code,c.ERROR_DESC,  ");
                }
            }

            sb.Append(" count(RR.SERIAL_NUMBER) QTY,");
            sb.Append($"to_char(100*count(*)/{total},'990.00') \"Rate\"");
            sb.Append($" FROM (select * from SFISM4.R_REPAIR_T union all select * from SFISM4.H_REPAIR_T union all select * from SFISM4.R_REPAIR_T_BAK ) RR, {tableSub} C ");
            if (model.iModelName || model.iModelSerial)
            {
                sb.Append(" ,sfis1.C_MODEL_DESC_T cm ");
            }
            sb.Append($" WHERE to_char(RR.TEST_TIME,'yyyymmddhh24miss') >= '{model.dateFrom}' || '{model.timeFrom}' ");
            if (model.iModelName || model.iModelSerial)
            {
                sb.Append(" AND RR.MODEL_NAME = cm.model_name ");
            }
            sb.Append($" AND to_char(RR.TEST_TIME,'yyyymmddhh24miss') < '{model.dateTo}' || '{model.timeTo}' ");
            //sb.Append(" and rr.model_name = cm.model_name(+) ");
            if (model.listModel.Count > 0)
            {
                var sbmodel_name = new StringBuilder();
                sbmodel_name.Append(" IN(");

                for (int i = 0; i < model.listModel.Count; i++)

                {
                    string mo = model.listModel[i].VALUE.ToString();

                    sbmodel_name.AppendFormat("'{0}'", mo);
                    if (i < model.listModel.Count - 1)
                    {
                        sbmodel_name.Append(",");
                    }
                }
                sbmodel_name.Append(")");

                sb.AppendFormat(" AND rr.model_name {0}", sbmodel_name.ToString());
            }
            if (model.listMo.Count > 0)
            {
                var sbMoNumbers = new StringBuilder();
                sbMoNumbers.Append(" IN(");

                for (int i = 0; i < model.listMo.Count; i++)

                {
                    string mo = model.listMo[i].VALUE.ToString();

                    sbMoNumbers.AppendFormat("'{0}'", mo);
                    if (i < model.listMo.Count - 1)
                    {
                        sbMoNumbers.Append(",");
                    }
                }
                sbMoNumbers.Append(")");

                sb.AppendFormat(" AND rr.MO_NUMBER {0}", sbMoNumbers.ToString());
            }
            if (model.listLine.Count > 0)
            {
                var sbLineNames = new StringBuilder();
                sbLineNames.Append(" IN(");

                for (int i = 0; i < model.listLine.Count; i++)

                {
                    string line = model.listLine[i].VALUE.ToString();

                    sbLineNames.AppendFormat("'{0}'", line);
                    if (i < model.listLine.Count - 1)
                    {
                        sbLineNames.Append(",");
                    }
                }
                sbLineNames.Append(")");

                sb.AppendFormat(" AND RR.TEST_LINE {0}", sbLineNames.ToString());
            }
            if (model.listGroup.Count > 0)
            {
                var sbGroupNames = new StringBuilder();
                sbGroupNames.Append(" IN(");

                for (int i = 0; i < model.listGroup.Count; i++)

                {
                    string group = model.listGroup[i].VALUE.ToString();

                    sbGroupNames.AppendFormat("'{0}'", group);
                    if (i < model.listGroup.Count - 1)
                    {
                        sbGroupNames.Append(",");
                    }
                }
                sbGroupNames.Append(")");

                sb.AppendFormat(" AND RR.TEST_GROUP {0}", sbGroupNames.ToString());
            }

            sb.Append(" AND rr.Test_Group not in  (select group_name from sfis1.c_group_noroute_t) ");

            if (model.defectType == 1 || model.defectType == 2 || model.defectType == 3)
            {
                sb.Append(" AND RR.Test_Code= c.ERROR_CODE(+) ");
            }
            else if (model.defectType == 4)
            {
                sb.Append(" AND RR.Reason_Code= c.Reason_Code(+) ");
            }
            else if (model.defectType == 5)
            {
                sb.Append(" AND RR.Duty_Type= c.Duty_type(+) ");
            }
            else if (model.defectType == 6)
            {
                sb.Append(" AND RR.Error_Item_Code= c.Item_Code(+) AND (RR.MODEL_NAME = C.MODEL_NAME OR RR.MODEL_NAME = 'unknown' OR upper(C.MODEL_NAME) = 'DEFAULT') ");
            }

            if (model.defectType == 2)
            {
                sb.Append(" and rr.Reason_Code is not null ");
            }
            else if (model.defectType == 3)
            {
                sb.Append(" and rr.Reason_Code is null ");
            }

            sb.Append(sbGroupBy);
            sb.Append(sbOrderBy);
            query = sb.ToString();
            dtCheck = DBConnect.GetData(query, model.database_name);
            DataTable dtListSerialNumber = new DataTable();
            DataTable dtListSerialNumberAll = new DataTable();

            int totalErrors = 0;

            if (dtCheck.Rows.Count != 0)
            {
                try
                {
                    string SQL = "SELECT RR.SERIAL_NUMBER,  " +
       "RR.TEST_CODE, " +
       " count(RR.SERIAL_NUMBER) qty  " +
       "FROM (select * from SFISM4.R_REPAIR_T union all select * from SFISM4.H_REPAIR_T union all select * from SFISM4.R_REPAIR_T_BAK ) RR " +
       $"WHERE to_char(RR.TEST_TIME,'yyyymmddhh24mi') >= '{model.dateFrom}' || '{model.timeFrom}' " +
       $"AND to_char(RR.TEST_TIME,'yyyymmddhh24mi') < '{model.dateTo}' || '{model.timeTo}' ";

                    SQL += GetSelectByList(model.listModel, "rr.model_name");
                    SQL += GetSelectByList(model.listMo, "rr.MO_Number");
                    SQL += GetSelectByList(model.listGroup, "rr.Test_Group");
                    SQL += GetSelectByList(model.listLine, "rr.test_line");
                    if (model.defectType == 3)
                    {
                        SQL += " and rr.Reason_Code is null ";
                    }
                    else if (model.defectType == 2 || model.defectType == 4)
                    {
                        SQL += "  And rr.Reason_Code is not null ";
                    }
                    SQL += $"AND upper(RR.Test_Code) = upper('{dtCheck.Rows[0]["Test_Code"]}') " +
                     "GROUP BY RR.SERIAL_NUMBER " +
                     ",RR.TEST_CODE  " +
                     " ORDER BY RR.SERIAL_NUMBER, qty ";
                    dtListSerialNumber = DBConnect.GetData(SQL, model.database_name);

                    string sqlAllSerial = "SELECT RR.SERIAL_NUMBER, count(RR.SERIAL_NUMBER) qty FROM (select * from SFISM4.R_REPAIR_T union all select * from SFISM4.H_REPAIR_T union all select * from SFISM4.R_REPAIR_T_BAK ) RR " +
            $"WHERE to_char(RR.TEST_TIME,'yyyymmddhh24mi') >= '{model.dateFrom}' || '{model.timeFrom}' " +
            $"AND to_char(RR.TEST_TIME,'yyyymmddhh24mi') < '{model.dateTo}' || '{model.timeTo}' ";
                    sqlAllSerial += GetSelectByList(model.listModel, "rr.model_name");
                    sqlAllSerial += GetSelectByList(model.listMo, "rr.MO_Number");
                    sqlAllSerial += GetSelectByList(model.listGroup, "rr.Test_Group");
                    sqlAllSerial += GetSelectByList(model.listLine, "rr.test_line");
                    if (model.defectType == 3)
                    {
                        sqlAllSerial += " and rr.Reason_Code is null ";
                    }
                    else if (model.defectType == 2 || model.defectType == 4)
                    {
                        sqlAllSerial += "  And rr.Reason_Code is not null ";
                    }
                    sqlAllSerial += " GROUP BY RR.SERIAL_NUMBER " +
            "ORDER BY qty desc ";
                    dtListSerialNumberAll = DBConnect.GetData(sqlAllSerial, model.database_name);
                }
                catch
                {

                }


            }
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, data1 = dtListSerialNumber, data2 = dtListSerialNumberAll, totalError = totalErrors });

        }

        [System.Web.Http.Route("GetDefectDetailSN")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetDefectDetailSN(QMElement model)
        {
            DataTable dtListSerialNumber = new DataTable();
            string SQL = "SELECT RR.SERIAL_NUMBER,  " +
        "RR.TEST_CODE, " +
        " count(RR.SERIAL_NUMBER) qty  " +
        "FROM (select * from SFISM4.R_REPAIR_T union all select * from SFISM4.H_REPAIR_T) RR " +
        $"WHERE to_char(RR.TEST_TIME,'yyyymmddhh24:mi') >= '{model.dateFrom}' || '{model.timeFrom}' " +
        $"AND to_char(RR.TEST_TIME,'yyyymmddhh24:mi') < '{model.dateTo}' || '{model.timeTo}' ";

            SQL += GetSelectByList(model.listModel, "rr.model_name");
            SQL += GetSelectByList(model.listMo, "rr.MO_NUMBER");
            SQL += GetSelectByList(model.listLine, "rr.TEST_LINE");
            SQL += GetSelectByList(model.listGroup, "rr.TEST_GROUP");
            SQL += $" AND upper(RR.Test_Code) = upper('{model.testCode}')  " +
             " GROUP BY RR.SERIAL_NUMBER " +
             ",RR.TEST_CODE  " +
             " ORDER BY RR.SERIAL_NUMBER, qty ";
            dtListSerialNumber = DBConnect.GetData(SQL, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtListSerialNumber });
        }

        [System.Web.Http.Route("GetRepairDetailSN")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetRepairDetailSN(QMElement model)
        {
            DataTable dtListSerialNumber = new DataTable();
            string SQL = "SELECT RR.SERIAL_NUMBER, count(RR.SERIAL_NUMBER) qty " +
         "FROM (select * from SFISM4.R_REPAIR_T union all select * from SFISM4.H_REPAIR_T) RR " +
         $"WHERE to_char(RR.TEST_TIME,'yyyymmddhh24:miss') >= '{model.dateFrom}' || '{model.timeFrom}' " +
         $"AND to_char(RR.TEST_TIME,'yyyymmddhh24:miss') < '{model.dateTo}' || '{model.timeTo}' ";
            SQL += GetSelectByList(model.listModel, "rr.model_name");
            SQL += GetSelectByList(model.listMo, "rr.MO_NUMBER");
            SQL += GetSelectByList(model.listLine, "rr.TEST_LINE");
            SQL += GetSelectByList(model.listGroup, "rr.TEST_GROUP");

            SQL += $"AND upper(RR.TEST_CODE) = upper('{model.testCode}') " +
    $" AND upper(RR.REASON_CODE) = upper('{model.reasonCode}') " +
    $" AND (RR.ERROR_ITEM_CODE = '{model.Desc}' or rr.error_item_code is null) " +
    $" AND RR.DUTY_TYPE = '{model.dutyType}' " +
    " GROUP BY RR.SERIAL_NUMBER " +
    " ORDER BY RR.SERIAL_NUMBER ";
            dtListSerialNumber = DBConnect.GetData(SQL, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtListSerialNumber });
        }


        [System.Web.Http.Route("QMYieldRate")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> QMYieldRate(QMElement model)
        {
            if (model.listModel.Count == 0 && model.listMo.Count == 0 && model.listGroup.Count == 0 && model.listMo.Count == 0) return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            bool HaveSection = true;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbGroupBy = new StringBuilder();
            StringBuilder sbOrderBy = new StringBuilder();

            string timeFrom = TimeToWorkSection(model.timeFrom);
            string timeTo = TimeToWorkSection(model.timeTo);
            string inStationTimeFrom = model.dateFromOrigi + model.timeFrom + "00";
            string inStationTimeTo = model.dateTo + model.timeTo + "00";

            if (model.timeFrom == "2400")
            {
                inStationTimeFrom = model.dateFromOrigi + "000000";
            }

            if (model.listModel.Count > 0)
            {
                if (model.listModel[0].VALUE == "HOUSING")
                {
                    DataTable dtHousing = new DataTable();

                    string strHousing = "select WORK_DATE,WORK_SECTION,MO_NUMBER,MODEL_NAME,LINE_NAME,SECTION_NAME,GROUP_NAME,PASS_QTY,FAIL_QTY,REPASS_QTY,REFAIL_QTY from sfism4.R102  " +
        $"       where model_name ='HOUSING' and WORK_DATE || trim(to_char(Work_Section,'00')) >=  ('{model.dateFrom}' || '{timeFrom}') and  WORK_DATE || trim(to_char(Work_Section,'00')) <  ('{model.dateTo}' || '{timeTo}') ";

                    dtHousing = DBConnect.GetData(strHousing, model.database_name);
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtHousing });
                }
            }
            if (!model.iDate && !model.iLine && !model.iModelSerial && !model.iModelName && !model.iSection && !model.iMoNumber && model.iGroup)
            {
                sb.Append(" Select  h.group_name,h.pass_qty,h.repass_qty,h.fail_qty,H.FIRST_FAIL_QTY,h.total_qty,H.FPY_Rate as \"F.P.Y.Rate\",h.yield_rate   ");

                if (model.iRepassQTY)
                {
                    sb.Append(",h.repass_qty");
                }
                if (model.iRefailQTY)
                {
                    sb.Append(",h.refail_qty");
                }
                if (model.iTFailPassQTY)
                {
                    sb.Append(",NVL(M.T_FAIL_PASS,'0')T_PassQty");
                }
                if (model.iTFailFailQTY)
                {
                    sb.Append(",NVL(M.T_FAIL_FAIL,'0') T_FailQty");
                }
                sb.Append(" from (select distinct r.GROUP_NAME, SUM (r.pass_qty) pass_qty, SUM (r.fail_qty) fail_qty,SUM(NVL(R.FIRST_FAIL_QTY,0)) FIRST_FAIL_QTY, ");
                sb.Append(" sum(R.PASS_QTY) + sum(R.FAIL_QTY) Total_qty, ");
                sb.Append(" to_number(to_char(100 * (sum(R.PASS_QTY)+sum(R.FAIL_QTY) -SUM(NVL(R.FIRST_FAIL_QTY,0)))/decode((sum(R.PASS_QTY)),0,1,(sum(R.PASS_QTY)+sum(R.FAIL_QTY))),'99000.00')) FPY_Rate, ");
                sb.Append(" to_char(100 * sum(R.PASS_QTY)/decode((sum(R.PASS_QTY)+sum(R.FAIL_QTY)),0,1,(sum(R.PASS_QTY)+sum(R.FAIL_QTY))),'99000.00') Yield_Rate,sum(nvl(r.repass_qty,0)) repass_qty,sum(nvl(r.refail_qty,0)) refail_qty,t.num ");
                sb.Append("");

                if (HaveSection)
                {
                    sb.Append(" FROM (SELECT work_section, mo_number, WORK_DATE, MODEL_NAME, LINE_NAME, SECTION_NAME, GROUP_NAME, repass_qty, refail_qty, ");
                    sb.Append(" PASS_QTY,FIRST_FAIL_QTY, FAIL_QTY FROM SFISM4.H_STATION_REC_T  ");
                    sb.Append(" union all SELECT work_section, mo_number, WORK_DATE, MODEL_NAME, LINE_NAME, SECTION_NAME, GROUP_NAME, repass_qty, refail_qty,PASS_QTY,FIRST_FAIL_QTY, FAIL_QTY FROM SFISM4.R_STATION_REC_T  ");
                    sb.Append(" ) R ");
                }
                else
                {
                    //viet sau
                }
                if (model.moState == "close")
                {
                    sb.Append(" ,(select mo_number, target_qty from Sfism4.r_mo_base_t where close_flag = '3' union all select mo_number, target_qty from sfism4.h_mo_base_t where close_flag='3') m ");
                }
                else if (model.moState == "working")
                {
                    sb.Append(" ,(select mo_number, target_qty from Sfism4.r_mo_base_t where close_flag = '2' union all select mo_number, target_qty from sfism4.h_mo_base_t where close_flag='2') m ");
                }
                sb.Append(" ,SFIS1.C_GROUP_CONFIG_T C, ");
                sb.Append(" (select group_next,min(step_sequence) num from sfis1.c_route_control_t where route_code in( ");
                sb.Append(" select distinct route_code from sfism4.r105 where mo_number in( ");
                sb.Append(" select distinct mo_number from sfism4.r_station_rec_t  ");
                sb.Append($" WHERE  WORK_DATE || trim(to_char(Work_Section,'00')) >=  ('{model.dateFrom}' || '{timeFrom}') and  WORK_DATE || trim(to_char(Work_Section,'00')) <  ('{model.dateTo}' || '{timeTo}') ");


                if (model.listModel.Count > 0)
                {
                    var lol = model.listModel;
                    var sbTemp = new StringBuilder();
                    sbTemp.Append(" and model_name IN(");

                    for (int i = 0; i < lol.Count; i++)
                    {
                        string val = lol[i].VALUE.ToString();

                        sbTemp.AppendFormat("'{0}'", val);
                        if (i < lol.Count - 1)
                        {
                            sbTemp.Append(",");
                        }
                    }
                    sbTemp.Append(")");

                    sb.Append(sbTemp.ToString());
                }
                sb.Append(" )) ");
                sb.Append(" group by group_next order by min(step_sequence))T ");
                sb.Append($" WHERE R.WORK_DATE || trim(to_char(Work_Section,'00')) >= ('{model.dateFrom}' || '{timeFrom}') and   R.WORK_DATE || trim(to_char(Work_Section,'00'))  < ('{model.dateTo}' || '{timeTo}')   ");
                if (model.moState != "all")
                {
                    sb.Append(" And r.mo_number = m.mo_number ");
                }

                sb.Append(GetSelectByList(model.listModel, "r.model_name"));
                sb.Append(GetSelectByList(model.listLine, "r.LINE_NAME"));
                sb.Append(GetSelectByList(model.listMo, "r.MO_NUMBER"));
                sb.Append(GetSelectByList(model.listGroup, "r.GROUP_NAME"));
                sb.Append(" AND R.GROUP_NAME = C.GROUP_NAME ");
                //sb.Append(" and r.section_name=c.section_name ");
                sb.Append(" AND C.GROUP_NAME NOT IN (SELECT GROUP_NAME FROM SFIS1.C_GROUP_NOROUTE_T) ");
                sb.Append(" AND R.GROUP_name=t.group_next Group by r.group_name, c.group_code, t.num ORDER BY t.num) h  ");

                if (model.iTFailFailQTY || model.iTFailPassQTY)
                {
                    sb.Append("LEFT outer JOIN (");
                    sb.Append(" SELECT DISTINCT I.GROUP_NAME,NVL(K.NUM2,'0') T_FAIL_FAIL,NVL(I.NUM1-K.NUM2,'0') T_FAIL_PASS FROM  (Select   ");
                    sb.Append(" E.GROUP_NAME,COUNT(DISTINCT F.SERIAL_NUMBER) NUM1   from sfism4.r108 F RIGHT OUTER JOIN  (SELECT D.SERIAL_NUMBER,C.GROUP_NAME  ");
                    sb.Append(" FROM  SFISM4.R109 D,(SELECT B.KEY_PART_SN,A.GROUP_NAME FROM SFISM4.R108 B,    (select  * from sfism4.r117 where MO_NUMBER IN(   ");
                    sb.Append(" select distinct mo_number from sfism4.r_station_rec_t WHERE WORK_DATE || trim(to_char(Work_Section,'00')) >=  ");
                    sb.Append($" ('{model.dateFrom}' || '{timeFrom}') and WORK_DATE || trim(to_char(Work_Section,'00'))  < ('{model.dateTo}' || '{timeTo}')  {GetSelectByList(model.listModel, "MODEL_NAME")} ) AND  TO_CHAR(IN_STATION_TIME,'YYYYMMDDHH24MISS') >= '{inStationTimeFrom}'  ");
                    sb.Append($" AND TO_CHAR(IN_STATION_TIME,'YYYYMMDDHH24MISS') <='{inStationTimeTo}')A WHERE A.SERIAL_NUMBER=B.SERIAL_NUMBER AND  ");
                    sb.Append(" SUBSTR(B.KEY_PART_NO,8,1)='T')C  WHERE   C.KEY_PART_SN=D.SERIAL_NUMBER)E ON F.KEY_PART_SN=E.SERIAL_NUMBER  GROUP BY  ");
                    sb.Append(" E.GROUP_NAME ) I, (SELECT H.GROUP_NAME, COUNT(DISTINCT(G.SERIAL_NUMBER)) NUM2 FROM SFISM4.R109 G RIGHT OUTER JOIN (Select   ");
                    sb.Append(" E.GROUP_NAME, F.SERIAL_NUMBER    from sfism4.r108 F RIGHT OUTER JOIN  (SELECT D.SERIAL_NUMBER,C.GROUP_NAME FROM  SFISM4.R109 D, ");
                    sb.Append(" (SELECT B.KEY_PART_SN,A.GROUP_NAME FROM SFISM4.R108 B,    (select  * from sfism4.r117 where MO_NUMBER IN(  select distinct  ");
                    sb.Append(" mo_number from sfism4.r_station_rec_t WHERE WORK_DATE || trim(to_char(Work_Section,'00')) >=   ");
                    sb.Append($" ('{model.dateFrom}' || '{timeFrom}') and WORK_DATE || trim(to_char(Work_Section,'00')) < ('{model.dateTo}' || '{timeTo}')  {GetSelectByList(model.listModel, "MODEL_NAME")} ) AND  TO_CHAR(IN_STATION_TIME,'YYYYMMDDHH24MISS') >= '{inStationTimeFrom}' AND  ");
                    sb.Append($" TO_CHAR(IN_STATION_TIME,'YYYYMMDDHH24MISS') <='{inStationTimeTo}')A WHERE A.SERIAL_NUMBER=B.SERIAL_NUMBER AND SUBSTR(B.KEY_PART_NO, ");
                    sb.Append(" 8,1)='T')C  WHERE   C.KEY_PART_SN=D.SERIAL_NUMBER)E ON F.KEY_PART_SN=E.SERIAL_NUMBER  )H ON G.SERIAL_NUMBER=H.SERIAL_NUMBER  ");
                    sb.Append(" GROUP BY H.GROUP_NAME)  K  WHERE I.GROUP_NAME=K.GROUP_NAME ");
                    sb.Append(" )M ON M.GROUP_NAME= H.GROUP_NAME ");
                }
                sb.Append(" order by h.num asc ");
            }
            else
            {
                sb.Append(" Select distinct ");
                sb.Append("");
                if (model.iDate || model.iLine || model.iSection || model.iGroup || model.iModelName || model.iModelSerial || model.iMoNumber || model.iRepassQTY || model.iRefailQTY || model.iTFailPassQTY || model.iTFailFailQTY)
                {
                    sbOrderBy.Append(" ORDER BY ");

                    sbOrderBy.Append(" 1 ");
                }


                if (model.iDate)
                {
                    sb.Append(" r.work_date, ");
                }
                if (model.iLine)
                {
                    sb.Append("r.line_name, ");
                }
                if (model.iModelSerial)
                {
                    sb.Append(" model_serial, ");
                }
                if (model.iModelName)
                {
                    sb.Append(" r.Model_name, ");
                }
                if (model.iMoNumber)
                {
                    sb.Append(" r.mo_number, m.target_qty target_qty, ");
                }
                if (model.iSection)
                {
                    sb.Append(" r.Section_NAME, ");
                }
                if (model.iGroup)
                {
                    sb.Append(" r.GROUP_NAME, ");
                }
                sb.Append(" sum(R.PASS_QTY) Pass_qty, sum(R.FAIL_QTY) Fail_qty, ");
                sb.Append(" sum(R.PASS_QTY) + sum(R.FAIL_QTY) Total_qty, ");
                sb.Append(" to_char(100 * sum(R.PASS_QTY)/decode((sum(R.PASS_QTY)+sum(R.FAIL_QTY)),0,1,(sum(R.PASS_QTY)+sum(R.FAIL_QTY))),'99000.00') Yield_Rate ");
                if (model.iRepassQTY)
                {
                    sb.Append(" ,sum(nvl(r.repass_qty,0)) repass_qty ");
                }
                if (model.iRefailQTY)
                {
                    sb.Append(" ,sum(nvl(r.refail_qty,0)) refail_qty ");
                }
                sb.Append(" FROM (SELECT work_section, mo_number, WORK_DATE, MODEL_NAME, LINE_NAME, SECTION_NAME, GROUP_NAME, repass_qty, refail_qty, ");
                sb.Append("  PASS_QTY, FAIL_QTY FROM SFISM4.H_STATION_REC_T union all ");
                sb.Append(" SELECT work_section, mo_number, WORK_DATE, MODEL_NAME, LINE_NAME, SECTION_NAME, GROUP_NAME, repass_qty, refail_qty, ");
                sb.Append(" PASS_QTY, FAIL_QTY FROM SFISM4.R_STATION_REC_T) R ");
                if (model.iMoNumber)
                {
                    sb.Append(" ,(select mo_number, target_qty from Sfism4.r_mo_base_t union all select mo_number, target_qty from sfism4.h_mo_base_t) m ");
                }
                if (model.iGroup)
                {
                    sb.Append(" ,SFIS1.C_GROUP_CONFIG_T C ");
                }
                if (model.moState == "close")
                {
                    sb.Append(" ,(select mo_number, target_qty from Sfism4.r_mo_base_t where close_flag = '3' union all select mo_number, target_qty from sfism4.h_mo_base_t where close_flag='3') m ");
                }
                else if (model.moState == "working")
                {
                    sb.Append(" ,(select mo_number, target_qty from Sfism4.r_mo_base_t where close_flag = '2' union all select mo_number, target_qty from sfism4.h_mo_base_t where close_flag='2') m ");
                }
                if (model.iSection)
                {
                    sb.Append(" ,SFIS1.C_Section_Config_T Cs ");
                }
                if (model.iModelName || model.iModelSerial)
                {
                    sb.Append(" , sfis1.c_model_desc_t cm ");
                }
                if (1 == 1)//co time
                {
                    sb.Append($" WHERE R.WORK_DATE || TRIM(to_char(Work_Section,'00')) >= ('{model.dateFrom}' || '{timeFrom}') and R.WORK_DATE || TRIM(to_char(Work_Section,'00')) < ('{model.dateTo}' || '{timeTo}') ");
                }
                else //khong time
                {
                    sb.Append($" WHERE R.WORK_DATE >= '{model.dateFrom}' ");
                    sb.Append($" AND R.WORK_DATE <= '{model.dateTo}' ");
                }
                if (model.moState != "all")
                {
                    sb.Append(" And r.mo_number = m.mo_number ");
                }
                if (model.iMoNumber)
                {
                    sb.Append(" And r.mo_number = m.mo_number ");
                }
                if (model.iModelName || model.iModelSerial)
                {
                    sb.Append(" AND R.MODEL_NAME = cm.model_name ");
                }
                if (model.listModel.Count > 0)
                {
                    var lol = model.listModel;
                    var sbTemp = new StringBuilder();
                    sbTemp.Append(" and r.model_name IN(");

                    for (int i = 0; i < lol.Count; i++)
                    {
                        string val = lol[i].VALUE.ToString();

                        sbTemp.AppendFormat("'{0}'", val);
                        if (i < lol.Count - 1)
                        {
                            sbTemp.Append(",");
                        }
                    }
                    sbTemp.Append(")");

                    sb.Append(sbTemp.ToString());
                }
                if (model.listLine.Count > 0)
                {
                    var lol = model.listLine;
                    var sbTemp = new StringBuilder();
                    sbTemp.Append(" and r.LINE_NAME IN(");

                    for (int i = 0; i < lol.Count; i++)
                    {
                        string val = lol[i].VALUE.ToString();

                        sbTemp.AppendFormat("'{0}'", val);
                        if (i < lol.Count - 1)
                        {
                            sbTemp.Append(",");
                        }
                    }
                    sbTemp.Append(")");

                    sb.Append(sbTemp.ToString());
                }
                if (model.listMo.Count > 0)
                {
                    var lol = model.listMo;
                    var sbTemp = new StringBuilder();
                    sbTemp.Append(" and r.MO_NUMBER IN(");

                    for (int i = 0; i < lol.Count; i++)
                    {
                        string val = lol[i].VALUE.ToString();

                        sbTemp.AppendFormat("'{0}'", val);
                        if (i < lol.Count - 1)
                        {
                            sbTemp.Append(",");
                        }
                    }
                    sbTemp.Append(")");

                    sb.Append(sbTemp.ToString());
                }
                if (model.listGroup.Count > 0)
                {
                    var lol = model.listGroup;
                    var sbTemp = new StringBuilder();
                    sbTemp.Append(" and r.GROUP_NAME IN(");

                    for (int i = 0; i < lol.Count; i++)
                    {
                        string val = lol[i].VALUE.ToString();

                        sbTemp.AppendFormat("'{0}'", val);
                        if (i < lol.Count - 1)
                        {
                            sbTemp.Append(",");
                        }
                    }
                    sbTemp.Append(")");

                    sb.Append(sbTemp.ToString());
                }
                if (model.iGroup) //group name
                {
                    sb.Append(" AND R.GROUP_NAME = C.GROUP_NAME ");
                    sb.Append(" and r.section_name=c.section_name ");
                    sb.Append(" AND C.GROUP_NAME NOT IN (SELECT GROUP_NAME FROM SFIS1.C_GROUP_NOROUTE_T) ");
                }
                if (model.iSection)
                {
                    sb.Append(" AND R.Section_NAME = Cs.Section_NAME ");
                }
                if (model.iDate || model.iLine || model.iSection || model.iGroup || model.iModelName || model.iModelSerial || model.iMoNumber || model.iRepassQTY || model.iRefailQTY || model.iTFailPassQTY || model.iTFailFailQTY)
                {
                    sbGroupBy.Append(" Group by ");
                    sbGroupBy.Append(" 1 ");
                }

                if (model.iDate)
                {
                    sbGroupBy.Append(" ,r.work_date ");
                    sbOrderBy.Append(" ,work_date ");
                }
                if (model.iLine)
                {
                    sbGroupBy.Append(" ,r.line_name ");
                    sbOrderBy.Append(" ,line_name ");
                }

                if (model.iModelSerial)
                {
                    sbGroupBy.Append(" ,model_Serial ");
                    sbOrderBy.Append(" ,model_serial ");
                }
                if (model.iModelName)
                {
                    sbGroupBy.Append(" ,r.Model_name ");
                    sbOrderBy.Append(" ,r.model_name ");
                }
                if (model.iMoNumber)
                {
                    sbGroupBy.Append(" ,r.mo_number, m.target_qty ");
                    sbOrderBy.Append("  ,mo_number ");
                }
                if (model.iSection)
                {
                    sbGroupBy.Append(" ,R.Section_NAME ");
                    sbOrderBy.Append(" ");
                }
                if (model.iGroup)
                {
                    sbGroupBy.Append(" ,R.GROUP_NAME,c.group_code ");
                    sbOrderBy.Append("  ");
                }

                sb.Append(sbGroupBy.ToString());
                sb.Append(sbOrderBy);
            }

            var query = sb.ToString();

            DataTable dtCheck = DBConnect.GetData(query, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                int passCount = ToNumber.ToNullableInt(dtCheck.Compute("Sum(PASS_QTY)", string.Empty).ToString());
                int failCount = ToNumber.ToNullableInt(dtCheck.Compute("Sum(FAIL_QTY)", string.Empty).ToString());
                double ttlRate = Math.Round((float)passCount / (passCount + failCount) * 100, 2);

                double rateTemp = 1;

                foreach (DataRow item in dtCheck.Rows)
                {
                    if (double.Parse(item["YIELD_RATE"].ToString()) != 0)
                    {
                        rateTemp = (float)rateTemp * double.Parse(item["YIELD_RATE"].ToString()) / 100;
                    }

                }
                var fpRate = Math.Round(rateTemp * 100, 2);

                DataTable dtGroup = new DataTable();

                try
                {
                    var selectGroup = dtCheck.AsEnumerable().Select(r => r.Field<string>("GROUP_NAME")).Distinct();
                    var checkExist = selectGroup.First();

                    StringBuilder sbGroup = new StringBuilder();
                    sbGroup.Append(" IN (");

                    string queryStringGroup = "select distinct group_name from SFIS1.C_GROUP_CONFIG_T C ";
                    var dtGroupNotIn = DBConnect.GetData(queryStringGroup, model.database_name);
                    var groupNew = dtGroupNotIn.AsEnumerable().Select(r => r.Field<string>("GROUP_NAME")).Except(selectGroup).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, ttlRate = ttlRate, fpRate = fpRate, allGroup = groupNew });
                }
                catch
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, ttlRate, fpRate, allGroup = dtGroup });
                }



            }
        }
        // GET: QM      
        [System.Web.Http.Route("GetQMElement")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetQMElement(QMElement model)
        {
            StringBuilder builder = new StringBuilder();
            string timeFrom = "";
            string timeTo = "";
            if (model.timeFrom == "ALL")
            {
                timeFrom = "00";
                timeTo = "25";
            }
            else if (model.timeFrom == "NO")
            {
                model.dateFrom = "00000000";
                model.dateTo = "99999999";
            }
            else
            {
                timeFrom = TimeToWorkSection(model.timeFrom);
                timeTo = TimeToWorkSection(model.timeTo);
            }
            if (model.type == "model")
            {
                string selectModel = "SELECT distinct MODEL_NAME VALUE " +
        "  FROM sfism4.R102 " +
        $" WHERE WORK_DATE || TRIM(TO_CHAR (WORK_SECTION, '00')) >= '{model.dateFrom}'||'{timeFrom}' AND WORK_DATE || TRIM(TO_CHAR (WORK_SECTION, '00')) <  '{model.dateTo}'||'{timeTo}' ";
                builder.Append(selectModel);
                //-----------check MO
                StringBuilder moStringBuilder = new StringBuilder();
                if (model.listMo.Count == 1)
                {
                    moStringBuilder.Append($" AND MO_NUMBER IN ('{model.listMo[0].VALUE}')");
                }
                else
                {
                    for (int i = 0; i < model.listMo.Count; i++)
                    {
                        if (i == 0)
                        {
                            moStringBuilder.Append($" AND MO_NUMBER IN ('{model.listMo[i].VALUE}'");
                        }
                        else if (i == model.listMo.Count - 1)
                        {
                            moStringBuilder.Append($",'{model.listMo[i].VALUE}') ");
                        }
                        else
                        {
                            moStringBuilder.Append($",'{model.listMo[i].VALUE}'");
                        }
                    }
                }
                string moString = moStringBuilder.ToString();
                builder.Append(moString);
                //------check line
                StringBuilder lineStringBuilder = new StringBuilder();
                if (model.listLine.Count == 1)
                {
                    lineStringBuilder.Append($" AND LINE_NAME IN ('{model.listLine[0].VALUE}')");
                }
                else
                {
                    for (int i = 0; i < model.listLine.Count; i++)
                    {
                        if (i == 0)
                        {
                            lineStringBuilder.Append($" AND LINE_NAME IN ('{model.listLine[i].VALUE}'");
                        }
                        else if (i == model.listLine.Count - 1)
                        {
                            lineStringBuilder.Append($",'{model.listLine[i].VALUE}') ");
                        }
                        else
                        {
                            lineStringBuilder.Append($",'{model.listLine[i].VALUE}'");
                        }
                    }
                }
                string lineString = lineStringBuilder.ToString();
                builder.Append(lineString);

                //--------check group name
                StringBuilder groupStringBuilder = new StringBuilder();
                if (model.listGroup.Count == 1)
                {
                    groupStringBuilder.Append($" AND GROUP_NAME IN ('{model.listGroup[0].VALUE}')");
                }
                else
                {
                    for (int i = 0; i < model.listGroup.Count; i++)
                    {
                        if (i == 0)
                        {
                            groupStringBuilder.Append($" AND GROUP_NAME IN ('{model.listGroup[i].VALUE}'");
                        }
                        else if (i == model.listGroup.Count - 1)
                        {
                            groupStringBuilder.Append($",'{model.listGroup[i].VALUE}') ");
                        }
                        else
                        {
                            groupStringBuilder.Append($",'{model.listGroup[i].VALUE}'");
                        }
                    }
                }
                string groupString = groupStringBuilder.ToString();
                builder.Append(groupString);
            }
            else if (model.type == "mo")
            {
                string selectMO = "SELECT distinct MO_NUMBER VALUE " +
        "  FROM sfism4.R102 " +
        $" WHERE WORK_DATE || TRIM(TO_CHAR (WORK_SECTION, '00')) >= '{model.dateFrom}'||'{timeFrom}' AND WORK_DATE || TRIM(TO_CHAR (WORK_SECTION, '00')) < '{model.dateTo}'||'{timeTo}' ";

                builder.Append(selectMO);
                //---model check
                StringBuilder modelStringBuilder = new StringBuilder();
                if (model.listModel.Count == 1)
                {
                    modelStringBuilder.Append($" AND MODEL_NAME IN ('{model.listModel[0].VALUE}')");
                }
                else
                {
                    for (int i = 0; i < model.listModel.Count; i++)
                    {
                        if (i == 0)
                        {
                            modelStringBuilder.Append($" AND MODEL_NAME IN ('{model.listModel[i].VALUE}'");
                        }
                        else if (i == model.listModel.Count - 1)
                        {
                            modelStringBuilder.Append($",'{model.listModel[i].VALUE}') ");
                        }
                        else
                        {
                            modelStringBuilder.Append($",'{model.listModel[i].VALUE}'");
                        }
                    }
                }
                string modelString = modelStringBuilder.ToString();
                builder.Append(modelString);
                //---line check
                StringBuilder lineStringBuilder = new StringBuilder();
                if (model.listLine.Count == 1)
                {
                    lineStringBuilder.Append($" AND LINE_NAME IN ('{model.listLine[0].VALUE}')");
                }
                else
                {
                    for (int i = 0; i < model.listLine.Count; i++)
                    {
                        if (i == 0)
                        {
                            lineStringBuilder.Append($" AND LINE_NAME IN ('{model.listLine[i].VALUE}'");
                        }
                        else if (i == model.listLine.Count - 1)
                        {
                            lineStringBuilder.Append($",'{model.listLine[i].VALUE}') ");
                        }
                        else
                        {
                            lineStringBuilder.Append($",'{model.listLine[i].VALUE}'");
                        }
                    }
                }
                string lineString = lineStringBuilder.ToString();
                builder.Append(lineString);

                //--group check
                StringBuilder groupStringBuilder = new StringBuilder();
                if (model.listGroup.Count == 1)
                {
                    groupStringBuilder.Append($" AND GROUP_NAME IN ('{model.listGroup[0].VALUE}')");
                }
                else
                {
                    for (int i = 0; i < model.listGroup.Count; i++)
                    {
                        if (i == 0)
                        {
                            groupStringBuilder.Append($" AND GROUP_NAME IN ('{model.listGroup[i].VALUE}'");
                        }
                        else if (i == model.listGroup.Count - 1)
                        {
                            groupStringBuilder.Append($",'{model.listGroup[i].VALUE}') ");
                        }
                        else
                        {
                            groupStringBuilder.Append($",'{model.listGroup[i].VALUE}'");
                        }
                    }
                }
                string groupString = groupStringBuilder.ToString();
                builder.Append(groupString);
            }
            else if (model.type == "line")
            {
                string selectLine = "SELECT distinct LINE_NAME VALUE " +
        "  FROM sfism4.R102 " +
        $" WHERE WORK_DATE || TRIM(TO_CHAR (WORK_SECTION, '00')) >= '{model.dateFrom}'||'{timeFrom}' AND WORK_DATE || TRIM(TO_CHAR (WORK_SECTION, '00')) < '{model.dateTo}'||'{timeTo}' ";

                builder.Append(selectLine);

                //-----------check MO
                StringBuilder moStringBuilder = new StringBuilder();
                if (model.listMo.Count == 1)
                {
                    moStringBuilder.Append($" AND MO_NUMBER IN ('{model.listMo[0].VALUE}')");
                }
                else
                {
                    for (int i = 0; i < model.listMo.Count; i++)
                    {
                        if (i == 0)
                        {
                            moStringBuilder.Append($" AND MO_NUMBER IN ('{model.listMo[i].VALUE}'");
                        }
                        else if (i == model.listMo.Count - 1)
                        {
                            moStringBuilder.Append($",'{model.listMo[i].VALUE}') ");
                        }
                        else
                        {
                            moStringBuilder.Append($",'{model.listMo[i].VALUE}'");
                        }
                    }
                }
                string moString = moStringBuilder.ToString();
                builder.Append(moString);
                //------check Model
                StringBuilder modelStringBuilder = new StringBuilder();
                if (model.listModel.Count == 1)
                {
                    modelStringBuilder.Append($" AND MODEL_NAME IN ('{model.listModel[0].VALUE}')");
                }
                else
                {
                    for (int i = 0; i < model.listModel.Count; i++)
                    {
                        if (i == 0)
                        {
                            modelStringBuilder.Append($" AND MODEL_NAME IN ('{model.listModel[i].VALUE}'");
                        }
                        else if (i == model.listModel.Count - 1)
                        {
                            modelStringBuilder.Append($",'{model.listModel[i].VALUE}') ");
                        }
                        else
                        {
                            modelStringBuilder.Append($",'{model.listModel[i].VALUE}'");
                        }
                    }
                }
                string modelString = modelStringBuilder.ToString();
                builder.Append(modelString);

                //--------check group name
                StringBuilder groupStringBuilder = new StringBuilder();
                if (model.listGroup.Count == 1)
                {
                    groupStringBuilder.Append($" AND GROUP_NAME IN ('{model.listGroup[0].VALUE}')");
                }
                else
                {
                    for (int i = 0; i < model.listGroup.Count; i++)
                    {
                        if (i == 0)
                        {
                            groupStringBuilder.Append($" AND GROUP_NAME IN ('{model.listGroup[i].VALUE}'");
                        }
                        else if (i == model.listGroup.Count - 1)
                        {
                            groupStringBuilder.Append($",'{model.listGroup[i].VALUE}') ");
                        }
                        else
                        {
                            groupStringBuilder.Append($",'{model.listGroup[i].VALUE}'");
                        }
                    }
                }
                string groupString = groupStringBuilder.ToString();
                builder.Append(groupString);
            }
            else if (model.type == "group")
            {
                string selectGroup = "SELECT distinct GROUP_NAME VALUE " +
        "  FROM sfism4.R102 " +
        $" WHERE WORK_DATE || TRIM(TO_CHAR (WORK_SECTION, '00')) >= '{model.dateFrom}'||'{timeFrom}' AND WORK_DATE || TRIM(TO_CHAR (WORK_SECTION, '00')) < '{model.dateTo}'||'{timeTo}' ";

                builder.Append(selectGroup);
                //-----------check MO
                StringBuilder moStringBuilder = new StringBuilder();
                if (model.listMo.Count == 1)
                {
                    moStringBuilder.Append($" AND MO_NUMBER IN ('{model.listMo[0].VALUE}')");
                }
                else
                {
                    for (int i = 0; i < model.listMo.Count; i++)
                    {
                        if (i == 0)
                        {
                            moStringBuilder.Append($" AND MO_NUMBER IN ('{model.listMo[i].VALUE}'");
                        }
                        else if (i == model.listMo.Count - 1)
                        {
                            moStringBuilder.Append($",'{model.listMo[i].VALUE}') ");
                        }
                        else
                        {
                            moStringBuilder.Append($",'{model.listMo[i].VALUE}'");
                        }
                    }
                }
                string moString = moStringBuilder.ToString();
                builder.Append(moString);
                //------check Model
                StringBuilder modelStringBuilder = new StringBuilder();
                if (model.listModel.Count == 1)
                {
                    modelStringBuilder.Append($" AND MODEL_NAME IN ('{model.listModel[0].VALUE}')");
                }
                else
                {
                    for (int i = 0; i < model.listModel.Count; i++)
                    {
                        if (i == 0)
                        {
                            modelStringBuilder.Append($" AND MODEL_NAME IN ('{model.listModel[i].VALUE}'");
                        }
                        else if (i == model.listModel.Count - 1)
                        {
                            modelStringBuilder.Append($",'{model.listModel[i].VALUE}') ");
                        }
                        else
                        {
                            modelStringBuilder.Append($",'{model.listModel[i].VALUE}'");
                        }
                    }
                }
                string modelString = modelStringBuilder.ToString();
                builder.Append(modelString);

                //---line check
                StringBuilder lineStringBuilder = new StringBuilder();
                if (model.listLine.Count == 1)
                {
                    lineStringBuilder.Append($" AND LINE_NAME IN ('{model.listLine[0].VALUE}')");
                }
                else
                {
                    for (int i = 0; i < model.listLine.Count; i++)
                    {
                        if (i == 0)
                        {
                            lineStringBuilder.Append($" AND LINE_NAME IN ('{model.listLine[i].VALUE}'");
                        }
                        else if (i == model.listLine.Count - 1)
                        {
                            lineStringBuilder.Append($",'{model.listLine[i].VALUE}') ");
                        }
                        else
                        {
                            lineStringBuilder.Append($",'{model.listLine[i].VALUE}'");
                        }
                    }
                }
                string lineString = lineStringBuilder.ToString();
                builder.Append(lineString);

            }
            builder.Append(" ");
            string queryString = $" SELECT * FROM ( {builder} )  ORDER BY VALUE";

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