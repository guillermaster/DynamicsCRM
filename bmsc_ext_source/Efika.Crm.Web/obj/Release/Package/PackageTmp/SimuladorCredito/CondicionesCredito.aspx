﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CondicionesCredito.aspx.cs"
    Inherits="Efika.Crm.Web.SimuladorCredito.CondicionesCredito" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="../Common/AdvertenciaMoneda.ascx" tagname="AdvertenciaMoneda" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Simulador de crédito - Condiciones</title>
    <script type="text/javascript">
        function CerrarVentana() {
            //if (confirm("La operación actual se cancelará. ¿Desea salir?")) 
            window.close();
        }
        function DisplayDdlText(textboxId, dropdownlistId) {
            document.getElementById(textboxId).value = document.getElementById(dropdownlistId).value;
            document.getElementById(textboxId).focus();
        }
        function mostrarCargando() {
            if (Page_ClientValidate(""))
                document.getElementById('loading').style.display = "block";
        }

        function validateOnlyNumbers(evt) {
            var theEvent = evt;
            var key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);
            var regex = /[0-9]|\,/;
            if (!regex.test(key)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) theEvent.preventDefault();
            }
        }

        function OnCurrencyValueKeyUp(nr) {

            //remove the existing ,
            var regex = /./g;
            nr = nr.value.replace('.', '');
            //force it to be a string
            nr += '';
            //split it into 2 parts  (for numbers with decimals, ex: 125.05125)
            var x = nr.split(',');
            var p1 = x[0];
            var p2 = x.length > 1 ? ',' + x[1] : '';
            //match groups of 3 numbers (0-9) and add , between them
            regex = /(\d+)(\d{3})/;
            while (regex.test(p1)) {
                p1 = p1.replace(regex, '$1' + '.' + '$2');
            }
            //join the 2 parts and return the formatted number
            return p1 + p2;
        }
    </script>
    
    <base target="_self" />
    <style type="text/css">
        .style1
        {
            width: 25px;
        }
        #fields
        {
            padding: 20px;
            background: #FFF;
        }
    </style>
