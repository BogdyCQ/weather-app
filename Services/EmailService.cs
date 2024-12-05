using SendGrid;
using SendGrid.Helpers.Mail;
using WeatherApp.Configs;

public class EmailService
{
    private readonly string _sendgridKey;
    private readonly WeatherLogsService _weatherLogsService;

    public EmailService(WeatherAppConfig config, WeatherLogsService weatherLogsService)
    {
        _sendgridKey = config.SendgridKey;
        _weatherLogsService = weatherLogsService;
    }

    public async Task SendEmail(WeatherData weatherData)
    {
        var client = new SendGridClient(_sendgridKey);

        var sender = "bogdancno1@gmail.com";
        var receiver = "bogdan.ciora@qubiz.com";

        var from = new EmailAddress("bogdancno1@gmail.com", "Weather Alert");
        var to = new EmailAddress("bogdan.ciora@qubiz.com");

        var subject = "Temperature alert from WeatherApp";
        var content = $"The weather in {weatherData.Name} is below the freezing point with a temperature of " +
            $"{weatherData.Main?.Temp}°C and a humidity of {weatherData.Main?.Humidity}%";
        var htmlContent = "<strong>The temperature has dropped below 0°C.</strong>";

        var msg = MailHelper.CreateSingleEmail(from, to, subject, content, htmlContent);
        var response = await client.SendEmailAsync(msg);

        if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
        {
            Console.WriteLine($"Failed to send email: {response.StatusCode}");
        }
        else
        {
            var emailLogs = new EmailLog
            {
                Sender = sender,
                Receiver = receiver,
                CityName = weatherData.Name ?? string.Empty,
                Temparature = weatherData.Main?.Temp ?? 0,
                Humidity = weatherData.Main?.Humidity ?? 0
            };

            await _weatherLogsService.LogEmailData(emailLogs);
        }
    }
}
