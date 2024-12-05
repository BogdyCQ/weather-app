using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using WeatherApp.Configs;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddSingleton<WeatherLogsService>();
builder.Services.AddSingleton<WeatherService>();
builder.Services.AddSingleton<EmailService>();

var keyVaultUri = new Uri("https://weather-vault-53.vault.azure.net/");

var client = new SecretClient(vaultUri: keyVaultUri, credential: new DefaultAzureCredential());

var apiKeySecret = await client.GetSecretAsync("openweather-api-key");
string apiKey = apiKeySecret.Value.Value ?? string.Empty;

var storageConnectionString = await client.GetSecretAsync("storage-connection-string");
string connectionString = storageConnectionString.Value.Value ?? string.Empty;

var sendgridApiKey = await client.GetSecretAsync("sendgrid-key");
string sendgridKey = sendgridApiKey.Value.Value ?? string.Empty;

var config = new WeatherAppConfig
{
    ApiKey = apiKey,
    StorageConnectionString = connectionString,
    SendgridKey = sendgridKey
};

builder.Services.AddSingleton(config);

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
