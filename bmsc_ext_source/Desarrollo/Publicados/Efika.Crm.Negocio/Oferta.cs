using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;
using System.Web.Services.Protocols;


namespace Efika.Crm.Negocio
{
    public class Oferta
    {
        private CrmService Servicio;
        private CredencialesCRM credenciales;
        private string error;

        public Oferta(CredencialesCRM credenciales)
        {
            Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
            this.credenciales = credenciales;
        }


        public bool ReabrirOfertaRiesgos(int numOferta)
        {
            try
            {
                bool success = true;
                int i = 0;
                Oportunidad negOportunidad = new Oportunidad(credenciales);
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "opportunityid", "efk_oferta_enviada_riesgos", "efk_oferta_reabierta_riesgos", 
                    "efk_oferta_revisada_riesgos", "efk_orden", "efk_oferta_cerrada" };
                BusinessEntityCollection becOp = negOportunidad.GetByNroOferta(numOferta, cs);

                while (success && i < becOp.BusinessEntities.Length)
                {
                    opportunity oport = (opportunity)becOp.BusinessEntities[i];
                    if (oport.efk_orden != null && oport.efk_orden.Value > 0)
                    {
                        if ((oport.efk_oferta_enviada_riesgos != null && oport.efk_oferta_enviada_riesgos.Value) &&
                            (oport.efk_oferta_cerrada != null && oport.efk_oferta_cerrada.Value) &&
                            (oport.efk_oferta_reabierta_riesgos != null && !oport.efk_oferta_reabierta_riesgos.Value) &&
                            (oport.efk_oferta_revisada_riesgos != null && !oport.efk_oferta_revisada_riesgos.Value))
                        {
                            negOportunidad.AbrirOportunidadParaRiesgos(oport.opportunityid.Value, oport.efk_orden.Value);
                        }
                        else
                        {
                            error = "La oferta no cumple las condiciones necesarias para ser reabierta por riesgos.";
                            success = false;
                        }
                        i++;
                    }
                }
                return success;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool ReabrirOfertaGerentes(int numOferta)
        {
            try
            {
                bool success = true;
                int i = 0;
                Oportunidad negOportunidad = new Oportunidad(credenciales);
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "opportunityid", "efk_oferta_reabierta_riesgos", 
                    "efk_orden", "efk_solicitud_score_evaluado", "efk_estado_revision_riesgos" };
                BusinessEntityCollection becOp = negOportunidad.GetByNroOferta(numOferta, cs);

                while (success && i < becOp.BusinessEntities.Length)
                {
                    opportunity oport = (opportunity)becOp.BusinessEntities[i];
                    if (oport.efk_orden != null && oport.efk_orden.Value > 0)
                    {
                        if ((oport.efk_solicitud_score_evaluado != null && oport.efk_solicitud_score_evaluado.Value) &&
                            ((oport.efk_oferta_reabierta_riesgos != null && !oport.efk_oferta_reabierta_riesgos.Value) ||
                             (oport.efk_estado_revision_riesgos != null && oport.efk_estado_revision_riesgos.Value == RevisionRiesgos.Rechazada.Codigo)))
                        {
                            negOportunidad.AbrirOportunidadParaGerentes(oport.opportunityid.Value, oport.efk_orden.Value);
                        }
                        else
                        {
                            error = "La oferta no cumple las condiciones necesarias para ser reabierta por riesgos.";
                            success = false;
                        }
                    }
                    i++;
                }
                return success;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void AprobarOfertaRiesgos(int numOferta, bool ofertaAprobada)
        {
            try
            {
                Oportunidad negOportunidad = new Oportunidad(credenciales);
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "opportunityid", "efk_oferta_enviada_riesgos", "efk_oferta_reabierta_riesgos", "efk_oferta_revisada_riesgos", 
                    "efk_oferta_cerrada", "efk_solicitud_score_evaluado", "statecode" };
                BusinessEntityCollection becOp = negOportunidad.GetByNroOferta(numOferta, cs);

                foreach (BusinessEntity beOp in becOp.BusinessEntities)
                {
                    opportunity oport = (opportunity)beOp;
                    if ((oport.statecode != null && oport.statecode.Value != OpportunityState.Lost) &&
                        (oport.efk_oferta_enviada_riesgos != null && oport.efk_oferta_enviada_riesgos.Value) &&
                        (oport.efk_oferta_cerrada != null && oport.efk_oferta_cerrada.Value || (!ofertaAprobada)) &&
                        (oport.efk_oferta_reabierta_riesgos != null && oport.efk_oferta_reabierta_riesgos.Value) &&
                        (oport.efk_oferta_revisada_riesgos != null && !oport.efk_oferta_revisada_riesgos.Value) &&
                        (oport.efk_solicitud_score_evaluado != null && oport.efk_solicitud_score_evaluado.Value) || (!ofertaAprobada))
                    {
                        negOportunidad.AprobacionRiesgos(oport.opportunityid.Value, ofertaAprobada);
                        if (!ofertaAprobada)
                        {
                            negOportunidad.SetOportunidadOfertaCerrada(oport.opportunityid.Value);
                        }
                    }
                    else
                        throw new Exception("La oferta no cumple las condiciones necesarias para ser finalizada por riesgos.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void EnviarOfertaARiesgos(Entidades.Oportunidad oportunidad)
        {
            try
            {
                Oportunidad negOport = new Oportunidad(credenciales);

                BusinessEntityCollection oportunidades;
                ColumnSet resultSetColumns = new ColumnSet();
                int contador = 0;
                int enviadasAFabrica = 0;

                resultSetColumns.Attributes = new string[] { "opportunityid", "efk_solicitud_enviada_fabrica" };
                oportunidades = negOport.GetByNroOferta(oportunidad.NumeroOferta, resultSetColumns);

                bool haySolEnviadaAFabrica = false;
                int i = 0;

                while (!haySolEnviadaAFabrica && i < oportunidades.BusinessEntities.Length)
                {
                    contador++;
                    opportunity _oportunity = (opportunity)oportunidades.BusinessEntities[i];

                    if (_oportunity != null)
                    {
                        if (_oportunity.efk_solicitud_enviada_fabrica == null || _oportunity.efk_solicitud_enviada_fabrica.Value == false)
                        {
                            haySolEnviadaAFabrica = true;
                        }
                        else if (_oportunity.efk_solicitud_enviada_fabrica != null && _oportunity.efk_solicitud_enviada_fabrica.Value == true)
                        {
                            enviadasAFabrica++;
                        }
                    }
                    i++;
                }

                if (contador > 0 && contador == enviadasAFabrica)
                {
                    foreach (BusinessEntity item in oportunidades.BusinessEntities)
                    {
                        opportunity _opportunity = (opportunity)item;
                        negOport.EnviarOportunidadARiesgos(_opportunity.opportunityid.Value, true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string Error
        {
            get
            {
                return error;
            }
        }

        public class RevisionRiesgos
        {
            public class Aprobada
            {
                public static int Codigo { get { return 221220000; } }
            }
            public class Rechazada
            {
                public static int Codigo { get { return 221220001; } }
            }
        }
    }
}
