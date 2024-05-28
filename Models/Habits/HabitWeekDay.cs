namespace XTracker.Models.Habits
{
    public class HabitWeekDay
    {
        public Guid Id { get; set; }
        public Guid HabitId { get; set; }
        public int WeekDay { get; set; }
        public Habit? Habit { get; set; }
    }
}
