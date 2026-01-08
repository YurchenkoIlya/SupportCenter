using System.Net.Http;
using System.Net.Http.Json;

public class LogService
{
    private readonly HttpClient _httpClient;

    public LogService(string baseUrl)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
    }

    public async Task LogAsync(string action, string namePc, string ipPc, int? idUser, string objectAction)
    {
        var dto = new
        {
            Action = action,
            NamePc = namePc,
            IpPc = ipPc,
            IdUser = idUser,
            ObjectAction = objectAction
        };

        var response = await _httpClient.PostAsJsonAsync("api/logs/add", dto);
        response.EnsureSuccessStatusCode();
    }
}