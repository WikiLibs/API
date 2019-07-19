using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Input.Users
{
    public class UserCreate : PostModel<UserCreate, User>
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Icon { get; set; }
        [Required]
        public string Email { get; set; }
        public bool? Private { get; set; }
        public string ProfileMsg { get; set; }
        [Required]
        public string Pseudo { get; set; }
        [Required]
        public string Password { get; set; }

        public override User CreateModel()
        {
            return (new User()
            {
                EMail = Email,
                FirstName = FirstName,
                LastName = LastName,
                Icon = Icon,
                Private = Private != null ? Private.Value : false,
                ProfileMsg = ProfileMsg,
                Points = 0,
                RegistrationDate = DateTime.UtcNow,
                Pass = Password,
                Pseudo = Pseudo,
                IsBot = false
            });
        }
    }
}
