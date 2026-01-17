namespace Domain.Application.Dtos;

public class GymResponse
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public int MinAvailability { get; set; }
    
    public int MaxAvailability { get; set; }
}