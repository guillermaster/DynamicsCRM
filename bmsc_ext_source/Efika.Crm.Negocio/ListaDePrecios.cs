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
    public class ListaDePrecios
    {

        public static Guid ListaDePreciosId(CrmService servicio, Guid transCurrencyId)
        {
            try
            {
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "pricelevelid" };

                ConditionExpression condExp = new ConditionExpression();
                condExp.AttributeName = "transactioncurrencyid";
                condExp.Operator = ConditionOperator.Equal;
                condExp.Values = new string[] { transCurrencyId.ToString() };

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.pricelevel.ToString();
                qryExp.ColumnSet = cs;
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { condExp };

                BusinessEntityCollection bec = servicio.RetrieveMultiple(qryExp);

                if (bec.BusinessEntities.Length > 0)
                {
                    pricelevel entListaPrecios = (pricelevel)bec.BusinessEntities[0];
                    return entListaPrecios.pricelevelid.Value;
                }
                else
                    throw new Exception("No se encontró lista de precios para la divisa con GUID " + transCurrencyId.ToString());
            }
            catch (SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText + " " + ex.StackTrace);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
