﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserManagementSystem.Database;

#nullable disable

namespace UserManagementSystem.Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20231030085527_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UserManagementSystem.Database.Entity.User", b =>
                {
                    b.Property<int>("Index")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Index"));

                    b.Property<short>("Age")
                        .HasColumnType("smallint")
                        .HasColumnOrder(2);

                    b.Property<bool>("IsInit")
                        .HasColumnType("bit")
                        .HasColumnOrder(4);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(20)")
                        .HasColumnOrder(1);

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(12)")
                        .HasColumnOrder(3);

                    b.HasKey("Index");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Index = 1,
                            Age = (short)10,
                            IsInit = true,
                            Name = "적길동",
                            PhoneNumber = "01012341200"
                        },
                        new
                        {
                            Index = 2,
                            Age = (short)15,
                            IsInit = true,
                            Name = "홍길동",
                            PhoneNumber = "01012341201"
                        },
                        new
                        {
                            Index = 3,
                            Age = (short)20,
                            IsInit = true,
                            Name = "황길동",
                            PhoneNumber = "01012341202"
                        },
                        new
                        {
                            Index = 4,
                            Age = (short)25,
                            IsInit = true,
                            Name = "록길동",
                            PhoneNumber = "01012341203"
                        },
                        new
                        {
                            Index = 5,
                            Age = (short)30,
                            IsInit = true,
                            Name = "청길동",
                            PhoneNumber = "01012341204"
                        },
                        new
                        {
                            Index = 6,
                            Age = (short)35,
                            IsInit = true,
                            Name = "남길동",
                            PhoneNumber = "01012341205"
                        },
                        new
                        {
                            Index = 7,
                            Age = (short)40,
                            IsInit = true,
                            Name = "자길동",
                            PhoneNumber = "01012341206"
                        },
                        new
                        {
                            Index = 8,
                            Age = (short)45,
                            IsInit = true,
                            Name = "백길동",
                            PhoneNumber = "01012341207"
                        },
                        new
                        {
                            Index = 9,
                            Age = (short)50,
                            IsInit = true,
                            Name = "회길동",
                            PhoneNumber = "01012341208"
                        },
                        new
                        {
                            Index = 10,
                            Age = (short)55,
                            IsInit = true,
                            Name = "흑길동",
                            PhoneNumber = "01012341209"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
