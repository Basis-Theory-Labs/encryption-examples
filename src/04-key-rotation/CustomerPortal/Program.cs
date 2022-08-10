using Common;
using Common.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .RegisterCommon(builder.Configuration)
    .AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Error");

app.UseStaticFiles()
    .UseRouting()
    .UseAuthorization();

app.MapRazorPages();

using var scope = app.Services.CreateScope();

var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
dbContext.Migrate();

app.Run();
