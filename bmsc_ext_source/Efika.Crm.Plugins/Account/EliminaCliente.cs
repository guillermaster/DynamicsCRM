using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using System.Collections;
using System.Reflection;

namespace Efika.Crm.Plugins.Account
{
    public sealed partial class EliminaCliente : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            IOrganizationServiceFactory factory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService servicio = factory.CreateOrganizationService(null);
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();           

            try
            {
                 Guid accountid = this.Target.Get(executionContext).Id; 

                 servicio.Delete("account", accountid);
                 bool resultado = true;
                 myBoolean.Set(executionContext, resultado);

            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("account: " + ex.Message.ToString(), ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Input("Target Entity")]
        [ReferenceTarget("account")]
        [RequiredArgument]
        public InArgument<EntityReference> Target { get; set; }

        [Output("My Boolean Output")]
        public OutArgument<bool> myBoolean { get; set; }

    }
}
