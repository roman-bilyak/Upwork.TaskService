namespace Upwork.TaskService.Tasks;

internal class TaskManager : DomainService, ITaskManager
{
    private readonly IRepository<TaskEntity> _taskRepository;

    public TaskManager
    (
        IRepository<TaskEntity> taskRepository
    )
    {
        _taskRepository = taskRepository;
    }

    public async Task<List<TaskEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _taskRepository.ListAsync(cancellationToken);
    }

    public async Task<TaskEntity?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _taskRepository.FindAsync(id, cancellationToken);
    }

    public async Task<TaskEntity> AddAsync(TaskEntity task, CancellationToken cancellationToken)
    {
        return await _taskRepository.AddAsync(task, cancellationToken);
    }

    public async Task<TaskEntity> UpdateAsync(TaskEntity task, CancellationToken cancellationToken)
    {
        return await _taskRepository.UpdateAsync(task, cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        await _taskRepository.DeleteAsync(id, cancellationToken);
    }
}