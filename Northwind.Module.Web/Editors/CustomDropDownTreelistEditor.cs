using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base.General;
using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using DevExpress.Xpo;
using Northwind.Module.BusinessObjects.Northwind;
using ValidationSettings = DevExpress.Web.ValidationSettings;

namespace Northwind.Module.Web.Editors
{
    [PropertyEditor(typeof(object), false)]
    public class CustomDropDownTreelistEditor : ASPxPropertyEditor
    {
        private ASPxDropDownEdit _dropDownControl;

        public CustomDropDownTreelistEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model)
        {
        }

        protected override void SetupControl(WebControl control)
        {
            base.SetupControl(control);
            if (ViewEditMode == ViewEditMode.Edit)
            {
                var collection = new XPCollection(((XPObjectSpace) View.ObjectSpace).Session, MemberInfo.MemberTypeInfo.Type);
                WebWindow.CurrentRequestWindow.RegisterClientScript($"{control.ID}_timeout_variable;", $"var {control.ID}_timeout;");
                _dropDownControl.ID = _dropDownControl.ClientInstanceName = Id;
                _dropDownControl.DropDownWindowTemplate = new DropDownTreeListTemplate(collection, Id);
                //_dropDownControl.ClientSideEvents.KeyUp = $"function(s, e) {{ clearTimeout({Id}_timeout); {Id}_timeout = setTimeout(function () {{ {Id}_TreeList.PerformCallback({Id}.GetText()) }}, 800); }}";
                _dropDownControl.ClearButton.DisplayMode = ClearButtonDisplayMode.Always;
                _dropDownControl.ClientSideEvents.LostFocus = "function(s,e){}";
                _dropDownControl.ValueChanged -= EditValueChangedHandler;
                 _dropDownControl.ValueChanged += EditValueChangedHandler;
            }
        }

        protected override WebControl CreateEditModeControlCore()
        {
            _dropDownControl = new ASPxDropDownEdit();
            _dropDownControl.ValueChanged -= EditValueChangedHandler;
            _dropDownControl.ValueChanged += EditValueChangedHandler;
            return _dropDownControl;
        }

        public override void BreakLinksToControl(bool unwireEventsOnly)
        {
            if (_dropDownControl != null)
            {
                _dropDownControl.ValueChanged -= EditValueChangedHandler;
            }
            base.BreakLinksToControl(unwireEventsOnly);
        }

        protected override object GetControlValueCore()
        {
            if (_dropDownControl.KeyValue == null || string.IsNullOrEmpty(_dropDownControl.KeyValue.ToString()))
            {
                return null;
            }
            var selectedObject = View.ObjectSpace.GetObjectByKey(MemberInfo.MemberTypeInfo.Type, int.Parse(_dropDownControl.KeyValue.ToString()));
            return ((ITreeNode)selectedObject)?.Name != _dropDownControl.Text ? null : selectedObject;
        }

        protected override void ReadEditModeValueCore()
        {
            object value = PropertyValue;
            if (value is Categories category)
            {
                _dropDownControl.KeyValue = category.CategoryID;
                _dropDownControl.Text = category.CategoryName;
            }
        }
    }

    public class DropDownTreeListTemplate : ITemplate
    {
        private const string DisplayColumnName = "CategoryName";
        private const string KeyFieldName = "CategoryID";
        private const string ParentFieldName = "Parent!Key";
        private const string DropDownHashtableId = "cpDropDownHashtable";

        private readonly XPCollection _treeListCollection;
        private readonly string _parentControlId;

        public DropDownTreeListTemplate(XPCollection collection, string parentControlId)
        {
            _treeListCollection = collection;
            _treeListCollection.DisplayableProperties = DisplayColumnName;
            _parentControlId = parentControlId;
        }

        public void InstantiateIn(Control container)
        {
            ASPxTreeList treeList = new ASPxTreeList();
            treeList.ID = treeList.ClientInstanceName = $"{_parentControlId}_TreeList";
            treeList.DataSource = _treeListCollection;
            treeList.ParentFieldName = ParentFieldName;
            treeList.KeyFieldName = KeyFieldName;
            treeList.DataBind();
            treeList.ClientSideEvents.NodeClick = $"function TreeListNodeClickHandler(s, e) {{ {_parentControlId}.SetKeyValue(e.nodeKey); {_parentControlId}.SetValue(e.nodeKey); " + 
                                                  $"{_parentControlId}.SetText({_parentControlId}_TreeList.{DropDownHashtableId}[e.nodeKey]); " +  
                                                  $"{_parentControlId}.RaiseValueChanged(); {_parentControlId}.HideDropDown(); {_parentControlId}.Validate(); }}";
            treeList.ClientSideEvents.EndCallback = $"function(s, e){{ {_parentControlId}.ShowDropDown(); }}";
            treeList.CustomJSProperties += TreeList_CustomJSProperties;
            treeList.Settings.ShowColumnHeaders = false;
            treeList.CustomCallback += TreeList_CustomCallback;
            container.Controls.Add(treeList);
        }

        private void TreeList_CustomCallback(object sender, TreeListCustomCallbackEventArgs e)
        {
            ((ASPxTreeList)sender).FilterExpression = $"[{DisplayColumnName}] Like '%{e.Argument}%'";
        }

        private void TreeList_CustomJSProperties(object sender, TreeListCustomJSPropertiesEventArgs e)
        {
            ASPxTreeList tree = sender as ASPxTreeList;
            Hashtable dropDownHashtable = new Hashtable();
            if (tree != null)
                foreach (TreeListNode node in tree.GetVisibleNodes())
                    dropDownHashtable.Add(node.Key, node[DisplayColumnName]);

            e.Properties[DropDownHashtableId] = dropDownHashtable;
        }
    }
}
