using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using WebApplication1.Dto;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/appeal-statistics")]
    public class AppealStatisticsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AppealStatisticsController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("users")]
        public async Task<IEnumerable<UserAppealStatDto>> GetUserAppealStatistics()
        {
            var result = new List<UserAppealStatDto>();

            var connStr = _config.GetConnectionString("DefaultConnection");

            await using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand(@"
          WITH all_appeals AS (
 
                SELECT executor AS user_id, status_executor AS status
                FROM main.appealfolder
                WHERE executor IS NOT NULL

                UNION ALL

                SELECT executor AS user_id, status_execute AS status
                FROM main.appealprinter
                WHERE executor IS NOT NULL

                UNION ALL

                SELECT otp_executor AS user_id, otp_status_executor AS status
                FROM main.appealprogram
                WHERE otp_executor IS NOT NULL
                )
                SELECT
                u.user_name AS fio,                       
                COUNT(*) FILTER (WHERE a.status = 2) AS done_count,     
                COUNT(*) FILTER (WHERE a.status = 1) AS in_work_count   
                FROM all_appeals a
                JOIN main.users u ON u.user_id = a.user_id
                GROUP BY u.user_name
                ORDER BY u.user_name;", conn);


            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new UserAppealStatDto
                {
                    UserName = reader.GetString(0),
                    DoneCount = reader.GetInt32(1),
                    InWorkCount = reader.GetInt32(2)
                });
            }

            return result;
        }
    }
}

