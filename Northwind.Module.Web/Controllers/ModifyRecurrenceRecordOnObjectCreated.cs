using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.Persistent.Base.General;

namespace Northwind.Module.Web.Controllers
{
    public class ModifyRecurrenceRecordOnObjectCreated : WebNewObjectViewController
    {
        protected override void OnObjectCreated(object newObject, IObjectSpace objectSpace)
        {
            base.OnObjectCreated(newObject, objectSpace);
        }
    }
}
