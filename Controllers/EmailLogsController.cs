using Microsoft.AspNetCore.Mvc;

public class EmailLogsController : Controller
{
    private readonly WeatherLogsService _weatherLogsService;

    public EmailLogsController(WeatherLogsService weatherLogsService)
    {
        _weatherLogsService = weatherLogsService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GetEmailLogs()
    {
        var weatherLogs = await _weatherLogsService.GetEmailLogsAsync();
        return View("Index", weatherLogs);
    }
}
