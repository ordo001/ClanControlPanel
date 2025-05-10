namespace ClanControlPanel.Application.Exceptions;

public class EntityIsExists<T> : Exception
    where T : class
{
    public EntityIsExists(object property) : base($"{typeof(T).Name} с таким {property} уже существует") {}
}