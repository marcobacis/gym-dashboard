namespace Domain.Application.Dtos;

public class AvailabilityItemResponse
{
    public DateTimeOffset Time { get; set; }
    
    public int? AvailableSeats { get; set; }
}

public static class AvailabilityItemResponseExtensions
{
    public static AvailabilityItemResponse ToResponse(this Entities.AvailabilityItem item)
    {
        return new AvailabilityItemResponse()
        {
            Time = item.Time,
            AvailableSeats = item.AvailableSeats
        };
    }
}