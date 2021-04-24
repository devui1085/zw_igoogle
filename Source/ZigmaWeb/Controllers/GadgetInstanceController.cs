using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Security;
using ZigmaWeb.Models;
using System.Net.Http.Formatting;
using Common.Loggers;
using Common.Session;

namespace ZigmaWeb.Controllers
{
    /// <summary>
    /// Manages settings of user gadget instances
    /// </summary>
    [RoutePrefix("api/gic")]
    public class GadgetInstanceController : BaseApiController
    {
        string _username;
        Guid _userId;

        GadgetInstanceManager _gadgetInstanceManager;

        public GadgetInstanceController()
        {
            _username = (User.Identity.IsAuthenticated) ? (User.Identity.Name) : "Guest";
            _userId = (Guid)Membership.GetUser(_username).ProviderUserKey;
            _gadgetInstanceManager = new GadgetInstanceManager(-1);
        }

        /// <summary>
        /// Get gadget instance settings
        /// </summary>
        /// <param name="giid">gadget istance id</param>
        /// <param name="keys">A comma delimited collection of keys</param>
        /// <returns>A WebApiResponse that contains result code (success or failure)</returns>
        [Authorize]
        [HttpPost]
        [Route("gs/{giid}")]
        public WebApiResponse GetSetting(int giid, [FromBody]string[] keys)
        {
            WebApiResponse response = new WebApiResponse();

            try {
                // perform the check
                checkCurrentUserHasPermissionOnGadgetInstance(giid);

                // get setting
                // response message
                response.Data = _gadgetInstanceManager.GetSetting(keys, true);
                response.Code = WebApiResponseCode.Ok;
            }
            catch (AccessDeniedException) {
                response.Code = WebApiResponseCode.AccessDenied;
            }
            catch (KeyNotExistException e) {
                response.Code = WebApiResponseCode.KeyNotExist;
                response.Data = e.keys;
            }

            return response;
        }


        /// <summary>
        /// Updates one or more key-value pairs of the gadget instance settings.
        /// </summary>
        /// <param name="giid">Gadget instance ID</param>
        /// <param name="keyValues">A key-value collection that is post from client</param>
        /// <returns>A WebApiResponse that contains result code (success or failure)</returns>
        [Authorize]
        [HttpPost]
        [Route("us/{giid}")]
        public WebApiResponse UpdateSetting(int giid, FormDataCollection keyValues)
        {
            WebApiResponse response = new WebApiResponse();

            try {
                // perform the check  
                checkCurrentUserHasPermissionOnGadgetInstance(giid);

                // update
                _gadgetInstanceManager.UpdateSetting(keyValues.ToArray(), true);

                // response message
                response.Code = WebApiResponseCode.Ok;
            }
            catch (AccessDeniedException) {
                response.Code = WebApiResponseCode.AccessDenied;
            }
            catch (KeyNotExistException e) {
                response.Code = WebApiResponseCode.KeyNotExist;
                response.Data = e.keys;
            }

            return response;
        }


        /// <summary>
        /// Sets one or more key-value pairs of the gadget instance settings.
        /// </summary>
        /// <param name="giid">Gadget instance ID</param>
        /// <param name="keyValues">A key-value collection that is post from client</param>
        /// <returns>A WebApiResponse that contains result code (success or failure)</returns>
        [Authorize]
        [HttpPost]
        [Route("ss/{giid}")]
        public WebApiResponse SetSetting(int giid, FormDataCollection keyValues)
        {
            WebApiResponse response = new WebApiResponse();
            try {
                // perform the check 
                checkCurrentUserHasPermissionOnGadgetInstance(giid);

                // update 
                _gadgetInstanceManager.UpdateSetting(keyValues.ToArray(), true);

                // response message
                response.Code = WebApiResponseCode.Ok;
            }
            catch (AccessDeniedException) {
                response.Code = WebApiResponseCode.AccessDenied;
            }
            catch (KeyNotExistException e) {
                response.Code = WebApiResponseCode.KeyNotExist;
                response.Data = e.keys;
            }

            return response;
        }


        /// <summary>
        /// Removes one or more key-value pairs from gadget instance settings.
        /// </summary>
        /// <param name="giid">Gadget instance ID</param>
        /// <param name="keys">A comma delimited collection of keys that should be removed from gadget instance settings</param>
        /// <returns>On success returns HTTP 200 (OK), otherwise returns ...</returns>
        [Authorize]
        [HttpPost]
        [Route("rs/{giid}")]
        public WebApiResponse RemoveSetting(int giid, [FromBody]string[] keys)
        {
            WebApiResponse response = new WebApiResponse();
            try {
                // perform the check
                checkCurrentUserHasPermissionOnGadgetInstance(giid);

                _gadgetInstanceManager.RemoveSetting(keys, true);
            }
            catch (AccessDeniedException) {
                response.Code = WebApiResponseCode.AccessDenied;
            }
            catch (KeyNotExistException e) {
                response.Code = WebApiResponseCode.KeyNotExist;
                response.Data = e.keys;
            }

            return response;
        }


        /// <summary>
        /// Removes all settings of gadget instance .
        /// </summary>
        /// <param name="giid">Gadget instance ID</param>
        /// <returns>On success returns HTTP 200 (OK), otherwise returns ...</returns>
        [Authorize]
        [HttpPost()]
        [Route("ras/{giid}")]
        public WebApiResponse RemoveAllSettings(int giid)
        {
            WebApiResponse response = new WebApiResponse();

            try {
                // perform the check
                checkCurrentUserHasPermissionOnGadgetInstance(giid);

                _gadgetInstanceManager.RemoveAllSetting(true);
            }
            catch (AccessDeniedException) {
                response.Code = WebApiResponseCode.AccessDenied;
            }
            catch (KeyNotExistException e) {
                response.Code = WebApiResponseCode.KeyNotExist;
                response.Data = e.keys;
            }

            return response;
        }

        /// <summary>
        /// Checks to see if current user has permission to manipulate specified gadget instance settings.
        /// </summary>
        /// <param name="giid">Gadget instance Id</param>
        /// <returns>Returns True if current user has permision to execute CRUD commands on the specefied gadget instance settings
        /// (User is owner of the gadget) otherwise returns False.</returns>
        private void checkCurrentUserHasPermissionOnGadgetInstance(int giid)
        {
            if (!new UserManager(CredentialHelper.CurrentCredential.UserID).UserHasCrudPermissionOnGadget(giid))
                throw new AccessDeniedException();
        }
    }
}
