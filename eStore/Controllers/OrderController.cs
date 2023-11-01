using eStore.Data;
using eStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace eStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly StoreDBContext _dbContext;
        public OrderController(StoreDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            List<Order> orders = new List<Order>();
            int? userID = HttpContext.Session.GetInt32("userID");
            if (userID != null)
            {
                if (userID != 1)
                {
                    orders = _dbContext.Orders.ToList().Where(member => member.MemberId == userID).ToList();
                }
                else orders = _dbContext.Orders.ToList();
            }
            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(Order order)
        {
            string cartString = HttpContext.Session.GetString("cart");
            if (cartString == null)
            {
                return RedirectToAction("Index","Product");
            }
            else
            {
                order.MemberId = (int)HttpContext.Session.GetInt32("userID");
                order.OrderDate = DateTime.Now;
                _dbContext.Add(order);
                _dbContext.SaveChanges();
                int orderID = _dbContext.Orders.ToList().Max(o => o.OrderId);
                List<Cart> carts = JsonConvert.DeserializeObject<List<Cart>>(cartString);
                foreach(Cart cart in carts)
                {
                    Product product = _dbContext.Products.Find(cart.Product.ProductId);
                    product.UnitsInStock -= cart.Quantity;
                    _dbContext.Entry<Product>(product).State = EntityState.Modified;
                    _dbContext.Add(new OrderDetail
                    {
                        OrderId = orderID,
                        ProductId = cart.Product.ProductId,
                        UnitPrice = cart.Product.UnitPrice,
                        Quantity = cart.Quantity,
                        Discount = 0
                    });
                    _dbContext.SaveChanges();
                }
                HttpContext.Session.Remove("cart");
            }            
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int id)
        {
            Order order = _dbContext.Orders.Find(id);
            List<OrderDetail> orderDetails = _dbContext.OrderDetails.ToList().Where(orderdetail => orderdetail.OrderId == id).ToList();
            List<Tuple<OrderDetail, Product>> detailsWithProduct = new List<Tuple<OrderDetail, Product>>();
            foreach(OrderDetail orderDetail in orderDetails)
            {
                detailsWithProduct.Add(new Tuple<OrderDetail, Product>(orderDetail, _dbContext.Products.Find(orderDetail.ProductId)));
            }
            ViewBag.order = order;
            ViewData["detailsWithProduct"] = detailsWithProduct;
            return View();
        }
    }
}
