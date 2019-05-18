using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Input.Admin
{
    public class GroupCreate : PostModel<GroupCreate, Group>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string[] Permissions { get; set; }

        public override Group CreateModel()
        {
            var group = new Group()
            {
                Name = Name
            };

            foreach (var p in Permissions)
            {
                group.Permissions.Add(new Permission()
                {
                    Group = group,
                    Perm = p
                });
            }
            return (group);
        }
    }
}
