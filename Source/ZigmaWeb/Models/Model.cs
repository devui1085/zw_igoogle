using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZigmaWeb.Models
{
    #region Exceptions
    public class AccessDeniedException : Exception { }

    public class KeyNotExistException : Exception
    {
        public KeyNotExistException(string key)
        {
           this.Key = key;
        }

        public KeyNotExistException(string[] keys)
        {
            this.keys = keys;
        }
        public string Key { get; set; }

        public string[] keys { get; set; }
    }

    public class InvalidKeyException : Exception
    {
        public InvalidKeyException(string key)
        {
            Key = key;
        }

        public string Key { get; set; }
    }

    #endregion

}