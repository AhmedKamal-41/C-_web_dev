using TeamDesk.Web.Components;
using TeamDesk.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

string? apiBaseAddress = builder.Configuration["TeamDeskApi:BaseAddress"];
if (string.IsNullOrWhiteSpace(apiBaseAddress))
{
    throw new InvalidOperationException("Setting 'TeamDeskApi:BaseAddress' is not configured.");
}

builder.Services.AddHttpClient<RequestApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
});

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

app.Run();
