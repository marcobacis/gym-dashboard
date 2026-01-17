using Domain.Application;
using Quartz;

namespace WebApp.Jobs;

public class AvailabilityCollectionJob(GymAvailabilityWriteService gymAvailabilityWriteService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await gymAvailabilityWriteService.RegisterCurrentAvailability(context.CancellationToken);
    }
}