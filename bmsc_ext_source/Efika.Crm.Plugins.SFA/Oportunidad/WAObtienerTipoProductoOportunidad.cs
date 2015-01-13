using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ServiceModel;
using System.Collections;
// Microsoft Dynamics CRM namespace(s)
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Workflow;

namespace Efika.Crm.Plugins.SFA.Oportunidad
{
    public class WAObtienerTipoProductoOportunidad : CodeActivity
    {
        #region Parametros

        [Input("Id Oportunidad")]
        [ReferenceTarget("opportunity")]
        public InArgument<EntityReference> IdOportunidadd { get; set; }

        [Output("Id Tipo Producto")]
        [ReferenceTarget("efk_tipo_producto")]
        public OutArgument<EntityReference> IdTipoProducto { get; set; }

        #endregion

        #region Propiedades

        IOrganizationService ServicioCRM { get; set; }
        ITracingService tracingService = null;
        IWorkflowContext contexto = null;
        Guid producto_oportunidadID = Guid.Empty;
        Guid[] productoId = null;
        Guid[] tipoproductoId = null;

        #endregion

        #region Queries
        private string fetchProductosOportunidad = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                                      <entity name=""opportunityproduct"">
                                                        <attribute name=""productid"" />
                                                        <order attribute=""createdon"" descending=""true"" />
                                                        <link-entity name=""opportunity"" from=""opportunityid"" to=""opportunityid"" alias=""aa"">
                                                          <filter type=""and"">
                                                            <condition attribute=""opportunityid"" operator=""eq"" uitype=""opportunity"" value=""{0}"" />
                                                          </filter>
                                                        </link-entity>
                                                      </entity>
                                                    </fetch>";
        #endregion

        #region Eventos

        protected override void Execute(CodeActivityContext executionContext)
        {
            tracingService = null;
            try
            {
                //traza del workflow
                tracingService = executionContext.GetExtension<ITracingService>();
                //creando el contexto del Workflow
                tracingService.Trace("Creando enlace a servicio CRM.");
                contexto = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService servicio = serviceFactory.CreateOrganizationService(contexto.UserId);
                ServicioCRM = serviceFactory.CreateOrganizationService(contexto.UserId);
                tracingService.Trace("Enlace a servicio creado.");
                Guid IdOportunidad = IdOportunidadd.Get(executionContext).Id;

                //Obtenemos todos los productos que están asociados
                string strfetchProductosOportunidad = String.Format(fetchProductosOportunidad, IdOportunidad.ToString());
                EntityCollection productos = servicio.RetrieveMultiple(new FetchExpression(strfetchProductosOportunidad));

                productoId = new Guid[productos.Entities.Count];
                tipoproductoId = new Guid[productos.Entities.Count];
                int i = 0;
                foreach (Entity producto in productos.Entities)
                {
                    productoId[i] = ((EntityReference)producto.Attributes["productid"]).Id;
                    i++;
                }

                string strfetchTipoProductoOportunidad = String.Empty;
                for (i = 0; i < productoId.Length; i++)
                {
                    Entity producto=servicio.Retrieve("product", productoId[i], new ColumnSet("efk_tipo_productoid"));

                    if (producto != null && producto.Attributes.Contains("efk_tipo_productoid") && producto.Attributes["efk_tipo_productoid"]!=null)
                    {
                        tipoproductoId[i] = ((EntityReference)producto.Attributes["efk_tipo_productoid"]).Id;
                    }
                }

                EntityReference tipoP = new EntityReference("efk_tipo_producto", tipoproductoId[0]);
                IdTipoProducto.Set(executionContext, tipoP);
                return;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("Ha ocurrido un error en el Workflow Activity: WAObtienerTipoProductoOportunidad.", ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("Error en método principal de ejecución WAObtienerTipoProductoOportunidad: " + ex.Message);
                throw ex;
            }
        }

        #endregion
    }
}
