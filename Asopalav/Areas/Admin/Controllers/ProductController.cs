using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asopalav.Models;
using DataAccessLayer;

namespace Asopalav.Areas.Admin.Controllers
{
    [Route("[area]/[controller]/[action]")]
    public class ProductController : Controller
    {
        AsopalavDBEntities objAsopalavDBEntities = new AsopalavDBEntities();

        public ActionResult Index()
        {
            ViewData["ProductTypeID"] = GetProductTypeList();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ActionName("Products")]
        public ActionResult AddProduct(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                int result = objAsopalavDBEntities.AddProduct(model.ProductCode, model.ProductName, model.ProductTypeID, model.WeightInGms, model.Height_width_inch, model.Price, model.IsOffer, model.OfferPrice, model.SmallImage, model.BigImage, model.IsActive, model.Description);
            }
            return View(model);
        }

        private List<SelectListItem> GetProductTypeList()
        {
            List<SelectListItem> listProductType = new List<SelectListItem>();
            listProductType = (from producttype in objAsopalavDBEntities.ProductTypeMasters
                               select new SelectListItem()
                               {
                                   Value = producttype.ProductTypeID.ToString(),
                                   Text = producttype.ProductType
                               }).ToList();
            return listProductType;
        }
    }
}