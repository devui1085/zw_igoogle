using System;
using System.Web;

namespace Common.Session
{
    public static class CredentialHelper
    {
        private static readonly string credentialKey;

        /// <summary>
        /// Determine session key name
        /// </summary>
        static CredentialHelper()
        {
            credentialKey = AppDomain.CurrentDomain.FriendlyName;
        }

        /// <summary>
        /// Get Current Credenial
        /// </summary>
        public static Credential CurrentCredential
        {
            get
            {
                if (System.Web.HttpContext.Current.Session[credentialKey] != null)
                    return (Credential)HttpContext.Current.Session[credentialKey];
                return null;
            }
            set
            {
                HttpContext.Current.Session[credentialKey] = value;
            }
        }
    }
}
