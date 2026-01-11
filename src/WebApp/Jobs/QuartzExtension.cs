using Quartz;

namespace WebApp.Jobs;

public static class QuartzExtension
{
    public static IServiceCollectionQuartzConfigurator AddJobWithCron<T>(
        this IServiceCollectionQuartzConfigurator quartzConfigurator,
        string cronExpression)
        where T : IJob
    {
        var jobName = nameof(T);

        quartzConfigurator.AddJob<T>(opt => opt.WithIdentity(jobName));

        quartzConfigurator.AddTrigger(opt => opt
            .ForJob(jobName)
            .WithIdentity($"{jobName}-trigger")
            .WithCronSchedule(cronExpression)
        );

        return quartzConfigurator;
    }
}