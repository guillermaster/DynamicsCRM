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

namespace Efika.Crm.Plugins.SFA.Oportunidad
{
    public class GeneraNroSolicitud: IPlugin
    {
        #region Variables
        private static string strFetchXmlCliente;                
        private string FetchXmlObtenerNroSolicitud
        {
            get
            {
                return
                    @"<fetch distinct='false' mapping='logical' aggregate='true'> 
                        <entity name='opportunity'> 
                           <attribute name='efk_nrosolicitud' alias='efk_nrosolicitud_max' aggregate='max' /> 
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
            IOrganizationService servicio = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(null);

            // Get a reference to the tracing service.
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));           
            
            Guid oportunidadid = ((Guid)context.OutputParameters["id"]);
            long aggregate5 = 0;
            long aggregate4 = 0;

            bool pendiente_recalcular = false;
            bool oferta_cerrada = false;
            bool solicitud_enviada_fabrica = false;
            bool solicitud_score_evaluado = false;
            bool lista_para_recalcular = false;  

            #region Verify execution context
            if ((context.GetType().GetProperty("CallerOrigin").GetValue(context, null).GetType().Name != "ApplicationOrigin") &&
                (context.GetType().GetProperty("CallerOrigin").GetValue(context, null).GetType().Name != "WebServiceApiOrigin"))
                return;

                EntityCollection estimatedvalue_max_result = servicio.RetrieveMultiple(new FetchExpression(FetchXmlObtenerNroSolicitud));

                if (estimatedvalue_max_result.Entities.Count > 0)
                {
                    foreach (var c in estimatedvalue_max_result.Entities)
                    {
                        if (c.Attributes.Contains("efk_nrosolicitud_max"))
                        {
                            aggregate5 = ((Int32)((AliasedValue)c["efk_nrosolicitud_max"]).Value);
                        }
                    }
                }          

                QueryExpression qbainicial_result = new QueryExpression();
                qbainicial_result.EntityName = "efk_paramtero_simulacion_crediticia";
                qbainicial_result.ColumnSet = new ColumnSet();
                string[] columnasOfertaValor = new string[] { "efk_paramtero_simulacion_crediticiaid", "efk_valor_entero","efk_name"};
                qbainicial_result.ColumnSet.AddColumns(columnasOfertaValor);
                qbainicial_result.Criteria.AddCondition(new ConditionExpression("efk_name", ConditionOperator.In, new string[] { "numero_solicitud_inicial"}));       

                EntityCollection retrievedOfertaValor = servicio.RetrieveMultiple(qbainicial_result);

                if (retrievedOfertaValor.Entities.Count > 0)
                {
                    for (int i = 0; i < retrievedOfertaValor.Entities.Count; i++)
                    {
                        string valor = retrievedOfertaValor.Entities[i]["efk_valor_entero"].ToString();
                        if (!String.IsNullOrWhiteSpace(valor)) aggregate4 = Int32.Parse(valor);
                    }
                }

                //******** ene ste plugin aparte de poner el numero de solicitud, se pondra la validacion de :
                // ---------  para una oprotunidad de "tipo de familia de producto" es activo, y el tipo de cliente es Dependiente.
                // ---------  Aplicar las siguientes condiciones: 
                //*** Al momento de generar oportunidad todas las oportunidades naceran con efk_pendiente_recalcular=TRUE, efk_oferta_cerrada=FALSE, 
                //efk_solicitud_enviada_fabrica=FALSE, efk_solicitud_score_evaluado=FALSE.  Pero únicamente la que tendrá el orden 1 tendrá efk_lista_para_recalcular=TRUE,
                //las demás tendrán efk_lista_para_recalcular=FALSE.

                    strFetchXmlCliente = string.Empty;

                    strFetchXmlCliente = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                        <entity name=""account"">
                                            <attribute name=""accountid"" />
                                            <attribute name=""efk_fuente_ingresos_ov"" />
                                            <order attribute=""name"" descending=""false"" />
                                            <filter type=""and"">
                                                <condition attribute=""accountid"" operator=""eq"" value=""{0}"" />
                                            </filter>
                                        </entity>
                                        </fetch>";

