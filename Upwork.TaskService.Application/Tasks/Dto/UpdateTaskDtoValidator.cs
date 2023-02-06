using FluentValidation;

namespace Upwork.TaskService.Tasks;

internal class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskDtoValidator()
    {
        Transform(x => x.Name, x => x.Trim())
            .NotEmpty()
            .MaximumLength(TaskEntity.MaximumNameLength);

        Transform(x => x.Description, x => x.Trim())
            .NotEmpty()
            .MaximumLength(TaskEntity.MaximumDescriptionLength);
    }
}