using Microsoft.AspNetCore.Mvc;
using Upwork.TaskService.Tasks;

namespace Upwork.TaskService.Tasks;

[ApiController]
[ApiExplorerSettings(GroupName = "Tasks")]
[Route("api/Tasks")]
public class TaskController : ControllerBase
{
    private readonly ITaskManager _taskManager;

    public TaskController
    (
        ITaskManager taskManager
    )
    {
        _taskManager = taskManager;
    }

    [HttpGet]
    public async Task<List<TaskDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return (await _taskManager.GetAllAsync(cancellationToken))
            .Select(x =>
                new TaskDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    DueDate = x.DueDate,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Priority = x.Priority,
                    Status = x.Status,
                })
            .ToList();
    }

    [HttpGet("{taskId}")]
    public async Task<TaskDto> GetByIdAsync(string taskId, CancellationToken cancellationToken)
    {
        TaskEntity? taskEntity = await _taskManager.GetByIdAsync(taskId, cancellationToken);
        if (taskEntity is null)
        {
            throw new EntityNotFoundException(typeof(TaskEntity), taskId);
        }

        return new TaskDto
        {
            Id = taskEntity.Id,
            Name = taskEntity.Name,
            Description = taskEntity.Description,
            DueDate = taskEntity.DueDate,
            StartDate = taskEntity.StartDate,
            EndDate = taskEntity.EndDate,
            Priority = taskEntity.Priority,
            Status = taskEntity.Status,
        };
    }

    [HttpPost]
    public async Task<TaskDto> CreateAsync(CreateTaskDto task, CancellationToken cancellationToken)
    {
        TaskEntity taskEntity = new()
        {
            Id = Guid.NewGuid().ToString(),
            Name = task.Name,
            Description = task.Description,
            DueDate = task.DueDate,
            StartDate = task.StartDate,
            EndDate = task.EndDate,
            Priority = task.Priority,
            Status = task.Status,
        };
        taskEntity = await _taskManager.AddAsync(taskEntity, cancellationToken);

        return new TaskDto
                {
                    Id = taskEntity.Id,
                    Name = taskEntity.Name,
                    Description = taskEntity.Description,
                    DueDate = taskEntity.DueDate,
                    StartDate = taskEntity.StartDate,
                    EndDate = taskEntity.EndDate,
                    Priority = taskEntity.Priority,
                    Status = taskEntity.Status,
                };
    }

    [HttpPut("{taskId}")]
    public async Task<TaskDto> UpdateAsync(string taskId, UpdateTaskDto task, CancellationToken cancellationToken)
    {
        TaskEntity taskEntity = new()
        {
            Id = taskId,
            Name = task.Name,
            Description = task.Description,
            DueDate = task.DueDate,
            StartDate = task.StartDate,
            EndDate = task.EndDate,
            Priority = task.Priority,
            Status = task.Status,
        };
        taskEntity = await _taskManager.UpdateAsync(taskEntity, cancellationToken);

        return new TaskDto
        {
            Id = taskEntity.Id,
            Name = taskEntity.Name,
            Description = taskEntity.Description,
            DueDate = taskEntity.DueDate,
            StartDate = taskEntity.StartDate,
            EndDate = taskEntity.EndDate,
            Priority = taskEntity.Priority,
            Status = taskEntity.Status,
        };
    }

    [HttpDelete("{taskId}")]
    public async Task DeleteAsync(string taskId, CancellationToken cancellationToken)
    {
        await _taskManager.DeleteAsync(taskId, cancellationToken);
    }
}