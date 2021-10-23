using System;
using System.Threading.Tasks;
using BlazorServerWeather.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorServerWeather.Pages
{
    public partial class EditForecast : IAsyncDisposable
    {
        [Parameter]
        public int Id { get; set; }
        
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ApplicationDbContext Context { get; set; }
        
        private WeatherForecast Forecast { get; set; } = new();

        protected override void OnParametersSet()
        {
            Forecast = Context.WeatherForecasts.Find(Id);
        }
        
        private HubConnection hubConnection;
        
        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/weatherhub"))
                .Build();
            
            await hubConnection.StartAsync();
        }
        
        private async void CreateForecast()
        {
            Context.WeatherForecasts.Update(Forecast);
            
            await Context.SaveChangesAsync();
            await hubConnection.SendAsync("EditForecast", Forecast);
            
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