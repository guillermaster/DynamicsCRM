<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SimuladorCredito.aspx.cs"
    Inherits="Efika.Crm.Web.SimuladorCredito.SimuladorCredito" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="../Common/AdvertenciaMoneda.ascx" tagname="AdvertenciaMoneda" tagprefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Simulador de crédito</title>
    <link href="../Styles/crm.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function CerrarVentana() {
            window.close();
        }

        function MostrarProcesando() {
            document.getElementById("divControles").style.display = "none";
            document.getElementById("tblProcesando").style.display = "block";
        }

        function mostrarBusquedaDeProducto(hdnFieldNameFamsProds, hdnFieldNameTiposProds,
                    hdnFieldNameProdId, hdnFieldNameProdNomb, hdnFieldNameProdTipoId, hdnFieldNameProdFamId, hdnFieldProdNombHdn, homologado, btnAdd) {

            var strFamsProdsIds = document.getElementById(hdnFieldNameFamsProds).value;
            var strTiposProdsIds = document.getElementById(hdnFieldNameTiposProds).value;
            var strCodUsuario = document.getElementById('hdnCodUsuario').value;
            
            var url = 'BusquedaProducto.aspx?strCodigoUsuario=' + strCodUsuario + '&strCodsFamsProd=' + strFamsProdsIds;
            url += '&strCodsTiposProd=' + strTiposProdsIds + '&ctrlProdId=' + hdnFieldNameProdId + '&ctrlProdNomb=' + hdnFieldNameProdNomb;
            url += '&ctrlProdTipoId=' + hdnFieldNameProdTipoId + '&ctrlProdFamId=' + hdnFieldNameProdFamId + '&ctrlProdNombHdn=' + hdnFieldProdNombHdn + '&valHomologacion=' + homologado;

            var resBusqueda = window.open(url, '', 'location=no,menubar=no,status=no,toolbar=no,resizable=0,scrollbars=1,width=680,height=480')
            
        }

        function mostrarCondicionesCredito(hdnFieldNameProdId, hdnFieldNameProdTipoId, hdnFieldNameProdFamId) {

            var prodId = document.getElementById(hdnFieldNameProdId).value;
            var prodTipoId = document.getElementById(hdnFieldNameProdTipoId).value;
            var prodFamId = document.getElementById(hdnFieldNameProdFamId).value;
            var strCodUsuario = document.getElementById('hdnCodUsuario').value;
            var clientId = document.getElementById('hdnClienteId').value;
            var simCredId = document.getElementById('hdnSimCredito').value;
            var numProdsSim = document.getElementById('hdnNumProdsSim').value;
            var codDivisa = document.getElementById('hdnCodDivisa').value;
            var nombProd = document.getElementById('txtActivo').innerText;
            var numOferta = document.getElementById('txtNoOferta').value;

            var url = 'CondicionesCredito.aspx?strCodigoUsuario=' + strCodUsuario + '&ProdId=' + prodId + '&FamId=' + prodFamId + '&TipoId=' + prodTipoId + '&ClienteId=' + clientId + '&SimCredId=' + simCredId + '&numProdsSim=' + numProdsSim + "&DivisaId=" + codDivisa + "&nombProd=" + nombProd + "&numOferta=" + numOferta;

            var resBusqueda = window.open(url, '', 'location=no,menubar=no,status=no,toolbar=no,resizable=0,scrollbars=1,width=580,height=540');
            //var resBusqueda = window.showModalDialog(url, '', 'scroll=1,dialogWidth=580,dialogHeight=540');
        }

        function abrirVentanaReporteProdSim(url) {
            window.open(url, '', 'location=no,menubar=no,status=no,toolbar=no,resizable=1,scrollbars=1,width=840,height=640');
        }

