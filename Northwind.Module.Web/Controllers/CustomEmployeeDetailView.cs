using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Layout;
using Northwind.Module.BusinessObjects.Northwind;

namespace Northwind.Module.Web.Controllers
{
    public class CustomEmployeeDetailView : ObjectViewController<DetailView, Employees>
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            ((WebLayoutManager)View.LayoutManager).ItemCreated += OnItemCreated;
            View.SelectionChanged += ViewOnSelectionChanged;
        }

        private void OnItemCreated(object sender, ItemCreatedEventArgs e)
        {
            if (e.ModelLayoutElement is IModelLayoutGroup group)
            {
                if (e.ModelLayoutElement.Id == "EmployeesCollection")
                {
                    var employee = ViewCurrentObject;

                    if (employee.City == "London")
                    {
                        group.ImageName = "Action_AboutInfo";
                        group.ToolTip = "London";
                    }
                    else if (employee.City == "Seattle")
                    {
                        group.ImageName = "Action_Bell";
                        group.ToolTip = "Seattle";
                    }
                    else if (employee.City == "Tacoma")
                    {
                        group.ImageName = "Action_Cancel";
                        group.ToolTip = "Tacoma";
                    }   
                }
            }
        }

        private void ViewOnSelectionChanged(object sender, EventArgs e)
        {
            var currentObject = View.CurrentObject;
        }

        protected override void OnDeactivated()
        {
            ((WebLayoutManager)View.LayoutManager).ItemCreated -= OnItemCreated;
            View.SelectionChanged -= ViewOnSelectionChanged;
            base.OnDeactivated();
        }
    }
}
