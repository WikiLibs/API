﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WikiLibs.Core.Auth;
using WikiLibs.Shared.Service;

namespace WikiLibs.Core.Services
{
    public class StandardUser : IUser
    {
        private Dictionary<string, bool> _perms;
        private Data.Context _context;

        public StandardUser(IHttpContextAccessor ctx, Data.Context dbCtx)
        {
            if (ctx.HttpContext.User != null
                && ctx.HttpContext.User.Identity.IsAuthenticated
                && ctx.HttpContext.User.Identity.AuthenticationType != ApiKeyAuthentication.SCHEME)
            {
                 _perms = new Dictionary<string, bool>();
                User = dbCtx.Users.Find(new object[] { ctx.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value });
                foreach (var perm in User.Group.Permissions)
                    _perms[perm.Perm] = true;
                _context = dbCtx;
            }
        }

        public bool HasPermission(string name)
        {
            if (_perms == null)
                return (false);
            if (_perms.ContainsKey("*") && _perms["*"])
                return (true);
            if (!_perms.ContainsKey(name))
                return (false);
            return (_perms[name]);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public bool IsExternal => false;

        public Data.Models.User User { get; }

        public string UserId => User.Id;

        public int Points
        {
            get
            {
                return (User.Points);
            }
            set
            {
                User.Points = value;
            }
        }
    }
}
