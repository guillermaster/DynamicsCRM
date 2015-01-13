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
    public sealed partial class ValidaActividades : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            IOrganizationServiceFactory factory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService servicio = factory.CreateOrganizationService(null);
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();           

            try
            {
              
                    Guid accountid = this.Target.Get(executionContext).Id; 

                    QueryExpression qe = new QueryExpression("activitypointer");
                    qe.ColumnSet.AddColumn("activityid");
                    qe.ColumnSet.AddColumn("regardingobjectid");
                    qe.ColumnSet.AddColumn("statecode");
                    qe.Criteria = new FilterExpression(LogicalOperator.And);
                    qe.Criteria.AddCondition(new ConditionExpression("regardingobjectid", ConditionOperator.Equal, accountid.ToString()));
                    qe.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
                    EntityCollection queryCondiciones = servicio.RetrieveMultiple(qe);

                    QueryExpression qe1 = new QueryExpression("campaignactivity");
                    qe1.ColumnSet.AddColumn("activityid");
                    qe1.ColumnSet.AddColumn("regardingobjectid");
                    qe.ColumnSet.AddColumn("statecode");
                    qe1.Criteria = new FilterExpression(LogicalOperator.And);
                    qe1.Criteria.AddCondition(new ConditionExpression("regardingobjectid", ConditionOperator.Equal, accountid.ToString()));
                    qe1.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
                    EntityCollection queryCondiciones1 = servicio.RetrieveMultiple(qe1);

                    QueryExpression qe2 = new QueryExpression("serviceappointment");
                    qe2.ColumnSet.AddColumn("activityid");
                    qe2.ColumnSet.AddColumn("regardingobjectid");
                    qe.ColumnSet.AddColumn("statecode");
                    qe2.Criteria = new FilterExpression(LogicalOperator.And);
                    qe2.Criteria.AddCondition(new ConditionExpression("regardingobjectid", ConditionOperator.Equal, accountid.ToString()));
                    qe2.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
                    EntityCollection queryCondiciones2 = servicio.RetrieveMultiple(qe2);

                    QueryExpression qe3 = new QueryExpression("opportunity");
                    qe3.ColumnSet.AddColumn("opportunityid");
                    qe3.ColumnSet.AddColumn("customerid");
                    qe.ColumnSet.AddColumn("statecode");
                    qe3.Criteria = new FilterExpression(LogicalOperator.And);
                    qe3.Criteria.AddCondition(new ConditionExpression("customerid", ConditionOperator.Equal, accountid.ToString()));
                    qe3.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
                    EntityCollection queryCondiciones3 = servicio.RetrieveMultiple(qe3);

                    int consulta = queryCondiciones3.Entities.Count + queryCondiciones2.Entities.Count + queryCondiciones1.Entities.Count + queryCondiciones.Entities.Count;
                    bool resultado = false;

                    if (consulta > 0) resultado = true;

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

