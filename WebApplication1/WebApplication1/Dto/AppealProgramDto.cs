namespace WebApplication1.Dto
{
    public class AppealProgramDto
    {
        public int id_appeal_program { get; set; }
        public string name_program { get; set; }
        public string pc_name { get; set; }
        public string ip_pc { get; set; }
        public string applicant_user { get; set; }

        public string oib_user_name { get; set; }
        public string oib_status { get; set; }

        public string oit_user_name { get; set; }
        public string oit_status { get; set; }

        public string otp_executor { get; set; }
        public string otp_status { get; set; }
    }
}