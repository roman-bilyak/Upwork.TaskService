using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Upwork.TaskService.Tasks;

internal class TaskControllerTests : BaseIntegrationTests
{
    private TaskController _taskController;

    [SetUp]
    protected void SetUp()
    {
        _taskController = new TaskController(ServiceProvider.GetRequiredService<IMediator>());
    }

    [Test]
    public async Task ShoudReturnAllTasks_Test()
    {
        List<TaskDto> response = await _taskController.GetAllAsync();

        Assert.That(response, Is.Not.Null);
    }

    [Test]
    public void ThrowNotFoundEntityExceptionWhenNotExists_Test()
    {
        string invalidTaskId = Guid.NewGuid().ToString();

        Assert.ThrowsAsync<EntityNotFoundException>(async () => await _taskController.GetByIdAsync(invalidTaskId));
    }

    [Test]
    public async Task ShouldReturnTaskById_Test()
    {
        CreateTaskDto createTaskDto = new()
        {
            Name = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            DueDate = DateTime.Today.AddDays(10),
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(2),
            Priority = TaskPriorityEnum.Low,
            Status = TaskStatusEnum.New,
        };
        string newTaskId = (await _taskController.CreateAsync(createTaskDto)).Id;

        TaskDto response = await _taskController.GetByIdAsync(newTaskId);

        Assert.That(response, Is.Not.Null);
        Assert.Equals(createTaskDto.Name, response.Name);
        Assert.Equals(createTaskDto.Description, response.Description);
        Assert.Equals(createTaskDto.DueDate, response.DueDate);
        Assert.Equals(createTaskDto.StartDate, response.StartDate);
        Assert.Equals(createTaskDto.EndDate, response.EndDate);
        Assert.Equals(createTaskDto.Priority, response.Priority);
        Assert.Equals(createTaskDto.Status, response.Status);
    }
}