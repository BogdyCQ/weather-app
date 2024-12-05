namespace WeatherApp.Configs
{
    public class WeatherAppConfig
    {
        public string ApiKey { get; set; } = string.Empty;

        public string StorageConnectionString { get; set; } = string.Empty;

        public string SendgridKey { get; set; } = string.Empty;
    }
}
