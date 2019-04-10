﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WikiLibs.Data.Models
{
    public class PrototypeParam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public virtual Prototype Prototype { get; set; }
        public string Data { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
    }
}