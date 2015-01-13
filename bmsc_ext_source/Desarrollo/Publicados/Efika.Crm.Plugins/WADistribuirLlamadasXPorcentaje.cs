using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ServiceModel;
using System.Collections;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Workflow;

namespace Efika.Crm.Plugins
{
    public sealed class WADistribuirLlamadasXPorcentaje : CodeActivity
    {

        #region Input Properties
       

        #endregion

        private string fetchProbabilidad =
            @"<fetch mapping=""logical"">
  <entity name=""account"">
    <attribute name=""name"" />
    <attribute name=""accountid"" />
    <attribute name=""efk_probabilidad_fuga"" />
    <filter>
      <condition attribute=""efk_probabilidad_fuga"" operator=""gt"" value=""0"" />
    </filter>
    <link-entity name=""efk_producto_pasivo""  from=""efk_cliente_juridico_id"" to=""accountid"" alias=""b"" link-type=""inner"">
      <attribute name=""efk_saldo_mes_anterior"" />
      <attribute name=""efk_saldo_disponible"" />
      <attribute name=""efk_cliente_juridico_id"" />
      <filter>
        <condition attribute=""efk_saldo_disponible"" operator=""gt"" value=""0"" />
      </filter>
    </link-entity>
  </entity>
</fetch>";

         private string fetchValores =
            @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
 <entity name=""efk_valores_oferta"">
  <attribute name=""efk_valores_ofertaid"" /> 
  <attribute name=""efk_name"" /> 
  <attribute name=""efk_valor"" /> 
  <order attribute=""efk_name"" descending=""false"" /> 
 <filter type=""and"">
  <condition attribute=""efk_name"" operator=""eq"" value=""PORCENTAJE"" /> 
  </filter>
  </entity>
  </fetch>";


//        private string fetchProbabilidad =
//            @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
//             <entity name=""account"">
//              <attribute name=""name"" /> 
//              <attribute name=""accountid"" /> 
//              <attribute name=""efk_probabilidad_fuga"" /> 
//              <order attribute=""name"" descending=""false"" /> 
//              </entity>
//              </fetch>";

//        private string fetchSaldos =
//       @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
// <entity name=""efk_producto_pasivo"">
//  <attribute name=""efk_cliente_juridico_id"" /> 
//  <attribute name=""efk_producto_pasivoid"" /> 
//  <attribute name=""efk_saldo_mes_anterior"" /> 
//  <attribute name=""efk_saldo_disponible"" /> 
//  <order attribute=""efk_cliente_juridico_id"" descending=""false"" /> 
//  </entity>
//  </fetch>";

//        private string fetchValores =
//            @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
// <entity name=""efk_valores_oferta"">
//  <attribute name=""efk_valores_ofertaid"" /> 
//  <attribute name=""efk_name"" /> 
//  <attribute name=""efk_valor"" /> 
//  <order attribute=""efk_name"" descending=""false"" /> 
// <filter type=""and"">
//  <condition attribute=""efk_name"" operator=""eq"" value=""PORCENTAJE"" /> 
//  </filter>
//  </entity>
//  </fetch>";


