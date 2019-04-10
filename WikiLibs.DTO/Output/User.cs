using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.DTO.Output
{
    public class User : IGetDTO<Data.Models.User>
    {
        public string Id { get; set; }
        public string Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Icon { get; set; }
        public string Email { get; set; }
        public bool Private { get; set; }
        public string ProfileMsg { get; set; }
        public int Points { get; set; }
        public string Pseudo { get; set; }
        public string Group { get; set; }

        public void Map(in Data.Models.User model)
        {
            Id = model.UUID;
            Date = model.RegistrationDate.ToString();
            FirstName = model.FirstName;
            LastName = model.LastName;
            Icon = model.Icon;
            Email = model.EMail;
            Private = model.Private;
            ProfileMsg = model.ProfileMsg;
            Points = model.Points;
            Pseudo = model.Pseudo;
            Group = model.Group.Name;
        }
    }
}
