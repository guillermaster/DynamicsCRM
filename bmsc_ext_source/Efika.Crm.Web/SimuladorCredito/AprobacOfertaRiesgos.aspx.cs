using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Efika.Crm.Negocio;

namespace Efika.Crm.Web.SimuladorCredito
{
    public partial class AprobacOfertaRiesgos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigurarPagina();

            if (!IsPostBack)
            {
                if (!Utilidades.ValidarAccesosWeb(Request.QueryString[Utilidades.NombresParametros.Usuario], Privilegios.NombresPrivilegios.ReabrirOferta))
                {
                    Response.Write(@"<script type=""text/javascript""> alert('No tiene privilegios para acceder a este formulario.'); window.close(); </script>");
                    return;
                }
            }

            try
            {
                bool aprobada;
                if (Request.QueryString[Utilidades.NombresParametros.Aprobacion] == "1")
                    aprobada = true;
                else
                    aprobada = false;
                SetAprobacion(int.Parse(Request.QueryString[Utilidades.NombresParametros.NumeroOferta]), aprobada);
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "El número de oferta es incorrecto.", "REABRIR_OFERTA_RIESGOS", ex);
                CerrarVentana(false);
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


        private void SetAprobacion(int numOferta, bool ofertaAprobada)
        {
            try
            {
                Negocio.Oferta negOferta = new Oferta(Credenciales.ObtenerCredenciales());
                negOferta.AprobarOfertaRiesgos(numOferta, ofertaAprobada);

                string mensaje = "<script type=\"text/javascript\"> alert(\"La oferta ha sido " + (ofertaAprobada? "aprobada" : "rechazada") + " exitosamente. \");</script>";
                ClientScript.RegisterStartupScript(GetType(), "APROBACION_OFERTA_RIESGOS", mensaje);
                CerrarVentana(true);
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "No se pudo actualizar el estado de aprobación.", "APROBACION_OFERTA_RIESGOS", ex);
                CerrarVentana(false);
            }
        }


        private void CerrarVentana(bool exito)
        {
            string jscript1 = "<script type=\"text/javascript\"> window.returnValue = " + (exito ? "1" : "0") + "  </script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "RETORNAR_APROBACION_OFERTARIESGOS", jscript1);
            string jscript2 = "<script type=\"text/javascript\"> window.close(); </script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "CERRAR_VENTANA_APROBACION_OFERTARIESGOS", jscript2);
        }
    }
}