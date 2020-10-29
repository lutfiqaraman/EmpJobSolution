using DBHelper;
using System;
using System.Data;

namespace DAL
{
    public class JobsDAL
    {
        #region Retrieve Data
        public static DataTable GetAllJobs(string name = null)
        {
            try
            {
                using(IDBHelper dBHelper = DBTypeSelector.GetDBInstant("GetAllJobs"))
                {
                    dBHelper.AddParameter("@Name", name);
                    return dBHelper.FillDataTable();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
