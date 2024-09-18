using Oracle.ManagedDataAccess.Client;
using SN_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class Route
    {
        DBConnect DBconn = new DBConnect();
        public int ROUTE_CODE { set; get; }
        public string GROUP_NAME { set; get; }
        public string GROUP_NEXT { set; get; }
        public int STATE_FLAG { set; get; }
        public int STEP_SEQUENCE { set; get; }
        public string ROUTE_DESC { set; get; }
        public Route(int rOUTE_CODE, string gROUP_NAME, string gROUP_NEXT, int sTATE_FLAG, int sTEP_SEQUENCE, string rOUTE_DESC)
        {
            ROUTE_CODE = rOUTE_CODE;
            GROUP_NAME = gROUP_NAME;
            GROUP_NEXT = gROUP_NEXT;
            STATE_FLAG = sTATE_FLAG;
            STEP_SEQUENCE = sTEP_SEQUENCE;
            ROUTE_DESC = rOUTE_DESC;
        }
        public Route()
        {

        }
        public List<Route> GetRoute(string DB, string type, string RouteCode, string GroupName = null)
        {
            OracleConnection conn = new DbContext().Connect(DB);

            List<Route> list = new List<Route>();

            string queryString = "";
            if (type == "GET-LIST-ROUTE")
            {
                queryString = "SELECT DISTINCT ROUTE_CODE,ROUTE_NAME FROM SFIS1.C_ROUTE_NAME_T" +
                              " WHERE(ROUTE_CODE LIKE '%" + RouteCode + "%' OR ROUTE_NAME LIKE '%" + RouteCode + "%')";
            }
            else if (type == "GET-ROUTE-DATA")
            {
                queryString = "SELECT DISTINCT ROUTE_CODE,GROUP_NAME,GROUP_NEXT,STATE_FLAG,STEP_SEQUENCE FROM SFIS1.C_ROUTE_CONTROL_T" +
                         " WHERE ROUTE_CODE = '" + RouteCode + "' ORDER BY STEP_SEQUENCE";
            }
            else if (type == "GET-ROUTE-INFO")
            {
                queryString = "SELECT DISTINCT ROUTE_CODE,ROUTE_NAME FROM SFIS1.C_ROUTE_NAME_T" +
                 " WHERE ROUTE_CODE = '" + RouteCode + "'";
            }
            else if (type == "GET-NEXT-GROUP")
            {
                try
                {
                    if (GroupName.Substring(0, 2) == "R_")
                    {
                        queryString = "SELECT * FROM SFIS1.C_ROUTE_CONTROL_T" +
                                        "  WHERE ROUTE_CODE = '" + RouteCode + "'" +
                                        "  AND GROUP_NAME = '" + GroupName + "'" +
                                        "  AND STATE_FLAG = 0" +
                                        "  ORDER BY STEP_SEQUENCE ";
                    }
                    else
                    {
                        queryString = "SELECT * FROM( SELECT * FROM SFIS1.C_ROUTE_CONTROL_T" +
                                       "   WHERE ROUTE_CODE = '" + RouteCode + "'" +
                                        "  AND GROUP_NAME = '" + GroupName + "'" +
                                        "  AND STATE_FLAG = 0" +
                                        "  ORDER BY STEP_SEQUENCE) WHERE ROWNUM=1";
                    }
                }
                catch
                {
                    queryString = "SELECT * FROM( SELECT * FROM SFIS1.C_ROUTE_CONTROL_T" +
                                       "   WHERE ROUTE_CODE = '" + RouteCode + "'" +
                                        "  AND GROUP_NAME = '" + GroupName + "'" +
                                        "  AND STATE_FLAG = 0" +
                                        "  ORDER BY STEP_SEQUENCE) WHERE ROWNUM=1";
                }


            }
            else if (type == "GET-R-GROUP")
            {
                queryString = "SELECT * FROM SFIS1.C_ROUTE_CONTROL_T" +
                        " WHERE ROUTE_CODE = '" + RouteCode + "'" +
                        " AND GROUP_NAME = '" + GroupName + "'" +
                        " AND STATE_FLAG = 1" +
                        " ORDER BY STEP_SEQUENCE";
            }
            else if (type == "GET-SPECIAL-GROUP")
            {
                queryString = "SELECT B.* FROM(SELECT * FROM(SELECT * FROM SFIS1.C_ROUTE_CONTROL_T WHERE ROUTE_CODE = '" + RouteCode + "' AND GROUP_NAME = '" + GroupName + "' ORDER BY STEP_SEQUENCE)WHERE  ROWNUM = 1) A," +
                             " SFIS1.C_ROUTE_CONTROL_T B" +
                    "  WHERE B.ROUTE_CODE = '" + RouteCode + "'" +
                     " AND B.GROUP_NAME = '" + GroupName + "'" +
                    "  AND B.STATE_FLAG <> 1" +
                    "  AND B.STEP_SEQUENCE > A.STEP_SEQUENCE ";
            }
            if (conn.State.ToString() == "CLOSED")
            {
                conn.Open();
            }
            first:
            try
            {
                using (var cmd = new OracleCommand(queryString, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string group_next = reader["GROUP_NEXT"].ToString();
                            list.Add(new Route(int.Parse(reader["ROUTE_CODE"].ToString()), reader["GROUP_NAME"].ToString(), reader["GROUP_NEXT"].ToString(), int.Parse(reader["STATE_FLAG"].ToString()), int.Parse(reader["STEP_SEQUENCE"].ToString()), reader["ROUTE_DESC"].ToString()));
                        }
                    }
                }
            }
            catch
            {
                conn.Open();
                goto first;
            }
            conn.Close();
            return list;
        }
    }
}