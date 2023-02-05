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
    public async Task ShoudReturnTasks_Test()
    {
        // Arrange
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

        // Act
        List<TaskDto> response = await _taskController.GetAllAsync();

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Any(), Is.True);
        Assert.That(response.Find(x => x.Id == newTaskId), Is.Not.Null);
    }

    [Test]
    public void ThrowNotFoundEntityExceptionWhenNotExists_Test()
    {
        // Arrange
        string invalidTaskId = Guid.NewGuid().ToString();

        // Act

        // Assert
        Assert.ThrowsAsync<EntityNotFoundException>(async () => await _taskController.GetByIdAsync(invalidTaskId));
    }

    [Test]
    public async Task ShouldReturnTaskById_Test()
    {
        // Arrange
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
        string taskId = (await _taskController.CreateAsync(createTaskDto)).Id;

        // Act
        TaskDto response = await _taskController.GetByIdAsync(taskId);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Id, Is.EqualTo(taskId));
        Assert.That(response.Name, Is.EqualTo(createTaskDto.Name));
        Assert.That(response.Description, Is.EqualTo(createTaskDto.Description));
        Assert.That(response.DueDate, Is.EqualTo(createTaskDto.DueDate));
        Assert.That(response.StartDate, Is.EqualTo(createTaskDto.StartDate));
        Assert.That(response.EndDate, Is.EqualTo(createTaskDto.EndDate));
        Assert.That(response.Priority, Is.EqualTo(createTaskDto.Priority));
        Assert.That(response.Status, Is.EqualTo(createTaskDto.Status));
    }

    [Test]
    public async Task ShouldCreateTask_Test()
    {
        // Arrange
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

        // Act
        TaskDto response = await _taskController.CreateAsync(createTaskDto);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Id, Is.Not.Null);
        Assert.That(response.Name, Is.EqualTo(createTaskDto.Name));
        Assert.That(response.Description, Is.EqualTo(createTaskDto.Description));
        Assert.That(response.DueDate, Is.EqualTo(createTaskDto.DueDate));
        Assert.That(response.StartDate, Is.EqualTo(createTaskDto.StartDate));
        Assert.That(response.EndDate, Is.EqualTo(createTaskDto.EndDate));
        Assert.That(response.Priority, Is.EqualTo(createTaskDto.Priority));
        Assert.That(response.Status, Is.EqualTo(createTaskDto.Status));
    }

    [Test]
    public async Task ShouldUpdateTask_Test()
    {
        // Arrange
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
        string taskId = (await _taskController.CreateAsync(createTaskDto)).Id;

        // Act
        UpdateTaskDto updateTaskDto = new()
        {
            Name = "[Updated]" + createTaskDto.Name,
            Description = "[Updated]" + createTaskDto.Description,
            DueDate = createTaskDto.DueDate.AddDays(5),
            StartDate = createTaskDto.StartDate.AddDays(2),
            EndDate = createTaskDto.EndDate.AddDays(3),
            Priority = TaskPriorityEnum.Medium,
            Status = TaskStatusEnum.InProgress,
        };
        TaskDto response = await _taskController.UpdateAsync(taskId, updateTaskDto);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Id, Is.EqualTo(taskId));
        Assert.That(response.Name, Is.EqualTo(updateTaskDto.Name));
        Assert.That(response.Description, Is.EqualTo(updateTaskDto.Description));
        Assert.That(response.DueDate, Is.EqualTo(updateTaskDto.DueDate));
        Assert.That(response.StartDate, Is.EqualTo(updateTaskDto.StartDate));
        Assert.That(response.EndDate, Is.EqualTo(updateTaskDto.EndDate));
        Assert.That(response.Priority, Is.EqualTo(updateTaskDto.Priority));
        Assert.That(response.Status, Is.EqualTo(updateTaskDto.Status));
    }

    [Test]
    public async Task ShouldDeleteTask_Test()
    {
        // Arrange
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
        string taskId = (await _taskController.CreateAsync(createTaskDto)).Id;

        // Act
        await _taskController.DeleteAsync(taskId);
    }
}