using System.Net.Http.Json;
using System.Text.Json.Serialization;
using GymDataFetcher.GymApi;
using Microsoft.Extensions.Options;

namespace GymClient;

public record AuthToken(string AccessToken, string? InstallationKey);

public sealed class GymApiTokenService
{
    private readonly HttpClient _http;
    private readonly GymClientOptions _options;
    private readonly TimeProvider _timeProvider;
    private LoginResponse? _current;
    private DateTime? _expiration;
    

    public GymApiTokenService(HttpClient http, IOptions<GymClientOptions> options, TimeProvider timeProvider)
    {
        _http = http;
        _http.BaseAddress = new Uri(options.Value.BaseUrl);
        _options = options.Value;
        _timeProvider = timeProvider;
    }

    public async Task<AuthToken?> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        if (_current != null && IsExpired(_current))
            await RefreshAsync(_current.RefreshToken, cancellationToken);

        if (_current is null)
            await AuthenticateAsync(_options.Username, _options.Password, cancellationToken);
        
        return _current is null ? null : new AuthToken(_current.AccessToken, _current.UserInfo.AppUserInstallations.FirstOrDefault()?.WebInstallationKey);
    }

    private async Task AuthenticateAsync(string email, string password, CancellationToken cancellationToken)
    {
        Console.WriteLine("Authenticating ...");
        var response = await _http.PostAsJsonAsync(
            "authenticate",
            new { email, password }, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        _current = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken);
        UpdateExpiration();
    }

    private async Task RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        Console.WriteLine("Refreshing ...");
        try
        {
            var response = await _http.PostAsJsonAsync(
                "token",
                new { refreshToken }, cancellationToken);

            response.EnsureSuccessStatusCode();
            _current = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken);
        }
        catch (Exception)
        {
            _current = null;
        }
        finally
        {
            UpdateExpiration();
        }
    }
    
    private void UpdateExpiration()
    {
        if(_current is null)
            _expiration = null;

        var now = _timeProvider.GetUtcNow().DateTime;
        _expiration = _current != null ? now.AddSeconds(_current.ExpiresIn) : null;
    }

    private bool IsExpired()
    {
        if (_expiration is null)
            return true;
        
        return _expiration <= _timeProvider.GetUtcNow().DateTime;
    }

    private sealed record LoginResponse(
        string AccessToken, 
        [property: JsonPropertyName("refreskToken")] 
        string RefreshToken, 
        int ExpiresIn, 
        UserInfo UserInfo
    );

    private sealed record UserInfo(List<UserInstallation> AppUserInstallations);

    private sealed record UserInstallation(string WebInstallationKey, string InstallationName);
}
