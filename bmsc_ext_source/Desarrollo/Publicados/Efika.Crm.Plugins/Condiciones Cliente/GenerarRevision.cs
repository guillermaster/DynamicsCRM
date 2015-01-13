using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.CondicionesCliente
{
    public class GenerarRevision : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            IOrganizationService iServices = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(context.UserId));

            // Get a reference to the tracing service.
             ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

             try
             {
                 object callerorigin =
                     context.GetType().GetProperty("CallerOrigin").GetValue(context, null);
                 if ((callerorigin.GetType().Name == "ApplicationOrigin"))
                 {

                     if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity && context.Depth <= 1)
                     {
                         Entity entity = (Entity)context.InputParameters["Target"];

                         if (entity.LogicalName != "efk_condicion_cliente")
                             return;

                         Guid efk_condicion_clienteid = context.PrimaryEntityId;

                         //Preguntamos por la Pre-Image

                         if (context.PreEntityImages.Contains("efk_condicion_cliente") && context.PostEntityImages.Contains("efk_condicion_cliente"))
                         {
                             Entity preTarget = (Entity)context.PreEntityImages["efk_condicion_cliente"];
                             Entity postTarget = (Entity)context.PostEntityImages["efk_condicion_cliente"];

                             if (preTarget.Attributes.Contains("efk_fecha_proximo_seguimiento"))
                             {
                                 DateTime fechaProximoSeguimiento = (DateTime)preTarget.Attributes["efk_fecha_proximo_seguimiento"];
                                 int numeroRevisionActual = 0;

                                 if (preTarget.Attributes.Contains("efk_numero_revisiones") && preTarget.Attributes["efk_numero_revisiones"] != null)
                                 {
                                     numeroRevisionActual = (int)preTarget.Attributes["efk_numero_revisiones"];
                                 }

                                 if (fechaProximoSeguimiento < DateTime.Now)
                                 {
                                     if (postTarget.Attributes.Contains("efk_resultado"))
                                     {
                                         OptionSetValue resultado = (OptionSetValue)postTarget.Attributes["efk_resultado"];

                                         int nroactual = numeroRevisionActual + 1;
                                         Entity cliente = new Entity("efk_revision_condiciones_cliente");
                                         cliente["efk_nombre"] = "Revisión No." + nroactual.ToString();
                                         if (postTarget.Attributes.Contains("efk_detalle_resultado") && postTarget.Attributes["efk_detalle_resultado"] != null)
                                         {
                                             String detalleresultado = (String)postTarget.Attributes["efk_detalle_resultado"];
                                             cliente["efk_detalle_resultado"] = detalleresultado;
                                         }
                                         cliente["efk_fecha_seguimiento"] = DateTime.Now;
                                         cliente["efk_condicion_clienteid"] = new EntityReference("efk_condicion_cliente", efk_condicion_clienteid);
                                         cliente["efk_resultado"] = resultado;
                                         service.Create(cliente);

                                         Entity condicionCliente = new Entity("efk_condicion_cliente");
                                         condicionCliente.Attributes["efk_condicion_clienteid"] = efk_condicion_clienteid;
                                         condicionCliente.Attributes["efk_numero_revisiones"] = numeroRevisionActual + 1;

                                         service.Update(condicionCliente);
                                     }
                                 }
                             }

                         }

                     }
                 }
             }
             catch (Exception ex)
             {
                 tracingService.Trace(this.GetType().ToString() + "_PreCreate: {0} ", ex.ToString());
                 throw new InvalidPluginExecutionException(ex.Message + " nn");
             }
        }
    }
}
