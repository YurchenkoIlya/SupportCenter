using SupportCenter.Classes;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class FolderApiGet
{
    private readonly HttpClient _client;

    public FolderApiGet()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5179") //  Web API
        };
    }

    public async Task<List<folderResponsible>> GetFolderAsync()
    {
        return await _client.GetFromJsonAsync<List<folderResponsible>>("api/folder/get");
    }

}

