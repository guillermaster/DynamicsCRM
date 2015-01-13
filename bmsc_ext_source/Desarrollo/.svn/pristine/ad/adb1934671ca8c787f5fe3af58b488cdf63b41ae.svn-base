using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;
using System.Web.Services.Protocols;

namespace Efika.Crm.Negocio
{
    public class Oportunidad
    {
        private CrmService Servicio;
        private CredencialesCRM credenciales;

        public Oportunidad(CredencialesCRM credenciales)
        {
            Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
            this.credenciales = credenciales;
        }

        public Guid CrearOportunidadDesdeRespCamp(string tema, Guid idCliente, Guid idCampania, Guid idRespCampania, DateTime fechaEstimadaCierre)
        {
            Guid idOportunidad = Guid.Empty;
            try
            {
                opportunity oportunidad = new opportunity();
                oportunidad.name = tema;

                oportunidad.customerid = new Customer();
                oportunidad.customerid.Value = idCliente;
                oportunidad.customerid.type = EntityName.account.ToString();
                oportunidad.campaignid = new Lookup();
                oportunidad.campaignid.Value = idCampania;
                oportunidad.estimatedclosedate = new CrmDateTime();
                oportunidad.estimatedclosedate.Value = fechaEstimadaCierre.ToString();
                oportunidad.efk_campaignresponse_id = new Lookup();
                oportunidad.efk_campaignresponse_id.Value = idRespCampania;
                //crear oportunidad
                idOportunidad = Servicio.Create(oportunidad);
                //asignar id de oportunidad en db
                oportunidad.opportunityid = new Key();
                oportunidad.opportunityid.Value = idOportunidad;
                //agregar productos
                int cantidadPropductosAgregados = AgregarProductosDeCampaniaAOportunidad(oportunidad);

                if (cantidadPropductosAgregados > 0)
                {
                    oportunidad = new opportunity();
                    oportunidad.opportunityid = new Key();
                    oportunidad.opportunityid.Value = idOportunidad;
                    oportunidad.efk_productos_agregados_campania = new CrmBoolean();
                    oportunidad.efk_productos_agregados_campania.Value = true;
                    oportunidad.efk_mensaje_productos_agregados_mostrado = new CrmBoolean();
                    oportunidad.efk_mensaje_productos_agregados_mostrado.Value = false;
                    oportunidad.efk_detalle_productos_agregados = "Se creó la Oportunidad con Productos (" + 
                        cantidadPropductosAgregados.ToString() + ") de Campaña agregados automáticamente a la Oportunidad.";

                    Servicio.Update(oportunidad);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return idOportunidad;
        }

        public Guid CrearOportunidad(string nombre, ProductoSimulado prodSim, Guid divisaId)
        {
            Guid idOportunidad = Guid.Empty;
            opportunity oportunidad = new opportunity();
            try
            {                
                oportunidad.name = nombre;
                oportunidad.customerid = new Customer();
                oportunidad.customerid.Value = prodSim.ClienteId;
                oportunidad.customerid.type = EntityName.account.ToString();

                Cliente negCliente = new Cliente(credenciales);
                oportunidad.efk_telefono_contacto = negCliente.GetTelefonoCliente(prodSim.ClienteId);
                oportunidad.efk_cuota_maxima_simulacion = new CrmMoney();
                oportunidad.efk_cuota_maxima_simulacion.Value = prodSim.CuotaMaxima;
                oportunidad.efk_cuota_maxima_solicitada = new CrmMoney();
                oportunidad.efk_cuota_maxima_solicitada.Value = prodSim.CuotaPactada;
                oportunidad.efk_numero_oferta = new CrmNumber();
                oportunidad.efk_numero_oferta.Value = prodSim.NumeroOferta;
                oportunidad.efk_monto_maximo = new CrmMoney();
                oportunidad.efk_monto_maximo.Value = prodSim.MontoMaximo;
                oportunidad.efk_monto_solicitado = new CrmMoney();
                oportunidad.efk_monto_solicitado.Value = prodSim.MontoSolicitado;
                oportunidad.efk_numero_cuotas = new CrmNumber();
                oportunidad.efk_numero_cuotas.Value = prodSim.NumeroCuotas;
                if (prodSim.Orden > 0)
                {
                    oportunidad.efk_orden = new CrmNumber();
                    oportunidad.efk_orden.Value = prodSim.Orden;
                }
                oportunidad.efk_producto_simuladoid = new Lookup();
                oportunidad.efk_producto_simuladoid.Value = prodSim.Id;
                oportunidad.efk_spread_fijo = new CrmDecimal();
                oportunidad.efk_spread_fijo.Value = prodSim.SpreadFijo;
                oportunidad.efk_tasa_fija = new CrmDecimal();
                oportunidad.efk_tasa_fija.Value = prodSim.TasaFija;
                oportunidad.efk_tre_semana = new CrmDecimal();
                oportunidad.efk_tre_semana.Value = prodSim.TreSemana;
                oportunidad.efk_tasa_variable_apartirde = new CrmNumber();
                oportunidad.efk_tasa_variable_apartirde.Value = prodSim.TasaVariableDesde;
                oportunidad.efk_tipo_familia_producto = new Picklist();
                oportunidad.efk_tipo_familia_producto.Value = prodSim.ProductoFamiliaCod;
                oportunidad.estimatedvalue = new CrmMoney();
                oportunidad.estimatedvalue.Value = prodSim.MontoSolicitado;
                oportunidad.estimatedclosedate = new CrmDateTime();
                oportunidad.estimatedclosedate.Value = DateTime.Now.AddDays(3).ToString("MM/dd/yyyy");
                if (prodSim.TipoPolizaCod != 0)
                {
                    oportunidad.efk_tipo_poliza = new Picklist();
                    oportunidad.efk_tipo_poliza.Value = prodSim.TipoPolizaCod;
                }
                oportunidad.transactioncurrencyid = new Lookup();
                oportunidad.transactioncurrencyid.Value = divisaId;

                oportunidad.pricelevelid = new Lookup();
                oportunidad.pricelevelid.Value = ListaDePrecios.ListaDePreciosId(Servicio, divisaId);

                oportunidad.efk_con_seguro_cesantia = new CrmBoolean();
                oportunidad.efk_con_seguro_cesantia.Value = prodSim.SeguroCesantia;

                oportunidad.efk_con_seguro_desgravamen = new CrmBoolean();
                oportunidad.efk_con_seguro_desgravamen.Value = prodSim.SeguroDesgravamen;

                oportunidad.efk_amortizacion_cada = prodSim.FrecuenciaAmortizacionCod.ToString();

                oportunidad.efk_pendiente_recalcular = new CrmBoolean();
                oportunidad.efk_pendiente_recalcular.Value = true;
                oportunidad.efk_lista_para_recalcular = new CrmBoolean();
                if (prodSim.Orden == 1)
                {
                    oportunidad.efk_lista_para_recalcular.Value = true;
                }
                else
                {
                    oportunidad.efk_lista_para_recalcular.Value = false;
                }
                if (prodSim.CampaniaId != Guid.Empty)
                {
                    oportunidad.campaignid = new Lookup();
                    oportunidad.campaignid.Value = prodSim.CampaniaId;
                }
                //crear oportunidad
                idOportunidad = Servicio.Create(oportunidad);
                
            }
            catch (SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText + ". " + ex.StackTrace + "oportunidad: " +
                "customerid.value: " + oportunidad.customerid.Value.ToString() + "customerid.type: " + oportunidad.customerid.type +
                "efk_telefono_contacto: " + oportunidad.efk_telefono_contacto);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return idOportunidad;
        }

        
        public Guid CrearProductoOportunidad(string nombre, Guid oportunidadId, string oportunidadNombre, ProductoSimulado prodSim, Guid divisaId)
        {
            Guid prodOportunidadId = Guid.Empty;
            try
            {
                opportunityproduct opProduct = new opportunityproduct();

                opProduct.opportunityid = new Lookup();
                opProduct.opportunityid.Value = oportunidadId;
                opProduct.opportunityid.name = oportunidadNombre;

                opProduct.productid = new Lookup();
                opProduct.productid.name = prodSim.ProductoNombre;
                opProduct.productid.Value = prodSim.ProductoId;

                opProduct.baseamount = new CrmMoney();
                opProduct.baseamount.Value = prodSim.MontoSolicitado;

                opProduct.priceperunit = new CrmMoney();
                opProduct.priceperunit.Value = prodSim.MontoSolicitado;
                
                opProduct.efk_plazo = new CrmNumber();
                opProduct.efk_plazo.Value = prodSim.NumeroCuotas;

                opProduct.efk_tasa_fija = new CrmDecimal();
                opProduct.efk_tasa_fija.Value = prodSim.TasaFija;
                
                opProduct.efk_familia_productosid = new Lookup();
                opProduct.efk_familia_productosid.Value = prodSim.ProductoFamiliaId;

                opProduct.efk_tipo_productoid = new Lookup();
                opProduct.efk_tipo_productoid.Value = prodSim.ProductoTipoId;

                opProduct.transactioncurrencyid = new Lookup();
                opProduct.transactioncurrencyid.Value = divisaId;

                opProduct.ispriceoverridden = new CrmBoolean();
                opProduct.ispriceoverridden.Value = true;

                opProduct.quantity = new CrmDecimal();
                opProduct.quantity.Value = 1;

                Producto nProd = new Producto(credenciales);
                opProduct.uomid = new Lookup();
                opProduct.uomid.Value = nProd.GetUnidadMedidaId();
                //monto, familia y tipo de producto
                
                prodOportunidadId = Servicio.Create(opProduct);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return prodOportunidadId;
        }


        public void AsignarPropietarioOportunidad(Guid oportunidadId, Guid usuarioId)
        {
            try
            {
                AssignRequest req = new AssignRequest();
                req.Assignee = new SecurityPrincipal();
                req.Assignee.PrincipalId = usuarioId;
                req.Assignee.Type = SecurityPrincipalType.User;
                req.Target = new TargetOwnedOpportunity();
                ((TargetOwnedOpportunity)req.Target).EntityId = oportunidadId;

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

        
        public void EditarOportunidad(Entidades.Oportunidad oportunidad)
        {
            try
            {
                opportunity entOportunidad = new opportunity();
                entOportunidad.opportunityid = new Key();
                entOportunidad.opportunityid.Value = oportunidad.Id;
                entOportunidad.efk_numero_cuotas = new CrmNumber();
                entOportunidad.efk_numero_cuotas.Value = oportunidad.NumCuotas;
                entOportunidad.efk_tasa_variable_apartirde = new CrmNumber();
                entOportunidad.efk_tasa_variable_apartirde.Value = oportunidad.InicioTasaVariable;
                entOportunidad.efk_monto_maximo = new CrmMoney();
                entOportunidad.efk_monto_maximo.Value = oportunidad.MontoMaximo;
                entOportunidad.efk_monto_solicitado = new CrmMoney();
                entOportunidad.efk_monto_solicitado.Value = oportunidad.MontoSolicitado;
                entOportunidad.efk_spread_fijo = new CrmDecimal();
                entOportunidad.efk_spread_fijo.Value = oportunidad.Spread;
                entOportunidad.efk_tasa_fija = new CrmDecimal();
                entOportunidad.efk_tasa_fija.Value = oportunidad.TasaFija;
                entOportunidad.efk_tre_semana.Value = oportunidad.TRE;
                Servicio.Update(entOportunidad);
            }
            catch (SoapException ex)
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

        public int AgregarProductosDeCampaniaAOportunidad(opportunity oportunidad)
        {
            // 1) obtener productos de datos importados de campaña, para una campaña idCampania y un cliente idCliente
            // 2) obtener productos de la entidad campaignitem (productos de campaña) para la campaña idCampania
            // 3) crear una lista, con los productos del paso 1 + productos del paso 2 que no se encuentren en paso 1

            BusinessEntityCollection entColDatosImp;
            List<Entidades.Producto> productosCamp;

            ArrayList prodProcesados = new ArrayList();

            ColumnSet csProdCamp = new ColumnSet();
            csProdCamp.Attributes = new string[] { "name", "productid", "efk_familia_productosid", "efk_tipo_productoid", "transactioncurrencyid" };

            ColumnSet csDatImp = new ColumnSet();
            csDatImp.Attributes = new string[] { "efk_subtipo_producto_id", "efk_cupo_monto" };


            //obtener datos importados de campaña (con id de producto y monto)
            DatosImportadosCampania datosImp = new DatosImportadosCampania(credenciales);
            entColDatosImp = datosImp.DatosImportadosPorCampanaYCliente(oportunidad.customerid.Value, oportunidad.campaignid.Value, csDatImp);

            int cantidadPropductosAgregados = 0;
            foreach (BusinessEntity beDatoImpCamp in entColDatosImp.BusinessEntities)
            {
                efk_datos_importados_campana datoImpCamp = (efk_datos_importados_campana)beDatoImpCamp;
                Guid idProducto = datoImpCamp.efk_subtipo_producto_id.Value;
                if (!prodProcesados.Contains(idProducto))
                {
                    Producto negProd = new Producto(credenciales);
                    Entidades.Producto producto = negProd.GetProducto(idProducto, csProdCamp);
                    if (AgregarProductoAOportunidad(oportunidad.opportunityid.Value, oportunidad.name, producto, datoImpCamp.efk_cupo_monto.Value))
                    {
                        prodProcesados.Add(datoImpCamp.efk_subtipo_producto_id.Value);
                        cantidadPropductosAgregados++;
                    }
                }
            }


            //obtener productos por campaña (campaignitem)
            Campania campania = new Campania(credenciales);
            productosCamp = campania.ProductosPorCampania(oportunidad.campaignid.Value, csProdCamp);

            foreach (Entidades.Producto producto in productosCamp)
            {
                if (!prodProcesados.Contains(producto.Id))
                {
                    if (AgregarProductoAOportunidad(oportunidad.opportunityid.Value, oportunidad.name, producto, 0))
                    {
                        prodProcesados.Add(producto.Id);
                        cantidadPropductosAgregados++;
                    }
                }
            }

            return cantidadPropductosAgregados;
        }

        private bool AgregarProductoAOportunidad(Guid oportunidadId, string oportunidadNombre, Entidades.Producto producto, decimal monto)
        {
            try
            {
                opportunityproduct opProduct = new opportunityproduct();

                opProduct.opportunityid = new Lookup();
                opProduct.opportunityid.Value = oportunidadId;
                opProduct.opportunityid.name = oportunidadNombre;

                opProduct.productid = new Lookup();
                opProduct.productid.name = producto.Nombre;
                opProduct.productid.Value = producto.Id;

                opProduct.priceperunit = new CrmMoney();
                opProduct.priceperunit.Value = monto;

                opProduct.efk_familia_productosid = new Lookup();
                opProduct.efk_familia_productosid.name = producto.FamiliaProductosNombre;
                opProduct.efk_familia_productosid.Value = producto.FamiliaProductosId;

                opProduct.efk_tipo_productoid = new Lookup();
                opProduct.efk_tipo_productoid.name = producto.TipoProductoNombre;
                opProduct.efk_tipo_productoid.Value = producto.TipoProductoId;

                opProduct.transactioncurrencyid = new Lookup();
                opProduct.transactioncurrencyid.name = producto.MonedaNombre;
                opProduct.transactioncurrencyid.Value = producto.MonedaId;

                Producto nProd = new Producto(credenciales);
                opProduct.uomid = new Lookup();
                opProduct.uomid.Value = nProd.GetUnidadMedidaId();
                //monto, familia y tipo de producto
                Servicio.Create(opProduct);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void RelacionarOportunidades(List<Guid> oportunidadesIds)
        {
            try
            {
                for (int i = 0; i < oportunidadesIds.Count; i++)
                {
                    Guid oport1Id = oportunidadesIds[i];

                    for (int j = i + 1; j < oportunidadesIds.Count; j++)
                    {
                        Guid oport2Id = oportunidadesIds[j];
                        RelacionarOportunidades(oport1Id, oport2Id);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RelacionarOportunidades(Guid oport1Id, Guid oport2Id)
        {
            try
            {
                Moniker monOport1;
                Moniker monOport2;

                monOport1 = new Moniker();
                monOport1.Name = EntityName.opportunity.ToString();
                monOport1.Id = oport1Id;

                monOport2 = new Moniker();
                monOport2.Name = EntityName.opportunity.ToString();
                monOport2.Id = oport2Id;

                AssociateEntitiesRequest crearelacion = new AssociateEntitiesRequest();
                crearelacion.Moniker1 = monOport1;
                crearelacion.Moniker2 = monOport2;
                crearelacion.RelationshipName = "efk_opportunity_opportunity";
                Servicio.Execute(crearelacion);

                AssociateEntitiesRequest crearelacion_opuesta = new AssociateEntitiesRequest();
                crearelacion_opuesta.Moniker1 = monOport2;
                crearelacion_opuesta.Moniker2 = monOport1;
                crearelacion_opuesta.RelationshipName = "efk_opportunity_opportunity";
                Servicio.Execute(crearelacion_opuesta);
            }
            catch (SoapException ex)
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


        public opportunity GetById(Guid guid, ColumnSet columnSet)
        {
            opportunity oportunidad = null;

            try
            {
                oportunidad = (opportunity)Servicio.Retrieve(EntityName.opportunity.ToString(), guid, columnSet);
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

            return oportunidad;
        }

        public BusinessEntityCollection GetByNroOferta(int nroOferta, ColumnSet columnSet)
        {
            try
            {
                Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression ConditionExp = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression();
                Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression ConditionExp1 = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression();
                Efika.Crm.AccesoServicios.CRMSDK.QueryExpression queryExp = new Efika.Crm.AccesoServicios.CRMSDK.QueryExpression();

                ConditionExp.AttributeName = "efk_numero_oferta";
                ConditionExp.Operator = Efika.Crm.AccesoServicios.CRMSDK.ConditionOperator.Equal;
                ConditionExp.Values = new string[] { nroOferta.ToString() };

                ConditionExp1.AttributeName = "statecode";
                ConditionExp1.Operator = Efika.Crm.AccesoServicios.CRMSDK.ConditionOperator.Equal;
                ConditionExp1.Values = new object[] { 0 }; //oportunidad abierta

                queryExp.EntityName = EntityName.opportunity.ToString();
                queryExp.Criteria = new Efika.Crm.AccesoServicios.CRMSDK.FilterExpression();
                queryExp.Criteria.Conditions = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression[] { ConditionExp, ConditionExp1 };
                queryExp.ColumnSet = columnSet;

                BusinessEntityCollection becOp = Servicio.RetrieveMultiple(queryExp);

                return becOp;
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


        public Entidades.Oportunidad GetByNroSolicitud(string nroSolicitud)
        {
            try
            {
                Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression ConditionExp = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression();
                Efika.Crm.AccesoServicios.CRMSDK.QueryExpression queryExp = new Efika.Crm.AccesoServicios.CRMSDK.QueryExpression();

                ConditionExp.AttributeName = "efk_nrosolicitud";
                ConditionExp.Operator = Efika.Crm.AccesoServicios.CRMSDK.ConditionOperator.Equal;
                ConditionExp.Values = new string[] { nroSolicitud };              

                queryExp.EntityName = EntityName.opportunity.ToString();
                queryExp.Criteria = new Efika.Crm.AccesoServicios.CRMSDK.FilterExpression();
                queryExp.Criteria.Conditions = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression[] { ConditionExp };
                ColumnSet cs1 = new ColumnSet();
                cs1.Attributes = new string[] { "opportunityid","name" };
                queryExp.ColumnSet = cs1;

                BusinessEntityCollection becOp = Servicio.RetrieveMultiple(queryExp);

                opportunity opp = (opportunity)becOp.BusinessEntities[0];
                Entidades.Oportunidad oportunidad = new Entidades.Oportunidad();
                oportunidad.Id = opp.opportunityid.Value;

                return oportunidad;
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

        public Entidades.Oportunidad GetOportunidad(Guid oportunidadId, ColumnSet columnSet)
        {
            try
            {
                ColumnSet cs = new ColumnSet();
                opportunity entOport = GetById(oportunidadId, columnSet);
                Entidades.Oportunidad oportunidad = new Entidades.Oportunidad();
                if (entOport.efk_monto_solicitado != null)
                {
                    oportunidad.MontoSolicitado = entOport.efk_monto_solicitado.Value;
                }
                if (entOport.efk_nrosolicitud != null)
                {
                    oportunidad.NumeroSolicitud = entOport.efk_nrosolicitud.Value;
                }
                if (entOport.efk_numero_oferta != null)
                {
                    oportunidad.NumeroOferta = entOport.efk_numero_oferta.Value;
                }
                if (entOport.efk_orden != null)
                {
                    oportunidad.Orden = entOport.efk_orden.Value;
                }
                oportunidad.Id = oportunidadId;
                return oportunidad;
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


        public Entidades.Producto GetProductoOportunidadSimulacion(Guid oportunidadId)
        {
            try
            {
                ConditionExpression condExp = new ConditionExpression();
                condExp.AttributeName = "opportunityid";
                condExp.Operator = Efika.Crm.AccesoServicios.CRMSDK.ConditionOperator.Equal;
                condExp.Values = new string[] { oportunidadId.ToString() };

                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "productid" };

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.opportunityproduct.ToString();
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { condExp };
                qryExp.ColumnSet = cs;

                BusinessEntityCollection becProds = Servicio.RetrieveMultiple(qryExp);

                opportunityproduct entOpportProduct = (opportunityproduct)becProds.BusinessEntities[0];

                Entidades.Producto producto = new Entidades.Producto();
                if(entOpportProduct.productid != null)
                {
                    producto.Id = entOpportProduct.productid.Value;
                    producto.Nombre = entOpportProduct.productid.name;
                }

                return producto;
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


        public Entidades.Producto GetProductoOportunidadHomologado(Guid oportunidadId)
        {
            try
            {
                ConditionExpression condExp = new ConditionExpression();
                condExp.AttributeName = "opportunityid";
                condExp.Operator = Efika.Crm.AccesoServicios.CRMSDK.ConditionOperator.Equal;
                condExp.Values = new string[] { oportunidadId.ToString() };

                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "productid" };

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.opportunityproduct.ToString();
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { condExp };
                qryExp.ColumnSet = cs;

                BusinessEntityCollection becProds = Servicio.RetrieveMultiple(qryExp);

                opportunityproduct entOpportProduct = (opportunityproduct)becProds.BusinessEntities[0];

                Entidades.Producto producto = new Entidades.Producto();
                if (entOpportProduct.productid != null)
                {
                    producto.Id = entOpportProduct.productid.Value;
                    Negocio.Producto produc = new Producto(credenciales);
                    ColumnSet cs1 = new ColumnSet();
                    cs1.Attributes = new string[] { "efk_productohomologado","name","efk_tipo_productoid" };
                    producto = produc.GetProducto(producto.Id, cs1);
                }

                return producto;
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

        public Entidades.Oportunidad GetOportunidadDatosSimulacion(Guid oportunidadId)
        {
            opportunity entOportunidad = null;
            Entidades.Oportunidad oportunidad;
            ColumnSet cs = new ColumnSet();
            cs.Attributes = new string[] {"efk_monto_solicitado", "efk_numero_cuotas", "efk_orden", "efk_producto_simuladoid",
                                        "efk_spread_fijo", "efk_tasa_fija", "efk_tre_semana", "efk_tasa_variable_apartirde",
                                        "efk_monto_maximo", "customerid", "name" };

            try
            {
                entOportunidad = (opportunity)Servicio.Retrieve(EntityName.opportunity.ToString(), oportunidadId, cs);
                oportunidad = new Entidades.Oportunidad(entOportunidad.opportunityid.Value, 
                    entOportunidad.customerid == null ? Guid.Empty : entOportunidad.customerid.Value,
                    entOportunidad.efk_producto_simuladoid == null ? Guid.Empty : entOportunidad.efk_producto_simuladoid.Value,
                    entOportunidad.name,
                    entOportunidad.efk_numero_cuotas == null ? 0 : entOportunidad.efk_numero_cuotas.Value,
                    entOportunidad.efk_tasa_variable_apartirde == null ? 0 : entOportunidad.efk_tasa_variable_apartirde.Value,
                    entOportunidad.efk_monto_maximo == null ? 0 : entOportunidad.efk_monto_maximo.Value,
                    entOportunidad.efk_monto_solicitado == null ? 0 : entOportunidad.efk_monto_solicitado.Value,
                    entOportunidad.efk_spread_fijo == null ? 0 : entOportunidad.efk_spread_fijo.Value,
                    entOportunidad.efk_tasa_fija == null ? 0 : entOportunidad.efk_tasa_fija.Value,
                    entOportunidad.efk_tre_semana == null ? 0 : entOportunidad.efk_tre_semana.Value,
                    entOportunidad.efk_orden == null ? 0 : entOportunidad.efk_orden.Value,
                    entOportunidad.transactioncurrencyid == null ? Guid.Empty : entOportunidad.transactioncurrencyid.Value,
                    entOportunidad.efk_moneda);
                return oportunidad;
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


        public void SetEnvioSolicitudSistemaExterno(Guid oportunidadId, bool enviada)
        {
            try
            {
                opportunity entOport = new opportunity();
                entOport.opportunityid = new Key();
                entOport.opportunityid.Value = oportunidadId;
                entOport.efk_solicitud_enviada_fabrica = new CrmBoolean();
                entOport.efk_solicitud_enviada_fabrica.Value = enviada;
                Servicio.Update(entOport);
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


        public bool ExisteEnvioSolicitudSistExt(Guid oportunidadId)
        {
            try
            {
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_solicitud_enviada_fabrica" };

                opportunity entOport = (opportunity) Servicio.Retrieve(EntityName.opportunity.ToString(), oportunidadId, cs);

                if (entOport.efk_solicitud_enviada_fabrica != null)
                {
                    return entOport.efk_solicitud_enviada_fabrica.Value;
                }
                else
                    return false;
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


        public void SetScoreEvaluado(Guid oportunidadId, bool evaluado)
        {
            try
            {
                opportunity entOportunid = new opportunity();
                entOportunid.opportunityid = new Key();
                entOportunid.opportunityid.Value = oportunidadId;
                entOportunid.efk_solicitud_score_evaluado = new CrmBoolean();
                entOportunid.efk_solicitud_score_evaluado.Value = evaluado;
                Servicio.Update(entOportunid);
            }
            catch (SoapException ex)
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

        public void SetRecalculoListoPendiente(Guid oportunidadId, bool pendienteRecalc, bool listaRecalc)
        {
            try
            {
                opportunity entOportunid = new opportunity();
                entOportunid.opportunityid = new Key();
                entOportunid.opportunityid.Value = oportunidadId;
                entOportunid.efk_pendiente_recalcular = new CrmBoolean();
                entOportunid.efk_pendiente_recalcular.Value = pendienteRecalc;
                entOportunid.efk_lista_para_recalcular = new CrmBoolean();
                entOportunid.efk_lista_para_recalcular.Value = listaRecalc;
                Servicio.Update(entOportunid);
            }
            catch (SoapException ex)
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

        public void SetEstadosRecalcularSgtesOportunidades(Guid oportunidadId, bool debenRecalcular, bool sgteListaRecalcular)
        {
            try
            {
                Guid idOportunidadOrden1 = Guid.Empty;
                int ordenMayor = 0;

                ColumnSet csOpAct = new ColumnSet();
                csOpAct.Attributes = new string[] { "efk_orden", "efk_numero_oferta" };
                Entidades.Oportunidad oportActual = GetOportunidad(oportunidadId, csOpAct);//obtiene datos de oportunidad actual
                
                ColumnSet csOpSgtes = new ColumnSet();
                csOpSgtes.Attributes = new string[] { "efk_orden" };
                BusinessEntityCollection becOportPorNumOf = GetByNroOferta(oportActual.NumeroOferta, csOpSgtes);

                foreach (BusinessEntity be in becOportPorNumOf.BusinessEntities)
                {
                    opportunity oportunidad = (opportunity)be;
                    if (oportunidad.efk_orden != null)
                    {
                        if (oportunidad.efk_orden.Value == 1)//obteine el GUID de la oportunidad de orden 1
                        {
                            idOportunidadOrden1 = oportunidad.opportunityid.Value;
                        }
                        if (oportunidad.efk_orden.Value > oportActual.Orden)//cambia los estados de una oportunidad de orden posterior
                        {
                            SetRecalculoListoPendiente(oportunidad.opportunityid.Value, debenRecalcular, 
                                (debenRecalcular && sgteListaRecalcular && oportunidad.efk_orden.Value == (oportActual.Orden + 1)) ? true : false);
                        }
                        if (oportunidad.efk_orden.Value > ordenMayor)//para obtener el número de mayor orden
                        {
                            ordenMayor = oportunidad.efk_orden.Value;
                        }
                    }
                }

                if (oportActual.Orden == ordenMayor)//si la oportunidad actual es la de mayor orden entre todas las oportunidades de la oferta
                {
                    //asignar estado lista_para_recalcular a la oportunidad con orden igual a 1
                    SetRecalculoListoPendiente(idOportunidadOrden1, false, true);
                }
            }
            catch (SoapException ex)
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


        public void AbrirOportunidadParaRiesgos(Guid oportunidadId, int oportunidadOrden)
        {
            try
            {
                opportunity oportunidad = new opportunity();
                oportunidad.opportunityid = new Key();
                oportunidad.opportunityid.Value = oportunidadId;
                oportunidad.efk_oferta_reabierta_riesgos = new CrmBoolean();
                oportunidad.efk_oferta_reabierta_riesgos.Value = true;
                oportunidad.efk_pendiente_recalcular = new CrmBoolean();
                oportunidad.efk_pendiente_recalcular.Value = true;
                oportunidad.efk_oferta_cerrada = new CrmBoolean();
                oportunidad.efk_oferta_cerrada.Value = false;
                oportunidad.efk_solicitud_score_evaluado = new CrmBoolean();
                oportunidad.efk_solicitud_score_evaluado.Value = false;

                oportunidad.efk_lista_para_recalcular = new CrmBoolean();
                if (oportunidadOrden == 1)
                    oportunidad.efk_lista_para_recalcular.Value = true;
                else
                    oportunidad.efk_lista_para_recalcular.Value = false;

                Servicio.Update(oportunidad);
            }
            catch (SoapException ex)
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


        public void AbrirOportunidadParaGerentes(Guid oportunidadId, int oportunidadOrden)
        {
            try
            {
                opportunity oportunidad = new opportunity();
                oportunidad.opportunityid = new Key();
                oportunidad.opportunityid.Value = oportunidadId;
                oportunidad.efk_pendiente_recalcular = new CrmBoolean();
                oportunidad.efk_pendiente_recalcular.Value = true;
                oportunidad.efk_oferta_cerrada = new CrmBoolean();
                oportunidad.efk_oferta_cerrada.Value = false;
                oportunidad.efk_oferta_enviada_riesgos = new CrmBoolean();
                oportunidad.efk_oferta_enviada_riesgos.Value = false;
                oportunidad.efk_oferta_reabierta_riesgos = new CrmBoolean();
                oportunidad.efk_oferta_reabierta_riesgos.Value = false;
                oportunidad.efk_solicitud_score_evaluado = new CrmBoolean();
                oportunidad.efk_solicitud_score_evaluado.Value = false;
                oportunidad.efk_solicitud_enviada_fabrica = new CrmBoolean();
                oportunidad.efk_solicitud_enviada_fabrica.Value = false;

                oportunidad.efk_lista_para_recalcular = new CrmBoolean();
                if (oportunidadOrden == 1)
                    oportunidad.efk_lista_para_recalcular.Value = true;
                else
                    oportunidad.efk_lista_para_recalcular.Value = false;

                Servicio.Update(oportunidad);
            }
            catch (SoapException ex)
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

        public void AprobacionRiesgos(Guid oportunidadId, bool aprobada)
        {
            try
            {
                opportunity oportunidad = new opportunity();
                oportunidad.opportunityid = new Key();
                oportunidad.opportunityid.Value = oportunidadId;
                oportunidad.efk_oferta_revisada_riesgos = new CrmBoolean();
                oportunidad.efk_oferta_revisada_riesgos.Value = true;
                oportunidad.efk_estado_revision_riesgos = new Picklist();
                if (aprobada)
                    oportunidad.efk_estado_revision_riesgos.Value = EstadoRevisionRiesgos.Aprobada.Codigo;
                else
                    oportunidad.efk_estado_revision_riesgos.Value = EstadoRevisionRiesgos.Rechazada.Codigo;

                Servicio.Update(oportunidad);
            }
            catch (SoapException ex)
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


        public void EnviarOportunidadARiesgos(Guid oportunidadId, bool bandera)
        {
            try
            {
                opportunity oportunidad = new opportunity();
                oportunidad.opportunityid = new Key();
                oportunidad.opportunityid.Value = oportunidadId;

                oportunidad.efk_oferta_enviada_riesgos = new CrmBoolean();
                oportunidad.efk_oferta_enviada_riesgos.Value = bandera;

                Servicio.Update(oportunidad);
            }
            catch (SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public void SetOportunidadOfertaCerrada(Guid oportunidadId)
        {
            try
            {
                opportunity oportunidad = new opportunity();
                oportunidad.opportunityid = new Key();
                oportunidad.opportunityid.Value = oportunidadId;

                oportunidad.efk_oferta_cerrada = new CrmBoolean();
                oportunidad.efk_oferta_cerrada.Value = true;

                Servicio.Update(oportunidad);
            }
            catch (SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public bool EsOportunidadCreadaPorSimulacion(Guid opportunityId)
        {
            try
            {
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_numero_oferta" };
                opportunity oportunidad = GetById(opportunityId, cs);
                if (oportunidad.efk_numero_oferta != null && oportunidad.efk_numero_oferta.Value > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public class EstadoRevisionRiesgos
        {
            public class Aprobada
            {
                public static int Codigo { get { return 221220000; } }
            }
            public class Rechazada
            {
                public static int Codigo { get { return 221220001; } }
            }
        }
    }
}
