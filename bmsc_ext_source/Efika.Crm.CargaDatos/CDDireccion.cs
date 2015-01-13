using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.CargaDatos.crmSDK;
using System.IO;

namespace Efika.Crm.CargaDatos
{
    public class CDDireccion
    {
        public Guid accountid;
        public Guid efk_direccionid;
        public int? efk_codigo_cliente;
        public int? efk_codigo;
        public int? efk_ciudad;
        public int? efk_barrio;
        public int? efk_zona;
        public string efk_direccion;
        public int? efk_tipo;
        public Guid ownerid;
        public Guid ownerAnteriorid;

        public void Cargar(CrmService servicio)
        {
            try
            {
                efk_direccion obj = new efk_direccion();

                if (accountid != Guid.Empty)
                {
                    obj.efk_cliente_juridico_id = new Lookup();
                    obj.efk_cliente_juridico_id.Value = accountid;
                }
                if (efk_codigo_cliente != null)
                {
                    obj.efk_codigo_cliente = new CrmNumber();
                    obj.efk_codigo_cliente.Value = efk_codigo_cliente.Value;
                }
                if (efk_codigo != null)
                {
                    obj.efk_codigo = new CrmNumber();
                    obj.efk_codigo.Value = efk_codigo.Value;
                }
                if(efk_ciudad!=null)
                {
                    obj.efk_ciudad = new Picklist();
                    obj.efk_ciudad.Value = efk_ciudad.Value;
                }
                if (efk_barrio != null)
                {
                    obj.efk_barrio = new Picklist();
                    obj.efk_barrio.Value = efk_barrio.Value;
                }
                if (efk_zona != null)
                {
                    obj.efk_zona = new Picklist();
                    obj.efk_zona.Value = efk_zona.Value;
                }
                if (efk_direccion != null)
                {
                    obj.efk_direccion1 = efk_direccion.ToString().Trim();                                                   
                    obj.efk_name = efk_direccion.Substring(0, (efk_direccion.Trim().Length > 100) ? 100 : efk_direccion.Trim().Length);
                }
                if (efk_tipo != null)
                {
                    obj.efk_tipo = new Picklist();
                    obj.efk_tipo.Value = efk_tipo.Value;
                }
                if (ownerid != Guid.Empty)
                {
                    obj.ownerid = new Owner();
                    obj.ownerid.type = "systemuser";
                    obj.ownerid.Value = ownerid;
                }

                if (this.efk_direccionid != Guid.Empty)
                {
                    //Actualizamos
                    obj.efk_direccionid = new Key();
                    obj.efk_direccionid.Value = efk_direccionid;

                    servicio.Update(obj);

                    if (ownerid != Guid.Empty && ownerAnteriorid != ownerid)
                    {
                        //reasignamos el cliente
                        AssignRequest req = new AssignRequest();
                        TargetOwnedefk_direccion dir = new TargetOwnedefk_direccion();
                        dir.EntityId = obj.efk_direccionid.Value;
                        req.Target = dir;
                        SecurityPrincipal sp = new SecurityPrincipal();
                        sp.Type = SecurityPrincipalType.User;
                        sp.PrincipalId = ownerid;
                        req.Assignee = sp;

                        servicio.Execute(req);

                    }
                }
                else
                {
                    this.efk_direccionid = servicio.Create(obj);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
