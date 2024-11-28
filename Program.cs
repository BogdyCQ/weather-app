using Azure.Identity;
using Azure.Security.KeyVault.Secrets;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddSingleton<WeatherService>();

var keyVaultUri = new Uri("https://weather-app-keyvault-2.vault.azure.net/");

var client = new SecretClient(vaultUri: keyVaultUri, credential: new DefaultAzureCredential());

var apiKeySecret = await client.GetSecretAsync("openweather-api-key");
string apiKey = apiKeySecret.Value.Value ?? string.Empty;

builder.Services.AddSingleton(apiKey);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Weather}/{action=Index}/{id?}");

app.Run();
