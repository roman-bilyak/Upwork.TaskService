using AutoMapper;
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
        private readonly IMapper _mapper;

        public UpdateTaskCommandHandler
        (
            ITaskManager taskManager,
            IValidator<UpdateTaskDto> validator,
            IMapper mapper
        )
        {
            ArgumentNullException.ThrowIfNull(taskManager, nameof(taskManager));
            ArgumentNullException.ThrowIfNull(validator, nameof(validator));
            ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));

            _taskManager = taskManager;
            _validator = validator;
            _mapper = mapper;
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

            _mapper.Map(request.Model, taskEntity);
            taskEntity = await _taskManager.UpdateAsync(taskEntity, cancellationToken);

            return _mapper.Map<TaskDto>(taskEntity);
        }
    }
}