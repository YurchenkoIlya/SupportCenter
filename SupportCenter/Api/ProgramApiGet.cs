using SupportCenter.Classes;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SupportCenter.Api;
public class ProgramApiGet
{

    private readonly HttpClient _client;

    public ProgramApiGet()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5179") //  Web API
        };
    }

    public async Task<List<ProgramDto>> GetProgramAsync()
    {
        return await _client.GetFromJsonAsync<List<ProgramDto>>("api/program/get");
    }
}