                    if (context.PostEntityImages.Contains("post_image_enti"))
                    {
                        if (context.PostEntityImages["post_image_enti"].Attributes.ContainsKey("customerid") &&
                             context.PostEntityImages["post_image_enti"].Attributes.ContainsKey("efk_tipo_familia_producto") &&
                              context.PostEntityImages["post_image_enti"].Attributes.ContainsKey("efk_orden"))
                        {
                            string CuentaId = ((EntityReference)context.PostEntityImages["post_image_enti"].Attributes["customerid"]).Id.ToString();
                            EntityCollection ecCliente = null;
                            strFetchXmlCliente = string.Format(strFetchXmlCliente, CuentaId);
                            ecCliente = service.RetrieveMultiple(new FetchExpression(strFetchXmlCliente));
                            int flagFunete = 0;

                            foreach (Entity eCliente in ecCliente.Entities)
                            {
                                flagFunete = ((OptionSetValue)(eCliente.Attributes["efk_fuente_ingresos_ov"])).Value;
                            }

                            if ((flagFunete == 221220001) && (((OptionSetValue)context.PostEntityImages["post_image_enti"].Attributes["efk_tipo_familia_producto"]).Value == 221220000))
                            {
                                pendiente_recalcular = true;
                                oferta_cerrada = false;
                                solicitud_enviada_fabrica = false;
                                solicitud_score_evaluado = false;

                                if (context.PostEntityImages["post_image_enti"].Attributes["efk_orden"].ToString() == "1")
                                    lista_para_recalcular = true;
                                else
                                    lista_para_recalcular = false;
                            }

                        }
                    }


                    if (aggregate5 > 0)
                    {
                        ActualizarNroSolicitud(oportunidadid, service, aggregate5 + 1, pendiente_recalcular, oferta_cerrada, 
                            solicitud_enviada_fabrica, solicitud_score_evaluado, lista_para_recalcular);
                    }
                    else
                    {
                        ActualizarNroSolicitud(oportunidadid, service, aggregate4, pendiente_recalcular, oferta_cerrada,
                            solicitud_enviada_fabrica, solicitud_score_evaluado, lista_para_recalcular);
                    }

            
            #endregion

        }


        private static void ActualizarNroSolicitud(Guid idoportunidad, IOrganizationService servicio, long mensaje,
            bool pendiente_recalcular, bool oferta_cerrada, bool solicitud_enviada_fabrica, bool solicitud_score_evaluado, bool lista_para_recalcular)
        {
            ColumnSet columnas = new ColumnSet("opportunityid", "efk_nrosolicitud");
            Entity oportunidad = servicio.Retrieve("opportunity", idoportunidad, columnas);

            if (oportunidad.Attributes.Contains("efk_nrosolicitud"))
            {
                oportunidad.Attributes["efk_nrosolicitud"] = mensaje;
            }
            else
            {
                oportunidad.Attributes.Add("efk_nrosolicitud", mensaje);
            }
            //--------------------------------------------------------------------------
            if (oportunidad.Attributes.Contains("efk_pendiente_recalcular"))
            {
                oportunidad.Attributes["efk_pendiente_recalcular"] = pendiente_recalcular;
            }
            else
            {
                oportunidad.Attributes.Add("efk_pendiente_recalcular", pendiente_recalcular);
            }
            //-----------------------------------------------------------------------------
            if (oportunidad.Attributes.Contains("efk_oferta_cerrada"))
            {
                oportunidad.Attributes["efk_oferta_cerrada"] = oferta_cerrada;
            }
            else
            {
                oportunidad.Attributes.Add("efk_oferta_cerrada", oferta_cerrada);
            }
            //-----------------------------------------------------------------------------
            if (oportunidad.Attributes.Contains("efk_solicitud_enviada_fabrica"))
            {
                oportunidad.Attributes["efk_solicitud_enviada_fabrica"] = solicitud_enviada_fabrica;
            }
            else
            {
                oportunidad.Attributes.Add("efk_solicitud_enviada_fabrica", solicitud_enviada_fabrica);
            }
            //-----------------------------------------------------------------------------
            if (oportunidad.Attributes.Contains("efk_solicitud_score_evaluado"))
            {
                oportunidad.Attributes["efk_solicitud_score_evaluado"] = solicitud_score_evaluado;
            }
            else
            {
                oportunidad.Attributes.Add("efk_solicitud_score_evaluado", solicitud_score_evaluado);
            }
            //-----------------------------------------------------------------------------
            if (oportunidad.Attributes.Contains("efk_lista_para_recalcular"))
            {
                oportunidad.Attributes["efk_lista_para_recalcular"] = lista_para_recalcular;
            }
            else
            {
                oportunidad.Attributes.Add("efk_lista_para_recalcular", lista_para_recalcular);
            }
 
            servicio.Update(oportunidad);
        }

    }
}
