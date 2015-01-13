using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Client;

namespace Efika.Crm.Plugins.Margen_Credito
{
    public class ValidarRelacionOperacionMargenCredito : IPlugin
    {
        private const string fieldNameRelationShipMargCred = "efk_margen_creditoid";
        private const string entLogNameMargenCredito = "efk_margen_credito";

        public void Execute(IServiceProvider serviceProvider)
        {
            
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            
            try
            {
                Entity entity = (Entity)context.InputParameters["Target"];

                if (entity.LogicalName != "efk_producto_activo")
                    return;

                validarEstadoMargenCredito(service, entity);
                validarRelacionProductoActivoLineaCredito(service, entity);
               
                //Validar el cliente.

                //Obtenemos la referencia al margen de crédito
                if (entity.Attributes.Contains("efk_margen_creditoid") && entity.Attributes["efk_margen_creditoid"]!=null)
                {
                    //Con esto asumimos que se ha actualizado el margen de crédito
                    EntityReference margenCreditoRef = (EntityReference)entity.Attributes["efk_margen_creditoid"];

                    //Obtenemos el cliente del margen de crédito
                    Entity margenCredito = service.Retrieve("efk_margen_credito", margenCreditoRef.Id, new ColumnSet("efk_accountid"));

                    //Obtenemos el cliente del producto del activo
                    Entity productoActivo = service.Retrieve("efk_producto_activo", context.PrimaryEntityId, new ColumnSet("efk_cliente_juridico_id","efk_clase_producto_banco"));

                    //Validamos que el producto no sea una garantía
                    if (productoActivo.Attributes.Contains("efk_clase_producto_banco") && productoActivo.Attributes["efk_clase_producto_banco"] != null)
                    {
                        if (((OptionSetValue)productoActivo.Attributes["efk_clase_producto_banco"]).Value == 221220005)
                        {
                            throw new InvalidPluginExecutionException("No puede asociar una garantía como operación del márgen de crédito.");
                        }
                    }

                    if (margenCredito.Attributes.Contains("efk_accountid") && productoActivo.Attributes.Contains("efk_cliente_juridico_id"))
                    {
                        if (((EntityReference)margenCredito.Attributes["efk_accountid"]).Id !=
                                ((EntityReference)productoActivo.Attributes["efk_cliente_juridico_id"]).Id)
                        {
                            throw new InvalidPluginExecutionException("No se puede asociar producto del activo porque no pertenece a este cliente.");
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                tracingService.Trace(this.GetType().ToString() + "_PreCreate {0}", ex.Message);
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }


        public void validarEstadoMargenCredito(IOrganizationService service, Entity entity)
        {
            const string fieldNameMargenCreditoEstado  = "efk_estado";
            const int fieldValueMargenCreditoEstadoCancelado = 221220001;

            if (entity.Attributes.Contains(fieldNameRelationShipMargCred) && entity[fieldNameRelationShipMargCred]!=null)
            {
                EntityReference margCredRef = (EntityReference)entity[fieldNameRelationShipMargCred];
                Entity margCredInst = service.Retrieve(entLogNameMargenCredito, margCredRef.Id, new ColumnSet(fieldNameMargenCreditoEstado));
                OptionSetValue optEstadoMargCred = (OptionSetValue)margCredInst.Attributes[fieldNameMargenCreditoEstado];
                //preguntar si el estado actual es igual a cancelado (221220001)
                if (optEstadoMargCred.Value == fieldValueMargenCreditoEstadoCancelado)
                {
                    //no permitir registrar relación
                    throw new InvalidPluginExecutionException("No se puede asociar producto del activo porque el margen de crédito se encuentra en estado cancelado.");
                }
            }
        }

        /** Método para validar si el producto a relacionar no se encuentra relacionado con una línea de crédito **/
        public void validarRelacionProductoActivoLineaCredito(IOrganizationService service, Entity entity)
        {
            if (entity.Attributes.Contains("efk_margen_creditoid") && entity.Attributes["efk_margen_creditoid"] != null)
            {

                const string fieldNameRelationshipLineaCred = "efk_linea_creditoid";
                // buscar la instancia de la entidad producto activo en DB, y traer columna de relacion entre producto activo y línea de crédito
                Entity prodActFromDB = service.Retrieve(entity.LogicalName, entity.Id, new ColumnSet(fieldNameRelationshipLineaCred));

                if (prodActFromDB.Contains(fieldNameRelationshipLineaCred))
                    throw new InvalidPluginExecutionException("No se puede asociar producto del activo porque este se encuentra asociado a una línea de crédito.");
            }
        }

    }
}
