using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.API.Tests.Helper
{
    static class DbUtils
    {
        public static Data.Context CreateFakeDB()
        {
            var ctx = new Data.Context(new DbContextOptionsBuilder().UseInMemoryDatabase("WikiLibs").UseLazyLoadingProxies().Options);

            ctx.Add(new Data.Models.Group()
            {
                Name = "Default"
            });
            var admin = new Data.Models.Group()
            {
                Name = "Admin"
            };
            admin.Permissions.Add(new Data.Models.Permission()
            {
                Group = admin,
                Perm = "*"
            });
            ctx.Add(admin);
            ctx.SaveChanges();
            return (ctx);
        }
    }
}
