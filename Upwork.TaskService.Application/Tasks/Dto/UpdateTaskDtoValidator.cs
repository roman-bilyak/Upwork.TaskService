﻿using FluentValidation;
using Microsoft.VisualBasic;

namespace Upwork.TaskService.Tasks;

internal class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskDtoValidator()
    {
        Transform(x => x.Name, x => x.Trim())
            .NotEmpty()
            .MaximumLength(100);

        Transform(x => x.Description, x => x.Trim())
            .NotEmpty()
            .MaximumLength(500);

        Transform(x => x.DueDate, x => x.Date)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage($"'{nameof(CreateTaskDto.DueDate)}' cannot be in the past");

        Transform(x => x.StartDate, x => x.Date);

        Transform(x => x.EndDate, x => x.Date);
    }
}