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

namespace SN_API.Controllers.Config
{
    
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public class Config88Controller : ApiController
        {
            [System.Web.Http.Route("GetConfig88Content")]
            [System.Web.Http.HttpPost]

            public async Task<HttpResponseMessage> GetConfig88Content(Config88Element model)
            {
                string strGetData = "";
                if (string.IsNullOrEmpty(model.ID))
                {
                    strGetData = $"  SELECT * FROM SFIS1.C_XINGWEI_PARAM_T  where rownum <=20  ";
                }
                else
                {
                    strGetData = $"  SELECT * FROM SFIS1.C_XINGWEI_PARAM_T A where  upper(A.ID) LIKE '%{model.ID.ToUpper()}%' and rownum < 20  ";
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

            [System.Web.Http.Route("Config88_LoadModel_Name")]
            [System.Web.Http.HttpPost]

            // public async Task<HttpResponseMessage> Config88_LoadModel_Name(SN_API.Models.Config.Config88Element model)

            public async Task<HttpResponseMessage> Config88_LoadModel_Name(Config88Element model)
            {
                string strGetData = "";

                strGetData = $" select  distinct MODEL_NAME as MODEL_NAME_S  from SFIS1.C_MODEL_DESC_T ";

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

            [System.Web.Http.Route("Config88_Setup_LoadGroup_Name")]
            [System.Web.Http.HttpPost]

            // public async Task<HttpResponseMessage> Config88_LoadModel_Name(SN_API.Models.Config.Config88Element model)

            public async Task<HttpResponseMessage> Config88_Setup_LoadGroup_Name(Config88_Setup_Search model)
            {
                string strGetData = "";

                strGetData = $" select  DISTINCT GROUP_NAME as GROUP_NAME_S  from SFIS1.C_XINGWEI_SETUP_T WHERE MODEL_NAME ='{model.MODEL_NAME}'  ";

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

            [System.Web.Http.Route("Config88_Combine_Load_MoType")]
            [System.Web.Http.HttpPost]

            // public async Task<HttpResponseMessage> Config88_LoadModel_Name(SN_API.Models.Config.Config88Element model)

            public async Task<HttpResponseMessage> Config88_Combine_Load_MoType(Config88_Combine model)
            {
                string strGetData = "";

                strGetData = $" select  DISTINCT MO_TYPE as MO_TYPE_S  from SFIS1.C_XINGWEI_COMBINE_T WHERE MODEL_NAME ='{model.MODEL_NAME}' ";

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

            [System.Web.Http.Route("Config88_Combine_Load_GroupName")]
            [System.Web.Http.HttpPost]

            // public async Task<HttpResponseMessage> Config88_LoadModel_Name(SN_API.Models.Config.Config88Element model)

            public async Task<HttpResponseMessage> Config88_Combine_Load_GroupName(Config88_Combine model)
            {
                string strGetData = "";

                strGetData = $" select  DISTINCT GROUP_NAME as GROUP_NAME_S  from SFIS1.C_XINGWEI_COMBINE_T WHERE MODEL_NAME ='{model.MODEL_NAME}'  and MO_TYPE='{model.MO_TYPE}'";

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

            [System.Web.Http.Route("Config88_LoadTatble_Name")]
            [System.Web.Http.HttpPost]

            public async Task<HttpResponseMessage> Config88_LoadTatble_Name(Config88Element model)
            {
                string strGetData = "";

                strGetData = $" SELECT DISTINCT TABLE_NAME FROM SFIS1.C_XINGWEI_PARAM_T ";

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

            [System.Web.Http.Route("Config88_LoadColumn_Name")]
            [System.Web.Http.HttpPost]

            public async Task<HttpResponseMessage> Config88_LoadColumn_Name(Config88Element model)
            {

                String[] spearator = { ".", "" };
                Int32 count = 2;

                // using the method
                String[] table_name = model.TABLE_NAME.Split(spearator, count,
                       StringSplitOptions.RemoveEmptyEntries);

                string strGetData = "";

                if (table_name[1].ToString() == "R108")
                {

                    strGetData = $" SELECT  COLUMN_NAME FROM  all_tab_columns a where a.table_name ='R_WIP_KEYPARTS_T' and a.owner='{table_name[0].ToString()}' ";
                }
                else
                {
                    strGetData = $" SELECT  COLUMN_NAME FROM  all_tab_columns a where a.table_name ='{table_name[1].ToString()}' and a.owner='{table_name[0].ToString()}' ";
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

            [System.Web.Http.Route("InsertUpdateConfig88_Param")]
            [System.Web.Http.HttpPost]
            public async Task<HttpResponseMessage> InsertUpdateConfig88_Param(Config88Element model)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sbLog = new StringBuilder();
                    string strPrivilege = "";
                    //check exist
                    string strCheckExist = $"  select ID from SFIS1.C_XINGWEI_PARAM_T where ID = '{model.ID}' ";
                    string actionString = " ";
                    if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                    {
                        //check privilege
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PARAM CONFIG ' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        // not exist => insert

                        sb.Append($" Begin  SFIS1.P_INSERT_C_XINGWEI_PARAM_T( '{model.ID}','{model.TABLE_NAME}','{model.PK_COLUMN}','{model.TYPE}','{model.DATA1}','{model.DATA2}','{model.DATA3}','{model.DATA4}','{model.DATA5}','INSERT'); end;");

                        actionString = "INSERT";
                    }
                    else
                    {
                        //check privilege
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PARAM CONFIG' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        //existed => update
                        actionString = "UPDATE";

                        sb.Append($" Begin  SFIS1.P_INSERT_C_XINGWEI_PARAM_T( '{model.ID}','{model.TABLE_NAME}','{model.PK_COLUMN}','{model.TYPE}','{model.DATA1}','{model.DATA2}','{model.DATA3}','{model.DATA4}','{model.DATA5}','UPDATE'); end;");

                    }
                    sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                    sbLog.Append(" VALUES ( ");
                    sbLog.Append($" '{model.EMP}', ");
                    sbLog.Append($" 'CONFIG', ");
                    sbLog.Append($" '{actionString}', ");
                    sbLog.Append($"  'Config88 PARAM CONFIG: {model.ID};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_ERROR_CODE_T' ");
                    sbLog.Append(" ) ");
                    string strInsertLog = sbLog.ToString();
                    string strInserUpdate = sb.ToString();
                    DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config 6
                    DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);                                                            // DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
                }

            }

            /// <summary>
            ///  config setup table 
            /// </summary>

            [System.Web.Http.Route("GetConfig88_setup_Content")]
            [System.Web.Http.HttpPost]

            public async Task<HttpResponseMessage> GetConfig88_setup_Content(Config88_Setup model)
            {
                string strGetData = "";
                if (string.IsNullOrEmpty(model.MODEL_NAME) && string.IsNullOrEmpty(model.GROUP_NAME))
                {
                    strGetData = $"  SELECT * FROM SFIS1.c_xingwei_setup_t  where rownum <=20  ";
                }
                else
                {
                    if (string.IsNullOrEmpty(model.GROUP_NAME) && !string.IsNullOrEmpty(model.MODEL_NAME))
                    {
                        strGetData = $"  SELECT * FROM SFIS1.c_xingwei_setup_t A where  upper(A.MODEL_NAME) = '{model.MODEL_NAME.ToUpper()}'   ";
                    }
                    else
                    {
                        strGetData = $"  SELECT * FROM SFIS1.c_xingwei_setup_t A where  upper(A.MODEL_NAME) = '{model.MODEL_NAME.ToUpper()}' and  upper(A.GROUP_NAME)  = '{model.GROUP_NAME.ToUpper()}'   ";
                    }

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

            [System.Web.Http.Route("InsertUpdateConfig88_Setup")]
            [System.Web.Http.HttpPost]
            public async Task<HttpResponseMessage> InsertUpdateConfig88_Setup(Config88_Setup model)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sbLog = new StringBuilder();
                    string strPrivilege = "";
                    //check exist
                    string strCheckExist = $"  select MODEL_NAME from SFIS1.C_XINGWEI_SETUP_T where MODEL_NAME = '{model.MODEL_NAME}' AND  GROUP_NAME= '{model.GROUP_NAME}' ";
                    string actionString = " ";
                    if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                    {
                        //check privilege
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PARAM CONFIG' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        // not exist => insert

                        sb.Append($" Begin  SFIS1.P_INSERT_C_XINGWEI_SETUP_T( '{model.MODEL_NAME}','{model.GROUP_NAME}','{model.PORT}','{model.FORMAT}','{model.ACTION}',{model.LENGTH},'{model.INPUT}','INSERT'); end;");

                        actionString = "INSERT";
                    }
                    else
                    {
                        //check privilege
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PARAM CONFIG' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        //existed => update
                        actionString = "UPDATE";

                        sb.Append($" Begin  SFIS1.P_INSERT_C_XINGWEI_SETUP_T( '{model.MODEL_NAME}','{model.GROUP_NAME}','{model.PORT}','{model.FORMAT}','{model.ACTION}',{model.LENGTH},'{model.INPUT}','UPDATE'); end;");

                    }
                    sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                    sbLog.Append(" VALUES ( ");
                    sbLog.Append($" '{model.EMP}', ");
                    sbLog.Append($" 'CONFIG', ");
                    sbLog.Append($" '{actionString}', ");
                    sbLog.Append($"  'Config88 SETUP CONFIG: {model.MODEL_NAME};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_ERROR_CODE_T' ");
                    sbLog.Append(" ) ");
                    string strInsertLog = sbLog.ToString();
                    string strInserUpdate = sb.ToString();
                    DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config 6
                    DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);         // DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
                }

            }
            // config combine table 

            [System.Web.Http.Route("GetConfig88_Combine_Content")]
            [System.Web.Http.HttpPost]

            public async Task<HttpResponseMessage> GetConfig88_Combine_Content(Config88_Combine model)
            {
                string strGetData = "";
                if (string.IsNullOrEmpty(model.MODEL_NAME) && string.IsNullOrEmpty(model.MO_TYPE) && string.IsNullOrEmpty(model.GROUP_NAME))
                {
                    strGetData = $"  select a.* from (SELECT * FROM SFIS1.C_XINGWEI_COMBINE_T  order by date_create desc) a   where rownum <=20  ";
                }
                else
                {

                    if (!string.IsNullOrEmpty(model.MODEL_NAME) && (string.IsNullOrEmpty(model.MO_TYPE) && string.IsNullOrEmpty(model.GROUP_NAME)))

                    {
                        strGetData = $" SELECT * FROM SFIS1.C_XINGWEI_COMBINE_T A where  upper(A.MODEL_NAME) = '{model.MODEL_NAME.ToUpper()}'   ";
                    }
                    else
                    {
                        if ((!string.IsNullOrEmpty(model.MODEL_NAME) && !string.IsNullOrEmpty(model.MO_TYPE)) && string.IsNullOrEmpty(model.GROUP_NAME))
                        {
                            strGetData = $" SELECT * FROM SFIS1.C_XINGWEI_COMBINE_T A where  upper(A.MODEL_NAME) = '{model.MODEL_NAME.ToUpper()}' and MO_TYPE='{model.MO_TYPE}'  ";
                        }
                        else
                        {
                            strGetData = $" SELECT * FROM SFIS1.C_XINGWEI_COMBINE_T A where  upper(A.MODEL_NAME) = '{model.MODEL_NAME.ToUpper()}' and MO_TYPE='{model.MO_TYPE}' and GROUP_NAME ='{model.GROUP_NAME.ToUpper()}'  ";
                        }
                    }

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

            // INSERT UPDATE COMBINE TABLE 

            [System.Web.Http.Route("InsertUpdateConfig88_Combine")]
            [System.Web.Http.HttpPost]
            public async Task<HttpResponseMessage> InsertUpdateConfig88_Combine(Config88_Combine_add model)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sbLog = new StringBuilder();
                    string strPrivilege = "";
                    string strCheckPassLengt = "";

                    //check exist
                    string strCheckExist = $" select * from  table(PKG_RETURN_TABLE.F_GETMODEl_XW_COMBINE('{model.Data_New}')); ";
                    string actionString = " ";
                    if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0 && model.Action_type== "INSERT")
                    {
                        //check privilege
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PARAM CONFIG' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        // not exist => insert

                        sb.Append($" Begin  SFIS1.P_ADD_C_XINGWEI_COMBINE_T( '{model.Data_Old}','{model.Data_New}','{model.EMP}', 'INSERT'); end;");

                        actionString = "INSERT";
                    }
                    
                    else 
                        if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count >0  && model.Action_type == "INSERT")
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, new { result = "exists" });

                            }
                            else
                            {

                                if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count > 0 && model.Action_type == "INSERT")
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "exists" });

                                }


                                //check privilege
                                 strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PARAM CONFIG' AND EMP='{model.EMP}'";
                                    if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                                    {
                                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                                    }

                                    strCheckPassLengt = $"  select * from table(PKG_RETURN_TABLE.F_CHECK_PA_FA_LENGHT('{model.Data_New}')) ";

                                    if (DBConnect.GetData(strCheckPassLengt, model.database_name).Rows.Count <= 0)
                                    {
                                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "error_pass" });
                                    }

                                    //existed => update
                                    actionString = "UPDATE";
                                    sb.Append($" Begin  SFIS1.P_ADD_C_XINGWEI_COMBINE_T( '{model.Data_Old}','{model.Data_New}','{model.EMP}', 'UPDATE'); end;");

                            }
                                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                                sbLog.Append(" VALUES ( ");
                                sbLog.Append($" '{model.EMP}', ");
                                sbLog.Append($" 'CONFIG', ");
                                sbLog.Append($" '{actionString}', ");
                                sbLog.Append($"  'Config88 COMBINE CONFIG: ;IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_ERROR_CODE_T' ");
                                sbLog.Append(" ) ");
                                string strInsertLog = sbLog.ToString();
                                string strInserUpdate = sb.ToString();
                               // DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);
                                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config 6
                                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);

                                // DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });

                
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
                }

            }

       

        // DUPLICATE CONFIG88_COMBINE 
        [System.Web.Http.Route("Duplicate_Config88_Combine")]
            [System.Web.Http.HttpPost]
            public async Task<HttpResponseMessage> Duplicate_Config88_Combine(Config88_Combine_add model)
            {

                try
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sbLog = new StringBuilder();
                    string strPrivilege = "";
                //check exist
                string strCheckExist = $" select * from  table(PKG_RETURN_TABLE.F_GETMODEl_XW_COMBINE('{model.Data_New}')) ";

                string actionString = " ";
                    if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                    {
                        //check privilege
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PARAM CONFIG' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        // not exist => insert

                        sb.Append($" Begin  SFIS1.P_COPY_XINGWEI_COMBINE_T( '{model.Data_Old}','{model.Data_New}','{model.EMP}','{model.Model_Old}', 'COPY'); end;");

                        actionString = "INSERT";
                    }
                    else
                    {
                        //check privilege

                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PARAM CONFIG' AND EMP='{model.EMP}'";

                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        //existed => update

                        actionString = "UPDATE";

                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "areadly setup" });

                        //  sb.Append($" Begin  SFIS1.P_INSERT_C_XINGWEI_SETUP_T( '{model.MODEL_NAME}','{model.GROUP_NAME}','{model.PORT}','{model.FORMAT}','{model.ACTION}',{model.LENGTH},'{model.INPUT}','UPDATE'); end;");

                    }

                    sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                    sbLog.Append(" VALUES ( ");
                    sbLog.Append($" '{model.EMP}', ");
                    sbLog.Append($" 'CONFIG', ");
                    sbLog.Append($" '{actionString}', ");
                    sbLog.Append($"  'Config88 SETUP CONFIG:;IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_ERROR_CODE_T' ");
                    sbLog.Append(" ) ");
                    string strInsertLog = sbLog.ToString();
                    string strInserUpdate = sb.ToString();
                    DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config 6
                    DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);         // DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
                }

            }

            // DUPLICATE CONFIG88_SETUP

            [System.Web.Http.Route("Duplicate_Config88_Setup")]
            [System.Web.Http.HttpPost]
            public async Task<HttpResponseMessage> Duplicate_Config88_Setup(Config88_Setup_Copy model)
            {

                try
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sbLog = new StringBuilder();
                    string strPrivilege = "";
                    //check exist
                    string strCheckExist = $"  select MODEL_NAME from SFIS1.C_XINGWEI_SETUP_T where MODEL_NAME = '{model.MODEL_NAME_NEW}' AND  GROUP_NAME= '{model.GROUP_NAME}' ";
                    string actionString = " ";
                    if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                    {
                        //check privilege
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PARAM CONFIG' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        // not exist => insert

                        sb.Append($" Begin  SFIS1.P_DUPLICATE_C_XINGWEI_SETUP_T( '{model.MODEL_NAME}','{model.GROUP_NAME}','{model.MODEL_NAME_NEW}','INSERT'); end;");

                        actionString = "INSERT";
                    }
                    else
                    {
                        //check privilege
                        strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PARAM CONFIG' AND EMP='{model.EMP}'";
                        if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                        }

                        //existed => update
                        actionString = "UPDATE";

                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "areadly setup" });

                        //  sb.Append($" Begin  SFIS1.P_INSERT_C_XINGWEI_SETUP_T( '{model.MODEL_NAME}','{model.GROUP_NAME}','{model.PORT}','{model.FORMAT}','{model.ACTION}',{model.LENGTH},'{model.INPUT}','UPDATE'); end;");

                    }
                    sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                    sbLog.Append(" VALUES ( ");
                    sbLog.Append($" '{model.EMP}', ");
                    sbLog.Append($" 'CONFIG', ");
                    sbLog.Append($" '{actionString}', ");
                    sbLog.Append($"  'Config88 SETUP CONFIG: {model.MODEL_NAME};IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_ERROR_CODE_T' ");
                    sbLog.Append(" ) ");
                    string strInsertLog = sbLog.ToString();
                    string strInserUpdate = sb.ToString();
                    DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config 6
                    DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);         // DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
                }

            }



            [System.Web.Http.Route("CheckPermisstionDelete_Setup")]
            [System.Web.Http.HttpPost]
            public async Task<HttpResponseMessage> CheckPermisstionDelete_Setup(LoginInfo privilege)
            {
                string strPrivilege = "";
                string database_name = privilege.DatabaseName;
                string UserName = privilege.UserName;
                string PassWord = privilege.PassWord;


                strPrivilege = $"  SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'PARAM CONFIG DELETE' AND EMP='{UserName}'";
                if (DBConnect.GetData(strPrivilege, database_name).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                }

                DataTable dtCheck = DBConnect.GetData(strPrivilege, database_name);

                if (dtCheck.Rows.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
                }
            }
        }
    
}