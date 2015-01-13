using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
//*********************************
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;

namespace Efika.Crm.Plugins.Account
{
    public class ActualizaRatificados : IPlugin
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

                if ((callerorigin.GetType().Name == "ApplicationOrigin") && context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];                    

                    if (entity.LogicalName != "account")
                        return;

                    if (context.Depth > 1)
                        return;

                    bool graba = false;                    

                    if (context.PostEntityImages.Contains("post_image_enti"))
                    {
                        if (context.PostEntityImages["post_image_enti"].Attributes.ContainsKey("efk_cliente_mis"))
                        {
                            if (Boolean.Parse(context.PostEntityImages["post_image_enti"].Attributes["efk_cliente_mis"].ToString()))
                            {

                                if (entity.Attributes.Contains("efk_ingresos_ov"))
                                {
                                    graba = true;
                                    entity.Attributes.Add("efk_ingresos_ratificado", false);
                                }

                                if (entity.Attributes.Contains("efk_estado_civil_ov"))
                                {
                                    graba = true;
                                    entity.Attributes.Add("efk_estado_civil_ratificado", false);
                                }

                                if (entity.Attributes.Contains("efk_nrodehijos_ov"))
                                {
                                    graba = true;
                                    entity.Attributes.Add("efk_nro_hijos_ratificado", false);
                                }

                                if (entity.Attributes.Contains("efk_fechadenacimiento_ov"))
                                {
                                    graba = true;
                                    entity.Attributes.Add("efk_fecha_nacimiento_ratificado", false);
                                }
                            }
                        }
                    }
              
                   if (graba) service.Update(entity);
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace(this.GetType().ToString() + "_PostUpdate: {0} ", ex.ToString());
                throw;
            }
        }
    }
}
