using Entities.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
    class SQLHelper: IDBHelper
    {
        #region variables
        string connectionString = string.Empty;
        SqlCommand sqlCommand = null;
        SqlConnection sqlConnection = null;
        #endregion

        #region Constructors

        public SQLHelper(string commandText)
        {
            PrepareCommand(commandText, CommandType.StoredProcedure);
        }

        public SQLHelper(string commandText, CommandType commandType)
        {
            PrepareCommand(commandText, commandType);
        }

        public SQLHelper(string connectionString, string commandText)
        {
            this.connectionString = connectionString;
            PrepareCommand(commandText, CommandType.StoredProcedure);
        }

        public SQLHelper(string connectionString, string commandText, CommandType commandType)
        {
            this.connectionString = connectionString;
            PrepareCommand(commandText, commandType);
        }
        #endregion

        #region General Methods

        private void PrepareCommand(string commandText, CommandType commandType)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    //Use default connection string
                    connectionString = SystemConnectionString.GetConnectionString();
                }

                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                sqlCommand = new SqlCommand();

                // Associate the connection with the command
                sqlCommand.Connection = sqlConnection;

                if (string.IsNullOrEmpty(commandText))
                {
                    throw new ArgumentNullException("Command Text");
                }

                // Set the command type
                sqlCommand.CommandType = commandType;

                // Set the command text (stored procedure name or SQL statement)
                sqlCommand.CommandText = commandText;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddParameter(string parameterName, object parameterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(parameterName))
                {
                    throw new ArgumentNullException("Parameter Name");
                }
                if (parameterValue == null)
                {
                    sqlCommand.Parameters.AddWithValue(parameterName, DBNull.Value);
                }
                else
                {
                    Type type = parameterValue.GetType();
                    if (type.Equals(typeof(DateTime)))
                    {
                        if ((DateTime)parameterValue == new DateTime())
                        {
                            sqlCommand.Parameters.AddWithValue(parameterName, DBNull.Value);
                        }
                        else
                        {
                            sqlCommand.Parameters.AddWithValue(parameterName, parameterValue);
                        }
                    }
                    else
                    {
                        if (type.BaseType.Equals(typeof(Enum)))
                        {
                            if ((int)parameterValue == -1)
                            {
                                sqlCommand.Parameters.AddWithValue(parameterName, DBNull.Value);
                            }
                            else
                            {
                                sqlCommand.Parameters.AddWithValue(parameterName, parameterValue);
                            }
                        }
                        else
                        {
                            sqlCommand.Parameters.AddWithValue(parameterName, parameterValue);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateParameter(string parameterName, object parameterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(parameterName) || sqlCommand.Parameters[parameterName] == null)
                {
                    throw new ArgumentNullException("Parameter Name");
                }
                if (parameterValue == null)
                {
                    sqlCommand.Parameters[parameterName].Value = DBNull.Value;
                }
                else
                {
                    Type type = parameterValue.GetType();
                    if (type.Equals(typeof(DateTime)))
                    {
                        if ((DateTime)parameterValue == new DateTime())
                        {
                            sqlCommand.Parameters[parameterName].Value = DBNull.Value;
                        }
                        else
                        {
                            sqlCommand.Parameters[parameterName].Value = parameterValue;
                        }
                    }
                    else
                    {
                        if (type.BaseType.Equals(typeof(Enum)))
                        {
                            if ((int)parameterValue == -1)
                            {
                                sqlCommand.Parameters[parameterName].Value = DBNull.Value;
                            }
                            else
                            {
                                sqlCommand.Parameters[parameterName].Value = parameterValue;
                            }
                        }
                        else
                        {
                            sqlCommand.Parameters[parameterName].Value = parameterValue;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Execute Non Query
        public bool ExecuteNonQuery()
        {
            try
            {
                return sqlCommand.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<bool> ExecuteNonQuerys(List<Dictionary<string, object>> paramList)
        {
            try
            {
                List<bool> resultList = new List<bool>();
                using (SqlTransaction dbTrans = sqlCommand.Connection.BeginTransaction())
                {
                    sqlCommand.Transaction = dbTrans;
                    int counter = 0;
                    foreach (Dictionary<string, object> parameters in paramList)
                    {
                        if (counter == 0)
                        {
                            foreach (var item in parameters)
                            {
                                AddParameter(item.Key, item.Value);
                            }
                        }
                        else
                        {
                            foreach (var item in parameters)
                            {
                                UpdateParameter(item.Key, item.Value);
                            }
                        }

                        if (sqlCommand.ExecuteNonQuery() != 0)
                        {
                            resultList.Add(true);
                        }
                        else
                        {
                            resultList.Add(false);
                        }
                        counter++;
                    }
                    dbTrans.Commit();
                    return resultList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Fill Dataset
        public DataSet FillDataSet()
        {
            try
            {
                DataSet dataSet = new DataSet();
                var dataAdaper = new SqlDataAdapter(sqlCommand);
                dataAdaper.Fill(dataSet);

                return dataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable FillDataTable()
        {
            try
            {
                DataTable dataTable = new DataTable();
                var dataAdapter = new SqlDataAdapter(sqlCommand);
                dataAdapter.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Execute Reader

        public IDataReader ExecuteReader()
        {
            try
            {
                return sqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ExecuteScalar
        public object ExecuteScalar()
        {
            try
            {
                return sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            sqlCommand.Dispose();
            sqlConnection.Close();
            sqlConnection.Dispose();
        }
        #endregion

        #region Destructors
        ~SQLHelper()
        {
            Dispose();
        }
        #endregion
    }
}
