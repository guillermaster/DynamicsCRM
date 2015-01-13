<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportarListaMarketing.aspx.cs"
    Inherits="Efika.Crm.Web.ImportarListaMarketing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Importaci&oacute;n de listas de Marketing</title>
    <link href="Styles/second.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CerrarVentana() {
            //if (confirm("La operación actual se cancelará. ¿Desea salir?")) 
            window.close();
        }
    </script>
    <base target="_self" />
    <style type="text/css">
        .style1
        {
            width: 25px;
        }
    </style>
</head>
<body class="page_content, DialogBkgd">
    <form id="form1" runat="server" onsubmit="document.all['btnImportar'].disabled=true;">
    <table style="height: 300px; width: 550px; border-style: none;" class="ColorTable, layouttable">
        <tr>
            <td colspan="2" class="TableTitles ">
                Ingrese la ruta del archivo a cargar e importe los clientes a la lista de marketing
                mencionada.
            </td>
        </tr>
        <tr>
            <td class="style1">
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="TableTitles">
                Archivo
            </td>
            <td>
                <asp:FileUpload ID="fupArchivo" runat="server" 
                    onchange="document.all['btnImportar'].disabled=false;" Enabled="False"/>
                &nbsp;
                <asp:RequiredFieldValidator ID="rfvArchivo" runat="server" ErrorMessage="Debe seleccionar un archivo"
                    ControlToValidate="fupArchivo" Display="Dynamic" Font-Size="8pt"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="TableTitles">
                Nombre
            </td>
            <td>
                <asp:TextBox ID="txtNombre" runat="server" Width="280px" Enabled="False"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;
            </td>
            <td>
                &nbsp;
                <asp:RequiredFieldValidator ID="rfvCampo" runat="server" ErrorMessage="Debe escribir un nombre válido."
                    ControlToValidate="txtNombre" Display="Dynamic" Font-Size="8pt"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="TableTitles">
                Resultado
            </td>
            <td>
                &nbsp;
                <asp:Label ID="lbResultado" runat="server" Font-Names="Tahoma" Font-Size="10pt" 
                    Text="Correcto" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:TextBox ID="txtMensaje" runat="server" Height="100px" TextMode="MultiLine" Width="90%"
                    ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <asp:Button ID="btnImportar" runat="server" OnClick="btnImportar_Click" 
                    Text="Importar" Enabled="False" />&nbsp;&nbsp;&nbsp;
                <input id="btnCerrar" type="button" value="Cerrar" onclick="CerrarVentana();" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
