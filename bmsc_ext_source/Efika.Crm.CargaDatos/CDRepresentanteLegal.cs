using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.CargaDatos.crmSDK;

namespace Efika.Crm.CargaDatos
{

    public class CDRepresentanteLegal
    {
        public Guid efk_representante_legalid;
        public int? iCodigoClienteBanco;
        public Guid efk_representantelegalid;
        public int? efk_tipo_identificacion;
        public string efk_codigo_banco;
        public string efk_documento_identidad;
        public string efk_cargo;
        public string sTipoReg;
        public Guid ownerid;
        public Guid ownerAnteriorid;
        public string efk_name;
        public string efk_codigo_banco_rep_legal;
       

        //FASE II
        public Guid efk_representante_legal_clienteid;

        //prioridad 0
        public bool? efk_firma_autorizada;
        public string efk_telefono;

        public void Cargar(CrmService servicio)
        {
            try
            {
                efk_representante_legal obj = new efk_representante_legal();

                if (sTipoReg == "E")
                {
                    if (this.efk_representante_legalid != Guid.Empty)
                        servicio.Delete(EntityName.efk_representante_legal.ToString(), this.efk_representante_legalid);
                }
                else
                {
                    if (efk_representantelegalid != Guid.Empty)
                    {
                        obj.efk_representantelegalid = new Lookup();
                        obj.efk_representantelegalid.Value = efk_representantelegalid;
                    }
                  
                    if (efk_name != null)
                    {
                        obj.efk_name = efk_name;
                    }
                    if (efk_codigo_banco != null)
                    {
                        obj.efk_codigo_banco = efk_codigo_banco;
                    }

                    if (efk_documento_identidad != null)
                    {
                        obj.efk_documento_identidad = efk_documento_identidad;
                    }

                    if (efk_cargo != null)
                    {
                        obj.efk_cargo = efk_cargo;
                    }

                    if (efk_representante_legal_clienteid != Guid.Empty)
                    {
                        obj.efk_representante_legal_clienteid = new Lookup();
                        obj.efk_representante_legal_clienteid.Value = efk_representante_legal_clienteid;
                    }


                    //**********************************************
                    if (efk_tipo_identificacion != null)
                    {
                        obj.efk_tipo_identificacion = new Picklist();
                        obj.efk_tipo_identificacion.Value = efk_tipo_identificacion.Value;
                    }
                    //**********************************************
                    if (ownerid != Guid.Empty)
                    {
                        obj.ownerid = new Owner();
                        obj.ownerid.type = "systemuser";
                        obj.ownerid.Value = ownerid;
                    }

                    //prioridad 0
                    if (efk_firma_autorizada != null)
                    {
                        obj.efk_firma_autorizada = new CrmBoolean();
                        obj.efk_firma_autorizada.Value = efk_firma_autorizada.Value;
                    }

                    if (!string.IsNullOrEmpty(efk_telefono))
                    {
                        obj.efk_telefono = efk_telefono;
                    }

                    if (this.efk_representante_legalid != Guid.Empty)
                    {
                        //Actualizamos
                        obj.efk_representante_legalid = new Key();
                        obj.efk_representante_legalid.Value = efk_representante_legalid;

                        servicio.Update(obj);

                        if (ownerid != Guid.Empty && ownerAnteriorid != ownerid)
                        {
                            //reasignamos el cliente
                            AssignRequest req = new AssignRequest();
                            TargetOwnedefk_representante_legal pp = new TargetOwnedefk_representante_legal();
                            pp.EntityId = obj.efk_representante_legalid.Value;
                            req.Target = pp;
                            SecurityPrincipal sp = new SecurityPrincipal();
                            sp.Type = SecurityPrincipalType.User;
                            sp.PrincipalId = ownerid;
                            req.Assignee = sp;

                            servicio.Execute(req);

                        }
                    }
                    else
                    {
                        this.efk_representante_legalid = servicio.Create(obj);
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
