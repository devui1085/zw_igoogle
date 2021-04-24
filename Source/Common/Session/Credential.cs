using System;

namespace Common.Session
{
    /// <summary>
    /// Define authorized user information
    /// </summary>
    public class Credential
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
    }
}
