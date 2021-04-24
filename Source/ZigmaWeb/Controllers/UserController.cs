using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Security;
using ZigmaWeb.Models;
using Common.Data;
using System.Net.Http.Formatting;

namespace ZigmaWeb.Controllers
{
    /// <summary>
    /// Manages user settings.
    /// </summary>

    [RoutePrefix("api/uc")]
    public class UserController : BaseApiController
    {
        string _username;
        Guid _userId;

        private UserManager UserManager { set; get; }

        public UserController()
        {
            _username = (User.Identity.IsAuthenticated) ? (User.Identity.Name) : "Guest";
            _userId = (Guid)Membership.GetUser(_username).ProviderUserKey;
            UserManager = new UserManager(_userId);
        }

        /// <summary>
        /// Returns configuration that is needed for initializing the application
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("gic")]
        public WebApiResponse GetInitialConfiguration()
        {
            WebApiResponse response = new WebApiResponse();

            try {
                response.Data = UserManager.GetInitialConfiguration();
                response.Code = WebApiResponseCode.Ok;
            }
            catch (Exception) {
                response.Code = WebApiResponseCode.ServerError;
            }

            return response;
        }


        /// <summary>
        /// Reads user settings from database.
        /// </summary>
        /// <param name="keys">A collection of keys of user settings</param>
        /// <returns>A WebApiResponse that contains values of the keys</returns>
        [Authorize]
        [Route("gs")]
        [HttpPost]
        public WebApiResponse GetSetting([FromBody]string[] keys)
        {
            WebApiResponse response = new WebApiResponse();

            try {
                Dictionary<string, string> keyValues = new Dictionary<string, string>(keys.Length);

                // Try reading all keys from the database
                foreach (string key in keys) {
                    keyValues.Add(key, UserManager.GetSetting(key, true));
                }

                // construct response message
                response.Data = keyValues;
                response.Code = WebApiResponseCode.Ok;
            }
            catch (KeyNotExistException e) {
                response.Code = WebApiResponseCode.KeyNotExist;
                response.Data = e.Key;
            }
            catch (InvalidKeyException e) {
                response.Code = WebApiResponseCode.InvalidKey;
                response.Data = e.Key;
            }

            return response;
        }


        /// <summary>
        /// Updates one or more key-value pairs of the user settings.
        /// </summary>
        /// <param name="keyValues">A key-value collection of user settings</param>
        /// <returns>A WebApiResponse that contains result code (success or failure)</returns>
        [Authorize]
        [HttpPost]
        [Route("us")]
        public WebApiResponse UpdateSetting(FormDataCollection keyValues)
        {
            WebApiResponse response = new WebApiResponse();

            try {
                // Try updating all keys 
                foreach (var pair in keyValues) {
                    UserManager.UpdateSetting(pair.Key, pair.Value, true);
                }

                // construct response message
                response.Code = WebApiResponseCode.Ok;
            }
            catch (KeyNotExistException e) {
                response.Code = WebApiResponseCode.KeyNotExist;
                response.Data = e.Key;
            }
            catch (InvalidKeyException e) {
                response.Code = WebApiResponseCode.InvalidKey;
                response.Data = e.Key;
            }

            return response;
        }


        /// <summary>
        /// Sets one or more key-value pairs of the user settings atomicaly.
        /// </summary>
        /// <param name="keyValues">A key-value collection of user settings</param>
        /// <returns>A WebApiResponse that contains result code (success or failure)</returns>
        [Authorize]
        [HttpPost]
        [Route("ss")]
        public WebApiResponse SetSetting(FormDataCollection keyValues)
        {
            WebApiResponse response = new WebApiResponse();

            try {
                // Try setting all keys
                foreach (var pair in keyValues) {
                    UserManager.SetSetting(pair.Key, pair.Value, true);
                }

                // construct response message
                response.Code = WebApiResponseCode.Ok;
            }
            catch (InvalidKeyException e) {
                response.Code = WebApiResponseCode.InvalidKey;
                response.Data = e.Key;
            }

            return response;
        }
    }
}