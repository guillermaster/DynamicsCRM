namespace Efika.Crm.Web.ScoreEvaluate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Reflection;
    using System.Text;
    using Efika.Crm.AccesoServicios;
    using Efika.Crm.Negocio;
    using Efika.Crm.Entidades;
    using Efika.Crm.Entidades.Common;
    using Efika.Crm.Entidades.TipoCambio;
    using Efika.Crm.Web.Common;
    using Efika.Crm.Web.BMSCEvaluarScoreService;
    using Efika.Crm.Entidades.ScoreEvaluate;
    using System.Security.Cryptography.X509Certificates;
    using System.Net.Security;
    using System.Net;

    
    public partial class ScoreEvaluate : System.Web.UI.Page
    {
        const string PRIVILEGIO_WRITE_OPORTUNIDAD = "prvWriteOpportunity";
        const string NUMERO_SOLICITUD = "efk_nrosolicitud";
        const string NOMBRE_USUARIO = "strUsuario";
        const string DOMINIO = "strDominio";
        const string OPORTUNIDADID = "IdOportunidad";
        const string OPTION_PARAMETER = "option";

        private Negocio.ScoreEvaluate scoreEvaluate;
        private Entidades.ScoreEvaluate.MsgRequestScoreEvaluate msgRequest;
        private Entidades.ScoreEvaluate.MsgResponseScoreEvaluate msgResponse;
        private List<ValidateField> validateField = new List<ValidateField>();
        private string cmrUserID;
        private string nroSolicitud = string.Empty;
        private string nombreUsuario = string.Empty;
        private string dominio = string.Empty;
        private string oportunidadID = string.Empty;
        private string msgText;
        private string queryStringOption;
        private string strOrigen = string.Empty;
        CredencialesCRM credenciales = new CredencialesCRM();


        protected void Page_Load(object sender, EventArgs e)
        {
            cmrUserID = Request.QueryString[Efika.Crm.Web.Utilidades.NombresParametros.Usuario];
            nroSolicitud = Request.QueryString[NUMERO_SOLICITUD];
            nombreUsuario = Request.QueryString[NOMBRE_USUARIO];
            dominio = Request.QueryString[DOMINIO];
            oportunidadID = Request.QueryString[OPORTUNIDADID];
            queryStringOption = Request.QueryString[OPTION_PARAMETER];

            ServicePointManager.ServerCertificateValidationCallback += MyCertificatePolicy.ValidateCertificate;

            if (!IsPostBack)
            {
                try
                {
                    if (!Efika.Crm.Web.Utilidades.ValidarAccesosWeb(Request.QueryString[Efika.Crm.Web.Utilidades.NombresParametros.Usuario], PRIVILEGIO_WRITE_OPORTUNIDAD))
                    {
                        Response.Write(@"<script type=""text/javascript""> alert('No tiene privilegios para acceder a este formulario.'); window.close(); </script>");
                        return;
                    }

                    if (ValidateScore())
                    {
                        //si el origen es nulo, entonces la oportunidad fue creada manualmente
                        if (OportunidadManual())
                        {
                            EvaluarScore();
                            CerrarVentana();
                        }
                        else//la oportunidad fue creada por simulación
                        {
                            overlay.Visible = false;
                            BtnEvaluarScore.Enabled = true;
                            ClientScript.RegisterStartupScript(GetType(), "ScoreEvaluate", "<script type=\"text/javascript\">  $('#dvLoading').hide();   $('#dvLoadingMsg').hide();</script>");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Efika.Crm.Web.Utilidades.ReportarError(this, Request.QueryString[Efika.Crm.Web.Utilidades.NombresParametros.Usuario], "Se generó una excepción validando la información, si el problema persiste favor comunicar al administrador del sistema. " + ex.Message, "Mensaje", ex);
                    CerrarVentana();
                }
            }
        }


        private void EvaluarScore()
        {
            try
            {
                string value;
                bool rvalue = true;
                value = CreateUpdate_FotoSolicitudCredito(ref rvalue);
                ActualizarOportunidadScoreEvaluado(new Guid(Request.QueryString[OPORTUNIDADID]));
                msgText = "Se generó correctamente la foto solicitud de crédito";
                if (!OportunidadManual())
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "EvaluarScoreAlert", "alert('La evaluación de score se ha realizado correctamente.');", true);
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "EvaluarScore", "checklist('../Images/check_ok.gif' , '" + msgText + "');", true);
                    
                }
            }
            catch (Exception ex)
            {
                if (!OportunidadManual())
                {
                    msgText = "No se generó correctamente la foto solicitud de crédito";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ScoreEvaluate", 
                        "checklist('../Images/notify_critico.png' , '" + msgText + "');", true);
                }

                Efika.Crm.Web.Utilidades.ReportarError(this, Request.QueryString[Efika.Crm.Web.Utilidades.NombresParametros.Usuario], "Se generó una excepción. Si el problema persiste por favor comunicarse con el administrador del sistema. ", "EvaluarScore", ex);
            }

            if (!OportunidadManual())
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ScoreEvaluate1", @"$('#dvLoading').hide();", true);
        }


        private bool OportunidadManual()
        {
            if (String.IsNullOrWhiteSpace(Request.QueryString[Efika.Crm.Web.Utilidades.NombresParametros.Origen]))
                return true;
            else
                return false;
        }

        protected void BtnEvaluarScore_Click(object sender, EventArgs e)
        {
            EvaluarScore();
        }

        protected void Cerrar_Click(object sender, EventArgs e)
        {
            CerrarVentana();
        }

        private void CerrarVentana()
        {
            Session["msgRequest"] = null;
            ClientScript.RegisterStartupScript(GetType(), "ScoreEvaluateCerrarVentana", "<script type=\"text/javascript\">window.close();</script>");
        }

        private bool ValidateScore()
        {
            bool returnValue;
            returnValue = true;
            try
            {
                credenciales = Credenciales.ObtenerCredenciales();
                if (credenciales == null)
                {
                    returnValue = false;
                    Efika.Crm.Web.Utilidades.AgregarErrorAlLog("No se puede obtener las credenciales.", this.GetType().Name,
                        MethodBase.GetCurrentMethod().Name, Request.QueryString[Efika.Crm.Web.Utilidades.NombresParametros.Usuario], string.Empty);
                    throw new Exception("No se puede obtener las credenciales.");

                }

                scoreEvaluate = new Negocio.ScoreEvaluate(this.credenciales);
                msgRequest = scoreEvaluate.GetMsgReqScoreEvaluate(new Guid(cmrUserID), new Guid(oportunidadID));
                Session["msgRequest"] = msgRequest;

                //validar el nro solicitud
                if (string.IsNullOrEmpty(nroSolicitud))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "updateinfo", "<script type='text/javascript'> checklist('../Images/notify_critico.png' , " +
                        "'La oportunidad debe tener un número de solicitud. No es posible evaluar esta oportunidad.'); </script>");
                    ClientScript.RegisterStartupScript(this.GetType(), "BtnEvaluarScore_disable",
                        "<script type='text/javascript'>  $('#BtnEvaluarScore').attr('disabled', 'disabled'); </script>");
                    BtnEvaluarScore.Enabled = false;
                    returnValue = false;
                }

                // Validar fecha de actualizacion de la informacion
                if (!scoreEvaluate.ValidateAntiguedadDatos(credenciales, new Guid(oportunidadID), new Guid(msgRequest.ClienteID),
                    msgRequest.OpportunityUpdateCondGarantias, msgRequest.ClienteUpdateModeloEvaluador, msgRequest.InfoDiaAntiguedad, ref validateField))
                {
                    foreach (var item in validateField)
                    {
                        msgText = "La información de Condiciones y Garantías de la oportunidad está actualizada";
                        if (item.Entity != "Oportunidad")
                        {
                            msgText = "La información de Datos Modelo Evaluador del cliente está actualizada";
                        }
                        ClientScript.RegisterStartupScript(this.GetType(), "updateinfooportunidad",
                            "<script type='text/javascript'> checklist('../Images/check_ok.gif' , '" + msgText + "'); </script>");

                        msgText = item.Message;
                        this.ClientScript.RegisterStartupScript(this.GetType(), "updateinfo", "<script type='text/javascript'> checklist('../Images/notify.png' , '" + msgText + "'); </script>");
                    }
                }
                else
                {
                    msgText = "La información de Condiciones y Garantias de la oportunidad está actualizada";
                    ClientScript.RegisterStartupScript(this.GetType(), "updateinfooportunidad", "<script type='text/javascript'> checklist('../Images/check_ok.gif' , '" + msgText + "'); </script>");

                    msgText = "La información de Datos Modelo Evaluador del cliente está actualizada";
                    ClientScript.RegisterStartupScript(this.GetType(), "updateinfocliente", "<script type='text/javascript'> checklist('../Images/check_ok.gif' , '" + msgText + "'); </script>");
                }

                // Validar los campos requerido Oportunidad/CondicionesGarantia.
                validateField.Clear();
                if (!scoreEvaluate.ValidateOportunidad_CondicionesGarantia(new Guid(oportunidadID), msgRequest.NroOferta, ref validateField))
                {
                    if (validateField.Count() > 0)
                    {
                        msgText = @"Son necesarios todos los campos en la sección ""Condiciones y Garantías""  de las Oportunidades con número de oferta:  " + validateField[0].Value;
                        ClientScript.RegisterStartupScript(this.GetType(), "OportunidadCondicionesGarantia",
                            "<script type='text/javascript'> checklist('../Images/notify.png' , '" + msgText + "'); </script>");
                    }
                }

                //validar los campos requerido Cliente/ModeloEvaluador
                validateField.Clear();
                if (!scoreEvaluate.ValidateCliente_ModeloEvaluador(credenciales, new Guid(msgRequest.ClienteID), ref validateField))
                {
                    if (validateField.Count() > 0)
                    {
                        msgText = @"Son necesarios todos los campos en la sección ""Datos Modelo Evaluador""  del cliente";
                        this.ClientScript.RegisterStartupScript(this.GetType(), "ClienteModeloEvaluador", "<script type='text/javascript'> checklist('../Images/notify.png' , '" + msgText + "'); </script>");
                        ValidateClienteSegmented(validateField, "Segmento-D");
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = false;
                Efika.Crm.Web.Utilidades.ReportarError(this.Page, Request.QueryString[Efika.Crm.Web.Utilidades.NombresParametros.Usuario], "Error al evaluar score.", "EVAL_SCORE", ex);
            }

            return returnValue;
        }


        private void ValidateClienteSegmented(List<ValidateField> list, string segmento)
        {
            foreach (var item in list)
            {
                if (item.Section == segmento)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ClienteModeloEvaluadorSegmento",
                        "<script type='text/javascript'> checklist('../Images/notify_critico.png' , '" + item.Message + "'); </script>");
                    ClientScript.RegisterStartupScript(this.GetType(), "BtnES_CMES_disable", "<script type='text/javascript'>  $('#BtnEvaluarScore').attr('disabled', 'disabled'); </script>");
                    break;
                }
            }
        }

        private string CreateUpdate_FotoSolicitudCredito(ref bool rvalue)
        {
            rvalue = true;
            msgRequest = (MsgRequestScoreEvaluate)Session["msgRequest"];
            if (msgRequest == null)
            {
                rvalue = false;
                if (!OportunidadManual())
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "NoScoreEvaluate", " $('#BtnEvaluarScore').attr('disabled', 'disabled');", true);
                throw new Exception("En este momento no se puede evaluar el score para esta oportunidad");
            }

            cmrUserID = Request.QueryString[Efika.Crm.Web.Utilidades.NombresParametros.Usuario];
            nroSolicitud = Request.QueryString[NUMERO_SOLICITUD];
            nombreUsuario = Request.QueryString[NOMBRE_USUARIO];
            dominio = Request.QueryString[DOMINIO];
            oportunidadID = Request.QueryString[OPORTUNIDADID];

            credenciales = Credenciales.ObtenerCredenciales();

            if (credenciales == null)
            {
                rvalue = false;
                Efika.Crm.Web.Utilidades.AgregarErrorAlLog("No se puede obtener las credenciales.", this.GetType().Name,
                    MethodBase.GetCurrentMethod().Name, Request.QueryString[Efika.Crm.Web.Utilidades.NombresParametros.Usuario], string.Empty);
                throw new Exception("No se puede obtener las credenciales.");
            }


            if (string.IsNullOrEmpty(nroSolicitud))
            {
                rvalue = false;
                throw new Exception("No existe número de solicitud");
            }

            msgRequest.NroSolicitud = int.Parse(nroSolicitud.Replace(",", "").Replace(".", "").Trim());
            msgRequest.LoginUsuario = dominio + "\\" + nombreUsuario;
            msgRequest.User = credenciales.Usuario;
            msgRequest.Password = credenciales.Password;
            msgRequest.Domain = credenciales.Dominio;

            // obtener  el tipo de cambio
            TipoCambioResult tcResult = new TipoCambioResult(0, 0, 0, 0, "");
            try
            {
                MsgTipoCambio msgTipoCambio = new MsgTipoCambio(credenciales.Usuario, credenciales.Password, credenciales.Dominio);
                Web.TipoCambio.TipoCambioServices tcService = new Web.TipoCambio.TipoCambioServices();

                ServicePointManager.ServerCertificateValidationCallback += MyCertificatePolicy.ValidateCertificate;
                tcResult = tcService.TipoCambio(msgTipoCambio);

                if (tcResult.Resultado == 0)
                {
                    msgText = "Obteniendo los valores - TIPO CAMBIO";
                    if(!OportunidadManual())
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ws_tipocambio", " checklist('../Images/check_ok.gif' , '" + msgText + "');", true);
                }
                else
                {
                    msgText = "No se pudo obtener los valores - TIPO CAMBIO";
                    if (!OportunidadManual())
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ws_tipocambio", " checklist('../Images/notify.png' , '" + msgText + "');", true);
                }
            }
            catch (Exception ex)
            {
                msgText = "Error: al obtener los valores del servicio web - TIPO CAMBIO";
                if (!OportunidadManual())
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ws_tipocambio1", " checklist('../Images/notify_critico.png' , '" + msgText + "');", true);
                Efika.Crm.Web.Utilidades.AgregarErrorAlLog("[Tipo de cambio] Error al invocar al ws del banco para obtener el tipo de cambio " + ex.Message, this.GetType().Name,
                    MethodBase.GetCurrentMethod().Name, Request.QueryString[Efika.Crm.Web.Utilidades.NombresParametros.Usuario], ex.StackTrace);
            }


            ScoreService se = new ScoreService();
            evaluarScore_response sresponse = new evaluarScore_response();
            
            ServicePointManager.ServerCertificateValidationCallback += MyCertificatePolicy.ValidateCertificate;
            sresponse = se.ScoreEvaluate(msgRequest);

            msgResponse = new Entidades.ScoreEvaluate.MsgResponseScoreEvaluate();
         
            if (Convert.ToInt32(sresponse.pRespuesta) == 0)
            {
                msgResponse.Mensaje = sresponse.pMensaje;
                msgResponse.Respuesta = sresponse.pRespuesta;
                msgResponse.OpportunityID = new Guid(oportunidadID);
                msgResponse.ClientID = new Guid(msgRequest.ClienteID);
                msgResponse.NroSolcicitud = msgRequest.NroSolicitud;
                msgResponse.NumeroOferta = msgRequest.NroOferta;
                msgResponse.OpportunityOrden = msgRequest.OpportunityOrden;

                msgResponse.TCContable = (tcResult.TCContable == null ? 0 : tcResult.TCContable);
                msgResponse.TCCompra = (tcResult.TCCompra == null ? 0 : tcResult.TCCompra);
                msgResponse.TCVenta = (tcResult.TCVenta == null ? 0 : tcResult.TCVenta);

                if (sresponse.pResultado != null)
                {
                    foreach (var item in sresponse.pResultado)
                    {
                        msgResponse.Result.Add(new ScoreEvaluateResult(item.Campo, item.Valor));
                    }
                }
                else
                {
                    rvalue = false;
                    throw new Exception("El servicio de evaluación de Score no retornó ningún resultado.");
                }
            }
            else
            {
                rvalue = false;
                throw new Exception("El servicio de evaluación de Score retornó el siguiente mensaje " + sresponse.pMensaje);
            }

            Negocio.FotoSolicitudCredito fsc = new Negocio.FotoSolicitudCredito(credenciales);
            
            return null;
        }

        private bool ActualizarOportunidadScoreEvaluado(Guid oportunidadId)
        {
            bool rvalue;
            rvalue = true;
            try
            {
                Negocio.Oportunidad negOport = new Negocio.Oportunidad(Credenciales.ObtenerCredenciales());
                negOport.SetScoreEvaluado(oportunidadId, true);
            }
            catch (Exception ex)
            {
                rvalue = false;
                throw ex;
            }

            return rvalue;
        }
    }
}