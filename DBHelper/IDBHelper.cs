using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
    public interface IDBHelper: IDisposable
    {
        #region General Methods
        void AddParameter(string parameterName, object parameterValue);
        #endregion

        #region Execute Non Query
        bool ExecuteNonQuery();
        List<bool> ExecuteNonQuerys(List<Dictionary<string, object>> paramList);
        #endregion

        #region Fill DataSet and DataTable
        DataSet FillDataSet();
        DataTable FillDataTable();
        #endregion

        #region Execute Data Reader
        IDataReader ExecuteReader();
        #endregion

        #region Execute Scalar
        object ExecuteScalar();
        #endregion
    }

    public class DBTypeSelector
    {
        public static IDBHelper GetDBInstant(string commandText)
        {
            return new SQLHelper(commandText);
        }
        public static IDBHelper GetDBInstant(string commandText, CommandType commandType)
        {
            return new SQLHelper(commandText, commandType);
        }
        public static IDBHelper GetDBInstant(string connectionString, string commandText)
        {
            return new SQLHelper(connectionString, commandText);
        }
        public static IDBHelper GetDBInstant(string connectionString, string commandText, CommandType commandType)
        {
            return new SQLHelper(connectionString, commandText, commandType);
        }
    }
}
