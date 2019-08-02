using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Input.Admin
{
    public class BotCreate : PostModel<BotCreate, User>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public bool? Private { get; set; }
        public string ProfileMsg { get; set; }
        [Required]
        public string Pseudo { get; set; }

        public override User CreateModel()
        {
            return (new User()
            {
                Email = Email,
                FirstName = Name,
                LastName = "",
                RegistrationDate = DateTime.UtcNow,
                IsBot = true,
                Points = 0,
                Private = Private != null ? Private.Value : false,
                ProfileMsg = ProfileMsg,
                Pseudo = Pseudo
            });
        }
    }
}
