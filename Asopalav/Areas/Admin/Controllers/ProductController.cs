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
using System.Data;
using System.Data.SqlClient;

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
            ViewBag.ProductTypeList = new SelectList(GetProductTypeList(), "Value", "Text");
            ViewBag.MetalList = new SelectList(GetMetalList(), "Value", "Text");
            ViewBag.GemList = new SelectList(GetGemList(), "Value", "Text");
            ViewBag.OccasionList = new SelectList(GetOccasionList(), "Value", "Text");

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
                objProductModel.WeightInGms = objProductDetailsModel.objProductMaster.WeightInGms;
                objProductModel.HeightInInch = objProductDetailsModel.objProductMaster.HeightInInch;
                objProductModel.WidthInInch = objProductDetailsModel.objProductMaster.WidthInInch;
                objProductModel.Price = objProductDetailsModel.objProductMaster.Price;
                objProductModel.IsOffer = objProductDetailsModel.objProductMaster.IsOffer;
                objProductModel.OfferPrice = objProductDetailsModel.objProductMaster.OfferPrice;
                objProductModel.IsActive = objProductDetailsModel.objProductMaster.IsActive;
                objProductModel.Description = objProductDetailsModel.objProductMaster.Description;
                objProductModel.OccasionId = objProductDetailsModel.objProductMaster.OccasionId;
                objProductModel.OfferStartDate = objProductDetailsModel.objProductMaster.OfferStartDate;
                objProductModel.OfferEndDate = objProductDetailsModel.objProductMaster.OfferEndDate;
                objProductModel.MakingChargePercentage = objProductDetailsModel.objProductMaster.MakingChargePercentage;
                objProductModel.MakingCharge = objProductDetailsModel.objProductMaster.MakingCharge;
                objProductModel.IsMakingChargePercentage = objProductDetailsModel.objProductMaster.IsMakingChargePercentage;
                objProductModel.MetalVariantId = objProductDetailsModel.objProductMaster.MetalVariantId;
                objProductModel.GemVariantId = objProductDetailsModel.objProductMaster.GemVariantId;
                objProductModel.AmazonUrl = objProductDetailsModel.objProductMaster.AmazonUrl;
                objProductModel.eBayUrl = objProductDetailsModel.objProductMaster.eBayUrl;

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
                    objProductModel.ImagePathList = objProductDetailsModel.objProductMaster.Images.Select(x => x.ImagePath).ToList();
                    objProductModel.Images = objProductDetailsModel.objProductMaster.Images;
                }
            }
            return View(objProductModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductMaster objProductMaster)
        {
            string errorMessage = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (objProductMaster.ProductID > 0)
                    {
                        if (Session["ExistingImg"] != null)
                        {
                            List<DataAccessLayer.Image> existingImageList = new List<DataAccessLayer.Image>();
                            existingImageList = ((ICollection<DataAccessLayer.Image>)Session["ExistingImg"])
                                                                                   .Select(x => new DataAccessLayer.Image
                                                                                   {
                                                                                       ImageID = x.ImageID,
                                                                                       ImageName = x.ImageName,
                                                                                       ImagePath = x.ImagePath,
                                                                                       ProductID = x.ProductID
                                                                                   }).ToList();
                            objProductMaster.Images = existingImageList;
                        }
                        Session.Remove("Images");
                        Session.Remove("ExistingImg");

                        if (imageUrlList != null)
                        {
                            var imageList = new List<DataAccessLayer.Image>();
                            foreach (var image in imageUrlList)
                            {
                                var img = new DataAccessLayer.Image { ProductID = objProductMaster.ProductID };
                                img.ImageName = image.Key;
                                img.ImagePath = GetUploadImageUrl(image.Value, objProductMaster.ProductCode);
                                objProductMaster.Images.Add(img);
                            }
                        }

                        List<DataAccessLayer.Image> images = new List<DataAccessLayer.Image>(objProductMaster.Images);
                        DataTable imgDataTable = ConversionHelper.ConvertToDataTable(images);
                        imgDataTable.Columns.Remove("ProductMaster");

                        SqlParameter imgDetails = new SqlParameter
                        {
                            ParameterName = "ImgDetails",
                            Value = imgDataTable,
                            SqlDbType = SqlDbType.Structured,
                            TypeName = " [dbo].[ImgTable]"
                        };
                        SqlParameter productID = new SqlParameter
                        {
                            ParameterName = "ProductID",
                            Value = objProductMaster.ProductID,
                            SqlDbType = SqlDbType.BigInt
                        };
                        SqlParameter productCode = new SqlParameter
                        {
                            ParameterName = "ProductCode",
                            Value = objProductMaster.ProductCode,
                            SqlDbType = SqlDbType.VarChar
                        };
                        SqlParameter productName = new SqlParameter
                        {
                            ParameterName = "ProductName",
                            Value = objProductMaster.ProductName,
                            SqlDbType = SqlDbType.VarChar
                        };
                        SqlParameter productTypeID = new SqlParameter
                        {
                            ParameterName = "ProductTypeID",
                            Value = objProductMaster.ProductTypeID,
                            SqlDbType = SqlDbType.Int
                        };
                        SqlParameter weightInGms = new SqlParameter
                        {
                            ParameterName = "WeightInGms",
                            Value = objProductMaster.WeightInGms,
                            SqlDbType = SqlDbType.Decimal
                        };
                        SqlParameter heightInInch = new SqlParameter
                        {
                            ParameterName = "HeightInInch",
                            Value = objProductMaster.HeightInInch,
                            SqlDbType = SqlDbType.VarChar
                        };
                        if (objProductMaster.HeightInInch == null)
                        {
                            heightInInch.Value = DBNull.Value;
                        }
                        SqlParameter widthInInch = new SqlParameter
                        {
                            ParameterName = "WidthInInch",
                            Value = objProductMaster.WidthInInch,
                            SqlDbType = SqlDbType.VarChar
                        };
                        if (objProductMaster.WidthInInch == null)
                        {
                            widthInInch.Value = DBNull.Value;
                        }
                        SqlParameter price = new SqlParameter
                        {
                            ParameterName = "Price",
                            Value = objProductMaster.Price,
                            SqlDbType = SqlDbType.Decimal
                        };
                        SqlParameter isOffer = new SqlParameter
                        {
                            ParameterName = "IsOffer",
                            Value = objProductMaster.IsOffer,
                            SqlDbType = SqlDbType.Bit
                        };
                        SqlParameter offerPrice = new SqlParameter
                        {
                            ParameterName = "OfferPrice",
                            Value = objProductMaster.OfferPrice,
                            SqlDbType = SqlDbType.Decimal
                        };
                        if (objProductMaster.OfferPrice == null)
                        {
                            offerPrice.Value = DBNull.Value;
                        }
                        SqlParameter isActive = new SqlParameter
                        {
                            ParameterName = "IsActive",
                            Value = objProductMaster.IsActive,
                            SqlDbType = SqlDbType.Bit
                        };
                        SqlParameter description = new SqlParameter
                        {
                            ParameterName = "Description",
                            Value = objProductMaster.Description,
                            SqlDbType = SqlDbType.VarChar
                        };
                        SqlParameter occasionId = new SqlParameter
                        {
                            ParameterName = "OccasionId",
                            Value = objProductMaster.OccasionId,
                            SqlDbType = SqlDbType.Int
                        };
                        if (objProductMaster.OccasionId == null)
                        {
                            occasionId.Value = DBNull.Value;
                        }
                        SqlParameter offerStartDate = new SqlParameter
                        {
                            ParameterName = "OfferStartDate",
                            Value = objProductMaster.OfferStartDate,
                            SqlDbType = SqlDbType.DateTime
                        };
                        if (objProductMaster.OfferStartDate == null)
                        {
                            offerStartDate.Value = DBNull.Value;
                        }
                        SqlParameter offerEndDate = new SqlParameter
                        {
                            ParameterName = "OfferEndDate",
                            Value = objProductMaster.OfferEndDate,
                            SqlDbType = SqlDbType.DateTime
                        };
                        if (objProductMaster.OfferEndDate == null)
                        {
                            offerEndDate.Value = DBNull.Value;
                        }
                        SqlParameter makingChargePercentage = new SqlParameter
                        {
                            ParameterName = "MakingChargePercentage",
                            Value = objProductMaster.MakingChargePercentage,
                            SqlDbType = SqlDbType.Decimal
                        };
                        if (objProductMaster.MakingChargePercentage == null)
                        {
                            makingChargePercentage.Value = DBNull.Value;
                        }
                        SqlParameter makingCharge = new SqlParameter
                        {
                            ParameterName = "MakingCharge",
                            Value = objProductMaster.MakingCharge,
                            SqlDbType = SqlDbType.Decimal
                        };
                        if (objProductMaster.MakingCharge == null)
                        {
                            makingCharge.Value = DBNull.Value;
                        }
                        SqlParameter isMakingChargePercentage = new SqlParameter
                        {
                            ParameterName = "IsMakingChargePercentage",
                            Value = objProductMaster.IsMakingChargePercentage,
                            SqlDbType = SqlDbType.Bit
                        };
                        SqlParameter metalVariantId = new SqlParameter
                        {
                            ParameterName = "MetalVariantId",
                            Value = objProductMaster.MetalVariantId,
                            SqlDbType = SqlDbType.Int
                        };
                        if (objProductMaster.MetalVariantId == null)
                        {
                            metalVariantId.Value = DBNull.Value;
                        }
                        SqlParameter gemVariantId = new SqlParameter
                        {
                            ParameterName = "GemVariantId",
                            Value = objProductMaster.GemVariantId,
                            SqlDbType = SqlDbType.Int
                        };
                        if (objProductMaster.GemVariantId == null)
                        {
                            gemVariantId.Value = DBNull.Value;
                        }
                        SqlParameter amazonUrl = new SqlParameter
                        {
                            ParameterName = "AmazonUrl",
                            Value = objProductMaster.AmazonUrl,
                            SqlDbType = SqlDbType.VarChar
                        };
                        if (objProductMaster.AmazonUrl == null)
                        {
                            amazonUrl.Value = DBNull.Value;
                        }
                        SqlParameter eBayUrl = new SqlParameter
                        {
                            ParameterName = "eBayUrl",
                            Value = objProductMaster.eBayUrl,
                            SqlDbType = SqlDbType.VarChar
                        };
                        if (objProductMaster.eBayUrl == null)
                        {
                            eBayUrl.Value = DBNull.Value;
                        }
                        object[] parameters = new object[] { productID, productCode, productName, productTypeID, weightInGms, heightInInch, widthInInch, price, isOffer, offerPrice, isActive, description, occasionId, offerStartDate, offerEndDate, makingChargePercentage, makingCharge, isMakingChargePercentage, metalVariantId, gemVariantId, imgDetails, amazonUrl, eBayUrl };

                        objAsopalavDBEntities.Database.ExecuteSqlCommand("EXEC [dbo].[AddUpdateProduct] @ProductID, @ProductCode, @ProductName ,@ProductTypeID, @WeightInGms, @HeightInInch, @WidthInInch, @Price, @IsOffer, @OfferPrice, @IsActive, @Description, @OccasionId, @OfferStartDate, @OfferEndDate, @MakingChargePercentage, @MakingCharge, @IsMakingChargePercentage, @MetalVariantId, @GemVariantId,@ImgDetails,@AmazonUrl,@eBayUrl", parameters);
                    }
                    else
                    {
                        var metalName = (objAsopalavDBEntities.MetalVariants.Where(m => m.MetalVariantId == objProductMaster.MetalVariantId)
                                                                            .Select(m => m.Name)).FirstOrDefault();
                        string gemName = (objAsopalavDBEntities.GemVariants.Where(m => m.GemVariantId == objProductMaster.GemVariantId)
                                                                            .Select(m => m.Name)).FirstOrDefault();

                        if (!(String.IsNullOrEmpty(gemName)))
                        {
                            var prodCountG = objAsopalavDBEntities.ProductMasters.Where(p => p.MetalVariant.Name == metalName &&
                                                                                        p.GemVariant.Name == gemName &&
                                                                                        p.IsActive == true).Count();
                            objProductMaster.ProductCode = "AJ" + metalName[0] + gemName[0] + "000" + Convert.ToString(prodCountG + 1);
                        }
                        else
                        {
                            var prodCountM = objAsopalavDBEntities.ProductMasters.Where(p => p.MetalVariant.Name == metalName && p.IsActive == true).Count();
                            objProductMaster.ProductCode = "AJ" + metalName[0] + "000" + Convert.ToString(prodCountM + 1);
                        }

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
                    }
                    TempData["isProductSaved"] = "true";
                    TempData["ProductSaveMsg"] = "Record Saved. (Item Name : " + objProductMaster.ProductCode + " )";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in ModelState.Keys)
                    {
                        if (ModelState[item].Errors.Count > 0)
                        {
                            errorMessage += ModelState[item].Errors.ToList()[0].ErrorMessage;
                            TempData["isProductSaved"] = "false";
                            TempData["ProductSaveMsg"] = errorMessage;
                        }
                    }
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
            ViewData["OccasionId"] = GetOccasionList();
            ViewData["MetalVariantId"] = GetMetalList();
            ViewData["GemVariantId"] = GetGemList();
            return View(objProductModel);
        }

        [HttpPost]
        [Route("~/Admin/Product/UploadImage")]
        public ActionResult Upload(List<string> imgs)
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
                ViewData["OccasionId"] = GetOccasionList();
                ViewData["MetalVariantId"] = GetMetalList();
                ViewData["GemVariantId"] = GetGemList();
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
                                   where producttype.IsActive == true && !producttype.ProductType.Contains("Gifts") && !producttype.ProductType.Contains("Silverware")
                                   select new SelectListItem()
                                   {
                                       Value = producttype.ProductTypeID.ToString(),
                                       Text = producttype.ProductType
                                   }).OrderBy(x => x.Text).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listProductType;
        }

        private List<SelectListItem> GetOccasionList()
        {
            List<SelectListItem> listOccasion = new List<SelectListItem>();
            try
            {
                listOccasion = (from o in objAsopalavDBEntities.OccasionMasters
                                where o.IsActive == true
                                select new SelectListItem()
                                {
                                    Value = o.OccasionId.ToString(),
                                    Text = o.Occasion
                                }).OrderBy(x => x.Text).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listOccasion;
        }

        private List<SelectListItem> GetMetalList()
        {
            List<SelectListItem> listMetal = new List<SelectListItem>();
            try
            {
                listMetal = (from m in objAsopalavDBEntities.MetalVariants
                             where m.IsActive == true
                             select new SelectListItem()
                             {
                                 Value = m.MetalVariantId.ToString(),
                                 Text = m.Name
                             }).OrderBy(x => x.Text).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listMetal;
        }

        private List<SelectListItem> GetGemList()
        {
            List<SelectListItem> listGem = new List<SelectListItem>();
            try
            {
                listGem = (from g in objAsopalavDBEntities.GemVariants
                           where g.IsActive == true
                           select new SelectListItem()
                           {
                               Value = g.GemVariantId.ToString(),
                               Text = g.Name
                           }).OrderBy(x => x.Text).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listGem;
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
            var prodList = objAsopalavDBEntities.ProductMasters.Where(x => x.IsActive == true).OrderByDescending(x => x.CreationDate).Select(x => new ProductListModel
            {
                //CreationDate = x.CreationDate,
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
            }).ToList();

            if (prodList != null)
            {
                return Json(new
                {
                    data = prodList
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    data = ""
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetImageList()
        {
            List<DataAccessLayer.Image> imageList = new List<DataAccessLayer.Image>();

            if (Session["Images"] != null)
            {
                imageList = ((ICollection<DataAccessLayer.Image>)Session["Images"]).Select(x => new DataAccessLayer.Image
                {
                    ImageID = x.ImageID,
                    ImageName = x.ImageName,
                    ImagePath = x.ImagePath,
                    ProductID = x.ProductID
                }).ToList();
                //Session.Remove("Images");
                objProductModel.Images = imageList;
            }
            else
            {
                imageList = null;
            }
            return Json(new { Data = imageList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMetalPrice(string metalName)
        {
            var metalPrice = (from m in objAsopalavDBEntities.MetalVariants where m.Name == metalName select m.UnitSellPrice).FirstOrDefault();
            return Json(new { data = metalPrice }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGemPrice(string gemName)
        {
            var gemPrice = (objAsopalavDBEntities.GemVariants.Where(m => m.Name == gemName).Select(m => m.Price)).FirstOrDefault();
            return Json(new { data = gemPrice }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetExistingImages(List<string> imgNames)
        {
            List<DataAccessLayer.Image> imageList = new List<DataAccessLayer.Image>();

            if (Session["Images"] != null)
            {
                imageList = ((ICollection<DataAccessLayer.Image>)Session["Images"])
                                  .Where(i => imgNames.Contains(i.ImageName))
                                  .Select(x => new DataAccessLayer.Image
                                  {
                                      ImageID = x.ImageID,
                                      ImageName = x.ImageName,
                                      ImagePath = x.ImagePath,
                                      ProductID = x.ProductID
                                  }).ToList();
            }
            Session["ExistingImg"] = imageList;
            return RedirectToAction("Add");
        }
    }
}