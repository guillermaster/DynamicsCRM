using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.CargaDatos.crmSDK;

namespace Efika.Crm.CargaDatos
{
    public class CDPoliza
    {
        public Guid efk_accountid;
        public int? efk_codigo_cliente;
        public Guid efk_polizasid;      
        public string efk_numero_poliza;
        public string efk_nombre;
        public string  efk_compania_aseguradora;
        public DateTime? efk_fecha_vencimiento;
        public string sTipoReg;
        public Guid ownerid;
        public Guid ownerAnteriorid;
        public string efk_name;

        public void Cargar(CrmService servicio)
        {
            try
            {
                efk_polizas obj = new efk_polizas();

                if (sTipoReg == "E")
                {
                    if (this.efk_polizasid != Guid.Empty)
                    {
                        if (ExisteRegistro(servicio, efk_polizasid, "efk_polizas"))
                            servicio.Delete(EntityName.efk_polizas.ToString(), this.efk_polizasid);
                    }
                }
                else
                {
                    if (efk_accountid != Guid.Empty)
                    {
                        obj.efk_accountid = new Lookup();
                        obj.efk_accountid.Value = efk_accountid;
                    }
                    if (efk_codigo_cliente != null)
                    {
                        obj.efk_codigo_cliente = new CrmNumber();
                        obj.efk_codigo_cliente.Value = efk_codigo_cliente.Value;
                    }
                    if (efk_numero_poliza != null)
                    {
                        obj.efk_numero_poliza = efk_numero_poliza;
                    }
                    if (efk_nombre != null)
                    {
                        obj.efk_nombre = efk_nombre;
                    }
                    if (efk_compania_aseguradora != null)
                    {
                        obj.efk_compania = efk_compania_aseguradora;
                    }

                    if (ownerid != Guid.Empty)
                    {
                        obj.ownerid = new Owner();
                        obj.ownerid.type = "systemuser";
                        obj.ownerid.Value = ownerid;
                    }
                    if (efk_fecha_vencimiento != null && efk_fecha_vencimiento.Value > new DateTime(1900, 1, 1))
                    {
                        obj.efk_fecha_vencimiento = new CrmDateTime();
                        obj.efk_fecha_vencimiento.Value = efk_fecha_vencimiento.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    if (this.efk_polizasid != Guid.Empty)
                    {
                        if (ExisteRegistro(servicio, efk_polizasid, "efk_polizas"))
                        {
                            //Actualizamos
                            obj.efk_polizasid = new Key();
                            obj.efk_polizasid.Value = efk_polizasid;

                            servicio.Update(obj);

                            if (ownerid != Guid.Empty && ownerAnteriorid != ownerid)
                            {
                                //reasignamos el cliente
                                AssignRequest req = new AssignRequest();
                                TargetOwnedefk_polizas poli = new TargetOwnedefk_polizas();
                                poli.EntityId = obj.efk_polizasid.Value;
                                req.Target = poli;
                                SecurityPrincipal sp = new SecurityPrincipal();
                                sp.Type = SecurityPrincipalType.User;
                                sp.PrincipalId = ownerid;
                                req.Assignee = sp;
                                servicio.Execute(req);
                            }
                        }
                    }
                    else
                    {
                        this.efk_polizasid = servicio.Create(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="servicio"></param>
        /// <param name="id"></param>
        /// <param name="entidad"></param>
        /// <returns></returns>
        private bool ExisteRegistro(CrmService servicio, Guid id, string entidad)
        {
            bool existe = false;
            ColumnSet columna = new ColumnSet();
            columna.Attributes = new String[] { "efk_polizas_Id" };
            try
            {
                BusinessEntity ent = servicio.Retrieve(entidad, id, columna);
                if (ent != null)
                    existe = true;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                if (ex.Detail.InnerText.Contains("0x80040217"))
                    existe = false;
            }

            return existe;
        }
    }
}
