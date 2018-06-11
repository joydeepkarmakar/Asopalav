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

        [Route("")]
        public ActionResult Index()
        {
            ViewData["ProductTypeID"] = GetProductTypeList();
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
                            var img = HardResizeImage(1024, 1024, CreateImage(original, 0, 0, original.Width, original.Height));
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
            listProductType = (from producttype in objAsopalavDBEntities.ProductTypeMasters
                               select new SelectListItem()
                               {
                                   Value = producttype.ProductTypeID.ToString(),
                                   Text = producttype.ProductType
                               }).ToList();
            return listProductType;
        }

        private string GetUploadImageUrl(string tempImagePath, string productCode)
        {
            string uploadImageUrl = string.Empty;
            var uploadFolder = "~/Uploads/" + productCode.Replace(" ", "_").Replace("/", "_").Replace(":", "_").ToLower();
            DirectoryInfo di = new DirectoryInfo(Server.MapPath(uploadFolder));
            if (!di.Exists)
            {
                di.Create();
            }
            if (tempImagePath != "")
            {
                var actfileName = GetUrlFileName(tempImagePath);
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
            return uploadImageUrl;
        }

        /// <summary>
        /// Gets an image from the specified URL.
        /// </summary>
        /// <param name="url">The URL containing an image.</param>
        /// <returns>The image as a bitmap.</returns>
        Bitmap GetImageFromUrl(string url)
        {
            var buffer = 1024;
            Bitmap image = null;

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                return image;

            using (var ms = new MemoryStream())
            {
                var req = WebRequest.Create(url);

                using (var resp = req.GetResponse())
                {
                    using (var stream = resp.GetResponseStream())
                    {
                        var bytes = new byte[buffer];
                        var n = 0;

                        while ((n = stream.Read(bytes, 0, buffer)) != 0)
                            ms.Write(bytes, 0, n);
                    }
                }

                image = Bitmap.FromStream(ms) as Bitmap;
            }

            return image;
        }

        /// <summary>
        /// Gets the filename that is placed under a certain URL.
        /// </summary>
        /// <param name="url">The URL which should be investigated for a file name.</param>
        /// <returns>The file name.</returns>
        string GetUrlFileName(string url)
        {
            var parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var last = parts[parts.Length - 1];
            return Path.GetFileNameWithoutExtension(last);
        }

        /// <summary>
        /// Creates a small image out of a larger image.
        /// </summary>
        /// <param name="original">The original image which should be cropped (will remain untouched).</param>
        /// <param name="x">The value where to start on the x axis.</param>
        /// <param name="y">The value where to start on the y axis.</param>
        /// <param name="width">The width of the final image.</param>
        /// <param name="height">The height of the final image.</param>
        /// <returns>The cropped image.</returns>
        Bitmap CreateImage(Bitmap original, int x, int y, int width, int height)
        {
            var img = new Bitmap(width, height);

            using (var g = Graphics.FromImage(img))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(original, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);
            }

            return img;
        }

        //Overload for crop that default starts top left of the image.
        public static System.Drawing.Image CropImage(System.Drawing.Image Image, int Height, int Width)
        {
            return CropImage(Image, Height, Width, 0, 0);
        }

        //The crop image sub
        public static System.Drawing.Image CropImage(System.Drawing.Image Image, int Height, int Width, int StartAtX, int StartAtY)
        {
            System.Drawing.Image outimage;
            MemoryStream mm = null;
            try
            {
                //check the image height against our desired image height
                if (Image.Height < Height)
                {
                    Height = Image.Height;
                }

                if (Image.Width < Width)
                {
                    Width = Image.Width;
                }

                //create a bitmap window for cropping
                Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(72, 72);

                //create a new graphics object from our image and set properties
                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

                //now do the crop
                grPhoto.DrawImage(Image, new Rectangle(0, 0, Width, Height), StartAtX, StartAtY, Width, Height, GraphicsUnit.Pixel);

                // Save out to memory and get an image from it to send back out the method.
                mm = new MemoryStream();
                bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Jpeg);
                Image.Dispose();
                bmPhoto.Dispose();
                grPhoto.Dispose();
                outimage = System.Drawing.Image.FromStream(mm);

                return outimage;
            }
            catch (Exception ex)
            {
                throw new Exception("Error cropping image, the error was: " + ex.Message);
            }
        }

        //Hard resize attempts to resize as close as it can to the desired size and then crops the excess
        public static System.Drawing.Image HardResizeImage(int Width, int Height, System.Drawing.Image Image)
        {
            int width = Image.Width;
            int height = Image.Height;
            System.Drawing.Image resized = null;
            if (Width > Height)
            {
                resized = ResizeImage(Width, Width, Image);
            }
            else
            {
                resized = ResizeImage(Height, Height, Image);
            }
            System.Drawing.Image output = CropImage(resized, Height, Width);
            //return the original resized image
            return output;
        }

        //Image resizing
        public static System.Drawing.Image ResizeImage(int maxWidth, int maxHeight, System.Drawing.Image Image)
        {
            int width = Image.Width;
            int height = Image.Height;
            if (width > maxWidth || height > maxHeight)
            {
                //The flips are in here to prevent any embedded image thumbnails -- usually from cameras
                //from displaying as the thumbnail image later, in other words, we want a clean
                //resize, not a grainy one.
                Image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipX);
                Image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipX);

                float ratio = 0;
                if (width > height)
                {
                    ratio = (float)width / (float)height;
                    width = maxWidth;
                    height = Convert.ToInt32(Math.Round((float)width / ratio));
                }
                else
                {
                    ratio = (float)height / (float)width;
                    height = maxHeight;
                    width = Convert.ToInt32(Math.Round((float)height / ratio));
                }

                //return the resized image
                return Image.GetThumbnailImage(width, height, null, IntPtr.Zero);
            }
            //return the original resized image
            return Image;
        }
    }
}