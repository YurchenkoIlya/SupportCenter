using Microsoft.AspNetCore.Mvc;
using Npgsql;
using NpgsqlTypes;
using WebApplication1.Dto;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FolderController : ControllerBase
    {
        private readonly IConfiguration _config;

        public FolderController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("get")]
        public async Task<IEnumerable<FolderDto>> GetFolder()
        {
            var folder = new List<FolderDto>();

            var connStr = _config.GetConnectionString("DefaultConnection");

            await using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("" +
                "Select u.id,u.name_folder,o.user_name,u.way_folder, u.access_group " +
                "from main.folders u " +
                "LEFT JOIN main.users o ON u.responsible_user = o.user_id", conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
               

                folder.Add(new FolderDto
                {
                    Id = reader.GetInt32(0),
                    nameFolder = reader.GetString(1),
                    responsibleUser = reader.GetString(2),
                    wayFolder = reader.GetString(3),
                    accessGroup = reader.GetString(4)

                });
            }

            return folder;

        }
    }
}
