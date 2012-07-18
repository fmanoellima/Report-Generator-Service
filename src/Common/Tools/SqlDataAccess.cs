using System;
using System.Collections.Generic;

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Common.Tools
{
    public class SqlDataAccess
    {
        private readonly string _strConnection;
        private Database _db;
        private DbConnection _dbConn;
        private DbTransaction _dbTrans;


        #region Properties

        /// <summary>
        /// Gets the Connection String 
        /// </summary>
        public string StrConnection
        {
            get { return _strConnection; }
        }

        /// <summary>
        /// Gets the DataBase Transaction Context
        /// </summary>
        public DbTransaction DbTrans
        {
            get { return _dbTrans; }
            set { _dbTrans = value; }
        }


        #endregion

        #region Constructor

        public SqlDataAccess(string strConnection)
        {
            _strConnection = strConnection;
        }

        #endregion

        #region Transaction Control

        public void BeginTransaction()
        {
            _db = new SqlDatabase(_strConnection);
            _dbConn = _db.CreateConnection();
            _dbConn.Open();
            _dbTrans = _dbConn.BeginTransaction();
        }

        public void Commit()
        {
            _dbTrans.Commit();
        }

        public void Rollback()
        {
            _dbTrans.Rollback();
        }

        #endregion

        #region ExecuteBySqlText Overloads

        public Object ExecuteBySqlText(ReturnType returnType, string sqlText, Dictionary<string, object> sqlValues)
        {
            return ExecuteBySqlText(0, returnType, sqlText, sqlValues);
        }

        public Object ExecuteBySqlText(int timeoutInSeconds, ReturnType returnType, string sqlText, Dictionary<string, object> sqlValues)
        {
            if (_db == null)
                _db = new SqlDatabase(_strConnection);

            Object obj = null;
            try
            {
                DbCommand sqlStringCommand = _db.GetSqlStringCommand(sqlText);

                if (sqlValues != null)
                {
                    foreach (KeyValuePair<string, object> kvp in sqlValues)
                    {
                        _db.AddInParameter(sqlStringCommand, kvp.Key, DbType.String, kvp.Value);
                    }
                }

                if (timeoutInSeconds > 0)
                {
                    sqlStringCommand.CommandTimeout = timeoutInSeconds;
                }

                switch (returnType)
                {
                    case ReturnType.NonQuery:
                        {
                            if (_dbTrans == null)
                                obj = _db.ExecuteNonQuery(sqlStringCommand);
                            else
                                obj = _db.ExecuteNonQuery(sqlStringCommand, _dbTrans);
                            break;
                        }
                    case ReturnType.Scalar:
                        {
                            if (_dbTrans == null)
                                obj = _db.ExecuteScalar(sqlStringCommand);
                            else
                                obj = _db.ExecuteScalar(sqlStringCommand, _dbTrans);
                            break;
                        }
                    case ReturnType.DataTable:
                        {
                            DataTable dataTable = new DataTable();
                            if (_dbTrans == null)
                                dataTable.Load(_db.ExecuteReader(sqlStringCommand));
                            else
                                dataTable.Load(_db.ExecuteReader(sqlStringCommand, _dbTrans));

                            obj = dataTable.Copy();
                            break;
                        }
                    //case ReturnType.DataReader:
                    //    {
                    //        DataSet dSet;
                    //        if (_dbTrans == null)
                    //            dSet = _db.ExecuteDataSet(sqlStringCommand);
                    //        else
                    //            dSet = _db.ExecuteDataSet(sqlStringCommand, _dbTrans);

                    //        if (dSet.Tables.Count > 0)
                    //            obj = dSet.Tables[0].CreateDataReader();
                    //        break;
                    //    }
                    case ReturnType.DataSet:
                        {
                            if (_dbTrans == null)
                                obj = _db.ExecuteDataSet(sqlStringCommand);
                            else
                                obj = _db.ExecuteDataSet(sqlStringCommand, _dbTrans);
                            break;
                        }
                }

                return obj;
            }
            catch (Exception exception)
            {
                string message = exception.ToString();
                try
                {
                    message = CreateErrorXml(exception, _db.ConnectionStringWithoutCredentials, "sqlquery", sqlText, sqlValues, -2147483648);
                }
                catch
                {
                }
                throw new Exception(message);
            }
        }

        #endregion

        #region ExecuteByStoredProc Overloads

        public Object ExecuteByStoredProc(ReturnType returnType, string sqlProcedure, ref Dictionary<string, object> sqlValues)
        {
            return ExecuteByStoredProc(0,returnType, sqlProcedure, ref sqlValues);
        }

        public Object ExecuteByStoredProc(int timeoutInSeconds, ReturnType returnType, string sqlProcedure, ref Dictionary<string, object> sqlValues)
        {
            if (_db == null)
                _db = new SqlDatabase(_strConnection);

            object result = null;
            DbCommand storedProcCommand = null;
            int returnedValue = -2147483648;
            try
            {
                storedProcCommand = _db.GetStoredProcCommand(sqlProcedure);
                _db.DiscoverParameters(storedProcCommand);

                if (sqlValues.Count == 0)
                {
                    for (int i = 1; i < storedProcCommand.Parameters.Count; i++)
                    {
                        storedProcCommand.Parameters[i].Value = null;
                    }
                }
                else
                {
                    //varrendo a lista de parametros da procedure
                    for (int i = 1; i < storedProcCommand.Parameters.Count; i++)
                    {
                        //se o parametro da procedure possuir uma chave na lista de parametros recebidos
                        if (sqlValues.ContainsKey(storedProcCommand.Parameters[i].ParameterName))
                        {
                            //se o valor da chave for nulo
                            if (sqlValues[storedProcCommand.Parameters[i].ParameterName] == null)
                            {
                                storedProcCommand.Parameters[i].Value = DBNull.Value;
                            }
                            //se o tipo do parametro da procedure for Boolean
                            else if (storedProcCommand.Parameters[i].DbType == DbType.Boolean)
                            {
                                if (Convert.ToString(sqlValues[storedProcCommand.Parameters[i].ParameterName]) == "1")
                                {
                                    storedProcCommand.Parameters[i].Value = true;
                                }
                                else if (Convert.ToString(sqlValues[storedProcCommand.Parameters[i].ParameterName]) == "0")
                                {
                                    storedProcCommand.Parameters[i].Value = false;
                                }
                                else
                                {
                                    storedProcCommand.Parameters[i].Value = Convert.ToString(sqlValues[storedProcCommand.Parameters[i].ParameterName]);
                                }
                            }
                            else
                            {
                                storedProcCommand.Parameters[i].Value = sqlValues[storedProcCommand.Parameters[i].ParameterName];
                            }
                        }//fim if
                    }//fim for
                }//fim else

                if (timeoutInSeconds > 0)
                {
                    storedProcCommand.CommandTimeout = timeoutInSeconds;
                }

                switch (returnType)
                {
                    case ReturnType.NonQuery:
                        {
                            if (_dbTrans == null)
                                result = _db.ExecuteNonQuery(storedProcCommand);
                            else
                                result = _db.ExecuteNonQuery(storedProcCommand, _dbTrans);
                            break;
                        }
                    case ReturnType.Scalar:
                        {
                            if (_dbTrans == null)
                                result = _db.ExecuteScalar(storedProcCommand);
                            else
                                result = _db.ExecuteScalar(storedProcCommand, _dbTrans);
                            break;
                        }
                    case ReturnType.DataTable:
                        {
                            using (DataTable dataTable = new DataTable())
                            {
                                if (_dbTrans == null)
                                    dataTable.Load(_db.ExecuteReader(storedProcCommand));
                                else
                                    dataTable.Load(_db.ExecuteReader(storedProcCommand, _dbTrans));

                                result = dataTable.Copy();
                            }
                            break;
                        }
                    //case ReturnType.DataReader:
                    //    {
                    //        DataSet dSet;
                    //        if (_dbTrans == null)
                    //            dSet = _db.ExecuteDataSet(storedProcCommand);
                    //        else
                    //            dSet = _db.ExecuteDataSet(storedProcCommand, _dbTrans);

                    //        if (dSet.Tables.Count > 0)
                    //            result = dSet.Tables[0].CreateDataReader();
                    //        dSet.Clear();
                    //        dSet.Dispose();

                    //        break;
                    //    }
                    case ReturnType.DataSet:
                        {
                            if (_dbTrans == null)
                                result = _db.ExecuteDataSet(storedProcCommand);
                            else
                                result = _db.ExecuteDataSet(storedProcCommand, _dbTrans);
                            break;
                        }
                }

                //Verificando cada parametro da procedure para saber se é um parametro de retorno, ou se é de OUTPUT
                for (int j = 0; j < storedProcCommand.Parameters.Count; j++)
                {
                    if (storedProcCommand.Parameters[j].Direction == ParameterDirection.ReturnValue)
                    {
                        returnedValue = (storedProcCommand.Parameters[j].Value == DBNull.Value) ? 0 : Convert.ToInt32(storedProcCommand.Parameters[j].Value);
                    }
                    if (sqlValues.ContainsKey(storedProcCommand.Parameters[j].ParameterName))
                    {
                        if ((storedProcCommand.Parameters[j].Direction == ParameterDirection.InputOutput) || (storedProcCommand.Parameters[j].Direction == ParameterDirection.Output))
                        {
                            sqlValues[storedProcCommand.Parameters[j].ParameterName] = storedProcCommand.Parameters[j].Value;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                if (storedProcCommand != null)
                {
                    for (int k = 0; k < storedProcCommand.Parameters.Count; k++)
                    {
                        if (storedProcCommand.Parameters[k].Direction == ParameterDirection.ReturnValue)
                        {
                            returnedValue = (storedProcCommand.Parameters[k].Value == DBNull.Value) ? 0 : Convert.ToInt32(storedProcCommand.Parameters[k].Value);
                        }
                        if (sqlValues.ContainsKey(storedProcCommand.Parameters[k].ParameterName))
                        {
                            if ((storedProcCommand.Parameters[k].Direction == ParameterDirection.InputOutput) || (storedProcCommand.Parameters[k].Direction == ParameterDirection.Output))
                            {
                                sqlValues[storedProcCommand.Parameters[k].ParameterName] = storedProcCommand.Parameters[k].Value;
                            }
                        }
                    }
                }
                string message;
                if (_db != null)
                {
                    message = CreateErrorXml(exception, _db.ConnectionStringWithoutCredentials, "sqlproc", sqlProcedure, sqlValues, returnedValue);
                }
                else
                {
                    message = CreateErrorXml(exception, null, "sqlproc", sqlProcedure, sqlValues, returnedValue);
                }
                throw new Exception(message);
            }
            return result;
        }

        #endregion

        private string CreateErrorXml(Exception ex, string sqlConnectionString, string sqlType, string sqlText, Dictionary<string, object> sqlValues, int returnedValue)
        {
            string strSource = "";
            string str2 = "";
            if (!(ex is SqlException))
            {
                string str7 = Xml.Cxml(ex.Message);
                if (str7.Length > 0xdea8)
                {
                    str7 = str7.Substring(0, 0xdea8);
                }
                str2 = str2 + string.Format("<message>{0}</message>", str7);
                goto Label_02B2;
            }
            SqlException exception = (SqlException)ex;
            if (!ex.Message.StartsWith("Error"))
            {
                str2 = (str2 + string.Format("<number>{0}</number>", exception.Number)) + string.Format("<state>{0}</state>", exception.State) + string.Format("<line>{0}</line>", exception.LineNumber);
                string str6 = Xml.Cxml(exception.Message);
                if (str6.Length > 0xdea8)
                {
                    str6 = str6.Substring(0, 0xdea8);
                }
                str2 = str2 + string.Format("<message>{0}</message>", str6);
                goto Label_02B2;
            }
            strSource = exception.Server;
            int index = 0;
        Label_01BA:
            if (index < ex.Message.Split(new[] { ',' }).Length)
            {
                string str3 = ex.Message.Split(new[] { ',' })[index];
                if (str3.IndexOf("Error:") > -1)
                {
                    str2 = str2 + string.Format("<number>{0}</number>", str3.Replace("Error:", "").Trim());
                }
                else if (str3.IndexOf("Level:") > -1)
                {
                    str2 = str2 + string.Format("<level>{0}</level>", str3.Replace("Level:", "").Trim());
                }
                else if (str3.IndexOf("State:") > -1)
                {
                    str2 = str2 + string.Format("<state>{0}</state>", str3.Replace("State:", "").Trim());
                }
                else if (str3.IndexOf("Line:") > -1)
                {
                    str2 = str2 + string.Format("<line>{0}</line>", str3.Replace("Line:", "").Trim());
                }
                else if (str3.IndexOf("Message:") > -1)
                {
                    string str5 = Xml.Cxml(ex.Message.Substring(ex.Message.IndexOf("Message: ") + 9));
                    if (str5.Length > 0xdea8)
                    {
                        str5 = str5.Substring(0, 0xdea8);
                    }
                    str2 = str2 + string.Format("<message>{0}</message>", str5);
                    goto Label_02B2;
                }
                index++;
                goto Label_01BA;
            }
        Label_02B2:
            if ((sqlConnectionString == null) || (sqlConnectionString.Trim().Length == 0))
            {
                str2 = str2 + string.Format("<connectionstring>{0}</connectionstring>", "") + string.Format("<database>{0}</database>", "");
            }
            else
            {
                if (sqlConnectionString.Trim().Length > 0)
                {
                    str2 = str2 + string.Format("<connectionstring>{0}</connectionstring>", Xml.Cxml(sqlConnectionString));
                }
                string str8 = "";
                foreach (string str9 in sqlConnectionString.Split(new[] { ';' }))
                {
                    if (str9.ToLower().Contains("initial catalog"))
                    {
                        if (str9.Split(new[] { '=' }).Length > 1)
                        {
                            str8 = str9.Split(new[] { '=' })[1];
                        }
                    }
                    else if (((strSource.Length == 0) && str9.ToLower().Contains("data source")) && (str9.Split(new[] { '=' }).Length > 1))
                    {
                        strSource = str9.Split(new[] { '=' })[1];
                    }
                }
                str2 = str2 + string.Format("<database>{0}</database>", Xml.Cxml(str8));
            }
            str2 = ((str2 + string.Format("<server>{0}</server>", Xml.Cxml(strSource))) + string.Format("<type>{0}</type>", sqlType) + string.Format("<returnvalue>{0}</returnvalue>", returnedValue)) + string.Format("<sqltext>{0}</sqltext>", Xml.Cxml(sqlText)) + string.Format("<source>{0}</source>", Xml.Cxml(ex.Source));
            string str10 = "<parameters>";

            if (sqlValues != null)
            {
                foreach (KeyValuePair<string, object> kvp in sqlValues)
                {
                    if (kvp.Value == null)
                    {
                        str10 = str10 + string.Format("<parameter>{0}</parameter>", "NULL");
                    }
                    else
                    {
                        str10 = str10 + string.Format("<parameter>{0}</parameter>", Xml.Cxml(kvp.Value.ToString()));
                    }
                }
            }

            str10 = str10 + "</parameters>";
            str2 = str2 + str10;
            if ((str2.Length + Xml.Cxml(ex.ToString()).Length) > 0xfa00)
            {
                str2 = str2 + string.Format("<stacktrace>{0}</stacktrace>", "[stacktrace was removed, it is too long to be shown]");
            }
            else
            {
                str2 = str2 + string.Format("<stacktrace>{0}</stacktrace>", Xml.Cxml(ex.ToString()));
            }
            return ("<error>" + str2 + "</error>");
        }

        #region Inner Types
        [Flags]
        public enum ReturnType
        {
            NonQuery = 0,
            Scalar = 1,
            DataTable = 2,
            //DataReader = 3,
            DataSet = 4
        }
        #endregion
    }
}