</script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptMngr" runat="server">
    </asp:ScriptManager>
            <OfficeWebUI:Manager ID="Manager1" runat="server" UITheme="Office2010Silver" IncludeJQuery="true"
                ChromeUI="true" />
            <div style="margin: -20px 0 0 0">
                <OfficeWebUI:OfficeRibbon ID="OfficeRibbon1" runat="server" ApplicationMenuType="None"
                    ShowToggleButton="false">
                    <Contexts>
                        <OfficeWebUI:RibbonContext ID="RibbonContext1" runat="server" ContextColor="transparent"
                            Text="">
                            <Tabs>
                                <OfficeWebUI:RibbonTab ID="RibbonTab101" runat="server" Text="Simulación de crédito">
                                    <Groups>
                                        <OfficeWebUI:RibbonGroup ID="RibbonGroup1" runat="server" Text="Acciones">
                                            <Zones>
                                                <OfficeWebUI:GroupZone ID="GroupZone1" runat="server" Text="Zone 1">
                                                    <Content>
                                                        <OfficeWebUI:LargeItem ID="btnGuardar" runat="server" ImageUrl="~/Images/32/icon86_32.png"
                                                            OnClick="Guardar_Click" Text="Guardar" Tooltip="<b>Guardar</b><br/>Guarda la simulación." />
                                                        <OfficeWebUI:LargeItem ID="btnGuardarCerrar" runat="server" ImageUrl="~/Images/32/SaveAndClose_32.png"
                                                            OnClick="GuardarCerrar_Click" Text="Guardar y cerrar" Tooltip="<b>Guardar y cerrar</b><br/>Guarda la simulación y cierra esta ventana" />
                                                        <OfficeWebUI:LargeItem ID="btnCerrar" runat="server" ImageUrl="~/Images/32/icon10_32.png"
                                                            OnClick="Cerrar_Click" Text="Cerrar" Tooltip="<b>Cerrar</b><br/>Cierra esta ventana." />
                                                    </Content>
                                                </OfficeWebUI:GroupZone>                                                
                                            </Zones>
                                        </OfficeWebUI:RibbonGroup>
                                        <OfficeWebUI:RibbonGroup ID="RibbonGroup2" runat="server" Text="Oportunidades">
                                            <Zones>
                                                <OfficeWebUI:GroupZone ID="GroupZone2" runat="server" Text="Zone 2">
                                                    <Content>
                                                        <OfficeWebUI:LargeItem ID="btnGenOport" runat="server" ImageUrl="~/Images/32/shoppingCart32.png"
                                                            OnClick="GeneraOportunidad_Click" ClientClick="javascript:this.disabled=true" Text="Generar oportunidades" Tooltip="<b>Generar oportunidades</b><br/>Tomando en cuenta los datos de esta simulación, se generan oportunidades." />
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
    <OfficeWebUI:OfficeWorkspace ID="Workspace1"  runat="server">
      <Content>
            <div>
                    <asp:UpdateProgress ID="updProg1" AssociatedUpdatePanelID="pnlMain" runat="server">
                        <ProgressTemplate>
                            <div style="position: absolute; top: 0; left: 0; width: 100%; height: 100%; z-index:100">
                                <div style="position: fixed; top: 50%; left: 50%; width: 401px; height: 150px; margin: -9em 0 0 -15em; border: 2px solid #000; background-color: #FFFFEE; filter: alpha(opacity=100);">
                                    <div style="position: fixed; top: 42%; left: 47%; text-align:center;">
                                        <img src="../waiting.gif" alt="Cargando" /><br /><br />
                                        <span style="color:#000099; font-weight: bold; font-size: 12">Procesando...</span>
                                    </div>
                                </div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress> 
                </div>
            <asp:UpdatePanel ID="pnlMain" runat="server">
        <ContentTemplate>
            <div id="divControles">
                <asp:HiddenField ID="hdnCodUsuario" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="hdnCodDivisa" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="hdnCargarProdsSims" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="hdnCodCampania" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="hdnSimGuardada" ClientIDMode="Static" Value="0" runat="server" />
                <table>
                    <tr>
                        <td>No. Oferta:</td>
                        <td><asp:TextBox ID="txtNoOferta" ClientIDMode="Static" Width="200" ReadOnly="true" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>
                            Cliente:
                        </td>
                        <td>
                            <asp:TextBox ID="txtClienteNombre" Width="400" ReadOnly="true" runat="server" />
                        </td>
                        <td>
                            Tipo de cliente:
                        </td>
                        <td>
                            <asp:TextBox ID="txtTipoCliente" Width="120" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Fuente de ingreso:
                        </td>
                        <td>
                            <asp:TextBox ID="txtFuenteIngreso" Width="400" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCampania" Text="Campaña:" runat="server" Visible="false" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtCampania" runat="server" ReadOnly="true" Width="400" Visible="false" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlOfertaValor" Visible="false" runat="server">
                    <h4>Oferta de Valor del cliente</h4>
                    <asp:GridView ID="gvOfertaValor" runat="server" OnPageIndexChanging="gvOfertaValor_PageIndexChanging" 
                        EnableSortingAndPagingCallbacks="True" ShowHeaderWhenEmpty="True" PageSize="5" AllowPaging="true" AutoGenerateColumns="false">
                        <PagerSettings  Mode="NextPreviousFirstLast" />
                        <Columns>
                            <asp:BoundField DataField="PrioridadSubtipoProducto" HeaderText="Prioridad S.P." ItemStyle-Width="85px" />
                            <asp:BoundField DataField="SubtipoProducto" HeaderText="Subtipo de Producto" />
                            <asp:BoundField DataField="PrioridadPortafolio" HeaderText="Prioridad Port." ItemStyle-Width="85px" />
                            <asp:BoundField DataField="Portafolio" HeaderText="Portafolio" />
                        </Columns>
                        <PagerTemplate>
                         <div style="float: right; width: 600px;" align="right">
                    	    <asp:ImageButton CommandName="Page" CommandArgument="First" ID="pgnLinkFirst" runat="server" ImageUrl="~/Images/grid/page_FL1.gif">
                            </asp:ImageButton>
                    	    <asp:ImageButton CommandName="Page" CommandArgument="Prev" ID="pgnLinkPrev" runat="server" ImageUrl="~/Images/grid/page_L1.gif">
                            </asp:ImageButton>
                    	    Página <%= gvOfertaValor.PageIndex + 1%>
                    	    <asp:ImageButton CommandName="Page" CommandArgument="Next" ID="pgnLinkNext" runat="server" ImageUrl="~/Images/grid/page_R1.gif">
                    	    </asp:ImageButton>
                    	    <asp:ImageButton CommandName="Page" CommandArgument="Last" ID="pgnLinkLast" runat="server" ImageUrl="~/Images/grid/page_LL1.gif">
                    	    </asp:ImageButton>
                          </div>
                        </PagerTemplate>
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel ID="pnlProductosCampania" Visible="false" runat="server">
                    <h4>Productos de Campaña</h4>
                    <asp:GridView ID="gvProductosCampania" runat="server" OnPageIndexChanging="gvProductosCampania_PageIndexChanging" 
                        EnableSortingAndPagingCallbacks="True" ShowHeaderWhenEmpty="True" PageSize="5" AllowPaging="true" AutoGenerateColumns="false">
                        <PagerSettings  Mode="NextPreviousFirstLast" />
                        <Columns>
                            <asp:BoundField DataField="nombreProducto" HeaderText="Subtipo de Producto" />
                            <asp:BoundField DataField="familiaProducto" HeaderText="Familia" />
                            <asp:BoundField DataField="tipoProducto" HeaderText="Tipo" />
                            <asp:BoundField DataField="puedeComercializar" HeaderText="Comercializable" />
                            <asp:BoundField DataField="cupoMonto" HeaderText="Cupo" />
                        </Columns>
                        <PagerTemplate>
                         <div style="float: right; width: 600px;" align="right">
                    	    <asp:ImageButton CommandName="Page" CommandArgument="First" ID="pgnLinkFirst" runat="server" ImageUrl="~/Images/grid/page_FL1.gif">
                            </asp:ImageButton>
                    	    <asp:ImageButton CommandName="Page" CommandArgument="Prev" ID="pgnLinkPrev" runat="server" ImageUrl="~/Images/grid/page_L1.gif">
                            </asp:ImageButton>
                    	    Página <%= gvOfertaValor.PageIndex + 1%>
                    	    <asp:ImageButton CommandName="Page" CommandArgument="Next" ID="pgnLinkNext" runat="server" ImageUrl="~/Images/grid/page_R1.gif">
                    	    </asp:ImageButton>
                    	    <asp:ImageButton CommandName="Page" CommandArgument="Last" ID="pgnLinkLast" runat="server" ImageUrl="~/Images/grid/page_LL1.gif">
                    	    </asp:ImageButton>
                          </div>
                        </PagerTemplate>
                    </asp:GridView>
                </asp:Panel>
                <h4>Productos y Servicios a simular</h4>
                <asp:HiddenField ID="hdnIdsValSeparator" ClientIDMode="Static" Value="/" runat="server" />
                <asp:HiddenField ID="hdnSimCredito" ClientIDMode="Static" runat="server" />
                <table>
                    <tr>
                        <td>Activo:</td>
                        <td>
                            <asp:Label ID="txtActivo" ClientIDMode="Static" Width="200" BorderStyle="Solid" BorderWidth="1" BorderColor="AppWorkspace" BackColor="White" Height="17" runat="server" />
                            <asp:HiddenField ID="hdnProdActNomb" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnTiposProdActIds" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnFamsProdActIds" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnProdActId" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnProdActTipoId" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnProdActFamId" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnClienteId" ClientIDMode="Static" runat="server" />
                        </td>
                        <td><asp:ImageButton ID="btnActivoLookup" SkinID="lookupButton" runat="server" 
                                OnClientClick="mostrarBusquedaDeProducto('hdnFamsProdActIds','hdnTiposProdActIds','hdnProdActId','txtActivo','hdnProdActTipoId','hdnProdActFamId', 'hdnProdActNomb','1','btnAgregarActivo');"
                                 OnClick="btnBusquedaActivo_Click" /></td>
                        <td><asp:ImageButton ID="btnAgregarActivo" runat="server" SkinID="addButton" ClientIDMode="Static"
                                OnClientClick="mostrarCondicionesCredito('hdnProdActId', 'hdnProdActTipoId', 'hdnProdActFamId');" OnClick="btnAgregarActivo_Click" /></td>
                    </tr>
                    <tr>
                        <td>Pasivo:</td>
                        <td>
                            <asp:Label ID="txtPasivo" ClientIDMode="Static" Width="200" BorderStyle="Solid" BorderWidth="1" BorderColor="AppWorkspace" BackColor="White" Height="17" runat="server" />
                            <asp:HiddenField ID="hdnProdPasNomb" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnTiposProdPasIds" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnFamsProdPasIds" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnProdPasId" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnProdPasTipoId" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnProdPasFamId" ClientIDMode="Static" runat="server" />
                        </td>
                        <td><asp:ImageButton ID="btnPasivoLookup" SkinID="lookupButton" runat="server" Enabled="false"
                                OnClientClick="mostrarBusquedaDeProducto('hdnFamsProdPasIds','hdnTiposProdPasIds','hdnProdPasId','txtPasivo','hdnProdPasTipoId','hdnProdPasFamId', 'hdnProdPasNomb', '0', 'btnAgregarPasivo');" OnClick="btnBusquedaPasivo_Click" /></td>
                        <td><asp:ImageButton ID="btnAgregarPasivo" ClientIDMode="Static" runat="server" SkinID="addButton" Enabled="false" OnClick="btnAgregarPasivo_Click" /></td>
                    </tr>
                    <tr>
                        <td>Servicio:</td>
                        <td>
                            <asp:Label ID="txtServicio" ClientIDMode="Static" Width="200" BorderStyle="Solid" BorderWidth="1" BorderColor="AppWorkspace" BackColor="White" Height="17" runat="server" />
                            <asp:HiddenField ID="hdnProdServNomb" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnTiposProdServIds" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnFamsProdServIds" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnProdServId" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnProdServTipoId" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnProdServFamId" ClientIDMode="Static" runat="server" />
                        </td>
                        <td><asp:ImageButton ID="btnServicioLookup" SkinID="lookupButton" runat="server" Enabled="false" 
                            OnClientClick="mostrarBusquedaDeProducto('hdnFamsProdServIds','hdnTiposProdServIds','hdnProdServId','txtServicio','hdnProdServTipoId','hdnProdServFamId', 'hdnProdServNomb', '0', 'btnAgregarServicio');"
                             OnClick="btnBusquedaServicio_Click" /></td>
                        <td><asp:ImageButton ID="btnAgregarServicio" ClientIDMode="Static" runat="server" SkinID="addButton" Enabled="false" OnClick="btnAgregarServicio_Click" /></td>
                    </tr>
                </table>
                <h4>Productos y Servicios simulados</h4>
                <asp:GridView ID="gvProdServSim" runat="server" OnRowCreated="gvProductosSimulados_RowCreated"  OnRowDataBound="gvProductosSimulados_RowDataBound"
                    onselectedindexchanged="gvProdServSim_SelectedIndexChanged" SkinID="grvHeader2"
                    EnableSortingAndPagingCallbacks="True" ShowHeaderWhenEmpty="True" AutoGenerateColumns="false">
                    <Columns>
                        <asp:CommandField ButtonType="Image" ItemStyle-Width="18"
                            SelectImageUrl="~/Images/16/ico_18_reports.gif" ShowSelectButton="True" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEliminarProdSim" runat="server" ImageUrl="~/Images/16/ico_16_delete.gif" OnClick="btnEliminarProdSim_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="IdProductoSimulado" HeaderText="IdProductoSimulado" />
                        <asp:BoundField DataField="CodTipoFamiliaProd" HeaderText="CodTipoFamiliaProd" />
                        <asp:BoundField DataField="ProductoNombre" HeaderText="Producto" />
                        <asp:BoundField DataField="MontoMaximo" HeaderText="Monto máximo" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="CuotaMaxima" HeaderText="Cuota máxima" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="MontoPactado" HeaderText="Monto solicitado" ItemStyle-HorizontalAlign="Right" />                        
                        <asp:BoundField DataField="CuotaPactada" HeaderText="Cuota máxima solicitada" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="ProductoId" />
                        <asp:BoundField DataField="ProductoTipoId" />
                        <asp:BoundField DataField="ProductoFamId" />
                        <asp:BoundField DataField="Plazo" HeaderText="Plazo" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="TasaFija" HeaderText="Tasa Fija" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="TRE" HeaderText="TRE" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="SpreadFijo" HeaderText="Spread" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="TasaVarPar" />
                        <asp:BoundField DataField="Orden" />
                        <asp:BoundField DataField="AmortizCada" />
                        <asp:BoundField DataField="SegCesantia" />
                        <asp:BoundField DataField="SegDesgrav" />
                        <asp:BoundField DataField="TipoPolizaCod" />
                        <asp:BoundField DataField="Nombre" />
                        <asp:BoundField DataField="MonedaOp" />
                    </Columns>
                </asp:GridView>
                <div align="center"><uc1:AdvertenciaMoneda ID="AdvertenciaMoneda1" runat="server" Visible="false" /></div>
                <asp:HiddenField ID="hdnNumProdsSim" ClientIDMode="Static" runat="server" />
            </div>
        </ContentTemplate>
        </asp:UpdatePanel>
       </Content>
    </OfficeWebUI:OfficeWorkspace>
    
    </form>
</body>
</html>
