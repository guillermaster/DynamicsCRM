using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.CargaDatos.crmSDK;

namespace Efika.Crm.CargaDatos
{
    public class CDTelefono
    {
        public Guid accountid;
        public Guid efk_telefonoid;
        public int? efk_codigo_cliente;
        public string efk_codigo;
        public string efk_numero_telefonico;
        public int? efk_tipo_telefono;
        public Guid ownerid;
        public Guid ownerAnteriorid;

        public void Cargar(CrmService servicio)
        {
            try
            {
                efk_telefono obj = new efk_telefono();

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
                    obj.efk_codigo = efk_codigo;
                }
                if (efk_numero_telefonico != null)
                {
                    obj.efk_numero_telefonico = efk_numero_telefonico;
                    obj.efk_name = efk_numero_telefonico;
                }
                if (efk_tipo_telefono != null)
                {
                    obj.efk_tipo_telefono = new Picklist();
                    obj.efk_tipo_telefono.Value = efk_tipo_telefono.Value;
                }
                if (ownerid != Guid.Empty)
                {
                    obj.ownerid = new Owner();
                    obj.ownerid.type = "systemuser";
                    obj.ownerid.Value = ownerid;
                }

                if (this.efk_telefonoid != Guid.Empty)
                {
                    //Actualizamos
                    obj.efk_telefonoid = new Key();
                    obj.efk_telefonoid.Value = efk_telefonoid;

                    servicio.Update(obj);

                    if (ownerid != Guid.Empty && ownerAnteriorid != ownerid)
                    {
                        //reasignamos el cliente
                        AssignRequest req = new AssignRequest();
                        TargetOwnedefk_telefono tel = new TargetOwnedefk_telefono();
                        tel.EntityId = obj.efk_telefonoid.Value;
                        req.Target = tel;
                        SecurityPrincipal sp = new SecurityPrincipal();
                        sp.Type = SecurityPrincipalType.User;
                        sp.PrincipalId = ownerid;
                        req.Assignee = sp;

                        servicio.Execute(req);

                    }
                }
                else
                {
                    this.efk_telefonoid = servicio.Create(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
