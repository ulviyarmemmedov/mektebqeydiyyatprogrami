using resad.Dtos;
using resad.Models;

namespace resad.VM
{
    public class UpdateSvm
    {
        public List<UpdateSDto> Students { get; set; }
        public List<Subject> Subjects { get; set; }
        public Student ustudent { get; set; }
        public int id { get; set; }
    }
}
