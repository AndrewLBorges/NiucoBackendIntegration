using Newtonsoft.Json.Converters;
using NiucoBackendIntegration.Enums;
using System.Text.Json.Serialization;

namespace NiucoBackendIntegration.Entities;

public class User
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? LastActivity { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole? Role { get; set; }
    public bool IsPayer { get; set; }
    public bool IsActive { get; set; }
}
