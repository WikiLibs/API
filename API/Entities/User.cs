using System;
using System.Collections.Generic;
using System.Text;

namespace API.Entities
{
    public class User
    {
        private Dictionary<string, bool> _perms = new Dictionary<string, bool>();

        public string UUID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Icon { get; set; }
        public string EMail { get; set; }
        public bool ShowEmail { get; set; }
        public string ProfileMsg { get; set; }
        public int Points { get; set; }
        public string Pseudo { get; set; }
        public string Group { get; set; }
        
        public bool HasPermission(string name)
        {
            return (_perms.ContainsKey(name));
        }

        /**
         * Does not save to database
         */
        public void AddPermission(string name)
        {
            _perms[name] = true;
        }
    }
}
