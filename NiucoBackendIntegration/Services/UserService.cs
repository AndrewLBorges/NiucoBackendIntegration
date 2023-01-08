using Newtonsoft.Json;
using NiucoBackendIntegration.DTOs;
using NiucoBackendIntegration.Entities;
using NiucoBackendIntegration.Enums;
using NiucoBackendIntegration.Interfaces;

namespace NiucoBackendIntegration.Services;

public class UserService : IUserService
{
    private readonly IUserProxy _userProxy;

    public UserService(IUserProxy userProxy)
    {
        _userProxy = userProxy;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var usersJson = await _userProxy.GetUsersJsonAsync();

        if(string.IsNullOrEmpty(usersJson))
        {
            throw new ArgumentNullException(nameof(usersJson));
        }

        var usersDto = JsonConvert.DeserializeObject<List<UserDTO>>(usersJson);

        var users = GetUsersFromDTO(usersDto!);

        return users;        
    }

    private static IEnumerable<User> GetUsersFromDTO(IEnumerable<UserDTO> dtoUsers) 
    {
        List<User> users = new();

        dtoUsers.ToList().ForEach(userDto =>
        {
            var newUser = new User()
            {
                Id = userDto.Id,
                Name = userDto.Name,
                Email = GetObfuscatedEmail(userDto.Email),
                LastActivity = FormatDateFromUnixEpochToISO8601(userDto.LastActivity),
                Role = GetUserRole(userDto.Role),
                IsActive = userDto.Status == "enabled" ? true : false,
                IsPayer = false
            };
            newUser.IsPayer = DetermineUserPayer(newUser.Role, newUser.IsActive);
            users.Add(newUser);
        });

        return users;
    }

    private static string GetObfuscatedEmail(string? email)
    {
        var emailResult = email ?? string.Empty;
        if(!string.IsNullOrEmpty(email))
        {
            var splittedEmail = email.Split('@');
            var alias = splittedEmail[0];
            var domain = splittedEmail[1];

            var asterisks = new String('*', alias.Length - 4);

            if(domain != "niuco.com.br")
            {
                emailResult = alias[..2] + asterisks + alias[^2..] + "@" + domain;
            }
        }
        return emailResult;
    }

    private static string FormatDateFromUnixEpochToISO8601(long unixTime)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
        var dateTime = epoch.AddSeconds(unixTime);

        return dateTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
    }

    private static UserRole? GetUserRole(string? role)
    {
        return role switch
        {
            "admin" => UserRole.Admin,
            "editor" => UserRole.Editor,
            "system" => UserRole.System,
            "viewer" => UserRole.Viewer,
            _ => null
        };
    }

    private static bool DetermineUserPayer(UserRole? role, bool isActive)
    {
        if (!isActive)
            return false;

        return role switch
        {
            UserRole.Admin => true,
            UserRole.Editor => true,
            UserRole.System => false,
            UserRole.Viewer => false,
            _ => false
        };
    }
}
