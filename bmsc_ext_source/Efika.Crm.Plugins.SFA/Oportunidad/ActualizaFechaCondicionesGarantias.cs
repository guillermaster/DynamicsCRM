using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;

namespace Efika.Crm.Plugins.SFA.Oportunidad
{
    public class ActualizaFechaCondicionesGarantias : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            IOrganizationService iServices = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(context.UserId));
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            try
            {
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity && context.Depth == 1)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "opportunity") return;

                    if (
                            entity.Attributes.Contains("efk_nrosolicitud") ||
                            entity.Attributes.Contains("efk_numero_oferta") ||
                            entity.Attributes.Contains("efk_numero_cuotas") ||
                            entity.Attributes.Contains("efk_moneda_operacion") ||
                            entity.Attributes.Contains("efk_spread_fijo") ||
                            entity.Attributes.Contains("efk_tasa_fija") ||
                            entity.Attributes.Contains("efk_tre_semana") ||
                            entity.Attributes.Contains("efk_tasa_variable_apartirde") ||
                            entity.Attributes.Contains("efk_tipo_poliza") ||
                            entity.Attributes.Contains("efk_con_seguro_cesantia") ||
                            entity.Attributes.Contains("efk_con_seguro_desgravamen") ||
                            entity.Attributes.Contains("efk_monto_maximo") ||
                            entity.Attributes.Contains("efk_monto_solicitado")
                        )
                    {
                        Entity oportunidad = new Entity("opportunity");
                        oportunidad.Id = context.PrimaryEntityId;
                        oportunidad.Attributes.Add("efk_fecha_ultima_actualizacion_cond_garantias", DateTime.Now);

                        iServices.Update(oportunidad);
                        return;
                    }
                }
            }
            catch (System.ServiceModel.FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("Oportunidad_PostUpdate plug-in: " + ex.Message.ToString(), ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("Oportunidad_PostUpdate plug-in: {0}", ex.ToString());
                throw;               
            }
        }
    }
}
