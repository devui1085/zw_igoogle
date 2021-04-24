using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ZigmaWeb.Models
{
    public static class AccountManager
    {
        public static MembershipUser RegisterUser(string username, string password, string email, string roleName = "", bool installDefaultGadgets = true, bool setDefaultSettings = true)
        {
            // Create user and set it's role
            var user = Membership.CreateUser(username, password, email);
            if (roleName != "") {
                if (!Roles.RoleExists(roleName))
                    Roles.CreateRole(roleName);
                Roles.AddUserToRole(username, roleName);
            }
       
            // Set default user settings
            //if (setDefaultSettings) {
            //    UserManager.SetUserSetting((Guid)user.ProviderUserKey, "main", "");
            //}

            // Install default gadgets
            if (installDefaultGadgets) {
                AppDataModelContainer context = new AppDataModelContainer();

                GadgetInstance gi = new GadgetInstance();
                var userManagerGadget = context.Gadgets.First(g => g.SystemName == "UserManager");

                gi.Gadget = userManagerGadget;
                gi.UserId = (Guid)user.ProviderUserKey;

                context.GadgetInstances.Add(gi);
                context.SaveChanges();
            }

            return user;
        }

        public static bool TryLogin(string username, string password, bool rememberMe)
        {
            bool success = false;

            if (Membership.ValidateUser(username, password)) {
                FormsAuthentication.SetAuthCookie(username, rememberMe);
                success = true;
            }

            return success;
        }





    }
}