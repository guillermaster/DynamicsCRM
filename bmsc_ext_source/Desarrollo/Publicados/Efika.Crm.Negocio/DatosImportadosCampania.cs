using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;

namespace Efika.Crm.Negocio
{

    public class DatosImportadosCampania
    {
        private CrmService Servicio;

        public DatosImportadosCampania(CredencialesCRM credenciales)
        {
            Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
        }

        public BusinessEntityCollection DatosImportadosPorCampanaYCliente(Guid idClienteJuridico, Guid idCampania, ColumnSet cs)
        {
            BusinessEntityCollection becDatImpCamp;

            ConditionExpression condExpCamp = new ConditionExpression();
            condExpCamp.AttributeName = "efk_campana_id";
            condExpCamp.Operator = ConditionOperator.Equal;
            condExpCamp.Values = new string[] { idCampania.ToString() };

            ConditionExpression condExpCliente = new ConditionExpression();
            condExpCliente.AttributeName = "efk_cliente_juridico_id";
            condExpCliente.Operator = ConditionOperator.Equal;
            condExpCliente.Values = new string[] { idClienteJuridico.ToString() };
            
            QueryExpression qryExp = new QueryExpression();
            qryExp.EntityName = EntityName.efk_datos_importados_campana.ToString();
            qryExp.Criteria = new FilterExpression();
            qryExp.Criteria.Conditions = new ConditionExpression[] { condExpCamp, condExpCliente };
            qryExp.ColumnSet = cs;

            try
            {
                becDatImpCamp = Servicio.RetrieveMultiple(qryExp);//retorna instancias de datos importados de campaña
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return becDatImpCamp;
        }


        public List<Entidades.DatosImportadosCampania> DatosImportadosPorCampanaYProducto(Guid idCampania, Guid idProducto)
        {
            try
            {
                List<Entidades.DatosImportadosCampania> datosImportados = new List<Entidades.DatosImportadosCampania>();

                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_cliente_juridico_id", "efk_cliente_natural_id", "efk_cupo_monto", "efk_datos_importados_campanaid",
                                        "efk_importacion_datos_preaprobadosid", "efk_name", "efk_numero_identificacion", "efk_prospecto_id"};

                ConditionExpression condExpCamp = new ConditionExpression();
                condExpCamp.AttributeName = "efk_campana_id";
                condExpCamp.Operator = ConditionOperator.Equal;
                condExpCamp.Values = new string[] { idCampania.ToString() };

                ConditionExpression condExpProd = new ConditionExpression();
                condExpProd.AttributeName = "efk_subtipo_producto_id";
                condExpProd.Operator = ConditionOperator.Equal;
                condExpProd.Values = new string[] { idProducto.ToString() };

                QueryExpression qryExp = new QueryExpression();
                qryExp.ColumnSet = cs;
                qryExp.EntityName = EntityName.efk_datos_importados_campana.ToString();
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { condExpCamp, condExpProd };

                BusinessEntityCollection bec = Servicio.RetrieveMultiple(qryExp);

                foreach (BusinessEntity be in bec.BusinessEntities)
                {
                    efk_datos_importados_campana entDatImpCamp = (efk_datos_importados_campana)be;
                    Entidades.DatosImportadosCampania datImpCamp = new Entidades.DatosImportadosCampania();
                    datImpCamp.CampaniaId = idCampania;
                    datImpCamp.ClienteJuridicoId = entDatImpCamp.efk_cliente_juridico_id.Value;
                    datImpCamp.ClienteNaturalId = entDatImpCamp.efk_cliente_natural_id.Value;
                    datImpCamp.CupoMonto = entDatImpCamp.efk_cupo_monto.Value;
                    datImpCamp.Id = entDatImpCamp.efk_datos_importados_campanaid.Value;
                    datImpCamp.ImportacDatosPreAprobadosId = entDatImpCamp.efk_importacion_datos_preaprobadosid.Value;
                    datImpCamp.Nombre = entDatImpCamp.efk_name;
                    datImpCamp.NumeroIdentificacion = entDatImpCamp.efk_numero_identificacion;
                    datosImportados.Add(datImpCamp);
                }

                return datosImportados;
            }
            catch (SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
