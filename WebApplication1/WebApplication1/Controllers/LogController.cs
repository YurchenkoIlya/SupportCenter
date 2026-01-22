using Microsoft.AspNetCore.Mvc;
using Npgsql;
using NpgsqlTypes;
using WebApplication1.Dto;

[ApiController]
[Route("api/logs")]
public class LogsController : ControllerBase
{
    private readonly string _connectionString;

    public LogsController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    [HttpPost]
    public async Task<IActionResult> CreateLog([FromBody] LogCreateDto dto)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var command = new NpgsqlCommand(
            @"INSERT INTO main.logs 
              (date, action, object_action, name_pc, ip_pc, id_user)
              VALUES (
                  date_trunc('second', @date),
                  @action,
                  @object_action,
                  @name_pc,
                  @ip_pc,
                  (SELECT user_id FROM main.users WHERE login_user = @login_user)
              )", connection);

        command.Parameters.Add("@date", NpgsqlDbType.Timestamp).Value = dto.Date;
        command.Parameters.Add("@action", NpgsqlDbType.Text).Value = dto.Action;
        command.Parameters.Add("@object_action", NpgsqlDbType.Text).Value = dto.ObjectAction;
        command.Parameters.Add("@name_pc", NpgsqlDbType.Text).Value = dto.NamePc;
        command.Parameters.Add("@ip_pc", NpgsqlDbType.Text).Value = dto.IpPc;
        command.Parameters.Add("@login_user", NpgsqlDbType.Text).Value = dto.LoginUser;

        await command.ExecuteNonQueryAsync();

        return Ok();
    }
}
