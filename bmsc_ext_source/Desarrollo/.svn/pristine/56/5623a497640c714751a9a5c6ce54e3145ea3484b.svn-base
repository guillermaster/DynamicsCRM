using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Efika.Crm.Negocio;
using Efika.Crm.AccesoServicios.CRMSDK;


namespace Efika.Crm.Web.SimuladorCredito
{
    public partial class EvaluarSimulacCreditoOportunidad : System.Web.UI.Page
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
                Entidades.Oportunidad oportunidad = GetOportunidad(new Guid(Request.QueryString[Utilidades.NombresParametros.OportunidadId]));
                if (ClienteTieneIngresosModeloEval(oportunidad.ClienteId))
                {
                    Evaluar(oportunidad);
                }
                else
                {
                    throw new Exception("El cliente no registra ingresos en los datos del modelo evaluador.");
                }
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario],
                        ex.Message, "EVALUAR_SIMULACIONCREDITO_OPORTUNIDAD", ex);
                CerrarVentana(0);
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

        private bool ClienteTieneIngresosModeloEval(Guid idCliente)
        {
            try
            {
                Cliente entCliente = new Cliente(Credenciales.ObtenerCredenciales());
                decimal ingresos = entCliente.GetIngresosModeloEvaluador(idCliente);
                if (ingresos > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void Evaluar(Entidades.Oportunidad oportunidad)
        {
            try
            {
                Entidades.ProductoSimulado productoSimulado = GetProductoSimulado(oportunidad.ProductoSimuladoId);

                //si no se ha enviado el parámetro bandera de monto solicitado en query string, encerar el valor del monto solicitado
                //de esta forma el stored procedure calculará el monto máximo
                if (Request.QueryString[Utilidades.NombresParametros.MontoSolicitado] == string.Empty ||
                    Request.QueryString[Utilidades.NombresParametros.MontoSolicitado] == null)
                {
                    oportunidad.MontoSolicitado = 0;
                }
                else
                {
                    oportunidad.MontoSolicitado = Decimal.Parse(Request.QueryString[Utilidades.NombresParametros.MontoSolicitado], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                }

                decimal monto = EjecutarEvaluacionCreditoSim(oportunidad, productoSimulado);

                string jscript = @"<script type=""text/javascript""> alert(""La oportunidad ha sido evaluada exitosamente.""); </script>";
                this.ClientScript.RegisterStartupScript(this.GetType(), "EVALUACION_SIMULACIONCREDITO_OPORTUNIDAD_EXITO", jscript);

                if(oportunidad.MontoSolicitado > 0)
                    SetBanderasRecalculo(oportunidad.Id);

                CerrarVentana(monto);
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], 
                    "No se pudo evaluar el monto de la oportunidad.", "EVALUAR_SIMULACIONCREDITO_OPORTUNIDAD", ex);
                CerrarVentana(0);
            }

            
        }

        private void SetBanderasRecalculo(Guid oportunidadId)
        {
            try
            {
                Oportunidad negOportunidad = new Oportunidad(Credenciales.ObtenerCredenciales());
                negOportunidad.SetRecalculoListoPendiente(oportunidadId, false, true);
                negOportunidad.SetEstadosRecalcularSgtesOportunidades(oportunidadId, true, false);
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this.Page, Request.QueryString[Utilidades.NombresParametros.Usuario],
                    "No se pudieron actualizar los estados para determinar el recálculo de monto.", "EVALUAR_SIMULACIONCREDITO_OPORTUNIDAD_BANDERAS", ex);
            }
        }

        private Entidades.Oportunidad GetOportunidad(Guid oportunidadId)
        {
            try
            {
                Oportunidad negOport = new Oportunidad(Credenciales.ObtenerCredenciales());
                Entidades.Oportunidad oportunidad = negOport.GetOportunidadDatosSimulacion(oportunidadId);
                return oportunidad;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private Entidades.ProductoSimulado GetProductoSimulado(Guid prodSimuladoId)
        {
            try
            {
                SimulacionCredito negSimCred = new SimulacionCredito(Credenciales.ObtenerCredenciales());
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_producto_catalogoid", "efk_amortizacion_cada", "efk_conseguro_cesantia", 
                                               "efk_conseguro_desgravamen", "efk_producto_simuladoid" };
                Entidades.ProductoSimulado prodSim = negSimCred.GetProductoSimulado(prodSimuladoId, cs);
                return prodSim;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private decimal EjecutarEvaluacionCreditoSim(Entidades.Oportunidad oportunidad, Entidades.ProductoSimulado productoSimulado)
        {
            try
            {
                SimulacionCredito negSimCred = new SimulacionCredito(Credenciales.ObtenerCredenciales());
                decimal monto = negSimCred.EvaluarCreditoOportunidad(oportunidad, productoSimulado, Credenciales.CadenaConexionBD(Credenciales.ObtenerCredendialesBD_BDI()));
                return monto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void CerrarVentana(decimal monto)
        {
            string jscript1 = "<script type=\"text/javascript\"> window.returnValue = " + monto.ToString(Producto.GetCulturaMoneda())  +"  </script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "RETORNAR_EVALUARSIMULAC_OPORTUNIDAD", jscript1);
            string jscript2 = "<script type=\"text/javascript\"> window.close(); </script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "CERRAR_VENTANA_EVALUARSIMULAC_OPORTUNIDAD", jscript2);
        }
    }
}