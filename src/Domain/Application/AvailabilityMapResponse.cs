using Domain.Entities;

namespace Domain.Application;

public class AvailabilityHourResponse
{
    public DayOfWeek DayOfWeek { get; set; }
    
    public int Hour { get; set; }
    
    public double Seats { get; set; }
}