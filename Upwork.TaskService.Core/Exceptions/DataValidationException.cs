using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

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

    public override string ToString()
    {
        StringBuilder result = new StringBuilder()
            .Append(base.ToString());

        if (Errors.Any())
        {
            result.AppendLine("Errors: ");
            foreach (var error in Errors)
            {
                result.AppendLine(error.ToString());
            }
        }

        return result.ToString();
    }
}