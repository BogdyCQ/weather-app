using System.Text.Json;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";

    public WeatherService(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    public async Task<WeatherData?> GetWeatherAsync(string city)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}?q={city}&appid={_apiKey}&units=metric");
        if (!response.IsSuccessStatusCode)
            return null;

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<WeatherData>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}

public class WeatherData
{
    public Main? Main { get; set; }
    public string? Name { get; set; }
}

public class Main
{
    public float Temp { get; set; }
    public float Humidity { get; set; }
}
