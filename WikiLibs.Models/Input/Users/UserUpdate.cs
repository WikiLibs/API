using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Input.Users
{
    public class UserUpdate : PatchModel<UserUpdate, User>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? Private { get; set; }
        public string ProfileMsg { get; set; }
        public string Pseudo { get; set; }
        public string CurPassword { get; set; }
        public string NewPassword { get; set; }

        public override User CreatePatch(in User current)
        {
            return (new User()
            {
                FirstName = FirstName != null ? FirstName : current.FirstName,
                LastName = LastName != null ? LastName : current.LastName,
                Private = Private != null ? Private.Value : current.Private,
                Pseudo = Pseudo != null ? Pseudo : current.Pseudo,
                Email = Email != null ? Email : current.Email,
                ProfileMsg = ProfileMsg != null ? ProfileMsg : current.ProfileMsg,
                Pass = NewPassword != null ? NewPassword : current.Pass
            });
        }
    }
}
