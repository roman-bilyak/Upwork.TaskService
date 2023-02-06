using FluentValidation;
using FluentValidation.Results;
using MediatR;
using DataAnnotationValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace Upwork.TaskService.Tasks;

public class UpdateTaskCommand : IRequest<TaskDto>
{
    public string Id { get; protected set; }

    public UpdateTaskDto Model { get; protected set; }

    public UpdateTaskCommand(string id, UpdateTaskDto model)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        Id = id;
        Model = model;
    }

    internal class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
    {
        private readonly ITaskManager _taskManager;
        private readonly IValidator<UpdateTaskDto> _validator;

        public UpdateTaskCommandHandler
        (
            ITaskManager taskManager,
            IValidator<UpdateTaskDto> validator
        )
        {
            ArgumentNullException.ThrowIfNull(taskManager, nameof(taskManager));
            ArgumentNullException.ThrowIfNull(validator, nameof(validator));

            _taskManager = taskManager;
            _validator = validator;
        }

        public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(request.Model, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(x => new DataAnnotationValidationResult(x.ErrorMessage, new[] { x.PropertyName }))
                    .ToArray();

                throw new DataValidationException(errors);
            }

            TaskEntity? taskEntity = await _taskManager.GetByIdAsync(request.Id, cancellationToken);
            if (taskEntity is null)
            {
                throw new EntityNotFoundException(typeof(TaskEntity), request.Id);
            }

            taskEntity.Name = request.Model.Name.Trim();
            taskEntity.Description = request.Model.Description.Trim();
            taskEntity.DueDate = request.Model.DueDate.Date;
            taskEntity.StartDate = request.Model.StartDate.Date;
            taskEntity.EndDate = request.Model.EndDate.Date;
            taskEntity.Priority = request.Model.Priority;
            taskEntity.Status = request.Model.Status;

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
    }
}
