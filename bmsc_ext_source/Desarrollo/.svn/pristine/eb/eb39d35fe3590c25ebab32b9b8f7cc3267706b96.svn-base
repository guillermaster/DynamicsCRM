using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.CargaDatos.crmSDK;

namespace Efika.Crm.CargaDatos
{
    public class CDRelacionesCliente
    {
        public Guid relacionclienteid;
        public Guid clienteid1;
        public string descripcion1;
        public Guid rolid1;

        public Guid clienteid2;
        public string descripcion2;
        public Guid rolid2;

        public string sTipoReg;
        public Guid ownerid;
        public Guid ownerAnteriorid;
        public string efk_name;

        public void Cargar(CrmService servicio)
        {
            try
            {
                customerrelationship obj = new customerrelationship();
                if (sTipoReg == "E")
                {
                    servicio.Delete(EntityName.customerrelationship.ToString(), this.relacionclienteid);
                }
                else
                {
                    if (clienteid1 != Guid.Empty)
                    {
                        obj.customerid = new Customer();
                        obj.customerid.type = "account";           
                        obj.customerid.Value = clienteid1;
                    }
                    if (descripcion1 != null)
                    {
                        obj.customerroledescription = descripcion1;
                    }
                    if (rolid1 != Guid.Empty)
                    {
                        obj.customerroleid = new Lookup();
                        obj.customerroleid.Value = rolid1;
                    }

                    if (clienteid2 != Guid.Empty)
                    {
                        obj.partnerid = new Customer();
                        obj.partnerid.type = "account";
                        obj.partnerid.Value = clienteid2;
                    }
                    if (descripcion2 != null)
                    {
                        obj.partnerroledescription = descripcion2;
                    }
                    if (rolid2 != Guid.Empty)
                    {
                        obj.partnerroleid = new Lookup();
                        obj.partnerroleid.Value = rolid2;
                    }
  

                    if (ownerid != Guid.Empty)
                    {
                        obj.ownerid = new Owner();
                        obj.ownerid.type = "systemuser";
                        obj.ownerid.Value = ownerid;
                    }

                    if (this.relacionclienteid != Guid.Empty)
                    {
                        //Actualizamos
                        obj.customerrelationshipid = new Key();
                        obj.customerrelationshipid.Value = relacionclienteid;
                        if (ownerid != Guid.Empty && ownerAnteriorid != ownerid)
                        {
                            //reasignamos el cliente
                            AssignRequest req = new AssignRequest();
                            TargetOwnedCustomerRelationship poli = new TargetOwnedCustomerRelationship();
                            poli.EntityId = obj.customerrelationshipid.Value;
                            req.Target = poli;
                            SecurityPrincipal sp = new SecurityPrincipal();
                            sp.Type = SecurityPrincipalType.User;
                            sp.PrincipalId = ownerid;
                            req.Assignee = sp;
                            servicio.Execute(req);
                        }
                    }
                    else
                    {
                        this.relacionclienteid = servicio.Create(obj);
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
