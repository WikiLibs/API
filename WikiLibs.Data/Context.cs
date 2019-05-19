using Microsoft.EntityFrameworkCore;
using System;
using WikiLibs.Data.Models;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Data
{
    public class Context : DbContext
    {
        public DbSet<Symbol> Symbols { get; set; }
        public DbSet<Prototype> Prototypes { get; set; }
        public DbSet<PrototypeParam> PrototypeParams { get; set; }
        public DbSet<SymbolRef> SymbolRefs { get; set; }
        public DbSet<Info> InfoTable { get; set; }

        public DbSet<Example> Examples { get; set; }
        public DbSet<ExampleCodeLine> ExampleCodeLines { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<APIKey> APIKeys { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        public Context(DbContextOptions options)
            : base(options)
        {
        }
    }
}
