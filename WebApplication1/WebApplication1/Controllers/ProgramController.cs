using Microsoft.AspNetCore.Mvc;
using Npgsql;
using NpgsqlTypes;
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
    [HttpPut("put")]
    public async Task<IActionResult> UpdateProgram([FromBody] ProgramDto dto)
    {

        var connStr = _config.GetConnectionString("DefaultConnection");


        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        const string sql = @"
            UPDATE main.program
            SET name_program = @name_program, 
            way_program = @way_program
            WHERE id_program = @id;
        ";

        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = dto.id_program;
        cmd.Parameters.Add("@name_program", NpgsqlDbType.Text).Value = dto.name_program;
        cmd.Parameters.Add("@way_program", NpgsqlDbType.Text).Value = dto.way_program;
       

        int rows = await cmd.ExecuteNonQueryAsync();

        if (rows == 0)
            return NotFound("Программа с таким ID не найдена");
        {
            Console.WriteLine($"Программа отредактирована Наименование - {dto.name_program} Путь - {dto.way_program}");
            return Ok("Программа отредактирована");
            
        }

    }
}
