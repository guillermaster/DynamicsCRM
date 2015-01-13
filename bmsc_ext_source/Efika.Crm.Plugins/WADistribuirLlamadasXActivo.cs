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
    public sealed  class WADistribuirLlamadasXActivo : CodeActivity
    {

        #region Input Properties
      

        #endregion


        private string fetchTiposProductos
        {
            get
            {
                return @"<fetch distinct=""false"" mapping=""logical"" output-format=""xml-platform"" version=""1.0"">
                            <entity name=""efk_tipo_producto"">
                                <attribute name=""{0}""/>
                                <filter type=""or"">
                                    <condition attribute=""efk_nombre"" value=""{1}"" operator=""eq""/>
                                    <condition attribute=""efk_nombre"" value=""{2}"" operator=""eq""/>
                                    <condition attribute=""efk_nombre"" value=""{3}"" operator=""eq""/>
                                    <condition attribute=""efk_nombre"" value=""{4}"" operator=""eq""/>
                                    <condition attribute=""efk_nombre"" value=""{5}"" operator=""eq""/>
                                </filter>
                            </entity>
                        </fetch>";
            }
        }

        private string fetchParametros
        {
            get
            {
                return
                    @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                          <entity name=""efk_parametro_oferta_valor_micro"">
                            <attribute name=""{0}"" />
                            <attribute name=""{1}"" />
                            <filter type=""or"">
                              <condition attribute=""efk_tipo_parametro"" operator=""eq"" value=""{2}"" />
                              <condition attribute=""efk_tipo_parametro"" operator=""eq"" value=""{3}"" />
                              <condition attribute=""efk_tipo_parametro"" operator=""eq"" value=""{4}"" />
                            </filter>
                          </entity>
                     </fetch>";

            }
        }

        private string fetchOfertaValorMaxProbabilidad
        {
            get
            {
                return 
                 @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                      <entity name=""efk_oferta_valor"">
                        <attribute name=""efk_name"" />
                        <link-entity name=""account"" link-type=""outer"" visible=""false"" to=""efk_cliente_juridico_id"" from=""accountid""> 
                            <attribute name=""accountid""/> 
                        </link-entity>
                        <order attribute=""efk_probabilidad_aceptacion"" descending=""true"" />
                        <filter type=""and"">
                          <condition attribute=""efk_tipo_productos_id"" operator=""in"">
                            {0}
                          </condition>
                          <condition attribute=""efk_probabilidad_aceptacion"" operator=""gt"" value=""{1}"" />
                        </filter>
                      </entity>
                    </fetch>";
            }
        }
           

        protected override void Execute(CodeActivityContext executionContext)
        {

            IWorkflowContext workflowContext = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory ServiceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService servicio = ServiceFactory.CreateOrganizationService(workflowContext.UserId);
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();
            decimal probabMinimaParaLlamada;
            decimal diasNvaAlarma;
            decimal maximoLlamadas;
            
            //obtener los IDS de los tipos de productos los cuales se buscarán en las Ofertas de Valor
            ArrayList idsTiposProductos = GetIdsTiposProductos(servicio);

            //obtener el minimo de probabilidad que deben cumplir las ofertas de valor para generar llamadas automáticas, y demás parámetros
            GetParametros(servicio, out probabMinimaParaLlamada, out maximoLlamadas, out diasNvaAlarma);

            //obtener las ofertas de valor que cumplen con las condiciones (tipo de producto y probabilidad mínima llamada)
            EntityCollection entColOfertasValor = GetOfertasValorParaLlamadasAutomaticas(servicio, idsTiposProductos, probabMinimaParaLlamada);

            Entity entCliente;
            string subject;
            int i = 0;
            int nLlamadas = 0;
            int diasDesdeUltimaLlamada = 0;

            while (i < entColOfertasValor.Entities.Count && nLlamadas < maximoLlamadas)
            {
                AliasedValue accountAlias = (AliasedValue)entColOfertasValor.Entities[i].Attributes["account1.accountid"];//obtiene el id del cliente
                entCliente = GetCliente(servicio, new Guid(accountAlias.Value.ToString()));//obtener datos de cliente
                diasDesdeUltimaLlamada = GetNumDiasDesdeUltimaLlamada(entCliente, Convert.ToInt16(diasNvaAlarma));

                if (diasDesdeUltimaLlamada >= diasNvaAlarma)
                {
                    subject = "Oferta de Valor: " + entColOfertasValor.Entities[i].Attributes["efk_name"].ToString();//obtener nombre oferta de valor, será el subject de la llamada
                    GenerarLlamada(servicio, entCliente, subject);
                    nLlamadas++;
                }
                i++;
            }

            foreach (Entity entOfValor in entColOfertasValor.Entities)
            {
                AliasedValue accountAlias = (AliasedValue)entOfValor.Attributes["account1.accountid"];//obtiene el id del cliente
                entCliente = GetCliente(servicio, new Guid(accountAlias.Value.ToString()));
                subject = "Oferta de Valor: " + entOfValor.Attributes["efk_name"].ToString();
                GenerarLlamada(servicio, entCliente, subject);
            }               

        }


        private ArrayList GetIdsTiposProductos(IOrganizationService servicio)
        {
            ArrayList idsTiposProds = new ArrayList();
            string fieldName = "efk_tipo_productoid";
            string query = string.Format(fetchTiposProductos, fieldName,
                TiposProductos.TarjetaCredito, TiposProductos.CreditoComercial, TiposProductos.CreditoVehicular, TiposProductos.CreditoVivienda, TiposProductos.CreditoConsumo);
            
            EntityCollection entTiposProductos = servicio.RetrieveMultiple(new FetchExpression(query));

            foreach(Entity entTipoProd in entTiposProductos.Entities)
            {
                idsTiposProds.Add(entTipoProd.Attributes[fieldName].ToString());
            }

            return idsTiposProds;
        }


        private void GetParametros(IOrganizationService servicio, out decimal minProbab, out decimal maxLlamadas, out decimal diasNvaAlarma)
        {
            decimal valorParametro;
            string fieldNameValorParam = "efk_valor";
            string fieldNameTipoParam = "efk_tipo_parametro";
            int valTipoPar;
            Microsoft.Xrm.Sdk.OptionSetValue tipoParOptionSetVal;

            minProbab = 1;//significa 100% de probabilidad, lo que no retornaría ningún registro de la oferta de valor
            maxLlamadas = 0;
            diasNvaAlarma = 0;
            
            EntityCollection entParametros = servicio.RetrieveMultiple(new FetchExpression(string.Format(fetchParametros, fieldNameValorParam, fieldNameTipoParam,
                Parametros.ProbabilidadMinimaParaLlamada.ToString(), Parametros.PeriodoParaGenerarNuevaLlamada.ToString(), Parametros.MaximoLlamadas)));
            
            foreach (Entity entParam in entParametros.Entities)
            {
                tipoParOptionSetVal = (Microsoft.Xrm.Sdk.OptionSetValue)entParam.Attributes[fieldNameTipoParam];
                valTipoPar = tipoParOptionSetVal.Value;
                valorParametro = Convert.ToDecimal(entParam.Attributes[fieldNameValorParam].ToString());

                if (valTipoPar == Parametros.MaximoLlamadas)
                {
                    maxLlamadas = valorParametro;
                }
                else if (valTipoPar == Parametros.PeriodoParaGenerarNuevaLlamada)
                {
                    diasNvaAlarma = valorParametro;
                }
                else if (valTipoPar == Parametros.ProbabilidadMinimaParaLlamada)
                {
                    minProbab = valorParametro;
                }
            }
        }


        private EntityCollection GetOfertasValorParaLlamadasAutomaticas(IOrganizationService servicio, ArrayList idsTiposProds, decimal probMinimaLlamada)
        {
            string tiposProdsCondition = string.Empty;
            string query;
            
            //generar condiciones para filtrar consulta por tipos de productos especificados
            foreach (string idTipoProd in idsTiposProds)
            {                
                tiposProdsCondition += string.Format(@"<value uitype=""efk_tipo_producto"">{0}</value>", idTipoProd);
            }

            query = string.Format(fetchOfertaValorMaxProbabilidad, tiposProdsCondition, probMinimaLlamada.ToString());
            
            EntityCollection entities = servicio.RetrieveMultiple(new FetchExpression(query));

            return entities;
        }


        private Entity GetCliente(IOrganizationService servicio, Guid idcliente)
        {
            ColumnSet cols = new ColumnSet();
            cols.AddColumns("ownerid", "telephone1", "efk_procesado_llamada_ov");
            Entity entCliente = servicio.Retrieve("account", idcliente, cols);
            return entCliente;
        }


        private int GetNumDiasDesdeUltimaLlamada(Entity entCliente, int defaultValue)
        {
            int differenceInDays;
            try
            {
                DateTime oldDate = Convert.ToDateTime(entCliente.Attributes["efk_procesado_llamada_ov"].ToString());
                DateTime newDate = DateTime.Now;
                TimeSpan ts = newDate - oldDate;
                differenceInDays = ts.Days;
            }
            catch
            {
                differenceInDays = defaultValue;
            }

            return differenceInDays;
        }


        private void GenerarLlamada(IOrganizationService servicio, Entity entCliente, string subject)
        {
            Entity newTask = new Entity("phonecall");
            newTask["regardingobjectid"] = new EntityReference("account", entCliente.Id);

            if (entCliente.Attributes.Contains("telephone1"))
            {
                newTask["phonenumber"] = entCliente.Attributes["telephone1"];
            }

            newTask["scheduledstart"] = DateTime.Now;
            newTask["scheduledend"] = DateTime.Now.AddDays(5);
            newTask["ownerid"] = (EntityReference)entCliente.Attributes["ownerid"];
            newTask["subject"] = subject;

            Entity newActivity = new Entity("activityparty");
            newActivity["partyid"] = (EntityReference)entCliente.Attributes["ownerid"];
            newTask["from"] = new EntityCollection(new[] { newActivity });

            Entity newActivity1 = new Entity("activityparty");
            newActivity1["partyid"] = new EntityReference("account", entCliente.Id);
            newTask["to"] = new EntityCollection(new[] { newActivity1 });

            Guid taskId = servicio.Create(newTask);
        }


        private class TiposProductos
        {
            public static string TarjetaCredito { get { return "TARJETAS DE CRÉDITO"; } }
            public static string CreditoComercial { get { return "CRÉDITOS COMERCIALES"; } }
            public static string CreditoVivienda { get { return "CRÉDITO DE VIVIENDA"; } }
            public static string CreditoVehicular { get { return "CREDITO VEHÍCULAR"; } }
            public static string CreditoConsumo { get { return "CREDITO DE CONSUMO"; } }
        }

        private class Parametros
        {
            public static int ProbabilidadMinimaParaLlamada { get { return 221220014; } }
            public static int PeriodoParaGenerarNuevaLlamada { get { return 221220015; } }
            public static int MaximoLlamadas { get { return 221220016; } }
        }
    }
}
