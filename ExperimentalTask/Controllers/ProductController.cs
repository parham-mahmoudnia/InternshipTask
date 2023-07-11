using ExperimentalTask.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ExperimentalTask.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductDbContext _ctx;
        public ProductController(ProductDbContext ctx)
        {
            _ctx = ctx;
        }
        public IActionResult Index()
        {
            return View();
        }

        //it is get method
        [Authorize]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _ctx.Product.Add(product);
                _ctx.SaveChanges();
                TempData["msg"] = "Added successfully";
                return RedirectToAction("AddProduct");

            }
            catch (Exception ex)
            {
                TempData["msg"] = "Could not added!!!";
                return View();
            }

        }

        public IActionResult DisplayProducts()
        {
            var products = _ctx.Product.ToList();
            return View(products);
        }
        [Authorize]
        public IActionResult EditProduct(int id)
        {
            var product = _ctx.Product.Find(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _ctx.Product.Update(product);
                _ctx.SaveChanges();
                return RedirectToAction("DisplayProducts");

            }
            catch (Exception ex)
            {
                TempData["msg"] = "Could not update!!!";
                return View();
            }

        }
        [Authorize]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var product = _ctx.Product.Find(id);
                if (product != null)
                {
                    _ctx.Product.Remove(product);
                    _ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {


            }
            return RedirectToAction("DisplayProducts");

        }
    }
}
