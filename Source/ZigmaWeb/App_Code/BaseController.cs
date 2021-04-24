using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Data;
using Common.Loggers;
using System.Web.Http;

namespace ZigmaWeb.Controllers
{
    /// <summary>
    /// This is base class for all site's controllers
    /// </summary>
    public class BaseController : Controller
    {
        protected bool _disposed; // Flag: Has Dispose already been called? 

        public BaseController()
        {
            _disposed = false;
        }
        /// <summary>
        /// Dispose the used resource.
        /// </summary>
        /// <param name="disposing">The disposing flag.</param>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing) {
                EFHelper.DisposeContext<AppDataModelContainer>();
                base.Dispose(disposing);
            }

            // Free any unmanaged objects here.
            //
            _disposed = true;
        }

        /// <summary>
        /// Called when an unhandled exception occurs in the action.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is UnauthorizedAccessException) {
                filterContext.ExceptionHandled = true;
                filterContext.Result = RedirectToAction("Home", "Index");
            }

            if (filterContext.Exception is Exception) {
                filterContext.ExceptionHandled = true;
                AppLogger.Error("Global exception handling", filterContext.Exception, filterContext.RouteData.Values["controller"].ToString(), filterContext.RouteData.Values["action"].ToString());
            }
            base.OnException(filterContext);
        }


        /// <summary>
        /// This method is called by ASP.NET MVC to process a request
        /// </summary>
        protected override void ExecuteCore()
        {
            base.ExecuteCore();
        }
    }
}