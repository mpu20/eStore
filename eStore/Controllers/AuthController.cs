using eStore.Data;
using eStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace eStore.Controllers
{
    public class AuthController : Controller
    {

        private readonly StoreDBContext _dbContext;

        public AuthController(StoreDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("userId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Member member)
        {
            member = _dbContext.Members.ToList().SingleOrDefault(mem => mem.Email.Equals(member.Email) && mem.Password.Equals(member.Password));
            if (member != null)
            {
                HttpContext.Session.SetInt32("userID", member.MemberId);
                return RedirectToAction("Index", "Home");
            }
            TempData["error"] = "The email or password is incorrect!";
            return View();
        }
        
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userID");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            if (HttpContext.Session.GetInt32("userID") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Member member, string confirmPassword)
        {
            if (member.Password.Equals(confirmPassword))
            {
                if (_dbContext.Members.ToList().SingleOrDefault(mem => mem.Email.Equals(member.Email)) != null)
                {
                    TempData["error"] = "The email has been already existed!";
                    return View(member);
                }
                _dbContext.Members.Add(member);
                _dbContext.SaveChanges();
                return Login(member);
            }
            TempData["error"] = "Confirm Password does not match!";
            return View(member);
        }
    }
}
