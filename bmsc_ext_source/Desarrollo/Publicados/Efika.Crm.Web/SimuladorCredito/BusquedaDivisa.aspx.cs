using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Efika.Crm.Entidades;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Negocio;
using Efika.Crm.AccesoServicios.CRMSDK;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Web.SimuladorCredito
{
    public partial class BusquedaDivisa : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Utilidades.ValidarAccesosWeb(Request.QueryString[Utilidades.NombresParametros.Usuario], Privilegios.NombresPrivilegios.SimulacionCredito))
                {
                    Response.Write(@"<script type=""text/javascript""> alert('No tiene privilegios para acceder a este formulario.'); window.close(); </script>");
                    return;
                }

                try
                {                    
                    Workspace1.ShowLeftPanel = false;
                    CargarParametrosDivisa();
                    LoadDivisas();
                }
                catch (Exception ex)
                {
                    Utilidades.ReportarError(this.Page, Request.QueryString[Utilidades.NombresParametros.Usuario],
                        "Ha ocurrido un error mientras se cargaban los datos iniciales.", "SIMULACCRED_CONDCRED_LOAD", ex);
                }
            }
        }

        private void CargarParametrosDivisa()
        {
            hdnCampoIdDivisa_parent.Value = Request.QueryString["ctrlIdDivisa"];
            hdnCampoCodIsoDivisa_parent.Value = Request.QueryString["ctrlCodDivisa"];
            hdnCampoNombreDivisa_parent.Value = Request.QueryString["ctrlNombDivisa"];
            hdnCampoSimboloDivisa_parent.Value = Request.QueryString["ctrlSimbDivisa"];            
        }

        #region Eventos de Botones del Ribbon
        protected void SeleccionarClick(object sender, EventArgs e)
        {
            if (gvDivisas.SelectedIndex == -1)
            {
                string mensaje = @"<script type=""text/javascript""> alert(""No ha seleccionado ninguna moneda."");</script>";
                this.ClientScript.RegisterStartupScript(this.GetType(), "BUSQUEDA", mensaje);
                return;
            }

            SeleccionarMoneda();
        }
        protected void BtnCerrarClick(object sender, EventArgs e)
        {
            Response.Write("<script type=\"text/javascript\"> window.close(); </script>");
        }
        #endregion
        #region Eventos del gridview
        protected void GvDivisasRowDataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // javascript function to call on row-click event
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvDivisas, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
                e.Row.Attributes.Add("title", "Seleccionar");
            }
        }
        protected void GvDivisasSelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void GvDivisasRowCreated(object sender, GridViewRowEventArgs e)
        {
            //Just changed the index of cells based on your requirements
            foreach (TableRow row in gvDivisas.Controls[0].Controls)
            {
                row.Cells[0].Visible = false;
            }

        }
        #endregion


        public void LoadDivisas()
        {
            try
            {
                Efika.Crm.Negocio.Divisa Divisas = new Efika.Crm.Negocio.Divisa(Credenciales.ObtenerCredenciales());
                DataTable dtDivisas = Divisas.ConsultaDivisas();
                gvDivisas.DataSource = dtDivisas;                
                gvDivisas.DataBind();
            }

            catch (Exception ex)
            {
                Utilidades.ReportarError(this.Page, "", "Error de carga de formulario de Búsqueda de Divisas", "Formulario de Búsquedas de Divisas", ex);
            }
        }

        private void SeleccionarMoneda()
        {

            string transactionCurrencyId = gvDivisas.SelectedRow.Cells[0].Text;
            string currencyName = System.Web.HttpUtility.HtmlDecode(gvDivisas.SelectedRow.Cells[1].Text);
            string isoCurrenCode = gvDivisas.SelectedRow.Cells[2].Text;            
            string currencySymbol = gvDivisas.SelectedRow.Cells[3].Text;
            RetornarValores(transactionCurrencyId, isoCurrenCode, currencyName, currencySymbol);
        }

        private void RetornarValores(string transactionCurrencyId, string isoCurrenCode, string currencyName, string currencySymbol)
        {
            string MonedaNombre = Utilidades.ReemplazarCaracteresEspeciales(currencyName);

            string scriptDivisaId = "window.opener.document.getElementById(\"" + hdnCampoIdDivisa_parent.Value + "\").value = \"" + transactionCurrencyId + "\"; ";
            string scriptDivisaCod = "window.opener.document.getElementById(\"" + hdnCampoCodIsoDivisa_parent.Value + "\").value = \"" + isoCurrenCode + "\"; ";
            string scriptDivisaName = "window.opener.document.getElementById(\"" + hdnCampoNombreDivisa_parent.Value + "\").value = \"" + MonedaNombre + "\"; ";
            string scriptDivisaSimbolo = "window.opener.document.getElementById(\"" +hdnCampoSimboloDivisa_parent.Value + "\").value = \"" +currencySymbol + "\"; ";

            Response.Write("<script type=\"text/javascript\"> " + scriptDivisaId + scriptDivisaCod + scriptDivisaName +
                            scriptDivisaSimbolo + " window.returnValue='S'; window.close(); </script>");
        }
    }
}