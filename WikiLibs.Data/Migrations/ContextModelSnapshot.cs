﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WikiLibs.Data;

namespace WikiLibs.Data.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WikiLibs.Data.Models.APIKey", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<DateTime>("ExpirationDate");

                    b.Property<int>("Flags");

                    b.Property<int>("UseNum");

                    b.HasKey("Id");

                    b.ToTable("APIKeys");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Examples.Example", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Description");

                    b.Property<DateTime>("LastModificationDate");

                    b.Property<long>("SymbolId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("SymbolId");

                    b.HasIndex("UserId");

                    b.ToTable("Examples");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Examples.ExampleCodeLine", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment");

                    b.Property<string>("Data");

                    b.Property<long>("ExampleId");

                    b.HasKey("Id");

                    b.HasIndex("ExampleId");

                    b.ToTable("ExampleCodeLines");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Examples.ExampleRequest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("ApplyToId");

                    b.Property<DateTime>("CreationDate");

                    b.Property<long?>("DataId");

                    b.Property<string>("Message");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ApplyToId");

                    b.HasIndex("DataId");

                    b.ToTable("ExampleRequests");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Group", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Permission", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("GroupId");

                    b.Property<string>("Perm");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.Info", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Data");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("InfoTable");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.Prototype", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Data");

                    b.Property<string>("Description");

                    b.Property<long>("SymbolId");

                    b.HasKey("Id");

                    b.HasIndex("SymbolId");

                    b.ToTable("Prototypes");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.PrototypeParam", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Data");

                    b.Property<string>("Description");

                    b.Property<string>("Path");

                    b.Property<long>("PrototypeId");

                    b.HasKey("Id");

                    b.HasIndex("PrototypeId");

                    b.ToTable("PrototypeParams");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.Symbol", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Lang");

                    b.Property<DateTime>("LastModificationDate");

                    b.Property<string>("Path");

                    b.Property<string>("Type");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Symbols");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.SymbolRef", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Path");

                    b.Property<long>("SymbolId");

                    b.HasKey("Id");

                    b.HasIndex("SymbolId");

                    b.ToTable("SymbolRefs");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Confirmation");

                    b.Property<string>("EMail");

                    b.Property<string>("FirstName");

                    b.Property<long?>("GroupId");

                    b.Property<string>("Icon");

                    b.Property<string>("LastName");

                    b.Property<string>("Pass");

                    b.Property<int>("Points");

                    b.Property<bool>("Private");

                    b.Property<string>("ProfileMsg");

                    b.Property<string>("Pseudo");

                    b.Property<DateTime>("RegistrationDate");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Examples.Example", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Symbols.Symbol", "Symbol")
                        .WithMany("Examples")
                        .HasForeignKey("SymbolId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WikiLibs.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Examples.ExampleCodeLine", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Examples.Example", "Example")
                        .WithMany("Code")
                        .HasForeignKey("ExampleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Examples.ExampleRequest", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Examples.Example", "ApplyTo")
                        .WithMany()
                        .HasForeignKey("ApplyToId");

                    b.HasOne("WikiLibs.Data.Models.Examples.Example", "Data")
                        .WithMany()
                        .HasForeignKey("DataId");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Permission", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Group", "Group")
                        .WithMany("Permissions")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.Prototype", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Symbols.Symbol", "Symbol")
                        .WithMany("Prototypes")
                        .HasForeignKey("SymbolId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.PrototypeParam", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Symbols.Prototype", "Prototype")
                        .WithMany("Parameters")
                        .HasForeignKey("PrototypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.Symbol", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.SymbolRef", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Symbols.Symbol", "Symbol")
                        .WithMany("Symbols")
                        .HasForeignKey("SymbolId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WikiLibs.Data.Models.User", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");
                });
#pragma warning restore 612, 618
        }
    }
}
