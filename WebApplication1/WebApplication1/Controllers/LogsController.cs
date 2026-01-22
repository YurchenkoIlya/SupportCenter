using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WebApplication1.Dto;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public LogsController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("last-logins")]
        public async Task<IEnumerable<LastLoginDto>> GetLastLogins()
        {
            var connStr = _config.GetConnectionString("DefaultConnection");
            var list = new List<LastLoginDto>();

            await using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand(@"
            SELECT u.user_name AS full_name,
            l.name_pc,
            l.ip_pc,
            l.date
            FROM main.logs l
            LEFT JOIN main.users u ON u.user_id = l.id_user
                WHERE l.date = (
                    SELECT MAX(date)
                    FROM main.logs
                    WHERE id_user = u.user_id) ORDER BY u.user_name;", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new LastLoginDto
                {
                    FullName = reader.GetString(0),
                    NamePc = reader.GetString(1),
                    IpPc = reader.GetString(2),
                    Date = reader.GetDateTime(3)
                });
            }

            return list;
        }

    }
}
