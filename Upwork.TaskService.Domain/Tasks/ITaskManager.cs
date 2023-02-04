namespace Upwork.TaskService.Tasks;

public interface ITaskManager
{
    Task<List<TaskEntity>> GetAllAsync(CancellationToken cancellationToken);

    Task<TaskEntity?> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task<TaskEntity> AddAsync(TaskEntity task, CancellationToken cancellationToken);

    Task<TaskEntity> UpdateAsync(TaskEntity task, CancellationToken cancellationToken);

    Task DeleteAsync(TaskEntity task, CancellationToken cancellationToken);
}