using System;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Reflection;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Negocio;
using Efika.Crm.Entidades;

namespace Efika.Crm.Web
{
    public partial class CerrarOportunidad : System.Web.UI.Page
    {

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAceptar.OnClientClick = "MostrarProcesando();";
            ConfigurarPagina();

            if (!IsPostBack)
            {

                try
                {
                    if (!Utilidades.ValidarAccesosWeb(Request.QueryString[Utilidades.NombresParametros.Usuario], Privilegios.NombresPrivilegios.CierreOportunidad))                    
                    {
                        Response.Write(@"<script type=""text/javascript""> alert('No tiene privilegios para acceder a este formulario.'); window.close(); </script>");
                        return;
                    }
                    hdnIdOportunidad.Value = Request.QueryString["IdOportunidad"];
                    CargarValoresRadioButtonsTipoCierre();
                    CargarFechaCierre();
                    CargarMonto();
                }
                catch (Exception ex)
                {
                    Utilidades.AgregarErrorAlLog(ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        Request.QueryString[Utilidades.NombresParametros.Usuario], ex.StackTrace);
                    Utilidades.ReportarError(this.Page, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al cargar datos de oportunidad.", "CIERRE_OPORTUNIDAD", ex);
                }
            }
        }
        
        private void ConfigurarPagina()
        {
            Response.Expires = 0;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Response.AddHeader("pragma", "no-cache");
            Response.AddHeader("cache-control", "private");
            Response.CacheControl = "no-cache";
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarListaRazones();
            if (rbnEstadoOportunidad.SelectedValue == Negocio.CierreOportunidad.TipoCierre.Perdida.ToString())
                CargarListaCompetidores();
            else
                ddlCompetidor.Enabled = false;
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbnEstadoOportunidad.SelectedValue == Negocio.CierreOportunidad.TipoCierre.Perdida.ToString()
                    && String.IsNullOrWhiteSpace(ddlCompetidor.SelectedValue))
                {
                    this.ClientScript.RegisterStartupScript(this.GetType(), "CIERREOPORTUNIDAD_CARGA", @"<script type=""text/javascript""> alert('Favor seleccione un competidor.');</script>");
                    return;
                }

                lblMensaje.Text = string.Empty;
                CerrarOportunidadActual();
                
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this.Page, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al cerrar oportunidad.", "CIERREOPORTUNIDAD_CERRAR", ex);
            }
        }


        #endregion


        #region Métodos
          

        private void CerrarOportunidadActual()
        {
            try
            {
                string error = string.Empty;
                Guid idOportunidad = new Guid(hdnIdOportunidad.Value);
                Guid idCompetidor = Guid.Empty;

                if (!string.IsNullOrWhiteSpace(ddlCompetidor.SelectedValue))
                    idCompetidor = new Guid(ddlCompetidor.SelectedValue);

                decimal monto = Decimal.Parse(txtIngresosReales.Text);
                
                Negocio.CierreOportunidad cierreOp = new Negocio.CierreOportunidad(Credenciales.ObtenerCredenciales());
                if (cierreOp.CerrarOportunidad(idOportunidad, rbnEstadoOportunidad.SelectedValue, int.Parse(ddlRazonEstado.SelectedValue),
                    idCompetidor, monto, txtDescripcion.Text, ref error))
                {
                    string mensaje = @"<script type=""text/javascript""> alert(""La oportunidad " + rbnEstadoOportunidad.SelectedValue + @" ha sido cerrada."");</script>";
                    this.ClientScript.RegisterStartupScript(this.GetType(), "CERRAR_OPORTUNIDAD_EXITO", mensaje);
                    //Al no causar error entonces el cierre fue exitoso
                    Response.Write(@"<script type=""text/javascript"">  window.returnValue='S'; window.close(); </script>");                    
                }
                else
                {
                    string mensaje = @"<script type=""text/javascript""> alert(""" + error + @""");</script>";
                    this.ClientScript.RegisterStartupScript(this.GetType(), "CERRAR_OPORTUNIDAD_FALLO", mensaje);
                }

            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this.Page, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al cerrar oportunidad", "CERRAROPORTUNIDAD", ex);
            }

        }       
        

        #endregion
        

        #region Controles
        private void CargarValoresRadioButtonsTipoCierre()
        {
            rbnEstadoOportunidad.Items.Add(CierreOportunidad.TipoCierre.Ganada.ToString());
            rbnEstadoOportunidad.Items.Add(CierreOportunidad.TipoCierre.Perdida.ToString());
        }

        private void CargarListaRazones()
        {
            ddlRazonEstado.Items.Clear();
            List<ListItem> listaRazones = Negocio.CierreOportunidad.ListaRazones(rbnEstadoOportunidad.SelectedValue);
            foreach (ListItem li in listaRazones)
            {
                ddlRazonEstado.Items.Add(li);
            }

        }

        private void CargarFechaCierre()
        {
            txtFechaCierre.Text = DateTime.Now.ToString();
        }

        private void CargarListaCompetidores()
        {

            ddlCompetidor.Items.Clear();
            ddlCompetidor.Enabled = true;
            CredencialesCRM credenciales = Credenciales.ObtenerCredenciales();
            //crear instancia de clase cierreoportunidad
            Negocio.CierreOportunidad cierreOport = new Negocio.CierreOportunidad(credenciales);
            //leer todos los competidores
            List<ListItem> listaComp = cierreOport.ListaCompetidores();
            //cargar dropdownlist con los valores en la lista
            foreach (ListItem li in listaComp)
            {
                ddlCompetidor.Items.Add(li);
            }
            

        }

        private void CargarMonto()
        {
            CredencialesCRM credenciales = Credenciales.ObtenerCredenciales();
            //crear instancia de clase cierreoportunidad
            Negocio.CierreOportunidad cierreOport = new Negocio.CierreOportunidad(credenciales);
            //leer el monto de la oportunidad y asignarlo al textbox correspondiente
            txtIngresosReales.Text = cierreOport.Monto(new Guid(hdnIdOportunidad.Value)).ToString();
        }

        #endregion

    }
}