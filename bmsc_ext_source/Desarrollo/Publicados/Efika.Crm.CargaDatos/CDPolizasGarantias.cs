using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.CargaDatos.crmSDK;

namespace Efika.Crm.CargaDatos
{
    public class CDPolizasGarantias
    {
        private AssociateEntitiesRequest crearelacion = null;
        private Moniker producto_garantia = null;
        private Moniker poliza = null;        
        public Guid efk_producto_garantiaid;
        public Guid efk_polizasid;        

        public void Cargar(CrmService servicio)
        {
            try
            {
                if (efk_producto_garantiaid != Guid.Empty)
                {
                    producto_garantia = new Moniker();
                    producto_garantia.Name = "efk_producto_activo";
                    producto_garantia.Id = efk_producto_garantiaid;
                }

                if (efk_polizasid != Guid.Empty)
                {
                    poliza = new Moniker();
                    poliza.Name = "efk_polizas";
                    poliza.Id = efk_polizasid;
                }

                if (poliza != null && producto_garantia != null)
                {
                    crearelacion = new AssociateEntitiesRequest();
                    crearelacion.Moniker1 = producto_garantia;
                    crearelacion.Moniker2 = poliza;
                    crearelacion.RelationshipName = "efk_producto_activo_poliza";
                    servicio.Execute(crearelacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
