using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.CargaDatos.crmSDK;


namespace Efika.Crm.CargaDatos
{
    public class CDCentralRiesgo
    {
        public Guid efk_calificacion_riesgoid;
        public int? iCodigoClienteBanco;
        public Guid accountid;

        public string efk_banco;
        public string efk_calificacion;
        public decimal? efk_riesgo;

        public string sTipoReg;
        public Guid ownerid;
        public Guid ownerAnteriorid;
        public string efk_name;

        public decimal? efk_riesgo_cartera;
        public decimal? efk_riesgo_contingente;

        public void Cargar(CrmService servicio)
        {
            try
            {
                efk_calificacion_riesgo obj = new efk_calificacion_riesgo();

                if (sTipoReg == "E")
                {
                    if (this.efk_calificacion_riesgoid != Guid.Empty)
                        servicio.Delete(EntityName.efk_calificacion_riesgo.ToString(), this.efk_calificacion_riesgoid);
                }
                else
                {

                    if (accountid != Guid.Empty)
                    {
                        obj.efk_cliente_juridico_id = new Lookup();
                        obj.efk_cliente_juridico_id.Value = accountid;
                    }

                    if (efk_name != null)
                    {
                        obj.efk_name = efk_name;
                    }
                    if (efk_banco != null)
                    {
                        obj.efk_banco = efk_banco;
                    }

                    if (efk_calificacion != null)
                    {
                        obj.efk_calificacion = efk_calificacion;
                    }


                    if (efk_riesgo != null)
                    {
                        obj.efk_riesgo = new CrmMoney();
                        obj.efk_riesgo.Value = efk_riesgo.Value;
                    }

                    //prioridad 0
                    if (efk_riesgo_cartera != null)
                    {
                        obj.efk_riesgo_cartera = new CrmMoney();
                        obj.efk_riesgo_cartera.Value = efk_riesgo_cartera.Value;
                    }

                    if (efk_riesgo_contingente != null)
                    {
                        obj.efk_riesgo_contingente = new CrmMoney();
                        obj.efk_riesgo_contingente.Value = efk_riesgo_contingente.Value;
                    }

                    //**********************************************
                    if (ownerid != Guid.Empty)
                    {
                        obj.ownerid = new Owner();
                        obj.ownerid.type = "systemuser";
                        obj.ownerid.Value = ownerid;
                    }

                    if (this.efk_calificacion_riesgoid != Guid.Empty)
                    {
                        //Actualizamos
                        obj.efk_calificacion_riesgoid = new Key();
                        obj.efk_calificacion_riesgoid.Value = efk_calificacion_riesgoid;

                        servicio.Update(obj);

                        if (ownerid != Guid.Empty && ownerAnteriorid != ownerid)
                        {
                            //reasignamos el cliente
                            AssignRequest req = new AssignRequest();
                            TargetOwnedefk_calificacion_riesgo pp = new TargetOwnedefk_calificacion_riesgo();
                            pp.EntityId = obj.efk_calificacion_riesgoid.Value;
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
                        this.efk_calificacion_riesgoid = servicio.Create(obj);
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
