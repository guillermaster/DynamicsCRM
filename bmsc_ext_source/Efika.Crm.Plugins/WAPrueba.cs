using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ServiceModel;
using System.Collections;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;

namespace Efika.Crm.Plugins
{
    public sealed class WAPrueba: CodeActivity
    {


        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext workflowContext = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory ServiceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService servicio = ServiceFactory.CreateOrganizationService(workflowContext.UserId);
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

        }
    }
}
