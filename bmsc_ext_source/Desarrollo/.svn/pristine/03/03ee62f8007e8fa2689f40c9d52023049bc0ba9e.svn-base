using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.SubTipo_Producto
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidarExistenciaSubTipoProducto : IPlugin
    {
        private string FetchXmlObtenerSubTipoProductoOfertaValor
        {
            get
            {
                return
                  @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                      <entity name=""efk_subtipo_producto_oferta_valor"">
                        <attribute name=""efk_name"" />      
                        <attribute name=""efk_oferta_valorid"" />
                        <attribute name=""efk_prioridad"" />
                        <attribute name=""efk_tipo_producto_portafolio"" />
                        <attribute name=""efk_subtipo_producto_crm"" />
                        <order attribute=""efk_name"" descending=""false"" />
                        <filter type=""and"">
                            <condition attribute=""statecode"" operator=""eq"" value=""0"" />
                            <condition attribute=""efk_oferta_valorid"" operator=""eq"" uiname=""{0}"" uitype=""efk_oferta_valor_b"" value=""{1}"" />                            
                            <condition attribute=""efk_subtipo_producto_crm"" operator=""eq"" uiname=""{2}"" uitype=""product"" value=""{3}"" />
                            <condition attribute=""efk_prioridad"" operator=""eq"" value=""{4}"" />                          
                        </filter>
                    </entity>
                  </fetch>";
            }
        }

        private string FetchXmlObtenerSubTipoProductoTipoProducto
        {
            get
            {
                return
                  @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                      <entity name=""efk_subtipo_producto_oferta_valor"">
                        <attribute name=""efk_name"" />      
                        <attribute name=""efk_oferta_valorid"" />
                        <attribute name=""efk_prioridad"" />
                        <attribute name=""efk_tipo_producto_portafolio"" />
                        <attribute name=""efk_subtipo_producto_crm"" />
                        <order attribute=""efk_name"" descending=""false"" />
                        <filter type=""and"">
                            <condition attribute=""statecode"" operator=""eq"" value=""0"" />                            
                            <condition attribute=""efk_tipo_producto_portafolio"" operator=""eq"" uiname=""{0}"" uitype=""efk_tipo_producto_portafolio"" value=""{1}"" />
                            <condition attribute=""efk_subtipo_producto_crm"" operator=""eq"" uiname=""{2}"" uitype=""product"" value=""{3}"" />
                            <condition attribute=""efk_prioridad"" operator=""eq"" value=""{4}"" />                          
                        </filter>
                    </entity>
                  </fetch>";
            }
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(null);

            IOrganizationService iServices = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(context.UserId));

            // Get a reference to the tracing service.
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            string _idofertavalor = "";
            string _idsubtipoproducto = "";
            string _prioridad = "";
            string _idtipoproducto = "";
            bool bandera = false;

            try
            {
                Entity entidad = null;
                // Recupero los datos de la entidad, para verificar los campos que no vayan a ser modificados y poder evaluarlos
                if (context.MessageName == "Update")
                    entidad = service.Retrieve("efk_subtipo_producto_oferta_valor", context.PrimaryEntityId, 
                        new ColumnSet(new string[] { "efk_oferta_valorid", "efk_subtipo_producto_crm", "efk_prioridad", "efk_tipo_producto_portafolio" }));

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "efk_subtipo_producto_oferta_valor")
                        return;

                    if (context.Depth > 1)
                        return;

                    //Validamos si algún campo ha sido modificado
                    if (entity.Attributes.Contains("efk_oferta_valorid") || entity.Attributes.Contains("efk_prioridad") 
                        || entity.Attributes.Contains("efk_subtipo_producto_crm") || entity.Attributes.Contains("efk_tipo_producto_portafolio")) 
                    {
                        EntityReference idofertavalor = null;
                        if (entity.Attributes.Contains("efk_oferta_valorid"))
                        {
                            idofertavalor = (EntityReference)entity.Attributes["efk_oferta_valorid"];
                            _idofertavalor = idofertavalor.Id.ToString();
                            bandera = true;
                        }
                        else
                        {
                            if (entidad != null)
                            {
                                idofertavalor = (EntityReference)entidad.Attributes["efk_oferta_valorid"];
                                _idofertavalor = idofertavalor.Id.ToString();
                            }
                        }

                        EntityReference idsubtipoproducto = null;
                        if (entity.Attributes.Contains("efk_subtipo_producto_crm"))
                        {
                            idsubtipoproducto = (EntityReference)entity.Attributes["efk_subtipo_producto_crm"];
                            _idsubtipoproducto = idsubtipoproducto.Id.ToString();
                        }
                        else
                        {
                            if (entidad != null)
                            {
                                idsubtipoproducto = (EntityReference)entidad.Attributes["efk_subtipo_producto_crm"];
                                _idsubtipoproducto = idsubtipoproducto.Id.ToString();
                            }
                        }

                        EntityReference idtipoproducto = null;
                        if (entity.Attributes.Contains("efk_tipo_producto_portafolio"))
                        {
                            idtipoproducto = (EntityReference)entity.Attributes["efk_tipo_producto_portafolio"];
                            _idtipoproducto = idtipoproducto.Id.ToString();
                            bandera = false;
                        }
                        else
                        {
                            if (entidad != null)
                            {
                                idtipoproducto = (EntityReference)entidad.Attributes["efk_tipo_producto_portafolio"];
                                _idtipoproducto = idtipoproducto.Id.ToString();
                            }
                        }

                        if (entity.Attributes.Contains("efk_prioridad"))
                            _prioridad = entity.Attributes["efk_prioridad"].ToString();
                        else
                        {
                            if (entidad != null) _prioridad = entidad.Attributes["efk_prioridad"].ToString();
                        }

                    }
                }
                if (bandera)
                {
                    EntityCollection ec = service.RetrieveMultiple(new FetchExpression(string.Format(FetchXmlObtenerSubTipoProductoOfertaValor, "", 
                        _idofertavalor, "", _idsubtipoproducto, _prioridad)));
                    if (ec.Entities.Count > 0)
                    {
                        throw new InvalidPluginExecutionException("Ya existe un Subtipo de Producto, para la oferta y prioridad indicados.");
                    }
                }
                else
                {
                    EntityCollection ec1 = service.RetrieveMultiple(new FetchExpression(string.Format(
                        FetchXmlObtenerSubTipoProductoTipoProducto, "", _idtipoproducto, "", _idsubtipoproducto, _prioridad)));
                    if (ec1.Entities.Count > 0)
                    {
                        throw new InvalidPluginExecutionException("Ya existe un Subtipo de Producto, para el tipo y prioridad indicados.");
                    }
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace(this.GetType().ToString() + "_PreCreate: {0} ", ex.ToString());
                throw;
            }
        }
    }
}
