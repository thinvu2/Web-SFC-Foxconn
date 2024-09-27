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

namespace SN_API.Controllers.Config
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class Config7Controller : ApiController
    {
        [System.Web.Http.Route("GetConfig7")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfig7(Config7Element model)
        {
            string getdataemp = string.Empty;
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(model.EMP_NO))
            {
                getdataemp = $"select ROWIDTOCHAR(ROWID) ID,A.* from SFIS1.C_EMP_DESC_T A where (A.EMAIL not in ('OFFWORK','LOCK') or A.email is null)";
            }
            else
            {
                getdataemp = $"select ROWIDTOCHAR(ROWID) ID,A.* from SFIS1.C_EMP_DESC_T A where A.emp_no='{model.EMP_NO.ToUpper()}' AND (A.EMAIL not in ('OFFWORK','LOCK') or A.email is null)";

            }
            dt = DBConnect.GetData(getdataemp, model.database_name);
            if (dt.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dt });
            }
        }
        [System.Web.Http.Route("GetGroupConfig7")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetGroupConfig7(Config7Element model)
        {
            string Get_Group_Data = string.Empty;

            if (string.IsNullOrEmpty(model.EMP_NO))
            {
                Get_Group_Data = $" select DISTINCT GROUP_NAME VALUE from SFIS1.C_GROUP_CONFIG_T where SUBSTR(group_name,1,2) NOT IN ('R_') ";
                if (model.database_name == "SFO")
                {
                    Get_Group_Data = $"select DISTINCT GROUP_NAME VALUE from SFIS1.C_GROUP_CONFIG_T ";
                }
            }
            else
            {
                Get_Group_Data = $"select DISTINCT GROUP_NAME VALUE from SFIS1.C_EMP_2_GROUP_T where emp_no='{model.EMP_NO}'";
            }
            DataTable dt_get = DBConnect.GetData(Get_Group_Data, model.database_name);
            if (dt_get.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dt_get });
            }
        }
        [System.Web.Http.Route("DeleteConfig7")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> DeleteConfig7(Config7Element model)
        {
            try
            {
                string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG' AND FUN = 'EMPLOYEE_DELETE' AND EMP='{model.emp}'";
                if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                }
                string sql_del = $"delete SFIS1.C_EMP_DESC_T where rowid='{model.ID}'";
                DBConnect.ExecuteNoneQuery(sql_del, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }

        [System.Web.Http.Route("InsertOrUpdateConfig7")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertOrUpdateConfig7(Config7Element model)
        {
            try
            {
                //string check_Privilege = $"SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND( FUN = 'EMPLOYEE_ADD' OR FUN='EMPLOYEE_EDIT') AND EMP='{model.emp}'";
                //DataTable dt_check_privilege = DBConnect.GetData(check_Privilege, model.database_name);
                string check_privilege = string.Empty;
                string strInserUpdate = string.Empty;
                string actionString = string.Empty;
                DataTable dt_check_prive = new DataTable();
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                DateTime time_quit_date = model.QuitDate;
                string time_quit = time_quit_date.ToString("yyyy/MM/dd");
                time_quit = time_quit + " 00:00:00";
                //var regexItem = new Regex("^[A-Z0-9]*$");
                var pattern = new Regex(@"^(?=.*[0-9A-Za-z])(?=.*\d)(?=.*[!@#$%^&*()_.+])[0-9A-Za-z\d][0-9A-Za-z\d!@#$%^&*()._+]{7,15}$");
                string check_data = $"SELECT * FROM SFIS1.C_EMP_DESC_T WHERE EMP_NO='{model.EMP_NO}' ";
                DataTable dt_check_emp = DBConnect.GetData(check_data, model.database_name);
                DateTime datetime = new DateTime();
                if (dt_check_emp.Rows.Count == 0)
                {
                    check_privilege = $"SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG' and    FUN='EMPLOYEE_ADD' AND EMP='{model.emp}' ";
                    dt_check_prive = DBConnect.GetData(check_privilege, model.database_name);
                    if (dt_check_prive.Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    actionString = "INSERT CONFIG";
                    sb.Append("INSERT INTO SFIS1.C_EMP_DESC_T( ");
                    sb.Append("EMP_NO,EMP_NAME,EMP_RANK");
                    sb.Append(",CLASS_NAME,STATION_NAME,");
                    sb.Append("QUIT_DATE,EMP_PASS,EMP_BC,");
                    sb.Append("EMP_PWD_PASS");
                    sb.Append(" ) ");
                    sb.Append(" VALUES( ");
                    sb.Append($"'{model.EMP_NO}',");
                    sb.Append($"'{model.EMP_NAME}',");
                    sb.Append($"'0',");
                    sb.Append($"'{model.CLASS_NAME}',to_char(sysdate,'yyyy/mm/dd'), ");
                    sb.Append($"TO_DATE('{time_quit}','YYYY/MM/DD HH24:MI:SS'),'{model.EMP_PASS}','{model.EMP_PASS}','{MRHA_DECRYPT(model.EMP_PASS)}'");
                    sb.Append(")");
                }
                else
                {
                    check_privilege = $"SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG' and    FUN='EMPLOYEE_EDIT' AND EMP='{model.emp}' ";
                    dt_check_prive = DBConnect.GetData(check_privilege, model.database_name);
                    if (dt_check_prive.Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                    actionString = "UPDATE CONFIG";
                    sb.Append("UPDATE SFIS1.C_EMP_DESC_T SET ");
                    sb.Append($"EMP_NO='{model.EMP_NO}',");
                    sb.Append($"EMP_NAME='{model.EMP_NAME}',");
                    sb.Append($"CLASS_NAME='{model.CLASS_NAME}',");
                    sb.Append($"QUIT_DATE=TO_DATE('{time_quit}','YYYY/MM/DD HH24:MI:SS'),");
                    sb.Append($"EMP_PASS='{model.EMP_PASS}',EMP_BC='{model.EMP_PASS}',");
                    sb.Append($"EMP_PWD_PASS ='{MRHA_DECRYPT(model.EMP_PASS)}'");
                    sb.Append(" WHERE ");
                    sb.Append($" ROWID='{model.ID}'");

                    string delete_group = $"DELETE SFIS1.C_EMP_2_GROUP_T where  EMP_NO='{model.EMP_NO}' ";
                    DBConnect.ExecuteNoneQuery(delete_group, model.database_name);
                }
                strInserUpdate = sb.ToString();
                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.emp}', ");
                sbLog.Append($" 'CONFIG', ");
                sbLog.Append($" '{actionString}', ");
                sbLog.Append($"  'Config7 EMPLOYEE: {model.EMP_NO},;IP:{AuthorizationController.UserIP()}; TABLE: SFIS1.C_EMP_DESC_T' ");
                sbLog.Append(" ) ");
                string strInsertLog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);  //Execute insert update config 6
                DBConnect.ExecuteNoneQuery(strInsertLog, model.database_name);  //Execute insert log
                if (model.LISTGROUPDF != null)
                {
                    if (model.LISTGROUPDF.Count > 0)
                    {
                        for (int i = 0; i < model.LISTGROUPDF.Count; i++)
                        {
                            InsertGroupForUser(model.EMP_NO, model.LISTGROUPDF[i].VALUE, model.database_name);
                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
        public void InsertGroupForUser(string emp_no, string group, string database)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(group))
                {
                    string check_group = $"SELECT * FROM SFIS1.C_EMP_2_GROUP_T where  EMP_NO='{emp_no}' and GROUP_NAME='{group}' ";
                    DataTable dt = DBConnect.GetData(check_group, database);
                    if (dt.Rows.Count <= 0)
                    {
                        string insert_group = $"INSERT INTO sfis1.C_EMP_2_GROUP_T (EMP_NO,GROUP_NAME) VALUES('{emp_no}','{group}') ";
                        DBConnect.ExecuteNoneQuery(insert_group, database);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [System.Web.Http.Route("ChangePasswordConfig")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ChangePasswordConfig(ConfigChangePassWord model)
        {
            string getstr = string.Empty;
            string insert_group = string.Empty;
            StringBuilder sb = new StringBuilder();
            try
            {
                getstr = $"SELECT * FROM SFIS1.C_EMP_DESC_T WHERE EMP_PASS='{model.PASSWORD}' AND EMP_NO='{model.EMP_NO}'";
                DataTable dt = new DataTable();
                dt = DBConnect.GetData(getstr, model.database_name);
                if (dt.Rows.Count > 0)
                {
                    getstr = $"SELECT * FROM SFIS1.C_EMP_DESC_T WHERE EMP_PASS='{model.NEW_PASSWORD}' AND EMP_NO<>'{model.EMP_NO}'";
                    dt = DBConnect.GetData(getstr, model.database_name);
                    if (dt.Rows.Count > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "PASSWORD HAD USE" });
                    }

                    var pattern = new Regex(@"^(?=.*[0-9A-Za-z])(?=.*\d)(?=.*[!@#$%^&*()_.+])[0-9A-Za-z\d][0-9A-Za-z\d!@#$%^&*()._+]{7,15}$");

                    if (pattern.IsMatch(model.NEW_PASSWORD) == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            result = "MẬT KHẢU PHẢI CÓ CHỮ, SỐ VÀ KÝ TỰ ĐẶC BIỆT!\n"
                                                                                     + " The password must be 8 to 16 characters in length!\n"
                                                                                     + " Must contain at least one letter and one number and a special character from !@#$%^&* ()_+.\n"
                                                                                     + " Should not start with a special character."
                        });
                    }

                    sb.Clear();
                    sb.Append("UPDATE SFIS1.C_EMP_DESC_T SET ");
                    sb.Append($"EMP_PASS='{model.NEW_PASSWORD}',EMP_BC='{model.NEW_PASSWORD}',");
                    sb.Append($"EMP_PWD_PASS ='{MRHA_DECRYPT(model.NEW_PASSWORD)}'");
                    sb.Append(" WHERE ");
                    sb.Append($" EMP_NO='{model.EMP_NO}'");
                    insert_group = sb.ToString();
                    DBConnect.ExecuteNoneQuery(insert_group, model.database_name);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "PASSWORD NOT MATCH!" });
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
        public static string MH(string TMPSTR)
        {
            string text = "";
            int num = 0;
            int num2 = 0;
            bool flag = false;
            num = int.Parse(TMPSTR);
            while (!flag)
            {
                if (num < 34)
                {
                    flag = true;
                    if (num >= 0 && num <= 9)
                    {
                        text = num.ToString() + text;
                    }
                    if (num >= 10 && num <= 17)
                    {
                        text = Convert.ToChar(num + 55).ToString() + text;
                    }
                    if (num >= 18 && num <= 22)
                    {
                        text = Convert.ToChar(num + 56).ToString() + text;
                    }
                    if (num >= 23 && num <= 33)
                    {
                        text = Convert.ToChar(num + 57).ToString() + text;
                    }
                }
                if (num > 33)
                {
                    num2 = num % 34;
                    num /= 34;
                    if (num2 >= 0 && num2 <= 9)
                    {
                        text = num2.ToString() + text;
                    }
                    if (num2 >= 10 && num2 <= 17)
                    {
                        text = Convert.ToChar(num2 + 55).ToString() + text;
                    }
                    if (num2 >= 18 && num2 <= 22)
                    {
                        text = Convert.ToChar(num2 + 56).ToString() + text;
                    }
                    if (num2 >= 23 && num2 <= 33)
                    {
                        text = Convert.ToChar(num2 + 57).ToString() + text;
                    }
                }
            }
            return text;
        }
        public static string MRHA_DECRYPT(string STR_STRING)
        {
            string text = "";
            int num = 9;
            long num2 = 0L;
            foreach (char value in STR_STRING)
            {
                int num3 = int.Parse(Convert.ToInt32(value).ToString()) ^ num;
                num2 += num3;
                text += MH(num2.ToString());
            }
            return text;
        }
        #region Privilege
        [System.Web.Http.Route("getEmpNo")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> getEmpNo(Config7Element model)
        {
            try
            {
                string getPrivilege = "";
                getPrivilege = $"SELECT EMP_NO ||'_'|| EMP_NAME AS EMP_NO_NAME FROM SFIS1.C_EMP_DESC_T";
                DataTable dtPrivilege = DBConnect.GetData(getPrivilege, model.database_name);
                if (dtPrivilege.Rows.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtPrivilege });
                }
            }
            catch (SqlException ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Database error", message = ex.Message });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }
        //EMP-CopyDF
        [System.Web.Http.Route("getEmpCopyDf")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> getEmpCopyDf(Config7Element model)
        {
            try
            {
                string getPrivilege = "";
                getPrivilege = $"SELECT EMP_NO ||'_'|| EMP_NAME AS EMP_COPYDF FROM SFIS1.C_EMP_DESC_T";
                DataTable dtPrivilege = DBConnect.GetData(getPrivilege, model.database_name);
                if (dtPrivilege.Rows.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtPrivilege });
                }
            }
            catch (SqlException ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Database error", message = ex.Message });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }
        //EMP-CopyNotDf
        [System.Web.Http.Route("getEmpCopyNotDf")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> getEmpCopyNotDf(Config7Element model)
        {
            try
            {
                string getPrivilege = "";
                getPrivilege = $"SELECT EMP_NO ||'_'|| EMP_NAME AS EMP_COPYNOTDF FROM SFIS1.C_EMP_DESC_T";
                DataTable dtPrivilege = DBConnect.GetData(getPrivilege, model.database_name);
                if (dtPrivilege.Rows.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtPrivilege });
                }
            }
            catch (SqlException ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Database error", message = ex.Message });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }
        //Emp-list
        [System.Web.Http.Route("selectMultipleEmp")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> selectMultipleEmp(Config7Element model)
        {
            try
            {
                string getPrivilege = "";
                string inputValue = "";
                var listInputEmp = model.LISTINPUTEMP?.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
   
                if (listInputEmp == null || listInputEmp.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "no data" });
                }
                for (int i = 0; i < listInputEmp.Count; i++)
                {
                    if (i == 0)
                    {
                        inputValue += "'" + listInputEmp[i] + "'";
                    }
                    else
                    {
                        inputValue += ",'" + listInputEmp[i] + "'";
                    }
                }
                getPrivilege = $"SELECT PRG_NAME ,FUN, PASSW, PRIVILEGE, EMP FROM SFIS1.C_PRIVILEGE WHERE EMP IN({inputValue}) ORDER BY EMP";
                string getEmp = $"SELECT distinct EMP FROM SFIS1.C_PRIVILEGE WHERE EMP IN({inputValue})";

                DataTable dtPrivilege = DBConnect.GetData(getPrivilege, model.database_name);
                DataTable dtgetEmp = DBConnect.GetData(getEmp, model.database_name);
                if (dtPrivilege.Rows.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtPrivilege,  EMP = dtgetEmp });
                }
            }
            catch (SqlException ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Database error", message = ex.Message });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }

        [System.Web.Http.Route("selectGetNotDefineApp")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> selectGetNotDefineApp(Config7Element model)
        {
            try
            {
                string getPrivilege = "";
                if (model.value == "false")
                {
                    getPrivilege = $"select 'ALL'SPRG_NAME from dual union SELECT distinct PRG_NAME as SPRG_NAME  FROM SFIS1.C_PRIVILEGE where emp in(select regexp_substr('{model.EMP_COPYNOTDF}','^[^_]+') as emp from dual) order by SPRG_NAME";
                }
                else
                {
                    getPrivilege = $"select 'ALL'SPRG_NAME from dual union SELECT distinct PRG_NAME as SPRG_NAME FROM (SELECT distinct PRG_NAME FROM SFIS1.C_PRIVILEGE) order by SPRG_NAME";

                }
                DataTable dtPrivilege = DBConnect.GetData(getPrivilege, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtPrivilege });
            }
            catch (SqlException ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Database error", message = ex.Message });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }
        [System.Web.Http.Route("selectGetDefineApp")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> selectGetDefineApp(Config7Element model)
        {
            try
            {
                string getPrivilege = "";
                if (model.valueInput == "true")
                {
                    string inputValue = "";
                    var listInputEmp = model.LISTINPUTEMP?.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (listInputEmp == null || listInputEmp.Count == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "no data" });
                    }
                    for (int i = 0; i < listInputEmp.Count; i++)
                    {
                        if (i == 0)
                        {
                            inputValue += "'" + listInputEmp[i] + "'";
                        }
                        else
                        {
                            inputValue += ",'" + listInputEmp[i] + "'";
                        }
                    }
                    getPrivilege = $"select 'ALL'DPRG_NAME from dual union SELECT distinct PRG_NAME as DPRG_NAME FROM SFIS1.C_PRIVILEGE WHERE EMP in ({inputValue}) order by DPRG_NAME";
                }
                else if (model.value == "false")
                {
                    getPrivilege = $"select 'ALL'DPRG_NAME from dual union SELECT distinct PRG_NAME as DPRG_NAME FROM SFIS1.C_PRIVILEGE WHERE EMP in(select regexp_substr ('{model.EMP_COPYDF}','^[^_]+') from dual) order by DPRG_NAME";
                }
                else
                {
                    getPrivilege = $"select 'ALL'DPRG_NAME from dual union SELECT distinct PRG_NAME as DPRG_NAME FROM SFIS1.C_PRIVILEGE WHERE EMP in(select regexp_substr ('{model.EMP_NO_NAME}','^[^_]+') from dual) order by DPRG_NAME";
                }
                DataTable dtPrivilege = DBConnect.GetData(getPrivilege, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtPrivilege });
            }
            catch (SqlException ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Database error", message = ex.Message });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }
        [System.Web.Http.Route("getNotDefineApp")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> getNotDefineApp(Config7Element model)
        {
            try
            {
                string getPrivilege = "";
                if (model.value == "false")
                {
                    if (model.SPRG_NAME == "" || model.SPRG_NAME == "ALL" || model.SPRG_NAME == null)
                    {
                        getPrivilege = $"SELECT distinct PRG_NAME,FUN, PASSW FROM SFIS1.C_PRIVILEGE WHERE EMP in(select regexp_substr ('{model.EMP_COPYNOTDF}','^[^_]+') from dual) order by prg_name";
                    }
                    else
                    {
                        getPrivilege = $"SELECT distinct PRG_NAME, FUN, PASSW FROM SFIS1.C_PRIVILEGE WHERE EMP in(select regexp_substr ('{model.EMP_COPYNOTDF}','^[^_]+') from dual) and PRG_NAME ='{model.SPRG_NAME}' order by prg_name";
                    }
                }
                else
                {
                    if (model.SPRG_NAME == "" || model.SPRG_NAME == "ALL" || model.SPRG_NAME == null)
                    {
                        getPrivilege = $"SELECT PRG_NAME,  FUN, passw  FROM (SELECT distinct passw ,PRG_NAME, FUN FROM SFIS1.C_PRIVILEGE minus SELECT passw ,PRG_NAME, FUN FROM SFIS1.C_PRIVILEGE WHERE EMP in(select regexp_substr ('{model.EMP_NO_NAME}','^[^_]+') from dual)) order by prg_name";
                    }
                    else
                    {
                        getPrivilege = $"SELECT PRG_NAME,  FUN, PASSW  FROM (SELECT distinct PRG_NAME,  FUN, PASSW FROM SFIS1.C_PRIVILEGE where PRG_NAME ='{model.SPRG_NAME}' minus SELECT PRG_NAME,  FUN, PASSW FROM SFIS1.C_PRIVILEGE WHERE EMP in(select regexp_substr ('{model.EMP_NO_NAME}','^[^_]+') from dual) and PRG_NAME ='{model.SPRG_NAME}') order by prg_name";
                    }
                }
                DataTable dtPrivilege = DBConnect.GetData(getPrivilege, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtPrivilege });
            } catch (SqlException ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Database error", message = ex.Message });
            } catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }
        [System.Web.Http.Route("getDefineApp")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> getDefineApp(Config7Element model)
        {
            try
            {
                string getPrivilege = "";

                if (model.valueInput == "true")
                {
                    string inputValue = "";
                    var listInputEmp = model.LISTINPUTEMP?.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (listInputEmp == null || listInputEmp.Count == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "no data" });
                    }
                    for (int i = 0; i < listInputEmp.Count; i++)
                    {
                        if (i == 0)
                        {
                            inputValue += "'" + listInputEmp[i] + "'";
                        }
                        else
                        {
                            inputValue += ",'" + listInputEmp[i] + "'";
                        }
                    }

                    if (model.DPRG_NAME == "" || model.DPRG_NAME == "ALL" || model.DPRG_NAME == null)
                    {
                        getPrivilege = $"select PRG_NAME ,FUN, PASSW, PRIVILEGE, EMP FROM SFIS1.C_PRIVILEGE where emp in ({inputValue}) order by PRG_NAME, PRIVILEGE, passw";
                    }
                    else
                    {
                        getPrivilege = $"SELECT PRG_NAME ,FUN, PASSW, PRIVILEGE, EMP FROM SFIS1.C_PRIVILEGE WHERE EMP IN ({inputValue}) AND PRG_NAME ='{model.DPRG_NAME}' ORDER BY PRG_NAME";
                    }
                }
                else if (model.value == "false")
                {
                    if (model.DPRG_NAME == "" || model.DPRG_NAME == "ALL" || model.DPRG_NAME == null)
                    {
                        getPrivilege = $"select PRG_NAME ,fun, passw, PRIVILEGE from SFIS1.C_PRIVILEGE where emp in (select regexp_substr ('{model.EMP_COPYDF}','^[^_]+') from dual) order by PRG_NAME, PRIVILEGE, passw";
                    }
                    else
                    {
                        getPrivilege = $"SELECT PRG_NAME ,FUN, PASSW, PRIVILEGE FROM SFIS1.C_PRIVILEGE WHERE EMP IN (SELECT REGEXP_SUBSTR ('{model.EMP_COPYDF}','^[^_]+') FROM DUAL) AND PRG_NAME ='{model.DPRG_NAME}' ORDER BY PRG_NAME";
                    }
                }
                else
                {
                    if (model.DPRG_NAME == "" || model.DPRG_NAME == "ALL" || model.DPRG_NAME == null)
                    {
                        getPrivilege = $"select PRG_NAME ,fun, passw, PRIVILEGE from SFIS1.C_PRIVILEGE where emp in (select regexp_substr ('{model.EMP_NO_NAME}','^[^_]+') from dual) order by PRG_NAME, PRIVILEGE, passw";
                    }
                    else
                    {
                        getPrivilege = $"SELECT PRG_NAME ,FUN, PASSW, PRIVILEGE FROM SFIS1.C_PRIVILEGE WHERE EMP IN (SELECT REGEXP_SUBSTR ('{model.EMP_NO_NAME}','^[^_]+') FROM DUAL) AND PRG_NAME ='{model.DPRG_NAME}' ORDER BY PRG_NAME";
                    }
                }

                DataTable dtPrivilege = DBConnect.GetData(getPrivilege, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtPrivilege });
            } catch (SqlException ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Database error", message = ex.Message });
            } catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }
        [System.Web.Http.Route("InsertDeleteDefineApp")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertDeleteDefineApp(Config7Element model)
        {
            try
            {
                var connectionString = new GetString().Get()[model.database_name];
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            
                            if ((model.LISTGROUPDF == null || model.LISTGROUPDF.Count == 0) && (model.LISTGROUPNOTDF == null || model.LISTGROUPNOTDF.Count == 0))
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, new { result = "There's nothing to save" });
                            }
                            if(model.LISTGROUPDF != null && model.LISTGROUPDF.Count > 0 && model.valueInput == "true")
                            {
                                if(model.LISTEMP == null || model.LISTEMP.Count == 0)
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "List emp error!" });
                                }
                                string inputValue = "";
                                for (int i = 0; i < model.LISTEMP.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        inputValue += "'" + model.LISTEMP[i].EMP + "'";
                                    }
                                    else
                                    {
                                        inputValue += ",'" + model.LISTEMP[i].EMP + "'";
                                    }
                                }
                                foreach (var group in model.LISTGROUPDF)
                                {
                                    string emp_no = model.EMP_COPYNOTDF.Split('_')[0];
                                    string passw = string.IsNullOrEmpty(group.PASSW) ? null : group.PASSW;
                                    string fun = string.IsNullOrEmpty(group.FUN) ? null : group.FUN;
                                    string privilege = group.PRIVILEGE ?? "0";
                                    string prgName = string.IsNullOrEmpty(group.PRG_NAME) ? null : group.PRG_NAME;

                                    string SqlInsertMultiple = $"INSERT INTO SFIS1.C_PRIVILEGE SELECT A.EMP_NO AS EMP, B.PASSW, B.FUN, B.PRIVILEGE, B.PRG_NAME FROM SFIS1.C_EMP_DESC_T A, SFIS1.C_PRIVILEGE B" +
                                    $" WHERE A.EMP_NO IN({inputValue}) AND B.EMP = '{emp_no}' AND " +
                                    (prgName == null ? "B.PRG_NAME IS NULL" : "B.PRG_NAME = :PRG_NAME") + " AND " +
                                    (fun == null ? "B.FUN IS NULL" : "B.FUN = :FUN") + " AND " +
                                    (passw == null ? "B.PASSW IS NULL" : "PASSW = :PASSW") +
                                    " AND B.PRIVILEGE = :PRIVILEGE";
                                    using (var command = new OracleCommand(SqlInsertMultiple, conn))
                                    {
                                        if(prgName != null)
                                        {
                                            command.Parameters.Add(new OracleParameter("PRG_NAME", prgName));
                                        }
                                        if(passw != null)
                                        {
                                            command.Parameters.Add(new OracleParameter("PASSW", passw));
                                        }
                                        if (fun != null)
                                        {
                                            command.Parameters.Add(new OracleParameter("FUN", fun));
                                        }
                                        command.Parameters.Add(new OracleParameter("PRIVILEGE", privilege));
                                        await command.ExecuteNonQueryAsync();
                                    }
                                }
                            }
                            else if(model.LISTGROUPDF != null && model.LISTGROUPDF.Count > 0)
                            {
                                string sqlInsert = "INSERT INTO SFIS1.C_PRIVILEGE (EMP, PASSW, FUN, PRIVILEGE, PRG_NAME) VALUES (:EMP, :PASSW, :FUN, :PRIVILEGE, :PRG_NAME)";
                                foreach (var group in model.LISTGROUPDF)
                                {
                                    string emp;
                                    if(model.value == "false")
                                    {
                                         emp = model.EMP_COPYDF.Split('_')[0];
                                    }
                                    else
                                    {
                                         emp = model.EMP_NO_NAME.Split('_')[0];
                                    }
                                    string passw = string.IsNullOrEmpty(group.PASSW) ? null : group.PASSW;
                                    string fun = string.IsNullOrEmpty(group.FUN) ? null : group.FUN;
                                    string privilege = group.PRIVILEGE ?? "0";
                                    string prgName = group.PRG_NAME;
                                    string sql = sqlInsert;

                                    using (var command = new OracleCommand(sql, conn))
                                    {
                                        command.Parameters.Add(new OracleParameter("EMP", emp));
                                        command.Parameters.Add(new OracleParameter("PASSW", (object)passw ?? DBNull.Value));
                                        command.Parameters.Add(new OracleParameter("FUN", (object)fun ?? DBNull.Value));
                                        command.Parameters.Add(new OracleParameter("PRIVILEGE", privilege));
                                        command.Parameters.Add(new OracleParameter("PRG_NAME", prgName));
                                        command.Transaction = transaction;
                                        await command.ExecuteNonQueryAsync();
                                    }
                                    // Insert log
                                    string logSql = "INSERT INTO SFISM4.R_SYSTEM_LOG_T (EMP_NO, PRG_NAME, ACTION_TYPE, ACTION_DESC) VALUES (:EMP_NO, 'CONFIG', 'INSERT', :ACTION_DESC)";
                                    using (var logCommand = new OracleCommand(logSql, conn))
                                    {
                                        logCommand.Parameters.Add(new OracleParameter("EMP_NO", model.EMP_NO));
                                        logCommand.Parameters.Add(new OracleParameter("ACTION_DESC", $"Config7 PRIVILEGE  EMP: {emp}, PASSW: {passw}, FUN: {fun}, PRIVILEGE: {privilege}, PRG_NAME: {prgName}, IP:{AuthorizationController.UserIP()}"));
                                        logCommand.Transaction = transaction;
                                        await logCommand.ExecuteNonQueryAsync();
                                    }
                                }
                            }
                            //delete
                            else if(model.LISTGROUPNOTDF != null && model.LISTGROUPNOTDF.Count > 0)
                            {
                                foreach (var group in model.LISTGROUPNOTDF)
                                {
                                    string emp;
                                    if (model.value == "false")
                                    {
                                        emp = model.EMP_COPYDF.Split('_')[0];
                                    }
                                    else
                                    {
                                        emp = model.EMP_NO_NAME.Split('_')[0];
                                    }
                                    string passw = string.IsNullOrEmpty(group.PASSW) ? null : group.PASSW;
                                    string fun = string.IsNullOrEmpty(group.FUN) ? null : group.FUN;
                                    string privilege = group.PRIVILEGE ?? "0";
                                    string prgName = group.PRG_NAME;

                                    string sqlDelete = "DELETE SFIS1.C_PRIVILEGE WHERE EMP = :EMP AND " +
                                        (passw == null ? "PASSW IS NULL" : "PASSW = :PASSW") + " AND " +
                                        (fun == null ? "FUN IS NULL" : "FUN = :FUN") + 
                                        " AND PRIVILEGE = :PRIVILEGE AND PRG_NAME = :PRG_NAME";

                                    using (var command = new OracleCommand(sqlDelete, conn))
                                    {
                                        command.Parameters.Add(new OracleParameter("EMP", emp));
                                        if(passw != null)
                                        {
                                            command.Parameters.Add(new OracleParameter("PASSW", passw));
                                        }
                                        if(fun != null)
                                        {
                                            command.Parameters.Add(new OracleParameter("FUN", fun));
                                        }
                                        command.Parameters.Add(new OracleParameter("PRIVILEGE", privilege));
                                        command.Parameters.Add(new OracleParameter("PRG_NAME", prgName));
                                        command.Transaction = transaction;
                                        await command.ExecuteNonQueryAsync();
                                    }
                                    // Insert log
                                    string logSql = "INSERT INTO SFISM4.R_SYSTEM_LOG_T (EMP_NO, PRG_NAME, ACTION_TYPE, ACTION_DESC) VALUES (:EMP_NO, 'CONFIG', 'DELETE', :ACTION_DESC)";
                                    using (var logCommand = new OracleCommand(logSql, conn))
                                    {
                                        logCommand.Parameters.Add(new OracleParameter("EMP_NO", model.EMP_NO));
                                        logCommand.Parameters.Add(new OracleParameter("ACTION_DESC", $"Config7 PRIVILEGE  EMP: {emp}, PASSW: {passw}, FUN: {fun}, PRIVILEGE: {privilege}, PRG_NAME: {prgName}, IP:{AuthorizationController.UserIP()}"));
                                        logCommand.Transaction = transaction;
                                        await logCommand.ExecuteNonQueryAsync();
                                    }
                                }
                            }
                                transaction.Commit();
                                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
                        }
                        catch (Exception ex)
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Transaction rolled back due to error: " + ex.Message });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Internal serve error: " + ex.Message });
            }
        }
        #endregion
    }
}