using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Input.Admin
{
    public class ApiKeyCreate : PostModel<ApiKeyCreate, ApiKey>
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public string Origin { get; set; }
        [Required]
        public int Flags { get; set; }
        [Required]
        public int UseNum { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }

        public override ApiKey CreateModel()
        {
            return (new ApiKey()
            {
                Description = Description,
                Origin = Origin,
                ExpirationDate = ExpirationDate,
                Flags = Flags,
                UseNum = UseNum
            });
        }
    }
}
