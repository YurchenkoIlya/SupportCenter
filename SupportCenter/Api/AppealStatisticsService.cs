using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApplication1.Dto;

namespace WpfApp.Services
{
    public class AppealStatisticsApi
    {
        private readonly HttpClient _client;

        public AppealStatisticsApi()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5179") // Web API
            };
        }

        public async Task<List<UserAppealStatDto>> GetUserAppealStatisticsAsync()
        {
           
                // Вызов API
                var stats = await _client.GetFromJsonAsync<List<UserAppealStatDto>>("api/appeal-statistics/users");

                return stats ?? new List<UserAppealStatDto>();
           
        }
    }
}