using AutoMapper;
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
        private readonly IMapper _mapper;

        public CreateTaskCommandHandler
        (
            ITaskManager taskManager,
            IValidator<CreateTaskDto> validator,
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

        public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(request.Model, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(x => new DataAnnotationValidationResult(x.ErrorMessage, new[] { x.PropertyName }))
                    .ToArray();

                throw new DataValidationException(errors);
            }

            TaskEntity taskEntity = _mapper.Map<TaskEntity>(request.Model);
            taskEntity = await _taskManager.AddAsync(taskEntity, cancellationToken);

            return _mapper.Map<TaskDto>(taskEntity);
        }
    }
}