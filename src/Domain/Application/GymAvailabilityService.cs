using Domain.Entities;
using Domain.Ports;

namespace Domain.Application;

public class GymAvailabilityService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IGymClient gymClient,
    TimeProvider timeProvider) : IApplicationService
{
    public async Task<int?> GetLatestAvailability(CancellationToken cancellationToken)
    {
        var unitOfWork = unitOfWorkFactory.Create();
        var item = await unitOfWork.Availability.GetLatestAvailabilityItem(cancellationToken);
        return item?.AvailableSeats;
    }
    
    public async Task<List<AvailabilityHourResponse>> GetAvailabilityByByHour(CancellationToken cancellationToken)
    {
        var unitOfWork = unitOfWorkFactory.Create();
        var items = await unitOfWork.Availability.GetAvailabilityItems(cancellationToken);
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

    public async Task<List<AvailabilityItemResponse>> GetAvailabilityItems(DateTime? startDate, DateTime? endDate,
        CancellationToken cancellationToken)
    {
        var unitOfWork = unitOfWorkFactory.Create();
        var items = await unitOfWork.Availability.GetAvailabilityItems(cancellationToken);
        if (startDate.HasValue)
            items = items.Where(i => i.Time.Date >= startDate.Value.Date).ToList();
        
        if (endDate.HasValue)
            items = items.Where(i => i.Time.Date <= endDate.Value.Date).ToList();
        
        return items.Select(i => i.ToResponse()).ToList();
    }

    public async Task RegisterCurrentAvailability(CancellationToken cancellationToken)
    {
        var unitOfWork = unitOfWorkFactory.Create();
        var now = timeProvider.GetUtcNow();
        var current = await gymClient.GetCurrentAvailability(cancellationToken);

        if (current.HasValue)
        {
            var item = new AvailabilityItem()
            {
                AvailableSeats = current.Value,
                Time = now
            };
            unitOfWork.Availability.AddAvailabilityItem(item);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}