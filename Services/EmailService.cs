using SendGrid;
using SendGrid.Helpers.Mail;
using WeatherApp.Configs;

public class EmailService
{
    private readonly string _sendgridKey;

    public EmailService(HttpClient httpClient, WeatherAppConfig config)
    {
        _sendgridKey = config.SendgridKey;
    }

    public async Task SendEmail(WeatherData weatherData)
    {
        var client = new SendGridClient(_sendgridKey);

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
    }
}
