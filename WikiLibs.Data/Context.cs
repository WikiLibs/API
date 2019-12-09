using Microsoft.EntityFrameworkCore;
using System;
using WikiLibs.Data.Models;
using WikiLibs.Data.Models.Examples;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Data
{
    public class Context : DbContext
    {
        #region SYMBOLS
        public DbSet<Symbol> Symbols { get; set; }
        public DbSet<Prototype> Prototypes { get; set; }
        public DbSet<PrototypeParam> PrototypeParams { get; set; }
        public DbSet<SymbolRef> SymbolRefs { get; set; }
        public DbSet<PrototypeParamSymbolRef> PrototypeParamSymbolRefs { get; set; }
        public DbSet<Lang> SymbolLangs { get; set; }
        public DbSet<Lib> SymbolLibs { get; set; }
        public DbSet<Models.Symbols.Type> SymbolTypes { get; set; }
        public DbSet<Import> SymbolImports { get; set; }
        #endregion

        #region EXAMPLES
        public DbSet<Example> Examples { get; set; }
        public DbSet<ExampleRequest> ExampleRequests { get; set; }
        public DbSet<ExampleCodeLine> ExampleCodeLines { get; set; }
        #endregion

        #region BASE
        public DbSet<User> Users { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        #endregion BASE

        public Context(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region BASE_BUILD
            modelBuilder.Entity<User>(builder =>
            {
                builder.HasOne(e => e.Group)
                    .WithOne()
                    .HasForeignKey<User>(e => e.GroupId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
                builder.HasIndex(e => e.GroupId).IsUnique(false);
            });
            modelBuilder.Entity<Permission>(builder =>
            {
                builder.HasOne(e => e.Group)
                    .WithMany(e => e.Permissions)
                    .HasForeignKey(e => e.GroupId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region SYMBOLS_BUILD
            modelBuilder.Entity<Symbol>(builder =>
            {
                builder.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
                builder.HasOne(e => e.Lang)
                    .WithMany()
                    .HasForeignKey(e => e.LangId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
                builder.HasOne(e => e.Lib)
                    .WithMany()
                    .HasForeignKey(e => e.LibId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
                builder.HasOne(e => e.Type)
                    .WithMany()
                    .HasForeignKey(e => e.TypeId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
                builder.HasOne(e => e.Import)
                    .WithMany()
                    .HasForeignKey(e => e.ImportId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
                builder.HasIndex(e => e.Path).IsUnique(true);
            });
            modelBuilder.Entity<Prototype>(builder =>
            {
                builder.HasOne(e => e.Symbol)
                    .WithMany(e => e.Prototypes)
                    .HasForeignKey(e => e.SymbolId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<PrototypeParam>(builder =>
            {
                builder.HasOne(e => e.Prototype)
                    .WithMany(e => e.Parameters)
                    .HasForeignKey(e => e.PrototypeId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<PrototypeParamSymbolRef>(builder =>
            {
                builder.HasOne(e => e.PrototypeParam)
                    .WithOne(e => e.SymbolRef)
                    .HasForeignKey<PrototypeParamSymbolRef>(e => e.PrototypeParamId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
                builder.HasOne(e => e.Ref)
                    .WithOne()
                    .HasForeignKey<PrototypeParamSymbolRef>(e => e.RefId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
                builder.HasIndex(e => e.RefId).IsUnique(false);
            });
            modelBuilder.Entity<SymbolRef>(builder =>
            {
                builder.HasOne(e => e.Symbol)
                    .WithMany(e => e.Symbols)
                    .HasForeignKey(e => e.SymbolId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
                builder.HasOne(e => e.Ref)
                    .WithOne()
                    .HasForeignKey<SymbolRef>(e => e.RefId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
                builder.HasIndex(e => e.RefId).IsUnique(false);
            });
            modelBuilder.Entity<Models.Symbols.Type>(builder =>
            {
                builder.HasIndex(e => e.Name).IsUnique(true);
            });
            modelBuilder.Entity<Models.Symbols.Lang>(builder =>
            {
                builder.HasIndex(e => e.Name).IsUnique(true);
            });
            #endregion

            #region EXAMPLES_BUILD
            modelBuilder.Entity<Example>(builder =>
            {
                builder.HasOne(e => e.Symbol)
                    .WithMany(e => e.Examples)
                    .HasForeignKey(e => e.SymbolId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
                builder.HasOne(e => e.Request)
                    .WithOne(e => e.Data)
                    .HasForeignKey<Example>(e => e.RequestId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
                builder.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
                builder.HasIndex(e => e.UserId).IsUnique(false);
            });
            modelBuilder.Entity<ExampleCodeLine>(builder =>
            {
                builder.HasOne(e => e.Example)
                    .WithMany(e => e.Code)
                    .HasForeignKey(e => e.ExampleId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<ExampleRequest>(builder =>
            {
                builder.HasOne(e => e.Data)
                    .WithOne(e => e.Request)
                    .HasForeignKey<ExampleRequest>(e => e.DataId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
                builder.HasOne(e => e.ApplyTo)
                    .WithMany()
                    .HasForeignKey(e => e.ApplyToId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion
        }
    }
}
