namespace XTracker.DTOs.HabitDTOs
{
    public class SummaryDTO
    {
        public Guid Id { get; set; }
        public DateTime? Date { get; set; }
        public int? Completed { get; set; }
        public int? Amount { get; set; }
    }
}
