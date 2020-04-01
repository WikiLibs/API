using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Output
{
    public class User : GetModel<User, Data.Models.User>
    {
        public string Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Private { get; set; }
        public bool IsBot { get; set; }
        public string ProfileMsg { get; set; }
        public int Points { get; set; }
        public string Pseudo { get; set; }
        public string Group { get; set; }
        public string[] Permissions { get; set; }

        public override void Map(in Data.Models.User model)
        {
            Id = model.Id;
            RegistrationDate = model.RegistrationDate;
            FirstName = model.FirstName;
            LastName = model.LastName;
            Email = model.Email;
            Private = model.Private;
            ProfileMsg = model.ProfileMsg;
            Points = model.Points;
            Pseudo = model.Pseudo;
            Group = model.Group.Name;
            IsBot = model.IsBot;
            Permissions = model.Group.Permissions.Select(o => o.Perm).ToArray();
        }
    }
}
