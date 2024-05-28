namespace XTracker.Models.Habits
{
    public class DayHabit
    {
        public Guid Id { get; set; }
        public Guid DayId { get; set; }
        public Guid HabitId { get; set; }
        public Day? Day { get; set; }
        public Habit? Habit { get; set; }
    }
}
