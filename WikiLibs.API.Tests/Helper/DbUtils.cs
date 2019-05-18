using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WikiLibs.API.Tests.Helper
{
    class ResettableValueGenerator : ValueGenerator<long>
    {
        private long _current;

        public override bool GeneratesTemporaryValues => false;

        public override long Next(EntityEntry entry) => Interlocked.Increment(ref _current);
        public void Reset() => _current = 0;
    }

    static class DbUtils
    {
        public static void ResetValueGenerators(DbContext context)
        {
            var cache = context.GetService<IValueGeneratorCache>();

            foreach (var keyProperty in context.Model.GetEntityTypes()
                .Select(e => e.FindPrimaryKey().Properties[0])
                .Where(p => p.ClrType == typeof(long) && p.ValueGenerated == ValueGenerated.OnAdd))
            {
                var generator = (ResettableValueGenerator)cache.GetOrAdd(keyProperty, keyProperty.DeclaringEntityType, (p, e) => new ResettableValueGenerator());
                generator.Reset();
            }
        }

        public static Data.Context CreateFakeDB()
        {
            var ctx = new Data.Context(new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).UseLazyLoadingProxies().Options);

            ResetValueGenerators(ctx);
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
