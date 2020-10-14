﻿// <auto-generated />
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WikiLibs.Data;

namespace WikiLibs.Data.Migrations
{
    [DbContext(typeof(Context))]
    [ExcludeFromCodeCoverage]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WikiLibs.Data.Models.ApiKey", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Flags")
                        .HasColumnType("int");

                    b.Property<string>("Origin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UseNum")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ApiKeys");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Examples.Example", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("RequestId")
                        .HasColumnType("bigint");

                    b.Property<long>("SymbolId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("SymbolId");

                    b.HasIndex("UserId");

                    b.ToTable("Examples");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Examples.ExampleCodeLine", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ExampleId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ExampleId");

                    b.ToTable("ExampleCodeLines");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Examples.ExampleComment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ExampleId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ExampleId");

                    b.HasIndex("UserId");

                    b.ToTable("ExampleComments");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Examples.ExampleRequest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("ApplyToId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("DataId")
                        .HasColumnType("bigint");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApplyToId");

                    b.HasIndex("DataId")
                        .IsUnique()
                        .HasFilter("[DataId] IS NOT NULL");

                    b.ToTable("ExampleRequests");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Group", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Permission", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("GroupId")
                        .HasColumnType("bigint");

                    b.Property<string>("Perm")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.Exception", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("PrototypeId")
                        .HasColumnType("bigint");

                    b.Property<long?>("RefId")
                        .HasColumnType("bigint");

                    b.Property<string>("RefPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PrototypeId");

                    b.HasIndex("RefId");

                    b.ToTable("Exceptions");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.Import", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SymbolImports");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.Lang", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Icon")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("SymbolLangs");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.Lib", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Icon")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Description");

                    b.Property<string>("Copyright");

                    b.HasKey("Id");

                    b.ToTable("SymbolLibs");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.Prototype", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("SymbolId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("SymbolId");

                    b.ToTable("Prototypes");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.PrototypeParam", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("PrototypeId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("PrototypeId");

                    b.ToTable("PrototypeParams");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.PrototypeParamSymbolRef", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("PrototypeParamId")
                        .HasColumnType("bigint");

                    b.Property<long?>("RefId")
                        .HasColumnType("bigint");

                    b.Property<string>("RefPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PrototypeParamId")
                        .IsUnique();

                    b.HasIndex("RefId");

                    b.ToTable("PrototypeParamSymbolRefs");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.Symbol", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("ImportId")
                        .HasColumnType("bigint");

                    b.Property<long>("LangId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("LastModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("LibId")
                        .HasColumnType("bigint");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("TypeId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("Views")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ImportId");

                    b.HasIndex("LangId");

                    b.HasIndex("LibId");

                    b.HasIndex("Path")
                        .IsUnique()
                        .HasFilter("[Path] IS NOT NULL");

                    b.HasIndex("TypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Symbols");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.SymbolRef", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("RefId")
                        .HasColumnType("bigint");

                    b.Property<string>("RefPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("SymbolId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RefId");

                    b.HasIndex("SymbolId");

                    b.ToTable("SymbolRefs");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.Type", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("SymbolTypes");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Confirmation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("GroupId")
                        .HasColumnType("bigint");

                    b.Property<byte[]>("Icon")
                        .HasColumnType("varbinary(max)");

                    b.Property<bool>("IsBot")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pass")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<bool>("Private")
                        .HasColumnType("bit");

                    b.Property<string>("ProfileMsg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pseudo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Examples.Example", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Symbols.Symbol", "Symbol")
                        .WithMany("Examples")
                        .HasForeignKey("SymbolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WikiLibs.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Examples.ExampleCodeLine", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Examples.Example", "Example")
                        .WithMany("Code")
                        .HasForeignKey("ExampleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Examples.ExampleComment", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Examples.Example", "Example")
                        .WithMany("Comments")
                        .HasForeignKey("ExampleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WikiLibs.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Examples.ExampleRequest", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Examples.Example", "ApplyTo")
                        .WithMany()
                        .HasForeignKey("ApplyToId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("WikiLibs.Data.Models.Examples.Example", "Data")
                        .WithOne("Request")
                        .HasForeignKey("WikiLibs.Data.Models.Examples.ExampleRequest", "DataId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Permission", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Group", "Group")
                        .WithMany("Permissions")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.Exception", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Symbols.Prototype", "Prototype")
                        .WithMany("Exceptions")
                        .HasForeignKey("PrototypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WikiLibs.Data.Models.Symbols.Symbol", "Ref")
                        .WithMany()
                        .HasForeignKey("RefId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.Prototype", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Symbols.Symbol", "Symbol")
                        .WithMany("Prototypes")
                        .HasForeignKey("SymbolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.PrototypeParam", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Symbols.Prototype", "Prototype")
                        .WithMany("Parameters")
                        .HasForeignKey("PrototypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.PrototypeParamSymbolRef", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Symbols.PrototypeParam", "PrototypeParam")
                        .WithOne("SymbolRef")
                        .HasForeignKey("WikiLibs.Data.Models.Symbols.PrototypeParamSymbolRef", "PrototypeParamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WikiLibs.Data.Models.Symbols.Symbol", "Ref")
                        .WithMany()
                        .HasForeignKey("RefId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.Symbol", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Symbols.Import", "Import")
                        .WithMany()
                        .HasForeignKey("ImportId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("WikiLibs.Data.Models.Symbols.Lang", "Lang")
                        .WithMany()
                        .HasForeignKey("LangId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WikiLibs.Data.Models.Symbols.Lib", "Lib")
                        .WithMany()
                        .HasForeignKey("LibId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WikiLibs.Data.Models.Symbols.Type", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WikiLibs.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("WikiLibs.Data.Models.Symbols.SymbolRef", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Symbols.Symbol", "Ref")
                        .WithMany()
                        .HasForeignKey("RefId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("WikiLibs.Data.Models.Symbols.Symbol", "Symbol")
                        .WithMany("Symbols")
                        .HasForeignKey("SymbolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WikiLibs.Data.Models.User", b =>
                {
                    b.HasOne("WikiLibs.Data.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.SetNull);
                });
#pragma warning restore 612, 618
        }
    }
}
