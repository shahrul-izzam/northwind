using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates.ActionContainers;
using DevExpress.Web;

namespace Northwind.Module.Web.Layout
{
    public class CustomLayoutGroupTemplate : LayoutGroupTemplate
    {
        public const string CardGroupContentCssClassName = "CardGroupContent";

        protected override void LayoutContentControls(LayoutGroupTemplateContainer templateContainer,
            IList<Control> controlsToLayout)
        {
            Control contentContainer = templateContainer;
            LayoutCSSInfo layoutCssInfo = templateContainer.LayoutManager.LayoutCSSInfo;
            LayoutItemCSSInfoBase itemCssInfo = layoutCssInfo?.GetCssInfo(templateContainer.Model);

            if(itemCssInfo != null && itemCssInfo.CardItem) {
                if(((IModelLayoutGroupWeb)templateContainer.Model).IsCollapsibleCardGroup) {
                    contentContainer = CreateCollapsibleCardGroup(templateContainer, itemCssInfo);
                }
                else {
                    contentContainer = CreateCardGroup(templateContainer, itemCssInfo);
                }
            }
            else {
                WebControl header = CreateLayoutContentHeader(templateContainer, false);
                if(header != null) {
                    templateContainer.Controls.Add(header);
                }
            }

            foreach (Control control in controlsToLayout)
            {
                if (templateContainer.LayoutManager.DelayedItemsInitialization &&
                    control is LayoutItemTemplateContainerBase @base)
                {
                    @base.Instantiate();
                }

                contentContainer.Controls.Add(control);
            }
        }

        private Control CreateCardGroup(LayoutGroupTemplateContainer templateContainer, LayoutItemCSSInfoBase itemCssInfo) {
            Table cardTable = RenderHelper.CreateTable();
            cardTable.BorderWidth = Unit.Empty;
            if(itemCssInfo.ParentDirection == FlowDirection.Horizontal) {
                cardTable.CssClass = itemCssInfo.EditorContainerCssClassName;
            }
            else {
                cardTable.CssClass = itemCssInfo.CardCssClassNameCore;
            }
            SetCustomCSSClass(templateContainer.Model, cardTable);
            cardTable.ID = WebIdHelper.GetCorrectedLayoutItemId(templateContainer.Model, "", "_CardTable");
            TableRow headerRow = new TableRow();
            headerRow.VerticalAlign = VerticalAlign.Top;
            cardTable.Rows.Add(headerRow);
            TableRow contentRow = new TableRow();
            contentRow.VerticalAlign = VerticalAlign.Top;
            cardTable.Rows.Add(contentRow);
            WebControl header = CreateLayoutContentHeader(templateContainer, itemCssInfo.CardItem);
            if(header != null) {
                TableCell headerCell = new TableCell();
                headerRow.Cells.Add(headerCell);
                headerCell.Controls.Add(header);
            }
            TableCell contentCell = new TableCell();
            contentCell.CssClass = CardGroupContentCssClassName;
            contentRow.Cells.Add(contentCell);
            templateContainer.Controls.Add(cardTable);
            return contentCell;
        }

