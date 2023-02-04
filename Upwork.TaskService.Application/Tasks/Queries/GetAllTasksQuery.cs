using MediatR;

namespace Upwork.TaskService.Tasks;

public class GetAllTasksQuery : IRequest<List<TaskDto>>
{
    internal class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, List<TaskDto>>
    {
        private readonly ITaskManager _taskManager;

        public GetAllTasksQueryHandler
        (
            ITaskManager taskManager
        )
        {
            ArgumentNullException.ThrowIfNull(taskManager, nameof(taskManager));

            _taskManager = taskManager;
        }

        public async Task<List<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
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
    }
}