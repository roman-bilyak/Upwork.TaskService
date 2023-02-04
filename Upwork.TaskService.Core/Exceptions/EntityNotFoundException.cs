namespace Upwork.TaskService;

public class EntityNotFoundException : UpworkException
{
    public Type EntityType { get; protected set; }

    public object Id { get; protected set; }

    public EntityNotFoundException(Type entityType, object id)
        : this(entityType, id, null)
    {

    }

    public EntityNotFoundException(Type entityType, object id, Exception? innerException)
        : base($"Entity (type = '{entityType.FullName}', id = '{id}') not found", innerException)
    {
        EntityType = entityType;
        Id = id;
    }
}