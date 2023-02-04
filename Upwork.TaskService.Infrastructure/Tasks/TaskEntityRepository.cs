namespace Upwork.TaskService.Tasks;

internal class TaskEntityRepository : IRepository<TaskEntity>
{
    public Task<List<TaskEntity>> ListAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TaskEntity?> FindAsync(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TaskEntity> AddAsync(TaskEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TaskEntity> UpdateAsync(TaskEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TaskEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}