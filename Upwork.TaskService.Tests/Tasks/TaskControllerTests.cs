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
            Name = GetRandomName(),
            Description = GetRandomDescription(),
            DueDate = DateTime.Now.AddDays(10),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
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
            Assert.That(taskDto.Name, Is.EqualTo(createTaskDto.Name.Trim()));
            Assert.That(taskDto.Description, Is.EqualTo(createTaskDto.Description.Trim()));
            Assert.That(taskDto.DueDate, Is.EqualTo(createTaskDto.DueDate.Date));
            Assert.That(taskDto.StartDate, Is.EqualTo(createTaskDto.StartDate.Date));
            Assert.That(taskDto.EndDate, Is.EqualTo(createTaskDto.EndDate.Date));
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
            Name = GetRandomName(),
            Description = GetRandomDescription(),
            DueDate = DateTime.Now.AddDays(10),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
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
            Assert.That(response.Name, Is.EqualTo(createTaskDto.Name.Trim()));
            Assert.That(response.Description, Is.EqualTo(createTaskDto.Description.Trim()));
            Assert.That(response.DueDate, Is.EqualTo(createTaskDto.DueDate.Date));
            Assert.That(response.StartDate, Is.EqualTo(createTaskDto.StartDate.Date));
            Assert.That(response.EndDate, Is.EqualTo(createTaskDto.EndDate.Date));
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
            Name = GetRandomName(),
            Description = GetRandomDescription(),
            DueDate = DateTime.Now.AddDays(10),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
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
            Assert.That(response.Name, Is.EqualTo(createTaskDto.Name.Trim()));
            Assert.That(response.Description, Is.EqualTo(createTaskDto.Description.Trim()));
            Assert.That(response.DueDate, Is.EqualTo(createTaskDto.DueDate.Date));
            Assert.That(response.StartDate, Is.EqualTo(createTaskDto.StartDate.Date));
            Assert.That(response.EndDate, Is.EqualTo(createTaskDto.EndDate.Date));
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
            Description = GetRandomDescription(),
            DueDate = DateTime.Now.AddDays(10),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
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
    public void Create_ShouldNotCreateTaskWithInvalidData_DueDateInPast_Test()
    {
        // Arrange
        CreateTaskDto createTaskDto = new()
        {
            Name = string.Empty,
            Description = GetRandomDescription(),
            DueDate = DateTime.Now.AddDays(-10),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
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
            Assert.That(dataValidationException.Errors[0].MemberNames.ElementAt(0), Is.EqualTo(nameof(CreateTaskDto.DueDate)));
            Assert.That(dataValidationException.Errors[0].ErrorMessage, Is.EqualTo($"'{nameof(CreateTaskDto.DueDate)}' cannot be in the past'"));
        });
    }

    [Test]
    public void Create_ShouldNotCreateTaskWithInvalidData_DescriptionRequired_Test()
    {
        // Arrange
        CreateTaskDto createTaskDto = new()
        {
            Name = GetRandomName(),
            Description = string.Empty,
            DueDate = DateTime.Now.AddDays(10),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
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
            Name = GetRandomName(TaskEntity.MaximumNameLength + 1, TaskEntity.MaximumNameLength + 10),
            Description = GetRandomDescription(),
            DueDate = DateTime.Now.AddDays(10),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
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
            Assert.That(dataValidationException.Errors[0].ErrorMessage, Is.EqualTo($"The length of '{nameof(CreateTaskDto.Name)}' must be {TaskEntity.MaximumNameLength} characters or fewer. You entered {createTaskDto.Name.Trim().Length} characters."));
        });
    }

    [Test]
    public void Create_ShouldNotCreateTaskWithInvalidData_DescriptionMaxLength_Test()
    {
        // Arrange
        CreateTaskDto createTaskDto = new()
        {
            Name = GetRandomName(),
            Description = GetRandomDescription(TaskEntity.MaximumDescriptionLength + 1, TaskEntity.MaximumDescriptionLength + 10),
            DueDate = DateTime.Now.AddDays(10),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
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
            Assert.That(dataValidationException.Errors[0].ErrorMessage, Is.EqualTo($"The length of '{nameof(CreateTaskDto.Description)}' must be {TaskEntity.MaximumDescriptionLength} characters or fewer. You entered {createTaskDto.Description.Trim().Length} characters."));
        });
    }

    [Test]
    public async Task Update_ShouldUpdateTask_Test()
    {
        // Arrange
        CreateTaskDto createTaskDto = new()
        {
            Name = GetRandomName(),
            Description = GetRandomDescription(),
            DueDate = DateTime.Now.AddDays(10),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            Priority = TaskPriorityEnum.Low,
            Status = TaskStatusEnum.New,
        };
        TaskDto taskDto = await _taskController.CreateAsync(createTaskDto);

        // Act
        UpdateTaskDto updateTaskDto = new()
        {
            Name = GetRandomName(),
            Description = GetRandomDescription(),
            DueDate = createTaskDto.DueDate.AddDays(5),
            StartDate = createTaskDto.StartDate.AddDays(2),
            EndDate = createTaskDto.EndDate.AddDays(3),
            Priority = TaskPriorityEnum.Medium,
            Status = TaskStatusEnum.InProgress,
        };
        TaskDto response = await _taskController.UpdateAsync(taskDto.Id, updateTaskDto);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(response.Id, Is.EqualTo(taskDto.Id));
            Assert.That(response.Name, Is.EqualTo(updateTaskDto.Name.Trim()));
            Assert.That(response.Description, Is.EqualTo(updateTaskDto.Description.Trim()));
            Assert.That(response.DueDate, Is.EqualTo(updateTaskDto.DueDate.Date));
            Assert.That(response.StartDate, Is.EqualTo(updateTaskDto.StartDate.Date));
            Assert.That(response.EndDate, Is.EqualTo(updateTaskDto.EndDate.Date));
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
            Name = GetRandomName(),
            Description = GetRandomDescription(),
            DueDate = DateTime.Now.AddDays(10),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            Priority = TaskPriorityEnum.Low,
            Status = TaskStatusEnum.New,
        };
        string taskId = (await _taskController.CreateAsync(createTaskDto)).Id;

        // Act
        UpdateTaskDto updateTaskDto = new()
        {
            Name = string.Empty,
            Description = GetRandomDescription(),
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
            Name = GetRandomName(),
            Description = GetRandomDescription(),
            DueDate = DateTime.Now.AddDays(10),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            Priority = TaskPriorityEnum.Low,
            Status = TaskStatusEnum.New,
        };
        string taskId = (await _taskController.CreateAsync(createTaskDto)).Id;

        // Act
        UpdateTaskDto updateTaskDto = new()
        {
            Name = GetRandomName(),
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
            Name = GetRandomName(),
            Description = GetRandomDescription(),
            DueDate = DateTime.Now.AddDays(10),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            Priority = TaskPriorityEnum.Low,
            Status = TaskStatusEnum.New,
        };
        string taskId = (await _taskController.CreateAsync(createTaskDto)).Id;

        // Act
        UpdateTaskDto updateTaskDto = new()
        {
            Name = GetRandomName(TaskEntity.MaximumNameLength + 1, TaskEntity.MaximumNameLength + 10),
            Description = GetRandomDescription(),
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
            Assert.That(dataValidationException.Errors[0].ErrorMessage, Is.EqualTo($"The length of '{nameof(UpdateTaskDto.Name)}' must be {TaskEntity.MaximumNameLength} characters or fewer. You entered {updateTaskDto.Name.Trim().Length} characters."));
        });
    }

    [Test]
    public async Task Update_ShouldNotUpdateTaskWithInvalidData_DescriptionMaxLength_Test()
    {
        // Arrange
        CreateTaskDto createTaskDto = new()
        {
            Name = GetRandomName(),
            Description = GetRandomDescription(),
            DueDate = DateTime.Now.AddDays(10),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            Priority = TaskPriorityEnum.Low,
            Status = TaskStatusEnum.New,
        };
        string taskId = (await _taskController.CreateAsync(createTaskDto)).Id;

        // Act
        UpdateTaskDto updateTaskDto = new()
        {
            Name = GetRandomName(),
            Description = GetRandomDescription(TaskEntity.MaximumDescriptionLength + 1, TaskEntity.MaximumDescriptionLength + 10),
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
            Assert.That(dataValidationException.Errors[0].ErrorMessage, Is.EqualTo($"The length of '{nameof(UpdateTaskDto.Description)}' must be {TaskEntity.MaximumDescriptionLength} characters or fewer. You entered {updateTaskDto.Description.Trim().Length} characters."));
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
            Name = GetRandomName(),
            Description = GetRandomDescription(),
            DueDate = DateTime.Now.AddDays(10),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
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
            Name = GetRandomName(),
            Description = GetRandomDescription(),
            DueDate = DateTime.Now.AddDays(10),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
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
        string invalidTaskId = GetRandomId();

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

    #region helper methods

    private static string GetRandomId()
    {
        return Guid.NewGuid().ToString();
    }

    private static string GetRandomName(int minLength = 1, int maxLength = TaskEntity.MaximumNameLength)
    {
        return new string(' ', Random.Shared.Next(0, 5)) + RandomString(minLength, maxLength) + new string(' ', Random.Shared.Next(0, 5));
    }

    private static string GetRandomDescription(int minLength = 1, int maxLength = TaskEntity.MaximumDescriptionLength)
    {
        return new string(' ', Random.Shared.Next(0, 5)) + RandomString(minLength, maxLength) + new string(' ', Random.Shared.Next(0, 5));
    }

    private static string RandomString(int minLength, int maxLength)
    {
        if (maxLength < minLength)
        {
            maxLength = minLength;
        }

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        return new string(Enumerable.Repeat(chars, Random.Shared.Next(minLength, maxLength + 1))
            .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
    }

    #endregion
}