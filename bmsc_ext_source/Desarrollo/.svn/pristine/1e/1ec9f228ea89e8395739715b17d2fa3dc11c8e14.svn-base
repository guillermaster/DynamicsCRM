using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;
using Efika.Crm.Negocio;

namespace Efika.Crm.Negocio
{
    public class Agencia
    {
        private Efika.Crm.Entidades.CredencialesCRM credenciales;


        public Agencia(CredencialesCRM credenciales)
        {
            this.credenciales = credenciales;
        }


        public efk_oficina getById(Guid guid, ColumnSet columnSet)
        {
            CrmService Servicio = ServicioCRM.ObtenerServicioCRM(this.credenciales);
            efk_oficina agencia = null;

            try
            {
                agencia = (efk_oficina)Servicio.Retrieve(EntityName.efk_oficina.ToString(), guid, columnSet);                                     
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }

            return agencia;
        }


        public efk_oficina getAgenciaByName(CredencialesCRM credenciales)
        {
            CrmService Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
            
            efk_oficina agencia = null;

            try
            {
                ConditionExpression stateCondition = new ConditionExpression();
                stateCondition.AttributeName = "efk_ciudad";
                stateCondition.Operator = ConditionOperator.Equal;
                stateCondition.Values = new string[] { "LA PAZ" };

                FilterExpression outerFilter = new FilterExpression();
                outerFilter.FilterOperator = LogicalOperator.And;
                outerFilter.Conditions = new ConditionExpression[] { stateCondition };

                ColumnSet resultSetColumns = new ColumnSet();
                resultSetColumns.Attributes = new string[] { "efk_sucursalid", "efk_oficinaid", "efk_ciudad" };
                QueryExpression qryExpression = new QueryExpression();
                qryExpression.Criteria = outerFilter;
                qryExpression.ColumnSet = resultSetColumns;

                qryExpression.EntityName = EntityName.efk_oficina.ToString();
                qryExpression.Distinct = false;

                BusinessEntityCollection segmentoResultSet = Servicio.RetrieveMultiple(qryExpression);
                agencia = segmentoResultSet.BusinessEntities.ElementAt(0) as efk_oficina;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }

            return agencia;
        }
    }
}
