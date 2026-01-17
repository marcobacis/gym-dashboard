using System.Net.Http.Headers;
using Domain.Ports;
using Microsoft.Extensions.Options;

namespace GymClient;

public record AuthToken(string AccessToken, string? InstallationKey);

public sealed class GymClientFactory(
    IHttpClientFactory httpClientFactory,
    IOptions<GymClientOptions> options,
    TimeProvider timeProvider)
    : IGymClientFactory
{
    private readonly GymClientOptions _options = options.Value;
    private GymClientOptions _gymClientOptions = options.Value;
    private readonly Dictionary<(string, string), LoginInfo> _loginCache = new();

    public async Task<IGymClient> Create(string apiPrefix, string username, string password, CancellationToken cancellationToken)
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri($"{_gymClientOptions.BaseUrl.TrimEnd('/')}/{apiPrefix}/");
        
        var gymClient = new HttpGymClient(httpClient);

        LoginInfo? current = _loginCache.TryGetValue((username, apiPrefix), out var curr) ? curr : null;
    
        if (current != null && current.Value.IsExpired(timeProvider.GetUtcNow().DateTime))
            current = LoginInfo.FromResponse(await gymClient.RefreshAsync(current.Value.RefreshToken, cancellationToken), timeProvider);

        if (current is null)
            current = LoginInfo.FromResponse(await gymClient.AuthenticateAsync(username, password, cancellationToken), timeProvider);

        if(current is null)
            throw new InvalidOperationException("Unable to authenticate to gym API");
        
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", current.Value.AccessToken);
        httpClient.DefaultRequestHeaders.Add("installationkey", current.Value.InstallationKey);
        
        return gymClient;
    }
    

    private struct LoginInfo
    {
        public string AccessToken { get; set; }
        
        public string RefreshToken { get; set; }
        
        public string InstallationKey { get; set; }
        
        public DateTime Expiration { get; set; }
        
        public bool IsExpired(DateTime now) => now >= Expiration;
        
        public static LoginInfo? FromResponse(HttpGymClient.LoginResponse? response, TimeProvider timeProvider)
        {
            if (response is null)
                return null;
            
            return new LoginInfo
            {
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken,
                InstallationKey = response.UserInfo.AppUserInstallations.FirstOrDefault()?.WebInstallationKey ?? string.Empty,
                Expiration = timeProvider.GetUtcNow().DateTime.AddSeconds(response.ExpiresIn - 60)
            };
        }
    }
    
    
}
