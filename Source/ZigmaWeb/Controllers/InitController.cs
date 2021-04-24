using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Web.Security;
using System.Xml;

using ZigmaWeb.Models;

namespace ZigmaWeb.Controllers
{
    public class InitController : BaseController
    {
        //
        // GET: /Db/
        public ActionResult Index()
        {
            AppDataModelContainer ctx = new AppDataModelContainer();

            #region Initialize Gadgets
            ctx.Gadgets.RemoveRange(ctx.Gadgets);

            // Load Gadgets from "Gadgets.xml" and save them into database
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath("~/App_Data/Gadgets.xml"));
            foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes) {
                Gadget g = new Gadget();
                string createDateValue, lastUpdateValue;

                createDateValue = xmlNode.Attributes["CreateDate"].Value;
                lastUpdateValue = xmlNode.Attributes["LastUpdate"].Value;
                g.SystemName = xmlNode.Attributes["SystemName"].Value;
                g.PublicName = xmlNode.Attributes["PublicName"].Value;
                g.GadgetType = (GadgetType)Enum.Parse(typeof(GadgetType), xmlNode.Attributes["GadgetType"].Value);
                g.Description = xmlNode.Attributes["Description"].Value;
                g.Version = xmlNode.Attributes["Version"].Value;
                g.FolderName = xmlNode.Attributes["FolderName"].Value;
                //g.DefaultRowSpan = Byte.Parse(xmlNode.Attributes["DefaultRowSpan"].Value);
                //g.DefaultColumnSpan = Byte.Parse(xmlNode.Attributes["DefaultColSpan"].Value);
                g.CreateDate = (createDateValue == "Now") ? DateTime.Now : DateTime.Parse(createDateValue);
                g.LastUpdate = (lastUpdateValue == "Now") ? DateTime.Now : DateTime.Parse(lastUpdateValue);
                g.HomePageUrl = xmlNode.Attributes["HomePageUrl"].Value;
                g.Enabled = bool.Parse(xmlNode.Attributes["Enabled"].Value);
                g.Data = xmlNode.Attributes["Data"].Value;
                //g.MainSetting = xmlNode.Attributes["Setting"].Value;

                ctx.Gadgets.Add(g);
            }

            ctx.SaveChanges();
            #endregion

            #region Initialize Users and Roles
            // Initialize roles
            if (!Roles.RoleExists("admin"))
                Roles.CreateRole("admin");

            // Delete all users
            MembershipUserCollection users = Membership.GetAllUsers();
            foreach (MembershipUser item in users) {
                Membership.DeleteUser(item.UserName, true);
            }

            // Create User "Admin" and initialize it
            var adminUser = Membership.CreateUser("Admin", "123456", "admin@zigmaweb.ir");
            Roles.AddUserToRole("Admin", "Admin");

            // Create User "Guest" and initialize it
            var guestUser = Membership.CreateUser("Guest", "123456", "guest@zigmaweb.ir");
            var guestUserId = (Guid)guestUser.ProviderUserKey;

            ctx.GadgetInstances.Add(new GadgetInstance("Calandar", guestUserId, 2, 0, 2, 2));
            ctx.GadgetInstances.Add(new GadgetInstance("Clock", guestUserId, 2, 2, 2, 2));
            ctx.GadgetInstances.Add(new GadgetInstance("Email", guestUserId, 0, 0, 2, 4));
            ctx.GadgetInstances.Add(new GadgetInstance("News", guestUserId, 0, 4, 4, 5));

            UserManager guestUserManager = new UserManager(guestUserId);
            guestUserManager.SetSetting(
                key: "PanelHost",
                value: Newtonsoft.Json.JsonConvert.SerializeObject(new {
                    rows = 4,
                    cw = 100,
                    ch = 100,
                    cg = 10,
                }),
                publicSetting: false);
            ctx.SaveChanges();
            #endregion

            return Content("Database Initialized.");
        }

    }
}