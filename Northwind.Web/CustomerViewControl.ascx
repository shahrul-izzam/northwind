<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerViewControl.ascx.cs" Inherits="Northwind.Web.CustomerViewControl" %>
<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<dx:ASPxCardView ID="StageCardView" runat="server" AutoGenerateColumns="True"></dx:ASPxCardView>
<%--<dx:ASPxRoundPanel runat="server" Width="100%" HeaderText="Customer">
    <PanelCollection>
        <dx:PanelContent runat="server">
            <table style="width:100%;">
                <tr>
                    <td><dx:ASPxLabel runat="server" ID="CompanyName_Label" Text="CompanyName"/></td>
                    <td><dx:ASPxLabel runat="server" ID="City_Label" Text="City"/></td>
                </tr>
                <tr>
                    <td><dx:ASPxTextBox ID="CompanyName_TextBox" runat="server"></dx:ASPxTextBox></td>
                    <td><dx:ASPxTextBox ID="City_TextBox" runat="server"></dx:ASPxTextBox></td>
                </tr>
                <tr>
                    <td colspan="2"><dx:ASPxGridView ID="OrdersCollection_GridView" runat="server" AutoGenerateColumns="True"></dx:ASPxGridView></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxRoundPanel>--%>
