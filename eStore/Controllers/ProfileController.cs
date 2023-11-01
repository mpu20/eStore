using eStore.Data;
using eStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace eStore.Controllers
{
    public class ProfileController : Controller
    {
        private readonly StoreDBContext _dbContext;
        public ProfileController(StoreDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public  IActionResult Index(int id)
        {
            int? userID = HttpContext.Session.GetInt32("userID");
            if (userID != null && (userID == id || userID == 1))
            {
                return View(_dbContext.Members.ToList().SingleOrDefault(member => member.MemberId == id));
            }
            return View(_dbContext.Members.ToList().SingleOrDefault(member => member.MemberId == userID));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Member member)
        {
            var memberInDB = _dbContext.Members.ToList().SingleOrDefault(mem => mem.MemberId == member.MemberId);
            if (member.Password.Equals(memberInDB.Password) || HttpContext.Session.GetInt32("userID") == 1) {
                _dbContext.Entry<Member>(member).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbContext.SaveChanges();
            }
            else
            {
                TempData["error"] = "The password is incorrect!";
            }
            return RedirectToAction("Index");
        }
    }
}
