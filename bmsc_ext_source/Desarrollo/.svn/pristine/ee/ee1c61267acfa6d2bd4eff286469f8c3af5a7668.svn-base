<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="BusquedaDivisa.aspx.cs" Inherits="Efika.Crm.Web.SimuladorCredito.BusquedaDivisa" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Búsqueda de Moneda</title>
    <link href="../Styles/crm.css" rel="stylesheet" type="text/css" />
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
                                <OfficeWebUI:RibbonTab ID="RibbonTab101" runat="server" Text="Búsqueda de Moneda">
                                    <Groups>
                                        <OfficeWebUI:RibbonGroup ID="RibbonGroup1" runat="server" Text="">
                                            <Zones>
                                                <OfficeWebUI:GroupZone ID="GroupZone1" runat="server" Text="Zone 1">
                                                    <Content>
                                                        <OfficeWebUI:LargeItem ID="btnSeleccionar" runat="server" ImageUrl="~/Images/32/icon5_32.png"
                                                            OnClick="SeleccionarClick" Text="Seleccionar" Tooltip="<b>Selecciona moneda</b>" />
                                                        <OfficeWebUI:LargeItem ID="btnCerrar" runat="server" ImageUrl="~/Images/32/icon10_32.png" OnClick="BtnCerrarClick"
                                                             Text="Salir" Tooltip="<b>Cierra esta ventana</b>" />
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
            <asp:UpdateProgress ID="updProg1" AssociatedUpdatePanelID="pnlMain" runat="server">
                        <ProgressTemplate>
                            <img src="../waiting.gif" alt="Cargando" />
                        </ProgressTemplate>
                    </asp:UpdateProgress> 
            <asp:UpdatePanel ID="pnlMain" runat="server">
        <ContentTemplate>
            <div id="divControlesNoCliente">
                <asp:HiddenField ID="hdnCampoIdDivisa_parent" ClientIDMode="Static" Value="" runat="server" />
                <asp:HiddenField ID="hdnCampoCodIsoDivisa_parent" ClientIDMode="Static" Value="" runat="server" />
                <asp:HiddenField ID="hdnCampoNombreDivisa_parent" ClientIDMode="Static" Value="" runat="server" />
                <asp:HiddenField ID="hdnCampoSimboloDivisa_parent" ClientIDMode="Static" Value="" runat="server" />
                                
                <asp:GridView ID="gvDivisas" runat="server" OnSelectedIndexChanged="GvDivisasSelectedIndexChanged" OnRowDataBound="GvDivisasRowDataBound" OnRowCreated="GvDivisasRowCreated">
                    <SelectedRowStyle BackColor="#A7CDF0" />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
       </Content>
       </OfficeWebUI:OfficeWorkspace>
    </form>
</body>
</html>
