using System;
using Efika.Crm.CargaDatos.crmSDK;

namespace Efika.Crm.CargaDatos
{
    public class DivisaCRM
    {
        CrmService servicio;

        public DivisaCRM(CrmService _servicio)
        {
            servicio = _servicio;
        }

        public Guid GetIdDivisa(string codIsoDivisa)
        {
            try
            {
                Guid idDivisa;

                ConditionExpression condExp = new ConditionExpression();
                condExp.AttributeName = "isocurrencycode";
                condExp.Operator = ConditionOperator.Equal;
                condExp.Values = new string[] { codIsoDivisa };

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.transactioncurrency.ToString();
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { condExp };

                BusinessEntityCollection becOp = servicio.RetrieveMultiple(qryExp);

                if (becOp.BusinessEntities.Length > 0)
                {
                    transactioncurrency transCurr = (transactioncurrency)becOp.BusinessEntities[0];
                    idDivisa = transCurr.transactioncurrencyid.Value;
                }
                else
                {
                    throw new Exception("No se encontró divisa con código " + codIsoDivisa);
                }

                return idDivisa;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
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
