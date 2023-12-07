namespace TaskList.DataAccess.DataContext;

public enum TaskItemStatus {
    Pending = 0,
    Completed = 1
}

internal class TaskItem {

    public int Id { get; set; }
    public required string Name { get; set;}
    public TaskItemStatus Status { get; set; }
    public required User User { get; set; }
    public int UserId { get; set;}

}