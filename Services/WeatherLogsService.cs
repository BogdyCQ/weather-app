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

    public async Task LogEmailData(EmailLog emailLog)
    {
        var serviceClient = new TableServiceClient(_connectionString);
        var tableClient = serviceClient.GetTableClient("EmailLogs");

        var now = DateTime.Now;
        var today = $"{now.Day}{now.Month}{now.Year}";

        var emailLogEntity = new TableEntity(today, Guid.NewGuid().ToString())
        {
            {"Sender", emailLog.Sender },
            {"Receiver", emailLog.Receiver },
            {"CityName", emailLog.CityName },
            {"Temperature", emailLog.Temparature },
            {"Humidity", emailLog.Humidity }
        };

        await tableClient.AddEntityAsync(emailLogEntity);
    }

    public async Task<List<EmailLog>> GetEmailLogsAsync()
    {
        var serviceClient = new TableServiceClient(_connectionString);
        var tableClient = serviceClient.GetTableClient("EmailLogs");

        var logs = new List<EmailLog>();

        await foreach (var entity in tableClient.QueryAsync<TableEntity>())
        {
            logs.Add(new EmailLog
            {
                Timestamp = entity.Timestamp.Value.DateTime,
                Sender = Convert.ToString(entity["Sender"]) ?? string.Empty,
                Receiver = Convert.ToString(entity["Receiver"]) ?? string.Empty,
                CityName = Convert.ToString(entity["CityName"]) ?? string.Empty,
                Temparature = Convert.ToSingle(entity["Temperature"]),
                Humidity = Convert.ToSingle(entity["Humidity"]),
            });
        }

        return logs.OrderByDescending(l => l.Timestamp).ToList();
    }
}

public class WeatherLogData
{
    public DateTime DateTime { get; set; }
    
    public required string CityName { get; set; }

    public float Temparature { get; set; }

    public float Humidity { get; set; }
}

public class EmailLog
{
    public DateTime Timestamp { get; set; }

    public required string Sender { get; set; }

    public required string Receiver { get; set; }

    public required string CityName { get; set; }

    public float Temparature { get; set; }

    public float Humidity { get; set; }
}
