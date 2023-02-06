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
    public async Task GetAll_ShoudReturnTasks_Test()
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

        TaskDto? taskDto = response.SingleOrDefault(x => x.Id == newTaskId);
        Assert.That(taskDto, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(taskDto.Id, Is.EqualTo(newTaskId));
            Assert.That(taskDto.Name, Is.EqualTo(createTaskDto.Name));
            Assert.That(taskDto.Description, Is.EqualTo(createTaskDto.Description));
            Assert.That(taskDto.DueDate, Is.EqualTo(createTaskDto.DueDate));
            Assert.That(taskDto.StartDate, Is.EqualTo(createTaskDto.StartDate));
            Assert.That(taskDto.EndDate, Is.EqualTo(createTaskDto.EndDate));
            Assert.That(taskDto.Priority, Is.EqualTo(createTaskDto.Priority));
            Assert.That(taskDto.Status, Is.EqualTo(createTaskDto.Status));
        });
    }

    [Test]
    public async Task GetById_ShouldReturnTask_Test()
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
        Assert.Multiple(() =>
        {
            Assert.That(response.Id, Is.EqualTo(taskId));
            Assert.That(response.Name, Is.EqualTo(createTaskDto.Name));
            Assert.That(response.Description, Is.EqualTo(createTaskDto.Description));
            Assert.That(response.DueDate, Is.EqualTo(createTaskDto.DueDate));
            Assert.That(response.StartDate, Is.EqualTo(createTaskDto.StartDate));
            Assert.That(response.EndDate, Is.EqualTo(createTaskDto.EndDate));
            Assert.That(response.Priority, Is.EqualTo(createTaskDto.Priority));
            Assert.That(response.Status, Is.EqualTo(createTaskDto.Status));
        });
    }

    [Test]
    public void GetById_ThrowNotFoundExceptionWhenTaskNotExists_Test()
    {
        // Arrange
        string invalidTaskId = Guid.NewGuid().ToString();

        // Act
        EntityNotFoundException exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await _taskController.GetByIdAsync(invalidTaskId));

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(exception.Message, Is.EqualTo($"Entity (type = '{typeof(TaskEntity)}', id = '{invalidTaskId}') not found"));
            Assert.That(exception.Id, Is.EqualTo(invalidTaskId));
            Assert.That(exception.EntityType, Is.EqualTo(typeof(TaskEntity)));
        });
    }

    [Test]
    public async Task Create_ShouldCreateTask_Test()
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
        Assert.Multiple(() =>
        {
            Assert.That(response.Id, Is.Not.Null);
            Assert.That(response.Name, Is.EqualTo(createTaskDto.Name));
            Assert.That(response.Description, Is.EqualTo(createTaskDto.Description));
            Assert.That(response.DueDate, Is.EqualTo(createTaskDto.DueDate));
            Assert.That(response.StartDate, Is.EqualTo(createTaskDto.StartDate));
            Assert.That(response.EndDate, Is.EqualTo(createTaskDto.EndDate));
            Assert.That(response.Priority, Is.EqualTo(createTaskDto.Priority));
            Assert.That(response.Status, Is.EqualTo(createTaskDto.Status));
        });
    }

    [Test]
    public void Create_ShouldNotCreateTaskWithInvalidData_NameRequired_Test()
    {
        // Arrange
        CreateTaskDto createTaskDto = new()
        {
            Name = string.Empty,
            Description = Guid.NewGuid().ToString(),
            DueDate = DateTime.Today.AddDays(10),
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(2),
            Priority = TaskPriorityEnum.Low,
            Status = TaskStatusEnum.New,
        };

        // Act
        var dataValidationException = Assert.ThrowsAsync<DataValidationException>(async () => await _taskController.CreateAsync(createTaskDto));

        // Assert
        Assert.That(dataValidationException, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(dataValidationException.Message, Is.EqualTo("ModelState is not valid! See ValidationErrors for details."));
            Assert.That(dataValidationException.Errors, Is.Not.Null);
        });
        Assert.That(dataValidationException.Errors, Has.Count.EqualTo(1));
        Assert.That(dataValidationException.Errors[0].MemberNames, Is.Not.Null);
        Assert.That(dataValidationException.Errors[0].MemberNames.Count, Is.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(dataValidationException.Errors[0].MemberNames.ElementAt(0), Is.EqualTo(nameof(CreateTaskDto.Name)));
            Assert.That(dataValidationException.Errors[0].ErrorMessage, Is.EqualTo($"'{nameof(CreateTaskDto.Name)}' must not be empty."));
        });
    }

    [Test]
    public void Create_ShouldNotCreateTaskWithInvalidData_DescriptionRequired_Test()
    {
        // Arrange
        CreateTaskDto createTaskDto = new()
        {
            Name = Guid.NewGuid().ToString(),
            Description = string.Empty,
            DueDate = DateTime.Today.AddDays(10),
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(2),
            Priority = TaskPriorityEnum.Low,
            Status = TaskStatusEnum.New,
        };

        // Act
        var dataValidationException = Assert.ThrowsAsync<DataValidationException>(async () => await _taskController.CreateAsync(createTaskDto));

        // Assert
        Assert.That(dataValidationException, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(dataValidationException.Message, Is.EqualTo("ModelState is not valid! See ValidationErrors for details."));
            Assert.That(dataValidationException.Errors, Is.Not.Null);
        });
        Assert.That(dataValidationException.Errors, Has.Count.EqualTo(1));
        Assert.That(dataValidationException.Errors[0].MemberNames, Is.Not.Null);
        Assert.That(dataValidationException.Errors[0].MemberNames.Count, Is.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(dataValidationException.Errors[0].MemberNames.ElementAt(0), Is.EqualTo(nameof(CreateTaskDto.Description)));
            Assert.That(dataValidationException.Errors[0].ErrorMessage, Is.EqualTo($"'{nameof(CreateTaskDto.Description)}' must not be empty."));
        });
    }

    [Test]
    public void Create_ShouldNotCreateTaskWithInvalidData_NameMaxLength_Test()
    {
        // Arrange
        CreateTaskDto createTaskDto = new()
        {
            Name = new string('a', TaskEntity.MaximumNameLength + 1),
            Description = Guid.NewGuid().ToString(),
            DueDate = DateTime.Today.AddDays(10),
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(2),
            Priority = TaskPriorityEnum.Low,
            Status = TaskStatusEnum.New,
        };

        // Act
        var dataValidationException = Assert.ThrowsAsync<DataValidationException>(async () => await _taskController.CreateAsync(createTaskDto));

        // Assert
        Assert.That(dataValidationException, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(dataValidationException.Message, Is.EqualTo("ModelState is not valid! See ValidationErrors for details."));
            Assert.That(dataValidationException.Errors, Is.Not.Null);
        });
        Assert.That(dataValidationException.Errors, Has.Count.EqualTo(1));
        Assert.That(dataValidationException.Errors[0].MemberNames, Is.Not.Null);
        Assert.That(dataValidationException.Errors[0].MemberNames.Count, Is.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(dataValidationException.Errors[0].MemberNames.ElementAt(0), Is.EqualTo(nameof(CreateTaskDto.Name)));
            Assert.That(dataValidationException.Errors[0].ErrorMessage, Is.EqualTo($"The length of '{nameof(CreateTaskDto.Name)}' must be {TaskEntity.MaximumNameLength} characters or fewer. You entered {createTaskDto.Name.Length} characters."));
        });
    }

    [Test]
    public void Create_ShouldNotCreateTaskWithInvalidData_DescriptionMaxLength_Test()
    {
        // Arrange
        CreateTaskDto createTaskDto = new()
        {
            Name = Guid.NewGuid().ToString(),
            Description = new string('a', TaskEntity.MaximumDescriptionLength + 1),
            DueDate = DateTime.Today.AddDays(10),
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(2),
            Priority = TaskPriorityEnum.Low,
            Status = TaskStatusEnum.New,
        };

        // Act
        var dataValidationException = Assert.ThrowsAsync<DataValidationException>(async () => await _taskController.CreateAsync(createTaskDto));

        // Assert
        Assert.That(dataValidationException, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(dataValidationException.Message, Is.EqualTo("ModelState is not valid! See ValidationErrors for details."));
            Assert.That(dataValidationException.Errors, Is.Not.Null);
        });
        Assert.That(dataValidationException.Errors, Has.Count.EqualTo(1));
        Assert.That(dataValidationException.Errors[0].MemberNames, Is.Not.Null);
        Assert.That(dataValidationException.Errors[0].MemberNames.Count, Is.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(dataValidationException.Errors[0].MemberNames.ElementAt(0), Is.EqualTo(nameof(CreateTaskDto.Description)));
            Assert.That(dataValidationException.Errors[0].ErrorMessage, Is.EqualTo($"The length of '{nameof(CreateTaskDto.Description)}' must be {TaskEntity.MaximumDescriptionLength} characters or fewer. You entered {createTaskDto.Description.Length} characters."));
        });
    }

    [Test]
    public async Task Update_ShouldUpdateTask_Test()
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
        Assert.Multiple(() =>
        {
            Assert.That(response.Id, Is.EqualTo(taskId));
            Assert.That(response.Name, Is.EqualTo(updateTaskDto.Name));
            Assert.That(response.Description, Is.EqualTo(updateTaskDto.Description));
            Assert.That(response.DueDate, Is.EqualTo(updateTaskDto.DueDate));
            Assert.That(response.StartDate, Is.EqualTo(updateTaskDto.StartDate));
            Assert.That(response.EndDate, Is.EqualTo(updateTaskDto.EndDate));
            Assert.That(response.Priority, Is.EqualTo(updateTaskDto.Priority));
            Assert.That(response.Status, Is.EqualTo(updateTaskDto.Status));
        });
    }

    [Test]
    public async Task Update_ShouldNotUpdateTaskWithInvalidData_NameRequired_Test()
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
            Name = string.Empty,
            Description = "[Updated]" + createTaskDto.Description,
            DueDate = createTaskDto.DueDate.AddDays(5),
            StartDate = createTaskDto.StartDate.AddDays(2),
            EndDate = createTaskDto.EndDate.AddDays(3),
            Priority = TaskPriorityEnum.Medium,
            Status = TaskStatusEnum.InProgress,
        };
        var dataValidationException = Assert.ThrowsAsync<DataValidationException>(async () => await _taskController.UpdateAsync(taskId, updateTaskDto));

        // Assert
        Assert.That(dataValidationException, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(dataValidationException.Message, Is.EqualTo("ModelState is not valid! See ValidationErrors for details."));
            Assert.That(dataValidationException.Errors, Is.Not.Null);
        });
        Assert.That(dataValidationException.Errors, Has.Count.EqualTo(1));
        Assert.That(dataValidationException.Errors[0].MemberNames, Is.Not.Null);
        Assert.That(dataValidationException.Errors[0].MemberNames.Count, Is.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(dataValidationException.Errors[0].MemberNames.ElementAt(0), Is.EqualTo(nameof(UpdateTaskDto.Name)));
            Assert.That(dataValidationException.Errors[0].ErrorMessage, Is.EqualTo($"'{nameof(UpdateTaskDto.Name)}' must not be empty."));
        });
    }

    [Test]
    public async Task Update_ShouldNotUpdateTaskWithInvalidData_DescriptionRequired_Test()
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
            Description = string.Empty,
            DueDate = createTaskDto.DueDate.AddDays(5),
            StartDate = createTaskDto.StartDate.AddDays(2),
            EndDate = createTaskDto.EndDate.AddDays(3),
            Priority = TaskPriorityEnum.Medium,
            Status = TaskStatusEnum.InProgress,
        };
        var dataValidationException = Assert.ThrowsAsync<DataValidationException>(async () => await _taskController.UpdateAsync(taskId, updateTaskDto));

        // Assert
        Assert.That(dataValidationException, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(dataValidationException.Message, Is.EqualTo("ModelState is not valid! See ValidationErrors for details."));
            Assert.That(dataValidationException.Errors, Is.Not.Null);
        });
        Assert.That(dataValidationException.Errors, Has.Count.EqualTo(1));
        Assert.That(dataValidationException.Errors[0].MemberNames, Is.Not.Null);
        Assert.That(dataValidationException.Errors[0].MemberNames.Count, Is.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(dataValidationException.Errors[0].MemberNames.ElementAt(0), Is.EqualTo(nameof(UpdateTaskDto.Description)));
            Assert.That(dataValidationException.Errors[0].ErrorMessage, Is.EqualTo($"'{nameof(UpdateTaskDto.Description)}' must not be empty."));
        });
    }

    [Test]
    public async Task Update_ShouldNotUpdateTaskWithInvalidData_NameMaxLength_Test()
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
            Name = new string('a', TaskEntity.MaximumNameLength + 5),
            Description = "[Updated]" + createTaskDto.Description,
            DueDate = createTaskDto.DueDate.AddDays(5),
            StartDate = createTaskDto.StartDate.AddDays(2),
            EndDate = createTaskDto.EndDate.AddDays(3),
            Priority = TaskPriorityEnum.Medium,
            Status = TaskStatusEnum.InProgress,
        };
        var dataValidationException = Assert.ThrowsAsync<DataValidationException>(async () => await _taskController.UpdateAsync(taskId, updateTaskDto));

        // Assert
        Assert.That(dataValidationException, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(dataValidationException.Message, Is.EqualTo("ModelState is not valid! See ValidationErrors for details."));
            Assert.That(dataValidationException.Errors, Is.Not.Null);
        });
        Assert.That(dataValidationException.Errors, Has.Count.EqualTo(1));
        Assert.That(dataValidationException.Errors[0].MemberNames, Is.Not.Null);
        Assert.That(dataValidationException.Errors[0].MemberNames.Count, Is.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(dataValidationException.Errors[0].MemberNames.ElementAt(0), Is.EqualTo(nameof(UpdateTaskDto.Name)));
            Assert.That(dataValidationException.Errors[0].ErrorMessage, Is.EqualTo($"The length of '{nameof(UpdateTaskDto.Name)}' must be {TaskEntity.MaximumNameLength} characters or fewer. You entered {updateTaskDto.Name.Length} characters."));
        });
    }

    [Test]
    public async Task Update_ShouldNotUpdateTaskWithInvalidData_DescriptionMaxLength_Test()
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
            Description = new string('a', TaskEntity.MaximumDescriptionLength + 10),
            DueDate = createTaskDto.DueDate.AddDays(5),
            StartDate = createTaskDto.StartDate.AddDays(2),
            EndDate = createTaskDto.EndDate.AddDays(3),
            Priority = TaskPriorityEnum.Medium,
            Status = TaskStatusEnum.InProgress,
        };
        var dataValidationException = Assert.ThrowsAsync<DataValidationException>(async () => await _taskController.UpdateAsync(taskId, updateTaskDto));

        // Assert
        Assert.That(dataValidationException, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(dataValidationException.Message, Is.EqualTo("ModelState is not valid! See ValidationErrors for details."));
            Assert.That(dataValidationException.Errors, Is.Not.Null);
        });
        Assert.That(dataValidationException.Errors, Has.Count.EqualTo(1));
        Assert.That(dataValidationException.Errors[0].MemberNames, Is.Not.Null);
        Assert.That(dataValidationException.Errors[0].MemberNames.Count, Is.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(dataValidationException.Errors[0].MemberNames.ElementAt(0), Is.EqualTo(nameof(UpdateTaskDto.Description)));
            Assert.That(dataValidationException.Errors[0].ErrorMessage, Is.EqualTo($"The length of '{nameof(UpdateTaskDto.Description)}' must be {TaskEntity.MaximumDescriptionLength} characters or fewer. You entered {updateTaskDto.Description.Length} characters."));
        });
    }

    [Test]
    public void Update_ThrowNotFoundExceptionWhenTaskNotExists_Test()
    {
        // Arrange
        string invalidTaskId = Guid.NewGuid().ToString();

        // Act
        UpdateTaskDto updateTaskDto = new()
        {
            Name = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            DueDate = DateTime.Today.AddDays(10),
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(2),
            Priority = TaskPriorityEnum.Medium,
            Status = TaskStatusEnum.InProgress,
        };
        EntityNotFoundException exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await _taskController.UpdateAsync(invalidTaskId, updateTaskDto));

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(exception.Message, Is.EqualTo($"Entity (type = '{typeof(TaskEntity)}', id = '{invalidTaskId}') not found"));
            Assert.That(exception.Id, Is.EqualTo(invalidTaskId));
            Assert.That(exception.EntityType, Is.EqualTo(typeof(TaskEntity)));
        });
    }

    [Test]
    public async Task Delete_ShouldDeleteTask_Test()
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

    [Test]
    public void Delete_ThrowNotFoundExceptionWhenTaskNotExists_Test()
    {
        // Arrange
        string invalidTaskId = Guid.NewGuid().ToString();

        // Act
        EntityNotFoundException exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await _taskController.DeleteAsync(invalidTaskId));

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(exception.Message, Is.EqualTo($"Entity (type = '{typeof(TaskEntity)}', id = '{invalidTaskId}') not found"));
            Assert.That(exception.Id, Is.EqualTo(invalidTaskId));
            Assert.That(exception.EntityType, Is.EqualTo(typeof(TaskEntity)));
        });
    }
}