using Microsoft.AspNetCore.Mvc;
using Npgsql;
using NpgsqlTypes;
using WebApplication1.Dto;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _config;

    public UsersController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet("get")]
    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        var users = new List<UserDto>();

        var connStr = _config.GetConnectionString("DefaultConnection");

        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        var cmd = new NpgsqlCommand("SELECT * FROM main.users", conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            string roleUser = reader.GetInt32(2) == 0 ? "Пользователь" : "Администратор";
            string activity = reader.GetInt32(4) == 1 ? "Включена" : "Выключена";

            users.Add(new UserDto
            {
                Id = reader.GetInt32(0),
                Login = reader.GetString(1),
                Role = roleUser,
                Name = reader.GetString(3),
                Activity = activity
            });
        }

        return users;

    }
    [HttpPut("put")]
    public async Task<IActionResult> UpdateUser([FromBody] EditUserDto dto)
    {
        
        var connStr = _config.GetConnectionString("DefaultConnection");
        int role = dto.Role == "Пользователь" ? 0 : 1;
        int activity = dto.ActivityFlag ? 1 : 0;

        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        const string sql = @"
            UPDATE main.users
            SET user_role = @role,
                activity = @activity
            WHERE user_id = @id;
        ";

        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.Add("@id", NpgsqlDbType.Bigint).Value = dto.Id;
        cmd.Parameters.Add("@role", NpgsqlDbType.Bigint).Value = role;
        cmd.Parameters.Add("@activity", NpgsqlDbType.Bigint).Value = activity;

        int rows = await cmd.ExecuteNonQueryAsync();

        if (rows == 0)
            return NotFound("Пользователь с таким ID не найден");
        {
            Console.WriteLine($"Пользователь с ID {dto.Id} успешно отредактирован. Роль ={role}, Активность ={activity}");
            return Ok("Пользователь отредактирован");

        }
    }
    }