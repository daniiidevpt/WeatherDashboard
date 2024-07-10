using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;
using WeatherDashboard.Models;
using Microsoft.Extensions.Logging;

namespace WeatherDashboard.Services
{
    public interface IWeatherService
    {
        Task<WeatherInfo?> GetWeatherAsync(string city);
    }


    public class WeatherService : IWeatherService
    {
        private readonly string _apiKey;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(IConfiguration configuration, ILogger<WeatherService> logger)
        {
            _apiKey = configuration["OpenWeatherMap:ApiKey"] ?? throw new ArgumentNullException(nameof(_apiKey), "API key cannot be null");
            _logger = logger;
        }

        public async Task<WeatherInfo?> GetWeatherAsync(string city)
        {
            try
            {
                _logger.LogInformation("Starting API call to fetch weather data for {City}", city);
                var client = new RestClient($"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={_apiKey}");
                var request = new RestRequest();
                request.Method = Method.Get;

                var response = await client.ExecuteAsync<WeatherResponse>(request);

                if (response.IsSuccessful && response.Data != null)
                {
                    var weather = response.Data;
                    _logger.LogInformation("Successfully fetched weather data for {City}: {Data}", city, response.Content);

                    return new WeatherInfo
                    {
                        City = weather.Name ?? string.Empty,
                        Description = weather.Weather?[0]?.Description ?? string.Empty,
                        Temperature = weather.Main?.Temp ?? 0,
                        Humidity = weather.Main?.Humidity ?? 0,
                        WindSpeed = weather.Wind?.Speed ?? 0
                    };
                }

                _logger.LogError("Failed to get weather data: {StatusCode} - {ErrorMessage}", response.StatusCode, response.ErrorMessage);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occurred while fetching weather data for {City}: {Message}", city, ex.Message);
                return null;
            }
        }

    }

    // Model for the weather response
    public class WeatherResponse
    {
        public string? Name { get; set; }
        public Weather[]? Weather { get; set; }
        public Main? Main { get; set; }
        public Wind? Wind { get; set; }
    }

    // Model for the weather details
    public class Weather
    {
        public string? Description { get; set; }
    }

    // Model for the main weather data
    public class Main
    {
        public double Temp { get; set; }
        public int Humidity { get; set; }
    }

    // Model for the wind data
    public class Wind
    {
        public double Speed { get; set; }
    }
}
