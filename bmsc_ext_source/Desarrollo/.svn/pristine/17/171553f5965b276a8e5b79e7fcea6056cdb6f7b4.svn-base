using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios.CRMMETADATA;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;
using Microsoft.Xrm.Sdk;


namespace Efika.Crm.Negocio
{
    public class Producto
    {
        private CrmService Servicio;
        private AccesoServicios.CRMMETADATA.MetadataService ServicioMetaData;

        public Producto(CredencialesCRM credenciales, bool servicioMetaData = false)
        {
            Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
            if (servicioMetaData)
                ServicioMetaData = ServicioCRM.ObtenerServicioMetaData(credenciales);
        }


        public Entidades.Producto GetProducto(Guid idProducto, ColumnSet columns)
        {
            Entidades.Producto producto = new Entidades.Producto();
            try
            {
                if (idProducto != null && idProducto != Guid.Empty)
                {
                    BusinessEntity beProd = Servicio.Retrieve(EntityName.product.ToString(), idProducto, columns);
                
                    product prod = (product)beProd;
                    producto.Nombre = prod.name;
                    producto.Id = idProducto;

                    if (prod.efk_tipo_productoid != null)
                    {
                        producto.TipoProductoNombre = prod.efk_tipo_productoid.name;
                        producto.TipoProductoId = prod.efk_tipo_productoid.Value;
                    }
                    if (prod.efk_familia_productosid != null)
                    {
                        producto.FamiliaProductosNombre = prod.efk_familia_productosid.name;
                        producto.FamiliaProductosId = prod.efk_familia_productosid.Value;
                    }
                    if (prod.transactioncurrencyid != null)
                    {
                        producto.MonedaNombre = prod.transactioncurrencyid.name;
                        producto.MonedaId = prod.transactioncurrencyid.Value;
                    }
                    if (prod.efk_habilitado_comercializar != null)
                    {
                        producto.HabilitadoComercializar = prod.efk_habilitado_comercializar.Value;
                    }
                    if (prod.efk_productohomologado != null)
                    {
                        producto.ProductoHomologado = prod.efk_productohomologado.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return producto;
        }


        public Guid GetUnidadMedidaId()
        {
            try
            {
                Guid unidadMedida = Guid.Empty;

                ColumnSet resultSetColumns = new ColumnSet();
                resultSetColumns.Attributes = new string[] { "uomid" };
                QueryExpression qryExpression = new QueryExpression();
                qryExpression.ColumnSet = resultSetColumns;

                qryExpression.EntityName = EntityName.uom.ToString();
                qryExpression.Distinct = false;

                BusinessEntityCollection unidadMedidadResultSet = Servicio.RetrieveMultiple(qryExpression);

                foreach (uom e_UnidadMedida in unidadMedidadResultSet.BusinessEntities)
                {
                    if (e_UnidadMedida.uomid != null)
                    {
                        unidadMedida = e_UnidadMedida.uomid.Value;
                    }
                }
                return unidadMedida;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                throw new Exception(ex.Detail.InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<string[]> GetMonedas()
        {
            List<string[]> monedas = new List<string[]>();

            try
            {
                monedas = AccesoServicios.ConjuntoOpcionesCRM.ObtenerOptionSetValue(ServicioMetaData, EntityName.efk_productosimulado.ToString(), "efk_moneda_operacion");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return monedas;
        }
                


        public List<string[]> GetTipoPoliza()
        {
            List<string[]> tipoPolizas = new List<string[]>();

            try
            {
                tipoPolizas = AccesoServicios.ConjuntoOpcionesCRM.ObtenerOptionSetValue(ServicioMetaData, EntityName.opportunity.ToString(), "efk_tipo_poliza");

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tipoPolizas;
        }


        public List<Entidades.Producto> BuscarProducto(string nombreProducto, string[] codFamiliasProductos, string[] codTiposProductos, ColumnSet campos, bool validarHomologacion = false)
        {
            List<Entidades.Producto> productos = new List<Entidades.Producto>();

            try
            {
                ConditionExpression condExpNombre = new ConditionExpression();
                if (!string.IsNullOrWhiteSpace(nombreProducto))
                {
                    condExpNombre.AttributeName = "name";
                    condExpNombre.Operator = ConditionOperator.Like;
                    condExpNombre.Values = new string[] { "%"+nombreProducto+"%" };
                }

                ConditionExpression condExpTiposProds = new ConditionExpression();
                condExpTiposProds.AttributeName = "efk_tipo_productoid";
                condExpTiposProds.Operator = ConditionOperator.In;
                condExpTiposProds.Values = codTiposProductos;

                ConditionExpression condExpFamsProds = new ConditionExpression();
                condExpFamsProds.AttributeName = "efk_familia_productosid";
                condExpFamsProds.Operator = ConditionOperator.In;
                condExpFamsProds.Values = codFamiliasProductos;

                ConditionExpression condExpHabComerc = new ConditionExpression();
                condExpHabComerc.AttributeName = "efk_habilitado_comercializar";
                condExpHabComerc.Operator = ConditionOperator.Equal;
                condExpHabComerc.Values = new string[] { "1" };

                ConditionExpression condExpValHomologac = new ConditionExpression();

                if (validarHomologacion)
                {
                    condExpValHomologac.AttributeName = "efk_productohomologado";
                    condExpValHomologac.Operator = ConditionOperator.NotNull;
                }

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.product.ToString();
                qryExp.Criteria = new FilterExpression();
                if (!string.IsNullOrWhiteSpace(nombreProducto))
                {
                    if(validarHomologacion)
                        qryExp.Criteria.Conditions = new ConditionExpression[] { condExpNombre, condExpFamsProds, condExpTiposProds, condExpValHomologac };
                    else
                        qryExp.Criteria.Conditions = new ConditionExpression[] { condExpNombre, condExpFamsProds, condExpTiposProds };
                }
                else
                {
                    if(validarHomologacion)
                        qryExp.Criteria.Conditions = new ConditionExpression[] { condExpFamsProds, condExpTiposProds, condExpValHomologac };
                    else
                        qryExp.Criteria.Conditions = new ConditionExpression[] { condExpFamsProds, condExpTiposProds };
                }
                qryExp.ColumnSet = campos;

                BusinessEntityCollection becProds = Servicio.RetrieveMultiple(qryExp);

                foreach (BusinessEntity beProd in becProds.BusinessEntities)
                {
                    product entProd = (product)beProd;
                    Entidades.Producto producto = GetProducto(entProd.productid.Value, campos);
                    productos.Add(producto);
                }

                return productos;
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


        public List<Entidades.FamiliaProducto> FamiliasProductos(string codTipoFamilia)
        {
            List<Entidades.FamiliaProducto> familias = new List<FamiliaProducto>();

            try
            {
                ConditionExpression condExp = new ConditionExpression();
                condExp.AttributeName = "efk_tipo_familia";
                condExp.Operator = ConditionOperator.Equal;
                condExp.Values = new string[] { codTipoFamilia };

                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_familia_productosid", "efk_nombre", "efk_tipo_familia" };

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.efk_familia_productos.ToString();
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { condExp };
                qryExp.ColumnSet = cs;

                BusinessEntityCollection becFams = Servicio.RetrieveMultiple(qryExp);

                foreach (BusinessEntity beFam in becFams.BusinessEntities)
                {
                    efk_familia_productos entFam = (efk_familia_productos)beFam;
                    Entidades.FamiliaProducto familia = new Entidades.FamiliaProducto();
                    familia.Id = entFam.efk_familia_productosid.Value;
                    familia.Nombre = entFam.efk_nombre;
                    familia.TipoFamiliaCod = entFam.efk_tipo_familia.Value;
                    familias.Add(familia);
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

            return familias;
        }


        public List<Entidades.TipoProducto> TiposProductos(string codFamilia)
        {
            List<Entidades.TipoProducto> tiposProds = new List<Entidades.TipoProducto>();

            try
            {
                ConditionExpression condExp = new ConditionExpression();
                condExp.AttributeName = "efk_familia_productosid";
                condExp.Operator = ConditionOperator.Equal;
                condExp.Values = new string[] { codFamilia };

                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_tipo_productoid", "efk_nombre", "efk_familia_productosid" };

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.efk_tipo_producto.ToString();
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { condExp };
                qryExp.ColumnSet = cs;

                BusinessEntityCollection becTiposProds = Servicio.RetrieveMultiple(qryExp);

                foreach (BusinessEntity beTiposProds in becTiposProds.BusinessEntities)
                {
                    efk_tipo_producto entTipoProd = (efk_tipo_producto)beTiposProds;
                    Entidades.TipoProducto tipoProd = new Entidades.TipoProducto();
                    tipoProd.Id = entTipoProd.efk_tipo_productoid.Value;
                    tipoProd.Nombre = entTipoProd.efk_nombre;
                    tipoProd.FamiliaId = entTipoProd.efk_familia_productosid.Value;
                    tiposProds.Add(tipoProd);
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

            return tiposProds;
        }


        public FamiliaProducto GetFamiliaProductos(Guid famProdId)
        {
            try
            {
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_nombre", "efk_tipo_familia" };
                BusinessEntity beFamProd = Servicio.Retrieve(EntityName.efk_familia_productos.ToString(), famProdId, cs);
                efk_familia_productos entFamProd = (efk_familia_productos)beFamProd;

                FamiliaProducto famProd = new FamiliaProducto();
                famProd.Id = famProdId;
                famProd.Nombre = entFamProd.efk_nombre;
                famProd.TipoFamiliaCod = entFamProd.efk_tipo_familia.Value;

                return famProd;
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

        public Entidades.Producto GetNombreIcono(Guid idSubtipoProd)
        {
            Entidades.Producto producto = new Entidades.Producto();
            ColumnSet cs = new ColumnSet();
            cs.Attributes = new string[] { "efk_icono", "name" };
            BusinessEntity be = Servicio.Retrieve(EntityName.product.ToString(), idSubtipoProd, cs);
            if (be != null)
            {
                product subtipoProd = (product)be;
                producto.Icono = subtipoProd.efk_icono;
                producto.Nombre = subtipoProd.name;
            }
            return producto;
        }

        public string GetNombreProducto(Guid idSubtipoProd)
        {
            string nombre = string.Empty;
            ColumnSet cs = new ColumnSet();
            cs.Attributes = new string[] { "name" };
            BusinessEntity be = Servicio.Retrieve(EntityName.product.ToString(), idSubtipoProd, cs);
            if (be != null)
            {
                product subtipoProd = (product)be;
                nombre = subtipoProd.name;
            }
            return nombre;
        }

        public static IFormatProvider GetCulturaMoneda()
        {
            return System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR");
        }

        public Guid GetSubtipoProductoFromProductoBanco(Guid idProdBanco)
        {
            try
            {
                Guid idSubtipoProd;
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_productid" };
                BusinessEntity be = Servicio.Retrieve(EntityName.efk_producto_core.ToString(), idProdBanco, cs);
                if (be != null)
                {
                    efk_producto_core entProdBanco = (efk_producto_core)be;
                    idSubtipoProd = entProdBanco.efk_productid.Value;
                }
                else
                {
                    throw new Exception("Producto banco no encontrado.");
                }
                return idSubtipoProd;
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


        public Guid GetTipoProductoFromProductoBanco(Guid idProductoBanco)
        {
            try
            {
                Guid idTipoProd;
                Guid idSubtipoProd = GetSubtipoProductoFromProductoBanco(idProductoBanco);
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_tipo_productoid" };
                BusinessEntity be = Servicio.Retrieve(EntityName.product.ToString(), idSubtipoProd, cs);
                if (be != null)
                {
                    product entProd = (product)be;
                    idTipoProd = entProd.efk_tipo_productoid.Value;
                }
                else
                {
                    throw new Exception("Subtipo de producto no encontrado.");
                }
                return idTipoProd;
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


        public class Tipos
        {
            private CrmService Servicio;

            public Tipos(CredencialesCRM credenciales)
            {
                Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
            }

            public Guid GetIdTipoProducto(string nombre)
            {
                try
                {
                    ColumnSet cs = new ColumnSet();
                    cs.Attributes = new string[] { "efk_tipo_productoid" };

                    ConditionExpression condexp = new ConditionExpression();
                    condexp.AttributeName = "efk_nombre";
                    condexp.Operator = ConditionOperator.Equal;
                    condexp.Values = new string[] { nombre };

                    QueryExpression qryexp = new QueryExpression();
                    qryexp.EntityName = EntityName.efk_tipo_producto.ToString();
                    qryexp.ColumnSet = cs;
                    qryexp.Criteria = new FilterExpression();
                    qryexp.Criteria.Conditions = new ConditionExpression[] { condexp };

                    BusinessEntityCollection bec = Servicio.RetrieveMultiple(qryexp);

                    if (bec.BusinessEntities.Length > 0)
                    {
                        efk_tipo_producto entTipoProd = (efk_tipo_producto)bec.BusinessEntities[0];
                        return entTipoProd.efk_tipo_productoid.Value;
                    }
                    else
                        throw new Exception("No se ha encontrado tipo de producto con nombre " + nombre);
                }
                catch (System.Web.Services.Protocols.SoapException ex)
                {
                    throw new Exception(ex.Detail.InnerText);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public Entidades.TipoProducto GetNombreIcono(Guid idTipoProd)
            {
                Entidades.TipoProducto tipoProd = new TipoProducto();
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_icono", "efk_nombre" };
                BusinessEntity be = Servicio.Retrieve(EntityName.efk_tipo_producto.ToString(), idTipoProd, cs);
                if (be != null)
                {
                    efk_tipo_producto entTipoProd = (efk_tipo_producto)be;
                    tipoProd.Icono = entTipoProd.efk_icono;
                    tipoProd.Nombre = entTipoProd.efk_nombre;
                }
                return tipoProd;
            }
        }
    }
}
