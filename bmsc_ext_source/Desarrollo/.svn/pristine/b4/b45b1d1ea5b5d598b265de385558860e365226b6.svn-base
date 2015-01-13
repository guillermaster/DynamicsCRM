using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.SFA.Oportunidad
{
    public class ValidarClienteOportunidad: IPlugin
    {
        #region Variables
        private static string strFetchXmlCliente;
        private static string strFetchXmlContacto;
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
                string CuentaId = string.Empty;
                string strNombreEntidad = string.Empty;

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    #region Atributos Entidad
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "opportunity")
                        return;

                    if (entity.Attributes.Contains("customerid"))
                    {
                        EntityReference eCuenta = (EntityReference)entity.Attributes["customerid"];
                        CuentaId = eCuenta.Id.ToString();
                        strNombreEntidad = eCuenta.LogicalName;
                    }
                    #endregion

                    #region Validar Cliente
                    if (CuentaId != string.Empty && strNombreEntidad != string.Empty)
                    {
                        if (strNombreEntidad == "account")
                        {
                            strFetchXmlCliente = ObtenerDatosCliente();

                            EntityCollection ecCliente = null;
                            strFetchXmlCliente = string.Format(strFetchXmlCliente, CuentaId);
                            ecCliente = service.RetrieveMultiple(new FetchExpression(strFetchXmlCliente));

                            foreach (Entity eCliente in ecCliente.Entities)
                            {
                                bool flagIndicador = Convert.ToBoolean(eCliente.Attributes["efk_indicador_no_vinculacion"]);
                                if (flagIndicador)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendLine("El cliente tiene indicador de No Vinculación. No puede crear una oportunidad");
                                    sb.AppendLine("con este cliente.");
                                    sb.AppendLine("");
                                    sb.AppendLine("Para continuar de clic en 'Aceptar'.");
                                    throw new Exception(sb.ToString());
                                }
                            }
                        }
                        else
                        {
                            if (strNombreEntidad == "contact")
                            {
                                strFetchXmlContacto = ObtenerDatosContacto();

                                EntityCollection ecContacto = null;
                                strFetchXmlContacto = string.Format(strFetchXmlContacto, CuentaId);
                                ecContacto = service.RetrieveMultiple(new FetchExpression(strFetchXmlContacto));

                                foreach (Entity eContacto in ecContacto.Entities)
                                {
                                    bool flagIndicador = Convert.ToBoolean(eContacto.Attributes["efk_indicador_no_vinculacion"]);
                                    if (flagIndicador)
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        sb.AppendLine("El cliente tiene indicador de No Vinculación. No puede crear una oportunidad");
                                        sb.AppendLine("con este cliente.");
                                        sb.AppendLine("");
                                        sb.AppendLine("Para continuar de clic en 'Aceptar'.");
                                        throw new Exception(sb.ToString());
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("BBOpportunity_PreCreate plug-in: " + ex.Message.ToString(), ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("BBOpportunity_PreCreate plug-in: {0}", ex.ToString());
                throw;
            }
        }
        #endregion

        #region Funciones

        public string ObtenerDatosCliente()
        {
            strFetchXmlCliente = string.Empty;

            strFetchXmlCliente = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                    <entity name=""account"">
                                        <attribute name=""accountid"" />
                                        <attribute name=""efk_indicador_no_vinculacion"" />
                                        <order attribute=""name"" descending=""false"" />
                                        <filter type=""and"">
                                            <condition attribute=""accountid"" operator=""eq"" value=""{0}"" />
                                        </filter>
                                    </entity>
                                    </fetch>";

            return strFetchXmlCliente;
        }

        public string ObtenerDatosContacto()
        {
            strFetchXmlContacto = string.Empty;

            strFetchXmlContacto = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                    <entity name=""contact"">
                                        <attribute name=""contactid"" />
                                        <attribute name=""efk_indicador_no_vinculacion"" />
                                        <order attribute=""fullname"" descending=""false"" />
                                        <filter type=""and"">
                                            <condition attribute=""contactid"" operator=""eq"" value=""{0}"" />
                                        </filter>
                                    </entity>
                                    </fetch>";

            return strFetchXmlContacto;
        }
        #endregion
    }
}
