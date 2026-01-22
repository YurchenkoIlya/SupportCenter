namespace WebApplication1.Dto
{
    public class CreateChatMessageDto
    {
        public int AppealId { get; set; }
        public int TypeAppeal { get; set; } // 1, 2 или 3
        public string Message { get; set; }
        public string Login { get; set; } 
    }
}
