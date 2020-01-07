using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Input.Users
{
    public class UserUpdateGlobal : PatchModel<UserUpdateGlobal, User>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? Private { get; set; }
        public string ProfileMsg { get; set; }
        public string Pseudo { get; set; }
        public int? Points { get; set; }
        public long? Group { get; set; }
        public bool? Activate { get; set; }

        public override User CreatePatch(in User current)
        {
            return (new User()
            {
                FirstName = FirstName != null ? FirstName : current.FirstName,
                LastName = LastName != null ? LastName : current.LastName,
                Private = Private != null ? Private.Value : current.Private,
                Pseudo = Pseudo != null ? Pseudo : current.Pseudo,
                Email = Email != null ? Email : current.Email,
                GroupId = Group != null ? Group.Value : current.GroupId,
                Points = Points != null ? Points.Value : current.Points,
                ProfileMsg = ProfileMsg != null ? ProfileMsg : current.ProfileMsg,
                Confirmation = Activate != null && Activate.Value ? null : current.Confirmation
            });
        }
    }
}
