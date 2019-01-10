using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.TestScripts;
using DevExpress.Xpo;

namespace Northwind.Module.Web.Editors
{
    [ListEditor(typeof(XPCollection), false)]
    public class CustomComplexListEditor : ListEditor, IComplexListEditor, ILinkedToControl
    {
        private XafApplication _application;
        private CollectionSourceBase _collectionSource;
        private ProxyCollection _proxyCollection;
        private readonly Dictionary<object, DetailFrameInfo> framesInfoDictionary = new Dictionary<object, DetailFrameInfo>();

        public CustomComplexListEditor()
        {

        }

        public CustomComplexListEditor(IModelListView model) : base(model)
        {

        }

        public new CustomPanel Control => base.Control as CustomPanel;

        public override SelectionType SelectionType => SelectionType.None;

        public override IList GetSelectedObjects()
        {
            return new ArrayList();
        }

        public override void Refresh()
        {
            UpdateDataSource();
        }

        public void Setup(CollectionSourceBase collectionSource, XafApplication application)
        {
            _collectionSource = collectionSource;
            _application = application;
        }

        public void BreakLinksToControl()
        {
            foreach (var frameInfo in framesInfoDictionary.Values)
            {
                ClearFrameInfo(frameInfo);
            }
        }

        public virtual Frame GetDetailFrame(object obj, int index)
        {
            if (!framesInfoDictionary.TryGetValue(obj, out DetailFrameInfo detailFrameInfo))
            {
                var detailView = _application.CreateDetailView(_collectionSource.ObjectSpace, Model.MasterDetailView, false, obj);
                var detailFrame = _application.CreateFrame(TemplateContext.NestedFrame);
                detailFrame.GetController<WebResetViewSettingsController>().ResetViewSettingsAction.Active.SetItemValue("PrototypeDisable", false);
                detailFrame.SetView(detailView);
                
                detailFrameInfo = new DetailFrameInfo
                {
                    FrameIndex = index,
                    DetailFrame = detailFrame
                };
                framesInfoDictionary[obj] = detailFrameInfo;
            }
            else
            {
                ClearFrameInfo(detailFrameInfo);
                detailFrameInfo.DetailFrame.SetTemplate(null);
            }
            detailFrameInfo.DetailFrame.CreateTemplate();
            return detailFrameInfo.DetailFrame;
        }

        public void RefreshObjectViewFromCache(object obj)
        {
            if (!framesInfoDictionary.ContainsKey(obj))
            {
                return;
            }

            var viewObjectSpace = framesInfoDictionary[obj].DetailFrame.View.ObjectSpace;
            if (viewObjectSpace != null)
            {
                viewObjectSpace.Refresh();
            }
            else
            {
                framesInfoDictionary[obj].DetailFrame.View.RefreshDataSource();
            }
        }

        protected override void AssignDataSourceToControl(object dataSource)
        {
            _proxyCollection = dataSource as ProxyCollection;
            UpdateDataSource();
        }

        protected override object CreateControlsCore()
        {
            return new CustomPanel();
        }

        protected virtual void ClearFrameInfo(DetailFrameInfo frameInfo)
        {
            frameInfo.DetailFrame.View.BreakLinksToControls();
            ((IDisposable)frameInfo.DetailFrame.Template).Dispose();
        }

        private void UpdateDataSource()
        {
            if (Control == null || _proxyCollection == null || _collectionSource == null)
            {
                return;
            }

            for (var i = 0; i < _proxyCollection.Count; i++)
            {
                var detailFrame = GetDetailFrame(_proxyCollection[i], i);
                Control.Controls.Add((Control)detailFrame.Template);
            }
        }
    }
}
