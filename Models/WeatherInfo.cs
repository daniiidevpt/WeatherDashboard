namespace WeatherDashboard.Models
{
    public class WeatherInfo
    {
        public string City { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
    }
}

