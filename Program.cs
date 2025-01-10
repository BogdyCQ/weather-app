using WeatherApp.Configs;
using WeatherApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddSingleton<WeatherLogsService>();
builder.Services.AddSingleton<EmailService>();

var keyVaultUri = new Uri("https://weather-vault-53.vault.azure.net/");
var keyVaultService = new KeyVaultService("https://weather-vault-53.vault.azure.net/");

string apiKey = await keyVaultService.GetSecretAsync("openweather-api-key");
string connectionString = await keyVaultService.GetSecretAsync("storage-connection-string");
string sendgridKey = await keyVaultService.GetSecretAsync("sendgrid-key");

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
