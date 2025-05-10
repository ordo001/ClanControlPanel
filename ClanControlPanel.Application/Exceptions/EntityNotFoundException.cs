namespace ClanControlPanel.Application.Exceptions;

public class EntityNotFoundException<T> : Exception
    where T : class
{
    public EntityNotFoundException(Guid id) : base($"{typeof(T).Name} с идентификатором {id} не найден") {}
    public EntityNotFoundException(string name) : base($"{typeof(T).Name} с именем {name} не найден") {}
}