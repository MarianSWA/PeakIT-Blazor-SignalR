using System.Threading.Tasks;
using BlazorServerWeather.Data;
using Microsoft.AspNetCore.SignalR;

namespace BlazorServerWeather.Hubs
{
    public class WeatherHub : Hub
    {
        public async Task AddForecast(WeatherForecast forecast)
        {
            await Clients.All.SendAsync("ReceiveAddForecast", forecast);
        }
        
        public async Task DeleteForecast(int forecastId)
        {
            await Clients.All.SendAsync("ReceiveDeleteForecast", forecastId);
        }
    }
}