using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Output
{
    public class Bot : GetModel<Bot, Data.Models.User>
    {
        public string Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Icon { get; set; }
        public string Email { get; set; }
        public bool Private { get; set; }
        public bool IsBot { get; set; }
        public string ProfileMsg { get; set; }
        public int Points { get; set; }
        public string Pseudo { get; set; }
        public string Group { get; set; }
        public string Secret { get; set; }

        public override void Map(in Data.Models.User model)
        {
            Id = model.Id;
            RegistrationDate = model.RegistrationDate;
            FirstName = model.FirstName;
            LastName = model.LastName;
            Icon = model.Icon;
            Email = model.Email;
            Private = model.Private;
            ProfileMsg = model.ProfileMsg;
            Points = model.Points;
            Pseudo = model.Pseudo;
            Group = model.Group.Name;
            IsBot = model.IsBot;
            Secret = model.Pass;
        }
    }
}
