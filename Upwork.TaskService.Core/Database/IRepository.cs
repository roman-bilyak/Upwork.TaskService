namespace Upwork.TaskService;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<TEntity?> FindAsync(string id, CancellationToken cancellationToken);

    Task<List<TEntity>> ListAsync(CancellationToken cancellationToken);

    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);

    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    Task DeleteAsync(string id, CancellationToken cancellationToken);
}
