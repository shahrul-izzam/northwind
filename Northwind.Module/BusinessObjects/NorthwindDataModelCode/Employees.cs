using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;

namespace Northwind.Module.BusinessObjects.Northwind
{

    public partial class Employees
    {
        public Employees(Session session) : base(session) { }

        [NonPersistent]
        public Employees ClonedMember { get; set; }

        public override void AfterConstruction() { base.AfterConstruction(); }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            ClonedMember = (Employees) MemberwiseClone();
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
        }

        protected override void OnDeleting()
        {
            base.OnDeleting();
        }

        public int ModuleId => 2;
    }

}
