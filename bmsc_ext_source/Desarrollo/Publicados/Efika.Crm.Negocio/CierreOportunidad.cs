using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;
using System.Web.Services.Protocols;

namespace Efika.Crm.Negocio
{
    public class CierreOportunidad
    {
        private string error;
        private CrmService Servicio;
        private CredencialesCRM credenciales;

        public CierreOportunidad(CredencialesCRM credenciales)
        {
            Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
            this.credenciales = credenciales;
        }


        public bool CerrarOportunidad(Guid idOportunidad, string tipoCierre, int razonCierre, Guid idCompetidor, decimal monto, string descripcion, ref string resultadoError)
        {
            try
            {
                ColumnSet csOp = new ColumnSet();
                csOp.Attributes = new string[] { "opportunityid", "efk_proceso_venta_finalizado", "name", "efk_fecha_cierre_verbal", "statuscode", "efk_numero_oferta", "efk_orden",
                "efk_solicitud_enviada_fabrica", "efk_lista_para_recalcular","efk_tipo_familia_producto", "efk_oferta_cerrada"};

                opportunity oportunidad = (opportunity)Servicio.Retrieve(EntityName.opportunity.ToString(), idOportunidad, csOp);

                string fechaCierre = DateTime.Now.ToString("u");
                opportunityclose opClose = new opportunityclose();
                opClose.opportunityid = new Lookup();
                opClose.opportunityid.Value = oportunidad.opportunityid.Value;
                opClose.description = descripcion;
                opClose.subject = oportunidad.name;
                opClose.actualend = new CrmDateTime();
                opClose.actualend.Value = fechaCierre;
                opClose.actualrevenue = new CrmMoney();
                opClose.actualrevenue.Value = monto;

                if (tipoCierre == TipoCierre.Perdida.ToString())//LA OPORTUNIDAD SE CIERRA COMO PERDIDA
                {
                    opClose.competitorid = new Lookup();
                    opClose.competitorid.Value = idCompetidor;
                    if (CerrarOportunidadPerdida(oportunidad, opClose, razonCierre, fechaCierre, ref resultadoError))
                    {

                        if (oportunidad.efk_numero_oferta != null && oportunidad.efk_orden != null && oportunidad.efk_lista_para_recalcular != null)
                        {
                            ActualizarEstadosOfertaSimulacEnOportunidadPerdida(oportunidad.opportunityid.Value, oportunidad.efk_numero_oferta.Value,
                            oportunidad.efk_orden.Value, oportunidad.efk_lista_para_recalcular.Value);
                        }

                        ReordenarSiguientesOportunidades(oportunidad);//(genera excepción)
                        return true;
                    }
                }
                else if (tipoCierre == TipoCierre.Ganada.ToString())//LA OPORTUNIDAD SE CIERRA COMO GANADA
                {
                    //validar si ha concluido el proceso de ventas
                    //validar si oportunidad tiene por lo menos un producto asociado
                    if (CerrarOportunidadGanada(oportunidad, opClose, razonCierre, fechaCierre, ref resultadoError))
                        return true;
                }
            }
            catch (SoapException ex)
            {
                throw new Exception(ex.Detail.InnerText, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }


        private void ActualizarEstadosOfertaSimulacEnOportunidadPerdida(Guid idOportunidad, int numOferta, int orden, bool listaRecalcular)
        {
            try
            {
                if (listaRecalcular)
                {//cambiar la oportunidad que está lista para recalcuar dentro de una oferta
                    if (numOferta > 0)
                        SetListaRecalcularSiguienteOportunidad(numOferta, orden);
                }

                opportunity oportunidad = new opportunity();
                oportunidad.opportunityid = new Key();
                oportunidad.opportunityid.Value = idOportunidad;
                oportunidad.efk_solicitud_score_evaluado = new CrmBoolean();
                oportunidad.efk_solicitud_score_evaluado.Value = true;
                oportunidad.efk_solicitud_enviada_fabrica = new CrmBoolean();
                oportunidad.efk_solicitud_enviada_fabrica.Value = true;                
                oportunidad.efk_lista_para_recalcular = new CrmBoolean();
                oportunidad.efk_lista_para_recalcular.Value = false;
                oportunidad.efk_pendiente_recalcular = new CrmBoolean();
                oportunidad.efk_pendiente_recalcular.Value = false;
                Servicio.Update(oportunidad);
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                throw new Exception(ex.Detail.InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetListaRecalcularSiguienteOportunidad(int numOferta, int orden)
        {
            try
            {
                bool updated = false;
                opportunity oppOrd1 = new opportunity(); ;

                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_orden", "statecode" };

                ConditionExpression condExp = new ConditionExpression();
                condExp.AttributeName = "efk_numero_oferta";
                condExp.Operator = ConditionOperator.Equal;
                condExp.Values = new string[] { numOferta.ToString() };

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.opportunity.ToString();
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { condExp };
                qryExp.ColumnSet = cs;

                BusinessEntityCollection bec = Servicio.RetrieveMultiple(qryExp);

                if (bec.BusinessEntities.Length > 1)
                {
                    foreach (BusinessEntity be in bec.BusinessEntities)
                    {
                        opportunity opp = (opportunity)be;
                        if (opp.efk_orden != null)
                        {
                            if (opp.efk_orden.Value == 1)
                                oppOrd1 = opp;
                            if (opp.efk_orden.Value == orden + 1)
                            {
                                if (opp.statecode.Value != OpportunityState.Lost)
                                {
                                    opp.efk_lista_para_recalcular = new CrmBoolean();
                                    opp.efk_lista_para_recalcular.Value = true;
                                    Servicio.Update(opp);
                                    updated = true;
                                    break;
                                }
                                else
                                    orden++;
                            }
                        }
                    }

                    if (!updated)//si ningún registro se actualizó significa que la oportunidad de orden (orden) es la última de la oferta, por lo tanto la oportunidad con orden 1 debe ser actualizada
                    {
                        oppOrd1.efk_lista_para_recalcular = new CrmBoolean();
                        oppOrd1.efk_lista_para_recalcular.Value = true;
                        Servicio.Update(oppOrd1);
                    }
                }

            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                throw new Exception(ex.Detail.InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /** Valida si una oportunidad puede ser cerrada como ganada **/
        private bool ValidarOportunidadGanada(opportunity oportunidad)
        {
            if (oportunidad.efk_proceso_venta_finalizado != null && oportunidad.efk_proceso_venta_finalizado.Value)
            {
                if (TieneProductoOportunidad(oportunidad))
                {
                    if (oportunidad.efk_tipo_familia_producto != null && oportunidad.efk_tipo_familia_producto.Value != Entidades.Producto.FamiliaTipos.Activo.Codigo)
                    {
                        return true;
                    }
                    else
                    {
                        if (oportunidad.efk_solicitud_enviada_fabrica.Value)
                            return true;
                        else
                            error = "La oportunidad aún no ha sido enviada al sistema de operaciones.";
                    }
                }
                else
                {
                    error = "La oportunidad no tiene ningún producto asociado.";
                }

                if (oportunidad.efk_solicitud_enviada_fabrica.Value)
                {
                    if (TieneProductoOportunidad(oportunidad))
                        return true;
                }
            }
            else
            {
                error = "El proceso de venta todavía no ha finalizado.";
            }

            return false;
        }

        /** Valida si una oportunidad puede ser cerrada como cerrada **/
        private bool ValidarOportunidadCerrada(opportunity oportunidad)
        {
            try
            {
                Negocio.Oportunidad negOp = new Oportunidad(credenciales);

                if (TieneProductoOportunidad(oportunidad))
                {
                    if (oportunidad.efk_tipo_familia_producto != null && oportunidad.efk_tipo_familia_producto.Value == Entidades.Producto.FamiliaTipos.Activo.Codigo)
                    {
                        if (negOp.EsOportunidadCreadaPorSimulacion(oportunidad.opportunityid.Value))
                        {
                            if ((oportunidad.efk_lista_para_recalcular != null && oportunidad.efk_lista_para_recalcular.Value) ||
                                (oportunidad.efk_oferta_cerrada != null && oportunidad.efk_oferta_cerrada.Value))
                            {
                                return true;
                            }
                            else
                            {
                                error = "La oportunidad no está lista para recalcular o la oferta no ha sido cerrada.";
                            }
                        }
                        else { return true; }
                    }
                    else { return true; }
                }
                else { error = "La oportunidad NO tiene productos."; }

            }
            catch (SoapException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        private bool TieneProductoOportunidad(opportunity oportunidad)
        {
            try
            {
                ConditionExpression condExp = new ConditionExpression();
                condExp.AttributeName = "opportunityid";
                condExp.Operator = ConditionOperator.Equal;
                condExp.Values = new string[] { oportunidad.opportunityid.Value.ToString() };
                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.opportunityproduct.ToString();
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { condExp };

                BusinessEntityCollection bec = Servicio.RetrieveMultiple(qryExp);
                if (bec.BusinessEntities.Length > 0)
                    return true;
                else
                    error = "La oportunidad no existe";
            }
            catch (SoapException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        private bool CerrarOportunidadPerdida(opportunity oportunidad, opportunityclose oportCerrada, int razonCierre, string fechaCierre, ref string resultadoError)
        {
            try
            {
                if (ValidarOportunidadCerrada(oportunidad))
                {
                    ActualizarCamposCierreOportunidad(oportunidad, razonCierre, fechaCierre, OpportunityState.Lost);
                    //cerrar oportunidad perdida
                    LoseOpportunityRequest requestLose = new LoseOpportunityRequest();
                    requestLose.OpportunityClose = oportCerrada;
                    requestLose.Status = razonCierre;
                    Servicio.Execute(requestLose);
                    resultadoError = string.Empty;
                    return true;
                }
                else
                    resultadoError = error;
            }
            catch (SoapException ex)
            {
                throw new Exception(ex.Detail.InnerText, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }


        private bool CerrarOportunidadGanada(opportunity oportunidad, opportunityclose oportCerrada, int razonCierre, string fechaCierre, ref string resultadoError)
        {
            try
            {
                if (ValidarOportunidadGanada(oportunidad))
                {
                    ActualizarCamposCierreOportunidad(oportunidad, razonCierre, fechaCierre, OpportunityState.Won);
                    //cerrar oportunidad ganada
                    WinOpportunityRequest requestWin = new WinOpportunityRequest();
                    requestWin.OpportunityClose = oportCerrada;
                    requestWin.Status = razonCierre;
                    Servicio.Execute(requestWin);
                    resultadoError = string.Empty;
                    return true;
                }
                else
                    resultadoError = error;
            }
            catch (SoapException ex)
            {
                throw new Exception(ex.Detail.InnerText, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }


        private void ActualizarCamposCierreOportunidad(opportunity oportunidad, int razonCierre, string fechaCierre, OpportunityState opState)
        {
            try
            {
                oportunidad.efk_fecha_cierre_verbal = new CrmDateTime();
                oportunidad.efk_fecha_cierre_verbal.Value = DateTime.Now.ToString("u");

                OpportunityStateInfo state = new OpportunityStateInfo();
                state.Value = opState;
                oportunidad.statecode = state;
                oportunidad.efk_estado_cierre = new Picklist();
                oportunidad.efk_estado_cierre.Value = 100000000;
                oportunidad.efk_razon_cierre_estado = new Picklist();
                oportunidad.efk_razon_cierre_estado.Value = 100000000;

                Servicio.Update(oportunidad);
            }
            catch (SoapException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ActualizarOrdenYEstadosRecalculoMonto(opportunity oportunidad, int nuevoOrden, bool pendienteRecalcular, bool listaRecalcular)
        {
            try
            {
                oportunidad.efk_orden = new CrmNumber();
                oportunidad.efk_orden.Value = nuevoOrden;
                oportunidad.efk_pendiente_recalcular = new CrmBoolean();
                oportunidad.efk_pendiente_recalcular.Value = pendienteRecalcular;
                oportunidad.efk_lista_para_recalcular = new CrmBoolean();
                oportunidad.efk_lista_para_recalcular.Value = listaRecalcular;
                Servicio.Update(oportunidad);
            }
            catch (SoapException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //ReordenarSiguientesOportunidades: Reordena y actualiza estados de las oportunidades cuyo orden es mayor a la oportunidadPerdida
        private void ReordenarSiguientesOportunidades(opportunity oportunidadPerdida)
        {
            int nvoOrden;
            bool listaParaRecalcular;

            try
            {
                if (oportunidadPerdida.efk_numero_oferta != null && oportunidadPerdida.efk_orden != null)
                {
                    ColumnSet cs = new ColumnSet();
                    cs.Attributes = new string[] { "efk_orden" };

                    ConditionExpression condExp = new ConditionExpression();
                    condExp.AttributeName = "efk_numero_oferta";
                    condExp.Operator = ConditionOperator.Equal;
                    condExp.Values = new string[] { oportunidadPerdida.efk_numero_oferta.Value.ToString() };
                    QueryExpression qryExp = new QueryExpression();
                    qryExp.EntityName = EntityName.opportunity.ToString();
                    qryExp.Criteria = new FilterExpression();
                    qryExp.Criteria.Conditions = new ConditionExpression[] { condExp };
                    qryExp.ColumnSet = cs;

                    BusinessEntityCollection bec = Servicio.RetrieveMultiple(qryExp);

                    foreach (BusinessEntity be in bec.BusinessEntities)
                    {
                        opportunity oportunidad = (opportunity)be;
                        if (oportunidad.efk_orden != null)
                        {
                            if (oportunidadPerdida.efk_orden.Value < oportunidad.efk_orden.Value)
                            {
                                nvoOrden = oportunidad.efk_orden.Value - 1;
                                listaParaRecalcular = (oportunidadPerdida.efk_orden.Value == nvoOrden ? true : false);
                                ActualizarOrdenYEstadosRecalculoMonto(oportunidad, nvoOrden, true, listaParaRecalcular);
                            }
                        }
                    }
                }
            }
            catch (SoapException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /** Retorna una lista (que se puede asociar a dropdownlists) de todos los competidores **/
        public List<ListItem> ListaCompetidores()
        {
            List<ListItem> list = new List<ListItem>();
            ColumnSet colset = new ColumnSet();
            colset.Attributes = new string[] { "name", "competitorid" };

            try
            {
                QueryExpression qryEx = new QueryExpression
                {
                    EntityName = EntityName.competitor.ToString(),
                    ColumnSet = colset
                };

                BusinessEntityCollection bec = Servicio.RetrieveMultiple(qryEx);
                foreach (BusinessEntity be in bec.BusinessEntities)
                {
                    competitor comp = (competitor)be;
                    list.Add(new ListItem(comp.name, comp.competitorid.Value.ToString()));
                }
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                throw new Exception(ex.Detail.InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public decimal Monto(Guid idOportunidad)
        {
            decimal monto;
            try
            {

                ColumnSet csOp = new ColumnSet();
                csOp.Attributes = new string[] { "opportunityid", "estimatedvalue" };

                opportunity oportunidad = (opportunity)Servicio.Retrieve(EntityName.opportunity.ToString(), idOportunidad, csOp);
                if (oportunidad.estimatedvalue != null)
                    monto = oportunidad.estimatedvalue.Value;
                else
                    monto = 0;
                return monto;
            }
            catch (SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Form Values
        public enum TipoCierre
        {
            Ganada, Perdida
        }

        /** Retorna una lista (que se puede asociar a dropdownlists) de las razones por las cuales se cierra una oportunidad **/
        public static List<ListItem> ListaRazones(string tipo)
        {
            List<ListItem> list = new List<ListItem>();

            if (tipo == TipoCierre.Ganada.ToString())
            {
                ListItem liGanado = new ListItem("Ganado", "3");
                list.Add(liGanado);
                ListItem liCondProd = new ListItem("Condiciones del producto", "100000000");
                list.Add(liCondProd);
                ListItem liImgInst = new ListItem("Imagen institucional", "100000001");
                list.Add(liImgInst);
                ListItem liCampania = new ListItem("Por campaña", "100000002");
                list.Add(liCampania);
            }
            else if (tipo == TipoCierre.Perdida.ToString())
            {
                ListItem liCancelado = new ListItem("Cancelado", "4");
                list.Add(liCancelado);
                ListItem liAgotado = new ListItem("Agotado", "5");
                list.Add(liAgotado);
                ListItem liCompetencia = new ListItem("Competencia", "100000005");
                list.Add(liCompetencia);
                ListItem liNoCumpleReq = new ListItem("No cumple requisitos", "100000006");
                list.Add(liNoCumpleReq);
                ListItem liMalComCentRiesgo = new ListItem("Mal comportamiento en la central de riesgo", "100000007");
                list.Add(liMalComCentRiesgo);
                ListItem liCancCamp = new ListItem("Cancelación campaña", "100000008");
                list.Add(liCancCamp);
                ListItem liFaltaGest = new ListItem("Falta de gestión", "100000009");
                list.Add(liFaltaGest);
            }

            return list;
        }
        #endregion
    }
}
