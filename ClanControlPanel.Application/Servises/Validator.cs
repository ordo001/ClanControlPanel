using System.ComponentModel.DataAnnotations;
using ClanControlPanel.Core.Interfaces.Services;

namespace ClanControlPanel.Application.Servises;

public class ValidatorService : IValidatorService
{
    public List<ValidationResult> ValidationEntity<T>(T entity)
    {
        var context = new ValidationContext(entity);
        var result = new List<ValidationResult>();
        if (!Validator.TryValidateObject(entity, context, result, true))
        {
            return result!;
        }

        return result!;
    }
}