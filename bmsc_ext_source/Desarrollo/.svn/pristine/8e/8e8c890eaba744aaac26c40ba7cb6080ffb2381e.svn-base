using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.CargaDatos.crmSDK;

namespace Efika.Crm.CargaDatos
{
    public class CDRolRelaciones
    {
        public Guid relathionshiproleid;
        public string nombre;
        public int statuscode;

        public Guid relationshiprolemapid;
        public int tipoobjetoasociado;

        public void Cargar(CrmService servicio)
        {
            relationshiprole rol = new relationshiprole();
            relationshiprolemap rolmap = new relationshiprolemap();
            try
            {
                if (this.relathionshiproleid == Guid.Empty)
                {
                    rol.name = nombre;
                    rol.statuscode = new Status();
                    rol.statuscode.Value = 1;

                    relathionshiproleid = servicio.Create(rol);

                    if (relathionshiproleid != Guid.Empty)
                    {
                        rolmap.relationshiproleid = new Lookup();
                        rolmap.relationshiproleid.Value = relathionshiproleid;
                        rolmap.associateobjecttypecode = new EntityNameReference();
                        rolmap.primaryobjecttypecode = new EntityNameReference();
                        rolmap.associateobjecttypecode.Value = "account";
                        rolmap.primaryobjecttypecode.Value = "account";
                        servicio.Create(rolmap);
                    }
                }
                else
                {
                    if (relationshiprolemapid == Guid.Empty)
                    {
                        rolmap.relationshiproleid = new Lookup();
                        rolmap.relationshiproleid.Value = relathionshiproleid;
                        rolmap.associateobjecttypecode = new EntityNameReference();
                        rolmap.primaryobjecttypecode = new EntityNameReference();
                        rolmap.associateobjecttypecode.Value = "account";
                        rolmap.primaryobjecttypecode.Value = "account";
                        servicio.Create(rolmap);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
