using Asopalav.Areas.Admin.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Asopalav.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    [RoutePrefix("Price")]
    [Route("{action}")]
    public class PriceController : Controller
    {
        AsopalavDBEntities objAsopalavDBEntities = new AsopalavDBEntities();

        [Route("~/Admin/Price")]
        public ActionResult Index()
        {
            Session["CurrentPage"] = "Prices";

            ProductController productController = new ProductController();
            ViewBag.MetalList = new SelectList(productController.GetMetalList(), "Value", "Text");
            ViewBag.GemList = new SelectList(productController.GetGemList(), "Value", "Text");
            ViewBag.CurrencyList = new SelectList(GetCurrencyList(), "Value", "Text");

            return View();
        }

        public ActionResult GetProdPriceList()
        {
            var prodPriceList = objAsopalavDBEntities.GetProdPriceList();
            return Json(new
            {
                data = prodPriceList
            }, JsonRequestBehavior.AllowGet);
        }

        protected internal List<SelectListItem> GetCurrencyList()
        {
            List<SelectListItem> listCurrency = new List<SelectListItem>();
            try
            {
                listCurrency = (from c in objAsopalavDBEntities.CurrencyMasters
                                where c.IsActive == true
                                select new SelectListItem()
                                {
                                    Value = c.CurrencyId.ToString(),
                                    Text = c.CurrencyCode
                                }).OrderBy(x => x.Text).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listCurrency;
        }

        [HttpPost]
        public JsonResult AddUpdateProdPrice(PriceModel objPriceModel)
        {
            bool status = false;

            try
            {
                SqlParameter currencyId = new SqlParameter
                {
                    ParameterName = "CurrencyId",
                    Value = objPriceModel.CurrencyId,
                    SqlDbType = SqlDbType.Int
                };

                SqlParameter metalVariantId = new SqlParameter
                {
                    ParameterName = "MetalId",
                    Value = objPriceModel.MetalVariantId,
                    SqlDbType = SqlDbType.Int
                };
                if (objPriceModel.MetalVariantId == null)
                {
                    metalVariantId.Value = DBNull.Value;
                }

                SqlParameter gemVariantId = new SqlParameter
                {
                    ParameterName = "GemId",
                    Value = objPriceModel.GemVariantId,
                    SqlDbType = SqlDbType.Int
                };
                if (objPriceModel.GemVariantId == null)
                {
                    gemVariantId.Value = DBNull.Value;
                }

                SqlParameter priceMeasure = new SqlParameter
                {
                    ParameterName = "PriceMeasure",
                    Value = objPriceModel.PriceMeasure,
                    SqlDbType = SqlDbType.VarChar
                };

                SqlParameter priceValue = new SqlParameter
                {
                    ParameterName = "PriceValue",
                    Value = objPriceModel.PriceValue,
                    SqlDbType = SqlDbType.Decimal
                };

                object[] parameters = new object[] { priceValue, priceMeasure, currencyId, metalVariantId, gemVariantId };

                objAsopalavDBEntities.Database.ExecuteSqlCommand("EXEC [dbo].[AddUpdatePrice] @PriceValue, @PriceMeasure, @CurrencyId ,@MetalId, @GemId", parameters);

                status = true;
            }
            catch (Exception)
            {
                throw;
            }

            return Json(new
            {
                IsSuccess = status,
            }, JsonRequestBehavior.AllowGet);
        }
    }
}