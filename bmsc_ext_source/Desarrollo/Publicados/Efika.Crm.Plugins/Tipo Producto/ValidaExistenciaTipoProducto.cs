using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.Tipo_Producto
{
    /// <summary>
    /// Valida la existencia de un tipo de producto por portafolio
    /// </summary>
    public class ValidaExistenciaTipoProducto : IPlugin
    {
        private string FetchXmlObtenerTipoProductoPortafolio
        {
            get
            {
                return
                  @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                      <entity name=""efk_tipo_producto_portafolio"">
                        <attribute name=""efk_name"" />      
                        <attribute name=""efk_producto_segmento"" />
                        <attribute name=""efk_prioridad"" />
                        <attribute name=""efk_tipodeproducto"" />
                        <order attribute=""efk_name"" descending=""false"" />
                        <filter type=""and"">
                            <condition attribute=""statecode"" operator=""eq"" value=""0"" />
                            <condition attribute=""efk_producto_segmento"" operator=""eq"" uiname=""{0}"" uitype=""efk_portafolio_segmento"" value=""{1}"" />
                            <condition attribute=""efk_tipodeproducto"" operator=""eq"" uiname=""{2}"" uitype=""efk_tipo_producto"" value=""{3}"" />
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
            string _idportafolio = "";
            string _prioridad = "";
            string _idtipoproducto = "";

            try
            {
                Entity entidad = null; 
                // Recupero los datos de la entidad, para verificar los campos que no vayan a ser modificados y poder evaluarlos
                if (context.MessageName == "Update")
                    entidad = service.Retrieve("efk_tipo_producto_portafolio", context.PrimaryEntityId, new ColumnSet(new string[] { "efk_producto_segmento", "efk_tipodeproducto", "efk_prioridad" }));

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "efk_tipo_producto_portafolio")
                        return;

                    if (context.Depth > 1)
                        return;

                    //Validamos si algún campo ha sido modificado
                    if (entity.Attributes.Contains("efk_producto_segmento") || entity.Attributes.Contains("efk_prioridad") || entity.Attributes.Contains("efk_tipodeproducto")) //efk_segmento
                    {
                        EntityReference idproductosegmento = null;
                        if (entity.Attributes.Contains("efk_producto_segmento"))
                        {
                            idproductosegmento = (EntityReference)entity.Attributes["efk_producto_segmento"];
                            _idportafolio = idproductosegmento.Id.ToString();
                        }
                        else
                        {
                            if (entidad != null)
                            {
                                idproductosegmento = (EntityReference)entidad.Attributes["efk_producto_segmento"];
                                _idportafolio = idproductosegmento.Id.ToString();
                            }
                        }

                        EntityReference idtipoproducto = null;
                        if (entity.Attributes.Contains("efk_tipodeproducto"))
                        {
                            idtipoproducto = (EntityReference)entity.Attributes["efk_tipodeproducto"];
                            _idtipoproducto = idtipoproducto.Id.ToString();
                        }
                        else
                        {
                            if (entidad != null)
                            {
                                idtipoproducto = (EntityReference)entidad.Attributes["efk_tipodeproducto"];
                                _idtipoproducto = idtipoproducto.Id.ToString();
                            }
                        }

                        if (entity.Attributes.Contains("efk_prioridad"))
                            _prioridad = entity.Attributes["efk_prioridad"].ToString();
                        else
                        {
                            if (entidad != null) _prioridad = entidad.Attributes["efk_prioridad"].ToString(); ;
                        }

                        EntityCollection ec = service.RetrieveMultiple(new FetchExpression(string.Format(FetchXmlObtenerTipoProductoPortafolio, "", _idportafolio, "", _idtipoproducto, _prioridad)));

                        if (ec.Entities.Count > 0)
                        {
                            throw new InvalidPluginExecutionException("Ya existe un Tipo de Producto, para el portafolio, tipo y prioridad indicados.");
                        }
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
