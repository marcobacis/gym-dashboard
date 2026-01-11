using Domain.Application;
using Quartz;

namespace WebApp.Jobs;

public class AvailabilityCollectionJob(GymAvailabilityService gymAvailabilityService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await gymAvailabilityService.RegisterCurrentAvailability(context.CancellationToken);
    }
}