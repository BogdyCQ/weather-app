using Azure.Data.Tables;
using System.Text.Json;
using WeatherApp.Configs;

public class WeatherLogsService
{
    private readonly string _connectionString;

    public WeatherLogsService(HttpClient httpClient, WeatherAppConfig config)
    {
        _connectionString = config.StorageConnectionString;
    }

    public async Task LogWeatherData(WeatherData weatherData)
    {
        var serviceClient = new TableServiceClient(_connectionString);
        var tableClient = serviceClient.GetTableClient("WeatherData");

        var weatherEntity = new TableEntity(weatherData.Name, Guid.NewGuid().ToString())
        {
            {"Temperature", weatherData.Main?.Temp},
            {"Humidity", weatherData.Main?.Humidity}
        };

        await tableClient.AddEntityAsync(weatherEntity);
    }

    public async Task<WeatherLogData> GetWeatherLogsAsync()
    {
        return new WeatherLogData();
    }
}

public class WeatherLogData
{
    public string DateTime { get; set; }
    
    public string CityName { get; set; }

    public float Temparature { get; set; }

    public float Humidity { get; set; }
}
