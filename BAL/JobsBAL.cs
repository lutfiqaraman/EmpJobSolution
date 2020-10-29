using DAL;
using System;
using System.Data;

namespace BAL
{
    public class JobsBAL
    {
        public static DataTable GetAllJobs(string name = null)
        {
            try
            {
                return JobsDAL.GetAllJobs(name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
