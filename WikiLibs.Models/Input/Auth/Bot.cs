using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WikiLibs.Models.Input.Auth
{
    public class Bot
    {
        [Required]
        public string AppId { get; set; }
        [Required]
        public string AppSecret { get; set; }
    }
}
