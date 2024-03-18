namespace XTracker.Models.Habits
{
    public class HabitWeekDay
    {
        public int? Id { get; set; }
        public int? HabitId { get; set; }
        public int? WeekDay { get; set; }
        public Habit? Habit { get; set; }
    }
}
