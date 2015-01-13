using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Efika.Crm.Web.BMSCTipoCambio;
using Efika.Crm.Entidades.TipoCambio;

namespace Efika.Crm.Web.TipoCambio
{
    public class TipoCambioServices
    {
        private BMSCTipoCambio.TipoCambioService wsTipoCambio = new BMSCTipoCambio.TipoCambioService();
        private BMSCTipoCambio.TipoCambio_Response wsResTipoCambio = new BMSCTipoCambio.TipoCambio_Response();
        private BMSCTipoCambio.TipoCambio_Request wsReqTipoCambio = new BMSCTipoCambio.TipoCambio_Request();
        private MsgTipoCambio msgReqSE = new MsgTipoCambio();
        private TipoCambioResult tcResult = new TipoCambioResult();

        public TipoCambioServices()
        { }


        public TipoCambioResult TipoCambio(MsgTipoCambio value)
        {
            wsReqTipoCambio = SetRequestTipoCambio(value);
            wsResTipoCambio = wsTipoCambio.TipoCambio(wsReqTipoCambio);
            tcResult = SetTipoCambioResult(wsResTipoCambio);
            return tcResult;
        }

        private BMSCTipoCambio.TipoCambio_Request SetRequestTipoCambio(MsgTipoCambio value)
        {
            BMSCTipoCambio.TipoCambio_Request wsReqTC = new BMSCTipoCambio.TipoCambio_Request();
            wsReqTC.pUsuario = value.Usuario;
            wsReqTC.pContrasena = value.Contrasena;
            wsReqTC.pDominio = value.Dominio;

            return wsReqTC;
        }
        
        private TipoCambioResult SetTipoCambioResult(BMSCTipoCambio.TipoCambio_Response value)
        {
            TipoCambioResult result = new TipoCambioResult();

            result.TCContable = value.pTCContable;
            result.TCCompra = value.pTCCompra;
            result.TCVenta = value.pTCVenta;
            result.Resultado = Convert.ToInt32(value.pResultado);
            
            return result;
        }

    }
}