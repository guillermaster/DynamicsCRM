using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;
using System.Configuration;
using Efika.Crm.AccesoServicios;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.Entidades;
using Efika.Crm.Negocio;
using System.Globalization;


namespace Efika.Crm.Web.SimuladorCredito
{
    public partial class SimuladorCredito : System.Web.UI.Page
    {
        private static DataTable dtOfertas;
        private static DataTable dtProductosCampania;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Workspace1.ShowLeftPanel = false;

                if (!Utilidades.ValidarAccesosWeb(Request.QueryString[Utilidades.NombresParametros.Usuario], Privilegios.NombresPrivilegios.SimulacionCredito))
                {
                    Response.Write(@"<script type=""text/javascript""> alert('No tiene privilegios para acceder a este formulario.'); window.close(); </script>");
                    return;
                }

                //si es una nueva simulación
                if (Request.QueryString[Utilidades.NombresParametros.SimulacionId] == null)
                {
                    bool cargarOfertaValor = true;
                    if (Request.QueryString[Utilidades.NombresParametros.CampaniaId] != null)
                    {
                        hdnCodCampania.Value = Request.QueryString[Utilidades.NombresParametros.CampaniaId];
                        CargarCampania(new Guid(hdnCodCampania.Value));
                        CargarProductosCampania(new Guid(hdnCodCampania.Value));
                        cargarOfertaValor = false;
                    }
                    InicializarNuevaSimulacion(cargarOfertaValor);
                }
                else//si se desea cargar una simulación ya existente
                {
                    InicializarSimulacionExistente();
                }
            }
            else
            {
                CargarProductosSimulados();
            }
        }


        #region Pre carga de datos

        protected void InicializarNuevaSimulacion(bool cargarOfertaValor)
        {
            hdnCodUsuario.Value = Request.QueryString[Utilidades.NombresParametros.Usuario];
            ResetCamposActivo();
            ResetCamposPasivo();
            ResetCamposServicio();
            CargarDatosCliente(cargarOfertaValor);
            CargarCodDivisa(new Guid(Request.QueryString[Utilidades.NombresParametros.DivisaId]), true);
            SetNumProdsSimulados();
            AgregarSimulacionCrediticia();
            CargarListasProductos();
        }

        protected void InicializarSimulacionExistente()
        {
            try
            {
                hdnCodUsuario.Value = Request.QueryString[Utilidades.NombresParametros.Usuario];
                hdnSimCredito.Value = Request.QueryString[Utilidades.NombresParametros.SimulacionId];
                hdnCargarProdsSims.Value = "1";
                CargarListasProductos();
                ResetCamposActivo();
                ResetCamposPasivo();
                ResetCamposServicio();
                CargarDatosCliente(true);
                CargarProductosSimulados();
                btnPasivoLookup.Enabled = true;
                btnServicioLookup.Enabled = true;

                Guid simCredId = new Guid(hdnSimCredito.Value);
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_name", "efk_numero_oferta", "efk_camapaniaid", "efk_simulacion_guardada", "transactioncurrencyid" };

                SimulacionCredito negSimCred = new SimulacionCredito(Credenciales.ObtenerCredenciales());

                SimulacionCrediticia simCred = negSimCred.GetSimulacionCrediticia(simCredId, cs);

                CargarCodDivisa(simCred.transactioncurrencyid, false);

                lblNoOferta.Text = simCred.NumeroOferta.ToString();
                if (simCred.CampaniaId != Guid.Empty)
                    hdnCodCampania.Value = simCred.CampaniaId.ToString();
                else
                    hdnCodCampania.Value = "";

                if (simCred.Guardada)
                    hdnSimGuardada.Value = "1";
                else
                    hdnSimGuardada.Value = "0";

                if (negSimCred.SimulacionGeneroOportunidades(simCredId))
                {
                    DeshabilitarEdicionFormulario();
                    DeshabilitarEliminarTodosProductosSimulados();
                }
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, hdnCodUsuario.Value, "Error mientras se cargaban datos de simulación existente", "Error carga simulación", ex);
            }
        }

        protected void CargarCodDivisa(Guid idDivisa, bool nuevaSimulacion)
        {
            hdnCodDivisa.Value = idDivisa.ToString();
            if (idDivisa != Guid.Empty)
            {
                Negocio.Divisa negDivisa = new Negocio.Divisa(Credenciales.ObtenerCredenciales());
                Entidades.Divisa entDivisa = negDivisa.ConsultaDivisa(idDivisa);
                HdCodIsoDivisa.Value = entDivisa.isoCurrencyCode;
                HdSimboloDivisa.Value = entDivisa.currencySymbol;
                txtMoneda.Text = entDivisa.currencyName;
                if (nuevaSimulacion)
                    btnMonedaLookup.Enabled = true;
                else
                    btnMonedaLookup.Enabled = false;
            }
            else
            {
                throw new Exception("El ID de divisa no puede ser vacío, para continuar seleccione la moneda correcta.");
            }
        }

        protected void CargarDatosCliente(bool cargarOfertaValor)
        {
            try
            {
                Guid idCliente = new Guid(Request.QueryString[Utilidades.NombresParametros.ClienteId]);

                hdnClienteId.Value = idCliente.ToString();

                AccesoServicios.CRMSDK.ColumnSet cs = new AccesoServicios.CRMSDK.ColumnSet();
                cs.Attributes = new string[] {"name", "efk_nombre_persona", "efk_primerapellido", "efk_segundoapellido",
                                              "efk_tipo_cliente", "efk_fuente_ingresos_ov","transactioncurrencyid" };

                Negocio.Cliente negCliente = new Negocio.Cliente(Credenciales.ObtenerCredenciales());
                Entidades.Cliente cliente = negCliente.DatosCliente(idCliente, cs);
                lblClienteNombre.Text = cliente.Nombre + " " + cliente.Apellido1 + " " + cliente.Apellido2;
                lblFuenteIngreso.Text = cliente.FuenteIngresoOfValDesc;
                lblTipoCliente.Text = cliente.TipoClienteDesc;
                CargarProductosSimulados();
                if (cargarOfertaValor)
                {
                    pnlOfertaValor.Visible = true;
                }
                else
                {
                    pnlOfertaValor.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Ha ocurrido un error al cargar los datos del cliente. " + ex.Message, "SIMULACCRED_OBTENERCLIENTE", ex);
            }
        }

        

        private void ActualizarDivisaSimulacion(string idDivisa)
        {
            try
            {
                Negocio.SimulacionCredito negSimCred = new SimulacionCredito(Credenciales.ObtenerCredenciales());
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, hdnCodUsuario.Value, "Error al actualizar divisa de simulación, por favor intente nuevamente. " + ex.Message, "Error actualización simulación", ex);
            }
        }

        private void CargarCampania(Guid campaniaId)
        {
            ColumnSet cs = new ColumnSet();
            cs.Attributes = new string[] { "name" };
            Negocio.Campania negCampania = new Negocio.Campania(Credenciales.ObtenerCredenciales());

            if (negCampania.CargarCampania(new Guid(hdnCodCampania.Value), cs))
            {
                txtCampania.Text = negCampania.CampaniaActual.Nombre;
                lblCampania.Visible = true;
                txtCampania.Visible = true;
            }
            else
            {
                Utilidades.ReportarError(this.Page, Request.QueryString[Utilidades.NombresParametros.Usuario],
                    "Error al cargar el título de la campaña.", "SIMCRED_TITCAMPANIA", new Exception(negCampania.CampaniaActual.Errores));
            }
        }


        private void CargarProductosCampania(Guid campaniaId)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("nombreProducto");
                dt.Columns.Add("familiaProducto");
                dt.Columns.Add("tipoProducto");
                dt.Columns.Add("puedeComercializar");
                dt.Columns.Add("cupoMonto");

                Negocio.Campania negCampania = new Negocio.Campania(Credenciales.ObtenerCredenciales());
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "name", "efk_familia_productosid", "efk_tipo_productoid", "efk_habilitado_comercializar" };
                List<Entidades.Producto> productosCampania = negCampania.ProductosPorCampania(campaniaId, cs);

                foreach (Entidades.Producto prodCamp in productosCampania)
                {
                    Negocio.DatosImportadosCampania negDatImpCamp = new Negocio.DatosImportadosCampania(Credenciales.ObtenerCredenciales());
                    List<Entidades.DatosImportadosCampania> datosImpCamp = negDatImpCamp.DatosImportadosPorCampanaYProducto(campaniaId, prodCamp.Id);
                    if (datosImpCamp.Count > 0)
                    {
                        foreach (Entidades.DatosImportadosCampania datoImp in datosImpCamp)
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = prodCamp.Nombre;
                            dr[1] = prodCamp.FamiliaProductosNombre;
                            dr[2] = prodCamp.TipoProductoNombre;
                            dr[3] = prodCamp.HabilitadoComercializar ? "Sí" : "No";
                            dr[4] = datoImp.CupoMonto;
                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = prodCamp.Nombre;
                        dr[1] = prodCamp.FamiliaProductosNombre;
                        dr[2] = prodCamp.TipoProductoNombre;
                        dr[3] = prodCamp.HabilitadoComercializar ? "Sí" : "No";
                        dr[4] = "";
                        dt.Rows.Add(dr);
                    }
                }
                gvProductosCampania.DataSource = dt;
                gvProductosCampania.DataBind();
                pnlProductosCampania.Visible = true;
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this.Page, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al cargar productos de la campaña.", "SIMCRED_PRODSCAMPANIA", ex);
            }
        }

        #endregion


        #region Carga de productos simulados

        private DataTable ProductosSimuladosToDataTable(List<ProductoSimulado> prodsSims)
        {
            DataTable dtProdsSims = new DataTable();
            dtProdsSims.Columns.Add("IdProductoSimulado");//0
            dtProdsSims.Columns.Add("CodTipoFamiliaProd");//1
            dtProdsSims.Columns.Add("ProductoNombre");//2
            dtProdsSims.Columns.Add("MontoMaximo");//3
            dtProdsSims.Columns.Add("MontoPactado");//4
            dtProdsSims.Columns.Add("CuotaMaxima");//5
            dtProdsSims.Columns.Add("CuotaPactada");//6
            dtProdsSims.Columns.Add("ProductoId");//7
            dtProdsSims.Columns.Add("ProductoTipoId");//8
            dtProdsSims.Columns.Add("ProductoFamId");//9
            dtProdsSims.Columns.Add("Plazo");//10
            dtProdsSims.Columns.Add("TasaFija");//11
            dtProdsSims.Columns.Add("TRE");//12
            dtProdsSims.Columns.Add("SpreadFijo");//13
            dtProdsSims.Columns.Add("TasaVarPar");//14
            dtProdsSims.Columns.Add("Orden");//15
            dtProdsSims.Columns.Add("AmortizCada");//16
            dtProdsSims.Columns.Add("SegCesantia");//17
            dtProdsSims.Columns.Add("SegDesgrav");//18
            dtProdsSims.Columns.Add("TipoPolizaCod");
            dtProdsSims.Columns.Add("Nombre");
            dtProdsSims.Columns.Add("MonedaOp");

            foreach (ProductoSimulado prodSim in prodsSims)
            {
                if (prodSim.ProductoFamiliaCod == Entidades.Producto.FamiliaTipos.Activo.Codigo && !ExisteSobranteDisponible(prodSim.CuotaSobrante, prodSim.NuevoCemAjustadoVivienda))
                {
                    DeshabilitarBusquedaActivo();
                }

                DataRow dr = dtProdsSims.NewRow();
                dr[0] = prodSim.Id;
                dr[1] = prodSim.ProductoFamiliaCod;
                dr[2] = prodSim.ProductoNombre;
                dr[3] = Utilidades.DecimalToFormatStringMoney(prodSim.MontoMaximo, HdSimboloDivisa.Value);
                dr[4] = Utilidades.DecimalToFormatStringMoney(prodSim.MontoSolicitado, HdSimboloDivisa.Value);
                dr[5] = Utilidades.DecimalToFormatStringMoney(prodSim.CuotaMaxima, HdSimboloDivisa.Value);
                dr[6] = Utilidades.DecimalToFormatStringMoney(prodSim.CuotaPactada, HdSimboloDivisa.Value);
                dr[7] = prodSim.ProductoId;

                ColumnSet csProd = new ColumnSet();
                csProd.Attributes = new string[] { "efk_tipo_productoid", "efk_familia_productosid" };
                Negocio.Producto negProd = new Negocio.Producto(Credenciales.ObtenerCredenciales());
                Entidades.Producto producto = negProd.GetProducto(prodSim.ProductoId, csProd);

                dr[8] = producto.TipoProductoId;
                dr[9] = producto.FamiliaProductosId;
                dr[10] = prodSim.NumeroCuotas;
                dr[11] = Utilidades.DecimalToFormatString(prodSim.TasaFija);
                dr[12] = Utilidades.DecimalToFormatString(prodSim.TreSemana);
                dr[13] = Utilidades.DecimalToFormatString(prodSim.SpreadFijo);
                dr[14] = prodSim.TasaVariableDesde;
                dr[15] = prodSim.Orden;
                dr[16] = prodSim.FrecuenciaAmortizacion;
                dr[17] = prodSim.SeguroCesantia ? "1" : "0";
                dr[18] = prodSim.SeguroDesgravamen ? "1" : "0";
                dr[19] = prodSim.TipoPolizaCod;
                dr[20] = prodSim.Nombre;
                dr[21] = prodSim.MonedaCodISO;

                dtProdsSims.Rows.Add(dr);
            }

            return dtProdsSims;
        }

        public void CargarProductosSimulados()
        {
            if (hdnCargarProdsSims.Value == "1")//solo recargar prods.sims. si se ha indicado (hdnCargarProdsSims=1)
            {
                if (!String.IsNullOrWhiteSpace(hdnSimCredito.Value))
                {
                    try
                    {
                        SimulacionCredito simulacCred = new SimulacionCredito(Credenciales.ObtenerCredenciales());
                        List<ProductoSimulado> productosSimulados = simulacCred.ProductosSimuladosDatosBasicos(new Guid(hdnSimCredito.Value));
                        DataTable dtProdsSims = ProductosSimuladosToDataTable(productosSimulados);
                        if (dtProdsSims.Rows.Count == 0)
                            HabilitarSeleccionMoneda();
                        else
                            DeshabilitarSeleccionMoneda();
                        gvProdServSim.DataSource = dtProdsSims;
                        gvProdServSim.DataBind();
                        SetNumProdsSimulados();
                        SetEliminarUltimoProductoActivo();
                        HabilitarDeshabBotonGeneraOportunidades();
                    }
                    catch (Exception ex)
                    {
                        Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error leyendo lista de productos simulados", "SIMULACCRED_LEERPRODSIMS", ex);
                    }
                }
                hdnCargarProdsSims.Value = "";
            }
        }

        private void HabilitarDeshabBotonGeneraOportunidades()
        {
            if (gvProdServSim.Rows.Count > 0)
            {
                btnGenOport.Enabled = true;
            }
            else
            {
                btnGenOport.Enabled = false;
            }
        }

        private void SetEliminarUltimoProductoActivo()
        {
            bool secuencia = true;
            int idx = gvProdServSim.Rows.Count - 1;

            while (secuencia)
            {
                if (gvProdServSim.Rows[idx].Cells[3].Text == Entidades.Producto.FamiliaTipos.Activo.Codigo.ToString())
                {
                    foreach (Control ctrl in gvProdServSim.Rows[idx].Cells[1].Controls)
                    {
                        ctrl.Visible = true;
                    }
                    secuencia = false;
                }
                idx -= 1;

                if (idx < 0)
                    secuencia = false;

            }

        }

        private void SetNumProdsSimulados()
        {
            int nProdAct = 0;
            foreach (GridViewRow gvProdSimRow in gvProdServSim.Rows)
            {
                string tipoFmProdCod = gvProdSimRow.Cells[3].Text; //tipo familia producto
                if (int.Parse(tipoFmProdCod) == Entidades.Producto.FamiliaTipos.Activo.Codigo)
                    nProdAct++;
            }

            hdnNumProdsSim.Value = nProdAct.ToString();
        }

        #endregion


        #region Carga de Valores de Familias de Productos

        protected string CargarFamiliasProductos(int codTipoFamProd, char separator)
        {
            string codigosFamsProds = string.Empty;
            try
            {
                Negocio.Producto negProd = new Negocio.Producto(Credenciales.ObtenerCredenciales());
                List<Entidades.FamiliaProducto> familias = negProd.FamiliasProductos(codTipoFamProd.ToString());
                foreach (Entidades.FamiliaProducto familia in familias)
                {
                    if (string.IsNullOrWhiteSpace(codigosFamsProds))
                    {
                        codigosFamsProds = familia.Id.ToString();
                    }
                    else
                    {
                        codigosFamsProds += separator + familia.Id.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilidades.AgregarErrorAlLog(ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        Request.QueryString[Utilidades.NombresParametros.Usuario], ex.StackTrace);
            }
            return codigosFamsProds;
        }
        #endregion


        #region Carga de Valores de Tipos de Productos

        protected string CargarTiposProductos(string[] idsFamilias, char separator)
        {
            string idsTiposProds = string.Empty;
            foreach (string idFam in idsFamilias)
            {
                if (string.IsNullOrWhiteSpace(idsTiposProds))
                {
                    idsTiposProds = CargarTiposProductos(idFam, separator);
                }
                else
                {
                    idsTiposProds += separator + CargarTiposProductos(idFam, separator);
                }
            }
            return idsTiposProds;
        }


        protected string CargarTiposProductos(string idFamilia, char separator)
        {
            string idsTiposProds = string.Empty;
            try
            {
                Negocio.Producto negProd = new Negocio.Producto(Credenciales.ObtenerCredenciales());
                List<Entidades.TipoProducto> tiposProd = negProd.TiposProductos(idFamilia);
                foreach (Entidades.TipoProducto tipoProd in tiposProd)
                {
                    if (string.IsNullOrWhiteSpace(idsTiposProds))
                    {
                        idsTiposProds = tipoProd.Id.ToString();
                    }
                    else
                    {
                        idsTiposProds += separator + tipoProd.Id.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilidades.AgregarErrorAlLog(ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        Request.QueryString[Utilidades.NombresParametros.Usuario], ex.StackTrace);
            }
            return idsTiposProds;
        }

        #endregion


        #region Core
        public ProductoSimulado CrearObjetoProductoSimulado(string productoNombre, string productoId, string famProdId, string codIsoMoneda)
        {
            try
            {
                ProductoSimulado prodSim = new ProductoSimulado();
                prodSim.Nombre = productoNombre + " - Oferta No. " + lblNoOferta.Text;
                prodSim.ProductoNombre = productoNombre;
                prodSim.ProductoId = new Guid(productoId);
                prodSim.ClienteId = new Guid(hdnClienteId.Value);
                prodSim.SimulacionCreditoId = new Guid(hdnSimCredito.Value);
                prodSim.ProductoFamiliaId = new Guid(famProdId);
                prodSim.FrecuenciaAmortizacion = int.Parse(ProductoSimulado.FrecAmortizacionText());
                prodSim.FrecuenciaAmortizacionCod = int.Parse(ProductoSimulado.FrecAmortizacionValue());
                prodSim.NumeroOferta = int.Parse(lblNoOferta.Text);

                prodSim.OwnerId = new Guid(hdnCodUsuario.Value);

                return prodSim;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CrearProductoSimuladoPasivoServicio(string productoNombre, string productoId, string famProdId)
        {
            try
            {
                ProductoSimulado prodSimulado;
                SimulacionCredito simulacCred = new SimulacionCredito(Credenciales.ObtenerCredenciales());
                //crear registro en crm
                prodSimulado = CrearObjetoProductoSimulado(productoNombre, productoId, famProdId, HdCodIsoDivisa.Value);
                prodSimulado.Id = simulacCred.AgregarProductoSimulado(prodSimulado, new Guid(hdnCodDivisa.Value));//envia prod.sim. y id divisa (moneda)
                simulacCred.AsignarPropietarioProductoSimulado(prodSimulado.Id, prodSimulado.OwnerId);
                String prodSimId = prodSimulado.Id.ToString();
                hdnCargarProdsSims.Value = "1";
                CargarProductosSimulados();
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al crear producto simulado.", "SIMULACCRED_CREAPRODSIM", ex);
            }
        }

        public void EliminarProductoSimulado(Guid prodSimId)
        {
            try
            {
                ProductoSimulado prodSim = new ProductoSimulado();
                prodSim.Id = prodSimId;
                SimulacionCredito simCred = new SimulacionCredito(Credenciales.ObtenerCredenciales());
                simCred.EliminarProductoSimulado(prodSim);
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al eliminar producto simulado.", "SIMULACCRED_ELIMINAPRODSIM", ex);
            }
        }


        private void ConvertirProductosSimuladosEnOportunidades(ProductoSimulado[] productosSim)
        {
            if (productosSim.Length > 0)
            {
                SimulacionCredito negSimCred = new SimulacionCredito(Credenciales.ObtenerCredenciales());
                ArrayList errores = negSimCred.CrearOportunidades(productosSim, new Guid(hdnSimCredito.Value), new Guid(hdnCodDivisa.Value));

                DeshabilitarEliminarTodosProductosSimulados();
                DeshabilitarIngresoActivo();
                DeshabilitarIngresoPasivo();
                DeshabilitarIngresoServicio();

                if (errores.Count == 0)
                {
                    //exito
                    DeshabilitarEdicionFormulario();
                    negSimCred.ActualizarEstadoOportunidadesGeneradas(new Guid(hdnSimCredito.Value), true);
                    string mensaje = @"<script type=""text/javascript""> alert(""Las oportunidades han sido creadas exitosamente."");</script>";
                    this.ClientScript.RegisterStartupScript(this.GetType(), "EXITO_PRODUCTOSSIMULADOS_OPORTUNIDADES", mensaje);
                }
                else
                {
                    //el menos existió un error
                    HabilitarEliminarProductosSimulados(errores);
                    foreach (object error in negSimCred.Errores)
                    {
                        Utilidades.ReportarError(this.Page, hdnCodUsuario.Value, "Error al crear oportunidad.", "SIM_CRED_GEN_OPORT", new Exception(error.ToString()));
                    }
                    string mensaje = @"<script type=""text/javascript""> alert(""Han existido errores al convertir productos simulados en oportunidades. Intente nuevamente por favor."");</script>";
                    this.ClientScript.RegisterStartupScript(this.GetType(), "ERROR_PRODUCTOSSIMULADOS_OPORTUNIDADES", mensaje);
                }
            }
            else
            {
                string mensaje = @"<script type=""text/javascript""> alert(""Debe agregar al menos un produto para poder generar oportunidades."");</script>";
                this.ClientScript.RegisterStartupScript(this.GetType(), "ERROR_PRODUCTOSSIMULADOS_OPORTUNIDADES", mensaje);
            }
        }


        private bool ExisteSobranteDisponible(decimal sobrante, decimal nvoCemAjustadoVivienda)
        {
            SimulacionCredito negSimCred = new SimulacionCredito(Credenciales.ObtenerCredenciales());
            decimal cuotaMinima = negSimCred.CuotaMinimaSobrante();

            if (sobrante > cuotaMinima || nvoCemAjustadoVivienda > cuotaMinima)
                return true;
            else
                return false;
        }

        public void MostrarReporte(string simId, string prodSimId, string clienteId, string productoId, string productoTipoId,
            string productoFamId, string plazo, string tasaFija, string tre, string spreadFijo, string tasaVarPar, string orden,
            string amortizacCada, string tasaVar, string montoSol, string segCesa, string segDesg, string productoNombre)
        {
            Reporte negReporte = new Reporte(Credenciales.ObtenerCredenciales());
            Guid idReport = negReporte.obtenerIdReporte(ConfigurationManager.AppSettings["ReporteProductoSimuladoNombre"]);
            string pathReporte = ConfigurationManager.AppSettings["ReporteRuta"];
            string urlReporte = "";
            string organizRep = ConfigurationManager.AppSettings["ReporteOrganizacion"];

            string paginaRep = "ReportViewer.aspx?%2f" + organizRep;
            paginaRep += "%2fCustomReports%2f%7b";
            paginaRep += idReport.ToString();
            paginaRep += "%7d&rs:Command=Render&";
            paginaRep += "ProductoSimuladoId=" + prodSimId + "&";
            paginaRep += "Cliente=" + clienteId + "&";
            paginaRep += "ProductId=" + productoId + "&";
            paginaRep += "ProductTipo=" + productoTipoId + "&";
            paginaRep += "ProductFamilia=" + productoFamId + "&";
            paginaRep += "Plazo=" + plazo + "&";

            paginaRep += "Orden=" + orden + "&";
            paginaRep += "Amocad=" + amortizacCada + "&";

            paginaRep += "SegCesantia=" + segCesa + "&";
            paginaRep += "SegDesgrav=" + segDesg + "&";
            paginaRep += "c_simulacionId=" + simId + "&";
            paginaRep += "ProductoDes=" + productoNombre;

            urlReporte = pathReporte + paginaRep;

            string jsWndOpn = "window.open('" + urlReporte + "', '', 'location=no,menubar=no,status=no,toolbar=no,resizable=1,scrollbars=1,width=840,height=640')";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "REPORTE", jsWndOpn, true);
        }

        public void SetEstadoGuardado()
        {
            try
            {
                SimulacionCredito negSimCred = new SimulacionCredito(Credenciales.ObtenerCredenciales());
                negSimCred.ActualizarEstadoGuardado(new Guid(hdnSimCredito.Value), true);
                hdnSimGuardada.Value = "1";
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this.Page, Request.QueryString[Utilidades.NombresParametros.Usuario], "No se pudo guardar la simulación, intente nuevamente por favor.",
                    "SIMULACION_GUARDAR", ex);
            }
        }
        #endregion


        #region Creación de simulador crediticio
        private void AgregarSimulacionCrediticia()
        {
            try
            {
                Guid idCliente = new Guid(hdnClienteId.Value);

                SimulacionCrediticia objSimCred = new SimulacionCrediticia();
                objSimCred.ClienteId = idCliente;
                objSimCred.DivisaId = new Guid(hdnCodDivisa.Value);
                objSimCred.OwnerId = new Guid(hdnCodUsuario.Value);
                if (hdnCodCampania.Value.Length > 0)
                    objSimCred.CampaniaId = new Guid(hdnCodCampania.Value);
                else
                    objSimCred.CampaniaId = Guid.Empty;
                SimulacionCredito negSimCred = new SimulacionCredito(Credenciales.ObtenerCredenciales());
                Guid idSimulacCred = negSimCred.AgregarSimulacionCredito(objSimCred);
                negSimCred.AsignarPropietarioSimulacion(idSimulacCred, objSimCred.OwnerId);
                hdnSimCredito.Value = idSimulacCred.ToString();
                lblNoOferta.Text = objSimCred.NumeroOferta.ToString();

                HabilitarBotonesAgregarProductos();
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this.Page, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al crear simulación crediticia.", "SIMULACCRED_CREASIMULAC", ex);
            }
        }

        private void CerrarVentana()
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "CERRARSimulacion", "<script type=\"text/javascript\"> window.close(); </script>");
        }
        #endregion


        #region Controles

        protected void txtMoneda_TextChanged(object sender, EventArgs e)
        {
            ActualizarDivisaSimulacion(hdnCodDivisa.Value);
        }

        private void ResetCamposPasivo()
        {
            txtPasivo.Text = string.Empty;
            hdnProdPasId.Value = string.Empty;
            hdnProdPasTipoId.Value = string.Empty;
            hdnProdPasFamId.Value = string.Empty;
            btnAgregarPasivo.Enabled = false;
            btnAgregarPasivo.ImageUrl = "~/Images/16/cmd_add_disabled.png";
            cpePasivo.Collapsed = true;
        }

        private void ResetCamposServicio()
        {
            txtServicio.Text = string.Empty;
            hdnProdServId.Value = string.Empty;
            hdnProdServTipoId.Value = string.Empty;
            hdnProdServFamId.Value = string.Empty;
            btnAgregarServicio.Enabled = false;
            btnAgregarServicio.ImageUrl = "~/Images/16/cmd_add_disabled.png";
            cpeServicio.Collapsed = true;
        }

        private void ResetCamposActivo()
        {
            txtActivo.Text = string.Empty;
            hdnProdActId.Value = string.Empty;
            hdnProdActTipoId.Value = string.Empty;
            hdnProdActFamId.Value = string.Empty;
            btnAgregarActivo.Enabled = false;
            btnAgregarActivo.ImageUrl = "~/Images/16/cmd_add_disabled.png";
            cpeActivo.Collapsed = true;
        }

        private void HabilitarBusquedaActivo()
        {
            btnActivoLookup.Enabled = true;
            btnActivoLookup.ImageUrl = "~/Images/multiselect_btn.png";
        }

        private void DeshabilitarBusquedaActivo()
        {
            btnActivoLookup.Enabled = false;
            btnActivoLookup.ImageUrl = "~/Images/multiselect_btn_disabled.png";
            ResetCamposActivo();
        }

        private void DeshabilitarSeleccionMoneda()
        {
            btnMonedaLookup.Enabled = false;
            btnMonedaLookup.ImageUrl = "~/Images/multiselect_btn_disabled.png";
        }
        private void HabilitarSeleccionMoneda()
        {
            btnMonedaLookup.Enabled = true;
            btnMonedaLookup.ImageUrl = "~/Images/btn_off_lookup.png";
        }

        private void DeshabilitarBusquedaPasivo()
        {
            btnPasivoLookup.Enabled = false;
            btnPasivoLookup.ImageUrl = "~/Images/multiselect_btn_disabled.png";
        }

        private void DeshabilitarIngresoActivo()
        {
            btnAgregarActivo.Enabled = false;
            btnAgregarActivo.ImageUrl = "~/Images/16/cmd_add_disabled.png";
        }

        private void DeshabilitarIngresoPasivo()
        {
            btnAgregarPasivo.Enabled = false;
            btnAgregarPasivo.ImageUrl = "~/Images/16/cmd_add_disabled.png";
            ResetCamposPasivo();
        }

        private void DeshabilitarBusquedaServicio()
        {
            btnServicioLookup.Enabled = false;
            btnServicioLookup.ImageUrl = "~/Images/multiselect_btn_disabled.png";
        }

        private void DeshabilitarIngresoServicio()
        {
            btnAgregarServicio.Enabled = false;
            btnAgregarServicio.ImageUrl = "~/Images/16/cmd_add_disabled.png";
            ResetCamposServicio();
        }

        private void HabilitarIngresoActivo()
        {
            btnAgregarActivo.ImageUrl = "~/Images/16/cmd_add.png";
            btnAgregarActivo.Enabled = true;
        }

        private void HabilitarIngresoPasivo()
        {
            btnAgregarPasivo.ImageUrl = "~/Images/16/cmd_add.png";
            btnAgregarPasivo.Enabled = true;
        }

        private void HabilitarIngresoServicio()
        {
            btnAgregarServicio.ImageUrl = "~/Images/16/cmd_add.png";
            btnAgregarServicio.Enabled = true;
        }

        private void HabilitarBotonesAgregarProductos()
        {
            btnActivoLookup.Enabled = true;
            btnPasivoLookup.Enabled = true;
            btnServicioLookup.Enabled = true;
        }

        private void DeshabilitarEliminarTodosProductosSimulados()
        {
            foreach (GridViewRow row in gvProdServSim.Rows)
            {
                foreach (Control ctrl in row.Cells[1].Controls)
                {
                    ctrl.Visible = false;
                }
            }
        }

        private void HabilitarEliminarProductosSimulados(ArrayList idsProdsSim)
        {
            for (int i = 0; i < idsProdsSim.Count; i++)
            {
                bool secuencia = true;
                int idx = 0;
                GridViewRow row = null;
                while (secuencia)
                {
                    row = gvProdServSim.Rows[idx];
                    if (row.Cells[2].Text == idsProdsSim[i].ToString())
                    {
                        foreach (Control ctrl in row.Cells[1].Controls)
                        {
                            ctrl.Visible = false;
                        }
                        secuencia = false;
                    }

                    idx += 1;

                    if (idx > gvProdServSim.Rows.Count)
                        secuencia = false;

                }
            }
        }

        private void DeshabilitarEdicionFormulario()
        {
            DeshabilitarIngresoActivo();
            DeshabilitarBusquedaActivo();
            DeshabilitarIngresoPasivo();
            DeshabilitarBusquedaPasivo();
            DeshabilitarIngresoServicio();
            DeshabilitarBusquedaServicio();
            btnGuardar.Enabled = false;
            btnGuardarCerrar.Enabled = false;
            btnGenOport.Enabled = false;
        }

        #endregion


        #region Eventos
        protected void GuardarClick(object sender, EventArgs e)
        {
            SetEstadoGuardado();
        }

        protected void GuardarCerrarClick(object sender, EventArgs e)
        {
            SetEstadoGuardado();
            CerrarVentana();
        }

        protected void CerrarClick(object sender, EventArgs e)
        {
            CerrarVentana();
        }

        protected void GeneraOportunidadClick(object sender, EventArgs e)
        {
            SetEstadoGuardado();

            List<ProductoSimulado> prodsSimulados = new List<ProductoSimulado>();

            foreach (GridViewRow gvRow in gvProdServSim.Rows)
            {
                ProductoSimulado prodSim = new ProductoSimulado();
                prodSim.Id = new Guid(gvRow.Cells[2].Text);
                prodSim.MontoSolicitado = Utilidades.FormatStringMoneyToDecimal(gvRow.Cells[7].Text, HdSimboloDivisa.Value);
                prodSim.MontoMaximo = Utilidades.FormatStringMoneyToDecimal(gvRow.Cells[5].Text, HdSimboloDivisa.Value);
                prodSim.CuotaMaxima = Utilidades.FormatStringMoneyToDecimal(gvRow.Cells[6].Text, HdSimboloDivisa.Value);
                prodSim.CuotaPactada = Utilidades.FormatStringMoneyToDecimal(gvRow.Cells[8].Text, HdSimboloDivisa.Value);
                prodSim.NumeroCuotas = int.Parse(gvRow.Cells[12].Text);
                prodSim.Orden = int.Parse(gvRow.Cells[17].Text);
                prodSim.SpreadFijo = decimal.Parse(gvRow.Cells[15].Text, System.Globalization.CultureInfo.CreateSpecificCulture("de-DE"));
                prodSim.TasaFija = decimal.Parse(gvRow.Cells[13].Text, System.Globalization.CultureInfo.CreateSpecificCulture("de-DE"));
                prodSim.TasaVariableDesde = int.Parse(gvRow.Cells[16].Text);
                prodSim.TreSemana = decimal.Parse(gvRow.Cells[14].Text, System.Globalization.CultureInfo.CreateSpecificCulture("de-DE"));
                prodSim.ProductoFamiliaCod = int.Parse(gvRow.Cells[3].Text);
                prodSim.TipoPolizaCod = int.Parse(gvRow.Cells[21].Text);
                prodSim.Nombre = gvRow.Cells[22].Text;
                prodSim.MonedaCodISO = gvRow.Cells[21].Text;
                prodSim.ProductoFamiliaId = new Guid(gvRow.Cells[11].Text);
                prodSim.ProductoTipoId = new Guid(gvRow.Cells[10].Text);
                prodSim.ClienteId = new Guid(hdnClienteId.Value);
                prodSim.ProductoId = new Guid(gvRow.Cells[9].Text);
                prodSim.SimulacionCreditoId = new Guid(hdnSimCredito.Value);
                prodSim.NumeroOferta = int.Parse(lblNoOferta.Text);
                prodSim.FrecuenciaAmortizacionCod = int.Parse(gvRow.Cells[18].Text);
                prodSim.SeguroCesantia = Convert.ToBoolean(int.Parse(gvRow.Cells[19].Text));
                prodSim.SeguroDesgravamen = Convert.ToBoolean(int.Parse(gvRow.Cells[20].Text));
                prodSim.GeneradaSimulacion = true;

                if (hdnCodCampania.Value.Length > 0)
                    prodSim.CampaniaId = new Guid(hdnCodCampania.Value);
                else
                    prodSim.CampaniaId = Guid.Empty;

                prodSim.OwnerId = new Guid(hdnCodUsuario.Value);

                prodsSimulados.Add(prodSim);
            }
            ConvertirProductosSimuladosEnOportunidades(prodsSimulados.ToArray());
        }



        protected void GvProductosCampaniaPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvProductosCampania.PageIndex = e.NewPageIndex;
                gvProductosCampania.DataSource = dtProductosCampania;
                gvProductosCampania.DataBind();
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this.Page, Request.QueryString[Utilidades.NombresParametros.Usuario],
                    "Error al cambiar índica de tabla de productos de campaña.", "SIMULACCRED_GRIDPRODSCAMP_CHANGEPAGE", ex);
            }
        }

        protected void GvProductosSimuladosRowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[2].Visible = false; // oculta la primera columna de datos
            e.Row.Cells[3].Visible = false; // oculta la segunda columna de datos (cod fam prod)

            for (int i = 9; i <= 23; i++)
                e.Row.Cells[i].Visible = false;

            e.Row.Cells[13].Visible = true;
            e.Row.Cells[14].Visible = true;
            e.Row.Cells[15].Visible = true;
        }


        protected void GvProductosSimuladosRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells[3].Text != Entidades.Producto.FamiliaTipos.Activo.Codigo.ToString())//si no es un activo
            {
                foreach (Control ctrl in e.Row.Cells[0].Controls)
                {
                    ctrl.Visible = false;//ocultar columna para ver reporte
                }
            }
            else//es un activo
            {
                foreach (Control ctrl in e.Row.Cells[1].Controls)
                {
                    ctrl.Visible = false;//ocultar columna para eliminar registro
                }
            }
        }

        protected void BtnAgregarPasivoClick(object sender, ImageClickEventArgs e)
        {
            CrearProductoSimuladoPasivoServicio(txtPasivo.Text, hdnProdPasId.Value, hdnProdPasFamId.Value);
            ResetCamposPasivo();
        }


        protected void BtnAgregarServicioClick(object sender, ImageClickEventArgs e)
        {
            CrearProductoSimuladoPasivoServicio(txtServicio.Text, hdnProdServId.Value, hdnProdServFamId.Value);
            ResetCamposServicio();
        }


        protected void BtnAgregarActivoClick(object sender, ImageClickEventArgs e)
        {
            ResetCamposActivo();
        }


        protected void GvProdServSimSelectedIndexChanged(object sender, EventArgs e)
        {
            string simId = hdnSimCredito.Value;
            string prodSimId = gvProdServSim.SelectedRow.Cells[2].Text;
            string prodNombre = gvProdServSim.SelectedRow.Cells[4].Text;
            string clienteId = hdnClienteId.Value;
            string productoId = gvProdServSim.SelectedRow.Cells[9].Text;
            string prodTipoId = gvProdServSim.SelectedRow.Cells[10].Text;
            string prodFamId = gvProdServSim.SelectedRow.Cells[11].Text;
            string plazo = gvProdServSim.SelectedRow.Cells[12].Text;//número de cuotas
            string tasaFija = gvProdServSim.SelectedRow.Cells[13].Text;
            string tre = gvProdServSim.SelectedRow.Cells[14].Text.Replace('.', ',');
            string spreadFijo = gvProdServSim.SelectedRow.Cells[15].Text;
            string tasaVarPar = gvProdServSim.SelectedRow.Cells[16].Text;
            string orden = gvProdServSim.SelectedRow.Cells[17].Text;
            string amortCada = gvProdServSim.SelectedRow.Cells[18].Text;
            string tasaVar = spreadFijo;
            string montoSol = gvProdServSim.SelectedRow.Cells[7].Text;
            int initPos = gvProdServSim.SelectedRow.Cells[7].Text.IndexOf(HdSimboloDivisa.Value) + HdSimboloDivisa.Value.Length;
            montoSol = montoSol.Substring(initPos, montoSol.Length - initPos).Trim();

            string segCesa = gvProdServSim.SelectedRow.Cells[19].Text;
            string segDesg = gvProdServSim.SelectedRow.Cells[20].Text;

            MostrarReporte(simId, prodSimId, clienteId, productoId, prodTipoId, prodFamId, plazo, tasaFija, tre,
                spreadFijo, tasaVarPar, orden, amortCada, tasaVar, montoSol, segCesa, segDesg, prodNombre);
        }

        protected void BtnEliminarProdSimClick(object sender, ImageClickEventArgs e)
        {
            Guid prodSimId = new Guid(((GridViewRow)((System.Web.UI.WebControls.Image)(sender)).Parent.Parent).Cells[2].Text);
            EliminarProductoSimulado(prodSimId);
            HabilitarBusquedaActivo();
            hdnCargarProdsSims.Value = "1";
            CargarProductosSimulados();
        }
        #endregion

        #region Búsqueda de Productos
        private void CargarListasProductos()
        {
            //cargar lista de productos del activo
            string[] idsFamsProdsActivos = CargarFamiliasProductos(Entidades.Producto.FamiliaTipos.Activo.Codigo);
            string[] idsTiposProdsActivos = CargarTiposProductos(idsFamsProdsActivos);
            List<Entidades.Producto> listaProductosAct = BuscarProducto(string.Empty, idsFamsProdsActivos, idsTiposProdsActivos, true);
            AgregarProductosAGrid(listaProductosAct, Entidades.Producto.FamiliaTipos.Activo.Codigo);
            //cargar lista de productos del pasivo
            string[] idsFamsProdsPasivos = CargarFamiliasProductos(Entidades.Producto.FamiliaTipos.Pasivo.Codigo);
            string[] idsTiposProdsPasivos = CargarTiposProductos(idsFamsProdsPasivos);
            List<Entidades.Producto> listaProductosPas = BuscarProducto(string.Empty, idsFamsProdsPasivos, idsTiposProdsPasivos, false);
            AgregarProductosAGrid(listaProductosPas, Entidades.Producto.FamiliaTipos.Pasivo.Codigo);
            //cargar lista de productos de servicios
            string[] idsFamsProdsServicios = CargarFamiliasProductos(Entidades.Producto.FamiliaTipos.Servicio.Codigo);
            string[] idsTiposProdsServicios = CargarTiposProductos(idsFamsProdsServicios);
            List<Entidades.Producto> listaProductosServ = BuscarProducto(string.Empty, idsFamsProdsServicios, idsTiposProdsServicios, false);
            AgregarProductosAGrid(listaProductosServ, Entidades.Producto.FamiliaTipos.Servicio.Codigo);
        }

        private string[] CargarFamiliasProductos(int codTipoFamProd)
        {
            ArrayList listCodsFamsProds = new ArrayList();
            try
            {
                Negocio.Producto negProd = new Negocio.Producto(Credenciales.ObtenerCredenciales());
                List<Entidades.FamiliaProducto> familias = negProd.FamiliasProductos(codTipoFamProd.ToString());
                foreach (Entidades.FamiliaProducto familia in familias)
                {
                    listCodsFamsProds.Add(familia.Id.ToString());
                }
            }
            catch (Exception ex)
            {
                Utilidades.AgregarErrorAlLog(ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        Request.QueryString[Utilidades.NombresParametros.Usuario], ex.StackTrace);
            }
            return (string[])listCodsFamsProds.ToArray(typeof(string));
        }


        private string[] CargarTiposProductos(string[] idsFamilias)
        {
            ArrayList listCodsTiposProds = new ArrayList();
            foreach (string idFam in idsFamilias)
            {
                string[] tmpTiposProds = CargarTiposProductos(idFam);

                foreach (string idTipoProd in tmpTiposProds)
                {
                    listCodsTiposProds.Add(idTipoProd.ToString());
                }
            }
            return (string[])listCodsTiposProds.ToArray(typeof(string));
        }

        protected string[] CargarTiposProductos(string idFamilia)
        {
            ArrayList idsTiposProds = new ArrayList();
            try
            {
                Negocio.Producto negProd = new Negocio.Producto(Credenciales.ObtenerCredenciales());
                List<Entidades.TipoProducto> tiposProd = negProd.TiposProductos(idFamilia);
                foreach (Entidades.TipoProducto tipoProd in tiposProd)
                {
                    idsTiposProds.Add(tipoProd.Id.ToString());
                }
            }
            catch (Exception ex)
            {
                Utilidades.AgregarErrorAlLog(ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        Request.QueryString[Utilidades.NombresParametros.Usuario], ex.StackTrace);
            }
            return (string[])idsTiposProds.ToArray(typeof(string));
        }


        private List<Entidades.Producto> BuscarProducto(string nombreProducto, string[] familiasProductos, string[] tiposProductos, bool homologados)
        {
            List<Entidades.Producto> productosEncontrados = new List<Entidades.Producto>();
            try
            {
                ColumnSet csProd = new ColumnSet();
                csProd.Attributes = new string[] { "productid", "name", "efk_familia_productosid", "efk_tipo_productoid", "efk_habilitado_comercializar" };
                Negocio.Producto negProd = new Negocio.Producto(Credenciales.ObtenerCredenciales());
                productosEncontrados = negProd.BuscarProducto(nombreProducto, familiasProductos, tiposProductos, csProd, homologados);
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al buscar productos. ", "ErrorBusquedaProducto", ex);
            }
            return productosEncontrados;
        }


        private void AgregarProductosAGrid(List<Entidades.Producto> productos, int tipoFamiliaProducto)
        {
            DataTable dtProductos = new DataTable("Productos encontrados");
            dtProductos.Columns.Add("Producto");
            dtProductos.Columns.Add("Tipo");
            dtProductos.Columns.Add("Comercializar");
            dtProductos.Columns.Add("id_producto");
            dtProductos.Columns.Add("id_tipo_producto");
            dtProductos.Columns.Add("id_familia_producto");

            foreach (Entidades.Producto producto in productos)
            {
                DataRow row = dtProductos.NewRow();
                row[0] = producto.Nombre;
                row[1] = producto.TipoProductoNombre;
                if (producto.HabilitadoComercializar)
                    row[2] = "Sí";
                else
                    row[2] = "No";
                row[3] = producto.Id;
                row[4] = producto.TipoProductoId;
                row[5] = producto.FamiliaProductosId;
                dtProductos.Rows.Add(row);
            }

            if (tipoFamiliaProducto == Entidades.Producto.FamiliaTipos.Activo.Codigo)
            {
                gvProductosActivos.DataSource = dtProductos;
                gvProductosActivos.DataBind();
                gvProductosActivos.Visible = true;
            }
            else if (tipoFamiliaProducto == Entidades.Producto.FamiliaTipos.Pasivo.Codigo)
            {
                gvProductosPasivos.DataSource = dtProductos;
                gvProductosPasivos.DataBind();
                gvProductosPasivos.Visible = true;
            }
            else if (tipoFamiliaProducto == Entidades.Producto.FamiliaTipos.Servicio.Codigo)
            {
                gvProductosServicios.DataSource = dtProductos;
                gvProductosServicios.DataBind();
                gvProductosServicios.Visible = true;
            }
        }

        protected void GvProdActivoSimRowCreated(object sender, GridViewRowEventArgs e)
        {
            //Just changed the index of cells based on your requirements
            foreach (TableRow row in gvProductosActivos.Controls[0].Controls)
            {
                row.Cells[3].Visible = false;
                row.Cells[4].Visible = false;
                row.Cells[5].Visible = false;
            }
        }
        protected void GvProdPasivosSimRowCreated(object sender, GridViewRowEventArgs e)
        {
            //Just changed the index of cells based on your requirements
            foreach (TableRow row in gvProductosPasivos.Controls[0].Controls)
            {
                row.Cells[3].Visible = false;
                row.Cells[4].Visible = false;
                row.Cells[5].Visible = false;
            }
        }
        protected void GvProdServiciosSimRowCreated(object sender, GridViewRowEventArgs e)
        {
            //Just changed the index of cells based on your requirements
            foreach (TableRow row in gvProductosServicios.Controls[0].Controls)
            {
                row.Cells[3].Visible = false;
                row.Cells[4].Visible = false;
                row.Cells[5].Visible = false;
            }
        }
        protected void GvProdActSimRowDataBound(Object sender, GridViewRowEventArgs e)
        {
            AttachClickEventToProductGridview(e, gvProductosActivos);
        }
        protected void GvProdPasSimRowDataBound(Object sender, GridViewRowEventArgs e)
        {
            AttachClickEventToProductGridview(e, gvProductosPasivos);
        }
        protected void GvProdServSimRowDataBound(Object sender, GridViewRowEventArgs e)
        {
            AttachClickEventToProductGridview(e, gvProductosServicios);
        }

        private void AttachClickEventToProductGridview(GridViewRowEventArgs e, GridView gvProductos)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // javascript function to call on row-click event
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvProductos, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
                e.Row.Attributes.Add("title", "Seleccionar");
            }
        }

        protected void GvProdActivosSelectedIndexChanged(object sender, EventArgs e)
        {
            txtActivo.Text = gvProductosActivos.SelectedRow.Cells[0].Text;
            hdnProdActId.Value = gvProductosActivos.SelectedRow.Cells[3].Text;
            hdnProdActTipoId.Value = gvProductosActivos.SelectedRow.Cells[4].Text;
            hdnProdActFamId.Value = gvProductosActivos.SelectedRow.Cells[5].Text;
            cpeActivo.Collapsed = true;
            cpeActivo.ClientState = "true";
            HabilitarIngresoActivo();
        }
        protected void GvProdPasivosSelectedIndexChanged(object sender, EventArgs e)
        {
            txtPasivo.Text = gvProductosPasivos.SelectedRow.Cells[0].Text;
            hdnProdPasId.Value = gvProductosPasivos.SelectedRow.Cells[3].Text;
            hdnProdPasTipoId.Value = gvProductosPasivos.SelectedRow.Cells[4].Text;
            hdnProdPasFamId.Value = gvProductosPasivos.SelectedRow.Cells[5].Text;
            cpePasivo.Collapsed = true;
            cpePasivo.ClientState = "true";
            HabilitarIngresoPasivo();
        }
        protected void GvProdServiciosSelectedIndexChanged(object sender, EventArgs e)
        {
            txtServicio.Text = gvProductosServicios.SelectedRow.Cells[0].Text;
            hdnProdServId.Value = gvProductosServicios.SelectedRow.Cells[3].Text;
            hdnProdServTipoId.Value = gvProductosServicios.SelectedRow.Cells[4].Text;
            hdnProdServFamId.Value = gvProductosServicios.SelectedRow.Cells[5].Text;
            cpeServicio.Collapsed = true;
            cpeServicio.ClientState = "true";
            HabilitarIngresoServicio();
        }
        protected void BtnBuscarClickActivo(object sender, ImageClickEventArgs e)
        {
            try
            {
                BuscarProducto(txtBusqProductoActivo.Text, gvProductosActivos);
                btnBuscarActivo.Visible = false;
                btnClrBuscarActivo.Visible = true;
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al buscar producto activo.", "BUSQUEDA_PRODUCTO_SIMULACION_CREDITO", ex);
            }
        }
        protected void BtnClearBuscarClickActivo(object sender, ImageClickEventArgs e)
        {
            try
            {
                ClearBuscarProducto(gvProductosActivos);
                btnBuscarActivo.Visible = true;
                btnClrBuscarActivo.Visible = false;
                txtBusqProductoActivo.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al borrar búsqueda producto activo.", "BUSQUEDA_PRODUCTO_SIMULACION_CREDITO", ex);
            }
        }
        protected void BtnBuscarClickPasivo(object sender, ImageClickEventArgs e)
        {
            try
            {
                BuscarProducto(txtBusqProductoPasivo.Text, gvProductosPasivos);
                btnBuscarPasivo.Visible = false;
                btnClrBuscarPasivo.Visible = true;
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al buscar producto pasivo.", "BUSQUEDA_PRODUCTO_SIMULACION_CREDITO", ex);
            }
        }
        protected void BtnClearBuscarClickPasivo(object sender, ImageClickEventArgs e)
        {
            try
            {
                ClearBuscarProducto(gvProductosPasivos);
                btnBuscarPasivo.Visible = true;
                btnClrBuscarPasivo.Visible = false;
                txtBusqProductoPasivo.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al borrar búsqueda producto pasivo.", "BUSQUEDA_PRODUCTO_SIMULACION_CREDITO", ex);
            }
        }
        protected void BtnBuscarClickServicio(object sender, ImageClickEventArgs e)
        {
            try
            {
                BuscarProducto(txtBusqProductoServicio.Text, gvProductosServicios);
                btnBuscarServicio.Visible = false;
                btnClrBuscarServicio.Visible = true;
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al buscar producto pasivo.", "BUSQUEDA_PRODUCTO_SIMULACION_CREDITO", ex);
            }
        }
        protected void BtnClearBuscarClickServicio(object sender, ImageClickEventArgs e)
        {
            try
            {
                ClearBuscarProducto(gvProductosServicios);
                btnBuscarServicio.Visible = true;
                btnClrBuscarServicio.Visible = false;
                txtBusqProductoServicio.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al borrar búsqueda producto pasivo.", "BUSQUEDA_PRODUCTO_SIMULACION_CREDITO", ex);
            }
        }
        private void BuscarProducto(string nombreProducto, GridView gvProductos)
        {
            foreach (GridViewRow gvRow in gvProductos.Rows)
            {
                if (!gvRow.Cells[0].Text.Contains(nombreProducto.ToUpper()))
                    gvRow.Visible = false;
            }
        }
        private void ClearBuscarProducto(GridView gvProductos)
        {
            foreach (GridViewRow gvRow in gvProductos.Rows)
            {
                gvRow.Visible = true;
            }
        }
        #endregion

    }
}