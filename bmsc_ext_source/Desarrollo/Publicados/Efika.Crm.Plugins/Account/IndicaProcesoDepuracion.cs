using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.Account
{
    public class IndicaProcesoDepuracion : IPlugin
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
                if (context.GetType().GetProperty("CallerOrigin").GetValue(context, null).GetType().Name != "ApplicationOrigin")
                    return;
                
                if (context.Depth > 1)
                    return;

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity && context.Depth == 1)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];

                    Entity updateEntity = new Entity(entity.LogicalName)
                    {
                        Id = entity.Id
                    };

                    if (updateEntity.LogicalName != "account")
                        return;

                    if (context.PostEntityImages.Contains("post_image_enti"))
                    {
                        if (context.PostEntityImages["post_image_enti"].Attributes.ContainsKey("efk_cliente_mis"))
                        {
                            if (!Boolean.Parse(context.PostEntityImages["post_image_enti"].Attributes["efk_cliente_mis"].ToString()))
                            {

                                if (updateEntity.Attributes.Contains("efk_indicador_propceso_depuracion"))
                                {
                                    updateEntity.Attributes["efk_indicador_propceso_depuracion"] = true;
                                }
                                else
                                {
                                    updateEntity.Attributes.Add("efk_indicador_propceso_depuracion", true);
                                }

                                service.Update(updateEntity);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                tracingService.Trace(this.GetType().ToString() + "_PostCreate: {0} ", ex.ToString());
                throw;
            }
        }
    }
}

