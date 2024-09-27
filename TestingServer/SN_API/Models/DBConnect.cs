using Oracle.ManagedDataAccess.Client;
using SN_API.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class DBConnect
    {
        public static OracleConnection oracleConnection;
        public DBConnect (string dbName)
        {
            oracleConnection = new DbContext().Connect(dbName);
        }
        public static void ExecuteNoneQuery(string queryString, string dbName)
        {
            var conn = new DbContext().Connect(dbName);           
            using (var cmd = new OracleCommand(queryString, conn))
            {
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
        public static DataTable GetData(string queryString, string dbName)
        {
            var conn = new DbContext().Connect(dbName);
            DataTable dt = new DataTable();         
            using (var cmd = new OracleCommand(queryString, conn))
            {
                try
                {
                    dt.Load(cmd.ExecuteReader());
                }
                catch (Exception ex)
                {
                    string ass = ex.Message;
                }
            }
            conn.Close();
            return dt;
        }        
        public static void BuildCommand(OracleCommand comm, params OracleParameter[] parameter)
        {
            if (parameter != null && parameter.Length > 0)
            {
                foreach (OracleParameter p in parameter)
                {
                    comm.Parameters.Add(p);
                }
            }
        }
        public static string RunProcedure(string proName, OracleConnection conn, params OracleParameter[] parameter)
        {
            string result = null;
            OracleCommand cmd = new OracleCommand(proName, conn);
            BuildCommand(cmd, parameter);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            object obj = cmd.Parameters[parameter[parameter.Length - 1].ParameterName].Value;
            if (!(Object.Equals(obj, null)) && !(Object.Equals(obj, System.DBNull.Value)))
            {
                result = obj.ToString();
            }
            cmd.Dispose();

            return cmd.Parameters[parameter[parameter.Length - 1].ParameterName].Value.ToString();
        }
        public Hashtable ExecuteSPReturnHashtable(string procedureName, Hashtable hashtable)
        {
            OracleCommand oracleCommand = new OracleCommand(procedureName, oracleConnection);
            OracleDataAdapter oracleDataAdapter = new OracleDataAdapter();
            Hashtable result;
            try
            {
                OracleParameter[] spparamList = GetSPParamList(procedureName, hashtable);
                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.Parameters.Clear();
                oracleCommand.Parameters.AddRange(spparamList);
                bool flag = oracleConnection.State != ConnectionState.Open;
                if (flag)
                {
                    oracleConnection.Open();
                }
                DataSet dataSet = new DataSet();
                oracleDataAdapter.SelectCommand = oracleCommand;
                oracleDataAdapter.Fill(dataSet);
                Hashtable hashtable2 = new Hashtable();
                foreach (object obj in oracleCommand.Parameters)
                {
                    OracleParameter oracleParameter = (OracleParameter)obj;
                    bool flag2 = oracleParameter.ParameterName == "RES_TABLE";
                    if (flag2)
                    {
                        hashtable2.Add(oracleParameter.ParameterName, dataSet.Tables[0]);
                    }
                    else
                    {
                        hashtable2.Add(oracleParameter.ParameterName, oracleParameter.Value);
                    }
                }
                result = hashtable2;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in " + procedureName + " :" + ex.Message);
            }
            finally
            {
                oracleDataAdapter.Dispose();
                oracleCommand.Dispose();
                oracleConnection.Close();
                oracleConnection.Dispose();
                OracleConnection.ClearPool(oracleConnection);
            }
            return result;
        }

        private static OracleParameter[] GetSPParamList(string procedureName, Hashtable hashtable)
        {
            DataTable dataTable = new DataTable();
            string arg = "MES1";
            string arg2 = procedureName;
            string[] array = procedureName.Split(new char[]
            {
                '.'
            });
            bool flag = array.Length == 2;
            if (flag)
            {
                arg = array[0];
                arg2 = array[1];
            }
            Hashtable hashtable2 = new Hashtable();
            foreach (object obj in hashtable.Keys)
            {
                string text = (string)obj;
                bool flag2 = !hashtable2.ContainsKey(text.ToUpper());
                if (flag2)
                {
                    hashtable2.Add(text.ToUpper(), hashtable[text]);
                }
            }
            string stringSQL = string.Format("SELECT UPPER (ARGUMENT_NAME)     COLUMN_NAME,\r\n                                                     DATA_TYPE,\r\n                                                     IN_OUT\r\n                                                FROM ALL_ARGUMENTS\r\n                                               WHERE OWNER = '{0}' AND OBJECT_NAME = '{1}'\r\n                                            ORDER BY SEQUENCE", arg, arg2);
            dataTable = ExecuteDataTable(stringSQL);
            bool flag3 = dataTable.Rows.Count == 0;
            if (flag3)
            {
                throw new Exception("Invalid SP Name, Please check SP: " + procedureName);
            }
            OracleParameter[] array2 = new OracleParameter[dataTable.Rows.Count];
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                array2[i] = new OracleParameter(dataTable.Rows[i]["COLUMN_NAME"].ToString(), DBNull.Value);
                bool flag4 = hashtable2.ContainsKey(array2[i].ParameterName);
                if (flag4)
                {
                    array2[i].Value = hashtable2[array2[i].ParameterName];
                }
                bool flag5 = dataTable.Rows[i]["IN_OUT"].ToString().ToUpper() == "OUT";
                if (flag5)
                {
                    array2[i].Direction = ParameterDirection.Output;
                    string text2 = dataTable.Rows[i]["DATA_TYPE"].ToString().ToUpper();
                    string text3 = text2;
                    if (text3 == null)
                    {
                        goto IL_20B;
                    }
                    if (!(text3 == "NUMBER"))
                    {
                        if (!(text3 == "REF CURSOR"))
                        {
                            goto IL_20B;
                        }

                        array2[i].OracleDbType = Oracle.ManagedDataAccess.Client.OracleDbType.RefCursor;// OracleType.Cursor;
                    }
                    else
                    {
                        array2[i].OracleDbType = Oracle.ManagedDataAccess.Client.OracleDbType.Int32;// OracleType.Number;
                    }
                    goto IL_22B;
                IL_20B:
                    array2[i].OracleDbType = OracleDbType.Varchar2;//  OracleType.VarChar;
                    array2[i].Size = 2000;
                }
            IL_22B:;
            }
            return array2;
        }

        public static DataTable ExecuteDataTable(string stringSQL)
        {
            OracleCommand oracleCommand = new OracleCommand();
            OracleDataAdapter oracleDataAdapter = new OracleDataAdapter();
            DataTable result;
            try
            {
                bool flag = oracleConnection.State != ConnectionState.Open;
                if (flag)
                {
                    oracleConnection.Open();
                }
                oracleCommand.Connection = oracleConnection;
                oracleCommand.CommandText = stringSQL;
                oracleCommand.CommandType = CommandType.Text;
                oracleDataAdapter.SelectCommand = oracleCommand;
                DataTable dataTable = new DataTable();
                oracleDataAdapter.Fill(dataTable);
                oracleCommand.Parameters.Clear();
                oracleConnection.Close();
                //oracleConnection.Dispose();
                result = dataTable;
            }
            catch
            {
                oracleConnection.Close();
                throw;
            }
            finally
            {
                bool flag2 = oracleConnection.State == ConnectionState.Open;
                if (flag2)
                {
                    oracleConnection.Close();
                }
            }
            return result;
        }
    }
}