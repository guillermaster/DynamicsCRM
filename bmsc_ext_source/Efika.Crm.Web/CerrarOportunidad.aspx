<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CerrarOportunidad.aspx.cs"
    Inherits="Efika.Crm.Web.CerrarOportunidad" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cierre de campa&ntilde;a</title>
    <link href="Styles/second.css" rel="stylesheet" type="text/css" />
    <base target="_self" />
    <script language="javascript" type="text/javascript">
        function CerrarVentana() {
            window.close();
        }

        function MostrarProcesando() {
            document.getElementById("divControles").style.display = "none";
            document.getElementById("tblProcesando").style.display = "block";
        }
</script>
    <style type="text/css">
        .style1
        {
            color: #FF0000;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
       <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="divControles">
        <table style="width: 550px; border-style: none;" class="ColorTable, layouttable">
            <tr>
                <td colspan="2" class="TableTitles" >
                    Cerrar Oportunidad
                </td>
            </tr>
            <tr>
                <td colspan="2" class="newStyle1">
                    Indique a continuaci&oacute;n los motivos por los cuales se cierra esta oportunidad.
                </td>
            </tr>
            <tr>
                <td colspan="2">
                   <asp:Label ID="lblMensaje" runat="server" Text="" CssClass="ErrorText"></asp:Label>
                </td>
            </tr>
            <tr class="DialogBkgd">
            
                <td colspan="2" align="center">
                    <table style="text-align : left">
                        <tr>
                            <td class="newStyle1" >
                                <asp:HiddenField ID="hdnIdOportunidad" runat="server" />
                                Estado <span class="ErrorText"><span class="style1">*</span> </span>
                            </td>
                            <td align="left" class="newStyle1">
                                <asp:RadioButtonList ID="rbnEstadoOportunidad" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="True" 
                                    onselectedindexchanged="RadioButtonList1_SelectedIndexChanged">
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="newStyle1">
                                Razon para el estado <span class="ErrorText"><span class="style1">*</span> </span>
                            </td>
                            <td class="newStyle1">
                                <asp:DropDownList ID="ddlRazonEstado" runat="server" Width="250px" 
                                    Font-Names="Tahoma" Font-Size="8pt" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td  class="newStyle1">
                                <span class="ErrorText">Monto </span>
                            </td>
                            <td class="newStyle1">
                                <asp:TextBox ID="txtIngresosReales" runat="server" Width="200px" 
                                    Font-Names="Tahoma" Font-Size="8pt" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="newStyle1">
                                Fecha de Cierre <span class="ErrorText"><span class="style1">*</span> </span>
                            </td>
                            <td class="newStyle1">
                                <asp:TextBox ID="txtFechaCierre" runat="server" Font-Names="Tahoma" 
                                    Font-Size="8pt" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="newStyle1">
                                Competidor <span class="ErrorText">&nbsp;</span></td>
                            <td class="newStyle1">
                                <asp:DropDownList ID="ddlCompetidor" runat="server" Width = "200px" 
                                    Font-Names="Tahoma" Font-Size="8pt" Enabled="False">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="newStyle1">
                                Descripción 
                            </td>
                            <td class="newStyle1">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center" class="newStyle1">
                                <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Height="100px"
                                    Width="500px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="DialogBkgd">
                <td colspan="2">
                </td>
            </tr>
            <tr class="DialogBkgd">
                <td>
                    &nbsp;
                </td>
                <td align="right">
                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" 
                        onclick="btnAceptar_Click" Font-Names="Tahoma" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <input id="btnCancelar" type="button" value="Cancelar" 
                        onclick="javascript:window.close();" style="font-family: Tahoma" />
                </td>
            </tr>
        </table>
    </div>
    <TABLE id="tblProcesando" style="DISPLAY: none; MARGIN: 10px;"
				align="center" border="0" runat="server">
				<tr>
					<td style="FONT-WEIGHT: bold; FONT-SIZE: 11px; COLOR: black; FONT-FAMILY: Tahoma" vAlign="middle"
						align="center">Procesando...
					</td>
				</tr>
				<TR>
					<TD vAlign="middle" align="center">
						<div style="position: absolute; top: 0; left: 0; width: 100%; height: 100%; z-index: 100">
                            <div style="position: fixed; top: 40%; left: 50%; width: 401px; height: 150px; margin: -9em 0 0 -15em;
                                border: 2px solid #000; background-color: #FFFFEE; filter: alpha(opacity=100);">
                                <div style="position: fixed; top: 25%; left: 40%; text-align: center;">
                                    <img src="waiting.gif" alt="Cargando" /><br />
                                    <br />
                                    <span style="color: #000099; font-weight: bold; font-size: 12">Procesando...</span>
                                </div>
                            </div>
                        </div>
					</TD>
				</TR>
			</TABLE>
    </form>
</body>
</html>
