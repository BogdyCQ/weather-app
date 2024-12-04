using Microsoft.AspNetCore.Mvc;

public class WeatherLogsController : Controller
{
    private readonly WeatherLogsService _weatherLogsService;

    public WeatherLogsController(WeatherLogsService weatherLogsService)
    {
        _weatherLogsService = weatherLogsService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GetWeatherLogs()
    {
        var weatherLogs = await _weatherLogsService.GetWeatherLogsAsync();
        return View("Index", weatherLogs);
    }
}