        protected override void Execute(CodeActivityContext executionContext)
        {

            IWorkflowContext workflowContext = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory ServiceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService servicio = ServiceFactory.CreateOrganizationService(workflowContext.UserId);
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            EntityCollection entities = servicio.RetrieveMultiple(new FetchExpression(fetchProbabilidad));
            EntityCollection entitiesparametro = servicio.RetrieveMultiple(new FetchExpression(fetchValores));

            //EntityCollection entities = servicio.RetrieveMultiple(new FetchExpression(fetchProbabilidad));
            //EntityCollection entitiesvalores = servicio.RetrieveMultiple(new FetchExpression(fetchSaldos));
            //EntityCollection entitiesparametro = servicio.RetrieveMultiple(new FetchExpression(fetchValores));

            decimal valProb = 0;
            decimal valDis = 0;
            decimal valMes = 0;
            decimal ValPar = 0;
            string campo = "";

            foreach (Entity evalores in entities.Entities)
            {
                foreach (Entity eparametro in entitiesparametro.Entities)
                {
                if (eparametro.Attributes.Contains("efk_valor"))
                {
                    ValPar = ((decimal)eparametro.Attributes["efk_valor"]);

                }
                if (evalores.Attributes.Contains("efk_probabilidad_fuga"))
                {
                    valProb = ((decimal)evalores.Attributes["efk_probabilidad_fuga"]);

                }
                if (evalores.Attributes.Contains("efk_saldo_mes_anterior"))
                {
                    valMes = ((decimal)evalores.Attributes["efk_saldo_mes_anterior"]);

                }
                if (evalores.Attributes.Contains("efk_saldo_disponible"))
                {
                    valDis = ((decimal)evalores.Attributes["efk_saldo_disponible"]);

                }
                decimal indice = ((valDis - valMes) * valProb);

               
                
                if (indice >= ValPar)
                {
                if (evalores.Attributes.Contains("accountid"))
                {
                    campo = "LLAMADA PORCENTAJE ";
                    Guid identificador = (Guid)evalores.Attributes["accountid"];
                    Entity newTask = new Entity("phonecall");
                    //newTask["regardingobjectid"] = new EntityReference("account", ((EntityReference)evalores.Attributes["accountid"]).Id);// workflowContext.PrimaryEntityId);
                    newTask["regardingobjectid"] = new EntityReference("account", identificador);// workflowContext.PrimaryEntityId);
                    ColumnSet cols = new ColumnSet();
                    cols.AddColumns("ownerid", "telephone1");
                    //Entity ent = servicio.Retrieve("account", ((EntityReference)evalores.Attributes["accountid"]).Id, cols);
                    Entity ent = servicio.Retrieve("account", identificador, cols);

                    if (ent.Attributes.Contains("telephone1"))
                    {
                        newTask["phonenumber"] = ent.Attributes["telephone1"];
                    }


                    newTask["ownerid"] = (EntityReference)ent.Attributes["ownerid"];
                    newTask["subject"] = campo;//newSubject;
                    Entity newActivity = new Entity("activityparty");
                    newActivity["partyid"] = (EntityReference)ent.Attributes["ownerid"];
                    newTask["from"] = new EntityCollection(new[] { newActivity });
                    Entity newActivity1 = new Entity("activityparty");
                    //newActivity1["partyid"] = new EntityReference("account", ((EntityReference)evalores.Attributes["efk_cliente_juridico_id"]).Id);
                    newActivity1["partyid"] = new EntityReference("account", identificador);
                    newTask["to"] = new EntityCollection(new[] { newActivity1 });
                    Guid taskId = servicio.Create(newTask);
                    campo = "";
                }

                }
                else
                {
                    if (((valProb * 100) >= 50) && (valMes > 9000))
                    {

                        Entity newTask = new Entity("phonecall");
                        newTask["regardingobjectid"] = new EntityReference("account", ((EntityReference)evalores.Attributes["accountid"]).Id);// workflowContext.PrimaryEntityId);
                        ColumnSet cols = new ColumnSet();
                        cols.AddColumns("ownerid", "telephone1");
                        Entity ent = servicio.Retrieve("account", ((EntityReference)evalores.Attributes["accountid"]).Id, cols);

                        if (ent.Attributes.Contains("telephone1"))
                        {
                            newTask["phonenumber"] = ent.Attributes["telephone1"];
                        }

                        newTask["ownerid"] = (EntityReference)ent.Attributes["ownerid"];
                        newTask["subject"] = campo;//newSubject;
                        Entity newActivity = new Entity("activityparty");
                        newActivity["partyid"] = (EntityReference)ent.Attributes["ownerid"];
                        newTask["from"] = new EntityCollection(new[] { newActivity });
                        Entity newActivity1 = new Entity("activityparty");
                        newActivity1["partyid"] = new EntityReference("account", ((EntityReference)evalores.Attributes["accountid"]).Id);
                        newTask["to"] = new EntityCollection(new[] { newActivity1 });
                        Guid taskId = servicio.Create(newTask);
                        campo = "";
                    }

                    if (((valProb * 100) >= 75) && ((valMes >= 1800) && (valMes <= 20000)))
                    {

                        Entity newTask = new Entity("phonecall");
                        newTask["regardingobjectid"] = new EntityReference("account", ((EntityReference)evalores.Attributes["accountid"]).Id);// workflowContext.PrimaryEntityId);
                        ColumnSet cols = new ColumnSet();
                        cols.AddColumns("ownerid", "telephone1");
                        Entity ent = servicio.Retrieve("account", ((EntityReference)evalores.Attributes["accountid"]).Id, cols);

                        if (ent.Attributes.Contains("telephone1"))
                        {
                            newTask["phonenumber"] = ent.Attributes["telephone1"];
                        }

                        newTask["ownerid"] = (EntityReference)ent.Attributes["ownerid"];
                        newTask["subject"] = campo;//newSubject;
                        Entity newActivity = new Entity("activityparty");
                        newActivity["partyid"] = (EntityReference)ent.Attributes["ownerid"];
                        newTask["from"] = new EntityCollection(new[] { newActivity });
                        Entity newActivity1 = new Entity("activityparty");
                        newActivity1["partyid"] = new EntityReference("account", ((EntityReference)evalores.Attributes["accountid"]).Id);
                        newTask["to"] = new EntityCollection(new[] { newActivity1 });
                        Guid taskId = servicio.Create(newTask);
                        campo = "";
                    }
                }
                }
            }

            //foreach (Entity evalores in entities.Entities)
            //{
            //    foreach (Entity e in entitiesvalores.Entities)
            //    {
            //        foreach (Entity eparametro in entitiesparametro.Entities)
            //        {
            //            if (e.Attributes["efk_cliente_juridico_id"].ToString() == evalores.Attributes["accountid"].ToString())
            //            {
                            
            //                if (eparametro.Attributes.Contains("efk_valor"))
            //                {
            //                    ValPar = ((decimal)eparametro.Attributes["efk_valor"]);

            //                }
            //                if (evalores.Attributes.Contains("efk_probabilidad_fuga"))
            //                {
            //                    valProb = ((decimal)evalores.Attributes["efk_probabilidad_fuga"]);

            //                }
            //                if (e.Attributes.Contains("efk_saldo_mes_anterior"))
            //                {
            //                    valMes = ((decimal)e.Attributes["efk_saldo_mes_anterior"]);

            //                }
            //                if (e.Attributes.Contains("efk_saldo_disponible"))
            //                {
            //                    valDis = ((decimal)e.Attributes["efk_saldo_disponible"]);

            //                }
            //                decimal indice = ((valDis - valMes) * valProb);

            //                if (indice >= ValPar)
            //                {
                                
            //                    Entity newTask = new Entity("phonecall");
            //                    newTask["regardingobjectid"] = new EntityReference("account", ((EntityReference)e.Attributes["efk_cliente_juridico_id"]).Id);// workflowContext.PrimaryEntityId);
            //                    ColumnSet cols = new ColumnSet();
            //                    cols.AddColumns("ownerid", "telephone1");
            //                    Entity ent = servicio.Retrieve("account", ((EntityReference)e.Attributes["efk_cliente_juridico_id"]).Id, cols);

            //                    if (ent.Attributes.Contains("telephone1"))
            //                    {
            //                        newTask["phonenumber"] = ent.Attributes["telephone1"];
            //                    }

            //                    newTask["ownerid"] = (EntityReference)ent.Attributes["ownerid"];
            //                    newTask["subject"] = campo;//newSubject;
            //                    Entity newActivity = new Entity("activityparty");
            //                    newActivity["partyid"] = (EntityReference)ent.Attributes["ownerid"];
            //                    newTask["from"] = new EntityCollection(new[] { newActivity });
            //                    Entity newActivity1 = new Entity("activityparty");
            //                    newActivity1["partyid"] = new EntityReference("account", ((EntityReference)e.Attributes["efk_cliente_juridico_id"]).Id);
            //                    newTask["to"] = new EntityCollection(new[] { newActivity1 });
            //                    Guid taskId = servicio.Create(newTask);
            //                    campo = "";
            //                }
            //                else
            //                { 
            //                    if (((valProb*100) >= 50)&&(valMes>9000))
            //                    {
                                    
            //                        Entity newTask = new Entity("phonecall");
            //                        newTask["regardingobjectid"] = new EntityReference("account", ((EntityReference)e.Attributes["efk_cliente_juridico_id"]).Id);// workflowContext.PrimaryEntityId);
            //                        ColumnSet cols = new ColumnSet();
            //                        cols.AddColumns("ownerid", "telephone1");
            //                        Entity ent = servicio.Retrieve("account", ((EntityReference)e.Attributes["efk_cliente_juridico_id"]).Id, cols);

            //                        if (ent.Attributes.Contains("telephone1"))
            //                        {
            //                            newTask["phonenumber"] = ent.Attributes["telephone1"];
            //                        }

            //                        newTask["ownerid"] = (EntityReference)ent.Attributes["ownerid"];
            //                        newTask["subject"] = campo;//newSubject;
            //                        Entity newActivity = new Entity("activityparty");
            //                        newActivity["partyid"] = (EntityReference)ent.Attributes["ownerid"];
            //                        newTask["from"] = new EntityCollection(new[] { newActivity });
            //                        Entity newActivity1 = new Entity("activityparty");
            //                        newActivity1["partyid"] = new EntityReference("account", ((EntityReference)e.Attributes["efk_cliente_juridico_id"]).Id);
            //                        newTask["to"] = new EntityCollection(new[] { newActivity1 });
            //                        Guid taskId = servicio.Create(newTask);
            //                        campo = "";
            //                    }

            //                    if (((valProb * 100) >= 75) && ((valMes >= 1800)&&(valMes <= 20000)))
            //                    {

            //                        Entity newTask = new Entity("phonecall");
            //                        newTask["regardingobjectid"] = new EntityReference("account", ((EntityReference)e.Attributes["efk_cliente_juridico_id"]).Id);// workflowContext.PrimaryEntityId);
            //                        ColumnSet cols = new ColumnSet();
            //                        cols.AddColumns("ownerid", "telephone1");
            //                        Entity ent = servicio.Retrieve("account", ((EntityReference)e.Attributes["efk_cliente_juridico_id"]).Id, cols);

            //                        if (ent.Attributes.Contains("telephone1"))
            //                        {
            //                            newTask["phonenumber"] = ent.Attributes["telephone1"];
            //                        }

            //                        newTask["ownerid"] = (EntityReference)ent.Attributes["ownerid"];
            //                        newTask["subject"] = campo;//newSubject;
            //                        Entity newActivity = new Entity("activityparty");
            //                        newActivity["partyid"] = (EntityReference)ent.Attributes["ownerid"];
            //                        newTask["from"] = new EntityCollection(new[] { newActivity });
            //                        Entity newActivity1 = new Entity("activityparty");
            //                        newActivity1["partyid"] = new EntityReference("account", ((EntityReference)e.Attributes["efk_cliente_juridico_id"]).Id);
            //                        newTask["to"] = new EntityCollection(new[] { newActivity1 });
            //                        Guid taskId = servicio.Create(newTask);
            //                        campo = "";
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }
    }

}

