using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Data;
using WikiLibs.Data.Models;
using WikiLibs.Shared;
using WikiLibs.Shared.Modules.Admin;

namespace WikiLibs.Admin
{
    class GroupManager : BaseCRUDOperations<Context, Group>, IGroupManager
    {
        public GroupManager(Context ctx) : base(ctx)
        {
        }

        public Group Get(string name)
        {
            return (Set.FirstOrDefault(x => x.Name == name));
        }

        public IQueryable<Group> GetAll()
        {
            return (Set.AsQueryable());
        }

        public override async Task<Group> PostAsync(Group mdl)
        {
            var group = await Set.FirstOrDefaultAsync(x => x.Name == mdl.Name);

            if (Set.Any(x => x.Name == mdl.Name))
                throw new Shared.Exceptions.ResourceAlreadyExists()
                {
                    ResourceId = group.Id.ToString(),
                    ResourceName = mdl.Name,
                    ResourceType = typeof(Group)
                };
            return (await base.PostAsync(mdl));
        }

        public override async Task<Group> PatchAsync(long key, Group mdl)
        {
            var group = await GetAsync(key);

            foreach (var p in group.Permissions)
                Context.Remove(p);
            foreach (var p in mdl.Permissions)
                group.Permissions.Add(new Permission()
                {
                    Group = group,
                    Perm = p.Perm
                });
            await SaveChanges();
            return (group);
        }
    }
}
