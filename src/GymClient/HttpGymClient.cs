using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Domain.Ports;

namespace GymClient;

public class HttpGymClient : IGymClient
{
    private readonly HttpClient _httpClient;

    public HttpGymClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int?> GetCurrentAvailability(CancellationToken cancellationToken = default)
    {
        var zonesReponse = await _httpClient.GetFromJsonAsync<ZoneResponse>("zones", cancellationToken);
        return zonesReponse?.Zones.FirstOrDefault()?.AvailablePlaces;
    }
    
    public async Task<LoginResponse?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken)
    {
        var requestBody = new { email, password };
        
        var response = await _httpClient.PostAsJsonAsync(
            "authenticate",
            requestBody, cancellationToken);
        
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken);
    }

    public async Task<LoginResponse?> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
            var response = await _httpClient.PostAsJsonAsync(
                "token",
                new { refreshToken }, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return null;
            
            return await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken);
    }
    
    public sealed record LoginResponse(
        string AccessToken, 
        [property: JsonPropertyName("refreskToken")] 
        string RefreshToken, 
        int ExpiresIn, 
        UserInfo UserInfo
    );

    public sealed record UserInfo(List<UserInstallation> AppUserInstallations);

    public sealed record UserInstallation(string WebInstallationKey, string InstallationName);
    
    record ZoneResponse(List<Zone> Zones);
    record Zone(int ZoneId, string Name, int AvailablePlaces);
}