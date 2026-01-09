using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SupportCenter.Classes;

public class AppealProgramApi
{
    private readonly HttpClient _client;

    public AppealProgramApi()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5179/")
        };
    }

    public async Task<List<AppealProgramDto>> GetAppealProgramsAsync()
    {
        var response = await _client.GetAsync("api/AppealProgram/get");

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Ошибка сервера: {response.StatusCode}");

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<List<AppealProgramDto>>(
    json,
    new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    });

        return result ?? new List<AppealProgramDto>();
    }
}
