﻿using Microsoft.EntityFrameworkCore;
using System;
using WikiLibs.Data.Models;

namespace WikiLibs.Data
{
    public class Context : DbContext
    {
        public DbSet<APIKey> APIKeys { get; set; }
        public DbSet<Symbol> Symbols { get; set; }
        public DbSet<SymbolRef> SymbolRefs { get; set; }
        public DbSet<Prototype> Prototypes { get; set; }
        public DbSet<PrototypeParam> PrototypeParams { get; set; }
        public DbSet<Example> Examples { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Info> InfoTable { get; set; }

        public Context(DbContextOptions options)
            : base(options)
        {
        }
    }
}