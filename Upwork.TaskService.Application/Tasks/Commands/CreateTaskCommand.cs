using FluentValidation;
using FluentValidation.Results;
using MediatR;
using DataAnnotationValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace Upwork.TaskService.Tasks;

public class CreateTaskCommand : IRequest<TaskDto>
{
    public CreateTaskDto Model { get; protected set; }

    public CreateTaskCommand(CreateTaskDto model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        Model = model;
    }

    internal class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
    {
        private readonly ITaskManager _taskManager;
        private readonly IValidator<CreateTaskDto> _validator;

        public CreateTaskCommandHandler
        (
            ITaskManager taskManager,
            IValidator<CreateTaskDto> validator
        )
        {
            ArgumentNullException.ThrowIfNull(taskManager, nameof(taskManager));
            ArgumentNullException.ThrowIfNull(validator, nameof(validator));

            _taskManager = taskManager;
            _validator = validator;
        }

        public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(request.Model, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(x => new DataAnnotationValidationResult(x.ErrorMessage, new[] { x.PropertyName }))
                    .ToArray();

                throw new DataValidationException("ModelState is not valid! See ValidationErrors for details.", errors);
            }

            TaskEntity taskEntity = new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Model.Name,
                Description = request.Model.Description,
                DueDate = request.Model.DueDate,
                StartDate = request.Model.StartDate,
                EndDate = request.Model.EndDate,
                Priority = request.Model.Priority,
                Status = request.Model.Status,
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
    }
}