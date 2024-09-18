using SN_API.Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data;
using SN_API.Models;
using System.Text;

namespace SN_API.Controllers.Config
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class Config2Controller : ApiController
    {
        [System.Web.Http.Route("GetTreeViewConfig2")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetTreeViewConfig2(Config2Element model)
        {
            string getsql = string.Empty;
            if (model.ACTION == "DETAIL")
            {
                getsql = $"SELECT GROUP_NAME,GROUP_DESC FROM SFIS1.C_GROUP_CONFIG_T WHERE SECTION_NAME='{model.SECTION_NAME}' AND GROUP_NAME='{model.GROUP_NAME}' order by group_name";
            }
            else
            {
                getsql = $"SELECT GROUP_NAME FROM SFIS1.C_GROUP_CONFIG_T WHERE SECTION_NAME='{model.SECTION_NAME}' order by group_name";
            }

            DataTable dt = DBConnect.GetData(getsql, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dt });
        }
        [System.Web.Http.Route("GetSectionNameConfig2")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetSectionNameConfig2(Config2Element model)
        {
            string getsectionsql = string.Empty;
            if (model.ACTION == "DETAIL")
            {
                getsectionsql = $"select SECTION_DESC from sfis1.c_section_config_t where section_name='{model.SECTION_NAME}' AND ROWNUM=1 ";
            }
            else
            {
                //getsectionsql = "SELECT distinct SECTION_NAME FROM SFIS1.C_GROUP_CONFIG_T  order by SECTION_NAME asc";
                getsectionsql = "SELECT distinct SECTION_NAME FROM sfis1.c_section_config_t  order by SECTION_NAME asc";
            }
            DataTable dt = DBConnect.GetData(getsectionsql, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dt });
        }
        [System.Web.Http.Route("InsertUpdateConfig2")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateConfig2(Config2Element model)
        {
            try
            {
                string sql_str = string.Empty;
                string check_data = string.Empty;
                string check_privilege = string.Empty;
                string update_work = string.Empty;
                string update_group = string.Empty;
                string update_section = string.Empty;
                string update_station = string.Empty;
                string sql_temp = string.Empty;
                DataTable dt_check_prive = new DataTable();
                DataTable dt_check_data = new DataTable();
                DataTable dt = new DataTable();
                StringBuilder sb = new StringBuilder();
                StringBuilder sb_log = new StringBuilder();
                if (model.ACTION.Contains("INSERT"))
                {
                    check_privilege = $"SELECT * FROM SFIS1.C_PRIVILEGE WHERE EMP='{model.EMP}' AND PRG_NAME ='CONFIG' AND FUN = 'SECTION/GROUP_ADD' ";
                    dt_check_prive = DBConnect.GetData(check_privilege, model.database_name);
                    if (dt_check_prive.Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                }
                else if (model.ACTION.Contains("UPDATE"))
                {
                    check_privilege = $"SELECT * FROM SFIS1.C_PRIVILEGE WHERE EMP='{model.EMP}' AND PRG_NAME ='CONFIG' AND FUN = 'SECTION/GROUP_EDIT' ";
                    dt_check_prive = DBConnect.GetData(check_privilege, model.database_name);
                    if (dt_check_prive.Rows.Count <= 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                    }
                }

                if (model.ACTION == "INSERT_SECTION")
                {
                    check_data = $" select * from   sfis1.c_section_config_t  where SECTION_NAME='{model.SECTION_NAME_NEW}' ";
                    dt_check_data = DBConnect.GetData(check_data, model.database_name);
                    if (dt_check_data.Rows.Count > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = $"{model.SECTION_NAME_NEW} ĐÃ TỒN TẠI TRONG HỆ THỐNG" });
                    }
                    sql_temp = "select max(SECTION_CODE) SECTION_CODE from   sfis1.c_section_config_t";
                    dt = DBConnect.GetData(sql_temp, model.database_name);
                    int section_code = Int32.Parse(dt.Rows[0]["SECTION_CODE"].ToString()) + 1;

                    sb.Clear();
                    sb.Append("insert into sfis1.c_section_config_t ");
                    sb.Append(" values ");
                    sb.Append($"( {section_code}, '{model.SECTION_NAME_NEW}', 'A', {section_code}, 0, '{model.SECTION_NAME_DESC}')");
                }
                else if (model.ACTION == "UPDATE_SECTION")
                {
                    sb.Clear();
                    sb.Append("update sfis1.c_section_config_t");
                    sb.Append($" set section_name = '{model.SECTION_NAME_NEW}',");
                    sb.Append($" section_desc = '{model.SECTION_NAME_DESC}'");
                    sb.Append($" where section_name = '{model.SECTION_NAME}'");



                    update_section = $"UPDATE SFIS1.C_WORK_DESC_T SET SECTION_NAME='{model.SECTION_NAME_NEW}'"
                        + $" WHERE SECTION_NAME='{model.SECTION_NAME}'";
                    DBConnect.ExecuteNoneQuery(update_section, model.database_name);

                    update_group = $" UPDATE SFIS1.C_GROUP_CONFIG_T SET SECTION_NAME='{model.SECTION_NAME_NEW}'  "
                        + $" WHERE SECTION_NAME='{model.SECTION_NAME}'";
                    DBConnect.ExecuteNoneQuery(update_group, model.database_name);

                    update_station = $"UPDATE SFIS1.C_STATION_CONFIG_T SET SECTION_NAME='{model.SECTION_NAME_NEW}' "
                        + $" WHERE SECTION_NAME='{model.SECTION_NAME}'";

                    DBConnect.ExecuteNoneQuery(update_station, model.database_name);

                }
                else if (model.ACTION == "INSERT_GROUP")
                {
                    check_data = $"select * from  sfis1.c_group_config_t where section_name='{model.SECTION_NAME}' and group_name='{model.GROUP_NAME}'";
                    dt_check_data = DBConnect.GetData(check_data, model.database_name);
                    if (dt_check_data.Rows.Count > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = $" GROUP :  {model.GROUP_NAME} ĐÃ TỒN TẠI TRONG HỆ THỐNG" });
                    }

                    sql_temp = $"select max(step) STEP from SFIS1.C_GROUP_CONFIG_T where SECTION_NAME='SC'";
                    dt = DBConnect.GetData(sql_temp, model.database_name);
                    int step = Int32.Parse(dt.Rows[0]["STEP"].ToString()) + 1;

                    sql_temp = $"select  max(group_code) GROUP_CODE from SFIS1.C_GROUP_CONFIG_T ";
                    dt = DBConnect.GetData(sql_temp, model.database_name);
                    int group_code = Int32.Parse(dt.Rows[0]["GROUP_CODE"].ToString()) + 1;

                    sb.Clear();
                    sb.Append($" insert into sfis1.c_group_config_t");
                    sb.Append($"( group_code, group_name, section_name, step, group_type, group_desc) ");
                    sb.Append($" values ");
                    sb.Append($" ( {group_code}, '{model.GROUP_NAME}', '{model.SECTION_NAME}', {step}, 1, '{model.GROUP_NAME_DESC}') ");
                }
                else if (model.ACTION == "UPDATE_GROUP")
                {

                    check_data = $"select * from  sfis1.c_group_config_t where section_name='{model.SECTION_NAME}' and group_name='{model.GROUP_NAME_NEW}'";
                    dt_check_data = DBConnect.GetData(check_data, model.database_name);
                    if (dt_check_data.Rows.Count > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = $"GROUP : {model.GROUP_NAME_NEW} ĐÃ TỒN TẠI TRONG HỆ THỐNG" });
                    }
                    sb.Clear();
                    sb.Append($"update sfis1.c_group_config_t ");
                    sb.Append($" set group_type = 1, ");
                    sb.Append($" group_name = '{model.GROUP_NAME_NEW}', ");
                    sb.Append($" group_desc = '{model.GROUP_NAME_DESC}' ");
                    sb.Append($" where section_name = '{model.SECTION_NAME}' ");
                    sb.Append($" and group_name = '{model.GROUP_NAME}'");

                    //update_group = $"UPDATE SFIS1.C_GROUP_CONFIG_T SET GROUP_NAME='{model.GROUP_NAME_NEW}' "
                    //    + $" WHERE GROUP_NAME='{model.GROUP_NAME}' ";

                    //DBConnect.ExecuteNoneQuery(update_group, model.database_name);

                    //update group trong bang bom_key_part_t
                    update_group = $" UPDATE SFIS1.C_BOM_KEYPART_T SET GROUP_NAME='{model.GROUP_NAME_NEW}' "
                        + $" WHERE GROUP_NAME='{model.GROUP_NAME}'";
                    DBConnect.ExecuteNoneQuery(update_group, model.database_name);

                    //update group trong bang route_control
                    update_group = $"UPDATE SFIS1.C_ROUTE_CONTROL_T SET GROUP_NAME='{model.GROUP_NAME_NEW}' "
                        + $" WHERE GROUP_NAME='{model.GROUP_NAME}'";
                    DBConnect.ExecuteNoneQuery(update_group, model.database_name);

                    //update group trong kanban_setup
                    // update_group = $"UPDATE SFIS1.C_KANBAN_SETUP SET GROUP_NAME='{model.GROUP_NAME_NEW}'"
                    //     + $" WHERE GROUP_NAME='{model.GROUP_NAME}' ";
                    //  DBConnect.ExecuteNoneQuery(update_group, model.database_name);

                    //update group trong station_config_t
                    update_group = $" UPDATE SFIS1.C_STATION_CONFIG_T SET GROUP_NAME='{model.GROUP_NAME_NEW}' "
                        + $" WHERE GROUP_NAME='{model.GROUP_NAME}' and SECTION_NAME='{model.SECTION_NAME}' ";
                    DBConnect.ExecuteNoneQuery(update_group, model.database_name);

                }
                sb_log.Clear();
                sb_log.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sb_log.Append(" VALUES ( ");
                sb_log.Append($" '{model.EMP}', ");
                sb_log.Append($" 'CONFIG', ");
                sb_log.Append($" '{model.ACTION}', ");
                sb_log.Append($"  'Config2 SECTION/GROUP: ");
                sb_log.Append($"  {model.EMP},;IP:{AuthorizationController.UserIP()}; ' ");
                sb_log.Append(" ) ");

                sql_str = sb.ToString();
                sql_temp = sb_log.ToString();
                DBConnect.ExecuteNoneQuery(sql_str, model.database_name);
                DBConnect.ExecuteNoneQuery(sql_temp, model.database_name);

                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }

    }
}
