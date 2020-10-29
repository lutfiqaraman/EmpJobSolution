using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Tools
{
    public static class SystemConnectionString
    {
        public static string GetConnectionString()
        {
            try
            {
                return
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
