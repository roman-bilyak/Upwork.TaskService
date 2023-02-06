using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace Upwork.TaskService.Tasks;

internal class TaskStoredProcedureRepository : IRepository<TaskEntity>
{
    private readonly IConfiguration _configuration;

    public TaskStoredProcedureRepository
    (
        IConfiguration configuration
    )
    {
        _configuration = configuration;
    }

    public async Task<List<TaskEntity>> ListAsync(CancellationToken cancellationToken)
    {
        return await ExecuteStoredProcedureAsync("spGetAllTasks", new Dictionary<string, object>(), cancellationToken);
    }

    public async Task<TaskEntity?> FindAsync(string id, CancellationToken cancellationToken)
    {
        Dictionary<string, object> parameters = new()
        {
            { nameof(TaskEntity.Id), id }
        };
        List<TaskEntity> result = await ExecuteStoredProcedureAsync("spGetTaskById", parameters, cancellationToken);
        return result.FirstOrDefault();
    }

    public async Task<TaskEntity> AddAsync(TaskEntity entity, CancellationToken cancellationToken)
    {
        Dictionary<string, object> parameters = new()
        {
            { nameof(TaskEntity.Id), entity.Id },
            { nameof(TaskEntity.Name), entity.Name },
            { nameof(TaskEntity.Description), entity.Description },
            { nameof(TaskEntity.DueDate), entity.DueDate },
            { nameof(TaskEntity.StartDate), entity.StartDate },
            { nameof(TaskEntity.EndDate), entity.EndDate },
            { nameof(TaskEntity.Priority), entity.Priority },
            { nameof(TaskEntity.Status), entity.Status }
        };
        List<TaskEntity> result = await ExecuteStoredProcedureAsync("spInsertTask", parameters, cancellationToken);
        TaskEntity? taskEntity = result.FirstOrDefault();
        if (taskEntity is null)
        {
            throw new InvalidOperationException();
        }
        return taskEntity;
    }

    public async Task<TaskEntity> UpdateAsync(TaskEntity entity, CancellationToken cancellationToken)
    {
        Dictionary<string, object> parameters = new()
        {
            { nameof(TaskEntity.Id), entity.Id },
            { nameof(TaskEntity.Name), entity.Name },
            { nameof(TaskEntity.Description), entity.Description },
            { nameof(TaskEntity.DueDate), entity.DueDate },
            { nameof(TaskEntity.StartDate), entity.StartDate },
            { nameof(TaskEntity.EndDate), entity.EndDate },
            { nameof(TaskEntity.Priority), entity.Priority },
            { nameof(TaskEntity.Status), entity.Status }
        };
        List<TaskEntity> result = await ExecuteStoredProcedureAsync("spUpdateTask", parameters, cancellationToken);
        TaskEntity? taskEntity = result.FirstOrDefault();
        if (taskEntity is null)
        {
            throw new InvalidOperationException();
        }
        return taskEntity;
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        Dictionary<string, object> parameters = new()
        {
            { nameof(TaskEntity.Id), id }
        };
        await ExecuteStoredProcedureAsync("spDeleteTask", parameters, cancellationToken);
    }

    #region helper methods

    private async Task<List<TaskEntity>> ExecuteStoredProcedureAsync(string name, Dictionary<string, object> parameters, CancellationToken cancellationToken)
    {
        return await ExecuteStoredProcedureAsync(name, parameters,
            x => new TaskEntity
            {
                Id = x.GetString(nameof(TaskEntity.Id)),
                Name = x.GetString(nameof(TaskEntity.Name)),
                Description = x.GetString(nameof(TaskEntity.Description)),
                DueDate = x.GetDateTime(nameof(TaskEntity.DueDate)),
                StartDate = x.GetDateTime(nameof(TaskEntity.StartDate)),
                EndDate = x.GetDateTime(nameof(TaskEntity.EndDate)),
                Priority = (TaskPriorityEnum)x.GetInt16(nameof(TaskEntity.Priority)),
                Status = (TaskStatusEnum)x.GetInt16(nameof(TaskEntity.Status)),
            }, cancellationToken);
    }

    private async Task<List<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, Dictionary<string, object> parameters, 
        Func<SqlDataReader, T> readItem, CancellationToken cancellationToken)
    {
        using SqlConnection connection = new(_configuration.GetConnectionString("TaskServiceDb"));
        using SqlCommand command = new(storedProcedureName, connection);
        command.CommandType = CommandType.StoredProcedure;

        foreach (var parameter in parameters)
        {
            command.Parameters.AddWithValue($"@{parameter.Key}", parameter.Value);
        }

        List<T> items = new();
        try
        {
            await connection.OpenAsync(cancellationToken);
            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                T item = readItem(reader);
                items.Add(item);
            }

            await reader.CloseAsync();
            await connection.CloseAsync();
            return items;
        }
        catch (SqlException ex)
        {
            SqlValidationResult? validationResult = null;
            try
            {
                validationResult = JsonConvert.DeserializeObject<SqlValidationResult>(ex.Message);
            }
            catch
            {
            }

            if (validationResult is not null)
            {
                throw new DataValidationException(validationResult.ToValidationResult());
            }
            else
            {
                throw;
            }
        }
        finally
        {
            if (connection.State != ConnectionState.Closed)
            {
                await connection.CloseAsync();
            }
        }
    }

    #endregion
}