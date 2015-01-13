using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.Account 
{
    /// <summary>
    /// Valida la existencia de un cliente en base al número de identificación: accountnumber
    /// </summary>
    public class ValidarExistenciaCliente : IPlugin
    {
        private int ClienteNatural=100000000;
        private int ClienteJuridico = 100000001;

        private string FetchXmlObtenerClientesMismaIdentificacion
        {
            get
            {
                return
                    @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                  <entity name=""account"">
                    <attribute name=""accountid"" />
                    <filter type=""and"">
                      <condition attribute=""statecode"" operator=""eq"" value=""0"" />
                      <condition attribute=""accountnumber"" operator=""eq"" value=""{0}"" />
                        <condition attribute=""accountid"" operator=""ne"" uiname="""" uitype=""account"" value=""{1}"" />
                    </filter>
                  </entity>
                </fetch>";
            }
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            //IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            IOrganizationService service = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(null);
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

                    //Validamos si existe otro cliente con el mismo número de identificación
                    if (entity.Attributes.Contains("accountnumber"))
                    {
                        string identificacion = entity.Attributes["accountnumber"].ToString();

                        //preguntamos primero si en los datos del cliente no llega el código de cliente
                        if (!entity.Attributes.Contains("efk_codigo_cliente"))
                        {
                            //en este punto se hace la validación
                            EntityCollection ec = service.RetrieveMultiple(new FetchExpression(string.Format(FetchXmlObtenerClientesMismaIdentificacion, identificacion, context.PrimaryEntityId)));

                            if (ec.Entities.Count > 0)
                            {
                                throw new InvalidPluginExecutionException("Ya existe un cliente con el mismo número de identificación ingresado.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace(this.GetType().ToString()+ "_PreCreate: {0} ", ex.ToString());
                throw;
            }
        }

    }
}
