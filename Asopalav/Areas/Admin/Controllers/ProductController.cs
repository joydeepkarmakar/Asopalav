using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Asopalav.Models;
using DataAccessLayer;
using Asopalav.Helpers;
using System.Data.Entity;

namespace Asopalav.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    [RoutePrefix("Product")]
    [Route("{action}")]
    public class ProductController : Controller
    {
        AsopalavDBEntities objAsopalavDBEntities = new AsopalavDBEntities();
        public static Dictionary<string, string> imageUrlList = new Dictionary<string, string>();
        ProductModel objProductModel = new ProductModel();

        [Route("~/Admin/Product/Add")]
        public ActionResult Index(int id = 0)
        {
            var host = System.Web.HttpContext.Current.Request.Url.OriginalString.Replace(System.Web.HttpContext.Current.Request.Url.PathAndQuery, "");
            var images = Session["Images"] as List<DataAccessLayer.Image> ?? new List<DataAccessLayer.Image>();
            ProductDetailsModel objProductDetailsModel = new ProductDetailsModel
            {
                objProductMaster = objAsopalavDBEntities.ProductMasters.Include("Images").Where(p => p.ProductID == id).FirstOrDefault()
            };
            if (objProductDetailsModel.objProductMaster != null)
            {
                objProductModel.ProductID = objProductDetailsModel.objProductMaster.ProductID;
                objProductModel.ProductCode = objProductDetailsModel.objProductMaster.ProductCode;
                objProductModel.ProductName = objProductDetailsModel.objProductMaster.ProductName;

                objProductModel.ProductTypeID = objProductDetailsModel.objProductMaster.ProductTypeID;
                ViewBag.ProductTypeList = new SelectList(GetProductTypeList(), "Value", "Text", objProductModel.ProductTypeID);

                objProductModel.WeightInGms = objProductDetailsModel.objProductMaster.WeightInGms;
                objProductModel.HeightInInch = objProductDetailsModel.objProductMaster.HeightInInch;
                objProductModel.WidthInInch = objProductDetailsModel.objProductMaster.WidthInInch;
                objProductModel.Price = objProductDetailsModel.objProductMaster.Price;
                objProductModel.IsOffer = objProductDetailsModel.objProductMaster.IsOffer;
                objProductModel.OfferPrice = objProductDetailsModel.objProductMaster.OfferPrice;
                objProductModel.IsActive = objProductDetailsModel.objProductMaster.IsActive;
                objProductModel.Description = objProductDetailsModel.objProductMaster.Description;

                if (objProductDetailsModel.objProductMaster.Images != null)
                {
                    foreach (var item in objProductDetailsModel.objProductMaster.Images)
                    {
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
                    }
                    Session["Images"] = objProductDetailsModel.objProductMaster.Images;
                }
            }
            else
            {
                var prodCount = objAsopalavDBEntities.ProductMasters.Count();
                objProductModel.ProductCode = "AJ000" + Convert.ToString(prodCount + 1);
                ViewBag.ProductTypeList = new SelectList(GetProductTypeList(), "Value", "Text");
            }
            return View(objProductModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductMaster objProductMaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objProductMaster.CreationDate = DateTime.Now;
                    objAsopalavDBEntities.ProductMasters.Add(objProductMaster);
                    if (imageUrlList != null)
                    {
                        var imageList = new List<DataAccessLayer.Image>();
                        foreach (var image in imageUrlList)
                        {
                            var img = new DataAccessLayer.Image { ProductID = objProductMaster.ProductID };
                            img.ImageName = image.Key;
                            img.ImagePath = GetUploadImageUrl(image.Value, objProductMaster.ProductCode);
                            imageList.Add(img);
                        }
                        objProductMaster.Images = imageList;
                    }
                    objAsopalavDBEntities.SaveChanges();
                    imageUrlList.Clear();
                    TempData["isProductSaved"] = "true";
                    TempData["ProductSaveMsg"] = "Record Saved. (Item Name : " + objProductMaster.ProductCode + " )";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["isSaved"] = "false";
                if (ex.InnerException != null)
                {
                    TempData["ProductSaveMsg"] = ex.InnerException.Message;
                }
                else
                {
                    TempData["ProductSaveMsg"] = ex.Message;
                }
            }
            ViewData["ProductTypeID"] = GetProductTypeList();
            return View(objProductModel);
        }

        [HttpPost]
        [Route("~/Admin/Product/UploadImage")]
        public ActionResult Upload()
        {
            bool isSavedSuccessfully = true;
            string imgFileName = string.Empty, imageUrl = string.Empty;
            Bitmap original = null;

            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    imgFileName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                        original = Bitmap.FromStream(file.InputStream) as Bitmap;
                        if (original != null)
                        {
                            var img = ImageHelper.HardResizeImage(1024, 1024, ImageHelper.CreateImage(original, 0, 0, original.Width, original.Height));
                            var fn = Server.MapPath("~/Uploads/Temp/" + fileNameWithoutExtension + ".png");
                            img.Save(fn, System.Drawing.Imaging.ImageFormat.Png);
                            var host = System.Web.HttpContext.Current.Request.Url.OriginalString.Replace(System.Web.HttpContext.Current.Request.Url.PathAndQuery, "");
                            imageUrl = (host + "\\" + fn.Substring(fn.IndexOf("Uploads"))).Replace(@"\", "/");
                        }
                        imageUrlList.Add(fileNameWithoutExtension, imageUrl);
                    }
                }

                ViewData["ProductTypeID"] = GetProductTypeList();
                objProductModel.ImageDetailsList = imageUrlList;
                return View("Index", objProductModel);
            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }
            if (isSavedSuccessfully)
            {
                return Json(new
                {
                    Message = imgFileName
                });
            }
            else
            {
                return Json(new
                {
                    Message = "Error in saving file"
                });
            }
        }

        private List<SelectListItem> GetProductTypeList()
        {
            List<SelectListItem> listProductType = new List<SelectListItem>();
            try
            {
                listProductType = (from producttype in objAsopalavDBEntities.ProductTypeMasters
                                   where producttype.IsActive == true
                                   select new SelectListItem()
                                   {
                                       Value = producttype.ProductTypeID.ToString(),
                                       Text = producttype.ProductType
                                   }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listProductType;
        }

        private string GetUploadImageUrl(string tempImagePath, string productCode)
        {
            string uploadImageUrl = string.Empty;
            try
            {
                var uploadFolder = "~/Uploads/" + productCode.Replace(" ", "_").Replace("/", "_").Replace(":", "_").ToLower();
                DirectoryInfo di = new DirectoryInfo(Server.MapPath(uploadFolder));
                if (!di.Exists)
                {
                    di.Create();
                }
                if (tempImagePath != "")
                {
                    var actfileName = ImageHelper.GetUrlFileName(tempImagePath);
                    var fileName = Path.GetFileName(tempImagePath.Replace(actfileName, actfileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff")));
                    if (fileName != "")
                    {
                        uploadImageUrl = uploadFolder.Remove(0, 2) + "/" + fileName;
                        var destinationPath = Server.MapPath(uploadFolder) + "\\" + fileName;
                        var host = System.Web.HttpContext.Current.Request.Url.OriginalString.Replace(System.Web.HttpContext.Current.Request.Url.PathAndQuery, "");
                        var sourcePhysicalPath = Server.MapPath(tempImagePath.Replace(host, "~").Replace(@"/", "\\"));
                        System.IO.File.Copy(sourcePhysicalPath, destinationPath, true);
                        System.IO.File.Delete(sourcePhysicalPath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return uploadImageUrl;
        }

        [Route("~/Admin/Product/List")]
        public ActionResult ProductList()
        {
            Session["CurrentPage"] = "Products";
            return View();
        }

        public ActionResult GetProductList()
        {
            /*IEnumerable<ProductListModel>*/
            var prodList = objAsopalavDBEntities.ProductMasters.Where(x => x.IsActive == true).Select(x => new ProductListModel
            {
                ProductId = x.ProductID,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                ProductType = x.ProductTypeMaster.ProductType,
                ProductTypeId = x.ProductTypeMaster.ProductTypeID,
                WeightInGms = x.WeightInGms,
                HeightInInch = x.HeightInInch,
                WidthInInch = x.WidthInInch,
                Price = x.Price,
                IsOffer = x.IsOffer,
                OfferPrice = x.OfferPrice,
                IsActive = x.IsActive,
                Description = x.Description
                //,ImagePaths= objAsopalavDBEntities.Images.Where(i => i.ProductID == x.ProductID)
                //                                         .Select(i => new {
                //                                                            ImageID =i.ImageID,

                //                                                                                }).ToList()
            }).ToList();

            return Json(new
            {
                data = prodList
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateProduct(ProductListModel objproductListModel)
        {
            objProductModel.ProductID = objproductListModel.ProductId;
            objProductModel.ProductCode = objproductListModel.ProductCode;
            objProductModel.ProductName = objproductListModel.ProductName;
            //objProductModel.ProductType = objproductListModel.ProductType;
            objProductModel.ProductTypeID = objproductListModel.ProductTypeId;
            objProductModel.WeightInGms = objproductListModel.WeightInGms;
            objProductModel.HeightInInch = objproductListModel.HeightInInch;
            objProductModel.WidthInInch = objproductListModel.WidthInInch;
            objProductModel.Price = objproductListModel.Price;
            objProductModel.IsOffer = objproductListModel.IsOffer;
            objProductModel.OfferPrice = objproductListModel.OfferPrice;
            objProductModel.IsActive = objproductListModel.IsActive;
            objProductModel.Description = objproductListModel.Description;
            //objProductModel.ImagePath = objproductListModel.ImagePaths;
            return RedirectToAction("Index", "Product", objProductModel);
        }

        public ActionResult GetImageList()
        {
            var imageList = Session["Images"];
            return Json(new { Data = imageList }, JsonRequestBehavior.AllowGet);
        }
    }
}