using System.ComponentModel.DataAnnotations;
using System.Net;

public static class RequestValidator
{
    public static ValidationResult Validate(object model)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model);

        if (Validator.TryValidateObject(model, validationContext, validationResults, true))
        {
            return ValidationResult.Success;
        }

        throw new HttpException(HttpStatusCode.BadRequest, string.Join("\n", validationResults.Select(vr => vr.ErrorMessage)));
    }
}