using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asopalav.Models;
using DataAccessLayer;

namespace Asopalav.Controllers
{
    public class ProductDetailsController : Controller
    {
        // GET: ProductDetails
        [Route("ProductDetails/{id}")]
        public ActionResult Index(long id)
        {
            var host = System.Web.HttpContext.Current.Request.Url.OriginalString.Replace(System.Web.HttpContext.Current.Request.Url.PathAndQuery, "");
            ProductDetailsModel objProductDetailsModel = new ProductDetailsModel();
            int counter = 0;
            using (var context = new AsopalavDBEntities())
            {
                objProductDetailsModel.objProductMaster = context.ProductMasters.Include("Images").Where(p => p.ProductID == id).FirstOrDefault();
                if (objProductDetailsModel.objProductMaster.Images != null)
                {
                    foreach (var item in objProductDetailsModel.objProductMaster.Images)
                    {
                        counter++;
                        if (item.ImagePath != null)
                        {
                            if (!System.IO.File.Exists(Server.MapPath("~/" + item.ImagePath.ToString().Substring(item.ImagePath.ToString().IndexOf("Uploads")))))
                            {
                                item.ImagePath = Request.UrlReferrer.AbsoluteUri.Replace(Request.UrlReferrer.AbsolutePath, "/Content/images/no-product-image.jpg");
                            }
                            else
                            {
                                var tempImagePath = (host + "\\" + item.ImagePath.Substring(item.ImagePath.IndexOf("Uploads"))).Replace(@"\", "/");
                                item.ImagePath = tempImagePath;
                            }
                        }
                        else
                        {
                            item.ImagePath = Request.UrlReferrer.AbsoluteUri.Replace(Request.UrlReferrer.AbsolutePath, "/Content/images/no-product-image.jpg");
                        }

                        if (counter == 1)
                        {
                            objProductDetailsModel.DefaultImagePath = item.ImagePath;
                        }
                    }
                }
            }
            return View(objProductDetailsModel);
        }
    }
}