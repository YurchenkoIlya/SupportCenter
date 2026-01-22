namespace WebApplication1.Dto
{
    public class LastLoginDto
    {
        public string FullName { get; set; } = null!;
        public string NamePc { get; set; } = null!;
        public string IpPc { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}
