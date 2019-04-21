using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.Data;
using WikiLibs.Data.Models;
using WikiLibs.Shared;
using WikiLibs.Shared.Modules;

namespace WikiLibs.Users
{
    public class UserManager : BaseCRUDOperations<Context, User, string>, IUserManager
    {
        public UserManager(Context ctx) : base(ctx)
        {
        }

        public async Task<User> GetAsync(string email, string pass)
        {
            return (await Set.FirstOrDefaultAsync(x => x.EMail == email && x.Pass == pass));
        }

        public void CheckDuplicates(string key, User mdl)
        {
            if (Set.Any(x => (x.Pseudo == mdl.Pseudo || x.EMail == mdl.EMail) && x.Id != key))
                throw new Shared.Exceptions.ResourceAlreadyExists()
                {
                    ResourceId = key,
                    ResourceName = mdl.EMail,
                    ResourceType = typeof(User)
                };
        }

        public override async Task<User> PostAsync(User mdl)
        {
            CheckDuplicates(null, mdl);
            return (await base.PostAsync(mdl));
        }

        public override async Task<User> PatchAsync(string key, User mdl)
        {
            var usr = await GetAsync(key);

            CheckDuplicates(key, mdl);
            usr.FirstName = mdl.FirstName;
            usr.LastName = mdl.LastName;
            usr.ProfileMsg = mdl.ProfileMsg;
            usr.Pseudo = mdl.Pseudo;
            usr.Icon = mdl.Icon;
            usr.Group = mdl.Group;
            usr.Points = mdl.Points;
            usr.Private = mdl.Private;
            usr.Pass = mdl.Pass;
            usr.EMail = mdl.EMail;
            await SaveChanges();
            return (usr);
        }
    }
}
