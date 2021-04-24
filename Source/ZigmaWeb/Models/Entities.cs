using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZigmaWeb.Models;

namespace ZigmaWeb
{

    //
    // Gadget
    //
    public partial class Gadget
    {
        Dictionary<string, string> _parsedData;

        public Dictionary<string, string> ParsedData
        { 
            get
            {
                if (_parsedData == null) {
                    ParseData();
                }

                return _parsedData;
            }
        }

        public Gadget(string systemName, GadgetType gadgetType)
        {
            this.SystemName = systemName;
            this.GadgetType = gadgetType;
        }

        private void ParseData()
        {
            // Parse Gadget.Data field and populate DataDictionary
            _parsedData = new Dictionary<string, string>();
            // Parse XML Data ...

        }
    }


    //
    // GadgetInstance
    //
    public partial class GadgetInstance
    {
        public GadgetInstance(Gadget gadget, Guid userId, byte row, byte column, byte rowSpan, byte columnSpan)
            : this()
        {
            this.Gadget = gadget;
            this.UserId = userId;
            this.Row = row;
            this.Column = column;
            this.RowSpan = rowSpan;
            this.ColumnSpan = columnSpan;
        }

        public GadgetInstance(string gadgetSystemName, Guid userId, byte row, byte column, byte rowSpan, byte columnSpan)
            : this()
        {
            this.Gadget = GadgetManager.GetByName(gadgetSystemName);
            this.UserId = userId;
            this.Row = row;
            this.Column = column;
            this.RowSpan = rowSpan;
            this.ColumnSpan = columnSpan;
        }
    }


    //
    // GadgetInstanceSetting
    //
    public partial class GadgetInstanceSetting
    {
        public GadgetInstanceSetting(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
    }


    //
    // UserSetting
    //
    public partial class UserSetting
    {
        public UserSetting()
        {
        }

        public UserSetting(Guid userId, string key, string value, bool publicSetting)
        {
            this.UserId = userId;
            this.Key = key;
            this.Value = value;
            this.Public = publicSetting;
        }
    }
}