using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Efika.Crm.Negocio;

namespace Efika.Crm.Web.SimuladorCredito
{
    public partial class CerrarOferta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigurarPagina();

            if (!IsPostBack)
            {
                if (!Utilidades.ValidarAccesosWeb(Request.QueryString[Utilidades.NombresParametros.Usuario], Privilegios.NombresPrivilegios.EnvioDatosOportunidadSistExt))
                {
                    Response.Write(@"<script type=""text/javascript""> alert('No tiene privilegios para acceder a este formulario.'); window.close(); </script>");
                    return;
                }
            }

            try
            {
                int numeroOferta = int.Parse(Request.QueryString[Utilidades.NombresParametros.NumeroOferta]);
                if (OfertaConOportunidadesMontoCero(numeroOferta))
                {
                    Entidades.EvaluacionScore evalScoreRes = GetEvalScoreResult(new Guid(Request.QueryString[Utilidades.NombresParametros.ClienteId]));
                    Negocio.Oferta negOferta = new Oferta(Credenciales.ObtenerCredenciales());
                    
                }
                else
                {
                    Utilidades.Alert(this, "En la Canasta existen Productos del activo con Monto Solicitado en 0, debe cerrar estos productos como Perdidos para continuar.", "Cerrar Oferta");
                }
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario],
                        ex.Message, "CERRAR_OFERTA", ex);
                CerrarVentana();
            }
        }


        private Entidades.EvaluacionScore GetEvalScoreResult(Guid idCliente)
        {
            try
            {
                Negocio.ScoreEvaluate negScoreEval = new Negocio.ScoreEvaluate(Credenciales.ObtenerCredenciales());                
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool OfertaConOportunidadesMontoCero(int numOferta)
        {
            try
            {
                Negocio.Oferta negOferta = new Negocio.Oferta(Credenciales.ObtenerCredenciales());                
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
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

        private void CerrarVentana()
        {
            string jscript1 = "<script type=\"text/javascript\"> window.returnValue = 1  </script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "CERRAR_OFERTA", jscript1);
            string jscript2 = "<script type=\"text/javascript\"> window.close(); </script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "CERRAR_OFERTA_WNDCLS", jscript2);
        }
    }
}