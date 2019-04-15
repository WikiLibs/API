using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Input
{
    public class UserUpdate : PatchModel<UserUpdate, User>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Icon { get; set; }
        public string Email { get; set; }
        public bool Private { get; set; }
        public string ProfileMsg { get; set; }
        public string Pseudo { get; set; }
        public string Password { get; set; }

        public override User CreatePatch(in User current)
        {
            throw new NotImplementedException();
        }
    }
}
