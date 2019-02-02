using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asopalav.Areas.Admin.Models
{
    public class PriceModel
    {
        public int CurrencyId { get; set; }
        public int? MetalVariantId { get; set; }
        public int? GemVariantId { get; set; }
        public string PriceMeasure { get; set; }
        public decimal PriceValue { get; set; }
    }
}