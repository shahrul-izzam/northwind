using System;
using DevExpress.Xpo;
using Northwind.Module.BusinessObjects.Northwind;

namespace Northwind.Module.BusinessObjects
{
    public interface ICustomerView
    {
        string ShipName { get; set; }
        string ShipAddress { get; set; }
        XPCollection<Orders> OrdersCollection { get; set; }
        event EventHandler Changed;
    }
}
