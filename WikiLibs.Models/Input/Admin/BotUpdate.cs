using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Input.Admin
{
    public class BotUpdate : PatchModel<BotUpdate, Data.Models.User>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool? Private { get; set; }
        public string ProfileMsg { get; set; }
        public string Pseudo { get; set; }
        public int? GroupId { get; set; }

        public override User CreatePatch(in User current)
        {
            return (new User()
            {
                Email = Email != null ? Email : current.Email,
                FirstName = Name != null ? Name : current.FirstName,
                GroupId = GroupId != null ? GroupId : current.GroupId,
                Private = Private != null ? Private.Value : current.Private,
                ProfileMsg = ProfileMsg != null ? ProfileMsg : current.ProfileMsg,
                Pseudo = Pseudo != null ? Pseudo : current.Pseudo
            });
        }
    }
}
