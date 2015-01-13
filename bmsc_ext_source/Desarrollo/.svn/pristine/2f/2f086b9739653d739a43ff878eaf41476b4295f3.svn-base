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



namespace Efika.Crm.Plugins.SFA
{
    public sealed class WAAsignarLlamadasFugaVivienda : CodeActivity
    {
        #region Input Properties
        [Input("Task Subject")]
        [Default("Empty Subject")]
        public InArgument<string> TaskSubject { get; set; }


        #endregion

        enum tipoParametros
        {
            probabilidad_maxima_fuga_por_producto = 221220003,
            umbral_alarma = 221220005,
            maximo_clientes_fuga_vivienda = 221220011,
            periodo_generar_nueva_alarma = 221220013
        }

        private string fetchTipoProductoCreditoVivienda
        {
            get
            {
                return @"<fetch distinct=""false"" mapping=""logical"" output-format=""xml-platform"" version=""1.0"">
                            <entity name=""efk_tipo_producto"">
                                <attribute name=""{0}""/>
                                <filter type=""and"">
                                    <condition attribute=""efk_nombre"" value=""CRÉDITO DE VIVIENDA"" operator=""eq""/>
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
                            <filter type=""and"">
                              <condition attribute=""efk_tipodeproductoid"" operator=""eq"" value=""{2}"" />
                            </filter>
                          </entity>
                     </fetch>";

            }
        }

        private string fetchCliente
        {
            get
            {
                return
                    @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                          <entity name=""account"">
                            <attribute name=""name"" />
                            <attribute name=""accountid"" />
                            <attribute name=""efk_alarma_de_vivienda"" />
                            <attribute name=""efk_probabilidad_fuga_vivienda"" />
                            <attribute name=""efk_procesado_alarma_fuga_vivienda"" />
                            <attribute name=""telephone1"" />
                            <attribute name=""ownerid"" />
                            <order attribute=""efk_probabilidad_fuga_vivienda"" descending=""true"" />
                            <filter type=""and"">
                              <condition attribute=""efk_alarma_de_vivienda"" operator=""gt"" value=""{0}"" />
                              <condition attribute=""efk_probabilidad_fuga_vivienda"" operator=""gt"" value=""{1}"" />
                            </filter>
                          </entity>
                        </fetch>";
            }
        }
              

        protected override void Execute(CodeActivityContext executionContext)
        {
            decimal valorumbral = 0;
            decimal valorprobabilidad = 0;            
            decimal maximoclientes = 0;
            decimal numDiasParaNvaAlarma;
            Guid idTipoProdCredVivienda;
            IWorkflowContext workflowContext = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory ServiceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService servicio = ServiceFactory.CreateOrganizationService(workflowContext.UserId);
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            string newSubject = "Alarma de fuga vivienda";//TaskSubject.Get<string>(executionContext);

            idTipoProdCredVivienda = GetIdTipoProdCredVivienda(servicio);//obtener el GUID del tipo de producto crédito de vivienda

            if (idTipoProdCredVivienda != Guid.Empty)
            {
                GetParametros(servicio, idTipoProdCredVivienda, out valorumbral, out valorprobabilidad, out maximoclientes, out numDiasParaNvaAlarma);//obtener parámetros a usarse abajo

                if (valorumbral != -1 && valorprobabilidad != -1 && maximoclientes != -1 && numDiasParaNvaAlarma != -1)
                {
                    //leer clientes que sobrepasen los máximos permitidos
                    string fetch = string.Format(fetchCliente, valorumbral, valorprobabilidad);
                    
                    EntityCollection entClientes = servicio.RetrieveMultiple(new FetchExpression(fetch));
                    int contadorcliente = 0;
                    int i = 0;
                    int differenceInDays;
                    Entity entCliente;
                    //recorrer los clientes hasta llegar al máximo permitido para generar las actividades de gestión
                    while (i < entClientes.Entities.Count && contadorcliente < maximoclientes)
                    {
                        entCliente = entClientes.Entities[contadorcliente];

                        differenceInDays = GetNumDiasDesdeUltimaAlarmaFuga(entCliente, Convert.ToInt16(numDiasParaNvaAlarma));
                        //throw new Exception("differenceInDays: " + differenceInDays.ToString() + "numDiasParaNvaAlarma: " + numDiasParaNvaAlarma + "telefono:" + entCliente.Attributes.Contains("telephone1").ToString());
                        //si han pasado más de 60 días desde la última alarma de fuga de vivienda generada para un cliente
                        if (differenceInDays >= numDiasParaNvaAlarma && entCliente.Attributes.Contains("telephone1"))
                        {
                            CrearLlamada(servicio, entCliente, newSubject);

                            //establecer hoy como fecha de última generación de alarma de fuga de vivienda
                            entCliente.Attributes["efk_procesado_alarma_fuga_vivienda"] = DateTime.Now;
                            servicio.Update(entCliente);

                            contadorcliente++;//incrementar contador solo luego de crear una llamada para gestión de la fuga de vivienda
                        }
                        i++;
                    }
                }
                else
                {
                    if(valorumbral != -1)
                    {
                        throw new Exception("No se pudo obtener parámetro de valor del umbral de fuga de vivienda en {WAAsignarLlamadasFugaVivienda}");
                    }
                    if(valorprobabilidad != -1)
                    {
                        throw new Exception("No se pudo obtener parámetro de valor de la probabilidad máxima de fuga de vivienda {WAAsignarLlamadasFugaVivienda}");
                    }
                    if(maximoclientes != -1)
                    {
                        throw new Exception("No se pudo obtener parámetro de máximo de clientes a procesar por fuga de vivienda {WAAsignarLlamadasFugaVivienda}");
                    }
                    if (numDiasParaNvaAlarma != -1)
                    {
                        throw new Exception("No se pudo obtener parámetro de lapso de días para generar nueva alarma {WAAsignarLlamadasFugaViviendaPorCuotaPagada}");
                    }  
                }
            }
            else
            {
                throw new Exception("No se pudo obtener el tipo de producto en {WAAsignarLlamadasFugaVivienda}");
            }
        }


