using SupportCenter.Classes;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class FolderApiRedact
{
    private readonly HttpClient _client;

    public FolderApiRedact()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5179")
        };
    }

    public async Task<string> UpdateFolderAsync(folderResponsible dto)
    {
        var json = JsonSerializer.Serialize(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync("api/folder/put", content);

        if (!response.IsSuccessStatusCode)
        {
            return $"Ошибка сервера: {response.StatusCode}";
        }

        return await response.Content.ReadAsStringAsync();
    }
}

