﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using XTracker.Context;

#nullable disable

namespace XTracker.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240417134316_CreateEntityTables")]
    partial class CreateEntityTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("ProviderKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("RoleId")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("XTracker.Models.Habits.Day", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int?>("Id"));

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("Date")
                        .IsUnique();

                    b.ToTable("Days");
                });

            modelBuilder.Entity("XTracker.Models.Habits.DayHabit", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int?>("Id"));

                    b.Property<int?>("DayId")
                        .HasColumnType("int");

                    b.Property<int?>("HabitId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DayId");

                    b.HasIndex("HabitId");

                    b.ToTable("DayHabits");
                });

            modelBuilder.Entity("XTracker.Models.Habits.Habit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.ToTable("Habits");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Beber 2L água"
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Exercitar"
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2024, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Ler 30 minutos"
                        });
                });

            modelBuilder.Entity("XTracker.Models.Habits.HabitWeekDay", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int?>("Id"));

                    b.Property<int?>("HabitId")
                        .HasColumnType("int");

                    b.Property<int?>("WeekDay")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HabitId");

                    b.ToTable("HabitWeekDays");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            HabitId = 1,
                            WeekDay = 0
                        },
                        new
                        {
                            Id = 2,
                            HabitId = 1,
                            WeekDay = 1
                        },
                        new
                        {
                            Id = 3,
                            HabitId = 1,
                            WeekDay = 2
                        },
                        new
                        {
                            Id = 4,
                            HabitId = 1,
                            WeekDay = 3
                        },
                        new
                        {
                            Id = 5,
                            HabitId = 1,
                            WeekDay = 4
                        },
                        new
                        {
                            Id = 6,
                            HabitId = 1,
                            WeekDay = 5
                        },
                        new
                        {
                            Id = 7,
                            HabitId = 1,
                            WeekDay = 6
                        },
                        new
                        {
                            Id = 8,
                            HabitId = 2,
                            WeekDay = 1
                        },
                        new
                        {
                            Id = 9,
                            HabitId = 2,
                            WeekDay = 2
                        },
                        new
                        {
                            Id = 10,
                            HabitId = 2,
                            WeekDay = 3
                        },
                        new
                        {
                            Id = 11,
                            HabitId = 2,
                            WeekDay = 4
                        },
                        new
                        {
                            Id = 12,
                            HabitId = 2,
                            WeekDay = 5
                        },
                        new
                        {
                            Id = 13,
                            HabitId = 3,
                            WeekDay = 1
                        },
                        new
                        {
                            Id = 14,
                            HabitId = 3,
                            WeekDay = 3
                        },
                        new
                        {
                            Id = 15,
                            HabitId = 3,
                            WeekDay = 5
                        });
                });

            modelBuilder.Entity("XTracker.Models.Habits.DayHabit", b =>
                {
                    b.HasOne("XTracker.Models.Habits.Day", "Day")
                        .WithMany("DayHabits")
                        .HasForeignKey("DayId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("XTracker.Models.Habits.Habit", "Habit")
                        .WithMany("DayHabits")
                        .HasForeignKey("HabitId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Day");

                    b.Navigation("Habit");
                });

            modelBuilder.Entity("XTracker.Models.Habits.HabitWeekDay", b =>
                {
                    b.HasOne("XTracker.Models.Habits.Habit", "Habit")
                        .WithMany("WeekDays")
                        .HasForeignKey("HabitId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Habit");
                });

            modelBuilder.Entity("XTracker.Models.Habits.Day", b =>
                {
                    b.Navigation("DayHabits");
                });

            modelBuilder.Entity("XTracker.Models.Habits.Habit", b =>
                {
                    b.Navigation("DayHabits");

                    b.Navigation("WeekDays");
                });
#pragma warning restore 612, 618
        }
    }
}
