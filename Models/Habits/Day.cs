namespace XTracker.Models.Habits
{
    public class Day
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public List<DayHabit>? DayHabits { get; set; }
    }
}
