﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Nodes;

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
        using SqlConnection connection = new(_configuration.GetConnectionString("TaskServiceDb"));
        using SqlCommand command = new("spGetAllTasks", connection);
        command.CommandType = CommandType.StoredProcedure;

        try
        {
            await connection.OpenAsync(cancellationToken);
            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            List<TaskEntity> items = new();
            while (await reader.ReadAsync(cancellationToken))
            {
                TaskEntity item = new()
                {
                    Id = reader.GetString(nameof(TaskEntity.Id)),
                    Name = reader.GetString(nameof(TaskEntity.Name)),
                    Description = reader.GetString(nameof(TaskEntity.Description)),
                    DueDate = reader.GetDateTime(nameof(TaskEntity.DueDate)),
                    StartDate = reader.GetDateTime(nameof(TaskEntity.StartDate)),
                    EndDate = reader.GetDateTime(nameof(TaskEntity.EndDate)),
                    Priority = (TaskPriorityEnum)reader.GetInt16(nameof(TaskEntity.Priority)),
                    Status = (TaskStatusEnum)reader.GetInt16(nameof(TaskEntity.Status)),
                };
                items.Add(item);
            }

            await reader.CloseAsync();
            await connection.CloseAsync();
            return items;
        }
        catch (SqlException ex)
        {
            throw;
        }
        finally
        {
            if (connection.State != ConnectionState.Closed)
            {
                await connection.CloseAsync();
            }
        }
    }

    public async Task<TaskEntity?> FindAsync(string id, CancellationToken cancellationToken)
    {
        using SqlConnection connection = new(_configuration.GetConnectionString("TaskServiceDb"));
        using SqlCommand command = new("spGetTaskById", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Id", id);

        try
        {
            await connection.OpenAsync(cancellationToken);
            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            TaskEntity? item = null;
            if (await reader.ReadAsync(cancellationToken))
            {
                item = new()
                {
                    Id = reader.GetString(nameof(TaskEntity.Id)),
                    Name = reader.GetString(nameof(TaskEntity.Name)),
                    Description = reader.GetString(nameof(TaskEntity.Description)),
                    DueDate = reader.GetDateTime(nameof(TaskEntity.DueDate)),
                    StartDate = reader.GetDateTime(nameof(TaskEntity.StartDate)),
                    EndDate = reader.GetDateTime(nameof(TaskEntity.EndDate)),
                    Priority = (TaskPriorityEnum)reader.GetInt16(nameof(TaskEntity.Priority)),
                    Status = (TaskStatusEnum)reader.GetInt16(nameof(TaskEntity.Status)),
                };
            }

            await reader.CloseAsync();
            await connection.CloseAsync();
            return item;
        }
        catch (SqlException ex)
        {
            throw;
        }
        finally
        {
            if (connection.State != ConnectionState.Closed)
            {
                await connection.CloseAsync();
            }
        }
    }

    public async Task<TaskEntity> AddAsync(TaskEntity entity, CancellationToken cancellationToken)
    {
        using SqlConnection connection = new(_configuration.GetConnectionString("TaskServiceDb"));
        using SqlCommand command = new("spInsertTask", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue($"@{nameof(TaskEntity.Id)}", entity.Id);
        command.Parameters.AddWithValue($"@{nameof(TaskEntity.Name)}", entity.Name);
        command.Parameters.AddWithValue($"@{nameof(TaskEntity.Description)}", entity.Description);
        command.Parameters.AddWithValue($"@{nameof(TaskEntity.DueDate)}", entity.DueDate);
        command.Parameters.AddWithValue($"@{nameof(TaskEntity.StartDate)}", entity.StartDate);
        command.Parameters.AddWithValue($"@{nameof(TaskEntity.EndDate)}", entity.EndDate);
        command.Parameters.AddWithValue($"@{nameof(TaskEntity.Priority)}", entity.Priority);
        command.Parameters.AddWithValue($"@{nameof(TaskEntity.Status)}", entity.Status);

        try
        {
            await connection.OpenAsync(cancellationToken);
            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (!await reader.ReadAsync(cancellationToken))
            {
                throw new InvalidOperationException();
            }

            TaskEntity item = new()
            {
                Id = reader.GetString(nameof(TaskEntity.Id)),
                Name = reader.GetString(nameof(TaskEntity.Name)),
                Description = reader.GetString(nameof(TaskEntity.Description)),
                DueDate = reader.GetDateTime(nameof(TaskEntity.DueDate)),
                StartDate = reader.GetDateTime(nameof(TaskEntity.StartDate)),
                EndDate = reader.GetDateTime(nameof(TaskEntity.EndDate)),
                Priority = (TaskPriorityEnum)reader.GetInt16(nameof(TaskEntity.Priority)),
                Status = (TaskStatusEnum)reader.GetInt16(nameof(TaskEntity.Status)),
            };

            await reader.CloseAsync();
            await connection.CloseAsync();
            return item;
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
                throw new DataValidationException(new ValidationResult(validationResult.Message, new[] { validationResult.Member }));
            }
            throw;
        }
        finally
        {
            if (connection.State != ConnectionState.Closed)
            {
                await connection.CloseAsync();
            }
        }
    }

    public async Task<TaskEntity> UpdateAsync(TaskEntity entity, CancellationToken cancellationToken)
    {
        using SqlConnection connection = new(_configuration.GetConnectionString("TaskServiceDb"));
        using SqlCommand command = new("spUpdateTask", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue($"@{nameof(TaskEntity.Id)}", entity.Id);
        command.Parameters.AddWithValue($"@{nameof(TaskEntity.Name)}", entity.Name);
        command.Parameters.AddWithValue($"@{nameof(TaskEntity.Description)}", entity.Description);
        command.Parameters.AddWithValue($"@{nameof(TaskEntity.DueDate)}", entity.DueDate);
        command.Parameters.AddWithValue($"@{nameof(TaskEntity.StartDate)}", entity.StartDate);
        command.Parameters.AddWithValue($"@{nameof(TaskEntity.EndDate)}", entity.EndDate);
        command.Parameters.AddWithValue($"@{nameof(TaskEntity.Priority)}", entity.Priority);
        command.Parameters.AddWithValue($"@{nameof(TaskEntity.Status)}", entity.Status);

        try
        {
            await connection.OpenAsync(cancellationToken);
            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (!await reader.ReadAsync(cancellationToken))
            {
                throw new InvalidOperationException();
            }

            TaskEntity item = new()
            {
                Id = reader.GetString(nameof(TaskEntity.Id)),
                Name = reader.GetString(nameof(TaskEntity.Name)),
                Description = reader.GetString(nameof(TaskEntity.Description)),
                DueDate = reader.GetDateTime(nameof(TaskEntity.DueDate)),
                StartDate = reader.GetDateTime(nameof(TaskEntity.StartDate)),
                EndDate = reader.GetDateTime(nameof(TaskEntity.EndDate)),
                Priority = (TaskPriorityEnum)reader.GetInt16(nameof(TaskEntity.Priority)),
                Status = (TaskStatusEnum)reader.GetInt16(nameof(TaskEntity.Status)),
            };

            await reader.CloseAsync();
            await connection.CloseAsync();
            return item;
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
                throw new DataValidationException(new ValidationResult(validationResult.Message, new[] { validationResult.Member }));
            }
            throw;
        }
        finally
        {
            if (connection.State != ConnectionState.Closed)
            {
                await connection.CloseAsync();
            }
        }
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        using SqlConnection connection = new(_configuration.GetConnectionString("TaskServiceDb"));
        using var command = new SqlCommand("spDeleteTask", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue($"@{nameof(TaskEntity.Id)}", id);

        try
        {
            await connection.OpenAsync(cancellationToken);
            await command.ExecuteNonQueryAsync(cancellationToken);
            await connection.CloseAsync();
        }
        catch (SqlException ex)
        {

            throw;
        }
        finally
        {
            if (connection.State != ConnectionState.Closed)
            {
                await connection.CloseAsync();
            }
        }
    }
}