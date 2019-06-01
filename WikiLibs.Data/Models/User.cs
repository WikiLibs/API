using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.Data.Models
{
    public class User : Model<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Icon { get; set; }
        public string EMail { get; set; }
        public string Confirmation { get; set; }
        public bool Private { get; set; }
        public string ProfileMsg { get; set; }
        public int Points { get; set; }
        public string Pseudo { get; set; }
        public long? GroupId { get; set; }
        public string Pass { get; set; }
        public DateTime RegistrationDate { get; set; }

        public virtual Group Group { get; set; }
    }
}
