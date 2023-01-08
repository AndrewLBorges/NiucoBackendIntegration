using Newtonsoft.Json;
using NiucoBackendIntegration.Enums;

namespace NiucoBackendIntegration.DTOs
{
    public class UserDTO
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("email")]
        public string? Email { get; set; }
        [JsonProperty("status")]
        public string? Status { get; set; }
        [JsonProperty("role")]
        public string? Role { get; set; }
        [JsonProperty("last_activity")]
        public long LastActivity { get; set; }
    }
}
