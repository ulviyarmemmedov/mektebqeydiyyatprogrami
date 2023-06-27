using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using resad.Data;
using resad.Dtos;
using resad.Models;
using resad.VM;
using System.Security.Claims;

namespace resad.Controllers
{
    [Authorize]
    public class studentController : Controller
    {
        private readonly MyDbContext _context;
        public studentController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult student()
        {

            Studentvm vm = new Studentvm() { subjects = _context.Subjects.ToList() };

            return View(vm);
        }

        public IActionResult Getteacher(int id)
        {
            var data = _context.Teachers.Where(x => x.SubjectId == id).ToList();
            return Json(data);
        }
        public IActionResult add(Studentvm st)
        {
            _context.Students.Add(st.student);
            _context.SaveChanges();
            return RedirectToAction("slist");
        }
        public IActionResult slist()
        {
            ViewBag.rol = User.FindFirst(ClaimTypes.Role)?.Value;
            var query = from s in _context.Students
                        join sb in _context.Subjects on s.SubjectId equals sb.Id
                        join t in _context.Teachers on s.TeacherId equals t.Id
                    
                        select new slist
                        {
                            Id = s.Id,
                            Name = s.Name,
                            SurName = s.SurName,
                            Subject = sb.Name,
                            Teacher= t.Name+" "+t.SurName
                        };
            slistvm vm = new slistvm()
            {
                melumatlar = query.ToList()
            };
            return View(vm);
        }
        public IActionResult delete(int id)
        {
            var di = _context.Students.FirstOrDefault(x => x.Id == id);
            _context.Students.Remove(di);
            _context.SaveChanges();
            return RedirectToAction("slist");
        }

        public IActionResult UpdateS(int id)
        {

            var query = from s in _context.Students

                        join sb in _context.Subjects on s.SubjectId equals sb.Id
                        join t in _context.Teachers on s.TeacherId equals t.Id
                        where s.Id == id
                        select new UpdateSDto
                        {
                            Id = s.Id,
                            Name = s.Name,
                            SurName = s.SurName,
                            Subject = sb.Name,
                            SubjectId = s.SubjectId,
                            TeacherId = s.TeacherId,
                            Teacher = t.Name + " " + t.SurName

                        };

            var yeni = query.ToList();

            var fenn = _context.Subjects.Where(x => x.Id != yeni[0].SubjectId).ToList();
            UpdateSvm vm = new UpdateSvm()
            {
                Subjects = fenn,
                Students = yeni

            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult UpdateS(UpdateSvm vm)
        {
            var d = _context.Students.FirstOrDefault(x => x.Id == vm.id);
            d.Name = vm.ustudent.Name;
            d.SurName = vm.ustudent.SurName;
            d.SubjectId = vm.ustudent.SubjectId;
            d.TeacherId = vm.ustudent.TeacherId;
            _context.Students.Update(d);
            _context.SaveChanges();
            return RedirectToAction("slist");
        }
        public IActionResult findteachers(int firstid)
        {
           
            var data=_context.Teachers.Where(x=>x.Id != firstid).ToList();
            return Json(data);

        }
      
    }
}
