using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using resad.Data;
using resad.Dtos;
using resad.VM;
using System.Security.Claims;

namespace resad.Controllers
{
    [Authorize]
    public class TeacherController : Controller
    {
        private readonly MyDbContext _context;
        public TeacherController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Teacher()
        {
            TeacherVm vm = new TeacherVm()
            {
                Subjects = _context.Subjects.ToList(),


            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult add(TeacherVm tc)
        {
            _context.Teachers.Add(tc.Teacher);
            _context.SaveChanges();
            return RedirectToAction("showTeacherlist");
        }
        public IActionResult ShowTeacherList()
        {
            ViewBag.rol = User.FindFirst(ClaimTypes.Role)?.Value;
            var query = from t in _context.Teachers
                        join s in _context.Subjects on t.SubjectId equals s.Id
                        select new Teacherlist
                        {
                            Id = t.Id,
                            Name = t.Name,
                            SurName = t.SurName,
                            Subject = s.Name
                        };

            Teacherlistvm vm = new Teacherlistvm()
            {
                Teachers = query




            };


            return View(vm);
        }
        public IActionResult deleteteacher(int id)
        {
            var deleteitem = _context.Teachers.FirstOrDefault(x => x.Id == id);
            _context.Teachers.Remove(deleteitem);
            _context.SaveChanges();
            return RedirectToAction("showteacherlist");
        }
        public IActionResult update(int id)
        {
            var gelendata = _context.Teachers.FirstOrDefault(x => x.Id == id);
            var lazimidata = _context.Subjects.Where(x => x.Id != gelendata.SubjectId).ToList();
            var query = from t in _context.Teachers
                        join s in _context.Subjects on t.SubjectId equals s.Id
                        where t.Id == id
                        select new Teacherlist
                        {
                            Id = t.Id,
                            Name = t.Name,
                            SurName = t.SurName,
                            Subject = s.Name
                        };
            updateTeachervm vm = new updateTeachervm() { subjects = lazimidata, Teacher = gelendata ,gelendatalar=query.ToList()};
            return View(vm);

        }
        [HttpPost]
        public IActionResult update(updateTeachervm vm)
        {
            var ld = _context.Teachers.FirstOrDefault(x => x.Id == vm.id);
            var g = vm.Teacher;
            ld.Name = g.Name;
            ld.SurName = g.SurName;
            ld.SubjectId = g.SubjectId;
            _context.Teachers.Update(ld);
            _context.SaveChanges();
            return RedirectToAction("showteacherlist");
        }
    }
}
