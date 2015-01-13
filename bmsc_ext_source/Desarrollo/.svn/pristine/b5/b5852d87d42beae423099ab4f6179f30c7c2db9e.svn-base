using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.CargaDatos.crmSDK;

namespace Efika.Crm.CargaDatos
{
    public class CDGarantiasOperaciones
    {
        private AssociateEntitiesRequest crearelacion = null;
        private AssociateEntitiesRequest crearelacion_opuesta = null;
        private Moniker producto_operacion_activo = null;
        private Moniker producto_garantia = null;
        public Guid efk_producto_activoid;
        public Guid efk_producto_garantiaid;

        public void Cargar(CrmService servicio)
        {
            try
            {
                if (efk_producto_activoid != Guid.Empty)
                {
                    producto_operacion_activo = new Moniker();
                    producto_operacion_activo.Name = "efk_producto_activo";
                    producto_operacion_activo.Id = efk_producto_activoid;
                }

                if (efk_producto_garantiaid != Guid.Empty)
                {
                    producto_garantia = new Moniker();
                    producto_garantia.Name = "efk_producto_activo";
                    producto_garantia.Id = efk_producto_garantiaid;
                }

                if (producto_operacion_activo != null && producto_garantia != null)
                {
                    crearelacion = new AssociateEntitiesRequest();
                    crearelacion.Moniker1 = producto_operacion_activo;
                    crearelacion.Moniker2 = producto_garantia;
                    crearelacion.RelationshipName = "efk_producto_activo_garantia";
                    servicio.Execute(crearelacion);

                    crearelacion_opuesta = new AssociateEntitiesRequest();
                    crearelacion_opuesta.Moniker1 = producto_garantia;
                    crearelacion_opuesta.Moniker2 = producto_operacion_activo;
                    crearelacion_opuesta.RelationshipName = "efk_producto_activo_garantia";
                    servicio.Execute(crearelacion_opuesta);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
