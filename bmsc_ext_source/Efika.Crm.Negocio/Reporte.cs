using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;

namespace Efika.Crm.Negocio
{
    public class Reporte
    {
        private CrmService Servicio;


        public Reporte(CredencialesCRM credenciales)
        {
            Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
        }


        public Guid obtenerIdReporte(string nombreReporte)
        {
            Guid idReporte = Guid.Empty;
            try
            {
                ColumnSet cols = new ColumnSet();
                cols.Attributes = new string[] { "reportid", "name" };

                ConditionExpression condExp = new ConditionExpression();
                condExp.AttributeName = "name";
                condExp.Operator = ConditionOperator.Equal;
                condExp.Values = new string[] { nombreReporte };

                FilterExpression filtExp = new FilterExpression();
                filtExp.Conditions = new ConditionExpression[] { condExp };

                QueryExpression query = new QueryExpression();
                query.EntityName = EntityName.report.ToString();
                query.Criteria = filtExp;
                query.ColumnSet = cols;

                RetrieveMultipleRequest retrieve = new RetrieveMultipleRequest();
                retrieve.Query = query;

                RetrieveMultipleResponse retrieved = (RetrieveMultipleResponse)Servicio.Execute(retrieve);

                if (retrieved.BusinessEntityCollection.BusinessEntities.Length > 0)
                {
                    foreach(BusinessEntity bEnt in retrieved.BusinessEntityCollection.BusinessEntities)
                    {
                        report publicReport = (report)bEnt;
                        idReporte = publicReport.reportid.Value;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return idReporte;
        }
    }
}
