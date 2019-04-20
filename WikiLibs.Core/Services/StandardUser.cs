using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WikiLibs.Shared.Service;

namespace WikiLibs.Core.Services
{
    public class StandardUser : IUser
    {
        private Dictionary<string, bool> _perms;

        public StandardUser(IHttpContextAccessor ctx, Data.Context dbCtx)
        {
            if (ctx.HttpContext.User != null && ctx.HttpContext.User.Identity.IsAuthenticated)
            {
                 _perms = new Dictionary<string, bool>();
                User = dbCtx.Users.Find(new object[] { ctx.HttpContext.User.FindFirst(ClaimTypes.Name).Value });
                foreach (var perm in User.Group.Permissions)
                    _perms[perm.Perm] = true;
            }
        }

        public bool HasPermission(string name)
        {
            if (_perms == null)
                return (false);
            return (_perms[name]);
        }

        public bool IsExternal => false;

        public Data.Models.User User { get; }

        public string UserId => User.Id;
    }
}
