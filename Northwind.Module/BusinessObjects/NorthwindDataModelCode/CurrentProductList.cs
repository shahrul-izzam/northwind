﻿using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace Northwind.Module.BusinessObjects.Northwind
{

    public partial class CurrentProductList
    {
        public CurrentProductList(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
