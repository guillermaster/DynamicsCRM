using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System;
using System.ServiceModel;
using System.Collections;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Workflow;

namespace Efika.Crm.Plugins.MA.Cliente
{
    public class ObtenerCorreoGerenteRegional : CodeActivity
    {

        #region Variables        
        private static string strFetchXmlUsuarioGerente;
        #endregion

        #region Funciones
        private string ObtenerUnidadDeNegocio()
        {
            strFetchXmlUsuarioGerente = string.Empty;

            strFetchXmlUsuarioGerente = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""true"">
                                              <entity name=""systemuser"">
                                                <attribute name=""fullname"" />
                                                <attribute name=""businessunitid"" />
                                                <attribute name=""title"" />
                                                <attribute name=""address1_telephone1"" />
                                                <attribute name=""systemuserid"" />
                                                <attribute name=""internalemailaddress"" />
                                                <order attribute=""fullname"" descending=""false"" />
                                                <filter type=""and"">
                                                  <condition attribute=""isdisabled"" operator=""eq"" value=""0"" />
                                                  <condition attribute=""efk_sucursalid"" operator=""eq"" uitype=""efk_sucursal"" value=""{0}"" />
                                                </filter>
                                                <link-entity name=""systemuserroles"" from=""systemuserid"" to=""systemuserid"" visible=""false"" intersect=""true"">
                                                  <link-entity name=""role"" from=""roleid"" to=""roleid"" alias=""aa"">
                                                    <filter type=""and"">
                                                      <condition attribute=""name"" operator=""eq"" value=""{0}"" />
                                                    </filter>
                                                  </link-entity>
                                                </link-entity>
                                              </entity>
                                            </fetch>";
            return strFetchXmlUsuarioGerente;
        }
        #endregion

        protected override void Execute(CodeActivityContext executionContext)
        {
            //Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            string strFetchXml = string.Format(strFetchXmlUsuarioGerente, Sucursal, RolGerente);
            EntityCollection bec = service.RetrieveMultiple(new FetchExpression(strFetchXml));

            if (bec.Entities.Count > 0)
            {
                Entity entidad = bec.Entities[0];
                UsuarioGerente.Set(executionContext, bec.Entities[0]);
            }
        }


        [Input("Id Sucursal")]
        [ReferenceTarget("efk_sucursal")]
        public InArgument<EntityReference> Sucursal { get; set; }

        [Input("Rol Gerente")]
        public InArgument<string> RolGerente { get; set; }

        [Output("Id Usuario")]
        [ReferenceTarget("systemuser")]
        public OutArgument<EntityReference> UsuarioGerente { get; set; }
    }
}
