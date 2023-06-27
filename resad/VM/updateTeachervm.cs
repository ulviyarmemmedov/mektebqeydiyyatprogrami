using resad.Dtos;
using resad.Models;

namespace resad.VM
{
    public class updateTeachervm
    {
        public Teacher Teacher { get; set; }
        public List<Teacherlist> gelendatalar { get; set; }
        public List<Subject> subjects { get; set; }
        public int id { get; set; }
    }
}
