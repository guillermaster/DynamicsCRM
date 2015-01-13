
namespace Efika.Crm.Web.ScoreEvaluate
{
    using BMSCEvaluarScoreService;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Efika.Crm.Entidades;
    using Efika.Crm.Entidades.ScoreEvaluate;

    public class ScoreService
    {
        private EvaluarScoreService wsScoreEvaluate = new EvaluarScoreService();
        private evaluarScore_response wsResScoreEvaluate = new evaluarScore_response();
        private evaluarScore_request wsReqScoreEvaluate = new evaluarScore_request();
        private MsgRequestScoreEvaluate msgReqSE = new MsgRequestScoreEvaluate();

        public ScoreService()
        { }

        public evaluarScore_response ScoreEvaluate(MsgRequestScoreEvaluate value)
        {
            wsReqScoreEvaluate = SetRequestScoreEvaluate(value);
            wsResScoreEvaluate = wsScoreEvaluate.EvaluarScore(wsReqScoreEvaluate);
            return wsResScoreEvaluate;
        }
        private evaluarScore_request SetRequestScoreEvaluate(MsgRequestScoreEvaluate value)
        {
            evaluarScore_request wsReqSE = new evaluarScore_request();
            wsReqSE.pNroSolicitud = value.NroSolicitud.ToString();
            wsReqSE.pGUICliente = value.ClienteID;
            wsReqSE.pCodigoLoginUsuario = value.LoginUsuario;
            wsReqSE.pCodigoAgencia = value.CodigoAgencia;
            wsReqSE.pCodigoSucursal = value.CodigoSucursal;
            wsReqSE.pFechaEjecucion = value.FechaEjecucion;
            wsReqSE.pUsuario = value.User;
            wsReqSE.pContrasena = value.Password;
            wsReqSE.pDominio = value.Domain;
            return wsReqSE;
        }
    }
}
