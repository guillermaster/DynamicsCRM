using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;

namespace Efika.Crm.Plugins.SFA.Cliente
{
    public class ActualizaFechaDatosModeloEvaluador : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            IOrganizationService iServices = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(context.UserId));
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            try
            {
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity && context.Depth == 1)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "account") return;

                    if (
                            entity.Attributes.Contains("efk_salario_liquido_titularmes1") ||
                            entity.Attributes.Contains("efk_salario_liquido_titularmes2") ||
                            entity.Attributes.Contains("efk_salario_liquido_titularmes3") ||
                            entity.Attributes.Contains("efk_salario_liquido_conyuguemes1") ||
                            entity.Attributes.Contains("efk_salario_liquido_conyuguemes2") ||
                            entity.Attributes.Contains("efk_salario_liquido_conyuguemes3") ||
                            entity.Attributes.Contains("efk_ingresos_mensuales_alquileres_titular") ||
                            entity.Attributes.Contains("efk_ingresos_mensuales_alquileres_conyugue") ||
                            entity.Attributes.Contains("efk_ingresos_anuales_abonos_titular") ||
                            entity.Attributes.Contains("efk_ingresos_anuales_abonos_conyugue") || 
                            entity.Attributes.Contains("efk_hipotecario_cuotas_respaldadas") ||
                            entity.Attributes.Contains("efk_hipotecario_cuotas_cartera") ||
                            entity.Attributes.Contains("efk_hipotecario_cuotas_ddjj") ||
                            entity.Attributes.Contains("efk_consumo_cuotas_respaldadas") ||
                            entity.Attributes.Contains("efk_consumo_saldo_cartera") ||
                            entity.Attributes.Contains("efk_consumo_cuotas_ddjj") ||
                            entity.Attributes.Contains("efk_consumotdc_cuotas_respaldadas") ||
                            entity.Attributes.Contains("efk_consumotdc_saldo_cartera") ||
                            entity.Attributes.Contains("efk_consumotdc_cuotas_ddj") ||
                            entity.Attributes.Contains("efk_comercialpyme_cuotas_respaldadas") ||
                            entity.Attributes.Contains("efk_comercialpyme_saldo_cartera") ||
                            entity.Attributes.Contains("efk_comercialpyme_cuotas_ddjj") ||
                            entity.Attributes.Contains("efk_microcredito_cuotas_respaldadas") ||
                            entity.Attributes.Contains("efk_microcredito_saldo_cartera") ||
                            entity.Attributes.Contains("efk_microcredito_cuotas_ddjj") ||
                            entity.Attributes.Contains("efk_deuda_empresa_empleadora") ||
                            entity.Attributes.Contains("efk_cuotas_bmsc") ||
                            entity.Attributes.Contains("efk_cuotas_bmsc_tramite") ||
                            entity.Attributes.Contains("efk_cuotas_credito_compra") ||
                            entity.Attributes.Contains("efk_gastos_personales") ||
                            entity.Attributes.Contains("efk_peor_calificacion_12meses") ||
                            entity.Attributes.Contains("efk_nroveces_calificacion_distintaa") ||
                            entity.Attributes.Contains("efk_nroveces_vencido_ejecucion") ||
                            entity.Attributes.Contains("efk_nroveces_vigente2_14dias") ||
                            entity.Attributes.Contains("efk_valor_liquidable_garantias_constituidas") ||
                            entity.Attributes.Contains("efk_total_cartera_garantia_hipotecaria") ||
                            entity.Attributes.Contains("efk_total_cartera_garantia_personal") ||
                            entity.Attributes.Contains("efk_total_cartera_asolafirma") ||
                            entity.Attributes.Contains("efk_nroveces_vencido_ejecucion_titular") ||
                            entity.Attributes.Contains("efk_nroveces_vencido_ejecucion_garante")
                        )
                    {
                        Entity cliente = new Entity("account");
                        cliente.Id = context.PrimaryEntityId;
                        cliente.Attributes.Add("efk_fecha_ultima_actualizacion_mod_evaluador", DateTime.Now);

                        iServices.Update(cliente);
                        return;
                    }                
                }
            }
            catch (System.ServiceModel.FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("Clientes_PostUpdate plug-in: " + ex.Message.ToString(), ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("Clientes_PostUpdate plug-in: {0}", ex.ToString());
                throw;
            }
        }

    }
}
