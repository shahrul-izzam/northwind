using System;
using System.Web.UI;
using DevExpress.Xpo;
using Northwind.Module.BusinessObjects;
using Northwind.Module.BusinessObjects.Northwind;

namespace Northwind.Web
{
    public partial class CustomerViewControl : UserControl, ICustomerView
    {
        private string _companyName;
        private string _city;
        private XPCollection<Orders> _orders;
        public event EventHandler Changed;
        private bool isInitialized;

        protected void Page_Load(object sender, EventArgs e)
        {
            isInitialized = true;
            UpdateCustomer();
            UpdateOrdersControl();
            this.Changed += OnChanged;
        }

        private void OnChanged(object sender, EventArgs e)
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public string ShipName
        {
            get { return _companyName; }
            set
            {
                _companyName = value;
                UpdateCustomer();
            }
        }

        public string ShipAddress
        {
            get { return _city; }
            set
            {
                _city = value;
                UpdateCustomer();
            }
        }

        public XPCollection<Orders> OrdersCollection
        {
            get { return _orders; }
            set
            {
                _orders = value;
                UpdateOrdersControl();
            }
        }

        private void UpdateCustomer()
        {
            if(!isInitialized) return;
            CompanyName_TextBox.Text = ShipName;
            City_TextBox.Text = ShipAddress;
        }
        
        private void UpdateOrdersControl() {
            if(!isInitialized) return;
            OrdersCollection_GridView.DataSource = OrdersCollection;
            OrdersCollection_GridView.DataBind();
        }
    }
}