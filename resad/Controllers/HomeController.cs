using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using resad.Data;
using resad.Dtos;
using resad.Models;
using resad.VM;
using System.Diagnostics;
using System.Security.Claims;

namespace resad.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;
        public HomeController(ILogger<HomeController> logger,MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult student()
        {
            return View();
        }
        public IActionResult show()
        {
            var query = from s in _context.Students
                        join sb in _context.Subjects on s.SubjectId equals sb.Id
                        join t in _context.Teachers on s.TeacherId equals t.Id

                        select new slist
                        {
                            Id = s.Id,
                            Name = s.Name,
                            SurName = s.SurName,
                            Subject = sb.Name,
                            Teacher = t.Name + " " + t.SurName
                        };
            slistvm vm = new slistvm()
            {
                melumatlar = query.ToList()
            };
            return View(vm);
        }
        [AllowAnonymous]
        public IActionResult login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> login(string name, string pass)
        {

            if (name == null || pass == null) NotFound();

            var item = _context.Users.Where(c => c.Name == name && c.PassWord == pass).FirstOrDefault();
            if (item != null)
            {
                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,item.Name),
                            new Claim(ClaimTypes.Role,item.Role),
                            new Claim("pass",item.PassWord)
                        };
                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), (authProperties));
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult signout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}