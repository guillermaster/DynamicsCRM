<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OfertaValorMicro.aspx.cs" Inherits="Efika.Crm.Web.OfertaValorMicro" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>oferta de Valor</title>
    <link href="../Styles/crm.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #FFF">
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="hdnIdCliente" runat="server" />
        <asp:DataList ID="dlOfertaValor" runat="server" OnItemCommand="dlOfertaValor_ItemCommand" >
            <ItemTemplate>
                <div style="float: left;">
                    <div style="text-align:center;">                        
                        <table style="width: 70px;">
                            <tr>
                                <td style="width: 70px; height: 64px; text-align:center"><img src="<%# Eval("icono")%>" alt="<%# Eval("titulo")%>" /></td>
                            </tr>
                            <tr>
                                <td style="width: 70px; height: 24px; text-align:center; font-size: 6pt"><%# Eval("titulo")%></td>
                            </tr>
                            <tr>
                                <td style="width: 70px; height: 18px; text-align:center; font-size: 8pt"><asp:Label ID="lblMontoMaxino" runat="server" Text='<%# Eval("efk_monto_maximo")%>' /></td>
                            </tr>
                            <tr>
                                <td style="width: 70px; height: 18px; text-align:center; font-size: 8pt"><asp:Label ID="lblProbAceptac" runat="server" Text='<%# Eval("efk_probabilidad_aceptacion")%>' />%</td>
                            </tr>
                            <tr>
                                <td style="width: 70px; text-align:center">
                                    <asp:ImageButton ID="mgBtnLike" runat="server"  Height="16" Width="16"  ImageUrl="~/Images/like.gif" ToolTip="Me gusta" CommandName="Select" CommandArgument="1" />
                                    <asp:ImageButton ID="mgBtnNoLike" runat="server" Height="16" Width="16" ImageUrl="~/Images/dislike.gif" ToolTip="No me gusta" CommandName="Select" CommandArgument="0" />
                                    <asp:Image ID="imgTick" runat="server" Height="16" Width="16" ImageUrl="~/Images/16/tick_shield.png" ToolTip="Si desea" Visible="false" />
                                    <asp:Image ID="imgMinus" runat="server" Height="16" Width="16" ImageUrl="~/Images/16/minus_shield.png" ToolTip="No desea" Visible="false" />
                                    <asp:DropDownList ID="ddlMotivoMalaCalificac" runat="server" Width="63" Visible="false" Font-Size="Smaller" AutoPostBack="true" onselectedindexchanged="ddlMotivoMalaCalificac_SelectedIndexChanged"  />
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="hdfTipoProductoId" runat="server" Value='<%# Eval("efk_tipo_productos_id")%>' />
                        <asp:HiddenField ID="hdfSubTipoProductoId" runat="server" Value='<%# Eval("efk_product_id")%>' />
                    </div>
                </div>
            </ItemTemplate>
        </asp:DataList>
        <asp:Label ID="lblMensajeSinProductosOV" Text="No hay ningún producto por ofertar a este cliente." runat="server" Visible="false" />
                     
                       
    </div>
    </form>
</body>
</html>
