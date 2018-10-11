﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        //[Required(ErrorMessage = "Product Dimension is required")]
        //[DisplayName("Height X Width(Inch)")]
        public string HeightInInch { get; set; }
        public string WidthInInch { get; set; }

        [Required(ErrorMessage = "Price is required")]
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

        public List<string> ImagePathList { get; set; }
    }

    public class ProductTypeModel
    {
        public int ProductTypeID { get; set; }
        public string ProductType { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProductListModel
    {
        public ProductListModel()
        {
            this.ImagePaths = new List<ImageModel>();
            this.CreationDate = DateTime.Now;
        }
        public long ProductId { get; set; }
        [DisplayName("Product Code")]
        public string ProductCode { get; set; }
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [DisplayName("Product Type")]
        public string ProductType { get; set; }
        public int ProductTypeId { get; set; }
        [DisplayName("Weight(Gram)")]
        public decimal WeightInGms { get; set; }
        public string HeightInInch { get; set; }
        public string WidthInInch { get; set; }
        public decimal Price { get; set; }
        [DisplayName("Set Offer")]
        public bool IsOffer { get; set; }
        public decimal? OfferPrice { get; set; }
        
        [DisplayName("Set Active")]
        public bool IsActive { get; set; }
        public string Description { get; set; }
        [DisplayName("Dimension Height X Width(Inch)")]
        public string Dimension
        {
            get
            {
                return String.Format("{0} X {1}", this.HeightInInch, this.WidthInInch);
            }
        }

        public DateTime CreationDate { get; set; }

        public List<ImageModel> ImagePaths { get; set; }
    }

    public class ImageModel
    {
        public int ImageID { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
    }
}