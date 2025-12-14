using Microsoft.AspNetCore.Mvc;
using Npgsql;
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

    [HttpGet]
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
                Login = reader.GetString(1), // вместо UserName
                Role = roleUser,
                Name = reader.GetString(3),
                Activity = activity
            });
        }

        return users;
    }
}