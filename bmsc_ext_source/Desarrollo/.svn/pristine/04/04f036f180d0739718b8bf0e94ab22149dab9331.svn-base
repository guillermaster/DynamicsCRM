using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Metadata;

namespace Efika.Crm.Plugins.SFA.Oportunidad
{
    public class ValidarTareaProcesoVenta : IPlugin
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

            try
            {
                if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
                {
                    //Obtiene el target business entity para los parametros de entrada
                    entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "task")
                        return;

                    //Validamos que no se pueda modificar la fecha de vencimiento si la tarea no pertenece al proceso de venta.
                    if (entity.Attributes.Contains("scheduledend"))
                    {
                        //Obtenemos el valor de efk_pertenece_proceso_venta
                        Entity tarea = servicio.Retrieve("task", entity.Id, new ColumnSet("activityid", "efk_pertenece_proceso_venta"));

                        if (tarea.Attributes.Contains("efk_pertenece_proceso_venta") && tarea.Attributes["efk_pertenece_proceso_venta"] != null)
                        {
                            if ((bool)tarea.Attributes["efk_pertenece_proceso_venta"])
                            {
                                throw new InvalidPluginExecutionException("No se puede modificar la fecha de vencimiento de una tarea que pertenece a un proceso de venta.");
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                tracingService.Trace(this.GetType().ToString() + "_PreUpdate: {0} ", ex.ToString());
                throw;
            }
        }
    }
}
