using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asopalav.Models
{
    public class DashboardModel
    {
        public List<GetLastAddedProducts_Result> objGetLastAddedProducts_Result = new List<GetLastAddedProducts_Result>();
        public List<GetProductsByProductType_Result> objGetProductsByProductType_Result = new List<GetProductsByProductType_Result>();
        public string ProductListPage { get; set; }
    }

    public class ProductDetailsModel
    {
        public ProductMaster objProductMaster = new ProductMaster();
        public string DefaultImagePath { get; set; }
    }

    public class CartVM
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get { return Quantity * Price; } }
        public IEnumerable<Image> Image { get; set; }
    }
}