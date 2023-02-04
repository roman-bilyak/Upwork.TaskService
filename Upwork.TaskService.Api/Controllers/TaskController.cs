using Microsoft.AspNetCore.Mvc;
using Upwork.TaskService.Tasks;

namespace Upwork.TaskService.Controllers
{
    [ApiController]
    [Route("api/Tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskManager _taskManager;

        public TaskController
        (
            ITaskManager taskManager
        )
        {
            _taskManager = taskManager;
        }

        [HttpGet]
        public async Task<List<TaskDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return (await _taskManager.GetAllAsync(cancellationToken))
                .Select(x =>
                    new TaskDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        DueDate = x.DueDate,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        Priority = x.Priority,
                        Status = x.Status,
                    })
                .ToList();
        }

        [HttpGet("{id}")]
        public async Task<TaskDto> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            TaskEntity? taskEntity = await _taskManager.GetByIdAsync(id, cancellationToken);
            if (taskEntity is null)
            {
                throw new EntityNotFoundException(typeof(TaskEntity), id);
            }

            return new TaskDto
            {
                Id = taskEntity.Id,
                Name = taskEntity.Name,
                Description = taskEntity.Description,
                DueDate = taskEntity.DueDate,
                StartDate = taskEntity.StartDate,
                EndDate = taskEntity.EndDate,
                Priority = taskEntity.Priority,
                Status = taskEntity.Status,
            };
        }

        [HttpPost]
        public async Task<TaskDto> CreateAsync(CreateTaskDto task, CancellationToken cancellationToken)
        {
            TaskEntity taskEntity = new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = task.Name,
                Description = task.Description,
                DueDate = task.DueDate,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                Priority = task.Priority,
                Status = task.Status,
            };
            taskEntity = await _taskManager.AddAsync(taskEntity, cancellationToken);

            return new TaskDto
                    {
                        Id = taskEntity.Id,
                        Name = taskEntity.Name,
                        Description = taskEntity.Description,
                        DueDate = taskEntity.DueDate,
                        StartDate = taskEntity.StartDate,
                        EndDate = taskEntity.EndDate,
                        Priority = taskEntity.Priority,
                        Status = taskEntity.Status,
                    };
        }

        [HttpPut("{id}")]
        public async Task<TaskDto> UpdateAsync(string id, UpdateTaskDto task, CancellationToken cancellationToken)
        {
            TaskEntity taskEntity = new()
            {
                Id = id,
                Name = task.Name,
                Description = task.Description,
                DueDate = task.DueDate,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                Priority = task.Priority,
                Status = task.Status,
            };
            taskEntity = await _taskManager.UpdateAsync(taskEntity, cancellationToken);

            return new TaskDto
            {
                Id = taskEntity.Id,
                Name = taskEntity.Name,
                Description = taskEntity.Description,
                DueDate = taskEntity.DueDate,
                StartDate = taskEntity.StartDate,
                EndDate = taskEntity.EndDate,
                Priority = taskEntity.Priority,
                Status = taskEntity.Status,
            };
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            TaskEntity? taskEntity = await _taskManager.GetByIdAsync(id, cancellationToken);
            if (taskEntity is null)
            {
                throw new EntityNotFoundException(typeof(TaskEntity), id);
            }

            await _taskManager.DeleteAsync(taskEntity, cancellationToken);
        }
    }
}