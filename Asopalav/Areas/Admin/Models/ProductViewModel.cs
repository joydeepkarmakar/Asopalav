using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Asopalav.Models
{
    public class ProductModel
    {
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
        public string Height_width_inch { get; set; }

        public int? Price { get; set; }

        [Required(ErrorMessage = "Set Offer is required")]
        [DisplayName("Set Offer")]
        public bool IsOffer { get; set; }

        [Required(ErrorMessage = "Offer Price is required")]
        [DisplayName("Offer Price")]
        public decimal OfferPrice { get; set; }

        [Required(ErrorMessage = "Small Image Path is required")]
        [DisplayName("Small Image Path")]
        public string SmallImage { get; set; }

        [Required(ErrorMessage = "Large Image Path is required")]
        [DisplayName("Large Image Path")]
        public string BigImage { get; set; }

        [Required(ErrorMessage = "Active is required")]
        [DisplayName("Active")]
        public bool IsActive { get; set; }

        public string Description { get; set; }

        public List<SelectListItem> ProductType { get; set; }
    }

    public class ProductTypeModel
    {
        public int ProductTypeID { get; set; }
        public string ProductType { get; set; }
        public bool IsActive { get; set; }
    }
}