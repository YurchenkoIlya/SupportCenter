using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApplication1.Dto;


    public class WorkstationsApiGet
    {
        private readonly HttpClient _httpClient;

        public WorkstationsApiGet()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5179") //  Web API
            };
        }

        public async Task<List<LastLoginDto>> GetLastLoginsAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<LastLoginDto>>("api/workstations/last-logins");
            return result ?? new List<LastLoginDto>();
        }
    }

