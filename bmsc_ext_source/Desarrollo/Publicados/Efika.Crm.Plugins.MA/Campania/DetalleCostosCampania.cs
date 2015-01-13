using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.MA.Campania
{
    public class DetalleCostoCampania_PostCreate : IPlugin
    {
        #region Variables
        private static string strFetchXmlDetalleCostoCampania;
        #endregion

        #region Main
        public void Execute(IServiceProvider serviceProvider)
        {

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            IOrganizationService iServices = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(context.UserId));

            // Get a reference to the tracing service.
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            decimal decMonto = 0;
            decimal decCostosDiversos = 0;
            string strCampaniaId = string.Empty;
            try
            {

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "efk_detalle_costos_campana")
                        return;

                    if (entity.Attributes.Contains("efk_monto"))
                    {
                        decMonto = Convert.ToDecimal(entity.GetAttributeValue<Money>("efk_monto").Value.ToString());

                        if (entity.Attributes.Contains("efk_campaignid"))
                        {
                            EntityReference CampaniaId = (EntityReference)entity.Attributes["efk_campaignid"];
                            strCampaniaId = CampaniaId.Id.ToString();
                        }

                        if (decMonto > 0 && strCampaniaId != string.Empty)
                        {
                            strFetchXmlDetalleCostoCampania = ObtenerDetalleCostoCampania();

                            EntityCollection ecDetalleCosto = null;
                            strFetchXmlDetalleCostoCampania = string.Format(strFetchXmlDetalleCostoCampania, strCampaniaId);

                            ecDetalleCosto = service.RetrieveMultiple(new FetchExpression(strFetchXmlDetalleCostoCampania));

                            if (ecDetalleCosto.Entities.Count > 0)
                            {
                                foreach (var reg in ecDetalleCosto.Entities)
                                {
                                    if (reg.Contains("efk_monto"))
                                    {
                                        decCostosDiversos = decCostosDiversos + Convert.ToDecimal(reg.GetAttributeValue<Money>("efk_monto").Value.ToString());
                                    }
                                }
                                Entity Campania = new Entity("campaign");
                                Campania.Id = new Guid(strCampaniaId);
                                Campania.Attributes.Add("othercost", new Money(decCostosDiversos));
                                service.Update(Campania);
                                Campania = null;
                            }
                        }
                    }
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("BBDetalleCostoCampania_PostCreate plug-in.", ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("BBDetalleCostoCampania_PostCreate plug-in: {0}", ex.ToString());
                throw;
            }
        }
        #endregion

        #region Funciones
        private string ObtenerDetalleCostoCampania()
        {
            strFetchXmlDetalleCostoCampania = string.Empty;

            strFetchXmlDetalleCostoCampania = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                                <entity name=""efk_detalle_costos_campana"">
                                                    <attribute name=""efk_monto"" />
                                                    <attribute name=""efk_campaignid"" />
                                                    <attribute name=""efk_detalle_costos_campanaid"" />
                                                    <order attribute=""efk_tipo"" descending=""true"" />
                                                    <filter type=""and"">
                                                      <condition attribute=""efk_campaignid"" operator=""eq"" value=""{0}"" />
                                                    </filter>
                                                  </entity>
                                                </fetch>";

            return strFetchXmlDetalleCostoCampania;
        }
        #endregion
    }

    public class DetalleCostoCampania_PostUpdate : IPlugin
    {
        #region Variables
        private static string strFetchXmlDetalleCostoCampania;
        #endregion

        #region Main
        public void Execute(IServiceProvider serviceProvider)
        {

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            IOrganizationService iServices = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(context.UserId));

            // Get a reference to the tracing service.
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            bool FlagActualizar = false;
            decimal decMonto = 0;
            decimal decCostosDiversos = 0;
            string strCampaniaId = string.Empty;
            try
            {

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "efk_detalle_costos_campana")
                        return;

                    Entity target = (Entity)context.PreEntityImages["efk_detalle_costos_campana_EntityImage"];

                    if (entity.Attributes.Contains("efk_monto"))
                    {
                        decMonto = Convert.ToDecimal(entity.GetAttributeValue<Money>("efk_monto").Value.ToString());
                        FlagActualizar = true;
                    }
                    else
                    {
                        decMonto = Convert.ToDecimal(target.GetAttributeValue<Money>("efk_monto").Value.ToString());
                    }

                    if (entity.Attributes.Contains("efk_campaignid"))
                    {
                        EntityReference CampaniaId = (EntityReference)entity.Attributes["efk_campaignid"];
                        strCampaniaId = CampaniaId.Id.ToString();
                        FlagActualizar = true;
                    }
                    else
                    {
                        EntityReference CampaniaId = (EntityReference)target.Attributes["efk_campaignid"];
                        strCampaniaId = CampaniaId.Id.ToString();
                    }

                    if (FlagActualizar == true)
                    {
                        strFetchXmlDetalleCostoCampania = ObtenerDetalleCostoCampania();

                        EntityCollection ecDetalleCosto = null;
                        strFetchXmlDetalleCostoCampania = string.Format(strFetchXmlDetalleCostoCampania, strCampaniaId);

                        ecDetalleCosto = service.RetrieveMultiple(new FetchExpression(strFetchXmlDetalleCostoCampania));

                        if (ecDetalleCosto.Entities.Count > 0)
                        {
                            foreach (var reg in ecDetalleCosto.Entities)
                            {
                                if (reg.Contains("efk_monto"))
                                {
                                    decCostosDiversos = decCostosDiversos + Convert.ToDecimal(reg.GetAttributeValue<Money>("efk_monto").Value.ToString());
                                }
                            }
                            Entity Campania = new Entity("campaign");
                            Campania.Id = new Guid(strCampaniaId);
                            Campania.Attributes.Add("othercost", new Money(decCostosDiversos));
                            service.Update(Campania);
                            Campania = null;
                        }
                    }
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("BBDetalleCostoCampania_PostUpdate plug-in.", ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("BBDetalleCostoCampania_PostUpdate plug-in: {0}", ex.ToString());
                throw;
            }
        }
        #endregion

        #region Funciones
        private string ObtenerDetalleCostoCampania()
        {
            strFetchXmlDetalleCostoCampania = string.Empty;

            strFetchXmlDetalleCostoCampania = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                                <entity name=""efk_detalle_costos_campana"">
                                                    <attribute name=""efk_monto"" />
                                                    <attribute name=""efk_campaignid"" />
                                                    <attribute name=""efk_detalle_costos_campanaid"" />
                                                    <order attribute=""efk_tipo"" descending=""true"" />
                                                    <filter type=""and"">
                                                      <condition attribute=""efk_campaignid"" operator=""eq"" value=""{0}"" />
                                                    </filter>
                                                  </entity>
                                                </fetch>";

            return strFetchXmlDetalleCostoCampania;
        }
        #endregion
    }

    public class DetalleCostoCampania_PostDelete : IPlugin
    {

        #region Variables
        private static string strFetchXmlDetalleCostoCampania;
        #endregion

        #region Main
        public void Execute(IServiceProvider serviceProvider)
        {

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            IOrganizationService iServices = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(context.UserId));

            // Get a reference to the tracing service.
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            decimal decCostosDiversos = 0;
            string strCampaniaId = string.Empty;
            try
            {

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is EntityReference)
                {
                    EntityReference entity = (EntityReference)context.InputParameters["Target"];

                    if (entity.LogicalName != "efk_detalle_costos_campana")
                        return;

                    Entity target = (Entity)context.PreEntityImages["efk_detalle_costos_campana_EntityImage"];

                    EntityReference CampaniaId = (EntityReference)target.Attributes["efk_campaignid"];
                    strCampaniaId = CampaniaId.Id.ToString();

                    strFetchXmlDetalleCostoCampania = ObtenerDetalleCostoCampania();

                    if (strCampaniaId != string.Empty)
                    {
                        EntityCollection ecDetalleCosto = null;
                        strFetchXmlDetalleCostoCampania = string.Format(strFetchXmlDetalleCostoCampania, strCampaniaId);

                        ecDetalleCosto = service.RetrieveMultiple(new FetchExpression(strFetchXmlDetalleCostoCampania));

                        if (ecDetalleCosto.Entities.Count >= 0)
                        {
                            foreach (var reg in ecDetalleCosto.Entities)
                            {
                                if (reg.Contains("efk_monto"))
                                {
                                    decCostosDiversos = decCostosDiversos + Convert.ToDecimal(reg.GetAttributeValue<Money>("efk_monto").Value.ToString());
                                }
                            }
                            Entity Campania = new Entity("campaign");
                            Campania.Id = new Guid(strCampaniaId);
                            Campania.Attributes.Add("othercost", new Money(decCostosDiversos));
                            service.Update(Campania);
                            Campania = null;
                        }
                    }
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("BBDetalleCostoCampania_PostUpdate plug-in.", ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("BBDetalleCostoCampania_PostUpdate plug-in: {0}", ex.ToString());
                throw;
            }
        }
        #endregion

        #region Funciones
        private string ObtenerDetalleCostoCampania()
        {
            strFetchXmlDetalleCostoCampania = string.Empty;

            strFetchXmlDetalleCostoCampania = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                                <entity name=""efk_detalle_costos_campana"">
                                                    <attribute name=""efk_monto"" />
                                                    <attribute name=""efk_campaignid"" />
                                                    <attribute name=""efk_detalle_costos_campanaid"" />
                                                    <order attribute=""efk_tipo"" descending=""true"" />
                                                    <filter type=""and"">
                                                      <condition attribute=""efk_campaignid"" operator=""eq"" value=""{0}"" />
                                                    </filter>
                                                  </entity>
                                                </fetch>";

            return strFetchXmlDetalleCostoCampania;
        }
        #endregion
    }
}
