using Microsoft.EntityFrameworkCore;
using XTracker.Models.Habits;

namespace XTracker.Context;
public class AppDbContext : DbContext
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
        // Configure HabitWeekDay
        modelBuilder.Entity<HabitWeekDay>()
            .HasKey(hw => hw.Id);
        modelBuilder.Entity<HabitWeekDay>()
            .HasOne(hw => hw.Habit)
            .WithMany(h => h.WeekDays)
            .HasForeignKey(hw => hw.HabitId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure DayHabit
        modelBuilder.Entity<DayHabit>()
            .HasKey(dh => dh.Id);
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

        // Seed
        modelBuilder.Entity<Habit>().HasData(
                  new Habit { Id = 1, Title = "Beber 2L água", CreatedAt = new DateTime(2022, 12, 31) },
                  new Habit { Id = 2, Title = "Exercitar", CreatedAt = new DateTime(2023, 1, 3) },
                  new Habit { Id = 3, Title = "Dormir 8h", CreatedAt = new DateTime(2023, 1, 8) },
                  // New habits that are not completed
                  new Habit { Id = 4, Title = "Ler 30 minutos", CreatedAt = new DateTime(2023, 1, 10) },
                  new Habit { Id = 5, Title = "Meditar", CreatedAt = new DateTime(2023, 1, 15) }
              );

        modelBuilder.Entity<HabitWeekDay>().HasData(
            new HabitWeekDay { Id = 1, HabitId = 1, WeekDay = 1 },
            new HabitWeekDay { Id = 2, HabitId = 1, WeekDay = 2 },
            new HabitWeekDay { Id = 3, HabitId = 1, WeekDay = 3 },
            new HabitWeekDay { Id = 4, HabitId = 2, WeekDay = 3 },
            new HabitWeekDay { Id = 5, HabitId = 2, WeekDay = 4 },
            new HabitWeekDay { Id = 6, HabitId = 2, WeekDay = 5 },
            new HabitWeekDay { Id = 7, HabitId = 3, WeekDay = 1 },
            new HabitWeekDay { Id = 8, HabitId = 3, WeekDay = 2 },
            new HabitWeekDay { Id = 9, HabitId = 3, WeekDay = 3 },
            new HabitWeekDay { Id = 10, HabitId = 3, WeekDay = 4 },
            new HabitWeekDay { Id = 11, HabitId = 3, WeekDay = 5 },
            new HabitWeekDay { Id = 12, HabitId = 4, WeekDay = 1 },
            new HabitWeekDay { Id = 13, HabitId = 4, WeekDay = 2 },
            new HabitWeekDay { Id = 14, HabitId = 5, WeekDay = 4 },
            new HabitWeekDay { Id = 15, HabitId = 5, WeekDay = 5 }
        );

        modelBuilder.Entity<Day>().HasData(
            new Day { Id = 1, Date = new DateTime(2023, 1, 2) },
            new Day { Id = 2, Date = new DateTime(2023, 1, 6) },
            new Day { Id = 3, Date = new DateTime(2023, 1, 4) }
        );

        modelBuilder.Entity<DayHabit>().HasData(
            new DayHabit { Id = 1, DayId = 1, HabitId = 1 },
            new DayHabit { Id = 2, DayId = 2, HabitId = 1 },
            new DayHabit { Id = 3, DayId = 3, HabitId = 1 },
            new DayHabit { Id = 4, DayId = 3, HabitId = 2 }
        );
    }
}