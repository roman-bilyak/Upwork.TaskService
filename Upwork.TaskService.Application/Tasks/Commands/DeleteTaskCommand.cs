using MediatR;
using System.Reflection;

namespace Upwork.TaskService.Tasks;

public class DeleteTaskCommand : IRequest
{
    public string Id { get; protected set; }

    public DeleteTaskCommand(string id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));

        Id = id;
    }

    internal class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
    {
        private readonly ITaskManager _taskManager;

        public DeleteTaskCommandHandler
        (
            ITaskManager taskManager
        )
        {
            ArgumentNullException.ThrowIfNull(taskManager, nameof(taskManager));

            _taskManager = taskManager;
        }

        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            TaskEntity? taskEntity = await _taskManager.GetByIdAsync(request.Id, cancellationToken);
            if (taskEntity is null)
            {
                throw new EntityNotFoundException(typeof(TaskEntity), request.Id);
            }
            await _taskManager.DeleteAsync(request.Id, cancellationToken);
            return Unit.Value;
        }
    }
}