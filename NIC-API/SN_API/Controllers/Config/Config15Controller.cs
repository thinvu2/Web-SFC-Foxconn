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
using static SN_API.Models.LineElement;

namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class Config15Controller : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfig15Content")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig15Content(Config15Element model)
        {
            string strGetData = "";
            if (string.IsNullOrEmpty(model.MODEL_NAME))
            {
                strGetData = $"SELECT A.MODEL_NAME ,A.VERSION_CODE ,A.TRAY_QTY ,A.CARTON_QTY ,A.PALLET_QTY ,A.CREATE_NAME ,A.COO ,A.SN_QTY ,A.LABEL_QTY ,A.PACK_FLAG,b.VR_CLASS IS_ADDTRAY,ROWIDTOCHAR(A.ROWID) ID  " +
                    $"FROM SFIS1.C_PACK_PARAM_T A left join(select* from SFIS1.C_PARAMETER_INI where PRG_NAME = 'PACKTRAYQTY') b on b.VR_CLASS = A.MODEL_NAME where rownum< 20 ORDER BY  MODEL_NAME ASC, VERSION_CODE ASC ";
            }
            else
            {
                strGetData = $"SELECT A.MODEL_NAME ,A.VERSION_CODE ,A.TRAY_QTY ,A.CARTON_QTY ,A.PALLET_QTY ,A.CREATE_NAME ,A.COO ,A.SN_QTY ,A.LABEL_QTY ,A.PACK_FLAG,b.VR_CLASS IS_ADDTRAY,ROWIDTOCHAR(A.ROWID) ID  " +
                    $" FROM SFIS1.C_PACK_PARAM_T A left join(select* from SFIS1.C_PARAMETER_INI where PRG_NAME = 'PACKTRAYQTY') b on b.VR_CLASS = A.MODEL_NAME " +
                    $" where upper(A.MODEL_NAME) LIKE '%{model.MODEL_NAME.ToUpper()}%' ORDER BY  MODEL_NAME ASC, VERSION_CODE ASC ";
            }
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            foreach (DataRow row in dtCheck.Rows)
            {
                row[10] = row[10].ToString() == "" ? "N" : "Y";
            }
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }
        [System.Web.Http.Route("InsertUpdateConfig15")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig15(Config15Element model)
        {
            try
            {
                //check privilege
                string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PACK PARAM_EDIT' AND EMP='{model.EMP}'";
                StringBuilder sb = new StringBuilder();

                StringBuilder sbLog = new StringBuilder();

                if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                }
                string packFlag = "AB000000";

                if (model.IS_CHECKMAC)
                {
                    packFlag = packFlag.Replace('B', '1');
                }
                else
                {
                    packFlag = packFlag.Replace('B', '0');
                }
                if (model.IS_ORDER_BOX)
                {
                    packFlag = packFlag.Replace('A', '1');
                }
                else
                {
                    packFlag = packFlag.Replace('A', '0');
                }
                //check exist
                string strCheckExist = $" select model_name from SFIS1.C_PACK_PARAM_T where model_name = '{model.MODEL_NAME}' and version_code ='{model.VERSION_CODE}' ";
                string actionString = " ";
                string modify = " ";
                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                {
                    // not exist => insert
                    sb.Append(" INSERT INTO SFIS1.C_PACK_PARAM_T ");
                    sb.Append(" (MODEL_NAME ,VERSION_CODE ,TRAY_QTY ,CARTON_QTY ,PALLET_QTY ,CREATE_NAME ,COO ,SN_QTY ,LABEL_QTY ,PACK_FLAG ) ");
                    sb.Append(" VALUES ");
                    sb.Append($" ('{model.MODEL_NAME}', '{model.VERSION_CODE}',{model.TRAY_QTY}, {model.CARTON_QTY}, {model.PALLET_QTY},null,null, {model.SN_QTY}, {model.LABEL_QTY}, '{packFlag}') ");
                    actionString = "INSERT";
                }
                else
                {
                    //existed => update
                    actionString = "UPDATE";
                    sb.Append(" UPDATE SFIS1.C_PACK_PARAM_T ");
                    sb.Append(" SET ");
                    sb.Append($" MODEL_NAME = '{model.MODEL_NAME}', "); //MODEL_NAME
                    sb.Append($" VERSION_CODE = '{model.VERSION_CODE}', "); //VERSION_CODE
                    sb.Append($" TRAY_QTY = '{model.TRAY_QTY}', "); //TRAY_QTY
                    sb.Append($" CARTON_QTY = '{model.CARTON_QTY}', "); //CARTON_QTY
                    sb.Append($" PALLET_QTY = '{model.PALLET_QTY}', "); //PALLET_QTY
                    sb.Append($" CREATE_NAME = '{model.CREATE_NAME}', "); //CREATE_NAME
                    sb.Append($" COO = '{model.COO}', "); //COO
                    sb.Append($" SN_QTY = '{model.SN_QTY}', "); //SN_QTY
                    sb.Append($" LABEL_QTY = '{model.LABEL_QTY}', "); //LABEL_QTY
                    sb.Append($" PACK_FLAG = '{packFlag}' "); //packFlag
                    sb.Append($" WHERE ROWID = '{model.ID}' "); //ID

                    modify = " UPDATE: ";
                    string query = $"select A.MODEL_NAME ,A.VERSION_CODE ,A.TRAY_QTY ,A.CARTON_QTY ,A.PALLET_QTY ,A.CREATE_NAME ,A.COO ," +
                                $"A.SN_QTY ,A.LABEL_QTY ,A.PACK_FLAG from SFIS1.C_PACK_PARAM_T A  WHERE ROWID = '{model.ID}' ";
                    DataTable dtModifly = DBConnect.GetData(query, model.database_name);
                    foreach (DataRow row in dtModifly.Rows)
                    {
                        if (row[0].ToString() != model.MODEL_NAME)
                        {
                            modify += $" MODEL_NAME: {row[0].ToString()};";
                        }
                        if (row[1].ToString() != model.VERSION_CODE)
                        {
                            modify += $" VERSION_CODE: {row[1].ToString()};";
                        }
                        if (row[2].ToString() != model.TRAY_QTY)
                        {
                            modify += $" TRAY_QTY: {row[2].ToString()} => {model.TRAY_QTY};";
                        }
                        if (row[3].ToString() != model.CARTON_QTY)
                        {
                            modify += $" CARTON_QTY: {row[3].ToString()} => {model.CARTON_QTY};";
                        }
                        if (row[4].ToString() != model.PALLET_QTY)
                        {
                            modify += $" PALLET_QTY: {row[4].ToString()} => {model.PALLET_QTY};";
                        }
                        if (row[7].ToString() != model.SN_QTY)
                        {
                            modify += $" SN_QTY: {row[7].ToString()} => {model.SN_QTY};";
                        }
                        if (row[8].ToString() != model.LABEL_QTY)
                        {
                            modify += $" LABEL_QTY: {row[8].ToString()} => {model.LABEL_QTY}";
                        }
                        if (row[9].ToString() != packFlag)
                        {
                            modify += $" PACK_FLAG: {row[9].ToString()} => {model.PACK_FLAG};";
                        }
                    }
                }
                string add4pcs = InsertUpdateTray(model) == true?"Y":"N";

                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config15 {model.MODEL_NAME};{model.VERSION_CODE}; {modify} ADD_4_PCS:{add4pcs}; IP:{AuthorizationController.UserIP()}' ");
                sbLog.Append(" ) ");

                string strInsertLog = sbLog.ToString();
                string strInserUpdate = sb.ToString();
                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config 6
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
        [System.Web.Http.Route("DeleteConfig15")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig15(Config15Element model)
        {
            //check privilege
            string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PACK PARAM_DELETE' AND EMP='{model.EMP}'";
            if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
            }
            string strDelete = $" delete SFIS1.C_PACK_PARAM_T where  ROWID = '{model.ID}' ";
            try
            {
                DBConnect.ExecuteNoneQuery(strDelete, model.database_name);
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" 'DELETE', ");
                sbLog.Append($"  'Config15 {model.MODEL_NAME};{model.VERSION_CODE};IP:{AuthorizationController.UserIP()}' ");
                sbLog.Append(" ) ");

                string strInsertLog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }

        public bool InsertUpdateTray(Config15Element model)
        {

            //check privilege
            StringBuilder sb = new StringBuilder();
            StringBuilder sbLog = new StringBuilder();
            int TRAY = Int32.Parse(model.TRAY_QTY) + 4;

            string strCheckExist = $" select * from SFIS1.C_PARAMETER_INI where PRG_NAME = 'PACKTRAYQTY'  AND VR_ITEM = 'OFFSET' AND VR_CLASS = '{model.MODEL_NAME}'";

            var dtCheck = DBConnect.GetData(strCheckExist, model.database_name);

            if (model.IS_ADDTRAY)
            {
                if (dtCheck.Rows.Count <= 0)
                {
                    // not exist => insert
                    sb.Append(" INSERT INTO SFIS1.C_PARAMETER_INI ");
                    sb.Append(" VALUES( ");
                    sb.Append($" 'PACKTRAYQTY','{model.MODEL_NAME}','OFFSET','OFFSET','{TRAY.ToString()}','' )");

                    string strInserUpdate = sb.ToString();
                    DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config 6
                   
                }
            }
            else
            {                
                if (dtCheck.Rows.Count > 0)
                {
                    sb.Append($" delete SFIS1.C_PARAMETER_INI where PRG_NAME = 'PACKTRAYQTY' AND VR_ITEM = 'OFFSET' AND VR_CLASS = '{model.MODEL_NAME}'");

                    string strInserUpdate = sb.ToString();
                    DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config 6
                    
                }
            }

            return model.IS_ADDTRAY;
        }
    }
}