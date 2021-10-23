using System;
using System.Threading.Tasks;
using BlazorServerWeather.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorServerWeather.Pages
{
    public partial class AddForecast : IAsyncDisposable
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ApplicationDbContext Context { get; set; }
        
        private WeatherForecast Forecast { get; set; } = new();

        private HubConnection hubConnection;
        
        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/weatherhub"))
                .Build();
            
            await hubConnection.StartAsync();
        }
        
        private void CreateForecast()
        {
            Context.WeatherForecasts.Add(Forecast);
            Context.SaveChanges();

            hubConnection.SendAsync("AddForecast", Forecast);
            
            NavigationManager.NavigateTo("/weather");
        }
        
        public async ValueTask DisposeAsync()
        {
            if (hubConnection is null)
            {
                return;
            }

            await hubConnection.DisposeAsync();
        }
    }
}