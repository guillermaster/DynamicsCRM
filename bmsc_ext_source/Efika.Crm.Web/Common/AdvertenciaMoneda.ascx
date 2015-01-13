<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdvertenciaMoneda.ascx.cs" Inherits="Efika.Crm.Web.Common.AdvertenciaMoneda" %>
<asp:Panel ID="pnlMensajeMoneda" runat="server">
    <div style="border: 1px dotted #ffd95c; background: #fffac4; width: 293px; height: 18px; margin: 15px; padding: 2px 10px;">
        <div style="float:left">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/16/warning16x16.png" /></div>
        <div style="float:left; margin-left: 20px; font-family: Tahoma; font-size: 8pt; color: #848571">
            Los montos son expresados en dólares americanos.
        </div>
    </div>
</asp:Panel>