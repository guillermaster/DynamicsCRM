using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;

namespace Efika.Crm.Negocio
{
    public class Campania
    {
        private CrmService Servicio;
        private Entidades.Campania campania;
        private CredencialesCRM credenciales;


        public Campania(CredencialesCRM credenciales)
        {
            Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
            this.credenciales = credenciales;
        }


        public bool CargarCampania(Guid idCampania, ColumnSet columnas)
        {
            this.campania = new Entidades.Campania();

            try
            {
                campaign campania = (campaign)Servicio.Retrieve(EntityName.campaign.ToString(), idCampania, columnas);
                //analizar columnas que se solicitaron, y agregar sus valores a la instancia de la clase Entidades.Campania
                foreach (string columna in columnas.Attributes)
                {
                    switch (columna)
                    {
                        case "proposedend":
                            this.campania.FechaFinPropuesta = DateTime.Parse(campania.proposedend.Value);
                            break;
                        case "name":
                        default:
                            this.campania.Nombre = campania.name;
                            break;
                    }
                }
                return true;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                throw new Exception(ex.Detail.InnerText);
            }
            catch (Exception ex)
            {
                campania.Errores = ex.Message;
                return false;
            }
        }


        public List<Entidades.Producto> ProductosPorCampania(Guid idCampania, ColumnSet cs)
        {
            BusinessEntityCollection productosCampania;
            List<Entidades.Producto> productos = new List<Entidades.Producto>();

            try
            {
                ConditionExpression condExp = new ConditionExpression();
                condExp.AttributeName = "campaignid";
                condExp.Operator = ConditionOperator.Equal;
                condExp.Values = new string[] { idCampania.ToString() };

                ColumnSet csCampItem = new ColumnSet();
                csCampItem.Attributes = new string[] { "campaignitemid", "entityid", "entitytype" };

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.campaignitem.ToString();
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { condExp };
                qryExp.ColumnSet = csCampItem;

                productosCampania = Servicio.RetrieveMultiple(qryExp);
               
                
                foreach (BusinessEntity beProdCamp in productosCampania.BusinessEntities)
                {
                    campaignitem prodCamp = (campaignitem)beProdCamp;
                    if (prodCamp.entitytype == EntityName.product.ToString())
                    {
                        Producto negProd = new Producto(credenciales);
                        Entidades.Producto prod = negProd.GetProducto(prodCamp.entityid.Value, cs);
                        productos.Add(prod);
                    }
                }
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                throw new Exception(ex.Detail.InnerText);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return productos;
        }

        public Entidades.Campania CampaniaActual
        {
            get
            {
                return campania;
            }
        }


    }
}
