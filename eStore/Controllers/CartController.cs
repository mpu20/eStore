using eStore.Data;
using eStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;

namespace eStore.Controllers
{
    public class CartController : Controller
    {
        private readonly StoreDBContext _dbContext;
        public CartController(StoreDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add(int productID)
        {
            List<Cart>? carts = new List<Cart>();
            string? cartString = HttpContext.Session.GetString("cart");
            if (cartString != null)
            {
                carts = JsonConvert.DeserializeObject<List<Cart>>(cartString);
            }
            bool found = false;
            foreach (Cart cart in carts)
            {
                if (cart.Product.ProductId == productID) { cart.Quantity++; found = true; }
            }
            if (!found)
            {
                Product product = _dbContext.Products.SingleOrDefault(pro => pro.ProductId == productID);
                carts.Add(new Cart{
                    Product = product, 
                    Quantity = 1 
                });
            }
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(carts));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ChangeQuantity(int id, int quantity)
        {
            List<Cart>? carts = new List<Cart>();
            string? cartString = HttpContext.Session.GetString("cart");
            if (cartString != null)
            {
                carts = JsonConvert.DeserializeObject<List<Cart>>(cartString);
            }
            foreach (Cart cart in carts)
            {
                if (cart.Product.ProductId == id) { cart.Quantity = quantity; }
            }
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(carts));
            return Json("Change quantity successfully");
        }

        [HttpPost]
        public JsonResult Remove(int id)
        {
            List<Cart>? carts = new List<Cart>();
            string? cartString = HttpContext.Session.GetString("cart");
            if (cartString != null)
            {
                carts = JsonConvert.DeserializeObject<List<Cart>>(cartString);
            }
            carts.RemoveAll(cart => cart.Product.ProductId == id);
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(carts));
            return Json("Remove successfully");
        }
    }
}
