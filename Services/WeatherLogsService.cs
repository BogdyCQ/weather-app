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

    public async Task<List<WeatherLogData>> GetWeatherLogsAsync()
    {
        var serviceClient = new TableServiceClient(_connectionString);
        var tableClient = serviceClient.GetTableClient("WeatherData");

        var logs = new List<WeatherLogData>();

        await foreach (var entity in tableClient.QueryAsync<TableEntity>())
        {
            logs.Add(new WeatherLogData 
            {
                DateTime = entity.Timestamp.Value.DateTime,
                CityName = entity.PartitionKey,
                Temparature = Convert.ToSingle(entity["Temperature"]),
                Humidity = Convert.ToSingle(entity["Humidity"]),
            });
        }

        return logs.OrderByDescending(l => l.DateTime).ToList();
    }
}

public class WeatherLogData
{
    public DateTime DateTime { get; set; }
    
    public string CityName { get; set; }

    public float Temparature { get; set; }

    public float Humidity { get; set; }
}
