using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Input.Admin
{
    public class APIKeyCreate : PostModel<APIKeyCreate, APIKey>
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public int Flags { get; set; }
        [Required]
        public int UseNum { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }

        public override APIKey CreateModel()
        {
            return (new APIKey()
            {
                Description = Description,
                ExpirationDate = ExpirationDate,
                Flags = Flags,
                UseNum = UseNum
            });
        }
    }
}
