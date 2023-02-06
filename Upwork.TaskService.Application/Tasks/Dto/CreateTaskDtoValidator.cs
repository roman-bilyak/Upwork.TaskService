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

        Transform(x => x.DueDate, x => x.Date);

        Transform(x => x.StartDate, x => x.Date);

        Transform(x => x.EndDate, x => x.Date);
    }
}