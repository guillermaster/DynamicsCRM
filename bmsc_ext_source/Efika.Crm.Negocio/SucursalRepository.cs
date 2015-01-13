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
    public class SucursalRepository
    {
        private Efika.Crm.Entidades.CredencialesCRM credenciales;


        public SucursalRepository(CredencialesCRM credenciales)
        {
            this.credenciales = credenciales;
        }


        public efk_sucursal getById(Guid guid, ColumnSet columnSet)
        {
            CrmService Servicio = ServicioCRM.ObtenerServicioCRM(this.credenciales);
            efk_sucursal sucursal = null;

            try
            {
                sucursal = (efk_sucursal)Servicio.Retrieve(EntityName.efk_sucursal.ToString(), guid, columnSet);                
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }

            return sucursal;
        }

    }
}
