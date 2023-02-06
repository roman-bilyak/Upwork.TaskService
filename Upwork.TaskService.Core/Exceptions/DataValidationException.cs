using System.ComponentModel.DataAnnotations;

namespace Upwork.TaskService;

public class DataValidationException : UpworkException
{
    public IReadOnlyList<ValidationResult> Errors { get; protected set; }

    public DataValidationException(params ValidationResult[] errors)
        : this("Input data is not valid! See ValidationErrors for more details.", errors)
    {

    }

    public DataValidationException(string message, params ValidationResult[] errors)
        : base(message)
    {
        Errors = new List<ValidationResult>(errors);
    }
}