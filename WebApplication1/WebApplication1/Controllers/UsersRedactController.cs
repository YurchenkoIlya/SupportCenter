using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using WebApplication1.Dto;

namespace WebApplication1.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class UsersRedactController : ControllerBase
    {
        private readonly string _connectionString;


    public UsersRedactController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
           
    }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] EditUserDto dto)
        {
            // Логика преобразования — 1 в 1 как в WPF

            int role = dto.role == "Пользователь" ? 0 : 1;
            int activity = dto.ActivityFlag ? 1 : 0;

            await using var conn = new NpgsqlConnection(_connectionString);
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
            Console.WriteLine($"Пользователь с ID {dto.Id} успешно отредактирован. Role={role}, Activity={activity}");
            return Ok("Пользователь отредактирован");
          
        }
        
        }
    }

