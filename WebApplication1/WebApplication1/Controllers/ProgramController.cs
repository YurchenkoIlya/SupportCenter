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
        public async Task<IEnumerable<ProgramDto>> GetProgram()
        {
            var programs = new List<ProgramDto>();

            var connStr = _config.GetConnectionString("DefaultConnection");

            await using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();

        var cmd = new NpgsqlCommand("Select id_program,name_program,way_program from main.program", conn);
        await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
               

                programs.Add(new ProgramDto
                {
                    id_program = reader.GetInt32(0),
                    name_program = reader.GetString(1),
                    way_program = reader.GetString(2)
                });
            }
           

            return programs;

        }
    }
