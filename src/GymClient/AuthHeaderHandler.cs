using System.Net.Http.Headers;
using GymClient;

namespace GymDataFetcher.GymApi;

public sealed class AuthHeaderHandler(GymApiTokenService gymApiTokenService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await gymApiTokenService.GetAccessTokenAsync(cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        request.Headers.Add("installationkey", token.InstallationKey);
        return await base.SendAsync(request, cancellationToken);
    }
}