using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Input.Admin
{
    public class GroupUpdate : PatchModel<GroupUpdate, Group>
    {
        public string Name { get; set; }
        public string[] Permissions { get; set; }

        public override Group CreatePatch(in Group current)
        {
            var group = new Group()
            {
                Name = Name != null ? Name : current.Name
            };

            if (Permissions == null)
                group.Permissions = null;
            else
            {
                foreach (var p in Permissions)
                {
                    group.Permissions.Add(new Permission()
                    {
                        Group = group,
                        Perm = p
                    });
                }
            }
            return (group);
        }
    }
}
