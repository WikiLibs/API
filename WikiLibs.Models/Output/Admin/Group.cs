using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Output.Admin
{
    public class Group : GetModel<Group, Data.Models.Group>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string[] Permissions { get; set; }

        public override void Map(in Data.Models.Group model)
        {
            Id = model.Id;
            Name = model.Name;
            Permissions = new string[model.Permissions.Count];
            int id = 0;
            foreach (var p in model.Permissions)
                Permissions[id++] = p.Perm;
        }
    }
}
