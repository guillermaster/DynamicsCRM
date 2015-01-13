using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.MA.Campania
{
    public class CierreActividadCampania : CodeActivity
    {
        #region Variables
        private static string strFetchXmlActividadCampania;
        private static string strFetchXmlOportunidadCampania;
        private static string strFetchCampaniasClientes;
        private static int intEstadoCancelado;
        private static int intRazonEstadoCancelado;
        #endregion

        protected override void Execute(CodeActivityContext executionContext)
        {
            //Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            tracingService.Trace("Leyendo parámetros de flujo");

            //Retrieve the contact id
            Guid IdCampania = Campania.Get(executionContext).Id;
            string strEstadoCampania = EstadoCampania.Get<string>(executionContext);
            
            tracingService.Trace("Buscando actividades de campaña");

            strFetchXmlActividadCampania = ObtenerActividadCampania();

            EntityCollection ecActividad = null;
            strFetchXmlActividadCampania = string.Format(strFetchXmlActividadCampania, IdCampania.ToString());

            ecActividad = service.RetrieveMultiple(new FetchExpression(strFetchXmlActividadCampania));

            tracingService.Trace("Creando notas en las actividades");

            if (ecActividad.Entities.Count > 0)
            {
                foreach (Entity eActividad in ecActividad.Entities)
                {
                    Guid ActividadId = new Guid(eActividad.Attributes["activityid"].ToString());
                    string strActivityTypeCode = eActividad.Attributes["activitytypecode"].ToString();

                    if (strActivityTypeCode == "fax" || strActivityTypeCode == "phonecall" || strActivityTypeCode == "letter" || strActivityTypeCode == "appointment")
                    {
                        Entity note = new Entity("annotation");
                        note.Attributes["notetext"] = "La presente actividad fue cerrada automáticamente como cancelada debido a que la campaña de origen '"
                            + NombreCampania.Get<string>(executionContext).ToString() + "' fue " + strEstadoCampania + ".";
                        note.Attributes["subject"] = "Cierre automático por campaña completada";
                        note.Attributes["objectid"] = new EntityReference(strActivityTypeCode, ActividadId);
                        Guid noteId = service.Create(note);

                        switch (strActivityTypeCode)
                        {
                            case "fax":
                                intEstadoCancelado = 2;
                                intRazonEstadoCancelado = 5;
                                break;
                            case "phonecall":
                                intEstadoCancelado = 2;
                                intRazonEstadoCancelado = 3;
                                break;
                            case "letter":
                                intEstadoCancelado = 2;
                                intRazonEstadoCancelado = 5;
                                break;
                            case "appointment":
                            default:
                                intEstadoCancelado = 2;
                                intRazonEstadoCancelado = 4;
                                break;
                        }

                        SetStateRequest state = new SetStateRequest();
                        state.EntityMoniker = new EntityReference(strActivityTypeCode, ActividadId);
                        state.State = new OptionSetValue(intEstadoCancelado);
                        state.Status = new OptionSetValue(intRazonEstadoCancelado);
                        service.Execute(state);
                    }

                    if (strEstadoCampania == "Cancelado")
                    {
                        tracingService.Trace("Obtener Oportunidad");

                        strFetchXmlOportunidadCampania = ObtenerOportunidadCampania();

                        EntityCollection ecOportunidad = null;
                        strFetchXmlOportunidadCampania = string.Format(strFetchXmlOportunidadCampania, IdCampania.ToString());

                        ecOportunidad = service.RetrieveMultiple(new FetchExpression(strFetchXmlOportunidadCampania));

                        tracingService.Trace("Cerrar oportunidad");

                        foreach (Entity eOportunidad in ecOportunidad.Entities)
                        {
                            Guid OportunidadId = new Guid(eOportunidad.Attributes["opportunityid"].ToString());

                            LoseOpportunityRequest CloseOpportunityReq = new LoseOpportunityRequest();

                            Entity eOpportunityClose = new Entity("opportunityclose");
                            eOpportunityClose.Attributes["opportunityid"] = new EntityReference("opportunity", OportunidadId);

                            CloseOpportunityReq.OpportunityClose = eOpportunityClose;
                            CloseOpportunityReq.Status = new OptionSetValue(-1);

                            LoseOpportunityResponse CloseOpportunityRes = (LoseOpportunityResponse)service.Execute(CloseOpportunityReq);
                        }
                    }
                    tracingService.Trace("Termino Actividades de distribución");

                    //Eliminamos los registros participante de campaña
                    tracingService.Trace("Eliminamos los registros de participación en campaña.");
                    string strFetchCampaniasClientes = String.Format(ObtenerCampaniasParticipantesCliente(), Campania.Get(executionContext).Id);
                    EntityCollection registrosExistentes = service.RetrieveMultiple(new FetchExpression(strFetchCampaniasClientes));

                    foreach (Entity registro in registrosExistentes.Entities)
                    {
                        service.Delete(registro.LogicalName, registro.Id);
                    }
                }
            }

            tracingService.Trace("Actividades Cerradas");

        }

        #region Funciones
        private string ObtenerActividadCampania()
        {
            strFetchXmlActividadCampania = string.Empty;

            strFetchXmlActividadCampania = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                            <entity name=""activitypointer"">
                                                <attribute name=""activitytypecode"" />
                                                <attribute name=""statecode"" />
                                                <attribute name=""prioritycode"" />
                                                <attribute name=""activityid"" />
                                                <attribute name=""instancetypecode"" />
                                                <order attribute=""modifiedon"" descending=""false"" />
                                                <filter type=""and"">
                                                    <condition attribute=""statecode"" operator=""in"">
                                                        <value>0</value>
                                                        <value>3</value>
                                                    </condition>
                                                </filter>
                                                <link-entity name=""campaignactivity"" from=""activityid"" to=""regardingobjectid"" alias=""aa"">
                                                    <attribute name=""activitytypecode"" />
                                                    <attribute name=""regardingobjectid"" />
                                                    <filter type=""and"">
                                                        <condition attribute=""statecode"" operator=""eq"" value=""1"" />
                                                    </filter>
                                                    <link-entity name=""campaign"" from=""campaignid"" to=""regardingobjectid"" alias=""af"">
                                                        <filter type=""and"">
                                                            <condition attribute=""campaignid"" operator=""eq"" value=""{0}"" />
                                                        </filter>
                                                    </link-entity>
                                                </link-entity>
                                            </entity>
                                        </fetch>";

            return strFetchXmlActividadCampania;
        }
        
        private string ObtenerOportunidadCampania()
        {
            strFetchXmlOportunidadCampania = string.Empty;

            strFetchXmlOportunidadCampania = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                            <entity name=""opportunity"">
                                                <attribute name=""customerid"" />
                                                <attribute name=""ownerid"" />
                                                <attribute name=""opportunityid"" />
                                                <attribute name=""name"" />
                                                <order attribute=""name"" descending=""false"" />
                                                <filter type=""and"">
                                                    <condition attribute=""campaignid"" operator=""eq"" value=""{0}"" />
                                                    <condition attribute=""statecode"" operator=""eq"" value=""0"" />
                                                </filter>
                                            </entity>
                                        </fetch>";

            return strFetchXmlOportunidadCampania;
        }

        private string ObtenerCampaniasParticipantesCliente()
        {
            strFetchCampaniasClientes = string.Empty;

            strFetchCampaniasClientes = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                          <entity name=""efk_cliente_campania"">
                            <attribute name=""efk_cliente_campaniaid"" />
                            <attribute name=""efk_nombre"" />
                            <attribute name=""createdon"" />
                            <attribute name=""efk_productid"" />
                            <attribute name=""efk_monto_pre_aprobado"" />
                            <attribute name=""efk_campaignid"" />
                            <attribute name=""efk_accountid"" />
                            <order attribute=""efk_nombre"" descending=""false"" />
                            <filter type=""and"">
                                <condition attribute=""efk_campaignid"" operator=""eq"" uitype=""campaign"" value=""{0}"" />
                            </filter>
                          </entity>
                        </fetch>";

            return strFetchCampaniasClientes;
        }
        #endregion


        [Input("Id Campania")]
        [ReferenceTarget("campaign")]
        public InArgument<EntityReference> Campania { get; set; }

        [Input("Estado Campania")]
        [Default("Completado")]
        public InArgument<string> EstadoCampania { get; set; }

        [Input("Nombre Campania")]
        [Default("Nombre Campania")]
        public InArgument<string> NombreCampania { get; set; }
    }
}
