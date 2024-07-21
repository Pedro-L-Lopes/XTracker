using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XTracker.Models.Habits;
using XTracker.Models.Users;
using System;
using XTracker.Models.ToDo;

namespace XTracker.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Mapping tables

        // Habits
        public DbSet<Habit> Habits { get; set; }
        public DbSet<HabitWeekDay> HabitWeekDays { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<DayHabit> DayHabits { get; set; }

        // ToDo
        public DbSet<ToDoTask> Tasks { get; set; }

        // FluentApi
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Identity
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(login => new { login.LoginProvider, login.ProviderKey });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(token => new { token.UserId, token.LoginProvider, token.Name });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(userRole => new { userRole.UserId, userRole.RoleId });

            // Configure HabitWeekDay
            modelBuilder.Entity<HabitWeekDay>()
                .HasKey(hw => hw.Id);
            modelBuilder.Entity<HabitWeekDay>()
                .Property(hw => hw.Id)
                .ValueGeneratedOnAdd();

            // Configure DayHabit
            modelBuilder.Entity<DayHabit>()
                .HasKey(dh => dh.Id);
            modelBuilder.Entity<DayHabit>()
                .Property(dh => dh.Id)
                .ValueGeneratedOnAdd();

            // Configure Day
            modelBuilder.Entity<Day>()
                .HasKey(d => d.Id);
            modelBuilder.Entity<Day>()
                .Property(d => d.Id)
                .ValueGeneratedOnAdd();

            // Configure Habit
            modelBuilder.Entity<Habit>()
                .HasKey(h => h.Id);
            modelBuilder.Entity<Habit>()
                .Property(h => h.Id)
                .ValueGeneratedOnAdd();

            // Configure ToDoTask
            modelBuilder.Entity<ToDoTask>()
                .HasKey(t => t.Id);
            modelBuilder.Entity<ToDoTask>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
