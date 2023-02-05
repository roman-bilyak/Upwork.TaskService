using FluentValidation;

namespace Upwork.TaskService.Tasks;

internal class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.DueDate)
            .LessThan(DateTime.Today)
            .WithMessage("The Due Date cannot be in the past");
    }
}