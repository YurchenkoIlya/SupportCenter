namespace WebApplication1.Dto
{
    public class ReportProgramDto
    {
  
            public string Name { get; set; }
            public int Total { get; set; }
            public int InWork { get; set; }
            public int Done { get; set; }
            public int Approved { get; set; }
            public int NotDone { get; set; }
            public int ApprovedNotInWork { get; set; }
        
    }
}
