using Domain.Entities;
using Domain.Ports;

namespace Domain.Application;

public class GymAvailabilityQueryService(IUnitOfWorkFactory unitOfWorkFactory) : IApplicationService
{
    public async Task<List<Gym>> GetGyms(CancellationToken cancellationToken)
    {
        var unitOfWork = unitOfWorkFactory.Create();
        return await unitOfWork.Gyms.GetGyms(cancellationToken);
    }
    
    public async Task<int?> GetLatestAvailability(Guid gymId, CancellationToken cancellationToken)
    {
        var unitOfWork = unitOfWorkFactory.Create();
        var item = await unitOfWork.Availability.GetLatestAvailabilityItem(gymId, cancellationToken);
        return item?.AvailableSeats;
    }
    
    public async Task<List<AvailabilityHourResponse>> GetAvailabilityByByHour(Guid gymId, CancellationToken cancellationToken)
    {
        var unitOfWork = unitOfWorkFactory.Create();
        var items = await unitOfWork.Availability.GetAvailabilityItems(gymId, cancellationToken);
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

    public async Task<List<AvailabilityItemResponse>> GetAvailabilityItems(Guid gymId, DateTime? startTime, DateTime? endTime,
        CancellationToken cancellationToken)
    {
        var unitOfWork = unitOfWorkFactory.Create();
        var items = await unitOfWork.Availability.GetAvailabilityItems(gymId,cancellationToken);
        if (startTime.HasValue)
            items = items.Where(i => i.Time >= startTime.Value).ToList();
        
        if (endTime.HasValue)
            items = items.Where(i => i.Time <= endTime.Value).ToList();
        
        return items.Select(i => i.ToResponse()).ToList();
    }
}