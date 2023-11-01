using eStore.Data;
using eStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace eStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly StoreDBContext _dbContext;

        public ProductController(StoreDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View(_dbContext.Products.Where(product => product.UnitsInStock > 0).ToList());
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View(_dbContext.Products.ToList().SingleOrDefault(product => product.ProductId == id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            _dbContext.Entry<Product>(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            return View(_dbContext.Products.ToList().SingleOrDefault(product => product.ProductId == id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(Product product)
        {
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
