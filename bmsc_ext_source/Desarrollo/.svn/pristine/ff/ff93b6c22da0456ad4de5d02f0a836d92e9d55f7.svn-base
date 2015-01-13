using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.Entidades;
using Efika.Crm.Negocio;
using System.Reflection;


namespace Efika.Crm.Web.SimuladorCredito
{
    public partial class CondicionesCredito : System.Web.UI.Page
    {
        private const string PRODUCTO_SIMULADO_CREAR = "crear_producto_simulado";
        private const string OPORTUNIDAD_EDITAR = "editar_oportunidad";


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
                    hdnCodUsuario.Value = Request.QueryString[Utilidades.NombresParametros.Usuario];
                    Workspace1.ShowLeftPanel = false;
                    CargarParametrosProducto();
                    CargarDatosCliente();
                    CargarTasas(DatosCliente());
                    CargarTipoPoliza();
                    SetTipoAccion();
                    txtMontoMax.Attributes.Add("OnChange", "this.value = OnCurrencyValueKeyUp(this);");
                    txtMontoSol.Attributes.Add("OnKeyUp", "this.value = OnCurrencyValueKeyUp(this);");
                    if (ProductoEsTarjetaCredito(hdnProdTipoId.Value))
                        DeshabilitarIngresoCondiciones();
                }
                catch (Exception ex)
                {
                    Utilidades.ReportarError(this.Page, Request.QueryString[Utilidades.NombresParametros.Usuario],
                        "Ha ocurrido un error mientras se cargaban los datos iniciales.", "SIMULACCRED_CONDCRED_LOAD", ex);
                }
            }
        }



        #region Carga de campos
        private void CargarParametrosProducto()
        {
            hdnProdFamId.Value = Request.QueryString["FamId"];
            hdnProdId.Value = Request.QueryString["ProdId"];
            hdnProdTipoId.Value = Request.QueryString["TipoId"];
            hdnClientId.Value = Request.QueryString["ClienteId"];
            hdnSimCredId.Value = Request.QueryString["SimCredId"];
            hdnCantidadProdsPrevio.Value = Request.QueryString["numProdsSim"];
            hdnDivisaId.Value = Request.QueryString["DivisaId"];
            hdnDivisaCodISO.Value = Request.QueryString[Utilidades.NombresParametros.CodIsoMoneda];
            lblProductoNombre.Text = Request.QueryString["nombProd"];
            hdnNumOferta.Value = Request.QueryString["numOferta"];
            lblSimbDivMontoMax.Text = Request.QueryString["simbDivisa"];
            lblSimbDivMontoSol.Text = lblSimbDivMontoMax.Text;
        }


        private Entidades.Cliente DatosCliente()
        {
            try
            {
                Negocio.Cliente negCliente = new Negocio.Cliente(Credenciales.ObtenerCredenciales());
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_tipo_cliente", "efk_segmento_ovid" };
                Entidades.Cliente cliente = negCliente.DatosCliente(new Guid(hdnClientId.Value), cs);
                return cliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CargarTasas(Entidades.Cliente cliente)
        {
            try
            {
                string codMonedaBMSC;
                //if (ddlMonedaOp.SelectedValue.Substring(ddlMonedaOp.SelectedValue.Length - 1, 1) == "0")
                //    codMonedaBMSC = "2";//dolares
                //else
                //    codMonedaBMSC = "0";//bolivianos
                codMonedaBMSC = hdnDivisaCodISO.Value;
                codMonedaBMSC = cliente.Moneda.ToString();
                SimulacionCredito negSimCred = new SimulacionCredito(Credenciales.ObtenerCredenciales());
                DataSet dsTasas = negSimCred.ObtenerTasas(cliente.TipoClienteDesc, cliente.SegmentoDesc, codMonedaBMSC, hdnProdId.Value);

                ddlSPREAD.DataSource = dsTasas.Tables["spread"];

                ddlSPREAD.DataValueField = dsTasas.Tables["spread"].Columns[0].ColumnName;
                ddlSPREAD.DataTextField = ddlSPREAD.DataValueField;
                ddlSPREAD.DataBind();

                ddlTasaFija.DataSource = dsTasas.Tables["tasafija"];

                ddlTasaFija.DataValueField = dsTasas.Tables["tasafija"].Columns[0].ColumnName;
                ddlTasaFija.DataTextField = ddlTasaFija.DataValueField;
                ddlTasaFija.DataBind();

                ddlTRE.DataSource = dsTasas.Tables["tasatre"];

                ddlTRE.DataValueField = dsTasas.Tables["tasatre"].Columns[0].ColumnName;
                ddlTRE.DataTextField = ddlTRE.DataValueField;
                ddlTRE.DataBind();
            }
            catch (Exception ex)
            {
                ddlSPREAD.Items.Add(new ListItem("0,20", "0,20"));
                ddlSPREAD.Items.Add(new ListItem("0,30", "0,30"));
                ddlTasaFija.Items.Add(new ListItem("0,20", "0,20"));
                ddlTasaFija.Items.Add(new ListItem("0,30", "0,30"));
                ddlTRE.Items.Add(new ListItem("0,20", "0,20"));
                ddlTRE.Items.Add(new ListItem("0,30", "0,30"));
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario],
                    "Ha ocurrido un error al conectarse al servicio de tasas de BMSC, se han aplicado tasas predeterminadas.", "SIMULACION_CONDICIONESCRED_TASASBMSC", ex);
            }

            txtSPREAD.Text = ddlSPREAD.SelectedItem.Text.Trim();
            txtTasaFija.Text = ddlTasaFija.SelectedItem.Text.Trim();
            txtTRE.Text = ddlTRE.SelectedItem.Text.Trim();

            ddlFrecAmortizac.Items.Add(new ListItem(ProductoSimulado.FrecAmortizacionText(), ProductoSimulado.FrecAmortizacionValue()));
        }

        private void CargarDatosCliente()
        {
            try
            {
                Guid idCliente = new Guid(hdnClientId.Value);

                AccesoServicios.CRMSDK.ColumnSet cs = new AccesoServicios.CRMSDK.ColumnSet();
                cs.Attributes = new string[] { "name", "efk_nombre_persona", "efk_primerapellido", "efk_segundoapellido" };

                Negocio.Cliente negCliente = new Negocio.Cliente(Credenciales.ObtenerCredenciales());
                Entidades.Cliente cliente = negCliente.DatosCliente(idCliente, cs);
                lblClienteNombre.Text = cliente.Nombre + " " + cliente.Apellido1 + " " + cliente.Apellido2;
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Ha ocurrido un error al cargar los datos del cliente.", "SIMULACCRED_OBTENERCLIENTE", ex);
            }
        }

        private void CargarTipoPoliza()
        {
            try
            {
                Negocio.Producto negProd = new Negocio.Producto(Credenciales.ObtenerCredenciales(), true);
                List<string[]> tiposPoliza = negProd.GetTipoPoliza();
                foreach (string[] tipoPoliza in tiposPoliza)
                {
                    ddlTipoPoliza.Items.Add(new ListItem(tipoPoliza[0], tipoPoliza[1]));
                }
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al cargar tipo de póliza.", "SIMULACION_CONDICIONESCRED_TIPOPOLIZA", ex);
            }
        }
        #endregion

        #region Validaciones
        public bool ExisteProductoSimulado()
        {
            if (string.IsNullOrWhiteSpace(hdnProdSimId.Value))
                return false;
            else
                return true;
        }
        public bool MontoMaximoEstablecido()
        {
            try
            {
                decimal montoMax = decimal.Parse(txtMontoMax.Text.Replace(".", "").Trim());
                if (montoMax > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool MontoSolicitadoEstablecido()
        {
            try
            {
                decimal montoMax = decimal.Parse(txtMontoSol.Text.Replace(".", "").Trim());
                if (montoMax > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public decimal GetMontoSolicitadoEstablecido()
        {
            try
            {
                decimal montoSol = decimal.Parse(txtMontoSol.Text.Replace(".", "").Trim(), Negocio.Producto.GetCulturaMoneda());
                return montoSol;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public decimal GetMontoMaximoEstablecido()
        {
            try
            {
                decimal montoMax = decimal.Parse(txtMontoMax.Text.Replace(".", "").Trim(), Negocio.Producto.GetCulturaMoneda());
                return montoMax;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ProductoEsTarjetaCredito(string idTipoProducto)
        {
            try
            {
                Negocio.Producto.Tipos negTiposProd = new Negocio.Producto.Tipos(Credenciales.ObtenerCredenciales());
                if (idTipoProducto == negTiposProd.GetIdTipoProducto(ConfigurationManager.AppSettings["NombreTipoProductoTC"]).ToString())
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al validar tipo de producto.", "SIMULACION_CONDICIONESCRED_VALIDATC", ex);
                return false;
            }
        }
        #endregion


        public ProductoSimulado CrearObjetoProductoSimulado(bool conMontos)
        {
            ProductoSimulado prodSimulado;
            //seguros
            bool segDesg, segCesant;
            if (rblSegDesgravamen.SelectedValue == "SI")
                segDesg = true;
            else
                segDesg = false;
            if (rblSegCesantia.SelectedValue == "SI")
                segCesant = true;
            else
                segCesant = false;
            //producto simulado
            Guid idProdSim;
            if (ExisteProductoSimulado())
                idProdSim = new Guid(hdnProdSimId.Value);
            else
                idProdSim = Guid.Empty;

            try
            {
                int ordenProdActual = int.Parse(hdnCantidadProdsPrevio.Value) + 1;
                string nombreProdSim = lblProductoNombre.Text + " - Oferta No." + hdnNumOferta.Value;
                if (conMontos)
                {
                    //monto máximo
                    decimal montoMax, montoSol;
                    if (MontoMaximoEstablecido())
                        montoMax = GetMontoMaximoEstablecido();
                    else
                        montoMax = 0;
                    if (MontoSolicitadoEstablecido())
                        montoSol = GetMontoSolicitadoEstablecido();
                    else
                        montoSol = 0;



                    if (montoSol <= montoMax)//el monto solicitado no puede ser mayor al monto máximo
                    {
                        prodSimulado = new ProductoSimulado(idProdSim, nombreProdSim, int.Parse(txtNumCuotas.Text.Trim()), hdnDivisaCodISO.Value,
                                            decimal.Parse(txtTasaFija.Text.Trim(), Negocio.Producto.GetCulturaMoneda()), decimal.Parse(txtTRE.Text.Trim(),
                                            Negocio.Producto.GetCulturaMoneda()), decimal.Parse(txtSPREAD.Text.Trim(), Negocio.Producto.GetCulturaMoneda()),
                                            int.Parse(ddlFrecAmortizac.SelectedItem.Text.Trim()), int.Parse(ddlFrecAmortizac.SelectedValue.Trim()),
                                            int.Parse(txtInicTasaVar.Text.Trim()), segDesg, segCesant, int.Parse(ddlTipoPoliza.Text.Trim()), montoMax, montoSol,
                                            new Guid(hdnProdId.Value), new Guid(hdnProdTipoId.Value), new Guid(hdnProdFamId.Value), new Guid(hdnClientId.Value),
                                            new Guid(hdnSimCredId.Value), lblProductoNombre.Text, int.Parse(hdnNumOferta.Value), ordenProdActual);
                        prodSimulado.OwnerId = new Guid(hdnCodUsuario.Value);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("montosolicitado");
                    }
                }
                else
                {
                    prodSimulado = new ProductoSimulado(idProdSim, nombreProdSim, int.Parse(txtNumCuotas.Text.Trim()), hdnDivisaCodISO.Value,
                                        decimal.Parse(txtTasaFija.Text.Trim(), Negocio.Producto.GetCulturaMoneda()), decimal.Parse(txtTRE.Text.Trim(),
                                        Negocio.Producto.GetCulturaMoneda()), decimal.Parse(txtSPREAD.Text.Trim(), Negocio.Producto.GetCulturaMoneda()),
                                        int.Parse(ddlFrecAmortizac.SelectedItem.Text.Trim()), int.Parse(ddlFrecAmortizac.SelectedValue.Trim()),
                                        int.Parse(txtInicTasaVar.Text.Trim()), segDesg, segCesant, int.Parse(ddlTipoPoliza.SelectedItem.Value.Trim()),
                                        new Guid(hdnProdId.Value), new Guid(hdnProdTipoId.Value), new Guid(hdnProdFamId.Value), new Guid(hdnClientId.Value),
                                        new Guid(hdnSimCredId.Value), lblProductoNombre.Text, int.Parse(hdnNumOferta.Value), ordenProdActual);
                    prodSimulado.OwnerId = new Guid(hdnCodUsuario.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return prodSimulado;
        }

        #region Transacciones
        public void GuardarProductoSimulado()
        {
            if (TipoAccion() == PRODUCTO_SIMULADO_CREAR)
            {
                if (ActualizarProductoSimulado(new Guid(hdnDivisaId.Value)))
                {
                    ActualizarGridSimuladorCredito();
                }
            }
            else if (TipoAccion() == OPORTUNIDAD_EDITAR)
            {
                ActualizarOportunidad();
            }

            OcultarCargando();
        }

        public void CrearProductoSimulado()
        {
            try
            {
                ProductoSimulado prodSimulado;
                SimulacionCredito simulacCred = new SimulacionCredito(Credenciales.ObtenerCredenciales());
                //crear registro en crm                                 
                prodSimulado = CrearObjetoProductoSimulado(false);

                if (prodSimulado.Id == Guid.Empty)
                {
                    prodSimulado.Id = simulacCred.AgregarProductoSimulado(prodSimulado, new Guid(hdnDivisaId.Value));
                    simulacCred.AsignarPropietarioProductoSimulado(prodSimulado.Id, prodSimulado.OwnerId);
                }
                else
                    simulacCred.ActualizarProductoSimulado(prodSimulado, new Guid(hdnDivisaId.Value));

                hdnProdSimId.Value = prodSimulado.Id.ToString();
                //obtener el monto máximo
                decimal montoMaximo = CalcularMaximo(simulacCred, prodSimulado);
                string[] montMax = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:C}", montoMaximo).Split('.');
                txtMontoMax.Text = montMax[0].Replace(',', '.').Substring(1, montMax[0].Length - 1) + "," + montMax[1];
                txtMontoSol.ReadOnly = false;
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al crear producto simulado.", "SIMULACION_CONDICIONESCRED_CREAR_PRODSIM", ex);
            }
        }


        public decimal CalcularMaximo(SimulacionCredito simCred, ProductoSimulado prodSim)
        {
            decimal montoMaximo = 0;
            try
            {
                int ordenProdActual = int.Parse(hdnCantidadProdsPrevio.Value) + 1;
                montoMaximo = simCred.ObtenerMontoMaximo(prodSim, ordenProdActual, hdnDivisaCodISO.Value, Credenciales.CadenaConexionBD(Credenciales.ObtenerCredendialesBD_BDI()));
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al calcular el monto máximo.", "SIMULACION_CONDICIONESCRED_CALCULAMONTOMAXIMO", ex);
            }
            return montoMaximo;
        }


        public bool ActualizarProductoSimulado(Guid divisaId)
        {
            try
            {
                if (MontoMaximoEstablecido() && MontoSolicitadoEstablecido())
                {
                    ProductoSimulado objProdSim = CrearObjetoProductoSimulado(true);
                    SimulacionCredito simulacCred = new SimulacionCredito(Credenciales.ObtenerCredenciales());
                    simulacCred.ActualizarProductoSimulado(objProdSim, divisaId);
                    CalcularMaximo(simulacCred, objProdSim);
                    return true;
                }
                else//primeramente se debe calcular el monto máximo
                {
                    string mensaje = @"<script type=""text/javascript""> alert(""Debe primeramente calcular el monto máximo e ingresar el monto solicitado."");</script>";
                    this.ClientScript.RegisterStartupScript(this.GetType(), "CALCULAR_MAXIMO", mensaje);
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                string mensaje = "<script type=\"text/javascript\"> alert(\"El monto solicitado no puede ser mayor al monto máximo\");</script>";
                this.ClientScript.RegisterStartupScript(this.GetType(), "MONTO_SOLICITADO", mensaje);
                Utilidades.AgregarErrorAlLog("El monto solicitado no puede ser mayor al monto máximo", this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        Request.QueryString[Utilidades.NombresParametros.Usuario], ex.StackTrace);
            }
            catch (Exception ex)
            {
                string mensaje = "<script type=\"text/javascript\"> alert(\"Ha ocurrido un error: " + ex.Message.Replace("\r\n", ". ") + "\");</script>";
                this.ClientScript.RegisterStartupScript(this.GetType(), "ACTUALIZAR_PRODUCTO_SIMULADO", mensaje);
                Utilidades.AgregarErrorAlLog(ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        Request.QueryString[Utilidades.NombresParametros.Usuario], ex.StackTrace);
            }
            return false;
        }
        #endregion

        #region Formulario
        private string TipoAccion()
        {
            return hdnTipoAccion.Value;
        }

        private void SetTipoAccion()
        {
            if (string.IsNullOrWhiteSpace(Request.QueryString["IdOportunidad"]))
            {
                //el modo es el normal (creación de producto simulado)
                hdnTipoAccion.Value = PRODUCTO_SIMULADO_CREAR;
            }
            else
            {
                //se trabajará sobre oportunidad (visualización y edición)
                hdnTipoAccion.Value = OPORTUNIDAD_EDITAR;
                hdnOportunidadId.Value = Request.QueryString["IdOportunidad"];
                btnCalcMax.Visible = false;
                CargarCamposOportunidad();
            }
        }

        private void DeshabilitarIngresoCondiciones()
        {
            txtInicTasaVar.Text = "0";
            txtInicTasaVar.Enabled = false;
            txtNumCuotas.Text = "0";
            txtNumCuotas.Enabled = false;
            txtSPREAD.Enabled = false;
            txtTasaFija.Enabled = false;
            txtTRE.Enabled = false;
            ddlFrecAmortizac.Enabled = false;
            ddlSPREAD.Enabled = false;
            ddlTasaFija.Enabled = false;
            ddlTipoPoliza.Enabled = false;
            ddlTRE.Enabled = false;
            rblSegCesantia.Items[1].Selected = true;
            rblSegCesantia.Enabled = false;
            rblSegDesgravamen.Items[0].Selected = true;
            rblSegDesgravamen.Enabled = false;
        }

        private void CerrarVentana()
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "CERRARprodSimulado", "<script type=\"text/javascript\"> window.close(); </script>");
        }

        public void ReiniciarMontos()
        {
            txtMontoMax.Text = "";
            txtMontoSol.Text = "";
            txtMontoSol.ReadOnly = true;
        }

        private void ActualizarGridSimuladorCredito()
        {
            string scriptFlagReload = "window.opener.document.getElementById(\"hdnCargarProdsSims\").value = \"1\"; ";
            string scriptString = "<script language=\"JavaScript\"> " + scriptFlagReload;
            scriptString += " if(navigator.appName==\"Microsoft Internet Explorer\") window.opener.document.forms(0).submit(); ";
            scriptString += " else window.opener.document.forms[0].submit(); </script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "ACTUALIZAR_SIMULADOR_CREDITO", scriptString);
        }

        private void OcultarCargando()
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "FIN_CALCULO_MAXIMO", "document.getElementById('loading').style.display = \"none\";");
            btnCalcMax.Enabled = true;
        }
        #endregion

        #region Modo Oportunidad
        private void ActualizarOportunidad()
        {
            int ordenProdActual = int.Parse(hdnCantidadProdsPrevio.Value) + 1;

            Entidades.Oportunidad oportunidad = new Entidades.Oportunidad(new Guid(hdnOportunidadId.Value), Guid.Empty, Guid.Empty, string.Empty,
                int.Parse(txtNumCuotas.Text.Trim()), int.Parse(txtInicTasaVar.Text.Trim()), GetMontoMaximoEstablecido(), GetMontoSolicitadoEstablecido(),
                decimal.Parse(txtSPREAD.Text.Trim(), Negocio.Producto.GetCulturaMoneda()), decimal.Parse(txtTasaFija.Text.Trim(), Negocio.Producto.GetCulturaMoneda()),
                decimal.Parse(txtTRE.Text.Trim(), Negocio.Producto.GetCulturaMoneda()), ordenProdActual, new Guid(hdnDivisaId.Value), hdnDivisaCodISO.Value);
            Negocio.Oportunidad negOport = new Negocio.Oportunidad(Credenciales.ObtenerCredenciales());
            negOport.EditarOportunidad(oportunidad);
        }


        private void CargarCamposOportunidad()
        {
            try
            {
                Negocio.Oportunidad negOport = new Negocio.Oportunidad(Credenciales.ObtenerCredenciales());

                Entidades.Oportunidad oportunidad = negOport.GetOportunidadDatosSimulacion(new Guid(hdnOportunidadId.Value));

                txtNumCuotas.Text = oportunidad.NumCuotas.ToString();
                txtInicTasaVar.Text = oportunidad.InicioTasaVariable.ToString();
                txtMontoMax.Text = String.Format(Negocio.Producto.GetCulturaMoneda(), "{0:0.0}", oportunidad.MontoMaximo);
                txtMontoSol.Text = String.Format(Negocio.Producto.GetCulturaMoneda(), "{0:0.0}", oportunidad.MontoSolicitado);
                txtSPREAD.Text = String.Format(Negocio.Producto.GetCulturaMoneda(), "{0:0.0}", oportunidad.Spread);
                txtTasaFija.Text = String.Format(Negocio.Producto.GetCulturaMoneda(), "{0:0.0}", oportunidad.TasaFija);
                txtTRE.Text = String.Format(Negocio.Producto.GetCulturaMoneda(), "{0:0.0}", oportunidad.TRE);
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al cargar datos de oportunidad", "CONDICIONESCRED_CARGAOPORTUNIDAD", ex);
            }
        }
        #endregion

        #region Eventos
        protected void GuardarClick(object sender, EventArgs e)
        {
            GuardarProductoSimulado();
        }

        protected void GuardarCerrarClick(object sender, EventArgs e)
        {
            GuardarProductoSimulado();
            CerrarVentana();
        }

        protected void CerrarClick(object sender, EventArgs e)
        {
            CerrarVentana();
        }

        protected void CalcularMaximoClick(object sender, EventArgs e)
        {
            CrearProductoSimulado();
            ActualizarGridSimuladorCredito();
            OcultarCargando();
        }

        protected void FormItemChanged(object sender, EventArgs e)
        {
            ReiniciarMontos();
        }

        protected void MonedaItemChanged(object sender, EventArgs e)
        {
            ReiniciarMontos();
            CargarTasas(DatosCliente());
            OcultarCargando();
        }
        #endregion
    }
}