namespace Domain.Application.Dtos;

public class AvailabilityHeatMapItemResponse
{
    public DayOfWeek DayOfWeek { get; set; }
    
    public int Hour { get; set; }
    
    public double Seats { get; set; }
}