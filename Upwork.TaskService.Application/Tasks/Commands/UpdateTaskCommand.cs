using MediatR;

namespace Upwork.TaskService.Tasks;

public class UpdateTaskCommand : IRequest<TaskDto>
{
    public string Id { get; protected set; }

    public UpdateTaskDto Model { get; protected set; }

    public UpdateTaskCommand(string id, UpdateTaskDto model)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        Id = id;
        Model = model;
    }

    internal class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
    {
        private readonly ITaskManager _taskManager;

        public UpdateTaskCommandHandler
        (
            ITaskManager taskManager
        )
        {
            ArgumentNullException.ThrowIfNull(taskManager, nameof(taskManager));

            _taskManager = taskManager;
        }

        public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            TaskEntity taskEntity = new()
            {
                Id = request.Id,
                Name = request.Model.Name,
                Description = request.Model.Description,
                DueDate = request.Model.DueDate,
                StartDate = request.Model.StartDate,
                EndDate = request.Model.EndDate,
                Priority = request.Model.Priority,
                Status = request.Model.Status,
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
    }
}
