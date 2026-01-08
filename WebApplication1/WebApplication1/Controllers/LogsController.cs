using Microsoft.AspNetCore.Mvc;
using Npgsql;

[ApiController]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
    private readonly string _connectionString;

    public LogsController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    [HttpPost("add")]
    public IActionResult AddLog([FromBody] ActionLogDto dto)
    {
        if (dto == null || string.IsNullOrEmpty(dto.Action))
            return BadRequest("Action is required");

        try
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = @"
                INSERT INTO logs (action, name_pc, ip_pc, id_user, date, object_action)
                VALUES (@action, @name_pc, @ip_pc, @id_user, @date, @object_action)";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("action", dto.Action);
            cmd.Parameters.AddWithValue("name_pc", (object)dto.NamePc ?? DBNull.Value);
            cmd.Parameters.AddWithValue("ip_pc", (object)dto.IpPc ?? DBNull.Value);
            cmd.Parameters.AddWithValue("id_user", (object)dto.IdUser ?? DBNull.Value);
            cmd.Parameters.AddWithValue("date", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("object_action", (object)dto.ObjectAction ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error saving log: {ex.Message}");
        }

        return Ok("Log added successfully");
    }
}

public class ActionLogDto
{
    public string Action { get; set; }
    public string NamePc { get; set; }
    public string IpPc { get; set; }
    public int? IdUser { get; set; }
    public string ObjectAction { get; set; }
}