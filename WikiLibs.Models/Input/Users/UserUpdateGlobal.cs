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
        public string Icon { get; set; }
        public string Email { get; set; }
        public bool Private { get; set; }
        public string ProfileMsg { get; set; }
        public string Pseudo { get; set; }
        public int Points { get; set; }
        public int Group { get; set; }

        public override User CreatePatch(in User current)
        {
            throw new NotImplementedException();
        }
    }
}
