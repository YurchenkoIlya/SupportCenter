using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Dto;

namespace SupportCenter.Api
{
    public class FolderReportApiGet
    {
        private readonly HttpClient _client;

        public FolderReportApiGet()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5179") // Web API
            };
        }

        public async Task<ReportFoldersDto> GetFolderReportAsync()
        {
            return await _client.GetFromJsonAsync<ReportFoldersDto>("api/ProgramReport/get-folder");
        }
    }
}
