namespace resad.Dtos
{
    public class UpdateSDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public int? SubjectId  { get; set; }
        public string? Subject { get; set; }
        public int? TeacherId { get; set; }
        public string? Teacher { get; set; }
    }
}
