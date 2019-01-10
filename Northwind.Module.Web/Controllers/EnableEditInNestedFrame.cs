using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web.Templates;
using Northwind.Module.Web.Editors;

namespace Northwind.Module.Web.Controllers
{
    public class EnableEditInNestedFrame : ViewController<ListView>
    {
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

            if (View.Editor is CustomComplexListEditor complexListEditor)
            {
                if (((DetailView)((NestedFrame)Frame).ViewItem.View).ViewEditMode == ViewEditMode.Edit)
                {
                    foreach (var c in complexListEditor.Control.Controls)
                    {
                        ((DetailView)((NestedFrameControlNew)c).View).ViewEditMode = ((DetailView)((NestedFrame)Frame).ViewItem.View).ViewEditMode;
                        
                    }
                }
                else
                {
                    foreach (var c in complexListEditor.Control.Controls)
                    {
                        ((DetailView)((NestedFrameControlNew)c).View).ViewEditMode = ViewEditMode.View;
                    }
                }
            }
        }
        
    }
}
