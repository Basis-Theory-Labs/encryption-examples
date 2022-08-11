using Azure.Core;
using CustomerPortal.Data;
using CustomerPortal.Encryption;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLazyCache();

var tokenCredential = new LocalTokenCredential();

builder.Services.AddSingleton<TokenCredential>(tokenCredential);

builder.Services.AddAzureClients(b =>
{
    var keyVaultUri = new Uri(builder.Configuration.GetValue<string>("Encryption:ProviderUri"));
    b.AddKeyClient(keyVaultUri);
    b.UseCredential(tokenCredential);
});

builder.Services.AddSingleton<EncryptionService>();
builder.Services.AddScoped<DatabaseMigrator>();

builder.Services.AddRazorPages();
builder.Services.AddDbContext<BankDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Banks")));

builder.Services.AddControllers();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Error");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

using var scope = app.Services.CreateScope();

var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
await dbContext.Migrate();

app.Run();
