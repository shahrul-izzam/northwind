using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.ExpressApp.Web.Templates.ActionContainers;
using Northwind.Module.BusinessObjects.Northwind;
using Northwind.Module.Web.Layout;

namespace Northwind.Module.Web.Controllers
{
    public class ApplyCustomHeaderContent : ObjectViewController<DetailView, Customers>
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            if (View.LayoutManager is WebLayoutManager layoutManager)
            {
                layoutManager.LayoutGroupTemplate = new CustomLayoutGroupTemplate();
            
                layoutManager.ItemCreated += OnItemCreated;
                layoutManager.PageControlCreated += OnPageControlCreated;
            }
                
            View.CurrentObjectChanged += ViewOnCurrentObjectChanged;
            View.ControlsCreated += ViewOnControlsCreated;
        }

        private void OnPageControlCreated(object sender, PageControlCreatedEventArgs e)
        {

        }

        private void OnItemCreated(object sender, ItemCreatedEventArgs e)
        {
        }

        private void ViewOnCurrentObjectChanged(object sender, EventArgs e)
        {
            
        }

        private void ViewOnControlsCreated(object sender, EventArgs e)
        {
            foreach (IActionContainer container in Frame.Template.GetContainers())
            {
                if(container is WebActionContainer webContainer && webContainer.ContainerId == "Save") {
                    //webContainer.Owner.Menu.HorizontalAlign = HorizontalAlign.Left;
                }
            }
        }

        protected override void OnDeactivated()
        {
            var layoutManager = (WebLayoutManager) View.LayoutManager;
            layoutManager.ItemCreated -= OnItemCreated;
            layoutManager.PageControlCreated -= OnPageControlCreated;
            View.CurrentObjectChanged -= ViewOnCurrentObjectChanged;
            View.ControlsCreated -= ViewOnControlsCreated;
            base.OnDeactivated();
        }
    }
}
