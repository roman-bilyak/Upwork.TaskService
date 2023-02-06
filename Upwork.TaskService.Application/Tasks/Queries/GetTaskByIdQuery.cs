using AutoMapper;
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
        private readonly IMapper _mapper;

        public GetTaskByIdQueryHandler
        (
            ITaskManager taskManager,
            IMapper mapper
        )
        {
            ArgumentNullException.ThrowIfNull(taskManager, nameof(taskManager));
            ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));

            _taskManager = taskManager;
            _mapper = mapper;
        }

        public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            TaskEntity? taskEntity = await _taskManager.GetByIdAsync(request.Id, cancellationToken);
            if (taskEntity is null)
            {
                throw new EntityNotFoundException(typeof(TaskEntity), request.Id);
            }

            return _mapper.Map<TaskDto>(taskEntity);
        }
    }
}