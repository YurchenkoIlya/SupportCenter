using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WebApplication1.Dto;
  
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProgramController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("get")]
        public async Task<IEnumerable<ProgramDto>> GetUsers()
        {
            var programs = new List<ProgramDto>();

            var connStr = _config.GetConnectionString("DefaultConnection");

            await using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();

        var cmd = new NpgsqlCommand("Select u.id_program,u.name_program,o.user_name,u.way_program " +
       "from main.program u " +
       "LEFT JOIN main.users o ON u.responsible_program = o.user_id", conn);
        await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
               

                programs.Add(new ProgramDto
                {
                    id_program = reader.GetInt32(0),
                    name_program = reader.GetString(1),
                    responsible_program = reader.GetString(2),
                    way_program = reader.GetString(3)
                });
            }

            return programs;

        }
    }
