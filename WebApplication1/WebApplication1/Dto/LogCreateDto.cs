namespace WebApplication1.Dto
{
    public class LogCreateDto
    {
        public DateTime Date { get; set; }
        public string Action { get; set; }
        public string ObjectAction { get; set; }
        public string NamePc { get; set; }
        public string IpPc { get; set; }
        public string LoginUser { get; set; }
    }
}
