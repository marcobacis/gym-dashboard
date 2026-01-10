namespace GymDataFetcher.GymApi;

public class GymClientOptions
{
    public static string SectionName = "GymClient";
    
    public required string BaseUrl { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
}