using BankProcessor;
using BankProcessor.Jobs;
using Common;
using Common.Data;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .RegisterCommon(builder.Configuration)
    .AddHangfire(options => options.UseSqlServerStorage(builder.Configuration.GetConnectionString("Banks")))
    .AddHangfireServer()
    .AddControllers();

var app = builder.Build();

app.UseStaticFiles()
    .UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHangfireDashboard(new DashboardOptions
        {
            Authorization = new [] { new HangFireAuthorizationFilter() }
        });
    });

app.MapGet("/", () => "Hello World!");

using var scope = app.Services.CreateScope();

var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
dbContext.Migrate();

RecurringJob.AddOrUpdate<ProcessBanks>("process-banks",
    service => service.Run(CancellationToken.None), Cron.Minutely, TimeZoneInfo.Utc);

app.Run();
