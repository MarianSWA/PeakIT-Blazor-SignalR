using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorServerWeather.Data
{
    public class WeatherForecast
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [Range(-50, 50)] 
        [Required] 
        public int TemperatureC { get; set; } = 20;

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Summary { get; set; }

        public bool NeedsUmbrella { get; set; }

        public void CopyFrom(WeatherForecast forecast)
        {
            Date = forecast.Date;
            Summary = forecast.Summary;
            NeedsUmbrella = forecast.NeedsUmbrella;
            TemperatureC = forecast.TemperatureC;
        }
    }
}
