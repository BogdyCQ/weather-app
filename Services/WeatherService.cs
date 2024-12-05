using System.Text.Json;
using WeatherApp.Configs;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly WeatherLogsService _weatherLogsService;
    private readonly string _apiKey;
    private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";

    public WeatherService(HttpClient httpClient, WeatherLogsService weatherLogsService, WeatherAppConfig config)
    {
        _httpClient = httpClient;
        _weatherLogsService = weatherLogsService;
        _apiKey = config.ApiKey;

    }

    public async Task<WeatherData?> GetWeatherAsync(string city)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}?q={city}&appid={_apiKey}&units=metric");
        if (!response.IsSuccessStatusCode)
            return new WeatherData { IsFormSubmitted = true };

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var weatherData = JsonSerializer.Deserialize<WeatherData>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        weatherData.IsFormSubmitted = true;

        if (weatherData != null)
            await _weatherLogsService.LogWeatherData(weatherData);

        return weatherData;
    }
}

public class WeatherData
{
    public Main? Main { get; set; }

    public string? Name { get; set; }

    public bool IsFormSubmitted { get; set; }
}

public class Main
{
    public float Temp { get; set; }

    public float Humidity { get; set; }
}
