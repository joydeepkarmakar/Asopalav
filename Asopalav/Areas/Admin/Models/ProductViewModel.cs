using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asopalav.Models
{
    public class ProductModel
    {
        public long ProductID { get; set; }

        [Required(ErrorMessage = "Product Code is required")]
        [DisplayName("Product Code")]
        public string ProductCode { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Product Type is required")]
        [DisplayName("Product Type")]
        public int? ProductTypeID { get; set; }

        [Required(ErrorMessage = "Product Weight is required")]
        [DisplayName("Weight(Gram)")]
        public decimal WeightInGms { get; set; }

        [Required(ErrorMessage = "Product Dimension is required")]
        [DisplayName("Height X Width(Inch)")]
        public string HeightInInch { get; set; }
        public string WidthInInch { get; set; }

        public decimal Price { get; set; }

        [DisplayName("Set Offer")]
        public bool IsOffer { get; set; }

        [DisplayName("Offer Price")]
        public decimal? OfferPrice { get; set; }

        [Required(ErrorMessage = "Image Path is required")]
        [DisplayName("Upload Image")]
        public string ImagePath { get; set; }

        [Required(ErrorMessage = "Set Active is required")]
        [DisplayName("Set Active")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Product Description is required")]
        public string Description { get; set; }

        public DateTime? ModifyDate { get; set; }

        public List<SelectListItem> ProductType { get; set; }
        public Dictionary<string, string> ImageDetailsList { get; set; }
    }

    public class ProductTypeModel
    {
        public int ProductTypeID { get; set; }
        public string ProductType { get; set; }
        public bool IsActive { get; set; }
    }

    //public class UploadImageModel
    //{
    //    public HttpPostedFileBase File { get; set; }
    //    public int X { get; set; }
    //    public int Y { get; set; }
    //    public int Width { get; set; }
    //    public int Height { get; set; }
    //}
}