namespace XTracker.Models.ToDo;
public class ToDoTask
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsImportant { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
}
