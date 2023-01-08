namespace NiucoBackendIntegration.Interfaces;

public interface IUserProxy
{
    Task<string> GetUsersJsonAsync();
}
