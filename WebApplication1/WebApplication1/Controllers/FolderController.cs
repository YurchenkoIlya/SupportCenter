using Microsoft.AspNetCore.Mvc;
using Npgsql;
using NpgsqlTypes;
using System.Security.Cryptography.X509Certificates;
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
        [HttpPut("put")]
        public async Task<IActionResult> UpdateFolder([FromBody] FolderDto dto)
        {

            var connStr = _config.GetConnectionString("DefaultConnection");


            await using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();

            const string sql = @"
            UPDATE main.folders
            SET name_folder = @name_folder, 
            responsible_user = @responsible_user,
            way_folder = @way_folder,
            access_group = @access_group
            WHERE id = @id;
        ";

            await using var cmd = new NpgsqlCommand(sql, conn);

            int responsibleUserId = await GetIdResponsibleAsync(dto.responsibleUser, conn);


            cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = dto.Id;
            cmd.Parameters.Add("@name_folder", NpgsqlDbType.Text).Value = dto.nameFolder;
            cmd.Parameters.Add("@responsible_user", NpgsqlDbType.Integer).Value = responsibleUserId;
            cmd.Parameters.Add("@way_folder", NpgsqlDbType.Text).Value = dto.wayFolder;
            cmd.Parameters.Add("@access_group", NpgsqlDbType.Text).Value = dto.accessGroup;

            int rows = await cmd.ExecuteNonQueryAsync();

            if (rows == 0)
                return NotFound("Пользователь с таким ID не найден");
            {
                Console.WriteLine($"Пользователь с ID {dto.Id} успешно отредактирован. Role={dto.Id}, Activity={dto.nameFolder}");
                return Ok("Пользователь отредактирован");

            }

        }
        private async Task<int> GetIdResponsibleAsync(string userName, NpgsqlConnection conn)
        {
            const string sql = @"
            SELECT user_id
            FROM main.users
            WHERE user_name = @name
            LIMIT 1;
    ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", userName);

            var result = await cmd.ExecuteScalarAsync();

            if (result == null)
                throw new Exception("Пользователь не найден");

            return Convert.ToInt32(result);
        }
    }
}
