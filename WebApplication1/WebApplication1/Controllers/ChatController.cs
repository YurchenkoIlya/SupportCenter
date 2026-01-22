using Microsoft.AspNetCore.Mvc;
using Npgsql;
using NpgsqlTypes;
using SupportCenter.Classes;
using WebApplication1.Dto;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ChatController(IConfiguration config)
        {
            _config = config;
        }

        // Получение сообщений по типу обращения и ID обращения
        [HttpGet("{typeAppeal:int}/{appealId:int}")]
        public async Task<IEnumerable<ChatMessageDto>> GetMessages(int typeAppeal, int appealId)
        {
            var messages = new List<ChatMessageDto>();
            var connStr = _config.GetConnectionString("DefaultConnection");

            await using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();

            var sql = @"
                SELECT u.login_user, c.message, c.date
                FROM main.chat_request c
                JOIN main.users u ON u.user_id = c.id_user
                WHERE c.type_appeal = @type
                  AND c.id_appeal = @id
                ORDER BY c.date
            ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.Add("@type", NpgsqlDbType.Integer).Value = typeAppeal;
            cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = appealId;

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                messages.Add(new ChatMessageDto
                {
                    Login = reader.GetString(0),
                    Message = reader.GetString(1),
                    Date = reader.GetDateTime(2)
                });
            }

            return messages;
        }
        // POST: api/chat
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] CreateChatMessageDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Message))
                return BadRequest("Сообщение пустое");

            if (dto.TypeAppeal < 1 || dto.TypeAppeal > 3)
                return BadRequest("Некорректный тип обращения");

            var connStr = _config.GetConnectionString("DefaultConnection");
            await using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();

            var sql = @"
                INSERT INTO main.chat_request
                    (id_appeal, type_appeal, id_user, message, date)
                VALUES
                    (@appealId, @typeAppeal, 
                     (SELECT user_id FROM main.users WHERE login_user = @login), 
                     @message, NOW())
            ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.Add("@appealId", NpgsqlDbType.Integer).Value = dto.AppealId;
            cmd.Parameters.Add("@typeAppeal", NpgsqlDbType.Integer).Value = dto.TypeAppeal;
            cmd.Parameters.Add("@login", NpgsqlDbType.Text).Value = dto.Login;
            cmd.Parameters.Add("@message", NpgsqlDbType.Text).Value = dto.Message;

            int rows = await cmd.ExecuteNonQueryAsync();

            if (rows == 0)
                return BadRequest("Не удалось добавить сообщение");

            return Ok("Сообщение добавлено");
        }
    }
}