using System.ComponentModel.DataAnnotations;
namespace ClanControlPanel.Core.Interfaces.Services;

public interface IValidatorService
{
    public List<ValidationResult> ValidationEntity<T>(T entity);
}