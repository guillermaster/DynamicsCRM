using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Metadata;


namespace Efika.Crm.Plugins.SFA.ProductoOportunidad
{
    public class SetearTipoProductoOportunidad : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            IOrganizationService servicio = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(context.UserId));

            // Get a reference to the tracing service.
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            Entity entity = null;

            #region Verify execution context
            if (context.GetType().GetProperty("CallerOrigin").GetValue(context, null).GetType().Name != "ApplicationOrigin")
                return;

            //Chequea si los parametros de entrada contienen un target
            //y crea una operacion y el target es de tipo DynamicEntity.

            if (context.InputParameters.Contains("Target") &&
               context.InputParameters["Target"] is Entity && context.Depth == 1)
            {
                //Obtiene el target business entity para los parametros de entrada
                entity = (Entity)context.InputParameters["Target"];

                //Verifica que la entidad representa una cuenta
                EntityReference Producto = null;
                EntityReference Oportunidad = null;
                EntityReference Familia = null;
                Guid idProducto = Guid.Empty;
                Guid idOportunidad = Guid.Empty;
                Guid idFamilia = Guid.Empty;

                Entity entidad = service.Retrieve("opportunityproduct", context.PrimaryEntityId, 
                    new ColumnSet(new string[] { "productid", "opportunityid", "efk_familia_productosid", 
                        "priceperunit", "efk_plazo", "efk_tasa_fija", "efk_tasa_variable" }));

                if (entidad != null)
                {
                    Producto = (EntityReference)entidad.Attributes["productid"];
                    idProducto = Producto.Id;

                    Oportunidad = (EntityReference)entidad.Attributes["opportunityid"];
                    idOportunidad = Oportunidad.Id;

                    Familia = (EntityReference)entidad.Attributes["efk_familia_productosid"];
                    idFamilia = Familia.Id;

                }

                try
                {
                    string familiaproducto = string.Empty;
                    int tipofamiliaproducto = 0;
                    Entity oportunidad_ = null;
                    oportunidad_ = new Entity("opportunity");
                    oportunidad_.Id = idOportunidad;


                    #region Consulta Familia Producto
                    Entity entfam = service.Retrieve("efk_familia_productos", idFamilia, new ColumnSet(new string[] { "efk_nombre", "efk_familia_productosid" }));
                    if (entfam != null)
                    {
                        familiaproducto = entfam.Attributes["efk_nombre"].ToString();
                    }


                    if (familiaproducto == "PASIVOS")
                        tipofamiliaproducto = 221220001;
                    else
                        if (familiaproducto == "SERVICIOS" || familiaproducto == "SERVICIOS COMEX" || familiaproducto == "SEGUROS" || familiaproducto == "OTROS" || familiaproducto == "CANALES")
                            tipofamiliaproducto = 221220002;
                        else
                            tipofamiliaproducto = 221220000;               

                    oportunidad_.Id = idOportunidad;
                    oportunidad_.Attributes.Add("efk_tipo_familia_producto", tipofamiliaproducto);

                    if (tipofamiliaproducto == 221220000)
                    {
                        if (entidad.Attributes["priceperunit"] != null)
                            oportunidad_.Attributes.Add("efk_monto_solicitado", entidad.Attributes["priceperunit"]);

                        if (entidad.Attributes["efk_plazo"] != null)
                            oportunidad_.Attributes.Add("efk_numero_cuotas", entidad.Attributes["efk_plazo"]);

                        if (entidad.Attributes["efk_tasa_fija"] != null)
                            oportunidad_.Attributes.Add("efk_tasa_fija", entidad.Attributes["efk_tasa_fija"]);

                        if (entidad.Attributes["efk_tasa_variable"] != null)
                            oportunidad_.Attributes.Add("efk_tre_semana", entidad.Attributes["efk_tasa_variable"]);
                    }

                    servicio.Update(oportunidad_);

                    #endregion

                }
                catch (System.Exception ex)
                {
                    tracingService.Trace(this.GetType().ToString() + "_PostCreate: {0} ", ex.ToString());
                    throw new InvalidPluginExecutionException(ex.ToString());
                }
            }
            else
            {
                return;
            }
            #endregion
        }

    }
}
