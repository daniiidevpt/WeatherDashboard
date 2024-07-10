using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeatherDashboard.Models;
using WeatherDashboard.Services;
using Microsoft.Extensions.Logging;

namespace WeatherDashboard.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        // Display the main page with city selection buttons
        public IActionResult Index()
        {
            return View();
        }

        // Fetch weather data for the selected city
        [HttpGet]
        public async Task<IActionResult> GetWeather(string city)
        {
            try
            {
                _logger.LogInformation("Fetching weather data for {City}", city);
                var weather = await _weatherService.GetWeatherAsync(city);
                if (weather != null)
                {
                    return View("WeatherInfo", weather);
                }

                _logger.LogError("Weather data for {City} not found or API call failed.", city);
                return View("Error");
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occurred while fetching weather data for {City}: {Message}", city, ex.Message);
                return View("Error");
            }
        }

    }
}

