namespace Domain.Entities;

public class AvailabilityItem
{
    public DateTimeOffset Time { get; set; }
    
    public int AvailableSeats { get; set; }
}