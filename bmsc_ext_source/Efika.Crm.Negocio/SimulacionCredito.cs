using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios;
using Efika.Crm.AccesoServicios.BMSCTASAS;
using Efika.Crm.Entidades;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Client;
using System.Xml;


namespace Efika.Crm.Negocio
{
    public class SimulacionCredito
    {
        private CrmService Servicio;
        private CredencialesCRM Credenciales;
        private ArrayList errores;

        public SimulacionCredito(CredencialesCRM credenciales)
        {
            Credenciales = credenciales;
            Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
        }

        #region Número de Oferta
        public long Genera_NumeroOferta()
        {
            //<snippetFetchAggregationAndGroupBy5>
            // *****************************************************************************************************************
            //                FetchXML      estimatedvalue_max   Aggregate 5
            // *****************************************************************************************************************
            // Fetch the maximum estimatedvalue of all opportunities.  This is the equivalent of 
            // SELECT MAX(estimatedvalue) AS estimatedvalue_max ... in SQL.
            try
            {
                long numero_oferta = 0;
                string efk_numero_oferta_max = @" 
                    <fetch distinct='false' mapping='logical' aggregate='true'> 
                        <entity name='efk_simulacion_crediticia'> 
                           <attribute name='efk_numero_oferta' alias='efk_numero_oferta_max' aggregate='max' /> 
                        </entity> 
                    </fetch>";

                string result = Servicio.Fetch(efk_numero_oferta_max);
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(result);

                for (int i = 0; i < xmlDoc.FirstChild.ChildNodes.Count; i++)
                {
                    if (xmlDoc.FirstChild.ChildNodes[i].HasChildNodes)
                    {
                        for (int Y = 0; Y < xmlDoc.FirstChild.ChildNodes[i].ChildNodes.Count; Y++)
                        {
                            if ((xmlDoc.FirstChild.ChildNodes[i].ChildNodes[Y].Name == "efk_numero_oferta_max") && !String.IsNullOrWhiteSpace(xmlDoc.FirstChild.ChildNodes[i].ChildNodes[Y].InnerText))
                            {
                                numero_oferta = int.Parse(xmlDoc.FirstChild.ChildNodes[i].ChildNodes[Y].InnerText);
                                return numero_oferta;
                            }
                        }
                    }
                }

                //</snippetFetchAggregationAndGroupBy5>
                return numero_oferta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Número de Oferta2
        public long GetNumeroOferta(Guid simulacionCrediticiaID)
        {
            //<snippetFetchAggregationAndGroupBy5>
            // *****************************************************************************************************************
            //                FetchXML      estimatedvalue_max   Aggregate 5
            // *****************************************************************************************************************
            // Fetch the maximum estimatedvalue of all opportunities.  This is the equivalent of 
            // SELECT MAX(estimatedvalue) AS estimatedvalue_max ... in SQL.
            try
            {
                long numero_oferta = 0;
                string efk_numero_oferta_max = @" 
                    <fetch distinct='false' mapping='logical' > 
                        <entity name='efk_simulacion_crediticia'> 
                           <attribute name='efk_numero_oferta' /> 
                           <attribute name='efk_simulacion_crediticiaid' /> 
                           <filter type=""and"">
                            <condition attribute=""efk_simulacion_crediticiaid"" operator=""eq"" value=""{0}"" />
                           </filter>
                        </entity> 
                    </fetch>";

                string result = Servicio.Fetch(string.Format(efk_numero_oferta_max, simulacionCrediticiaID));
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(result);

                for (int i = 0; i < xmlDoc.FirstChild.ChildNodes.Count; i++)
                {
                    if (xmlDoc.FirstChild.ChildNodes[i].HasChildNodes)
                    {
                        for (int Y = 0; Y < xmlDoc.FirstChild.ChildNodes[i].ChildNodes.Count; Y++)
                        {
                            if ((xmlDoc.FirstChild.ChildNodes[i].ChildNodes[Y].Name == "efk_numero_oferta") && !String.IsNullOrWhiteSpace(xmlDoc.FirstChild.ChildNodes[i].ChildNodes[Y].InnerText))
                            {
                                numero_oferta = long.Parse(xmlDoc.FirstChild.ChildNodes[i].ChildNodes[Y].InnerText);
                                return numero_oferta;
                            }
                        }
                    }
                }

                //</snippetFetchAggregationAndGroupBy5>
                return numero_oferta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Inicial_NumeroOferta()
        {
            Parametros simCredPar = new Parametros(Credenciales);
            return simCredPar.GetIntParam("numero_oferta_inicial");
        }
        #endregion

        #region Transacciones
        private efk_productosimulado GetEntidadProductoSimulado(ProductoSimulado prodSim, Guid divisaId, bool conMontos)
        {
            try
            {
                efk_productosimulado entProdSim = new efk_productosimulado();
                if (prodSim.Id != Guid.Empty)
                {
                    entProdSim.efk_productosimuladoid = new Key();
                    entProdSim.efk_productosimuladoid.Value = prodSim.Id;
                }
                entProdSim.efk_numero_cuotas = new CrmNumber();
                entProdSim.efk_numero_cuotas.Value = prodSim.NumeroCuotas;
                //entProdSim.efk_moneda_operacion = new Picklist();
                //entProdSim.efk_moneda_operacion.Value = prodSim.MonedaOperacionCod;
                entProdSim.efk_tasa_fija = new CrmDecimal();
                entProdSim.efk_tasa_fija.Value = prodSim.TasaFija;
                entProdSim.efk_tre_semana = new CrmDecimal();
                entProdSim.efk_tre_semana.Value = prodSim.TreSemana;
                entProdSim.efk_spread_fijo = new CrmDecimal();
                entProdSim.efk_spread_fijo.Value = prodSim.SpreadFijo;
                entProdSim.efk_amortizacion_cada = new Picklist();
                entProdSim.efk_amortizacion_cada.Value = prodSim.FrecuenciaAmortizacionCod;
                entProdSim.efk_tasa_variable_apartirde = new CrmNumber();
                entProdSim.efk_tasa_variable_apartirde.Value = prodSim.TasaVariableDesde;
                entProdSim.efk_conseguro_desgravamen = new CrmBoolean();
                entProdSim.efk_conseguro_desgravamen.Value = prodSim.SeguroDesgravamen;
                entProdSim.efk_conseguro_cesantia = new CrmBoolean();
                entProdSim.efk_conseguro_cesantia.Value = prodSim.SeguroCesantia;
                if (conMontos)
                {
                    entProdSim.efk_monto_solicitado = new CrmMoney();
                    entProdSim.efk_monto_solicitado.Value = prodSim.MontoSolicitado;
                    entProdSim.efk_monto_maximo = new CrmMoney();
                    entProdSim.efk_monto_maximo.Value = prodSim.MontoMaximo;
                }
                entProdSim.efk_producto_catalogoid = new Lookup();
                entProdSim.efk_producto_catalogoid.name = prodSim.ProductoNombre;
                entProdSim.efk_producto_catalogoid.Value = prodSim.ProductoId;
                entProdSim.efk_producto_simuladoid = new Lookup();//setear el id de la simulación de crédito
                entProdSim.efk_producto_simuladoid.Value = prodSim.SimulacionCreditoId;//asignar id de simulación de crédito

                if (prodSim.ProductoFamiliaId != Guid.Empty)//agregar valor de tipo de familia de producto
                {
                    Producto negProd = new Producto(Credenciales);
                    FamiliaProducto famProd = negProd.GetFamiliaProductos(prodSim.ProductoFamiliaId);
                    entProdSim.efk_tipo_familia_producto = new Picklist();
                    entProdSim.efk_tipo_familia_producto.Value = famProd.TipoFamiliaCod;
                    if (entProdSim.efk_tipo_familia_producto.Value == Entidades.Producto.FamiliaTipos.Activo.Codigo)
                    {
                        entProdSim.efk_orden = new CrmNumber();
                        entProdSim.efk_orden.Value = prodSim.Orden;
                    }
                }

                if (prodSim.TipoPolizaCod != 0)
                {
                    entProdSim.efk_tipo_poliza = new Picklist();
                    entProdSim.efk_tipo_poliza.Value = prodSim.TipoPolizaCod;
                }

                entProdSim.transactioncurrencyid = new Lookup();
                entProdSim.transactioncurrencyid.Value = divisaId;
                entProdSim.efk_name = prodSim.Nombre;
                entProdSim.efk_numero_oferta = new CrmNumber();
                entProdSim.efk_numero_oferta.Value = prodSim.NumeroOferta;

                return entProdSim;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Guid AgregarProductoSimulado(ProductoSimulado prodSim, Guid divisaId)
        {
            Guid idProdSim;
            try
            {
                efk_productosimulado entProdSim = GetEntidadProductoSimulado(prodSim, divisaId, false);
                idProdSim = Servicio.Create(entProdSim);
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return idProdSim;
        }


        public Guid AgregarSimulacionCredito(SimulacionCrediticia simCred)
        {
            Guid idSimulacCred;

            try
            {
                efk_simulacion_crediticia entSimCredit = new efk_simulacion_crediticia();
                long numero_oferta = 0;
                simCred.Nombre = "Simulación Oferta No. " + numero_oferta.ToString();
                simCred.NumeroOferta = numero_oferta;

                entSimCredit.efk_name = simCred.Nombre;

                CrmNumber numberof = new CrmNumber();
                numberof.Value = Int32.Parse(simCred.NumeroOferta.ToString());

                entSimCredit.efk_numero_oferta = numberof;
                entSimCredit.efk_simulacion_clienteid = new Lookup();
                entSimCredit.efk_simulacion_clienteid.Value = simCred.ClienteId;

                if (simCred.DivisaId != Guid.Empty)
                {
                    entSimCredit.efk_divisaid = new Lookup();
                    entSimCredit.efk_divisaid.Value = simCred.DivisaId;
                    
                }

                if (simCred.CampaniaId != Guid.Empty)
                {
                    entSimCredit.efk_camapaniaid = new Lookup();
                    entSimCredit.efk_camapaniaid.Value = simCred.CampaniaId;
                }

                idSimulacCred = Servicio.Create(entSimCredit);
                numero_oferta = GetNumeroOferta(idSimulacCred);

                simCred.NumeroOferta = numero_oferta;

            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return idSimulacCred;
        }


        public void AsignarPropietarioSimulacion(Guid simulacionId, Guid usuarioId)
        {
            try
            {
                AssignRequest req = new AssignRequest();
                req.Assignee = new SecurityPrincipal();
                req.Assignee.PrincipalId = usuarioId;
                req.Assignee.Type = SecurityPrincipalType.User;
                req.Target = new TargetOwnedefk_simulacion_crediticia();
                ((TargetOwnedefk_simulacion_crediticia)req.Target).EntityId = simulacionId;

                Servicio.Execute(req);

            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void AsignarPropietarioProductoSimulado(Guid productoSimId, Guid usuarioId)
        {
            try
            {
                AssignRequest req = new AssignRequest();
                req.Assignee = new SecurityPrincipal();
                req.Assignee.PrincipalId = usuarioId;
                req.Assignee.Type = SecurityPrincipalType.User;
                req.Target = new TargetOwnedefk_productosimulado();
                ((TargetOwnedefk_productosimulado)req.Target).EntityId = productoSimId;

                Servicio.Execute(req);

            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ActualizarProductoSimulado(ProductoSimulado prodSim, Guid divisaId)
        {
            try
            {
                efk_productosimulado entProdSim = GetEntidadProductoSimulado(prodSim, divisaId, false);
                Servicio.Update(entProdSim);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void EliminarProductoSimulado(ProductoSimulado prodSim)
        {
            try
            {
                Servicio.Delete(EntityName.efk_productosimulado.ToString(), prodSim.Id);
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ArrayList CrearOportunidades(ProductoSimulado[] prodSimulados, Guid simulacionId, Guid divisaId)
        {
            Oportunidad negOportunidad = new Oportunidad(Credenciales);
            ArrayList failedProds = new ArrayList();
            List<Guid> oportCreadasIds = new List<Guid>();
            errores = new ArrayList();

            foreach (ProductoSimulado prodSim in prodSimulados)
            {
                try
                {
                    if (prodSim.MontoSolicitado > 0 ||
                        (prodSim.ProductoFamiliaCod == Entidades.Producto.FamiliaTipos.Pasivo.Codigo) ||
                        (prodSim.ProductoFamiliaCod == Entidades.Producto.FamiliaTipos.Servicio.Codigo))//crear oportunidades solo para productos simulados cuyo monto solicitado es mayor a 0
                    {
                        Guid opportunityId = negOportunidad.CrearOportunidad(prodSim.Nombre, prodSim, divisaId);
                        negOportunidad.AsignarPropietarioOportunidad(opportunityId, prodSim.OwnerId);
                        Guid prodOpportunityId = negOportunidad.CrearProductoOportunidad(prodSim.Nombre, opportunityId, prodSim.Nombre, prodSim, divisaId);
                        SetOportunidadProdSimCreada(prodSim.Id);
                        oportCreadasIds.Add(opportunityId);
                    }
                }
                catch (Exception ex)
                {
                    failedProds.Add(prodSim.Id);
                    errores.Add(ex.ToString());
                }
            }

            if (failedProds.Count == 0)
            {
                SetOportunidadesSimulacionCreadas(simulacionId);
                negOportunidad.RelacionarOportunidades(oportCreadasIds);
            }

            return failedProds;
        }


        public ArrayList Errores
        {
            get
            {
                return errores;
            }
        }


        private void SetOportunidadProdSimCreada(Guid prodSimId)
        {
            try
            {
                efk_productosimulado prodSim = new efk_productosimulado();
                prodSim.efk_productosimuladoid = new Key();
                prodSim.efk_productosimuladoid.Value = prodSimId;
                prodSim.efk_oportunidad_generada = new CrmBoolean();
                prodSim.efk_oportunidad_generada.Value = true;
                Servicio.Update(prodSim);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void SetOportunidadesSimulacionCreadas(Guid simId)
        {
            try
            {
                efk_simulacion_crediticia simulac = new efk_simulacion_crediticia();
                simulac.efk_simulacion_crediticiaid = new Key();
                simulac.efk_simulacion_crediticiaid.Value = simId;
                simulac.efk_oportunidades_generadas = new CrmBoolean();
                simulac.efk_oportunidades_generadas.Value = true;
                Servicio.Update(simulac);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void ActualizarMoneda(Guid simId, Guid divisaId)
        {
            try
            {
                efk_simulacion_crediticia entSimCred = new efk_simulacion_crediticia();
                entSimCred.efk_simulacion_crediticiaid = new Key();
                entSimCred.efk_simulacion_crediticiaid.Value = simId;
                entSimCred.efk_divisaid = new Lookup();
                entSimCred.efk_divisaid.Value = divisaId;
                Servicio.Update(entSimCred);
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void ActualizarEstadoGuardado(Guid simId, bool guardada)
        {
            try
            {
                efk_simulacion_crediticia entSimCred = new efk_simulacion_crediticia();
                entSimCred.efk_simulacion_crediticiaid = new Key();
                entSimCred.efk_simulacion_crediticiaid.Value = simId;
                entSimCred.efk_simulacion_guardada = new CrmBoolean();
                entSimCred.efk_simulacion_guardada.Value = guardada;
                Servicio.Update(entSimCred);
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ActualizarEstadoOportunidadesGeneradas(Guid simId, bool guardada)
        {
            try
            {
                efk_simulacion_crediticia entSimCred = new efk_simulacion_crediticia();
                entSimCred.efk_simulacion_crediticiaid = new Key();
                entSimCred.efk_simulacion_crediticiaid.Value = simId;
                entSimCred.efk_oportunidades_generadas = new CrmBoolean();
                entSimCred.efk_oportunidades_generadas.Value = guardada;
                Servicio.Update(entSimCred);
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Consultas
        public ProductoSimulado GetProductoSimulado(Guid prodSimId, AccesoServicios.CRMSDK.ColumnSet cs)
        {
            try
            {
                Producto negProducto = new Producto(Credenciales);
                Entidades.Producto producto = new Entidades.Producto();

                efk_productosimulado entProdSim = (efk_productosimulado)Servicio.Retrieve(EntityName.efk_productosimulado.ToString(), prodSimId, cs);

                if (entProdSim.efk_producto_catalogoid != null)
                {
                    AccesoServicios.CRMSDK.ColumnSet csProd = new AccesoServicios.CRMSDK.ColumnSet();
                    csProd.Attributes = new string[] { "efk_familia_productosid", "efk_tipo_productoid" };
                    producto = negProducto.GetProducto(entProdSim.efk_producto_catalogoid.Value, csProd);
                }

                ProductoSimulado prodSimulado = new ProductoSimulado(entProdSim.efk_productosimuladoid.Value, entProdSim.efk_name,
                    entProdSim.efk_numero_cuotas == null ? 0 : entProdSim.efk_numero_cuotas.Value,
                    entProdSim.efk_moneda,
                    entProdSim.efk_tasa_fija == null ? 0 : entProdSim.efk_tasa_fija.Value,
                    entProdSim.efk_tre_semana == null ? 0 : entProdSim.efk_tre_semana.Value,
                    entProdSim.efk_spread_fijo == null ? 0 : entProdSim.efk_spread_fijo.Value,
                    entProdSim.efk_amortizacion_cada == null ? 0 : entProdSim.efk_amortizacion_cada.Value,
                    entProdSim.efk_amortizacion_cada == null ? 0 : entProdSim.efk_amortizacion_cada.Value,
                    entProdSim.efk_tasa_variable_apartirde == null ? 0 : entProdSim.efk_tasa_variable_apartirde.Value,
                    entProdSim.efk_conseguro_desgravamen == null ? false : entProdSim.efk_conseguro_desgravamen.Value,
                    entProdSim.efk_conseguro_cesantia == null ? false : entProdSim.efk_conseguro_cesantia.Value,
                    entProdSim.efk_tipo_poliza == null ? 0 : entProdSim.efk_tipo_poliza.Value,
                    entProdSim.efk_monto_maximo == null ? 0 : entProdSim.efk_monto_maximo.Value,
                    entProdSim.efk_monto_solicitado == null ? 0 : entProdSim.efk_monto_solicitado.Value,
                    entProdSim.efk_producto_catalogoid == null ? Guid.Empty : entProdSim.efk_producto_catalogoid.Value,
                    entProdSim.efk_producto_catalogoid == null ? Guid.Empty : producto.TipoProductoId,
                    entProdSim.efk_producto_catalogoid == null ? Guid.Empty : producto.FamiliaProductosId,
                    Guid.Empty,
                    entProdSim.efk_producto_simuladoid == null ? Guid.Empty : entProdSim.efk_producto_simuladoid.Value,
                    entProdSim.efk_producto_catalogoid == null ? string.Empty : entProdSim.efk_producto_catalogoid.name,
                    entProdSim.efk_numero_oferta == null ? 0 : entProdSim.efk_numero_oferta.Value,
                    entProdSim.efk_orden == null ? 0 : entProdSim.efk_orden.Value);
                return prodSimulado;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public SimulacionCrediticia GetSimulacionCrediticia(Guid simId, AccesoServicios.CRMSDK.ColumnSet cs)
        {
            try
            {
                efk_simulacion_crediticia entSimulac = (efk_simulacion_crediticia)Servicio.Retrieve(EntityName.efk_simulacion_crediticia.ToString(), simId, cs);
                SimulacionCrediticia simulacion = new SimulacionCrediticia();

                if (entSimulac.efk_simulacion_clienteid != null)
                {
                    simulacion.ClienteId = entSimulac.efk_simulacion_clienteid.Value;
                }

                if (entSimulac.efk_divisaid != null)
                {
                    simulacion.DivisaId = entSimulac.efk_divisaid.Value;
                }

                simulacion.Nombre = entSimulac.efk_name;

                if (entSimulac.efk_numero_oferta != null)
                {
                    simulacion.NumeroOferta = entSimulac.efk_numero_oferta.Value;
                }

                if (entSimulac.efk_camapaniaid != null)
                {
                    simulacion.CampaniaId = entSimulac.efk_camapaniaid.Value;
                }
                else
                {
                    simulacion.CampaniaId = Guid.Empty;
                }

                if (entSimulac.efk_simulacion_guardada != null)
                {
                    simulacion.Guardada = entSimulac.efk_simulacion_guardada.Value;
                }
                else
                {
                    simulacion.Guardada = false;
                }

                simulacion.Id = simId;

                return simulacion;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ProductoSimulado> ProductosSimuladosDatosBasicos(Guid simulacCredId)
        {
            try
            {
                List<ProductoSimulado> prodsSim = new List<ProductoSimulado>();
                Efika.Crm.AccesoServicios.CRMSDK.ColumnSet cs = new Efika.Crm.AccesoServicios.CRMSDK.ColumnSet();

                cs.Attributes = new string[] { "efk_productosimuladoid", "efk_producto_catalogoid", "efk_monto_maximo", 
                    "efk_monto_solicitado", "efk_cuota_maxima_solicitada", "efk_cuota_maxima_simulacion", "efk_tipo_familia_producto",
                    "efk_numero_cuotas", "efk_orden", "efk_spread_fijo", "efk_tasa_fija", "efk_tre_semana",
                    "efk_tasa_variable_apartirde", "efk_amortizacion_cada", "efk_conseguro_cesantia", "efk_conseguro_desgravamen", "efk_cuota_sobrante",
                    "efk_tipo_poliza", "efk_name", "efk_moneda_operacion", "efk_cem_ajustado_vivienda2"};

                BusinessEntity[] beProdsSim = GetProdSimuladosEnSimulacion(simulacCredId, cs);
                foreach (BusinessEntity beProdSim in beProdsSim)
                {
                    efk_productosimulado entProdSim = (efk_productosimulado)beProdSim;
                    ProductoSimulado prodSim = new ProductoSimulado();
                    prodSim.Id = entProdSim.efk_productosimuladoid.Value;
                    prodSim.ProductoNombre = entProdSim.efk_producto_catalogoid.name;
                    prodSim.ProductoId = entProdSim.efk_producto_catalogoid.Value;
                    if (entProdSim.efk_monto_maximo != null)
                        prodSim.MontoMaximo = entProdSim.efk_monto_maximo.Value;
                    if (entProdSim.efk_monto_solicitado != null)
                        prodSim.MontoSolicitado = entProdSim.efk_monto_solicitado.Value;
                    if (entProdSim.efk_cuota_maxima_solicitada != null)
                        prodSim.CuotaPactada = entProdSim.efk_cuota_maxima_solicitada.Value;
                    if (entProdSim.efk_cuota_maxima_simulacion != null)
                        prodSim.CuotaMaxima = entProdSim.efk_cuota_maxima_simulacion.Value;
                    if (entProdSim.efk_tipo_familia_producto != null)
                        prodSim.ProductoFamiliaCod = entProdSim.efk_tipo_familia_producto.Value;
                    if (entProdSim.efk_numero_cuotas != null)
                        prodSim.NumeroCuotas = entProdSim.efk_numero_cuotas.Value;
                    if (entProdSim.efk_orden != null)
                        prodSim.Orden = entProdSim.efk_orden.Value;
                    if (entProdSim.efk_spread_fijo != null)
                        prodSim.SpreadFijo = entProdSim.efk_spread_fijo.Value;
                    if (entProdSim.efk_tasa_fija != null)
                        prodSim.TasaFija = entProdSim.efk_tasa_fija.Value;
                    if (entProdSim.efk_tre_semana != null)
                        prodSim.TreSemana = entProdSim.efk_tre_semana.Value;
                    if (entProdSim.efk_tasa_variable_apartirde != null)
                        prodSim.TasaVariableDesde = entProdSim.efk_tasa_variable_apartirde.Value;
                    if (entProdSim.efk_amortizacion_cada != null)
                        prodSim.FrecuenciaAmortizacion = entProdSim.efk_amortizacion_cada.Value;
                    if (entProdSim.efk_conseguro_cesantia != null)
                        prodSim.SeguroCesantia = entProdSim.efk_conseguro_cesantia.Value;
                    if (entProdSim.efk_conseguro_desgravamen != null)
                        prodSim.SeguroDesgravamen = entProdSim.efk_conseguro_desgravamen.Value;
                    if (entProdSim.efk_cuota_sobrante != null)
                        prodSim.CuotaSobrante = entProdSim.efk_cuota_sobrante.Value;
                    if (entProdSim.efk_tipo_poliza != null)
                        prodSim.TipoPolizaCod = entProdSim.efk_tipo_poliza.Value;
                    if (entProdSim.efk_moneda_operacion != null)
                        prodSim.MonedaOperacionCod = entProdSim.efk_moneda_operacion.Value;
                    if (entProdSim.efk_cem_ajustado_vivienda2 != null && entProdSim.efk_cuota_maxima_solicitada != null)
                        prodSim.NuevoCemAjustadoVivienda = entProdSim.efk_cem_ajustado_vivienda2.Value - entProdSim.efk_cuota_maxima_solicitada.Value;
                    prodSim.Nombre = entProdSim.efk_name;
                    prodsSim.Add(prodSim);
                }
                return prodsSim;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //retorna la cantidad de productos simulados asociados a una simulación de crédito
        private BusinessEntity[] GetProdSimuladosEnSimulacion(Guid simCredId, Efika.Crm.AccesoServicios.CRMSDK.ColumnSet cs)
        {
            try
            {
                Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression condExpSimulac = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression();
                condExpSimulac.AttributeName = "efk_producto_simuladoid";
                condExpSimulac.Operator = Efika.Crm.AccesoServicios.CRMSDK.ConditionOperator.Equal;
                condExpSimulac.Values = new string[] { simCredId.ToString() };

                Efika.Crm.AccesoServicios.CRMSDK.QueryExpression qryExp = new Efika.Crm.AccesoServicios.CRMSDK.QueryExpression();
                qryExp.EntityName = EntityName.efk_productosimulado.ToString();
                qryExp.Criteria = new Efika.Crm.AccesoServicios.CRMSDK.FilterExpression();
                qryExp.Criteria.Conditions = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression[] { condExpSimulac };
                qryExp.ColumnSet = cs;

                BusinessEntityCollection becProds = Servicio.RetrieveMultiple(qryExp);

                return becProds.BusinessEntities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //retorna el número (ordinal) de un producto simulado a insertarse en CRM
        private int GetOrdenProductoSimulado(Guid simCredId, Guid prodSimId)
        {
            int pos = 0;
            try
            {
                Efika.Crm.AccesoServicios.CRMSDK.ColumnSet cs = new Efika.Crm.AccesoServicios.CRMSDK.ColumnSet();
                cs.Attributes = new string[] { "efk_productosimuladoid" };
                BusinessEntity[] prodsSim = GetProdSimuladosEnSimulacion(simCredId, cs);
                int i = 0;
                foreach (BusinessEntity beProdSim in prodsSim)
                {
                    i++;
                    efk_productosimulado prodSim = (efk_productosimulado)beProdSim;
                    if (prodSim.efk_productosimuladoid.Value == prodSimId)
                    {
                        pos = i;
                        break;
                    }
                }
            }
            catch
            {
                pos = -1;
            }
            return pos;
        }

        //retorna el número de cuotas predeterminado para tarjetas de crédito
        public int GetNumeroCuotasDefectoTarjetaCredito()
        {
            Parametros simCredPar = new Parametros(Credenciales);
            return simCredPar.GetIntParam("numero_cuotas_tarjeta_credito");
        }

        public DataSet ObtenerTasas(string tipoClienteDesc, string segmentoClienteDesc, string moneda, string productoId)
        {
            DataSet dsTasas = new DataSet();

            DataTable dtTasaFija = new DataTable("tasafija");
            dtTasaFija.Columns.Add();

            DataTable dtTasaTre = new DataTable("tasatre");
            dtTasaTre.Columns.Add();

            DataTable dtSpread = new DataTable("spread");
            dtSpread.Columns.Add();

            System.Net.ServicePointManager.ServerCertificateValidationCallback += Entidades.Common.MyCertificatePolicy.ValidateCertificate;

            AccesoServicios.BMSCTASAS.ObtenerTasaService srvTasas = new AccesoServicios.BMSCTASAS.ObtenerTasaService();
            AccesoServicios.BMSCTASAS.obtenerTasa_request reqTasas = new AccesoServicios.BMSCTASAS.obtenerTasa_request();
            reqTasas.pContrasena = Credenciales.Password;
            reqTasas.pDominio = Credenciales.Dominio;
            reqTasas.pUsuario = Credenciales.Usuario;
            reqTasas.pProducto = productoId;
            reqTasas.pTipoCliente = tipoClienteDesc;
            reqTasas.pSegmentoCliente = segmentoClienteDesc;
            reqTasas.pMonedaOperacion = moneda;
            AccesoServicios.BMSCTASAS.obtenerTasa_response resTasas = srvTasas.ObtenerTasa(reqTasas);

            foreach (Result res in resTasas.pResultado)
            {
                if (res.Campo == "TASA FIJA")
                {
                    DataRow drTasaFija = dtTasaFija.NewRow();
                    drTasaFija[0] = res.Valor.Replace(".", ",");
                    dtTasaFija.Rows.Add(drTasaFija);
                }
                else if (res.Campo == "TASA TRE")
                {
                    DataRow drTasaTre = dtTasaTre.NewRow();
                    drTasaTre[0] = res.Valor.Replace(".", ",");
                    dtTasaTre.Rows.Add(drTasaTre);
                }
                else if (res.Campo == "SPREAD FIJO")
                {
                    DataRow drSpread = dtSpread.NewRow();
                    drSpread[0] = res.Valor.Replace(".", ",");
                    dtSpread.Rows.Add(drSpread);
                }
            }

            if (dtTasaFija.Rows.Count == 0 || dtTasaTre.Rows.Count == 0 || dtSpread.Rows.Count == 0)
            {
                throw new Exception("El servicio web de tasas no retornó los valores correspondientes.");
            }

            dsTasas.Tables.Add(dtTasaFija);
            dsTasas.Tables.Add(dtTasaTre);
            dsTasas.Tables.Add(dtSpread);

            return dsTasas;
        }

        public bool SimulacionGuardadaPorUsuario(Guid simId)
        {
            try
            {
                AccesoServicios.CRMSDK.ColumnSet cs = new AccesoServicios.CRMSDK.ColumnSet();
                cs.Attributes = new string[] { "efk_simulacion_guardada" };
                efk_simulacion_crediticia entSimCred = (efk_simulacion_crediticia)Servicio.Retrieve(EntityName.efk_simulacion_crediticia.ToString(), simId, cs);
                if (entSimCred.efk_simulacion_guardada != null)
                {
                    return entSimCred.efk_simulacion_guardada.Value;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SimulacionGeneroOportunidades(Guid simId)
        {
            try
            {
                AccesoServicios.CRMSDK.ColumnSet cs = new AccesoServicios.CRMSDK.ColumnSet();
                cs.Attributes = new string[] { "efk_oportunidades_generadas" };
                efk_simulacion_crediticia entSimCred = (efk_simulacion_crediticia)Servicio.Retrieve(EntityName.efk_simulacion_crediticia.ToString(), simId, cs);
                if (entSimCred.efk_oportunidades_generadas != null)
                {
                    return entSimCred.efk_oportunidades_generadas.Value;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public decimal CuotaMinimaSobrante()
        {
            try
            {
                Parametros simCredParam = new Parametros(Credenciales);
                return simCredParam.GetDecimalParam("cuota_min_sobrante");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Invocación a Stored Procedures
        public decimal ObtenerMontoMaximo(ProductoSimulado prodSim, int orden, string codMoneda, string connStr)
        {
            decimal montoMaximo = 0;
            SqlCommand sqlCmd = null;
            SqlConnection conn = null;

            try
            {
                sqlCmd = new SqlCommand("sp_maximo_endeudamiento");
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add(new SqlParameter("@c_productosimuladoId", SqlDbType.VarChar));
                sqlCmd.Parameters["@c_productosimuladoId"].Value = prodSim.Id.ToString();
                sqlCmd.Parameters.Add(new SqlParameter("@c_simulacionId", SqlDbType.VarChar));
                sqlCmd.Parameters["@c_simulacionId"].Value = prodSim.SimulacionCreditoId.ToString();
                sqlCmd.Parameters.Add(new SqlParameter("@c_cliente", SqlDbType.VarChar));
                sqlCmd.Parameters["@c_cliente"].Value = prodSim.ClienteId.ToString();
                sqlCmd.Parameters.Add(new SqlParameter("@c_ProductId", SqlDbType.VarChar));
                sqlCmd.Parameters["@c_ProductId"].Value = prodSim.ProductoId.ToString();
                sqlCmd.Parameters.Add(new SqlParameter("@c_ProductoTipo", SqlDbType.VarChar));
                sqlCmd.Parameters["@c_ProductoTipo"].Value = prodSim.ProductoTipoId.ToString();
                sqlCmd.Parameters.Add(new SqlParameter("@c_ProductoFamilia", SqlDbType.VarChar));
                sqlCmd.Parameters["@c_ProductoFamilia"].Value = prodSim.ProductoFamiliaId.ToString();
                sqlCmd.Parameters.Add(new SqlParameter("@c_plazo", SqlDbType.Int));
                sqlCmd.Parameters["@c_plazo"].Value = prodSim.NumeroCuotas;
                sqlCmd.Parameters.Add(new SqlParameter("@c_tasfij", SqlDbType.Decimal));
                sqlCmd.Parameters["@c_tasfij"].Value = prodSim.TasaFija;
                sqlCmd.Parameters.Add(new SqlParameter("@c_tre", SqlDbType.Decimal));
                sqlCmd.Parameters["@c_tre"].Value = prodSim.TreSemana;
                sqlCmd.Parameters.Add(new SqlParameter("@c_sprfij", SqlDbType.Decimal));
                sqlCmd.Parameters["@c_sprfij"].Value = prodSim.SpreadFijo;
                sqlCmd.Parameters.Add(new SqlParameter("@c_tasvarpar", SqlDbType.Int));
                sqlCmd.Parameters["@c_tasvarpar"].Value = prodSim.TasaVariableDesde;
                sqlCmd.Parameters.Add(new SqlParameter("@c_orden", SqlDbType.Int));
                sqlCmd.Parameters["@c_orden"].Value = orden;
                sqlCmd.Parameters.Add(new SqlParameter("@c_amocad", SqlDbType.Int));
                sqlCmd.Parameters["@c_amocad"].Value = prodSim.FrecuenciaAmortizacion;
                sqlCmd.Parameters.Add(new SqlParameter("@c_tasvar", SqlDbType.Decimal));
                sqlCmd.Parameters["@c_tasvar"].Value = prodSim.TasaVariableDesde;
                sqlCmd.Parameters.Add(new SqlParameter("@c_monsol", SqlDbType.Decimal));
                sqlCmd.Parameters["@c_monsol"].Value = prodSim.MontoSolicitado;
                sqlCmd.Parameters.Add(new SqlParameter("@c_llamada", SqlDbType.Int));
                sqlCmd.Parameters["@c_llamada"].Value = 0;
                sqlCmd.Parameters.Add(new SqlParameter("@c_segcesantia", SqlDbType.Int));
                sqlCmd.Parameters["@c_segcesantia"].Value = prodSim.SeguroCesantia ? 1 : 0;
                sqlCmd.Parameters.Add(new SqlParameter("@c_segdesgrav", SqlDbType.Int));
                sqlCmd.Parameters["@c_segdesgrav"].Value = prodSim.SeguroDesgravamen ? 1 : 0;
                sqlCmd.Parameters.Add(new SqlParameter("@c_codMoneda", SqlDbType.VarChar));
                sqlCmd.Parameters["@c_codMoneda"].Value = codMoneda;

                conn = new SqlConnection(connStr);
                conn.Open();
                sqlCmd.Connection = conn;

                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                montoMaximo = decimal.Parse(dt.Rows[0][0].ToString());


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlCmd.Dispose();
                conn.Close();
            }

            return montoMaximo;
        }       
            
        public decimal EvaluarCreditoOportunidad(Entidades.Oportunidad oportunidad, ProductoSimulado prodSim, string connStr)
        {
            decimal montoMaximo = 0;
            SqlCommand sqlCmd = null;
            SqlConnection conn = null;

            try
            {
                sqlCmd = new SqlCommand("sp_evaluacion_cred");
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add(new SqlParameter("@c_productosimuladoId", SqlDbType.VarChar));
                sqlCmd.Parameters["@c_productosimuladoId"].Value = prodSim.Id.ToString();
                sqlCmd.Parameters.Add(new SqlParameter("@c_OpportunityId", SqlDbType.VarChar));
                sqlCmd.Parameters["@c_OpportunityId"].Value = oportunidad.Id.ToString();
                sqlCmd.Parameters.Add(new SqlParameter("@c_SimulacionId", SqlDbType.VarChar));
                sqlCmd.Parameters["@c_SimulacionId"].Value = prodSim.SimulacionCreditoId.ToString();
                sqlCmd.Parameters.Add(new SqlParameter("@c_cliente", SqlDbType.VarChar));
                sqlCmd.Parameters["@c_cliente"].Value = oportunidad.ClienteId.ToString();
                sqlCmd.Parameters.Add(new SqlParameter("@c_ProductId", SqlDbType.VarChar));
                sqlCmd.Parameters["@c_ProductId"].Value = prodSim.ProductoId.ToString();
                sqlCmd.Parameters.Add(new SqlParameter("@c_ProductoTipo", SqlDbType.VarChar));
                sqlCmd.Parameters["@c_ProductoTipo"].Value = prodSim.ProductoTipoId.ToString();
                sqlCmd.Parameters.Add(new SqlParameter("@c_ProductoFamilia", SqlDbType.VarChar));
                sqlCmd.Parameters["@c_ProductoFamilia"].Value = prodSim.ProductoFamiliaId.ToString();
                sqlCmd.Parameters.Add(new SqlParameter("@c_plazo", SqlDbType.Int));
                sqlCmd.Parameters["@c_plazo"].Value = oportunidad.NumCuotas;
                sqlCmd.Parameters.Add(new SqlParameter("@c_tasfij", SqlDbType.Decimal));
                sqlCmd.Parameters["@c_tasfij"].Value = oportunidad.TasaFija;
                sqlCmd.Parameters.Add(new SqlParameter("@c_tre", SqlDbType.Decimal));
                sqlCmd.Parameters["@c_tre"].Value = oportunidad.TRE;
                sqlCmd.Parameters.Add(new SqlParameter("@c_sprfij", SqlDbType.Decimal));
                sqlCmd.Parameters["@c_sprfij"].Value = oportunidad.Spread;
                sqlCmd.Parameters.Add(new SqlParameter("@c_tasvarpar", SqlDbType.Int));
                sqlCmd.Parameters["@c_tasvarpar"].Value = oportunidad.InicioTasaVariable;
                sqlCmd.Parameters.Add(new SqlParameter("@c_orden", SqlDbType.Int));
                sqlCmd.Parameters["@c_orden"].Value = oportunidad.Orden;
                sqlCmd.Parameters.Add(new SqlParameter("@c_amocad", SqlDbType.Int));
                sqlCmd.Parameters["@c_amocad"].Value = prodSim.FrecuenciaAmortizacion;
                sqlCmd.Parameters.Add(new SqlParameter("@c_tasvar", SqlDbType.Decimal));
                sqlCmd.Parameters["@c_tasvar"].Value = oportunidad.TRE + oportunidad.Spread;
                sqlCmd.Parameters.Add(new SqlParameter("@c_monsol", SqlDbType.Decimal));
                sqlCmd.Parameters["@c_monsol"].Value = oportunidad.MontoSolicitado.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                sqlCmd.Parameters.Add(new SqlParameter("@c_llamada", SqlDbType.Int));
                sqlCmd.Parameters["@c_llamada"].Value = 0;
                sqlCmd.Parameters.Add(new SqlParameter("@c_segcesantia", SqlDbType.Int));
                sqlCmd.Parameters["@c_segcesantia"].Value = prodSim.SeguroCesantia ? 1 : 0;
                sqlCmd.Parameters.Add(new SqlParameter("@c_segdesgrav", SqlDbType.Int));
                sqlCmd.Parameters["@c_segdesgrav"].Value = prodSim.SeguroDesgravamen ? 1 : 0;
                sqlCmd.Parameters.Add(new SqlParameter("@c_opportunityname", SqlDbType.VarChar));
                sqlCmd.Parameters["@c_opportunityname"].Value = oportunidad.Nombre;

                conn = new SqlConnection(connStr);
                conn.Open();
                sqlCmd.Connection = conn;

                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                montoMaximo = decimal.Parse(dt.Rows[0][0].ToString());


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlCmd.Dispose();
                conn.Close();
            }

            return montoMaximo;
        }
        #endregion

        public class Parametros
        {
            private CrmService Servicio;

            public Parametros(CredencialesCRM credenciales)
            {
                Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
            }

            public int GetIntParam(string nombreParametro)
            {
                try
                {

                    AccesoServicios.CRMSDK.ConditionExpression condExp = new AccesoServicios.CRMSDK.ConditionExpression();
                    condExp.AttributeName = "efk_name";
                    condExp.Operator = AccesoServicios.CRMSDK.ConditionOperator.Equal;
                    condExp.Values = new string[] { nombreParametro };

                    AccesoServicios.CRMSDK.ColumnSet cs = new AccesoServicios.CRMSDK.ColumnSet();
                    cs.Attributes = new string[] { "efk_valor_entero" };

                    AccesoServicios.CRMSDK.QueryExpression qryExp = new AccesoServicios.CRMSDK.QueryExpression();
                    qryExp.EntityName = "efk_paramtero_simulacion_crediticia";
                    qryExp.ColumnSet = cs;
                    qryExp.Criteria = new Crm.AccesoServicios.CRMSDK.FilterExpression();
                    qryExp.Criteria.Conditions = new Crm.AccesoServicios.CRMSDK.ConditionExpression[] { condExp };

                    BusinessEntityCollection bEntColl = Servicio.RetrieveMultiple(qryExp);

                    if (bEntColl.BusinessEntities.Length > 0)
                    {
                        efk_paramtero_simulacion_crediticia entParamSimCred = (efk_paramtero_simulacion_crediticia)bEntColl.BusinessEntities[0];
                        return entParamSimCred.efk_valor_entero.Value;
                    }
                    else
                        throw new Exception("Parámetro de simulación no encontrado.");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public decimal GetDecimalParam(string nombreParametro)
            {
                try
                {

                    AccesoServicios.CRMSDK.ConditionExpression condExp = new AccesoServicios.CRMSDK.ConditionExpression();
                    condExp.AttributeName = "efk_name";
                    condExp.Operator = AccesoServicios.CRMSDK.ConditionOperator.Equal;
                    condExp.Values = new string[] { nombreParametro };

                    AccesoServicios.CRMSDK.ColumnSet cs = new AccesoServicios.CRMSDK.ColumnSet();
                    cs.Attributes = new string[] { "efk_valor_decimal" };

                    AccesoServicios.CRMSDK.QueryExpression qryExp = new AccesoServicios.CRMSDK.QueryExpression();
                    qryExp.EntityName = "efk_paramtero_simulacion_crediticia";
                    qryExp.ColumnSet = cs;
                    qryExp.Criteria = new Crm.AccesoServicios.CRMSDK.FilterExpression();
                    qryExp.Criteria.Conditions = new Crm.AccesoServicios.CRMSDK.ConditionExpression[] { condExp };

                    BusinessEntityCollection bEntColl = Servicio.RetrieveMultiple(qryExp);

                    if (bEntColl.BusinessEntities.Length > 0)
                    {
                        efk_paramtero_simulacion_crediticia entParamSimCred = (efk_paramtero_simulacion_crediticia)bEntColl.BusinessEntities[0];
                        return entParamSimCred.efk_valor_decimal.Value;
                    }
                    else
                        throw new Exception("Parámetro de simulación no encontrado.");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
    }
}
