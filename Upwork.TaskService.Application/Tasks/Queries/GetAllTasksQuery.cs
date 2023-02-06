using AutoMapper;
using MediatR;

namespace Upwork.TaskService.Tasks;

public class GetAllTasksQuery : IRequest<List<TaskDto>>
{
    internal class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, List<TaskDto>>
    {
        private readonly ITaskManager _taskManager;
        private readonly IMapper _mapper;

        public GetAllTasksQueryHandler
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

        public async Task<List<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<List<TaskDto>>(await _taskManager.GetAllAsync(cancellationToken));
        }
    }
}