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
            User userTmp = null;
            if (this.FirstName != null) {
                userTmp.FirstName = this.FirstName;
            } else {
                userTmp.FirstName = current.FirstName;
            }
            if (this.LastName != null) {
                userTmp.LastName = this.LastName;
            } else {
                userTmp.LastName = current.LastName;
            }
            if (this.Icon != null) {
                userTmp.Icon = this.Icon;
            } else {
                userTmp.Icon = current.Icon;
            }
            if (this.Email != null) {
                userTmp.EMail = this.Email;
            } else {
                userTmp.EMail = current.EMail;
            }
            if (this.ProfileMsg != null) {
                userTmp.ProfileMsg = this.ProfileMsg;
            } else {
                userTmp.ProfileMsg = current.ProfileMsg;
            }
            if (this.Pseudo != null) {
                userTmp.Pseudo = this.Pseudo;
            } else {
                userTmp.Pseudo = current.Pseudo;
            }
            if (this.Password != null) {
                userTmp.Pass = this.Password;
            } else {
                userTmp.Pass = current.Pass;
            }
            return userTmp;
            //throw new NotImplementedException();
        }
    }
}
