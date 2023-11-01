using eStore.Data;
using Microsoft.AspNetCore.Mvc;

namespace eStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly StoreDBContext _dbContext;
        public AccountController(StoreDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View(_dbContext.Members.ToList());
        }
    }
}
