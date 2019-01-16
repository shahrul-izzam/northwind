using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors;
using DevExpress.Xpo;
using Northwind.Module.BusinessObjects;
using Northwind.Module.BusinessObjects.Northwind;

namespace Northwind.Module.Web.Controllers
{
    public class CustomCustomerDetailView : ObjectViewController<DetailView, Orders>
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            foreach (var item in View.GetItems<WebCustomUserControlViewItem>())
            {
                item.ControlCreated += ItemOnControlCreated;
            }

            View.CurrentObjectChanged += ViewOnCurrentObjectChanged;
        }

        private void ViewOnCurrentObjectChanged(object sender, EventArgs e)
        {
            foreach (var item in View.GetItems<WebCustomUserControlViewItem>())
            {
                if (item.Control is ICustomerView customer)
                {
                    Initialize(customer);
                }
            }
        }

        private void ItemOnControlCreated(object sender, EventArgs e)
        {
            if (((WebCustomUserControlViewItem) sender).Control is ICustomerView customer)
            {
                customer.Changed += CustomerOnChanged;
                Initialize(customer);
            }
        }

        private void CustomerOnChanged(object sender, EventArgs e)
        {
            if (ViewCurrentObject != null)
            {
                ViewCurrentObject.ShipName = ((ICustomerView) sender).ShipName;
                ViewCurrentObject.ShipAddress = ((ICustomerView) sender).ShipAddress;
            }
        }

        protected override void OnDeactivated()
        {
            foreach (var item in View.GetItems<WebCustomUserControlViewItem>())
            {
                item.ControlCreated -= ItemOnControlCreated;
            }

            View.CurrentObjectChanged -= ViewOnCurrentObjectChanged;
            base.OnDeactivated();
        }

        private void Initialize(ICustomerView customer)
        {
            if (ViewCurrentObject != null)
            {
                customer.ShipName = ViewCurrentObject.ShipName;
                customer.ShipAddress = ViewCurrentObject.ShipAddress;
                customer.OrdersCollection = ViewCurrentObject.OrdersCollection;
            }
            else
            {
                customer.ShipName = null;
                customer.ShipAddress = null;
                customer.OrdersCollection = new XPCollection<Orders>();
            }
        }
    }
}
