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
    public sealed class WAAsignarLlamadasFugaViviendaPorCuaotaPagada : CodeActivity
    {
        #region Input Properties
        [Input("Task Subject")]
        [Default("Empty Subject")]
        public InArgument<string> TaskSubject { get; set; }
        #endregion

        enum tipoParametros
        {
            maximo_clientes_fuga_vivienda = 221220011,
            cuota_fuga_vivienda = 221220012,
            periodo_generar_nueva_alarma = 221220013
        }

        enum claseProductoBanco
        {
            credito_vehicular_hipotecario = 221220001
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
                                <filter type=""or"">
                                  <condition attribute=""efk_tipo_parametro"" operator=""eq"" value=""{2}"" />
                                  <condition attribute=""efk_tipo_parametro"" operator=""eq"" value=""{3}"" />
                                  <condition attribute=""efk_tipo_parametro"" operator=""eq"" value=""{4}"" />
                                </filter>
                            </filter>
                          </entity>
                     </fetch>";

            }
        }        

        private string fetchProducto
        {
            get
            {
                return
                    @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                          <entity name=""efk_producto_activo"">
                            <attribute name=""{0}"" />
                            <order attribute=""efk_monto_desembolsado"" descending=""true"" />
                            <filter type=""and"">
                              <condition attribute=""efk_clase_producto_banco"" operator=""eq"" value=""{1}"" />
                              <condition attribute=""efk_ultima_cuota_pagada"" operator=""ge"" value=""{2}"" />
                            </filter>
                          </entity>
                     </fetch>";
            }
        }


        protected override void Execute(CodeActivityContext executionContext)
        {            
            decimal maximoClientes;
            decimal cuotaFugaVivienda;
            int differenceInDays;
            decimal numDiasParaNvaAlarma;
            IWorkflowContext workflowContext = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory ServiceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService servicio = ServiceFactory.CreateOrganizationService(workflowContext.UserId);
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            string newSubject = "Alarma de fuga de vivienda (cuota 11 pagada)";//;TaskSubject.Get<string>(executionContext);

            GetParametros(servicio, out maximoClientes, out cuotaFugaVivienda, out numDiasParaNvaAlarma);

            if (maximoClientes != -1 && cuotaFugaVivienda != -1 && numDiasParaNvaAlarma != -1)
            {
                Entity entCliente;
                Entity entProducto;
                int contadorcliente = 0;
                int i = 0;
                string attributeNameProdIdCliente = "efk_cliente_juridico_id";
                //consultar productos
                EntityCollection entProductosCuotaFuga = servicio.RetrieveMultiple(new FetchExpression(
                    string.Format(fetchProducto, attributeNameProdIdCliente,
                    "221220001",//claseProductoBanco.credito_vehicular_hipotecario.ToString(), 
                    "221220012")));//tipoParametros.cuota_fuga_vivienda.ToString())));

                //recorrer los clientes hasta llegar al máximo permitido para generar las actividades de gestión
                while (contadorcliente < entProductosCuotaFuga.Entities.Count && contadorcliente < maximoClientes)
                {
                    entProducto = entProductosCuotaFuga.Entities[contadorcliente];
                    entCliente = GetCliente(servicio, new Guid(entProducto.Attributes[attributeNameProdIdCliente].ToString()));

                    if (entCliente.Attributes.Contains("efk_procesado_alarma_fuga_vivienda"))
                    {
                        differenceInDays = GetNumDiasDesdeUltimaAlarmaFuga(entCliente, Convert.ToInt16(numDiasParaNvaAlarma));

                        if (differenceInDays >= numDiasParaNvaAlarma && entCliente.Attributes.Contains("telephone1"))
                        {
                            CrearLlamada(servicio, entCliente, newSubject);
                            //establecer hoy como fecha de última generación de alarma de fuga de vivienda
                            entCliente.Attributes["efk_procesado_alarma_fuga_vivienda"] = DateTime.Now;
                            servicio.Update(entCliente);

                            contadorcliente++;//incrementar contador solo luego de crear una llamada para gestión de la fuga de vivienda
                        }
                    }
                    i++;
                }
            }
            else
            {
                if (maximoClientes != -1)
                {
                    throw new Exception("No se pudo obtener parámetro de máximo de clientes a procesar por fuga de vivienda {WAAsignarLlamadasFugaViviendaPorCuotaPagada}");
                }
                if (cuotaFugaVivienda != -1)
                {
                    throw new Exception("No se pudo obtener parámetro de cuota de fuga de vivienda {WAAsignarLlamadasFugaViviendaPorCuotaPagada}");
                }
                if (numDiasParaNvaAlarma != -1)
                {
                    throw new Exception("No se pudo obtener parámetro de lapso de días para generar nueva alarma {WAAsignarLlamadasFugaViviendaPorCuotaPagada}");
                }  
            }
        }


        private void GetParametros(IOrganizationService servicio, out decimal maxClientes, out decimal cuotaFugaVivienda, out decimal diasNvaAlarma)
        {
            string fieldNameTipoPar = "efk_tipo_parametro";
            string fieldNameValue = "efk_valor";
            maxClientes = -1;
            cuotaFugaVivienda = -1;
            diasNvaAlarma = -1;
            int valTipoPar;
            Microsoft.Xrm.Sdk.OptionSetValue tipoParOptionSetVal;

            EntityCollection entParametros = servicio.RetrieveMultiple(new FetchExpression(
                string.Format(fetchParametros, fieldNameTipoPar, fieldNameValue, "221220011",//tipoParametros.maximo_clientes_fuga_vivienda.ToString(), 
                "221220012",//tipoParametros.cuota_fuga_vivienda.ToString(), 
                "221220013")));//tipoParametros.periodo_generar_nueva_alarma.ToString())));
            
            foreach (Entity eParametro in entParametros.Entities)
            {
                tipoParOptionSetVal = (Microsoft.Xrm.Sdk.OptionSetValue)eParametro.Attributes[fieldNameTipoPar];
                valTipoPar = tipoParOptionSetVal.Value;

                if (valTipoPar == 221220011)//int.Parse(tipoParametros.maximo_clientes_fuga_vivienda.ToString()))
                {
                    maxClientes = Convert.ToDecimal(eParametro.Attributes[fieldNameValue].ToString());
                }

                if (valTipoPar == 221220012)//int.Parse(tipoParametros.cuota_fuga_vivienda.ToString()))
                {
                    cuotaFugaVivienda = Convert.ToDecimal(eParametro.Attributes[fieldNameValue].ToString());
                }

                if (valTipoPar == 221220013)//int.Parse(tipoParametros.periodo_generar_nueva_alarma.ToString()))
                {
                    diasNvaAlarma = Convert.ToDecimal(eParametro.Attributes[fieldNameValue].ToString());
                }
            }
        }

        private Entity GetCliente(IOrganizationService servicio, Guid idCliente)
        {
            ColumnSet cols = new ColumnSet();
            cols.AddColumns("ownerid", "telephone1", "efk_procesado_alarma_fuga_vivienda", "accountid");
            Entity entCliente = servicio.Retrieve("account", idCliente, cols);
            return entCliente;
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