        private Guid GetIdTipoProdCredVivienda(IOrganizationService servicio)
        {
            Guid idTipoProdCredViv = Guid.Empty;
            string fieldName = "efk_tipo_productoid";
            EntityCollection entTiposProductos = servicio.RetrieveMultiple(new FetchExpression(string.Format(fetchTipoProductoCreditoVivienda, fieldName)));

            if (entTiposProductos.Entities.Count > 0)
            {
                idTipoProdCredViv = new Guid(entTiposProductos.Entities[0].Attributes[fieldName].ToString());
            }

            return idTipoProdCredViv;
        }


        private void GetParametros(IOrganizationService servicio, Guid idTipoProducto, out decimal valorUmbral, out decimal valorMaxProbabilidad, 
            out decimal maximoClientes, out decimal diasNvaAlarma)
        {
            string fieldNameTipoPar = "efk_tipo_parametro";
            string fieldNameValue = "efk_valor";
            int valTipoPar;
            Microsoft.Xrm.Sdk.OptionSetValue tipoParOptionSetVal;

            valorUmbral = -1;
            valorMaxProbabilidad = -1;
            maximoClientes = -1;
            diasNvaAlarma = -1;

            EntityCollection entParametros = servicio.RetrieveMultiple(new FetchExpression(string.Format(fetchParametros, fieldNameTipoPar, fieldNameValue, idTipoProducto.ToString())));

            foreach (Entity eParametro in entParametros.Entities)
            {
                //throw new Exception("efk_tipo_parametro=" + eParametro.Attributes[fieldNameTipoPar].ToString());
                tipoParOptionSetVal = (Microsoft.Xrm.Sdk.OptionSetValue) eParametro.Attributes[fieldNameTipoPar];
                valTipoPar = tipoParOptionSetVal.Value;

                if (valTipoPar == 221220005)//int.Parse(tipoParametros.umbral_alarma.ToString()))
                {
                    valorUmbral = Convert.ToDecimal(eParametro.Attributes[fieldNameValue].ToString());
                }

                if (valTipoPar == 221220011)//int.Parse(tipoParametros.maximo_clientes_fuga_vivienda.ToString()))
                {
                    maximoClientes = Convert.ToDecimal(eParametro.Attributes[fieldNameValue].ToString());
                }

                if (valTipoPar == 221220003)//int.Parse(tipoParametros.probabilidad_maxima_fuga_por_producto.ToString()))
                {
                    valorMaxProbabilidad = Convert.ToDecimal(eParametro.Attributes[fieldNameValue].ToString());
                }

                if (valTipoPar == 221220013)//int.Parse(tipoParametros.periodo_generar_nueva_alarma.ToString()))
                {
                    diasNvaAlarma = Convert.ToDecimal(eParametro.Attributes[fieldNameValue].ToString());
                }
            }
        }

        private int GetNumDiasDesdeUltimaAlarmaFuga(Entity entCliente, int defaultValue)
        {
            int differenceInDays;
            try
            {
                DateTime oldDate = Convert.ToDateTime(entCliente.Attributes["efk_procesado_alarma_fuga_vivienda"].ToString());
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


        private void CrearLlamada(IOrganizationService servicio, Entity entCliente, string subject)
        {
            Guid idCliente = new Guid(entCliente.Attributes["accountid"].ToString());

            Entity newTask = new Entity("phonecall");
            newTask["regardingobjectid"] = new EntityReference("account", idCliente);
            newTask["phonenumber"] = entCliente.Attributes["telephone1"];
            newTask["ownerid"] = (EntityReference)entCliente.Attributes["ownerid"];//el propietario de la actividad será el propietario del cliente
            newTask["subject"] = subject;
            newTask["scheduledstart"] = DateTime.Now;
            newTask["scheduledend"] = DateTime.Now.AddDays(5);

            Entity newActivity = new Entity("activityparty");
            newActivity["partyid"] = (EntityReference)entCliente.Attributes["ownerid"];
            newTask["from"] = new EntityCollection(new[] { newActivity });//relate to phonecall

            Entity newActivity1 = new Entity("activityparty");
            newActivity1["partyid"] = new EntityReference("account", idCliente);
            newTask["to"] = new EntityCollection(new[] { newActivity1 });//relate to phonecall

            //servicio.Create(newTask);
            Guid idTask = servicio.Create(newTask);
        }
    }
}
