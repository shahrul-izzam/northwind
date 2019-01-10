using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Xpo.Metadata;

namespace Northwind.Module.BusinessObjects.Northwind
{

    public partial class Products
    {
        public Products(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        protected override void FireChangedByCustomPropertyStore(XPMemberInfo member, object oldValue, object newValue)
        {
            base.FireChangedByCustomPropertyStore(member, oldValue, newValue);
        }

        protected override void FireChangedByXPPropertyDescriptor(string memberName)
        {
            base.FireChangedByXPPropertyDescriptor(memberName);
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
        }
    }

}
