using Microsoft.AspNetCore.Mvc;

namespace Upwork.TaskService.Controllers
{
    [ApiController]
    [Route("api")]
    public class TaskController : ControllerBase
    {
        [HttpGet("Tasks")]
        public async Task<IEnumerable<TaskDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPost("Tasks")]
        public async Task<TaskDto> CreateAsync(CreateTaskDto task, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpGet("Tasks/{id}")]
        public async Task<TaskDto> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPut("Tasks/{id}")]
        public async Task<TaskDto> UpdateAsync(int id, UpdateTaskDto task, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("Tasks/{id}")]
        public async Task<TaskDto> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}