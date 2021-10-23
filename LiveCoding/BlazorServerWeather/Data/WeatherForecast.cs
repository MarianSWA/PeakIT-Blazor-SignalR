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

        [Required] 
        [Range(-40, 50)] 
        public int TemperatureC { get; set; } = 20;

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [Required]
        [MinLength(2)]
        [MaxLength(200)]
        public string Summary { get; set; }

        public bool NeedsUmbrella { get; set; }
    }
}
