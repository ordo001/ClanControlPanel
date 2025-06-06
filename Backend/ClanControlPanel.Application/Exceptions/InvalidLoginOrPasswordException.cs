namespace ClanControlPanel.Application.Exceptions;

public class InvalidLoginOrPasswordException : Exception
{
    public InvalidLoginOrPasswordException() : base("Неверный логин или пароль") {}
}