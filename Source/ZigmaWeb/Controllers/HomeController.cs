using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ZigmaWeb;

namespace ZigmaWeb.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            try {
                // log
                Common.Loggers.DataLogger.Info("message", this.GetType().Name, "Index");
            }
            catch (Exception ex) {
                Common.Loggers.AppLogger.Error("message", ex, this.GetType().Name, "Index");
            }
            return View();
        }
    }
}