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

    public async Task RegisterCurrentAvailability(CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow().DateTime;
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