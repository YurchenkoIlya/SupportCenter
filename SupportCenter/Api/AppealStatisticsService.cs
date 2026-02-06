using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using WebApplication1.Dto;

namespace WpfApp.Services
{
    public class AppealStatisticsApi
    {
        private readonly HttpClient _httpClient;

        public AppealStatisticsApi()
        {
            var baseAddress = Application.Current.Resources["ApiBaseAddress"]?.ToString()
                          ?? throw new Exception("ApiBaseAddress не найден в App.xaml");

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task<List<UserAppealStatDto>> GetUserAppealStatisticsAsync()
        {
           
                // Вызов API
                var stats = await _httpClient.GetFromJsonAsync<List<UserAppealStatDto>>("api/appeal-statistics/users");

                return stats ?? new List<UserAppealStatDto>();
           
        }
    }
}