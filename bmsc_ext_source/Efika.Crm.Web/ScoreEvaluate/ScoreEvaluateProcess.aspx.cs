using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Negocio;
using Efika.Crm.Entidades;
using Efika.Crm.Web.Common;
using Efika.Crm.Web.BMSCEvaluarScoreService;
using Efika.Crm.Entidades.ScoreEvaluate;

namespace Efika.Crm.Web.ScoreEvaluate
{
    public partial class ScoreEvaluateProcess : System.Web.UI.Page
    {
        const string PRIVILEGIO_WRITE_OPORTUNIDAD = "prvWriteOpportunity";
        const string CODIGO_USUARIO = "strCodigoUsuario";
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
        private string option;
        CredencialesCRM credenciales = new CredencialesCRM();

        protected void Page_Load(object sender, EventArgs e)
        {
            Utility.SettingPage(this.Response);

            cmrUserID = Request.QueryString[CODIGO_USUARIO];
            nroSolicitud = Request.QueryString[NUMERO_SOLICITUD];
            nombreUsuario = Request.QueryString[NOMBRE_USUARIO];
            dominio = Request.QueryString[DOMINIO];
            oportunidadID = Request.QueryString[OPORTUNIDADID];
            option = Request.QueryString[OPTION_PARAMETER];


            if (option == null) option = string.Empty;

            Response.Expires = -1;

            switch (option.ToUpper())
            {
                case "VALIDATESCORE":
                    Response.ContentType = "text/plain";
                    Response.Write("opcion 1");
                    Response.End();
                    break;
                case "FOTOSOLICITUDCREDITO":
                    Response.ContentType = "text/plain";
                    Response.Write("opcion 2");
                    Response.End();
                    break;
                default:
                    break;
            }
        }


        private void ValidateScore()
        {
            scoreEvaluate = new Negocio.ScoreEvaluate(this.credenciales);

            msgRequest = scoreEvaluate.GetMsgReqScoreEvaluate(new Guid(cmrUserID), new Guid(oportunidadID));

            // Validar fecha de actualizacion de la informacion
            if (!scoreEvaluate.ValidateAntiguedadDatos(credenciales, new Guid(oportunidadID), new Guid(msgRequest.ClienteID), 
                msgRequest.OpportunityUpdateCondGarantias, msgRequest.ClienteUpdateModeloEvaluador, msgRequest.InfoDiaAntiguedad, ref validateField))
            {
                foreach (var item in validateField)
                {
                    msgText = item.Message + "\n";
                    Response.Write(@"<script type=""text/javascript""> alert('" + msgText + "'); </script>");
                }
            }

            Response.Flush();

            // Validar los campos requerido Oportunidad/CondicionesGarantia.
            validateField.Clear();
            if (!scoreEvaluate.ValidateOportunidad_CondicionesGarantia(new Guid(oportunidadID), msgRequest.NroOferta, ref validateField))
            {
                if (validateField.Count() > 0)
                {
                    msgText = @"Son necesario todos los campos en la seccion ""Condiciones y Garantias""  de la Oportunidad";
                    Response.Write(@"<script type=""text/javascript""> alert('" + msgText + "'); </script>");
                }
            }

            //validar los campos requerido Cliente/ModeloEvaluador
            validateField.Clear();
            if (!scoreEvaluate.ValidateCliente_ModeloEvaluador(credenciales, new Guid(msgRequest.ClienteID), ref validateField))
            {
                if (validateField.Count() > 0)
                {
                    msgText = @"Son necesario todos los campos en la seccion ""Datos Modelo Evaluador""  del cliente";
                    Response.Write(@"<script type=""text/javascript""> alert('" + msgText + "'); </script>");
                }

            }
        }

        private string CreateUpdate_FotoSolicitudCredito()
        {

            msgRequest.NroSolicitud = int.Parse(nroSolicitud.Replace(",", "").Replace(".", "").Trim());
            msgRequest.LoginUsuario = dominio + "\\" + nombreUsuario;

            ScoreService se = new ScoreService();
            evaluarScore_response sresponse = new evaluarScore_response();

            sresponse = se.ScoreEvaluate(msgRequest);

            msgResponse = new Entidades.ScoreEvaluate.MsgResponseScoreEvaluate();

            if (Convert.ToInt32(sresponse.pRespuesta) == 0)
            {
                msgResponse.Mensaje = sresponse.pMensaje;
                msgResponse.Respuesta = sresponse.pRespuesta;
                msgResponse.OpportunityID = new Guid(oportunidadID);
                msgResponse.ClientID = new Guid(msgRequest.ClienteID);
                msgResponse.NroSolcicitud = msgRequest.NroSolicitud;

                foreach (var item in sresponse.pResultado)
                {
                    msgResponse.Result.Add(new ScoreEvaluateResult(item.Campo, item.Valor));
                }
            }
            else
            {
                throw new Exception("El servicio de evaluacion de Score retorno el siguiente mensaje \n" + sresponse.pMensaje);
            }

            Negocio.FotoSolicitudCredito fsc = new Negocio.FotoSolicitudCredito(credenciales);
            
            return null;
        }
    }
}