using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Efika.Crm.Negocio;

namespace Efika.Crm.Web.SimuladorCredito
{
    public partial class ReabrirOfertaGerentes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigurarPagina();

            if (!IsPostBack)
            {
                if (!Utilidades.ValidarAccesosWeb(Request.QueryString[Utilidades.NombresParametros.Usuario], Privilegios.NombresPrivilegios.ReabrirOfertaGerente))
                {
                    Response.Write(@"<script type=""text/javascript""> alert('No tiene privilegios para acceder a este formulario.'); window.close(); </script>");
                    return;
                }
            }

            try
            {
                AbrirOferta(int.Parse(Request.QueryString[Utilidades.NombresParametros.NumeroOferta]));
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "El número de oferta es incorrecto.", "REABRIR_OFERTA_GERENTE", ex);
                CerrarVentana(false);
            }
        }


        private void AbrirOferta(int numOferta)
        {
            try
            {
                Negocio.Oferta negOferta = new Oferta(Credenciales.ObtenerCredenciales());
                if (!negOferta.ReabrirOfertaGerentes(numOferta))
                {
                    Utilidades.Alert(this, "REABRIR_OFERTA_GERENTE", negOferta.Error); 
                }
                CerrarVentana(true);
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "No se pudo reabrir oferta.", "REABRIR_OFERTA_GERENTE", ex);
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

        private void CerrarVentana(bool exito)
        {
            string jscript1 = "<script type=\"text/javascript\"> window.returnValue = " + (exito ? "1" : "0") + "  </script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "RETORNAR_REABRIR_OFERTA_GERENTE", jscript1);
            string jscript2 = "<script type=\"text/javascript\"> window.close(); </script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "CERRAR_VENTANA_REABRIR_OFERTA_GERENTE", jscript2);
        }
    }
}