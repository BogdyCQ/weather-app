using System.Text.Json;

public class WeatherLogsService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";

    public WeatherLogsService(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
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
