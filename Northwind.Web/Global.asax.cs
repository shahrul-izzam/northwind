using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Web;
using DevExpress.Persistent.AuditTrail;
using DevExpress.Web;
using DevExpress.Xpo;
using Northwind.Module.BusinessObjects;
using Northwind.Module.BusinessObjects.Northwind;

namespace Northwind.Web {
    public class Global : System.Web.HttpApplication {
        public Global() {
            InitializeComponent();
        }
        protected void Application_Start(Object sender, EventArgs e) {
            ASPxWebControl.CallbackError += new EventHandler(Application_Error);
#if EASYTEST
            DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = true;
#endif
        }
        protected void Session_Start(Object sender, EventArgs e) {
            Tracing.Initialize();
            WebApplication.SetInstance(Session, new NorthwindAspNetApplication());
            //DevExpress.ExpressApp.Web.Templates.DefaultVerticalTemplateContentNew.ClearSizeLimit();
            WebApplication.Instance.Settings.DefaultVerticalTemplateContentPath = "DefaultVerticalTemplateContent.ascx";
            WebApplication.Instance.SwitchToNewStyle();
            if(ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
#if DEBUG
            if(System.Diagnostics.Debugger.IsAttached && WebApplication.Instance.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                WebApplication.Instance.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif

            #region AuditTrail
            AuditTrailService.Instance.ObjectAuditingMode = ObjectAuditingMode.Lightweight;
            AuditTrailService.Instance.CustomizeAuditTrailSettings += Instance_CustomizeAuditTrailSettings;
            AuditTrailService.Instance.SaveAuditTrailData += Instance_SaveAuditTrailData;
            //AuditTrailService.Instance.AuditDataStore = new OverrideAuditDataItems();

            #endregion

            WebApplication.Instance.Setup();
            WebApplication.Instance.Start();
        }
        protected void Application_BeginRequest(Object sender, EventArgs e) {
        }
        protected void Application_EndRequest(Object sender, EventArgs e) {
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e) {
        }
        protected void Application_Error(Object sender, EventArgs e) {
            ErrorHandling.Instance.ProcessApplicationError();
        }
        protected void Session_End(Object sender, EventArgs e) {
            WebApplication.LogOff(Session);
            WebApplication.DisposeInstance(Session);
        }
        protected void Application_End(Object sender, EventArgs e) {
        }

        static void Instance_CustomizeAuditTrailSettings(object sender, CustomizeAuditTrailSettingsEventArgs  e)
        {
            var currentSettings = e.AuditTrailSettings;
        }

        static void Instance_SaveAuditTrailData(object sender, SaveAuditTrailDataEventArgs e) {
            //Save the data passed as the e.AuditTrailDataItems parameter 
            //Disable the default storing procedure if it is necessary 
            using (var uow = new UnitOfWork(e.Session.DataLayer))
            {
                foreach (var auditTrailDataItem in e.AuditTrailDataItems)
                {
                    var log = new LogEntries(uow)
                    {
                        ModuleID = 1, 
                        Date = DateTime.Now, 
                        RecordFriendlyName = $"from: {auditTrailDataItem.OldValue} to: {auditTrailDataItem.NewValue}",
                        IPAddress = HttpContext.Current.Request.UserHostAddress
                    };

                    try
                    {
                        uow.CommitChanges();
                    }
                    catch
                    {
                        uow.RollbackTransaction();
                        throw;
                    }
                }
            }
            //e.Handled = true;
        }

        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
        }
        #endregion
    }
}
