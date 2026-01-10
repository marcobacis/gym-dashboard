namespace Persistence;

public class AppDbContextOptions
{
    public static readonly string SectionName = "ConnectionStrings";
    
    public required string Database { get; set; }
}