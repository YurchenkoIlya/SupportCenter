using SupportCenter.Classes;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class UsersApiClient
{
    private readonly HttpClient _http;

    public UsersApiClient()
    {
        _http = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5179") //  Web API
        };
    }

    public async Task<List<Users>> GetUsersAsync()
    {
        return await _http.GetFromJsonAsync<List<Users>>("api/users/get");
    }
}