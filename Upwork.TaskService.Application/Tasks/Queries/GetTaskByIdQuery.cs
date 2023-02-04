using MediatR;

namespace Upwork.TaskService.Tasks;

public class GetTaskByIdQuery : IRequest<TaskDto>
{
    public string Id { get; protected set; }

    public GetTaskByIdQuery(string id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));

        Id = id;
    }

    internal class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
    {
        private readonly ITaskManager _taskManager;

        public GetTaskByIdQueryHandler
        (
            ITaskManager taskManager
        )
        {
            ArgumentNullException.ThrowIfNull(taskManager, nameof(taskManager));

            _taskManager = taskManager;
        }

        public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            TaskEntity? taskEntity = await _taskManager.GetByIdAsync(request.Id, cancellationToken);
            if (taskEntity is null)
            {
                throw new EntityNotFoundException(typeof(TaskEntity), request.Id);
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
    }
}