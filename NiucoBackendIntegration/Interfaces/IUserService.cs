using NiucoBackendIntegration.Entities;

namespace NiucoBackendIntegration.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsersAsync();
}
