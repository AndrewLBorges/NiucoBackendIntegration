using NiucoBackendIntegration.Interfaces;

namespace NiucoBackendIntegration.Proxies;

public class UserProxy : IUserProxy
{
    private readonly ILogger<string> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly string _niucoApiBaseUrl;
    public UserProxy(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<string> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _niucoApiBaseUrl = _configuration.GetValue<string>("URLs:NiucoUsersBaseUrl");
        _logger = logger;
    }

    public async Task<string> GetUsersJsonAsync()
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("UserClient");

            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"{_niucoApiBaseUrl}/users");

            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            return response;

        } catch (Exception ex)
        {
            _logger.LogError($"Error while calling the Niuco SaaS Api. Exception message: {ex.Message}");
            return String.Empty;
        }
    }
}
