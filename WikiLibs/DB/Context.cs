using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.DB
{
    public class Context : DbContext
    {
        public DbSet<APIKey> APIKeys;
        public DbSet<Symbol> Symbols;
        public DbSet<Example> Examples;
        public DbSet<User> Users;
        public DbSet<Group> Groups;
        public DbSet<Permission> Permissions;
        public DbSet<Info> InfoTable;
    }
}
