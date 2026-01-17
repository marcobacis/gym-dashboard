using Domain.Application.Dtos;
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
    
    public async Task<GymResponse?> GetGymById(Guid gymId, CancellationToken cancellationToken)
    {
        var unitOfWork = unitOfWorkFactory.Create();
        var gym = await unitOfWork.Gyms.GetGymById(gymId, cancellationToken);
        if (gym == null)
            return null;

        var availabilityItems = await unitOfWork.Availability.GetAvailabilityItems(gymId, cancellationToken);
        return new GymResponse()
        {
            Id = gym.Id,
            Name = gym.Name,
            MinAvailability = availabilityItems.Count > 0 ? availabilityItems.Select(i => i.AvailableSeats).Min() : 0,
            MaxAvailability = availabilityItems.Count > 0 ? availabilityItems.Select(i => i.AvailableSeats).Max() : 100,
        };
    }
    
    public async Task<int?> GetLatestAvailability(Guid gymId, CancellationToken cancellationToken)
    {
        var unitOfWork = unitOfWorkFactory.Create();
        var item = await unitOfWork.Availability.GetLatestAvailabilityItem(gymId, cancellationToken);
        return item?.AvailableSeats;
    }
    
    public async Task<List<AvailabilityHeatMapItemResponse>> GetAvailabilityByByHour(Guid gymId, CancellationToken cancellationToken)
    {
        var unitOfWork = unitOfWorkFactory.Create();
        var items = await unitOfWork.Availability.GetAvailabilityItems(gymId, cancellationToken);
        return items.GroupBy(a => new { a.Time.DayOfWeek, a.Time.Hour })
            .Select(group =>
            {
                var seats = group.Select(g => g.AvailableSeats).Average();
                return new AvailabilityHeatMapItemResponse()
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
        var originalItems = await unitOfWork.Availability.GetAvailabilityItems(gymId, cancellationToken);
        var items = originalItems;
        if (startTime.HasValue)
            items = items.Where(i => i.Time >= startTime.Value).ToList();
        
        if (endTime.HasValue)
            items = items.Where(i => i.Time <= endTime.Value).ToList();

        return items.Select(i => i.ToResponse()).ToList();
    }
}