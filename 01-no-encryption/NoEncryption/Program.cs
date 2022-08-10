using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using NoEncryption.Data;
using NoEncryption.Jobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<BankDbContext>(options => options.UseInMemoryDatabase("InMemory"));

builder.Services
    .AddHangfire(options => options.UseMemoryStorage())
    .AddHangfireServer();

builder.Services.AddControllers();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHangfireDashboard();
});

app.MapRazorPages();

RecurringJob.AddOrUpdate<ProcessBanks>("process-banks",
    service => service.Run(CancellationToken.None), Cron.Minutely, TimeZoneInfo.Utc);

app.Run();
