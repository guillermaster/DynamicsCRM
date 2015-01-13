using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.Account
{
    public class ValidarReasignacion : IPlugin
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

                 if ((callerorigin.GetType().Name == "ApplicationOrigin") && context.InputParameters.Contains("Target") && context.InputParameters["Target"] is EntityReference)
                 {
                     EntityReference entity = (EntityReference)context.InputParameters["Target"];

                     if (entity.LogicalName != "account")
                         return;

                     if (context.Depth > 1)
                         return;
                     
                     if (context.PreEntityImages.Contains("post_image_enti"))
                     {
                         if (context.PreEntityImages["post_image_enti"].Attributes.ContainsKey("efk_cliente_mis"))
                         {
                             if (Boolean.Parse(context.PreEntityImages["post_image_enti"].Attributes["efk_cliente_mis"].ToString()))
                             {
                                 throw new InvalidPluginExecutionException("No puede reasignar un cliente del banco, debe reasignarlo en FISA.");
                               
                             }
                         }
                     }

                 }
                  
             }
             catch (Exception ex)
             {
                 tracingService.Trace("Exception: {0}", ex.ToString());
                 throw ex;
             }
         }

    }
}
