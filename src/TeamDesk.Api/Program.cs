using Microsoft.EntityFrameworkCore;
using TeamDesk.Api.Data;
using TeamDesk.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    // Without this, ASP.NET Core treats every non-nullable string property as required
    // and rejects a POST that leaves out Status. The controller does its own validation.
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

string? connectionString = builder.Configuration.GetConnectionString("TeamDesk");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'TeamDesk' is not configured.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<RequestService>();

var app = builder.Build();

app.MapControllers();

app.Run();
