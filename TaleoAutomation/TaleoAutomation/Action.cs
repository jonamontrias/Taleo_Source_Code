using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaleoAutomation
{
    class Action
    {
        #region Properties
        //private properties
        private string _name = "";
        private string _id = "";
        private string _type = "";
        private string _value = "";


        //public properties
        public string name
        {
            get { return _name; }
            set { if (!value.Equals(""))
                    _name = value; 
            }
        }

        public string id
        {
            get { return _id; }
            set
            {
                if (!value.Equals(""))
                    _id = value;
            }
        }

        public string type
        {
            get { return _type; }
            set
            {
                if (!value.Equals(""))
                    _type = value;
            }
        }

        public string value
        {
            get { return _value; }
            set
            {
                if (!value.Equals(""))
                    _value = value;
            }
        }
        #endregion

        public Action(string newName, string newId, string newType, string newValue)
        {
            _name = newName;
            _id = newId;
            _type = newType;
            _value = newValue;
        }
    }
}
