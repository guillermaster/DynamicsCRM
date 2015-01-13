using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios.CRMMETADATA;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;
using System.Web.Services.Protocols;
using System.Data.SqlClient;


namespace Efika.Crm.Negocio
{
    public class OfertaValor
    {
        private CrmService Servicio;
        private MetadataService ServicioMetaData;
        private CredencialesCRM credenciales;
        private string dbConnectionStr;

        public OfertaValor(string connectionString)
        {
            dbConnectionStr = connectionString;
        }

        public OfertaValor(CredencialesCRM credenciales, bool servicioMetaData = false)
        {
            Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
            if (servicioMetaData)
                ServicioMetaData = ServicioCRM.ObtenerServicioMetaData(credenciales);
            this.credenciales = credenciales;            
        }


        public bool GeneraOfertaValorMicro(Guid idCliente)
        {
            SqlCommand sqlCmd = null;
            SqlConnection conn = new SqlConnection(dbConnectionStr);
            try
            {
                sqlCmd = new SqlCommand("sp_calcula_oferta_valor_micro");
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add(new SqlParameter("@idcliente", SqlDbType.UniqueIdentifier));
                sqlCmd.Parameters["@idcliente"].Value = idCliente;
                sqlCmd.Connection = conn;
                conn.Open();
                int affectedRows = sqlCmd.ExecuteNonQuery();
                if (affectedRows > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                throw;
            }
            finally
            {
                sqlCmd.Dispose();
                conn.Close(); 
            }
        }

        public DataTable GetOfertaValorMicro(Guid idCliente, string urlWebResources)
        {
            Cliente objCliente = new Cliente(credenciales);
            bool nvoElementoOV = false;
            string icono = string.Empty;
            string titulo = string.Empty;
            DataTable dtOfertaValor = new DataTable("OfertaValor");
            dtOfertaValor.Columns.Add("icono");            
            dtOfertaValor.Columns.Add("efk_probabilidad_aceptacion");
            dtOfertaValor.Columns.Add("efk_tipo_productos_id");
            dtOfertaValor.Columns.Add("efk_product_id");
            dtOfertaValor.Columns.Add("titulo");
            dtOfertaValor.Columns.Add("efk_monto_maximo");
            ParametroOfertaValor parOfVal = new ParametroOfertaValor(Servicio);
            
            try
            {
                Efika.Crm.AccesoServicios.CRMSDK.ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_tipo_productos_id", "efk_product_id", "efk_probabilidad_aceptacion", "efk_monto_maximo" };

                ConditionExpression condExpOV = new ConditionExpression();
                condExpOV.AttributeName = "efk_cliente_juridico_id";
                condExpOV.Operator = Efika.Crm.AccesoServicios.CRMSDK.ConditionOperator.Equal;
                condExpOV.Values = new string[] { idCliente.ToString() };

                OrderExpression ordExp = new OrderExpression();
                ordExp.AttributeName = "efk_probabilidad_aceptacion";
                ordExp.OrderType = OrderType.Descending;

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.efk_oferta_valor.ToString();
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { condExpOV };
                qryExp.ColumnSet = cs;
                qryExp.Orders = new OrderExpression[] { ordExp };

                BusinessEntityCollection becOV = Servicio.RetrieveMultiple(qryExp);

                foreach (BusinessEntity beOv in becOV.BusinessEntities)
                {
                    efk_oferta_valor entOfertaValor = (efk_oferta_valor)beOv;
                    DataRow dr = dtOfertaValor.NewRow();
                    if (entOfertaValor.efk_tipo_productos_id != null)//si es de tipo de producto
                    {
                        //validar probabilidad de aceptación
                        if (entOfertaValor.efk_probabilidad_aceptacion != null
                            && entOfertaValor.efk_probabilidad_aceptacion.Value >= parOfVal.GetValorMinimoAceptacionTipoProducto(entOfertaValor.efk_tipo_productos_id.Value))
                        {
                            //validar si ya tiene el producto
                            bool tieneProducto = false;
                            if(objCliente.TieneTipoProductoActivo(idCliente, entOfertaValor.efk_tipo_productos_id.Value))
                            {
                                tieneProducto = true;
                            }
                            else if (objCliente.TieneTipoProductoPasivo(idCliente, entOfertaValor.efk_tipo_productos_id.Value))
                            {
                                tieneProducto = true;
                            }

                            if (!tieneProducto)
                            {
                                Producto.Tipos negTipoProd = new Producto.Tipos(credenciales);
                                Entidades.TipoProducto tipoProd = new TipoProducto();
                                tipoProd = negTipoProd.GetNombreIcono(entOfertaValor.efk_tipo_productos_id.Value);
                                icono = tipoProd.Icono;
                                titulo = tipoProd.Nombre;
                                dr[2] = entOfertaValor.efk_tipo_productos_id.Value;
                                nvoElementoOV = true;
                            }
                        }
                    }
                    if (entOfertaValor.efk_product_id != null)//si es de subtipo de producto
                    {
                        if (entOfertaValor.efk_probabilidad_aceptacion != null
                            && entOfertaValor.efk_probabilidad_aceptacion.Value >= parOfVal.GetValorMinimoAceptacionSubtipoProducto(entOfertaValor.efk_product_id.Value))
                        {
                            //validar si ya tiene el producto                            
                            bool tieneProducto = false;
                            if (objCliente.TieneSubTipoProductoActivo(idCliente, entOfertaValor.efk_product_id.Value))
                            {
                                tieneProducto = true;
                            }
                            else if (objCliente.TieneSubTipoProductoPasivo(idCliente, entOfertaValor.efk_product_id.Value))
                            {
                                tieneProducto = true;
                            }

                            if (!tieneProducto)
                            {
                                Producto negProducto = new Producto(credenciales);
                                Entidades.Producto producto = new Entidades.Producto();
                                producto = negProducto.GetNombreIcono(entOfertaValor.efk_product_id.Value);
                                icono = producto.Icono;
                                titulo = producto.Nombre;
                                dr[3] = entOfertaValor.efk_product_id.Value;
                                nvoElementoOV = true;
                            }
                        }
                    }

                    if (nvoElementoOV)//si una oferta de valor si se está agregando
                    {
                        dr[0] = urlWebResources + "/" + icono;
                        dr[4] = titulo;

                        if (entOfertaValor.efk_probabilidad_aceptacion != null)
                        {
                            dr[1] = Math.Round((entOfertaValor.efk_probabilidad_aceptacion.Value * 100), 2);
                        }
                        else
                        {
                            dr[1] = 0;
                        }



                        if (entOfertaValor.efk_monto_maximo != null)
                        {
                            dr[5] = entOfertaValor.efk_monto_maximo.Value;
                        }

                        dtOfertaValor.Rows.Add(dr);
                        nvoElementoOV = false;
                    }
                }

                //si no tiene ningún producto en oferta, y no tiene pasivos, ofertar producto determinado
                if (dtOfertaValor.Rows.Count == 0)
                {
                    decimal defaultValue;
                    Guid idSubtipoProd = GetIdSubtipoProductoOfrecerOfertaVacia(idCliente, out defaultValue);
                    if (idSubtipoProd != Guid.Empty && !objCliente.TieneSubTipoProductoPasivo(idCliente, idSubtipoProd))
                    {
                        Producto negProducto = new Producto(credenciales);
                        Entidades.Producto producto = new Entidades.Producto();
                        producto = negProducto.GetNombreIcono(idSubtipoProd);
                        icono = producto.Icono;
                        titulo = producto.Nombre;

                        DataRow dr = dtOfertaValor.NewRow();
                        dr[0] = urlWebResources + "/" + icono;
                        dr[1] = defaultValue * 100;
                        dr[3] = idSubtipoProd;
                        dr[4] = titulo;
                        dtOfertaValor.Rows.Add(dr);
                    }
                }

                return dtOfertaValor;
            }
            catch (SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText + ". " + ex.StackTrace + "Oferta Valor: " +
                "accountid.value: " + idCliente);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Guid GetIdSubtipoProductoOfrecerOfertaVacia(Guid idCliente, out decimal defaultValue)
        {
            Guid idSubtipoProd = Guid.Empty;
            int parProdOfertaVacia = 221220017;
            defaultValue = 0;
            try
            {
                ConditionExpression condTipoPar = new ConditionExpression();
                condTipoPar.AttributeName = "efk_tipo_parametro";
                condTipoPar.Operator = ConditionOperator.Equal;
                condTipoPar.Values = new string[] { parProdOfertaVacia.ToString() };

                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_subtipodeproductoid", "efk_valor" };

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.efk_parametro_oferta_valor_micro.ToString();
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { condTipoPar };
                qryExp.ColumnSet = cs;

                BusinessEntityCollection becProds = Servicio.RetrieveMultiple(qryExp);
                if (becProds.BusinessEntities.Length > 0)
                {
                    efk_parametro_oferta_valor_micro entParam = (efk_parametro_oferta_valor_micro) becProds.BusinessEntities[0];
                    if (entParam.efk_subtipodeproductoid != null)
                    {
                        idSubtipoProd = entParam.efk_subtipodeproductoid.Value;
                    }
                    if (entParam.efk_valor != null)
                    {
                        defaultValue = entParam.efk_valor.Value;
                    }
                }

                return idSubtipoProd;
            }
            catch (SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText + ". " + ex.StackTrace + "Oferta Valor: " +
                "accountid.value: " + idCliente);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RegistrarEfectividadOfertaValor(Guid idCliente, bool efectividad, decimal montoMaximo, decimal probabilidadaceptacion, Guid subtipodeproductoid, Guid tipodeproductoid, int opcMotivoCalifNeg=0)
        {
            efk_efectividad_oferta_valor efectividad_oferta_valor = new efk_efectividad_oferta_valor();
            try
            {
                efectividad_oferta_valor.efk_clienteid = new Lookup();
                efectividad_oferta_valor.efk_clienteid.Value = idCliente;
                efectividad_oferta_valor.efk_efectividad = new AccesoServicios.CRMSDK.CrmBoolean();
                efectividad_oferta_valor.efk_efectividad.Value = efectividad;
                if (opcMotivoCalifNeg != 0)
                {
                    efectividad_oferta_valor.efk_motivo_mala_calificacion = new Picklist();
                    efectividad_oferta_valor.efk_motivo_mala_calificacion.Value = opcMotivoCalifNeg;
                }
                efectividad_oferta_valor.efk_probabilidad_aceptacion = new CrmDecimal();
                if (montoMaximo > 0)
                {
                    efectividad_oferta_valor.efk_monto_maximo = new CrmMoney();
                    efectividad_oferta_valor.efk_monto_maximo.Value = montoMaximo;
                }
                efectividad_oferta_valor.efk_probabilidad_aceptacion.Value = probabilidadaceptacion;
                if (subtipodeproductoid != Guid.Empty)
                {
                    efectividad_oferta_valor.efk_subtipodeproductoid = new Lookup();
                    efectividad_oferta_valor.efk_subtipodeproductoid.Value = subtipodeproductoid;
                }
                if (tipodeproductoid != Guid.Empty)
                {
                    efectividad_oferta_valor.efk_tipodeproductoid = new Lookup();
                    efectividad_oferta_valor.efk_tipodeproductoid.Value = tipodeproductoid;
                }
                Servicio.Create(efectividad_oferta_valor);
            }
            catch (SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText + ". " + ex.StackTrace + "efectividad_oferta_valor: " +
                "accountid: " + efectividad_oferta_valor.efk_clienteid.Value.ToString() + "customerid.type: " + efectividad_oferta_valor.efk_clienteid.type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
//    

        }


        public bool TieneRegistroEfectividad(Guid idCliente, Guid idTipoProd, Guid idSubtipoProd, out bool positivo)
        {
            try
            {
                Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression[] conditions;
                Efika.Crm.AccesoServicios.CRMSDK.ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_efectividad" };
                Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression condExpC = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression();
                condExpC.AttributeName = "efk_clienteid";
                condExpC.Operator = Efika.Crm.AccesoServicios.CRMSDK.ConditionOperator.Equal;
                condExpC.Values = new string[] { idCliente.ToString() };
                conditions = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression[] { condExpC };
                
                if (idTipoProd != null && idTipoProd != Guid.Empty)
                {
                    Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression condExpTP = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression();
                    condExpTP.AttributeName = "efk_tipodeproductoid";
                    condExpTP.Operator = Efika.Crm.AccesoServicios.CRMSDK.ConditionOperator.Equal;
                    condExpTP.Values = new string[] { idTipoProd.ToString() };
                    conditions = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression[] { condExpC, condExpTP };
                }
                else if (idSubtipoProd != null && idSubtipoProd != Guid.Empty)
                {
                    Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression condExpSP = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression();
                    condExpSP.AttributeName = "efk_subtipodeproductoid";
                    condExpSP.Operator = Efika.Crm.AccesoServicios.CRMSDK.ConditionOperator.Equal;
                    condExpSP.Values = new string[] { idSubtipoProd.ToString() };
                    conditions = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression[] { condExpC, condExpSP };
                }
                

                Efika.Crm.AccesoServicios.CRMSDK.QueryExpression qryExp = new Efika.Crm.AccesoServicios.CRMSDK.QueryExpression();
                qryExp.EntityName = EntityName.efk_efectividad_oferta_valor.ToString();
                qryExp.Criteria = new Efika.Crm.AccesoServicios.CRMSDK.FilterExpression();
                qryExp.Criteria.Conditions = conditions;                
                qryExp.ColumnSet = cs;

                BusinessEntityCollection becOV = Servicio.RetrieveMultiple(qryExp);

                if (becOV.BusinessEntities.Length > 0)
                {
                    efk_efectividad_oferta_valor efectividadOV = (efk_efectividad_oferta_valor) becOV.BusinessEntities[0];
                    positivo = efectividadOV.efk_efectividad != null ? efectividadOV.efk_efectividad.Value : false;
                    return true;
                }
                else
                {
                    positivo = false;
                    return false;
                }
            }
            catch (SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText + ". " + ex.StackTrace + "Reg.Efect.Oferta Valor");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<string[]> GetOpcionesMotivosMalaCalificacionOV()
        {
            List<string[]> motivos = new List<string[]>();

            try
            {
                motivos = AccesoServicios.ConjuntoOpcionesCRM.ObtenerOptionSetValue(ServicioMetaData, EntityName.efk_efectividad_oferta_valor.ToString(), "efk_motivo_mala_calificacion");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return motivos;
        }

        public class ParametroOfertaValor
        {
            private CrmService Servicio;

            public ParametroOfertaValor(CrmService Servicio)
            {
                this.Servicio = Servicio;
            }

            public decimal GetValorMinimoAceptacionTipoProducto(Guid idTipoProducto)
            {
                return GetParametro(idTipoProducto, Guid.Empty, Entidades.ParametroOfertaValor.TipoParametro.ProbabMinimaAceptacion.Valor);
            }

            public decimal GetValorMinimoAceptacionSubtipoProducto(Guid idSubtipoProducto)
            {
                return GetParametro(Guid.Empty, idSubtipoProducto, Entidades.ParametroOfertaValor.TipoParametro.ProbabMinimaAceptacion.Valor);
            }

            public decimal GetValorMaximoFugaTipoProducto(Guid idTipoProducto)
            {
                return GetParametro(idTipoProducto, Guid.Empty, Entidades.ParametroOfertaValor.TipoParametro.ProbabMaxFugaProducto.Valor);
            }

            public decimal GetValorMaximoFugaClienteNatural()
            {
                return GetParametro(Guid.Empty, Guid.Empty, Entidades.ParametroOfertaValor.TipoParametro.ProbabMaxFugaClienteNatural.Valor);
            }

            public decimal GetValorMaximoFugaClienteJuridico()
            {
                return GetParametro(Guid.Empty, Guid.Empty, Entidades.ParametroOfertaValor.TipoParametro.ProbabMaxFugaClienteJuridico.Valor);
            }

            private decimal GetParametro(Guid idTipoProducto, Guid idSubtipoProducto, string tipoParametro)
            {
                try
                {
                    Efika.Crm.AccesoServicios.CRMSDK.ColumnSet cs = new ColumnSet();
                    cs.Attributes = new string[] { "efk_valor" };

                    Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression condExpTipopar = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression();
                    condExpTipopar.Operator = Efika.Crm.AccesoServicios.CRMSDK.ConditionOperator.Equal;
                    condExpTipopar.AttributeName = "efk_tipo_parametro";
                    condExpTipopar.Values = new string[] { tipoParametro };

                    Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression condExp = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression();
                    condExp.Operator = Efika.Crm.AccesoServicios.CRMSDK.ConditionOperator.Equal;
                    if (idTipoProducto != null && idTipoProducto != Guid.Empty)
                    {
                        condExp.AttributeName = "efk_tipodeproductoid";
                        condExp.Values = new string[] { idTipoProducto.ToString() };
                    }
                    else if (idSubtipoProducto != null && idSubtipoProducto != Guid.Empty)
                    {
                        condExp.AttributeName = "efk_subtipodeproductoid";
                        condExp.Values = new string[] { idSubtipoProducto.ToString() };
                    }
                    
                    Efika.Crm.AccesoServicios.CRMSDK.QueryExpression qryExp = new Efika.Crm.AccesoServicios.CRMSDK.QueryExpression();
                    qryExp.EntityName = EntityName.efk_parametro_oferta_valor_micro.ToString();
                    qryExp.Criteria = new Efika.Crm.AccesoServicios.CRMSDK.FilterExpression();
                    qryExp.Criteria.Conditions = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression[] { condExp };
                    qryExp.ColumnSet = cs;

                    BusinessEntityCollection becOV = Servicio.RetrieveMultiple(qryExp);
                    if (becOV.BusinessEntities.Length > 0)
                    {
                        efk_parametro_oferta_valor_micro entParOV = (efk_parametro_oferta_valor_micro)becOV.BusinessEntities[0];
                        return entParOV.efk_valor.Value;
                    }
                    else
                    {
                        throw new Exception("Parámetro umbral de oferta de valor no encontrado. Para el tipo de producto " + idTipoProducto.ToString() +
                            " Subtipo de producto " + idSubtipoProducto.ToString() + " tipo de parámetro " + tipoParametro);
                    }
                }
                catch (SoapException ex)
                {
                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.LoadXml(ex.Detail.InnerXml);
                    throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText + ". " + ex.StackTrace);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
