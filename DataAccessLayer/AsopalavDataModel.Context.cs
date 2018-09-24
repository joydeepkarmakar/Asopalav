﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccessLayer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class AsopalavDBEntities : DbContext
    {
        public AsopalavDBEntities()
            : base("name=AsopalavDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<MenuMaster> MenuMasters { get; set; }
        public virtual DbSet<ProductTypeMaster> ProductTypeMasters { get; set; }
        public virtual DbSet<RoleMaster> RoleMasters { get; set; }
        public virtual DbSet<RoleMenuPermission> RoleMenuPermissions { get; set; }
        public virtual DbSet<UserAddressDetail> UserAddressDetails { get; set; }
        public virtual DbSet<UserProfileMaster> UserProfileMasters { get; set; }
        public virtual DbSet<FeedbackMaster> FeedbackMasters { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<ProductMaster> ProductMasters { get; set; }
        public virtual DbSet<MarketTracker> MarketTrackers { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<CartDetail> CartDetails { get; set; }
    
        public virtual int AddUser(string primary_Email, string password, string user_Fname, string user_Mname, string user_Lname, string secondary_Email, string mobile, string alternate_Mobile, string gender, Nullable<System.DateTime> user_DOB, Nullable<System.DateTime> user_Anniversary)
        {
            var primary_EmailParameter = primary_Email != null ?
                new ObjectParameter("Primary_Email", primary_Email) :
                new ObjectParameter("Primary_Email", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var user_FnameParameter = user_Fname != null ?
                new ObjectParameter("User_Fname", user_Fname) :
                new ObjectParameter("User_Fname", typeof(string));
    
            var user_MnameParameter = user_Mname != null ?
                new ObjectParameter("User_Mname", user_Mname) :
                new ObjectParameter("User_Mname", typeof(string));
    
            var user_LnameParameter = user_Lname != null ?
                new ObjectParameter("User_Lname", user_Lname) :
                new ObjectParameter("User_Lname", typeof(string));
    
            var secondary_EmailParameter = secondary_Email != null ?
                new ObjectParameter("Secondary_Email", secondary_Email) :
                new ObjectParameter("Secondary_Email", typeof(string));
    
            var mobileParameter = mobile != null ?
                new ObjectParameter("Mobile", mobile) :
                new ObjectParameter("Mobile", typeof(string));
    
            var alternate_MobileParameter = alternate_Mobile != null ?
                new ObjectParameter("Alternate_Mobile", alternate_Mobile) :
                new ObjectParameter("Alternate_Mobile", typeof(string));
    
            var genderParameter = gender != null ?
                new ObjectParameter("Gender", gender) :
                new ObjectParameter("Gender", typeof(string));
    
            var user_DOBParameter = user_DOB.HasValue ?
                new ObjectParameter("User_DOB", user_DOB) :
                new ObjectParameter("User_DOB", typeof(System.DateTime));
    
            var user_AnniversaryParameter = user_Anniversary.HasValue ?
                new ObjectParameter("User_Anniversary", user_Anniversary) :
                new ObjectParameter("User_Anniversary", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddUser", primary_EmailParameter, passwordParameter, user_FnameParameter, user_MnameParameter, user_LnameParameter, secondary_EmailParameter, mobileParameter, alternate_MobileParameter, genderParameter, user_DOBParameter, user_AnniversaryParameter);
        }
    
        public virtual ObjectResult<ValidateUserAndMenu_Result> ValidateUserAndMenu(string userName, string password)
        {
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ValidateUserAndMenu_Result>("ValidateUserAndMenu", userNameParameter, passwordParameter);
        }
    
        public virtual int AddUpdateProduct(Nullable<long> productID, string productCode, string productName, Nullable<int> productTypeID, Nullable<decimal> weightInGms, string heightInInch, string widthInInch, Nullable<decimal> price, Nullable<bool> isOffer, Nullable<decimal> offerPrice, Nullable<bool> isActive, string description, Nullable<System.DateTime> modifyDate)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(long));
    
            var productCodeParameter = productCode != null ?
                new ObjectParameter("ProductCode", productCode) :
                new ObjectParameter("ProductCode", typeof(string));
    
            var productNameParameter = productName != null ?
                new ObjectParameter("ProductName", productName) :
                new ObjectParameter("ProductName", typeof(string));
    
            var productTypeIDParameter = productTypeID.HasValue ?
                new ObjectParameter("ProductTypeID", productTypeID) :
                new ObjectParameter("ProductTypeID", typeof(int));
    
            var weightInGmsParameter = weightInGms.HasValue ?
                new ObjectParameter("WeightInGms", weightInGms) :
                new ObjectParameter("WeightInGms", typeof(decimal));
    
            var heightInInchParameter = heightInInch != null ?
                new ObjectParameter("HeightInInch", heightInInch) :
                new ObjectParameter("HeightInInch", typeof(string));
    
            var widthInInchParameter = widthInInch != null ?
                new ObjectParameter("WidthInInch", widthInInch) :
                new ObjectParameter("WidthInInch", typeof(string));
    
            var priceParameter = price.HasValue ?
                new ObjectParameter("Price", price) :
                new ObjectParameter("Price", typeof(decimal));
    
            var isOfferParameter = isOffer.HasValue ?
                new ObjectParameter("IsOffer", isOffer) :
                new ObjectParameter("IsOffer", typeof(bool));
    
            var offerPriceParameter = offerPrice.HasValue ?
                new ObjectParameter("OfferPrice", offerPrice) :
                new ObjectParameter("OfferPrice", typeof(decimal));
    
            var isActiveParameter = isActive.HasValue ?
                new ObjectParameter("IsActive", isActive) :
                new ObjectParameter("IsActive", typeof(bool));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var modifyDateParameter = modifyDate.HasValue ?
                new ObjectParameter("ModifyDate", modifyDate) :
                new ObjectParameter("ModifyDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddUpdateProduct", productIDParameter, productCodeParameter, productNameParameter, productTypeIDParameter, weightInGmsParameter, heightInInchParameter, widthInInchParameter, priceParameter, isOfferParameter, offerPriceParameter, isActiveParameter, descriptionParameter, modifyDateParameter);
        }
    
        public virtual ObjectResult<GetDollarSilverRate_Result> GetDollarSilverRate()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetDollarSilverRate_Result>("GetDollarSilverRate");
        }
    
        public virtual int GetDailyDollarSilverRate()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetDailyDollarSilverRate");
        }
    
        public virtual int SilverRate()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SilverRate");
        }
    
        [DbFunction("AsopalavDBEntities", "SplitString")]
        public virtual IQueryable<string> SplitString(string input, string character)
        {
            var inputParameter = input != null ?
                new ObjectParameter("Input", input) :
                new ObjectParameter("Input", typeof(string));
    
            var characterParameter = character != null ?
                new ObjectParameter("Character", character) :
                new ObjectParameter("Character", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<string>("[AsopalavDBEntities].[SplitString](@Input, @Character)", inputParameter, characterParameter);
        }
    
        public virtual ObjectResult<GetLastAddedProducts_Result> GetLastAddedProducts(string currentCurrency, string conversionRate)
        {
            var currentCurrencyParameter = currentCurrency != null ?
                new ObjectParameter("CurrentCurrency", currentCurrency) :
                new ObjectParameter("CurrentCurrency", typeof(string));
    
            var conversionRateParameter = conversionRate != null ?
                new ObjectParameter("ConversionRate", conversionRate) :
                new ObjectParameter("ConversionRate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetLastAddedProducts_Result>("GetLastAddedProducts", currentCurrencyParameter, conversionRateParameter);
        }
    
        public virtual ObjectResult<GetProductsByProductType_Result> GetProductsByProductType(string productType, string currentCurrency, string conversionRate)
        {
            var productTypeParameter = productType != null ?
                new ObjectParameter("ProductType", productType) :
                new ObjectParameter("ProductType", typeof(string));
    
            var currentCurrencyParameter = currentCurrency != null ?
                new ObjectParameter("CurrentCurrency", currentCurrency) :
                new ObjectParameter("CurrentCurrency", typeof(string));
    
            var conversionRateParameter = conversionRate != null ?
                new ObjectParameter("ConversionRate", conversionRate) :
                new ObjectParameter("ConversionRate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetProductsByProductType_Result>("GetProductsByProductType", productTypeParameter, currentCurrencyParameter, conversionRateParameter);
        }
    }
}
