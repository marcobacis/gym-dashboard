using ApexCharts;
using Domain;
using GymClient;
using Persistence;
using Quartz;
using WebApp.Components;
using WebApp.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add configuration from environment variables
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddDomainModule();
builder.Services.AddPersistenceModule();
builder.Services.AddGymApiModule();

builder.Services.AddQuartz(quartz =>
{
    var availabilityCron = builder.Configuration.GetValue<string>("AvailabilityCollectionJob:CronExpression");
    quartz.AddJobWithCron<AvailabilityCollectionJob>(availabilityCron!);
});

builder.Services.AddQuartzHostedService(opt =>
{
    opt.WaitForJobsToComplete = true;
    opt.AwaitApplicationStarted = true;
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddApexCharts();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.Services.ApplyMigrations();
await app.RunAsync();