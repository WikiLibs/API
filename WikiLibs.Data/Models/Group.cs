﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.Data.Models
{
    public class Group : Model
    {
        public string Name { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; } = new HashSet<Permission>();
    }
}
