using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using Northwind.Module.BusinessObjects.Northwind;

namespace Northwind.Module.Web.Controllers
{
    public class ApplyFiltering : ObjectViewController<ListView, Employees>
    {
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (View.Editor is ASPxGridListEditor editor)
            {
                editor.Grid.FilterExpression = "[FirstName] = 'Andrew'";
                editor.Grid.SettingsText.Title = editor.Grid.FilterExpression;
            }
        }
    }
}
