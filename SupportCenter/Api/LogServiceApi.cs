using SupportCenter.Classes;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SupportCenter.Api
{
    public class LogServiceApi
    {
        private readonly HttpClient _httpClient;

        public LogServiceApi()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5179")
            };
        }

        public async Task SendLogAsync(
            string action,
            string objectAction,
            string namePc,
            string ipPc,
            string loginUser)
        {
            var dto = new LogCreateDto
            {
                Date = DateTime.Now,
                Action = action,
                ObjectAction = objectAction,
                NamePc = namePc,
                IpPc = ipPc,
                LoginUser = loginUser
            };

            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _httpClient.PostAsync("api/logs", content);
        }
    }
}
