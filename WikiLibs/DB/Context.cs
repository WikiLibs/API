using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.DB
{
    public class Context : DbContext
    {
        public DbSet<APIKey> APIKeys { get; set; }
        public DbSet<Symbol> Symbols { get; set; }
        public DbSet<Example> Examples { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Info> InfoTable { get; set; }

        public Context(DbContextOptions<Context> ctx)
            : base(ctx)
        {
        }
    }
}
