<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="SimuladorCredito.aspx.cs"
    Inherits="Efika.Crm.Web.SimuladorCredito.SimuladorCredito" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="../Common/AdvertenciaMoneda.ascx" tagname="AdvertenciaMoneda" tagprefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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

        function mostrarBusquedaDeMoneda(hdnFieldIdDivisa, hdnFieldCodDivisa, hdnFieldNameDivisa, hdnFieldSimboloDivisa) {
            var strCodUsuario = document.getElementById('hdnCodUsuario').value;

            var url = 'BusquedaDivisa.aspx?strCodigoUsuario=' + strCodUsuario + '&ctrlIdDivisa=' + hdnFieldIdDivisa;
            url += '&ctrlCodDivisa=' + hdnFieldCodDivisa + '&ctrlNombDivisa=' + hdnFieldNameDivisa + '&ctrlSimbDivisa=' + hdnFieldSimboloDivisa;

            var resBusqueda = window.open(url, '', 'location=no,menubar=no,status=no,toolbar=no,resizable=0,scrollbars=1,width=480,height=280')

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
            var numOferta = document.getElementById('lblNoOferta').innerText;
            var codIsoDivisa = document.getElementById('HdCodIsoDivisa').value;
            var simbDivisa = document.getElementById('HdSimboloDivisa').value;

            var url = 'CondicionesCredito.aspx?strCodigoUsuario=' + strCodUsuario + '&ProdId=' + prodId + '&FamId=' + prodFamId + '&TipoId=' + prodTipoId + '&ClienteId=' + clientId + '&SimCredId=' + simCredId + '&numProdsSim=' + numProdsSim + "&DivisaId=" + codDivisa + "&nombProd=" + nombProd + "&numOferta=" + numOferta + "&codIsoDivisa=" + codIsoDivisa + "&simbDivisa=" + simbDivisa;

            var resBusqueda = window.open(url, '', 'location=no,menubar=no,status=no,toolbar=no,resizable=0,scrollbars=0,width=580,height=525');
            //var resBusqueda = window.showModalDialog(url, '', 'scroll=1,dialogWidth=580,dialogHeight=540');
        }

        function abrirVentanaReporteProdSim(url) {
            window.open(url, '', 'location=no,menubar=no,status=no,toolbar=no,resizable=1,scrollbars=1,width=840,height=640');
        }

        function SetOfertaValorQryString() {
            var queryString = window.location.search;
            var iframe = document.getElementsByTagName("iframe")[0];
            var url = iframe.contentWindow.location.href;
            iframe.setAttribute("src", url + queryString);
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
                                                            OnClick="GuardarClick" Text="Guardar" Tooltip="<b>Guardar</b><br/>Guarda la simulación." />
                                                        <OfficeWebUI:LargeItem ID="btnGuardarCerrar" runat="server" ImageUrl="~/Images/32/SaveAndClose_32.png"
                                                            OnClick="GuardarCerrarClick" Text="Guardar y cerrar" Tooltip="<b>Guardar y cerrar</b><br/>Guarda la simulación y cierra esta ventana" />
                                                        <OfficeWebUI:LargeItem ID="btnCerrar" runat="server" ImageUrl="~/Images/32/icon10_32.png"
                                                            OnClick="CerrarClick" Text="Cerrar" Tooltip="<b>Cerrar</b><br/>Cierra esta ventana." />
                                                    </Content>
                                                </OfficeWebUI:GroupZone>                                                
                                            </Zones>
                                        </OfficeWebUI:RibbonGroup>
                                        <OfficeWebUI:RibbonGroup ID="RibbonGroup2" runat="server" Text="Oportunidades">
                                            <Zones>
                                                <OfficeWebUI:GroupZone ID="GroupZone2" runat="server" Text="Zone 2">
                                                    <Content>
                                                        <OfficeWebUI:LargeItem ID="btnGenOport" runat="server" ImageUrl="~/Images/32/shoppingCart32.png"
                                                            OnClick="GeneraOportunidadClick" ClientClick="javascript:this.disabled=true" Text="Generar oportunidades" Tooltip="<b>Generar oportunidades</b><br/>Tomando en cuenta los datos de esta simulación, se generan oportunidades." />
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
            <div>
                    <asp:UpdateProgress ID="updProg1" AssociatedUpdatePanelID="pnlMain" runat="server">
                        <ProgressTemplate>
                            <div style="position: absolute; top: 0; left: 0; width: 100%; height: 100%; z-index:100">
                                <div style="position: fixed; top: 50%; left: 50%; width: 401px; height: 150px; margin: -9em 0 0 -15em; border: 2px solid #000; background-color: #FFFFEE; filter: alpha(opacity=100);">
                                    <div style="position: fixed; top: 42%; left: 47%; text-align:center;">
                                        <img src="../Images/loading.gif" alt="Cargando" /><br /><br />
                                        <span style="color:#000099; font-weight: bold; font-size: 12">Cargando</span>
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
                <div style="margin: 10px 35px">
                    <asp:Label ID="lblClienteNombre" runat="server" style="text-transform: capitalize; font-weight: bold; font-size:larger;"></asp:Label>
                </div>
                <div style="width: 80%; height: 46px; margin: 15px 5px 0 40px">
                    <table>
                        <tr>
                            <td>No. Oferta:</td>
                            <td><asp:Label ID="lblNoOferta" ClientIDMode="Static" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Tipo de cliente:</td>
                            <td><asp:Label ID="lblTipoCliente" ClientIDMode="Static" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Fuente de ingreso:</td>
                            <td><asp:Label ID="lblFuenteIngreso" ClientIDMode="Static" runat="server" /></td>
                        </tr>                                             
                        <tr>
                            <td><asp:Label ID="lblCampania" Text="Campaña:" runat="server" Visible="false" /></td>
                            <td><asp:Label ID="txtCampania" runat="server" Visible="false" /></td>
                        </tr>
                        <tr>
                            <td>Moneda:</td>
                            <td>
                                <table style="width:140px">
                                    <tr>
                                        <td style="width: 98px"> 
                                            <asp:TextBox ID="txtMoneda" OnTextChanged="txtMoneda_TextChanged" AutoPostBack="True" ClientIDMode="Static" Width="100" BorderStyle="Solid" BorderWidth="1" BorderColor="AppWorkspace" BackColor="White" Height="17" runat="server" ></asp:TextBox>
                                            <asp:HiddenField ID="HdCodIsoDivisa" ClientIDMode="Static" runat="server"/>                            
                                            <asp:HiddenField ID="HdSimboloDivisa" ClientIDMode="Static" runat="server"/>                            
                                        </td>
                                        <td style="width: 30px"><asp:ImageButton ID="btnMonedaLookup" ImageUrl="~/Images/btn_off_lookup.png" 
                                            OnClientClick="mostrarBusquedaDeMoneda('hdnCodDivisa', 'HdCodIsoDivisa', 'txtMoneda','HdSimboloDivisa');"  runat="server" style="margin: 0 0 0 0px" />
                                        </td>                            
                                    </tr>   
                                </table>  
                            </td>
                        </tr>
                    </table>
                                           
                </div>
                <div >
                &nbsp;                 
                </div>

                </br>
                </br>  
                
                <asp:Panel ID="pnlOfertaValor" Visible="false" runat="server" style="padding-top: 15px">
                    <h4 style="margin: 0">Oferta de Valor del cliente</h4>
                    <iframe src="../OfertaValorMicro.aspx" style="margin: 0; padding: 0" id="frmOfertaValor" frameborder="0" scrolling="auto" width="1050" height="180"></iframe>
                </asp:Panel>
                
                <asp:Panel ID="pnlProductosCampania" Visible="false" runat="server">
                    <h4>Productos de Campaña</h4>
                    <asp:GridView ID="gvProductosCampania" runat="server" OnPageIndexChanging="GvProductosCampaniaPageIndexChanging" 
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
                    	    Página <%= gvProductosCampania.PageIndex + 1%>
                    	    <asp:ImageButton CommandName="Page" CommandArgument="Next" ID="pgnLinkNext" runat="server" ImageUrl="~/Images/grid/page_R1.gif">
                    	    </asp:ImageButton>
                    	    <asp:ImageButton CommandName="Page" CommandArgument="Last" ID="pgnLinkLast" runat="server" ImageUrl="~/Images/grid/page_LL1.gif">
                    	    </asp:ImageButton>
                          </div>
                        </PagerTemplate>
                    </asp:GridView>
                </asp:Panel>
                
                <h4>Productos y Servicios a simular</h4>
                <asp:HiddenField ID="hdnSimCredito" ClientIDMode="Static" runat="server" />
                <a name="opcCanasta"></a>
                <table style="width: 900px">
                    <tr>
                        <td style="width: 30px">Activo:</td>
                        <td style="width: 200px">
                            <asp:Label ID="txtActivo" ClientIDMode="Static" Width="200" BorderStyle="Solid" BorderWidth="1" BorderColor="AppWorkspace" BackColor="White" Height="17" runat="server" />
                            <asp:HiddenField ID="hdnProdActId" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnProdActTipoId" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnProdActFamId" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnClienteId" ClientIDMode="Static" runat="server" />
                        </td>
                        <td style="width: 30px"><asp:ImageButton ID="btnActivoLookup" ImageUrl="~/Images/btn_off_lookup.png" OnClientClick="document.location='#opcCanasta';return false;"  runat="server" style="margin: 1px 0 0 -5px" /></td>
                        <td style="width: 30px"><asp:ImageButton ID="btnAgregarActivo" runat="server" SkinID="addButton" ClientIDMode="Static"
                                OnClientClick="mostrarCondicionesCredito('hdnProdActId', 'hdnProdActTipoId', 'hdnProdActFamId');" OnClick="BtnAgregarActivoClick" /></td>
                        <td style="width: 30px">&nbsp;</td>
                        <td style="width: 30px">Pasivo:</td>
                        <td style="width: 200px">
                            <asp:Label ID="txtPasivo" ClientIDMode="Static" Width="200" BorderStyle="Solid" BorderWidth="1" BorderColor="AppWorkspace" BackColor="White" Height="17" runat="server" />
                            <asp:HiddenField ID="hdnProdPasId" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnProdPasTipoId" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnProdPasFamId" ClientIDMode="Static" runat="server" />                            
                        </td>
                        <td style="width: 30px"><asp:ImageButton ID="btnPasivoLookup" ImageUrl="~/Images/btn_off_lookup.png" OnClientClick="document.location='#opcCanasta';return false;" runat="server" style="margin: 1px 0 0 -5px" /></td>
                        <td style="width: 30px"><asp:ImageButton ID="btnAgregarPasivo" ClientIDMode="Static" runat="server" SkinID="addButton" Enabled="false" OnClick="BtnAgregarPasivoClick" /></td>
                        <td>&nbsp;</td>
                        <td style="width: 30px">Servicio:</td>
                        <td style="width: 200px">
                            <asp:Label ID="txtServicio" ClientIDMode="Static" Width="200" BorderStyle="Solid" BorderWidth="1" BorderColor="AppWorkspace" BackColor="White" Height="17" runat="server" />
                            <asp:HiddenField ID="hdnProdServId" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnProdServTipoId" ClientIDMode="Static" runat="server" />
                            <asp:HiddenField ID="hdnProdServFamId" ClientIDMode="Static" runat="server" />
                        </td>
                        <td style="width: 30px"><asp:ImageButton ID="btnServicioLookup" ImageUrl="~/Images/btn_off_lookup.png" OnClientClick="document.location='#opcCanasta';return false;" runat="server" style="margin: 1px 0 0 -5px" /></td>
                        <td style="width: 30px"><asp:ImageButton ID="btnAgregarServicio" ClientIDMode="Static" runat="server" SkinID="addButton" Enabled="false" OnClick="BtnAgregarServicioClick" /></td>
                    </tr>
                    <tr>
                        <td colspan="4" style="width: 350px;">
                            <asp:CollapsiblePanelExtender id="cpeActivo" runat="server"
                                TargetControlID="pnlActivo"
                                CollapsedSize="0"
                                ExpandedSize="200"
                                Collapsed="True"
                                ExpandControlID="btnActivoLookup"
                                CollapseControlID="btnActivoLookup"
                                AutoCollapse="False"
                                AutoExpand="False"
                                ScrollContents="True"
                                ImageControlID="Image1"
                                ExpandedImage="~/Images/btn_off_lookup_collapse.png"
                                CollapsedImage="~/Images/btn_off_lookup.png"
                                ExpandDirection="Vertical">
                            </asp:CollapsiblePanelExtender>
                            <asp:Panel ID="pnlActivo" runat="server">
                                <asp:Panel ID="pnlSearch" DefaultButton="btnBuscarActivo" runat="server">
                                    <table>
                                        <tr>
                                            <td><asp:TextBox ID="txtBusqProductoActivo" Width="200" runat="server" /></td>
                                            <td>
                                                <asp:ImageButton ID="btnBuscarActivo" SkinID="searchButton" OnClick="BtnBuscarClickActivo" runat="server" />
                                                <asp:ImageButton ID="btnClrBuscarActivo" SkinID="clearSearchButton" OnClick="BtnClearBuscarClickActivo" Visible="false" style="margin: -2px 0 0 0" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:GridView ID="gvProductosActivos" OnRowCreated="GvProdActivoSimRowCreated"
                                onselectedindexchanged="GvProdActivosSelectedIndexChanged" OnRowDataBound="GvProdActSimRowDataBound" style="width: 100%" runat="server">
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                        <td>&nbsp;</td>
                        <td colspan="4" style="width: 350px">
                            <asp:CollapsiblePanelExtender id="cpePasivo" runat="server"
                                TargetControlID="pnlPasivo"
                                CollapsedSize="0"
                                ExpandedSize="200"
                                Collapsed="True"
                                ExpandControlID="btnPasivoLookup"
                                CollapseControlID="btnPasivoLookup"
                                AutoCollapse="False"
                                AutoExpand="False"
                                ScrollContents="True"
                                ImageControlID="Image1"
                                ExpandedImage="~/Images/btn_off_lookup_collapse.png"
                                CollapsedImage="~/Images/btn_off_lookup.png"
                                ExpandDirection="Vertical">
                            </asp:CollapsiblePanelExtender>
                            <asp:Panel ID="pnlPasivo" runat="server">
                                <asp:Panel ID="pnlSearchPasivo" DefaultButton="btnBuscarPasivo" runat="server">
                                    <table>
                                        <tr>
                                            <td><asp:TextBox ID="txtBusqProductoPasivo" Width="200" runat="server" /></td>
                                            <td>
                                                <asp:ImageButton ID="btnBuscarPasivo" SkinID="searchButton" OnClick="BtnBuscarClickPasivo" runat="server" />
                                                <asp:ImageButton ID="btnClrBuscarPasivo" SkinID="clearSearchButton" OnClick="BtnClearBuscarClickPasivo" Visible="false" style="margin: -2px 0 0 0" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:GridView ID="gvProductosPasivos" OnRowCreated="GvProdPasivosSimRowCreated" OnRowDataBound="GvProdPasSimRowDataBound"
                                    onselectedindexchanged="GvProdPasivosSelectedIndexChanged" runat="server">
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                        <td>&nbsp;</td>
                        <td colspan="4" style="width: 350px">
                            <asp:CollapsiblePanelExtender id="cpeServicio" runat="server"
                                TargetControlID="pnlServicio"
                                CollapsedSize="0"
                                ExpandedSize="200"
                                Collapsed="True"
                                ExpandControlID="btnServicioLookup"
                                CollapseControlID="btnServicioLookup"
                                AutoCollapse="False"
                                AutoExpand="False"
                                ScrollContents="True"
                                ImageControlID="Image1"
                                ExpandedImage="~/Images/btn_off_lookup_collapse.png"
                                CollapsedImage="~/Images/btn_off_lookup.png"
                                ExpandDirection="Vertical">
                            </asp:CollapsiblePanelExtender>
                            <asp:Panel ID="pnlServicio" runat="server">
                                <asp:Panel ID="pnlSearchServicio" DefaultButton="btnBuscarServicio" runat="server">
                                    <table>
                                        <tr>
                                            <td><asp:TextBox ID="txtBusqProductoServicio" Width="200" runat="server" /></td>
                                            <td>
                                                <asp:ImageButton ID="btnBuscarServicio" SkinID="searchButton" OnClick="BtnBuscarClickServicio" runat="server" />
                                                <asp:ImageButton ID="btnClrBuscarServicio" SkinID="clearSearchButton" OnClick="BtnClearBuscarClickServicio" Visible="false" style="margin: -2px 0 0 0" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:GridView ID="gvProductosServicios" OnRowCreated="GvProdServiciosSimRowCreated"
                                     onselectedindexchanged="GvProdServiciosSelectedIndexChanged" OnRowDataBound="GvProdServSimRowDataBound" runat="server">
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                
                <h4>Productos y Servicios simulados</h4>
                <asp:GridView ID="gvProdServSim" runat="server" OnRowCreated="GvProductosSimuladosRowCreated"  OnRowDataBound="GvProductosSimuladosRowDataBound"
                    onselectedindexchanged="GvProdServSimSelectedIndexChanged" 
                    EnableSortingAndPagingCallbacks="True" ShowHeaderWhenEmpty="True" AutoGenerateColumns="false">
                    <Columns>
                        <asp:CommandField ButtonType="Image" ItemStyle-Width="18"
                            SelectImageUrl="~/Images/16/ico_18_reports.gif" ShowSelectButton="True" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEliminarProdSim" runat="server" ImageUrl="~/Images/16/ico_16_delete.gif" OnClick="BtnEliminarProdSimClick" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="IdProductoSimulado" HeaderText="IdProductoSimulado" />
                        <asp:BoundField DataField="CodTipoFamiliaProd" HeaderText="CodTipoFamiliaProd" />
                        <asp:BoundField DataField="ProductoNombre" HeaderText="Producto" ItemStyle-CssClass="crmSelectedTableCol" />
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
                <asp:HiddenField ID="hdnNumProdsSim" ClientIDMode="Static" runat="server" />
            </div>
        </ContentTemplate>
        </asp:UpdatePanel>
       </Content>
    </OfficeWebUI:OfficeWorkspace>
    
    </form>
</body>
<script language="javascript" type="text/javascript">
    if (window.addEventListener) {
        window.addEventListener('load', SetOfertaValorQryString, false); //W3C
    }
    else {
        window.attachEvent('onload', SetOfertaValorQryString); //IE
    }
</script>
</html>
