using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XTracker.Models.Habits;
using XTracker.Models.Users;

namespace XTracker.Context;
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Mapping tables
    public DbSet<Habit> Habits { get; set; }
    public DbSet<HabitWeekDay> HabitWeekDays { get; set; }
    public DbSet<Day> Days { get; set; }
    public DbSet<DayHabit> DayHabits { get; set; }

    // FluentApi
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User Identity
        modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(login => new { login.LoginProvider, login.ProviderKey });
        modelBuilder.Entity<IdentityUserToken<string>>().HasKey(token => new { token.UserId, token.LoginProvider, token.Name });

        modelBuilder.Entity<IdentityUserRole<string>>().HasKey(userRole => new { userRole.UserId, userRole.RoleId });
        //modelBuilder.Entity<SubjectAssigned>().HasKey(x => new {x.sub});

        // Configure HabitWeekDay
        modelBuilder.Entity<HabitWeekDay>()
            .HasKey(hw => hw.Id);
        modelBuilder.Entity<HabitWeekDay>()
            .Property(hw => hw.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<HabitWeekDay>()
            .HasOne(hw => hw.Habit)
            .WithMany(h => h.WeekDays)
            .HasForeignKey(hw => hw.HabitId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure DayHabit
        modelBuilder.Entity<DayHabit>()
            .HasKey(dh => dh.Id);
        modelBuilder.Entity<DayHabit>()
            .Property(dh => dh.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<DayHabit>()
            .HasOne(dh => dh.Day)
            .WithMany(d => d.DayHabits)
            .HasForeignKey(dh => dh.DayId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<DayHabit>()
            .HasOne(dh => dh.Habit)
            .WithMany(h => h.DayHabits)
            .HasForeignKey(dh => dh.HabitId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Day
        modelBuilder.Entity<Day>()
            .HasKey(d => d.Id);
        modelBuilder.Entity<Day>()
            .Property(d => d.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Day>()
            .HasIndex(d => d.Date)
            .IsUnique();

        // Configure Habit
        modelBuilder.Entity<Habit>()
            .HasKey(h => h.Id);
        modelBuilder.Entity<Habit>()
            .Property(h => h.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Habit>()
            .HasIndex(h => h.CreatedAt);
        modelBuilder.Entity<Habit>()
            .Property(h => h.Title)
            .HasMaxLength(255)
            .IsRequired();
        modelBuilder.Entity<Habit>()
            .Property(h => h.UserId)
            .IsRequired();

        modelBuilder.Entity<Habit>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(h => h.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}