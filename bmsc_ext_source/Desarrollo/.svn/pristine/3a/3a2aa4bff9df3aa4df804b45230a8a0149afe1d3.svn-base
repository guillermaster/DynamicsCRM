using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Efika.Crm.AccesoServicios;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.Entidades;
using Efika.Crm.Negocio;
using System.Globalization;

namespace Efika.Crm.Web
{
    public partial class OfertaValorMicro : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Utilidades.ValidarAccesosWeb(Request.QueryString[Utilidades.NombresParametros.Usuario], Privilegios.NombresPrivilegios.OfertaValorMicro, false))
                {
                    return;
                }
                hdnIdCliente.Value = Request.QueryString[Utilidades.NombresParametros.ClienteId];                
                LoadOfertaValorMicro(new Guid(hdnIdCliente.Value));
                LoadMotivosRespuestaNegativa();
            }            
        }

        protected void LoadOfertaValorMicro(Guid idCliente)
        {
            try
            {
                CredencialesCRM credenciales = Credenciales.ObtenerCredenciales();
                Negocio.OfertaValor negOfertaValor = new Negocio.OfertaValor(credenciales);

                string urlWebResources = Utilidades.GetUrlCRM() + "/WebResources";

                DataTable dtOfertaValor = negOfertaValor.GetOfertaValorMicro(idCliente, urlWebResources);
                dlOfertaValor.DataSource = dtOfertaValor;
                dlOfertaValor.RepeatDirection = RepeatDirection.Horizontal;
                dlOfertaValor.DataBind();

                if (dlOfertaValor.Items.Count == 0)
                {
                    lblMensajeSinProductosOV.Visible = true;
                }

                VerificarRegistrosEfectividad();
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al cargar oferta de valor micro con cliente " + hdnIdCliente.Value, "OFERTA_VALOR", ex);
            }
        }

        private decimal GetMontoMaximo(DataListItem listItem)
        {
            decimal montoMaximo;
            try
            {
                montoMaximo = Convert.ToDecimal(((Label)listItem.FindControl("lblMontoMaxino")).Text);
            }
            catch (FormatException)
            {
                montoMaximo = 0;
            }
            catch (NullReferenceException)
            {
                montoMaximo = 0;
            }
            return montoMaximo;
        }

        private decimal GetProbabilidad(DataListItem listItem)
        {            
            try
            {
                decimal probAceptac;
                //obtener la probabilidad de aceptación expresada como % en HTML, convertir a decimal y dividir para 100
                probAceptac = Convert.ToDecimal(Convert.ToDecimal(((Label)listItem.FindControl("lblProbAceptac")).Text) / 100);
                return probAceptac;
            }
            catch (FormatException ex)
            {
                throw new Exception("La probabilidad de aceptación no puede estar vacía, cliente " + hdnIdCliente.Value, ex);
            }
        }

        protected void dlOfertaValor_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            try
            {
                decimal montoMaximo;
                decimal probAceptac;
                Guid idTipoProd;
                Guid idSubtProd;

                montoMaximo = GetMontoMaximo(e.Item);
                probAceptac = GetProbabilidad(e.Item);
                idTipoProd = GetTipoProductoId(e.Item);
                idSubtProd = GetSubTipoProductoId(e.Item);

                if (e.CommandArgument.ToString() == "1")
                {
                    RegistrarEfectividadOfertaValorCalculo(true, montoMaximo, probAceptac, idTipoProd, idSubtProd);
                    MostrarImagenSiDesea(e.Item);                                        
                }
                else
                {
                    e.Item.FindControl("ddlMotivoMalaCalificac").Visible = true;
                }
                //ocultar los botones de like y dislike para este item
                OcultarBotonesCalificacion(e.Item);
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al guardar efectividad de oferta de valor.", "OFERTA_VALOR_EF", ex);
            }
        }


        private void RegistrarEfectividadOfertaValorCalculo(bool efectividad, decimal montoMaximo, decimal probAceptac, Guid idTipoProd, Guid idSubtProd, int opcMotivoRechazo=0)
        {
            CredencialesCRM credenciales = Credenciales.ObtenerCredenciales();
            Negocio.OfertaValor negOfertaValor = new Negocio.OfertaValor(credenciales);
            Guid idCliente = new Guid(hdnIdCliente.Value);
            negOfertaValor.RegistrarEfectividadOfertaValor(idCliente, efectividad, montoMaximo, probAceptac, idSubtProd, idTipoProd, opcMotivoRechazo);
        }


        //método en el cual para cada registro de oferta de valor se consultará si ya tiene creado un registro de efectividad
        // el objetivo es evitar tener más de un registro por cada oferta
        protected void VerificarRegistrosEfectividad()
        {
            try
            {
                Guid idTipoProd;
                Guid idSubtipoProd;
                bool resultado;
                CredencialesCRM credenciales = Credenciales.ObtenerCredenciales();
                Negocio.OfertaValor negOfertaValor = new Negocio.OfertaValor(credenciales);

                foreach(DataListItem item in dlOfertaValor.Items)
                {
                    idTipoProd = GetTipoProductoId(item);
                    idSubtipoProd = GetSubTipoProductoId(item);
                    if (negOfertaValor.TieneRegistroEfectividad(new Guid(hdnIdCliente.Value), idTipoProd, idSubtipoProd, out resultado))
                    {
                        //ocultar los botones de like y dislike para este item
                        OcultarBotonesCalificacion(item);
                        if (resultado)
                        {
                            MostrarImagenSiDesea(item);
                        }
                        else
                        {
                            MostrarImagenNoDesea(item);
                        }
                    }
                }
                                
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al verificar registro de efectividad de oferta ", "OFERTA_VALOR", ex);
            }
        }

        //retorna el ID o GUID vacío del tipo de producto de un ítem en el data list de oferta de valor
        private Guid GetTipoProductoId(DataListItem listItem)
        {
            Guid idTipoProd;
            try
            {
                idTipoProd = new Guid(((HiddenField)listItem.FindControl("hdfTipoProductoId")).Value);
            }
            catch (FormatException)
            {
                idTipoProd = Guid.Empty;
            }
            catch (NullReferenceException)
            {
                idTipoProd = Guid.Empty;
            }
            return idTipoProd;
        }

        //retorna el ID o GUID vacío del SUB tipo de producto de un ítem en el data list de oferta de valor
        private Guid GetSubTipoProductoId(DataListItem listItem)
        {
            Guid idSubTipoProd;
            try
            {
                idSubTipoProd = new Guid(((HiddenField)listItem.FindControl("hdfSubTipoProductoId")).Value);
            }
            catch (FormatException)
            {
                idSubTipoProd = Guid.Empty;
            }
            catch (NullReferenceException)
            {
                idSubTipoProd = Guid.Empty;
            }
            return idSubTipoProd;
        }

        private void OcultarBotonesCalificacion(DataListItem listItem)
        {
            //ocultar los botones de like y dislike para este item
            listItem.FindControl("mgBtnLike").Visible = false;
            listItem.FindControl("mgBtnNoLike").Visible = false;
        }

        private void MostrarImagenSiDesea(DataListItem listItem)
        {
            listItem.FindControl("imgTick").Visible = true;
            listItem.FindControl("imgMinus").Visible = false;
        }

        private void MostrarImagenNoDesea(DataListItem listItem)
        {
            listItem.FindControl("imgTick").Visible = false;
            listItem.FindControl("imgMinus").Visible = true;
        }

        private void LoadMotivosRespuestaNegativa()
        {
            CredencialesCRM credenciales = Credenciales.ObtenerCredenciales();
            Negocio.OfertaValor negOfertaValor = new Negocio.OfertaValor(credenciales, true);
            List<string[]> motivos = negOfertaValor.GetOpcionesMotivosMalaCalificacionOV();
            DataTable dtMotivos = new DataTable();
            dtMotivos.Columns.Add("codigo");
            dtMotivos.Columns.Add("descripcion");
            DataRow drEmpty = dtMotivos.NewRow();
            drEmpty[0] = 0;
            drEmpty[1] = "Motivo:";
            dtMotivos.Rows.Add(drEmpty);

            foreach (string[] motivo in motivos)
            {
                DataRow drMotivos = dtMotivos.NewRow();
                drMotivos[0] = motivo[1];
                drMotivos[1] = motivo[0];
                dtMotivos.Rows.Add(drMotivos);
            }

            foreach(DataListItem listItem in dlOfertaValor.Items)
            {
                DropDownList ddlMotivos = (DropDownList)listItem.FindControl("ddlMotivoMalaCalificac");
                ddlMotivos.DataSource = dtMotivos;
                ddlMotivos.DataTextField = dtMotivos.Columns[1].ColumnName;
                ddlMotivos.DataValueField = dtMotivos.Columns[0].ColumnName;
                ddlMotivos.DataBind();
            }
        }

        protected void ddlMotivoMalaCalificac_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlMotivo = (DropDownList)sender;
            DataListItem listItem = (DataListItem)ddlMotivo.Parent;
            RegistrarCalificacionNegativa(listItem, int.Parse(ddlMotivo.SelectedValue));
            ddlMotivo.Visible = false;
        }

        private void RegistrarCalificacionNegativa(DataListItem listItem, int opcMotivoCalifNeg)
        {
            try
            {
                decimal montoMaximo;
                decimal probAceptac;
                Guid idTipoProd;
                Guid idSubtProd;

                montoMaximo = GetMontoMaximo(listItem);
                probAceptac = GetProbabilidad(listItem);
                idTipoProd = GetTipoProductoId(listItem);
                idSubtProd = GetSubTipoProductoId(listItem);

                RegistrarEfectividadOfertaValorCalculo(false, montoMaximo, probAceptac, idTipoProd, idSubtProd, opcMotivoCalifNeg);
                MostrarImagenNoDesea(listItem);                
                //ocultar los botones de like y dislike para este item
                OcultarBotonesCalificacion(listItem);
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al guardar efectividad de oferta de valor.", "OFERTA_VALOR_EF", ex);
            }
        }
    }
}