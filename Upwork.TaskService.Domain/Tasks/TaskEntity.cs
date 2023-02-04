namespace Upwork.TaskService.Tasks;

public class TaskEntity
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime DueDate { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public TaskPriorityEnum Priority { get; set; }

    public TaskStatusEnum Status { get; set; }
}