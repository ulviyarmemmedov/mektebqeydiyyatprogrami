namespace resad.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public int? TeacherId { get; set; }
        public int? SubjectId { get; set; }
    }
}
