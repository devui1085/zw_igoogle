using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Data;
using System.Collections.Specialized;

namespace ZigmaWeb.Models
{

    public class GadgetInstanceManager
    {
        int _giid; // Gadget Instance ID
        private AppDataModelContainer _context { get; set; }

        public GadgetInstanceManager(int gadgetInstanceId)
        {
            _context = EFHelper.GetContext<AppDataModelContainer>();
            _giid = gadgetInstanceId;
        }


        /// <summary>
        /// Reads a gadget instance setting from the database
        /// </summary>
        /// <param name="key">The key of the (key,value) pair that should be read. if this key doesn't exist
        /// in the gadget instance settings, operation will fail.</param>
        /// <param name="publicSetting">Set this parameter to True if the (key,value) pair is a public setting,
        /// otherwise set it to False</param>
        /// <param name="settingValue">The value of the (key,value) pair that is read from gadget instance settings</param>
        /// <returns>On success returns True, otherwise returns False</returns>
        public string GetSetting(string key, bool publicSetting)
        {
            GadgetInstanceSetting setting = _context.GadgetInstanceSettings.Where(r => r.Key == key && r.Public == publicSetting).SingleOrDefault();

            if (setting == null)
                throw new InvalidKeyException(key);

            return setting.Value;
        }

        /// <summary>
        /// Reads a gadget instance settings from the database
        /// </summary>
        /// <param name="key">The key of the (key,value) pair that should be read. if this key doesn't exist
        /// in the gadget instance settings, operation will fail.</param>
        /// <param name="publicSetting">Set this parameter to True if the (key,value) pair is a public setting,
        /// otherwise set it to False</param>
        /// <param name="settingValue">The value of the (key,value) pair that is read from gadget instance settings</param>
        /// <returns>On success returns True, otherwise returns False</returns>
        public Dictionary<string, string> GetSetting(string[] keys, bool publicSetting)
        {
            var settings = (from g in _context.GadgetInstanceSettings
                            where keys.Contains(g.Key) && g.Public == publicSetting
                            select g).ToDictionary(g => g.Key, g => g.Value);

            if (settings.Count() != keys.Length) {
                throw new KeyNotExistException(keys.Except(settings.Keys.ToArray<string>()).ToArray());
            }

            return settings;
        }



        /// <summary>
        /// Updates a (key,value) pair in the gadget instance settings 
        /// </summary>
        /// <param name="key">The key of the key-value pair. this key should exist in the gadget instance settings,
        /// otherwise update operation will fail</param>
        /// <param name="newValue">new value for the (key,value) pair</param>
        /// <param name="publicSetting">Set this parameter to True if the (key,value) pair is a public setting,
        /// otherwise set it to False</param>
        /// <returns>On success returns True, otherwise returns False</returns>
        public void UpdateSetting(string key, string newValue, bool publicSetting)
        {
            GadgetInstanceSetting setting = _context.GadgetInstanceSettings.Where(r => r.Key == key && r.Public == publicSetting).SingleOrDefault();

            if (setting == null)
                throw new InvalidKeyException(key);

            // set values
            setting.Value = newValue;

            // update changes
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates a collection of (key,value) pairs in the gadget instance settings 
        /// </summary>
        /// <param name="key">The key of the key-value pair. this key should exist in the gadget instance settings,
        /// otherwise update operation will fail</param>
        /// <param name="newValue">new value for the (key,value) pair</param>
        /// <param name="publicSetting">Set this parameter to True if the (key,value) pair is a public setting,
        /// otherwise set it to False</param>
        /// <returns>On success returns True, otherwise returns False</returns>
        public void UpdateSetting(KeyValuePair<string, string>[] keyValuePairs, bool publicSetting)
        {
            GadgetInstanceSetting gis;

            for (int i = 0; i < keyValuePairs.Length; i++) {

                gis = new GadgetInstanceSetting(keyValuePairs[i].Key, keyValuePairs[i].Value);

                _context.GadgetInstanceSettings.Attach(gis);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Sets a (key,value) pair in the gadget instance settings.
        /// </summary>
        /// <param name="key">The key of the key-value pair. if this key exists in the gadget instanse settings,
        /// its value will updated, otherwise a new (key,value) pair will be inserted in the gadget instance settings.</param>
        /// <param name="value">new value of the (key,value) pair</param>
        /// <param name="publicSetting">Set this parameter to True if the (key,value) pair is a public setting,
        /// otherwise set it to False</param>
        /// <returns>On success returns True, otherwise returns False</returns>
        public void SetSetting(string key, string value, bool publicSetting)
        {

            GadgetInstanceSetting setting = _context.GadgetInstanceSettings.FirstOrDefault(
                gis => (gis.GadgetInstance.Id == _giid && gis.Key == key && gis.Public == publicSetting));

            if (setting != null) {
                setting.Value = value;
            }
            else {
                _context.GadgetInstances.Single(gi => gi.Id == _giid).Settings.Add(new GadgetInstanceSetting(key, value));
            }

            _context.SaveChanges();

        }

        /// <summary>
        /// Sets a (key,value) pair in the gadget instance settings.
        /// </summary>
        /// <param name="key">The key of the key-value pair. if this key exists in the gadget instanse settings,
        /// its value will updated, otherwise a new (key,value) pair will be inserted in the gadget instance settings.</param>
        /// <param name="value">new value of the (key,value) pair</param>
        /// <param name="publicSetting">Set this parameter to True if the (key,value) pair is a public setting,
        /// otherwise set it to False</param>
        /// <returns>On success returns True, otherwise returns False</returns>
        public void SetSetting(KeyValuePair<string, string>[] keyValuePairs, bool publicSetting)
        {
            GadgetInstanceSetting gis;

            for (int i = 0; i < keyValuePairs.Length; i++) {

                gis = new GadgetInstanceSetting(keyValuePairs[i].Key, keyValuePairs[i].Value);

                _context.GadgetInstanceSettings.Attach(gis);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Remove a collection of Key-Value pair from the user settings
        /// </summary>
        /// <param name="keys">The key of the Key-Value pair</param>
        /// <param name="publicSetting">Setting type, True for public settings, False for system (private) settings</param>
        public void RemoveSetting(string[] keys, bool publicSetting)
        {
            var gadgetInstanceSetting = _context.GadgetInstanceSettings.SingleOrDefault(
                gis => (gis.Id == _giid) && keys.Contains(gis.Key) && (gis.Public == publicSetting));

            if (gadgetInstanceSetting != null) {
                _context.GadgetInstanceSettings.Remove(gadgetInstanceSetting);
                _context.SaveChanges();
            }
            else {
                throw new KeyNotExistException(keys);
            }
        }


        /// <summary>
        /// Removes a Key-Value pair from the user settings
        /// </summary>
        /// <param name="key">The key of the Key-Value pair</param>
        /// <param name="publicSetting">Setting type, True for public settings, False for system (private) settings</param>
        public void RemoveAllSetting(bool publicSetting)
        {
            var gadgetInstanceSettings = _context.GadgetInstanceSettings.SingleOrDefault(
                gis => (gis.Id == _giid) && (gis.Public == publicSetting));

            if (gadgetInstanceSettings != null) {
                _context.GadgetInstanceSettings.Remove(gadgetInstanceSettings);
                _context.SaveChanges();
            }
            else {
                throw new KeyNotExistException(string.Empty);
            }
        }
    }
}