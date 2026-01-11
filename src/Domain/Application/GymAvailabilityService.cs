using Domain.Entities;
using Domain.Ports;

namespace Domain.Application;

public class GymAvailabilityService(
    IGymAvailabilityRepository repository,
    IUnitOfWork unitOfWork,
    IGymClient gymClient,
    TimeProvider timeProvider) : IApplicationService
{
    public async Task<int?> GetCurrentAvailability(CancellationToken cancellationToken)
    {
        return await gymClient.GetCurrentAvailability(cancellationToken);
    }
    
    public async Task<List<AvailabilityHourResponse>> GetAvailabilityMap(CancellationToken cancellationToken)
    {
        var items = await repository.GetAvailabilityItems(cancellationToken);
        return items.GroupBy(a => new { a.Time.DayOfWeek, a.Time.Hour })
            .Select(group =>
            {
                var seats = group.Select(g => g.AvailableSeats).Average();
                return new AvailabilityHourResponse()
                {
                    DayOfWeek = group.Key.DayOfWeek,
                    Hour = group.Key.Hour,
                    Seats = seats,
                };
            })
            .ToList();
    }

    public async Task RegisterCurrentAvailability(CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();
        var current = await gymClient.GetCurrentAvailability(cancellationToken);

        if (current.HasValue)
        {
            var item = new AvailabilityItem()
            {
                AvailableSeats = current.Value,
                Time = now
            };
            repository.AddAvailabilityItem(item);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}