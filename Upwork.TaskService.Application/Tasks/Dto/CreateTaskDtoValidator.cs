using FluentValidation;

namespace Upwork.TaskService.Tasks;

internal class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskDtoValidator()
    {
        Transform(x => x.Name, x => x.Trim())
            .NotEmpty()
            .MaximumLength(TaskEntity.MaximumNameLength);

        Transform(x => x.Description, x => x.Trim())
            .NotEmpty()
            .MaximumLength(TaskEntity.MaximumDescriptionLength);
    }
}