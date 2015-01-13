using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.Margen_Credito
{
    public class ActualizarMontoOperaciones : IPlugin
    {
        #region Variables
        private static string strFetchXmlDetalleOperaciones;
        private decimal decmontoutilizado = 0;
        private decimal decmontoaprobado = 0;
        private string strMargenCreditoId = string.Empty;
        private string strMargenCreditoId_Image = string.Empty;
        private Entity entity = null;
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

            tracingService.Trace("EMpezando trace");
            
            try
            {

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    entity =(Entity) context.InputParameters["Target"];
                    if (entity.LogicalName != "efk_producto_activo")
                        return;

                    tracingService.Trace("EMpezando trace 1");
                    Entity preEntity = (Entity)context.PreEntityImages["efk_producto_activo_EntityImage"];
                    Entity postEntity = (Entity)context.PostEntityImages["efk_producto_activo_EntityImage"];

                    //Validamos que la operación "tenga" o "haya tenido" margen de crédito
                    if ((preEntity.Contains("efk_margen_creditoid") && preEntity["efk_margen_creditoid"] != null)
                        || (postEntity.Contains("efk_margen_creditoid") && postEntity["efk_margen_creditoid"] != null))
                    {
                        tracingService.Trace("EMpezando trace 2");
                        if (entity.Attributes.Contains("efk_saldo_fecha") && entity.Attributes["efk_saldo_fecha"] != null)
                        {
                            tracingService.Trace("EMpezando trace 3");
                            decmontoutilizado = Convert.ToDecimal(entity.GetAttributeValue<Money>("efk_saldo_fecha").Value.ToString());

                            FlagActualizar = true;
                        }
                        tracingService.Trace("EMpezando trace 4");
                        if (entity.Attributes.Contains("efk_margen_creditoid") && entity.Attributes["efk_margen_creditoid"] != null)
                        {
                            tracingService.Trace("EMpezando trace 5");
                            strMargenCreditoId = ((EntityReference)postEntity.Attributes["efk_margen_creditoid"]).Id.ToString();
                            FlagActualizar = true;
                        }
                        else
                        {
                            tracingService.Trace("EMpezando trace 6");
                            strMargenCreditoId = ((EntityReference)preEntity.Attributes["efk_margen_creditoid"]).Id.ToString();
                            FlagActualizar = true;
                        }

                        if (FlagActualizar)
                        {
                            tracingService.Trace("Actualizar montos de margen de credito");
                            Actualizar_Montos_MargenCredito(service, strMargenCreditoId);

                             //Obtenemos el cliente
                            Entity margen=service.Retrieve("efk_margen_credito", new Guid(strMargenCreditoId), new ColumnSet("efk_accountid"));

                            if (margen != null && margen.Attributes.Contains("efk_accountid") && margen.Attributes["efk_accountid"]!=null)
                                Actualizar_Montos_MargenCredito_Cliente(service, ((EntityReference)margen.Attributes["efk_accountid"]).Id.ToString());
                        }

                        EntityReference preMargenCredito = null;

                        if (preEntity.Contains("efk_margen_creditoid"))
                            preMargenCredito = ((EntityReference)preEntity.Attributes["efk_margen_creditoid"]);

                        EntityReference postMargenCredito = null;
                        
                        if(postEntity.Contains("efk_margen_creditoid"))
                            postMargenCredito=((EntityReference)postEntity.Attributes["efk_margen_creditoid"]);

                        //Comparamos ambos
                        if (preMargenCredito!=null && (postMargenCredito == null || (preMargenCredito.Id != postMargenCredito.Id)))
                        {
                            Actualizar_Montos_MargenCredito(service, preMargenCredito.Id.ToString());

                            //Obtenemos el cliente
                            Entity margen = service.Retrieve("efk_margen_credito", new Guid(strMargenCreditoId), new ColumnSet("efk_accountid"));

                            if (margen != null && margen.Attributes.Contains("efk_accountid") && margen.Attributes["efk_accountid"] != null)
                                Actualizar_Montos_MargenCredito_Cliente(service, ((EntityReference)margen.Attributes["efk_accountid"]).Id.ToString());
                        }

                    }
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("ActualizarMontoOperaciones_PostUpdate plug-in." + ex.Message, ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("ActualizarMontoOperaciones_PostUpdate plug-in: {0} " + ex.Message, ex.ToString());
                throw;
            }
        }

        private void Actualizar_Montos_MargenCredito(IOrganizationService service, string strMargenCreditoId_Actualizar)
        {
            strFetchXmlDetalleOperaciones = ObtenerDetalleOperaciones();

            EntityCollection ecDetalleSaldoOperaciones = null;
            strFetchXmlDetalleOperaciones = string.Format(strFetchXmlDetalleOperaciones, strMargenCreditoId_Actualizar);

            ecDetalleSaldoOperaciones = service.RetrieveMultiple(new FetchExpression(strFetchXmlDetalleOperaciones));
            decmontoutilizado = 0;
            if (ecDetalleSaldoOperaciones.Entities.Count > 0)
            {
                foreach (var reg in ecDetalleSaldoOperaciones.Entities)
                {
                    if (reg.Contains("efk_saldo_fecha"))
                    {
                        decmontoutilizado = decmontoutilizado + Convert.ToDecimal(reg.GetAttributeValue<Money>("efk_saldo_fecha").Value.ToString());
                    }
                }
            }
                
            Entity margCreditoEntity = service.Retrieve("efk_margen_credito", new Guid(strMargenCreditoId_Actualizar), new ColumnSet("efk_monto"));

            if (margCreditoEntity.Attributes.Contains("efk_monto") && margCreditoEntity.Attributes["efk_monto"] != null)
                decmontoaprobado = ((Money)margCreditoEntity.Attributes["efk_monto"]).Value;

            Entity Margen = new Entity("efk_margen_credito");
            Margen.Id = new Guid(strMargenCreditoId_Actualizar);
            Margen.Attributes.Add("efk_monto_disponible", new Money((decmontoaprobado - decmontoutilizado)));
            Margen.Attributes.Add("efk_monto_utilizado", new Money(decmontoutilizado));
            if (decmontoaprobado > 0)
                Margen.Attributes.Add("efk_porcentaje_uso", (decmontoutilizado / decmontoaprobado) * 100);

            service.Update(Margen);
            Margen = null;
            margCreditoEntity = null;
        }

        private void Actualizar_Montos_MargenCredito_Cliente(IOrganizationService service, string strClienteId)
        {
            string strFetchXmlDetalleCliente = ObtenerDetalleCliente();

            EntityCollection ec = null;
            strFetchXmlDetalleCliente = string.Format(strFetchXmlDetalleCliente, strClienteId);

            ec = service.RetrieveMultiple(new FetchExpression(strFetchXmlDetalleCliente));

            if (ec.Entities.Count > 0)
            {
                decimal monto_aprobado = 0;
                decimal monto_utilizado = 0;

                foreach (Entity e in ec.Entities)
                {
                    if (e.Contains("efk_monto") && e.Attributes["efk_monto"] != null) 
                        monto_aprobado += ((Money)e["efk_monto"]).Value;
                    if(e.Contains("efk_monto_utilizado") && e.Attributes["efk_monto_utilizado"]!=null)
                        monto_utilizado += ((Money)e["efk_monto_utilizado"]).Value;
                }

                Entity cliente = new Entity("account");
                if (monto_aprobado > 0)
                {
                    cliente["efk_monto_total_margenes_credito_vigentes"] = new Money(monto_aprobado);
                }
                else
                {
                    cliente["efk_monto_total_margenes_credito_vigentes"] = null;
                }
                if (monto_utilizado > 0)
                {
                    cliente["efk_monto_total_util_margenes_credito_vigente"] = new Money(monto_utilizado);
                }
                else{
                    cliente["efk_monto_total_util_margenes_credito_vigente"] = null;
                }
                if (monto_aprobado > 0 && monto_utilizado > 0)
                {
                    cliente["efk_porc_util_margenes_credito_vigentes"] = (double)((monto_utilizado / monto_aprobado)*100);
                }
                else
                {
                    cliente["efk_porc_util_margenes_credito_vigentes"] = null;
                }
                cliente.Id = new Guid(strClienteId);

                service.Update(cliente);
            }
        }


        #endregion

        #region Funciones
        private string ObtenerDetalleOperaciones()
        {
            strFetchXmlDetalleOperaciones = string.Empty;

            strFetchXmlDetalleOperaciones = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                                <entity name=""efk_producto_activo"">
                                                    <attribute name=""efk_saldo_fecha"" />
                                                    <attribute name=""efk_margen_creditoid"" />                                                
                                                    <order attribute=""efk_name"" descending=""true"" />
                                                    <filter type=""and"">
                                                      <condition attribute=""efk_margen_creditoid"" operator=""eq"" value=""{0}"" />
                                                    </filter>
                                                  </entity>
                                                </fetch>";

            return strFetchXmlDetalleOperaciones;
        }

        private string ObtenerDetalleCliente()
        {
            string strFetchXmlDetalleCliente = string.Empty;

            strFetchXmlDetalleCliente = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                              <entity name=""efk_margen_credito"">
                                                <attribute name=""efk_margen_creditoid"" />
                                                <attribute name=""efk_monto"" />
                                                <attribute name=""efk_monto_utilizado"" />
                                                <order attribute=""efk_nombre"" descending=""false"" />
                                                <filter type=""and"">
                                                  <condition attribute=""efk_accountid"" operator=""eq""  uitype=""account"" value=""{0}"" />
                                                  <condition attribute=""efk_estado"" operator=""eq"" value=""221220000"" />
                                                </filter>
                                              </entity>
                                            </fetch>";

            return strFetchXmlDetalleCliente;
        }
        #endregion
    }

    public class ActualizarMontoOperaciones_PostDelete : IPlugin
    {
        #region Variables
        private static string strFetchXmlDetalleOperaciones;
        private decimal decmontoutilizado = 0;
        private decimal decmontoaprobado = 0;
        private string strMargenCreditoId = string.Empty;
        private string strMargenCreditoId_Image = string.Empty;
        private EntityReference entity = null;
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

            try
            {

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is EntityReference)
                {
                    entity = (EntityReference)context.InputParameters["Target"];
                    if (entity.LogicalName != "efk_producto_activo")
                        return;

                    Entity preEntity = (Entity)context.PreEntityImages["efk_producto_activo_EntityImage"];
                    
                    //Validamos que la operación "haya tenido" margen de crédito
                    if ((preEntity.Contains("efk_margen_creditoid") && preEntity["efk_margen_creditoid"] != null))
                    {
                        strMargenCreditoId = ((EntityReference)preEntity.Attributes["efk_margen_creditoid"]).Id.ToString();
                        Actualizar_Montos_MargenCredito(service, strMargenCreditoId);

                        //Obtenemos el cliente
                        Entity margen = service.Retrieve("efk_margen_credito", new Guid(strMargenCreditoId), new ColumnSet("efk_accountid"));

                        if (margen != null && margen.Attributes.Contains("efk_accountid") && margen.Attributes["efk_accountid"] != null)
                            Actualizar_Montos_MargenCredito_Cliente(service, ((EntityReference)margen.Attributes["efk_accountid"]).Id.ToString());
                    }
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("ActualizarMontoOperaciones_PostUpdate plug-in." + ex.Message, ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("ActualizarMontoOperaciones_PostUpdate plug-in: {0} " + ex.Message, ex.ToString());
                throw;
            }
        }

        private void Actualizar_Montos_MargenCredito(IOrganizationService service, string strMargenCreditoId_Actualizar)
        {
            strFetchXmlDetalleOperaciones = ObtenerDetalleOperaciones();

            EntityCollection ecDetalleSaldoOperaciones = null;
            strFetchXmlDetalleOperaciones = string.Format(strFetchXmlDetalleOperaciones, strMargenCreditoId_Actualizar);

            ecDetalleSaldoOperaciones = service.RetrieveMultiple(new FetchExpression(strFetchXmlDetalleOperaciones));
            decmontoutilizado = 0;
            if (ecDetalleSaldoOperaciones.Entities.Count > 0)
            {
                foreach (var reg in ecDetalleSaldoOperaciones.Entities)
                {
                    if (reg.Contains("efk_saldo_fecha"))
                    {
                        decmontoutilizado = decmontoutilizado + Convert.ToDecimal(reg.GetAttributeValue<Money>("efk_saldo_fecha").Value.ToString());
                    }
                }
            }

            Entity margCreditoEntity = service.Retrieve("efk_margen_credito", new Guid(strMargenCreditoId_Actualizar), new ColumnSet("efk_monto"));

            if (margCreditoEntity.Attributes.Contains("efk_monto") && margCreditoEntity.Attributes["efk_monto"] != null)
                decmontoaprobado = ((Money)margCreditoEntity.Attributes["efk_monto"]).Value;

            Entity Margen = new Entity("efk_margen_credito");
            Margen.Id = new Guid(strMargenCreditoId_Actualizar);
            Margen.Attributes.Add("efk_monto_disponible", new Money((decmontoaprobado - decmontoutilizado)));
            Margen.Attributes.Add("efk_monto_utilizado", new Money(decmontoutilizado));
            if (decmontoaprobado > 0)
                Margen.Attributes.Add("efk_porcentaje_uso", (decmontoutilizado / decmontoaprobado) * 100);

            service.Update(Margen);
            Margen = null;
            margCreditoEntity = null;
        }

        private void Actualizar_Montos_MargenCredito_Cliente(IOrganizationService service, string strClienteId)
        {
            string strFetchXmlDetalleCliente = ObtenerDetalleCliente();

            EntityCollection ec = null;
            strFetchXmlDetalleCliente = string.Format(strFetchXmlDetalleCliente, strClienteId);

            ec = service.RetrieveMultiple(new FetchExpression(strFetchXmlDetalleCliente));

            if (ec.Entities.Count > 0)
            {
                decimal monto_aprobado = 0;
                decimal monto_utilizado = 0;

                foreach (Entity e in ec.Entities)
                {
                    if (e.Contains("efk_monto") && e.Attributes["efk_monto"] != null)
                        monto_aprobado += ((Money)e["efk_monto"]).Value;
                    if (e.Contains("efk_monto_utilizado") && e.Attributes["efk_monto_utilizado"] != null)
                        monto_utilizado += ((Money)e["efk_monto_utilizado"]).Value;
                }

                Entity cliente = new Entity("account");
                if (monto_aprobado > 0)
                {
                    cliente["efk_monto_total_margenes_credito_vigentes"] = new Money(monto_aprobado);
                }
                else
                {
                    cliente["efk_monto_total_margenes_credito_vigentes"] = null;
                }
                if (monto_utilizado > 0)
                {
                    cliente["efk_monto_total_util_margenes_credito_vigente"] = new Money(monto_utilizado);
                }
                else
                {
                    cliente["efk_monto_total_util_margenes_credito_vigente"] = null;
                }
                if (monto_aprobado > 0 && monto_utilizado > 0)
                {
                    cliente["efk_porc_util_margenes_credito_vigentes"] = (double)((monto_utilizado / monto_aprobado)*100);
                }
                else
                {
                    cliente["efk_porc_util_margenes_credito_vigentes"] = null;
                }
                cliente.Id = new Guid(strClienteId);

                service.Update(cliente);
            }
        }


        #endregion

        #region Funciones
        private string ObtenerDetalleOperaciones()
        {
            strFetchXmlDetalleOperaciones = string.Empty;

            strFetchXmlDetalleOperaciones = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                                <entity name=""efk_producto_activo"">
                                                    <attribute name=""efk_saldo_fecha"" />
                                                    <attribute name=""efk_margen_creditoid"" />                                                
                                                    <order attribute=""efk_name"" descending=""true"" />
                                                    <filter type=""and"">
                                                      <condition attribute=""efk_margen_creditoid"" operator=""eq"" value=""{0}"" />
                                                    </filter>
                                                  </entity>
                                                </fetch>";

            return strFetchXmlDetalleOperaciones;
        }

        private string ObtenerDetalleCliente()
        {
            string strFetchXmlDetalleCliente = string.Empty;

            strFetchXmlDetalleCliente = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                              <entity name=""efk_margen_credito"">
                                                <attribute name=""efk_margen_creditoid"" />
                                                <attribute name=""efk_monto"" />
                                                <attribute name=""efk_monto_utilizado"" />
                                                <order attribute=""efk_nombre"" descending=""false"" />
                                                <filter type=""and"">
                                                  <condition attribute=""efk_accountid"" operator=""eq""  uitype=""account"" value=""{0}"" />
                                                  <condition attribute=""efk_estado"" operator=""eq"" value=""221220000"" />
                                                </filter>
                                              </entity>
                                            </fetch>";

            return strFetchXmlDetalleCliente;
        }
        #endregion
    }

    public class ActualizarMontoMargenCredito : IPlugin
    {
        #region Variables
        private string strMargenCreditoId = string.Empty;
        private string strMargenCreditoId_Image = string.Empty;
        private Entity entity = null;
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
            
            try
            {

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    entity = (Entity)context.InputParameters["Target"];
                    if (entity.LogicalName != "efk_margen_credito")
                        return;

                    Entity postEntity = (Entity)context.PostEntityImages["efk_margen_credito_EntityImage"];

                    //Validamos que la operación "tenga" o "haya tenido" margen de crédito
                    if ((postEntity.Contains("efk_accountid") && postEntity["efk_accountid"] != null))
                    {
                        Actualizar_Montos_MargenCredito_Cliente(service, ((EntityReference)postEntity.Attributes["efk_accountid"]).Id.ToString());

                    }
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("ActualizarMontoOperaciones_PostUpdate plug-in." + ex.Message, ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("ActualizarMontoOperaciones_PostUpdate plug-in: {0} " + ex.Message, ex.ToString());
                throw;
            }
        }

        private void Actualizar_Montos_MargenCredito_Cliente(IOrganizationService service, string strClienteId)
        {
            string strFetchXmlDetalleCliente = ObtenerDetalleCliente();

            EntityCollection ec = null;
            strFetchXmlDetalleCliente = string.Format(strFetchXmlDetalleCliente, strClienteId);

            ec = service.RetrieveMultiple(new FetchExpression(strFetchXmlDetalleCliente));

            if (ec.Entities.Count > 0)
            {
                decimal monto_aprobado = 0;
                decimal monto_utilizado = 0;

                foreach (Entity e in ec.Entities)
                {
                    if (e.Contains("efk_monto") && e.Attributes["efk_monto"] != null)
                        monto_aprobado += ((Money)e["efk_monto"]).Value;
                    if (e.Contains("efk_monto_utilizado") && e.Attributes["efk_monto_utilizado"] != null)
                        monto_utilizado += ((Money)e["efk_monto_utilizado"]).Value;
                }

                Entity cliente = new Entity("account");
                if (monto_aprobado > 0)
                {
                    cliente["efk_monto_total_margenes_credito_vigentes"] = new Money(monto_aprobado);
                }
                else
                {
                    cliente["efk_monto_total_margenes_credito_vigentes"] = null;
                }
                if (monto_utilizado > 0)
                {
                    cliente["efk_monto_total_util_margenes_credito_vigente"] = new Money(monto_utilizado);
                }
                else
                {
                    cliente["efk_monto_total_util_margenes_credito_vigente"] = null;
                }
                if (monto_aprobado > 0 && monto_utilizado > 0)
                {
                    cliente["efk_porc_util_margenes_credito_vigentes"] = (double)((monto_utilizado / monto_aprobado) * 100);
                }
                else
                {
                    cliente["efk_porc_util_margenes_credito_vigentes"] = null;
                }
                cliente.Id = new Guid(strClienteId);

                service.Update(cliente);
            }
        }


        #endregion

        #region Funciones

        private string ObtenerDetalleCliente()
        {
            string strFetchXmlDetalleCliente = string.Empty;

            strFetchXmlDetalleCliente = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                              <entity name=""efk_margen_credito"">
                                                <attribute name=""efk_margen_creditoid"" />
                                                <attribute name=""efk_monto"" />
                                                <attribute name=""efk_monto_utilizado"" />
                                                <order attribute=""efk_nombre"" descending=""false"" />
                                                <filter type=""and"">
                                                  <condition attribute=""efk_accountid"" operator=""eq""  uitype=""account"" value=""{0}"" />
                                                  <condition attribute=""efk_estado"" operator=""eq"" value=""221220000"" />
                                                </filter>
                                              </entity>
                                            </fetch>";

            return strFetchXmlDetalleCliente;
        }
        #endregion
    }

    public class ActualizarMontoMargenCredito_PostDelete : IPlugin
    {
        #region Variables
        private string strMargenCreditoId = string.Empty;
        private string strMargenCreditoId_Image = string.Empty;
        private EntityReference entity = null;
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

            try
            {

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is EntityReference)
                {
                    entity = (EntityReference)context.InputParameters["Target"];
                    if (entity.LogicalName != "efk_margen_credito")
                        return;

                    Entity preEntity = (Entity)context.PreEntityImages["efk_margen_credito_EntityImage"];

                    //Validamos que la operación "tenga" o "haya tenido" margen de crédito
                    if ((preEntity.Contains("efk_accountid") && preEntity["efk_accountid"] != null))
                    {
                        Actualizar_Montos_MargenCredito_Cliente(service, ((EntityReference)preEntity["efk_accountid"]).Id.ToString());

                    }
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("ActualizarMontoOperaciones_PostUpdate plug-in." + ex.Message, ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("ActualizarMontoOperaciones_PostUpdate plug-in: {0} " + ex.Message, ex.ToString());
                throw;
            }
        }

        private void Actualizar_Montos_MargenCredito_Cliente(IOrganizationService service, string strClienteId)
        {
            string strFetchXmlDetalleCliente = ObtenerDetalleCliente();

            EntityCollection ec = null;
            strFetchXmlDetalleCliente = string.Format(strFetchXmlDetalleCliente, strClienteId);

            ec = service.RetrieveMultiple(new FetchExpression(strFetchXmlDetalleCliente));

            if (ec.Entities.Count > 0)
            {
                decimal monto_aprobado = 0;
                decimal monto_utilizado = 0;

                foreach (Entity e in ec.Entities)
                {
                    if (e.Contains("efk_monto") && e.Attributes["efk_monto"] != null)
                        monto_aprobado += ((Money)e["efk_monto"]).Value;
                    if (e.Contains("efk_monto_utilizado") && e.Attributes["efk_monto_utilizado"] != null)
                        monto_utilizado += ((Money)e["efk_monto_utilizado"]).Value;
                }

                Entity cliente = new Entity("account");
                if (monto_aprobado > 0)
                {
                    cliente["efk_monto_total_margenes_credito_vigentes"] = new Money(monto_aprobado);
                }
                else
                {
                    cliente["efk_monto_total_margenes_credito_vigentes"] = null;
                }
                if (monto_utilizado > 0)
                {
                    cliente["efk_monto_total_util_margenes_credito_vigente"] = new Money(monto_utilizado);
                }
                else
                {
                    cliente["efk_monto_total_util_margenes_credito_vigente"] = null;
                }
                if (monto_aprobado > 0 && monto_utilizado > 0)
                {
                    cliente["efk_porc_util_margenes_credito_vigentes"] = (double)((monto_utilizado / monto_aprobado) * 100);
                }
                else
                {
                    cliente["efk_porc_util_margenes_credito_vigentes"] = null;
                }
                cliente.Id = new Guid(strClienteId);

                service.Update(cliente);
            }
        }


        #endregion

        #region Funciones

        private string ObtenerDetalleCliente()
        {
            string strFetchXmlDetalleCliente = string.Empty;

            strFetchXmlDetalleCliente = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                              <entity name=""efk_margen_credito"">
                                                <attribute name=""efk_margen_creditoid"" />
                                                <attribute name=""efk_monto"" />
                                                <attribute name=""efk_monto_utilizado"" />
                                                <order attribute=""efk_nombre"" descending=""false"" />
                                                <filter type=""and"">
                                                  <condition attribute=""efk_accountid"" operator=""eq""  uitype=""account"" value=""{0}"" />
                                                  <condition attribute=""efk_estado"" operator=""eq"" value=""221220000"" />
                                                </filter>
                                              </entity>
                                            </fetch>";

            return strFetchXmlDetalleCliente;
        }
        #endregion
    }
}
