namespace Domain.Entities;

public class AvailabilityItem
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    
    public Guid GymId { get; set; }
    
    public Gym Gym { get; set; } = null!;
    
    public DateTimeOffset Time { get; set; }
    
    public int AvailableSeats { get; set; }
}