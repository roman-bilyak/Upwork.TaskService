using MediatR;

namespace Upwork.TaskService.Tasks;

public class CreateTaskCommand : IRequest<TaskDto>
{
    public CreateTaskDto Model { get; protected set; }

    public CreateTaskCommand(CreateTaskDto model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        Model = model;
    }

    internal class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
    {
        private readonly ITaskManager _taskManager;

        public CreateTaskCommandHandler
        (
            ITaskManager taskManager
        )
        {
            ArgumentNullException.ThrowIfNull(taskManager, nameof(taskManager));

            _taskManager = taskManager;
        }

        public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            TaskEntity taskEntity = new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Model.Name,
                Description = request.Model.Description,
                DueDate = request.Model.DueDate,
                StartDate = request.Model.StartDate,
                EndDate = request.Model.EndDate,
                Priority = request.Model.Priority,
                Status = request.Model.Status,
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
    }
}