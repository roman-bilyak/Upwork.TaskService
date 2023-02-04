using Upwork.TaskService.Tasks;

namespace Upwork.TaskService.Controllers;

public record CreateTaskDto
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime DueDate { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public TaskPriorityEnum Priority { get; set; }

    public TaskStatusEnum Status { get; set; }
}