</head>
<body>
    <div id="divForm">
        <form id="form1" runat="server">
        
                <OfficeWebUI:Manager ID="Manager1" runat="server" UITheme="Office2010Silver" IncludeJQuery="true"
                    ChromeUI="true" />
                <div style="margin: -20px 0 0 0">
                    <OfficeWebUI:OfficeRibbon ID="OfficeRibbon1" runat="server" ApplicationMenuType="None"
                        ShowToggleButton="false">
                        <Contexts>
                       
                            <OfficeWebUI:RibbonContext ID="RibbonContext1" runat="server" ContextColor="transparent"
                                Text="">
                                <Tabs>
                                    <OfficeWebUI:RibbonTab ID="RibbonTab101" runat="server" Text="Condiciones de crédito">
                                        <Groups>
                                        
                                            <OfficeWebUI:RibbonGroup ID="RibbonGroup1" runat="server" Text="Acciones">
                                                <Zones>
                                                    <OfficeWebUI:GroupZone ID="GroupZone1" runat="server" Text="Zone 1">
                                                        <Content>
                                                            <OfficeWebUI:LargeItem ID="btnGuardar" runat="server" ImageUrl="~/Images/32/icon86_32.png" ClientClick="mostrarCargando()" ValidationGroup="valSummaryGroup"
                                                                OnClick="Guardar_Click" PerformValidation="true" Text="Guardar" Tooltip="<b>Guardar</b><br/>Guarda condiciones de simulación." />
                                                            <OfficeWebUI:LargeItem ID="btnGuardarCerrar" runat="server" ImageUrl="~/Images/32/SaveAndClose_32.png" ClientClick="mostrarCargando()" PerformValidation="true" ValidationGroup="valSummaryGroup"
                                                                OnClick="GuardarCerrar_Click" Text="Guardar y cerrar" Tooltip="<b>Guardar y cerrar</b><br/>Guarda condiciones de simulación y cierra esta ventana." />
                                                            <OfficeWebUI:LargeItem ID="btnCancelar" runat="server" ImageUrl="~/Images/32/icon10_32.png"
                                                                OnClick="Cerrar_Click" PerformValidation="false" Text="Cerrar" Tooltip="<b>Cerrar</b><br/>Cierra esta ventana sin guardar cambios." />
                                                        </Content>
                                                    </OfficeWebUI:GroupZone>
                                                </Zones>
                                            </OfficeWebUI:RibbonGroup>
                                            <OfficeWebUI:RibbonGroup ID="RibbonGroup2" runat="server" Text="">
                                                <Zones>
                                                    <OfficeWebUI:GroupZone ID="GroupZone2" runat="server" Text="Zone 1">
                                                        <Content>
                                                            <OfficeWebUI:LargeItem ID="btnCalcMax" runat="server" ImageUrl="~/Images/32/maximizevalue.png" ValidationGroup="valSummaryGroup"
                                                                OnClick="CalcularMaximo_Click" PerformValidation="true" Text="Calcular Máximo" ClientClick="mostrarCargando()"
                                                                Tooltip="<b>Calcular máximo</b><br/>Calcula el monto máximo para este producto/servicio." />
                                                        </Content>
                                                    </OfficeWebUI:GroupZone>
                                                </Zones>
                                            </OfficeWebUI:RibbonGroup>
                                        </Groups>
                                    </OfficeWebUI:RibbonTab>
                                </Tabs>
                            </OfficeWebUI:RibbonContext>
                        </Contexts>
                    </OfficeWebUI:OfficeRibbon>
                </div>
                <OfficeWebUI:OfficeWorkspace ID="Workspace1" runat="server">
                    <Content>
                      <div id="fields">
                        <div style="height: 16px";>
                        <div id="loading" style="display: none">
                            <img src="../waiting.gif" alt="Cargando" />
                        </div>
                        </div>
                        <div id="divControles">
                            
                                <asp:HiddenField ID="hdnProdId" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hdnProdTipoId" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hdnProdFamId" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hdnProdSimId" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hdnClientId" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hdnSimCredId" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hdnCantidadProdsPrevio" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hdnDivisaId" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hdnNumOferta" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hdnTipoAccion" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hdnOportunidadId" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hdnCodUsuario" ClientIDMode="Static" runat="server" />
                                <table>
                                    <tr>
                                        <td>
                                            Producto:
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtProductoNombre" ReadOnly="true" Width="385px" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Número de cuotas:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNumCuotas" runat="server" OnTextChanged="formItem_Changed" AutoPostBack="true" onkeypress="validateOnlyNumbers(event)" />
                                            <asp:RequiredFieldValidator ID="reqValNumCuotas" runat="server" ValidationGroup="valSummaryGroup" ErrorMessage="Debe ingresar el número de cuotas" Display="None" ControlToValidate="txtNumCuotas" />
                                            <asp:RegularExpressionValidator ID="rexValNumCuotas" runat="server" ValidationGroup="valSummaryGroup" ErrorMessage="El número de cuotas ingresado no es correcto." Display="None" ValidationExpression="^[0-9]+$" ControlToValidate="txtNumCuotas" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Moneda de la operación:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlMonedaOp" runat="server" OnSelectedIndexChanged="monedaItem_Changed" AutoPostBack="true" />
                                            <asp:RequiredFieldValidator ID="reqValMoneda" runat="server" ValidationGroup="valSummaryGroup" ErrorMessage="Debe seleccionar la moneda de operación" Display="None" ControlToValidate="ddlMonedaOp" />
                                        </td>
                                        <td>
                                            Tasa Fija:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTasaFija" ClientIDMode="Static" runat="server" SkinID="txtDdl" onkeypress="validateOnlyNumbers(event)" /> 
                                            <asp:DropDownList ID="ddlTasaFija" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="formItem_Changed" onchange="DisplayDdlText('txtTasaFija', 'ddlTasaFija');" AutoPostBack="true" />
                                            <asp:RegularExpressionValidator ID="rexValTasaFija" runat="server" ValidationGroup="valSummaryGroup" ErrorMessage="La tasa fija ingresada no es correcta." ValidationExpression="^[0-9]\d*(,\d+)?$" Display="None" ControlToValidate="txtTasaFija"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="reqValTasaFija" runat="server" ValidationGroup="valSummaryGroup" ErrorMessage="Debe ingresar la tasa fija" ControlToValidate="txtTasaFija" Display="None" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            TRE Semana:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTRE" ClientIDMode="Static" runat="server" SkinID="txtDdl" onkeypress="validateOnlyNumbers(event)" /> 
                                            <asp:DropDownList ID="ddlTRE" ClientIDMode="Static" runat="server" OnSelectedIndexChanged="formItem_Changed" onchange="DisplayDdlText('txtTRE', 'ddlTRE');" AutoPostBack="true" />
                                            <asp:RegularExpressionValidator ID="rexValTre" runat="server" ValidationGroup="valSummaryGroup" ErrorMessage="El TRE ingresado no es correcto." ValidationExpression="^[0-9]\d*(,\d+)?$" ControlToValidate="txtTRE" Display="None" />
                                            <asp:RequiredFieldValidator ID="reqValTre" runat="server" ValidationGroup="valSummaryGroup" ErrorMessage="Debe ingresar el TRE" ControlToValidate="txtTRE" Display="None" />
                                        </td>
                                        <td>
                                            SPREAD Fijo:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSPREAD" ClientIDMode="Static" runat="server" SkinID="txtDdl" onkeypress="validateOnlyNumbers(event)" /> 
                                            <asp:DropDownList ID="ddlSPREAD" ClientIDMode="Static" runat="server" OnSelectedIndexChanged="formItem_Changed" onchange="DisplayDdlText('txtSPREAD', 'ddlSPREAD');" AutoPostBack="true" />
                                            <asp:RegularExpressionValidator ID="rexValSpread" ValidationGroup="valSummaryGroup" runat="server" ErrorMessage="El Spread ingresado no es correcto" ValidationExpression="^[0-9]\d*(,\d+)?$" ControlToValidate="txtTRE" Display="None"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="reqValSpread" ValidationGroup="valSummaryGroup" runat="server" ErrorMessage="Debe ingresar el Spread" ControlToValidate="txtSPREAD" Display="None" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Amortización cada:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlFrecAmortizac" runat="server" OnSelectedIndexChanged="formItem_Changed" />
                                            <asp:RequiredFieldValidator ID="reqValFrecAmort" ValidationGroup="valSummaryGroup" runat="server" ErrorMessage="Debe seleccionar el período de amortización."
                                                ControlToValidate="ddlFrecAmortizac" Display="None" />
                                        </td>
                                        <td>
                                            Tasa variable a partir de:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInicTasaVar" runat="server" onselectedindexchanged="formItem_Changed" AutoPostBack="true" onkeypress="validateOnlyNumbers(event)" />
                                            <asp:RequiredFieldValidator ID="reqValInicTasaVar" runat="server" ErrorMessage="Debe de ingresar el valor para tasa variable a partir de." ValidationGroup="valSummaryGroup"
                                                ControlToValidate="txtInicTasaVar" Display="None" />
                                            <asp:RegularExpressionValidator ID="rexValInicTasaVar" runat="server" ValidationGroup="valSummaryGroup" ErrorMessage="El valor para tasa variable no es correcto." ValidationExpression="^[0-9]+$" ControlToValidate="txtInicTasaVar" Display="None"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Con seguro de desgravamen:
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblSegDesgravamen" runat="server" RepeatDirection="Horizontal"
                                                OnSelectedIndexChanged="formItem_Changed" AutoPostBack="true">
                                                <asp:ListItem>SI</asp:ListItem>
                                                <asp:ListItem>NO</asp:ListItem>
                                            </asp:RadioButtonList>
                                            <asp:RequiredFieldValidator ID="reqValSegDesg" runat="server" ValidationGroup="valSummaryGroup" ErrorMessage="Debe de indicar si aplica seguro de desgravamen o no." Display="None" ControlToValidate="rblSegDesgravamen" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Con seguro de cesantía:
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblSegCesantia" runat="server" RepeatDirection="Horizontal"
                                                OnSelectedIndexChanged="formItem_Changed" AutoPostBack="true">
                                                <asp:ListItem>SI</asp:ListItem>
                                                <asp:ListItem>NO</asp:ListItem>
                                            </asp:RadioButtonList>
                                            <asp:RequiredFieldValidator ID="reqValSegCes" ValidationGroup="valSummaryGroup" runat="server" ErrorMessage="Debe indicar si aplica seguro de cesantía o no." Display="None" ControlToValidate="rblSegCesantia" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Tipo de póliza:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoPoliza" runat="server" />
                                            <asp:RequiredFieldValidator ID="reqValTipoPoliza" runat="server" ErrorMessage="Debe de seleccionar el tipo de póliza" Display="None" ValidationGroup="valSummaryGroup"
                                                ControlToValidate="ddlTipoPoliza" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Monto máximo:
                                        </td>
                                        <td>
                                           <div style="background: url('../Images/dollar.png') no-repeat; border: solid 1px #ABADB3; width: 134px; 
