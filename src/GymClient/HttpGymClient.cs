using System.Net.Http.Json;
using Domain.Ports;

namespace GymClient;

public class HttpGymClient(HttpClient httpClient) : IGymClient
{
    public async Task<int?> GetCurrentAvailability()
    {
        var zonesReponse = await httpClient.GetFromJsonAsync<ZoneResponse>("zones");
        return zonesReponse?.Zones.FirstOrDefault()?.AvailablePlaces;
    }
    
    record ZoneResponse(List<Zone> Zones);
    record Zone(int ZoneId, string Name, int AvailablePlaces);
}