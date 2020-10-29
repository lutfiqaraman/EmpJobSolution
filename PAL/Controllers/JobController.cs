using BAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PAL.Controllers
{
    public class JobController : Controller
    {
        // GET: Job
        public ActionResult Index()
        {
            GetJobs();
            return View();
        }

        private void GetJobs(string jobname = null)
        {
            try
            {
                DataTable jobs = JobsBAL.GetAllJobs(jobname);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}