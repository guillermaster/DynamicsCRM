using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.Account
{
    public class GenerarNombreCliente: IPlugin
    {
        private int ClienteNatural = 221220000;
        private int ClienteJuridico = 221220001;


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
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "account")
                        return;

                    if (context.Depth > 1)
                        return;

                    //creamos el nombre
                    if (entity.Attributes.Contains("efk_primerapellido") || entity.Attributes.Contains("efk_segundoapellido") ||
                        entity.Attributes.Contains("efk_nombre_persona") || entity.Attributes.Contains("efk_tipo_cliente"))
                    {
                        //quiere decir que ha cambiado uno de esos campo, por lo tanto generamos el nombre
                        //validamos primero que existan los datos
                        Guid accountid = context.PrimaryEntityId;
                        string primerApellido = "";
                        string segundoApellido = "";
                        string nombre = "";
                        int tipoCliente = 0;

                        ColumnSet columnas = new ColumnSet("efk_primerapellido", "efk_segundoapellido", "efk_nombre_persona", "efk_tipo_cliente");

                        Entity cliente = service.Retrieve("account", accountid, columnas);

                        if (entity.Attributes.Contains("efk_tipo_cliente"))
                            tipoCliente = ((OptionSetValue)entity.Attributes["efk_tipo_cliente"]).Value;
                        else
                            if (cliente.Attributes.Contains("efk_tipo_cliente"))
                                tipoCliente = ((OptionSetValue)cliente.Attributes["efk_tipo_cliente"]).Value;

                        
                        if (tipoCliente == ClienteNatural)
                        {
                            if (entity.Attributes.Contains("efk_primerapellido") && entity.Attributes["efk_primerapellido"]!=null)
                                primerApellido = entity.Attributes["efk_primerapellido"].ToString();
                            else
                            {
                                if (cliente.Attributes.Contains("efk_primerapellido") && cliente.Attributes["efk_primerapellido"] != null)
                                    primerApellido = cliente.Attributes["efk_primerapellido"].ToString();
                            }

                            if (entity.Attributes.Contains("efk_segundoapellido") && entity.Attributes["efk_segundoapellido"] != null)
                                segundoApellido = entity.Attributes["efk_segundoapellido"].ToString();
                            else
                                if (cliente.Attributes.Contains("efk_segundoapellido") && cliente.Attributes["efk_segundoapellido"] != null)
                                    segundoApellido = cliente.Attributes["efk_segundoapellido"].ToString();

                            if (entity.Attributes.Contains("efk_nombre_persona") && entity.Attributes["efk_nombre_persona"] != null)
                                nombre = entity.Attributes["efk_nombre_persona"].ToString();
                            else
                                if (cliente.Attributes.Contains("efk_nombre_persona") && cliente.Attributes["efk_nombre_persona"] != null)
                                    nombre = cliente.Attributes["efk_nombre_persona"].ToString();

                            string nombreCompleto = "";

                            if (primerApellido != "")
                                nombreCompleto += primerApellido;
                            if (segundoApellido != "")
                                nombreCompleto += " " + segundoApellido;
                            if (nombre != "")
                            {
                                if (nombreCompleto != "")
                                    nombreCompleto += ", " + nombre;
                                else
                                    nombreCompleto = nombre;
                            }

                            if (nombreCompleto != "")
                                cliente.Attributes.Add("name", nombreCompleto);
                            else
                                cliente.Attributes["name"] = null;

                            service.Update(cliente);
                        }
                        else
                        {
                            //Borramos los datos
                            cliente.Attributes["efk_primerapellido"] = null;
                            cliente.Attributes["efk_segundoapellido"] = null;
                            cliente.Attributes["efk_nombre_persona"] = null;

                            service.Update(cliente);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace(this.GetType().ToString() + "_PreCreate: {0} ", ex.ToString());
                throw;
            }
        }
    }
}
