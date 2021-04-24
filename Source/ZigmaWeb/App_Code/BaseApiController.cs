using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common.Data;
using Common.Loggers;

using System.Web.Http.Filters;

namespace ZigmaWeb.Controllers
{
    public enum WebApiResponseCode
    {
        Ok = 0,
        ServerError = 1,
        AccessDenied = 2,
        KeyNotExist = 3,
        InvalidKey = 4,
        InvalidUsernameOrPassword = 5
    }

    /// <summary>
    /// Defines a generic Web API response template.
    /// </summary>
    public class WebApiResponse
    {
        public WebApiResponse() { }
        public WebApiResponse(WebApiResponseCode responseCode, object data)
        {
            Code = responseCode;
            Data = data;
        }

        public WebApiResponseCode Code { get; set; }
        public object Data { get; set; }
    }





    /// <summary>
    /// this is base class for all ApiControllers.
    /// </summary>
    public class BaseApiController : ApiController
    {

        protected bool _disposed;
        public BaseApiController()
        {
            _disposed = false;
        }

        // Flag: Has Dispose already been called? 

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

        public class NotImplExceptionFilterAttribute : ExceptionFilterAttribute
        {
            public override void OnException(HttpActionExecutedContext context)
            {
                if (context.Exception is NotImplementedException) {
                    
                    AppLogger.Error("Global exception handling"
                                    , context.Exception
                                    , context.ActionContext.ControllerContext.RouteData.Values["controller"].ToString()
                                    , context.ActionContext.ControllerContext.RouteData.Values["action"].ToString());

                    context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                }
            }
        }
    }
}
