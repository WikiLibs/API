﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.DB
{
    public class User
    {
        [Key]
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
        public string Pass { get; set; }
        public DateTime Date { get; set; }
    }
}
