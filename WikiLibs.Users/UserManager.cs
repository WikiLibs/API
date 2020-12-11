using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.Data;
using WikiLibs.Data.Models;
using WikiLibs.Shared;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Modules.File;

namespace WikiLibs.Users
{
    [Module(Interface = typeof(IUserManager))]
    public class UserManager : BaseCRUDOperations<Context, User, string>, IUserManager
    {
        public UserManager(Context ctx) : base(ctx)
        {
        }

        public async Task<User> GetAsync(string email, string pass)
        {
            return (await Set.FirstOrDefaultAsync(x => x.Email == email && x.Pass == pass));
        }

        public void CheckDuplicates(string key, User mdl)
        {
            if (Set.Any(x => (x.Pseudo == mdl.Pseudo || x.Email == mdl.Email) && x.Id != key))
                throw new Shared.Exceptions.ResourceAlreadyExists()
                {
                    ResourceId = key,
                    ResourceName = mdl.Email,
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

            if (mdl.Pseudo != usr.Pseudo && Set.Any(x => (x.Pseudo == mdl.Pseudo)))
                throw new Shared.Exceptions.ResourceAlreadyExists()
                {
                    ResourceId = key,
                    ResourceName = mdl.Pseudo,
                    ResourceType = typeof(User)
                };
            if (mdl.Email != usr.Email && Set.Any(x => (x.Email == mdl.Email)))
                throw new Shared.Exceptions.ResourceAlreadyExists()
                {
                    ResourceId = key,
                    ResourceName = mdl.Email,
                    ResourceType = typeof(User)
                };
            usr.FirstName = mdl.FirstName;
            usr.LastName = mdl.LastName;
            usr.ProfileMsg = mdl.ProfileMsg;
            usr.Pseudo = mdl.Pseudo;
            var grp = Context.Groups.Where(x => x.Id == mdl.GroupId).FirstOrDefault();
            if (grp != null)
                usr.Group = grp;
            usr.Points = mdl.Points;
            usr.Private = mdl.Private;
            usr.Pass = mdl.Pass;
            usr.Email = mdl.Email;
            await SaveChanges();
            return (usr);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return (await Set.FirstOrDefaultAsync(x => x.Email == email));
        }

        public async Task PostFileAsync(User data, ImageFile fle)
        {
            var res = ImageUtils.ResizeImage(fle.OpenReadStream(), new System.Drawing.Size(512, 512), System.Drawing.Imaging.ImageFormat.Jpeg);
            data.Icon = res;
            await SaveChanges();
        }

        class UserIcon : ImageFile
        {
            public override string ContentType => "image/jpeg";

            public override long Length => _data.Length;

            public override string Name => "Image";

            private byte[] _data;

            public UserIcon(byte[] data)
            {
                _data = data;
            }

            public override Stream OpenReadStream()
            {
                return (new MemoryStream(_data));
            }
        }

        public ImageFile GetFile(User data)
        {
            if (data.Icon == null || data.Icon.Length <= 0)
                throw new Shared.Exceptions.ResourceNotFound()
                {
                    ResourceId = data.Id.ToString(),
                    ResourceName = "Icon",
                    ResourceType = typeof(User)
                };
            return (new UserIcon(data.Icon));
        }
    }
}