        private Control CreateCollapsibleCardGroup(LayoutGroupTemplateContainer templateContainer, LayoutItemCSSInfoBase layoutCssInfo) {
            ASPxRoundPanel cardPanel = new ASPxRoundPanel();
            cardPanel.ID = WebIdHelper.GetCorrectedLayoutItemId(templateContainer.Model, "", "_CardTable");
            cardPanel.BorderWidth = Unit.Empty;
            cardPanel.HeaderTemplate = new CustomTemplate(templateContainer);

            new CollapsibleCardGroupSynchronizer(templateContainer, cardPanel);
            if(layoutCssInfo.ParentDirection == FlowDirection.Horizontal) {
                cardPanel.CssClass = layoutCssInfo.EditorContainerCssClassName;
            }
            else {
                cardPanel.CssClass = layoutCssInfo.CardCssClassNameCore;
            }
            SetCustomCSSClass(templateContainer.Model, cardPanel);
            WebControl cardGroupContent = new WebControl(HtmlTextWriterTag.Div);
            cardGroupContent.CssClass = CardGroupContentCssClassName;
            if((!templateContainer.IsOnTabPage || templateContainer.ParentGroupDirection == FlowDirection.Vertical)) {
                cardGroupContent.CssClass += " CollapsibleContent";
                cardPanel.AllowCollapsingByHeaderClick = false;
                cardPanel.ShowCollapseButton = true;
                cardPanel.HeaderStyle.CssClass = "GroupHeader Label";
                
                
                if(templateContainer.HasHeaderImage) {
                    cardPanel.HeaderImage.AlternateText = templateContainer.Caption;
                    ASPxImageHelper.SetImageProperties(cardPanel.HeaderImage, templateContainer.HeaderImageInfo);
                }
                if(templateContainer.ShowCaption) {
                    if(WebApplicationStyleManager.EnableGroupUpperCase) {
                        cardPanel.HeaderText = templateContainer.Caption.ToUpper();
                    }
                    else {
                        cardPanel.HeaderText = templateContainer.Caption;
                    }
                }
                else {
                    cardPanel.HeaderText = "";
                }
                ((ISupportToolTip)this).SetToolTip(cardPanel, templateContainer.Model);
            }
            else {
                cardPanel.ShowHeader = false;
            }
            cardPanel.Controls.Add(cardGroupContent);
            templateContainer.Controls.Add(cardPanel);
            return cardGroupContent;
        }

        private WebControl CreateLayoutContentHeader(LayoutGroupTemplateContainer templateContainer, bool isCardItem) {
            WebControl div = null;
            if(templateContainer.ShowCaption && (!templateContainer.IsOnTabPage || templateContainer.ParentGroupDirection == FlowDirection.Vertical)) {
                div = new WebControl(HtmlTextWriterTag.Div);
                div.CssClass = "GroupHeader";
                if(templateContainer.HasHeaderImage) {
                    WebControl imageDiv = new WebControl(HtmlTextWriterTag.Div);
                    ASPxImage imageHeader = new ASPxImage();
                    imageDiv.Controls.Add(imageHeader);
                    imageHeader.CssClass = "Image";
                    ASPxImageHelper.SetImageProperties(imageHeader, templateContainer.HeaderImageInfo);
                    imageHeader.AlternateText = templateContainer.Caption;
                    div.Controls.Add(imageDiv);
                }
                WebControl labelDiv = new WebControl(HtmlTextWriterTag.Div);
                Label label = new Label();
                labelDiv.Controls.Add(label);
                if(isCardItem && WebApplicationStyleManager.IsNewStyle && WebApplicationStyleManager.EnableGroupUpperCase) {
                    label.Text = templateContainer.Caption.ToUpper();
                }
                else {
                    label.Text = templateContainer.Caption;
                }
                ((ISupportToolTip)this).SetToolTip(label, templateContainer.Model);
                templateContainer.CaptionControl = label;
                label.CssClass = "Label";
                div.Controls.Add(labelDiv);
            }
            return div;
        }
    }

    public class CollapsibleCardGroupSynchronizer {
        private ASPxRoundPanel panel;
        private LayoutGroupTemplateContainer templateContainer;
        public CollapsibleCardGroupSynchronizer(LayoutGroupTemplateContainer templateContainer, ASPxRoundPanel panel) {
            this.templateContainer = templateContainer;
            this.panel = panel;
            panel.Collapsed = ((IModelLayoutGroupWeb)templateContainer.Model).IsCardGroupCollapsed;
            if(WebApplication.OptimizationSettings.LockRecoverViewStateOnNavigationCallback || WebApplication.OptimizationSettings.UseFastCallbackHandlers) {
                panel.ClientSideEvents.CollapsedChanged = "function(s,e) { xaf.LayoutManagementController.CollapsiblePanelCollapsedChanged(s, e); }";
            }
            panel.Unload += panel_Unload;
        }
        private void panel_Unload(object sender, EventArgs e) {
            if(panel != null) {
                ((IModelLayoutGroupWeb) templateContainer.Model).IsCardGroupCollapsed = panel.Collapsed;
                templateContainer = null;
                panel.Unload -= panel_Unload;
                panel = null;
            }
        }
    }

