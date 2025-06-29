using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class OrderController(IOrderRepository repoService, Cart cartService) : Controller
    {
        private IOrderRepository repository = repoService;
        private Cart cart = cartService;
        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count == 0)
            {
                ModelState.AddModelError("", "Sorry your cart is empty");
            }
            if (ModelState.IsValid)
            {
                order.Lines = [.. cart.Lines];
                repository.SaveOrder(order);
                cart.Clear();
                return RedirectToPage("/Completed", new { OrderId = order.OrderID });
            }
            else
            {
                return View();
            }
        }
    }
}