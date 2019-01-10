using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.Persistent.AuditTrail;

namespace Northwind.Module.BusinessObjects.Northwind
{

    public partial class Customers
    {
        public Customers(Session session) : base(session) { }

        [NonPersistent]
        public Customers ClonedMember { get; set; }
        
        protected override void OnLoaded()
        {
            base.OnLoaded();
            ClonedMember = (Customers)MemberwiseClone();
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        
        public int ModuleId => 1;
    }

}
