using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Upwork.TaskService.Tasks;

[ApiController]
[ApiExplorerSettings(GroupName = "Tasks")]
[Route("api/Tasks")]
public class TaskController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskController
    (
        IMediator mediator
    )
    {
        ArgumentNullException.ThrowIfNull(mediator, nameof(mediator));

        _mediator = mediator;
    }

    [HttpGet]
    public async Task<List<TaskDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetAllTasksQuery(), cancellationToken);
    }

    [HttpGet("{taskId}")]
    public async Task<TaskDto> GetByIdAsync(string taskId, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetTaskByIdQuery(taskId), cancellationToken);
    }

    [HttpPost]
    public async Task<TaskDto> CreateAsync(CreateTaskDto task, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new CreateTaskCommand(task), cancellationToken);
    }

    [HttpPut("{taskId}")]
    public async Task<TaskDto> UpdateAsync(string taskId, UpdateTaskDto task, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new UpdateTaskCommand(taskId, task), cancellationToken);
    }

    [HttpDelete("{taskId}")]
    public async Task DeleteAsync(string taskId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTaskCommand(taskId), cancellationToken);
    }
}