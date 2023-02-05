using System.ComponentModel.DataAnnotations;

namespace Upwork.TaskService;

public class DataValidationException : UpworkException
{
    public IReadOnlyList<ValidationResult> Errors { get; protected set; }

    public DataValidationException(string message, params ValidationResult[] errors)
        : base(message)
    {
        Errors = new List<ValidationResult>(errors);
    }
}