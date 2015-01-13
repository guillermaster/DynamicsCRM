using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.Entidades;

namespace Efika.Crm.AccesoServicios
{
    public class PrivilegiosCRM
    {
        public static bool ValidarPrivilegio(string privilegio, Guid idUsuario, CredencialesCRM credenciales)
        {
            QueryExpression queryReadPrivilege = new QueryExpression
            {
                EntityName = "privilege",
                Criteria = new FilterExpression()
            };
            ColumnSet columnas = new ColumnSet();
            columnas.Attributes = new string[] { "privilegeid", "name" };
            queryReadPrivilege.ColumnSet = columnas;

            ConditionExpression condiciones = new ConditionExpression();
            condiciones.AttributeName = "name";
            condiciones.Operator = ConditionOperator.Equal;
            condiciones.Values = new object[]{privilegio};

            queryReadPrivilege.Criteria.Conditions = new ConditionExpression[] { condiciones };

            CrmService servicio = ServicioCRM.ObtenerServicioCRM(credenciales);

            // Retrieve the prvReadQueue privilege.
            BusinessEntityCollection bec = servicio.RetrieveMultiple(queryReadPrivilege);

            if (bec.BusinessEntities.Length > 0)
            {
                Guid privilegeid = ((privilege)bec.BusinessEntities[0]).privilegeid.Value;

                //Obtenemos todos los privilegios de un usuario
                RetrieveUserPrivilegesRequest req = new RetrieveUserPrivilegesRequest();
                req.UserId = idUsuario;
                RetrieveUserPrivilegesResponse res = (RetrieveUserPrivilegesResponse)servicio.Execute(req);

                foreach (RolePrivilege p in res.RolePrivileges)
                {
                    if (p.PrivilegeId == privilegeid)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
