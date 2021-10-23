using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorServerWeather.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerWeather.Pages
{
    public partial class WeatherForecasts : IAsyncDisposable
    {
        [Inject]
        private ApplicationDbContext Context { get; set; }

        [Inject] 
        private NavigationManager NavigationManager { get; set; }
        
        private List<WeatherForecast> forecasts;

        private void AddForecast()
        {
            NavigationManager.NavigateTo("/weather/add");
        }
        
        private HubConnection hubConnection;
        
        protected override async Task OnInitializedAsync()
        {
            forecasts = await Context.WeatherForecasts.ToListAsync();
            
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/weatherhub"))
                .Build();
            
            hubConnection.On("ReceiveAddForecast", (WeatherForecast forecast) =>
            {
                Console.WriteLine("Received command to add: " + forecast.Id);
                forecasts.Add(forecast);
                StateHasChanged();
            });

            hubConnection.On("ReceiveDeleteForecast", (int forecastId) =>
            {
                Console.WriteLine("Received command to delete: " + forecastId);
                WeatherForecast forecastToRemove = forecasts.FirstOrDefault(i => i.Id == forecastId);
                forecasts.Remove(forecastToRemove);
                StateHasChanged();
            });
            
            hubConnection.On("ReceiveEditForecast", (WeatherForecast forecast) =>
            {
                Console.WriteLine("Received command to edit: " + forecast.Id);
                WeatherForecast forecastToEdit = forecasts.FirstOrDefault(i => i.Id == forecast.Id);
                forecastToEdit.CopyFrom(forecast);
                StateHasChanged();
            });
            
            await hubConnection.StartAsync();
        }

        private async void DeleteForecast(WeatherForecast forecast)
        {
            Context.WeatherForecasts.Remove(forecast);
            int changes = Context.SaveChanges();
            
            if (changes > 0)
            {
                forecasts.Remove(forecast);
                await hubConnection.SendAsync("DeleteForecast", forecast.Id);
            }
        }

        private void UpdateForecast(WeatherForecast forecast)
        {
            NavigationManager.NavigateTo($"/weather/update/{forecast.Id}");
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