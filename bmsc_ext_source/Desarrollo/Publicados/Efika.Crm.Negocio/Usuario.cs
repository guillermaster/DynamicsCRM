

namespace Efika.Crm.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Efika.Crm.AccesoServicios.CRMSDK;
    using Efika.Crm.AccesoServicios;
    using Efika.Crm.Entidades;

    public class Usuario
    {
        private CrmService Servicio = new CrmService();
        private CredencialesCRM credenciales;



        public Usuario(CredencialesCRM credenciales)
        {
            Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
            this.credenciales = credenciales;
        }

        public systemuser  GetUserById(Guid userID, ColumnSet columns)
        {
            systemuser crmUser = new systemuser();
         

            BusinessEntity beSystemUser = Servicio.Retrieve(EntityName.systemuser.ToString(), userID, columns);

            crmUser = (systemuser)beSystemUser;
            
            return crmUser;
        }


    }
}
