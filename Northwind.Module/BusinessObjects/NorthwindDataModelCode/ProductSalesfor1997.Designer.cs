﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace Northwind.Module.BusinessObjects.Northwind
{

    [NonPersistent]
    public partial class ProductSalesfor1997 : XPLiteObject
    {
        string fCategoryName;
        [Size(15)]
        public string CategoryName
        {
            get { return fCategoryName; }
            set { SetPropertyValue<string>(nameof(CategoryName), ref fCategoryName, value); }
        }
        string fProductName;
        [Size(40)]
        public string ProductName
        {
            get { return fProductName; }
            set { SetPropertyValue<string>(nameof(ProductName), ref fProductName, value); }
        }
        decimal fProductSales;
        public decimal ProductSales
        {
            get { return fProductSales; }
            set { SetPropertyValue<decimal>(nameof(ProductSales), ref fProductSales, value); }
        }
    }

}