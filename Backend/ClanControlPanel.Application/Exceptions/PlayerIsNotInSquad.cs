namespace ClanControlPanel.Application.Exceptions;

public class PlayerIsNotInSquad<T> : Exception
    where T : class
{
    public PlayerIsNotInSquad(Guid id) : base($"{typeof(T).Name} с индентификатором {id} не состоит в отряде") {}
}