    public class CustomTemplate : ITemplate
    {
        private readonly LayoutGroupTemplateContainer _templateContainer;

        public CustomTemplate(LayoutGroupTemplateContainer templateContainer)
        {
            _templateContainer = templateContainer;
        }
        public void InstantiateIn(Control container)
        {
            Table cardTable = RenderHelper.CreateTable();
            cardTable.BorderWidth = Unit.Empty;

            TableRow contentRow = new TableRow();
            contentRow.VerticalAlign = VerticalAlign.Top;
            cardTable.Rows.Add(contentRow);

            Label caption = new Label
            {
                ID = WebIdHelper.GetCorrectedLayoutItemId(_templateContainer.Model, "", "_Caption"),
                Text = _templateContainer.Caption
            };

            ASPxImage icon = new ASPxImage
            {
                ID = WebIdHelper.GetCorrectedLayoutItemId(_templateContainer.Model, "", "_Icon"),
                ImageUrl = ImageLoader.Instance.GetImageInfo("Action_Debug_Start").ImageUrl
            };

            Label number = new Label
            {
                ID = WebIdHelper.GetCorrectedLayoutItemId(_templateContainer.Model, "", "_Number"), Text = "(6)"
            };

            ASPxImage warning = new ASPxImage
            {
                ID = WebIdHelper.GetCorrectedLayoutItemId(_templateContainer.Model, "", "_Warning"),
                ImageUrl = ImageLoader.Instance.GetImageInfo("Action_CreateDashboard").ImageUrl
            };

            var actionContainer = CreateActionContainerHolder();

            ASPxImage collapsible = new ASPxImage();
            collapsible.CssClass = "dxWeb_rpCollapseButton_XafTheme";
            collapsible.ClientSideEvents.Click = "";

            WebControl iconDiv = new WebControl(HtmlTextWriterTag.Div);
            iconDiv.Controls.Add(icon);

            WebControl captionDiv = new WebControl(HtmlTextWriterTag.Div);
            captionDiv.Controls.Add(caption);

            WebControl numberDiv = new WebControl(HtmlTextWriterTag.Div);
            numberDiv.Style.Add("float","right");
            numberDiv.Controls.Add(number);

            WebControl warningDiv = new WebControl(HtmlTextWriterTag.Div);
            warningDiv.Style.Add("float", "right");
            warningDiv.Controls.Add(warning);

            WebControl collapsibleDiv = new WebControl(HtmlTextWriterTag.Div);
            collapsibleDiv.Style.Add("float", "right");
            collapsibleDiv.Controls.Add(collapsible);

            WebControl containerDiv = new WebControl(HtmlTextWriterTag.Div);
            containerDiv.Style.Add("float", "right");
            containerDiv.Controls.Add(actionContainer);
            
            container.Controls.Add(iconDiv);
            container.Controls.Add(captionDiv);
            container.Controls.Add(collapsibleDiv);
            container.Controls.Add(containerDiv);
            container.Controls.Add(warningDiv);
            container.Controls.Add(numberDiv);
            
        }

        protected ActionContainerHolder CreateActionContainerHolder()
        {
            var actionContainerHolder = new ActionContainerHolder();
            actionContainerHolder.ID = "CustomActionContainerHolder";
            actionContainerHolder.ContainerStyle = ActionContainerStyle.Buttons;
            actionContainerHolder.Menu.ItemWrap = true;

            var webActionContainer = new WebActionContainer();
            webActionContainer.ContainerId = "ObjectsCreation";
            webActionContainer.IsDropDown = true;
            
            actionContainerHolder.ActionContainers.Add(webActionContainer);

            return actionContainerHolder;
        }
    }
}
