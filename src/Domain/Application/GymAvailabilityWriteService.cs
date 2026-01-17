using Domain.Entities;
using Domain.Ports;

namespace Domain.Application;

public class GymAvailabilityWriteService(IUnitOfWorkFactory unitOfWorkFactory,
    IGymClientFactory gymClientFactory,
    TimeProvider timeProvider
    ) : IApplicationService
{
    public async Task RegisterCurrentAvailability(CancellationToken cancellationToken)
    {
        var unitOfWork = unitOfWorkFactory.Create();
        var now = timeProvider.GetUtcNow();
        
        var gyms = await unitOfWork.Gyms.GetGyms(cancellationToken);

        foreach (var gym in gyms)
        {
            var client = await gymClientFactory.Create(gym.ApiPrefix, gym.Username, gym.Password, cancellationToken);
            var current = await client.GetCurrentAvailability(cancellationToken);
            if (current.HasValue)
            {
                var item = new AvailabilityItem()
                {
                    AvailableSeats = current.Value,
                    Time = now,
                    GymId = gym.Id,
                };
                unitOfWork.Availability.AddAvailabilityItem(item);
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}