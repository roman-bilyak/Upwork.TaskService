using System.ComponentModel.DataAnnotations;

namespace Upwork.TaskService.Tasks;

internal static class SqlValidationResultExtensions
{
    public static ValidationResult ToValidationResult(this SqlValidationResult validationResult)
    {
        return new ValidationResult(validationResult.Message, new[] { validationResult.Member });
    }
}