height: 19px; padding: 3px 0 0 15px;">
                                            <asp:TextBox ID="txtMontoMax" ReadOnly="true" runat="server" SkinId="txtTransparent" />
                                           </div>
                                        </td>
                                        <td>
                                            Monto solicitado:
                                        </td>
                                        <td>                                          
                                          <div style="background: url('../Images/dollar.png') no-repeat; border: solid 1px #ABADB3; width: 134px; 
height: 19px; padding: 3px 0 0 15px;">
                                            <asp:TextBox ID="txtMontoSol" runat="server" ReadOnly="true" onkeypress="validateOnlyNumbers(event)"  
SkinId="txtTransparent" />
                                          </div>
                                            <asp:RegularExpressionValidator ID="rexValMontoSol" runat="server" ValidationGroup="valSummaryGroup" 
ErrorMessage="El monto solicitado no es correcto." ValidationExpression="^(((\d{1,3})(.\d{3})*)|(\d+))(,\d+)?$" Display="None" 
ControlToValidate="txtMontoSol"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <div align="center"><uc1:AdvertenciaMoneda ID="AdvertenciaMoneda1" runat="server" Visible="false" /></div>
                                            <asp:ValidationSummary ID="valSummary" ValidationGroup="valSummaryGroup"  ShowMessageBox="true" ShowSummary="false" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            
                        </div>
                       </div>
                    </Content>
                </OfficeWebUI:OfficeWorkspace>
        </form>
    </div>
</body>
</html>