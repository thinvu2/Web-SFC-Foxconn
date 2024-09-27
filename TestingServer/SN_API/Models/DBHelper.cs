using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class DBHelper
    {
        private static OracleConnection connection = null;

        //返回一個已經打開的連接
        public static OracleConnection getConnection()
        {
            if (connection != null)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                return connection;
            }

            connection = new OracleConnection("");
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }

        public static List<object> executeProcedureList(string procedureName, params OracleParameter[] parameters)
        {
            Dictionary<string, object> dicResult = executeProcedureDic(procedureName, parameters);
            return dicResult.Values.ToList();
        }

        public static Dictionary<string, object> executeProcedureDic(string procedureName, params OracleParameter[] parameters)
        {
            Dictionary<string, object> dicResult = new Dictionary<string, object>();
            OracleConnection connection = getConnection();
            connection.BeginTransaction();

            OracleCommand cmd = new OracleCommand()
            {
                CommandText = procedureName,
                Connection = connection,
                CommandType = CommandType.StoredProcedure
            };

            parameters.ToList().ForEach(parameter =>
            {
                cmd.Parameters.Add(parameter);
            });

            try
            {
                cmd.ExecuteNonQuery();

                parameters.ToList().ForEach(parameter =>
                {
                    if (parameter.Direction.Equals(System.Data.ParameterDirection.Output))
                    {
                        dicResult.Add(parameter.ParameterName, parameter.Value);
                    }
                });
                cmd.Transaction.Commit();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                cmd.Transaction.Rollback();
                throw e;
            }
            finally
            {
                connection.Close();
            }


            return dicResult;
        }


        //獲取到 SFIS1.TE_CALL_SFC 的所有參數信息
        public void getSchema()
        {
            var restrictions = new string[2];
            //restrictions[0] = "TE_CALL_SFC";
            restrictions[0] = "SFIS1";
            DataTable dt = connection.GetSchema("ProcedureParameters", restrictions);
            int count = dt.Rows.Count;
            DataView dv1 = dt.DefaultView;
            dv1.RowFilter = "OBJECT_NAME='TE_CALL_SFC'";
        }

        public static string executeProcedure(string procedureName, List<OracleParameter> parameters)
        {
            Dictionary<string, object> dicResult = executeProcedureDic(procedureName, parameters.ToArray());
            OracleParameter outParameter = parameters.FindLast(t => t.Direction == ParameterDirection.Output);
            if (outParameter != null && dicResult.ContainsKey(outParameter.ParameterName))
            {
                return dicResult[outParameter.ParameterName].ToString();
            }
            else
            {
                return "Forget to define the out parameter or parameter name is different?";
            }

            //OracleConnection connection = getConnection();
            //string result = string.Empty;
            //connection.BeginTransaction();

            //OracleCommand cmd = new OracleCommand()
            //{
            //    CommandText = procedureName,
            //    Connection = connection,
            //    CommandType = CommandType.StoredProcedure
            //};

            //parameters.ForEach(parameter =>
            //{
            //    cmd.Parameters.Add(parameter);
            //});

            //try
            //{
            //    cmd.ExecuteNonQuery();

            //    parameters.ToList().ForEach(parameter =>
            //    {
            //        if (parameter.Direction.Equals(System.Data.ParameterDirection.Output))
            //        {
            //            result = parameter.Value.ToString();
            //            return;
            //        }
            //    });
            //    cmd.Transaction.Commit();

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //    cmd.Transaction.Rollback();
            //    throw e;
            //    //return "Execute procedure failed: "+e.Message;
            //}
            //finally
            //{
            //    connection.Close();
            //}
            //return result;
        }
    }
}