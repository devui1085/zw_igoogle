using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace ZigmaWeb.Controllers
{
    [RoutePrefix("api/ac")]
    public class AccountController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="un">Username</param>
        /// <param name="pw">Password</param>
        /// <param name="pw">Remember Me</param>
        /// <returns></returns>
        [Route("tl")]
        public WebApiResponse TryLogin([FromBody]string un, [FromBody]string pw, [FromBody]bool rm)
        {
            WebApiResponse response = new WebApiResponse();

            if (Membership.ValidateUser(un, pw)) {
                // User cridentials is valid
                FormsAuthentication.SetAuthCookie(un, rm);

                // Construct response (We should send Initial Configuration to the clients)
                response.Code = WebApiResponseCode.Ok;
            }
            else {
                response.Code = WebApiResponseCode.InvalidUsernameOrPassword;
            }
            
            return response;
        }
	}
}