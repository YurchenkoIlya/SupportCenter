using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WebApplication1.Dto;

[ApiController]
[Route("api/[controller]")]
public class AppealProgramController : ControllerBase
{
    private readonly IConfiguration _config;

    public AppealProgramController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet("get")]
    public async Task<IEnumerable<AppealProgramDto>> GetAppealPrograms()
    {
        var result = new List<AppealProgramDto>();

        var connStr = _config.GetConnectionString("DefaultConnection");

        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        var cmd = new NpgsqlCommand(@"
            SELECT u.id_appeal_program,
                   p.name_program,
                   u.pc_name,
                   u.ip_pc,
                   user_applicant.user_name AS applicant_user,
                   oib_user.user_name AS oib_user_name,
                   u.oib_status_responsible,
                   oit_user.user_name AS oit_user_name,
                   u.oit_status_responsible,
                   otp_user.user_name AS otp_user_name,
                   u.otp_status_executor
            FROM main.appealprogram u
            LEFT JOIN main.users oib_user ON u.oib_responsible = oib_user.user_id
            LEFT JOIN main.users oit_user ON u.oit_responsible = oit_user.user_id
            LEFT JOIN main.users otp_user ON u.otp_executor = otp_user.user_id
            LEFT JOIN main.users user_applicant ON u.applicant = user_applicant.user_id
            LEFT JOIN main.program p ON u.id_program = p.id_program
        ", conn);

        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new AppealProgramDto
            {
                id_appeal_program = reader.GetInt32(0),
                name_program = reader.GetString(1),
                pc_name = reader.GetString(2),
                ip_pc = reader.GetString(3),
                applicant_user = reader.GetString(4),

                oib_user_name = reader.IsDBNull(5) ? "—" : reader.GetString(5),
                oib_status = MapOibStatus(reader.GetInt32(6)),

                oit_user_name = reader.IsDBNull(7) ? "—" : reader.GetString(7),
                oit_status = MapOitStatus(reader.GetInt32(8)),

                otp_executor = reader.IsDBNull(9) ? "Нет исполнителя" : reader.GetString(9),
                otp_status = MapOtpStatus(reader.GetInt32(10))
            });
        }

        return result;
    }

    private static string MapOibStatus(int status) =>
        status switch
        {
            0 => "Не согласовано",
            1 => "Согласовано",
            2 => "Отказано",
            _ => "Неизвестно"
        };

    private static string MapOitStatus(int status) =>
        status switch
        {
            0 => "Не согласовано",
            1 => "Согласовано",
            2 => "Отказано",
            _ => "Неизвестно"
        };

    private static string MapOtpStatus(int status) =>
        status switch
        {
            0 => "Не в работе",
            1 => "В работе",
            2 => "Выполнено",
            3 => "Отменено",
            _ => "Неизвестно"
        };

    /*https://localhost:5179/api/AppealProgram/get */
}