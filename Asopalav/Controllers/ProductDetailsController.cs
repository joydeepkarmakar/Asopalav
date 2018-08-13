using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asopalav.Helpers;
using Asopalav.Models;
using DataAccessLayer;

namespace Asopalav.Controllers
{
    public class ProductDetailsController : Controller
    {
        // GET: ProductDetails
        [Route("ProductDetails/{id}")]
        public ActionResult Index(string id)
        {
            var host = System.Web.HttpContext.Current.Request.Url.OriginalString.Replace(System.Web.HttpContext.Current.Request.Url.PathAndQuery, "");
            ProductDetailsModel objProductDetailsModel = new ProductDetailsModel();
            int counter = 0;
            using (var context = new AsopalavDBEntities())
            {
                var decodedId = int.Parse(id.Base64Decode());
                objProductDetailsModel.objProductMaster = context.ProductMasters.Include("Images").Where(p => p.ProductID == decodedId).FirstOrDefault();

                if ((string)Session["CurrentCurrency"] == "INR")
                {
                    objProductDetailsModel.objProductMaster.Price = Math.Round(objProductDetailsModel.objProductMaster.Price * Convert.ToDecimal(Session["DollarRate"]), 2);
                    objProductDetailsModel.objProductMaster.OfferPrice = Math.Round(Convert.ToDecimal(objProductDetailsModel.objProductMaster.OfferPrice) * Convert.ToDecimal(Session["DollarRate"]), 2);
                }

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