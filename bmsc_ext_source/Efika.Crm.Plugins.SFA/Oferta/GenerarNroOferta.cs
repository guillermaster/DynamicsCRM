using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using System.Net;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Metadata;
using System.Threading;
using System.Xml.Linq;
using System.ServiceModel.Security;


namespace Efika.Crm.Plugins.SFA.Oferta
{
   public  class GenerarNroOferta : IPlugin
    {
        #region Properties
        private string FetchXmlObtenerNroOferta
        {
            get
            {
                return
                    @"<fetch distinct='false' mapping='logical' aggregate='true'> 
                        <entity name='efk_simulacion_crediticia'> 
                           <attribute name='efk_numero_oferta' alias='efk_nrooferta_max' aggregate='max' /> 
                        </entity> 
                    </fetch>";
            }
        }
        #endregion

        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            IOrganizationService servicio = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(context.UserId));

            // Get a reference to the tracing service.
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            Guid simulacionCrediticiaID = ((Guid)context.OutputParameters["id"]);
            int aggregate5 = 0;

            #region Verify execution context

            if ((context.GetType().GetProperty("CallerOrigin").GetValue(context, null).GetType().Name != "ApplicationOrigin") &&
                (context.GetType().GetProperty("CallerOrigin").GetValue(context, null).GetType().Name != "WebServiceApiOrigin"))
                return;

            aggregate5 = GetNroOfertaMax(servicio);
            
            if (aggregate5 > 0)
                ActualizarNroOferta(simulacionCrediticiaID, service, aggregate5 + 1);
            else
                ActualizarNroOferta(simulacionCrediticiaID, service, GetNroOfertaParametro(servicio));

            #endregion

        }

        private int GetNroOfertaParametro(IOrganizationService servicio)
        {

            QueryExpression qbainicial_result = new QueryExpression();
            qbainicial_result.EntityName = "efk_paramtero_simulacion_crediticia";
            qbainicial_result.ColumnSet = new ColumnSet();
            string[] columnasOfertaValor = new string[] { "efk_paramtero_simulacion_crediticiaid", "efk_valor_entero", "efk_name" };
            qbainicial_result.ColumnSet.AddColumns(columnasOfertaValor);
            qbainicial_result.Criteria.AddCondition(new ConditionExpression("efk_name", ConditionOperator.In, new string[] { "numero_oferta_inicial" }));

            int aggregate4 = 0;

            EntityCollection retrievedOfertaValor = servicio.RetrieveMultiple(qbainicial_result);

            if (retrievedOfertaValor.Entities.Count > 0)
            {
                for (int i = 0; i < retrievedOfertaValor.Entities.Count; i++)
                {
                    string valor = retrievedOfertaValor.Entities[i]["efk_valor_entero"].ToString();
                    if (!String.IsNullOrWhiteSpace(valor)) aggregate4 = int.Parse(valor);
                }
            }
            return aggregate4;
        }

        private int GetNroOfertaMax(IOrganizationService servicio)
        {
            EntityCollection estimatedvalue_max_result = servicio.RetrieveMultiple(new FetchExpression(this.FetchXmlObtenerNroOferta));
            int aggregate5 = 0;

            if (estimatedvalue_max_result.Entities.Count > 0)
            {
                foreach (var c in estimatedvalue_max_result.Entities)
                {
                    if (c.Attributes.Contains("efk_nrooferta_max"))
                    {
                        aggregate5 = ((int)((AliasedValue)c["efk_nrooferta_max"]).Value);
                    }
                }
            }

            return aggregate5;

        }

        private static void ActualizarNroOferta(Guid simulacionCrediticiaID, IOrganizationService servicio, int numero)
        {
            ColumnSet columnas = new ColumnSet("efk_numero_oferta","efk_name", "efk_simulacion_crediticiaid");
            Entity oportunidad = servicio.Retrieve("efk_simulacion_crediticia", simulacionCrediticiaID, columnas);
            string msg_nro_oferta = "Simulación oferta Nro. " + numero.ToString();  

            if (oportunidad.Attributes.Contains("efk_numero_oferta"))
            {
                oportunidad.Attributes["efk_numero_oferta"] = numero;
            }
            else
            {
                oportunidad.Attributes.Add("efk_numero_oferta", numero);
            }

            if (oportunidad.Attributes.Contains("efk_name"))
            {
                oportunidad.Attributes["efk_name"] = msg_nro_oferta;
            }
            else
            {
                oportunidad.Attributes.Add("efk_name", msg_nro_oferta);
            }

            servicio.Update(oportunidad);
        }


    }
}
