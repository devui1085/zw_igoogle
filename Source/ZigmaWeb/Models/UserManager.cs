using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Common.Data;

namespace ZigmaWeb.Models
{
    public class UserManager
    {
        Guid _userId;
        private AppDataModelContainer _context { get; set; }

        public UserManager(Guid userId)
        {
            _userId = userId;
            _context = EFHelper.GetContext<AppDataModelContainer>(); ;
        }

        //public static GadgetInstance AddGadgetForUser(Guid userId, Gadget gadget, GadgetInstanceUserSetting[] userSettings)
        //{
        //    GadgetInstance gi = new GadgetInstance();
        //    gi.UserId = userId;
        //    gi.Gadget = gadget;
        //    foreach (var item in userSettings) {
        //        gi.UserSettings.Add(item);
        //    }

        //    AppDataModelContainer context = new AppDataModelContainer();
        //    context.GadgetInstances.Add(gi);
        //    context.SaveChanges();

        //    return gi;
        //}


        public object GetInitialConfiguration()
        {
            // Load user instance gadgets
            GadgetInstance[] userGadgets = GetUserGadgets();

            // Load panelHost settings (JSON string)
            string panelHostConfig = GetSetting("PanelHost", false);

            // Return configuration object
            return new {
                phc = panelHostConfig, // PanelHost Configuration
                // User Gadget Collection
                ugc = from ug in userGadgets
                      select new {
                          giid = ug.Id,
                          sName = ug.Gadget.SystemName,
                          pName = ug.Gadget.PublicName,
                          row = ug.Row,
                          col = ug.Column,
                          rs = ug.RowSpan,
                          cs = ug.ColumnSpan
                      }
            };
        }

        /// <summary>
        /// Returns user gadgets.
        /// </summary>
        /// <param name="userId">Id of the user</param>
        public GadgetInstance[] GetUserGadgets()
        {
            return (from gi in _context.GadgetInstances
                    where gi.UserId == _userId
                    select gi).ToArray();
        }

        /// <summary>
        /// Gets a user setting by it's key
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="publicSetting">Setting type, True for public settings, False for system (private) settings</param>
        /// <returns>If the key found, its associated value will return</returns>
        public string GetSetting(string key, bool publicSetting)
        {
            UserSetting userSetting = (from us in _context.UserSettings
                                       where (us.UserId == _userId) && (us.Key == key) && (us.Public == publicSetting)
                                       select us).FirstOrDefault();

            // Throw exception if the key is not exist in the user settings
            if (userSetting == null) {
                throw new KeyNotExistException(key);
            }

            return userSetting.Value;
        }


        /// <summary>
        /// Adds a Key-Value pair to the user settings if the 'key' is not already in the database,
        /// otherwise updates the corresponding 'value' of the existing key 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="publicSetting">Setting type, True for public settings, False for system (private) settings</param>
        public void SetSetting(string key, string value, bool publicSetting)
        {
            var userSetting = (from us in _context.UserSettings
                               where (us.UserId == _userId) && (us.Key == key) && (us.Public == publicSetting)
                               select us).FirstOrDefault();

            if (userSetting != null) {
                userSetting.Value = value;
            }
            else {
                _context.UserSettings.Add(new UserSetting(_userId, key, value, publicSetting));
            }

            _context.SaveChanges();
        }


        /// <summary>
        /// Updates a Key-Value pair of the user settings.
        /// if the 'key' is not already in the database, an Exception is thrown. 
        /// </summary>
        /// <param name="key">The key of the key-value pair</param>
        /// <param name="newValue">New value of the key-value pair</param>
        /// <param name="publicSetting">Setting type, True for public settings, False for system (private) settings</param>
        public void UpdateSetting(string key, string newValue, bool publicSetting)
        {
            var userSetting = (from us in _context.UserSettings
                               where (us.UserId == _userId) && (us.Key == key) && (us.Public == publicSetting)
                               select us).FirstOrDefault();

            if (userSetting != null) {
                userSetting.Value = newValue;
                _context.SaveChanges();
            }
            else {
                throw new KeyNotExistException(key);
            }
        }


        /// <summary>
        /// Removes a Key-Value pair from the user settings
        /// </summary>
        /// <param name="key">The key of the Key-Value pair</param>
        /// <param name="publicSetting">Setting type, True for public settings, False for system (private) settings</param>
        public void RemoveSetting(string key, bool publicSetting)
        {
            var userSetting = _context.UserSettings.SingleOrDefault(
                us => (us.UserId == _userId) && (us.Key == key) && (us.Public == publicSetting));

            if (userSetting != null) {
                _context.UserSettings.Remove(userSetting);
                _context.SaveChanges();
            }
            else {
                throw new KeyNotExistException(key);
            }
        }


        //public static Guid get()
        //{

        //}


        #region user gadgets

        /// <summary>
        ///Check to see current user is owner of a gadget with instance id equal to "giid"
        /// </summary>
        /// <param name="gadgetID"></param>
        /// <returns></returns>
        public bool UserHasCrudPermissionOnGadget(int gadgetID)
        {
            return EFHelper.GetContext<AppDataModelContainer>().GadgetInstances.Any(i => i.UserId == this._userId && i.Id == gadgetID);
        }

        #endregion
    }
}