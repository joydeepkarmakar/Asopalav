using Asopalav.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;

namespace Asopalav.Controllers
{
    public class CartDetailsController : Controller
    {
        [Route("Cart/{id}")]
        public ActionResult Index()
        {
            // Init the cart list
            var cart = Session["Cart"] as List<CartVM> ?? new List<CartVM>();

            // Check if cart is empty
            if (cart.Count == 0 || Session["Cart"] == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return View();
            }

            // Calculate total and save to ViewBag

            decimal total = 0m;

            foreach (var item in cart)
            {
                total += item.Total;
            }

            ViewBag.GrandTotal = total;

            // Return view with list
            return View(cart);
        }

        public ActionResult AddToCartPartial(int id)
        {
            // Init CartVM list
            List<CartVM> cart = Session["Cart"] as List<CartVM> ?? new List<CartVM>();

            // Init CartVM
            CartVM model = new CartVM();

            using (AsopalavDBEntities db = new AsopalavDBEntities())
            {
                // Get the product
                ProductMaster product = db.ProductMasters.Find(id);

                // Check if the product is already in cart
                var productInCart = cart.FirstOrDefault(x => x.ProductId == id);

                // If not, add new
                if (productInCart == null)
                {
                    cart.Add(new CartVM()
                    {
                        ProductId = product.ProductID,
                        ProductName = product.ProductName,
                        Quantity = 1,
                        Price = product.Price,
                        Image = product.Images.ToList()
                    });
                }
                else
                {
                    // If it is, increment
                    productInCart.Quantity++;
                }
            }

            // Get total qty and price and add to model

            int qty = 0;
            decimal price = 0m;

            foreach (var item in cart)
            {
                qty += item.Quantity;
                price += item.Quantity * item.Price;
            }

            model.Quantity = qty;
            model.Price = price;

            // Save cart back to session
            Session["Cart"] = cart;

            // Return partial view with model
            return PartialView(model);
        }
    }
}