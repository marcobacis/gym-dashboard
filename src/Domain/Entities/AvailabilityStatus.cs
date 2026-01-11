namespace Domain.Entities;

public enum AvailabilityStatus
{
    Worse,
    Bad,
    Neutral,
    Good,
    Great,
}

public static class AvailabilityStatusExtensions
{
    public static AvailabilityStatus ToAvailabilityStatus(this int availableSeats)
    {
        return availableSeats switch
        {
            <= 30 => AvailabilityStatus.Worse,
            <= 60 => AvailabilityStatus.Bad,
            <= 80 => AvailabilityStatus.Neutral,
            <= 105 => AvailabilityStatus.Good,
            _ => AvailabilityStatus.Great,
        };
    }
}