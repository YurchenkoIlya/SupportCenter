using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WebApplication1.Dto;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramReportController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProgramReportController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("get")]
        public async Task<ReportProgramDto> GetProgramReport()
        {
            var connStr = _config.GetConnectionString("DefaultConnection");

            await using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand(@"
            SELECT
            COUNT(*) AS total,
            COUNT(*) FILTER (WHERE otp_executor IS NOT NULL AND otp_status_executor = 0) AS in_work,
            COUNT(*) FILTER (WHERE otp_status_executor = 1) AS done,
            COUNT(*) FILTER (WHERE oib_status_responsible = 1) AS approved,
            COUNT(*) FILTER (WHERE otp_status_executor = 0) AS not_done,
            COUNT(*) FILTER (
            WHERE oib_status_responsible = 1 AND otp_executor IS NULL
             ) AS approved_not_in_work
            FROM main.appealprogram;", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            await reader.ReadAsync();

            var dto = new ReportProgramDto
            {
                Total = reader.GetInt32(0),
                InWork = reader.GetInt32(1),
                Done = reader.GetInt32(2),
                Approved = reader.GetInt32(3),
                NotDone = reader.GetInt32(4),
                ApprovedNotInWork = reader.GetInt32(5)
            };

            return dto;
        }
    }
}
