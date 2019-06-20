using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Scheduler.Web;
using DevExpress.Persistent.Base.General;
using DevExpress.XtraScheduler;

namespace Northwind.Module.Web.Controllers
{
    public class ModifyRecurrenceRecordController: ObjectViewController<ListView, IEvent>
    {
        private ASPxSchedulerListEditor _listEditor;

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            _listEditor = View.Editor as ASPxSchedulerListEditor;
            if (_listEditor != null)
            {
                _listEditor.SchedulerControl.InitNewAppointment += SchedulerControlOnInitNewAppointment;
            }
        }

        private void SchedulerControlOnInitNewAppointment(object sender, AppointmentEventArgs e)
        {
            e.Appointment.Start = e.Appointment.Start.AddDays(1);
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            if (_listEditor?.SchedulerControl != null) {
                _listEditor.SchedulerControl.InitNewAppointment -= SchedulerControlOnInitNewAppointment;
                _listEditor = null;
            }
        }
    }
}
