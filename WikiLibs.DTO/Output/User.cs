using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.DTO.Output
{
    public class User : IGetDTO<Data.Models.User>
    {
        public string id { get; set; }
        public string date { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string icon { get; set; }
        public string email { get; set; }
        public bool @private { get; set; }
        public string profileMsg { get; set; }
        public int points { get; set; }
        public string pseudo { get; set; }
        public string group { get; set; }

        public void Map(in Data.Models.User model)
        {
            id = model.UUID;
            date = model.RegistrationDate.ToString();
            firstName = model.FirstName;
            lastName = model.LastName;
            icon = model.Icon;
            email = model.EMail;
            @private = model.Private;
            profileMsg = model.ProfileMsg;
            points = model.Points;
            pseudo = model.Pseudo;
            group = model.Group.Name;
        }
    }
}
