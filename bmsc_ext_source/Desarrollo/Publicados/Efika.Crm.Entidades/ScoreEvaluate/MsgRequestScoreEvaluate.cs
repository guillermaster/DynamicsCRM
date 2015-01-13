namespace Efika.Crm.Entidades.ScoreEvaluate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class MsgRequestScoreEvaluate
    {

        #region Vars
        private int pNroSolicitud;
        private int nroOferta = 0;
        private int opportunityOrden;
        private string pClienteID;
        private string pCodigoLoginUsuario;
        private string pCodigoAgencia;
        private string pCodigoSucursal;
        private int infoDiaAntiguedad = ScoreEvaluateResult.INFO_DIAS_ANTIGUEDAD;
        private string pUsuario;
        private string pContrasena;
        private string pDominio;
        private DateTime pFechaEjecucion;
        private DateTime opportunityUpdateCondGarantias;
        private DateTime clienteUpdateModeloEvaluador;
        private int pmoneda;

        #endregion

        #region ScoreEvaluate

        public MsgRequestScoreEvaluate()
        {
        }

        public void SetRequestScoreEvaluate(int nroSolicitud, string guiCliente, string codigoLoginUsuario, string codigoAgencia, string codigoSucursal, DateTime fechaEjecucion)
        {
            pNroSolicitud = nroSolicitud;
            pClienteID = guiCliente;
            pCodigoLoginUsuario = codigoLoginUsuario;
            pCodigoAgencia = codigoAgencia;
            pCodigoSucursal = codigoSucursal;
            pFechaEjecucion = fechaEjecucion;
        }

        #endregion

        #region Properties
        public int NroSolicitud
        {
            get
            {
                return this.pNroSolicitud;
            }
            set
            {
                this.pNroSolicitud = value;
            }
        }
        public int NroOferta
        {
            get { return this.nroOferta; }
            set { this.nroOferta = value; }
        }
        public int OpportunityOrden
        {
            get { return this.opportunityOrden; }
            set { this.opportunityOrden = value; }
        }
        public string ClienteID
        {
            get { return this.pClienteID; }
            set { this.pClienteID = value; }
        }
        public string LoginUsuario
        {
            get { return this.pCodigoLoginUsuario; }
            set { this.pCodigoLoginUsuario = value; }
        }
        public string CodigoAgencia
        {
            get { return this.pCodigoAgencia; }
            set { this.pCodigoAgencia = value; }
        }
        public string CodigoSucursal
        {
            get { return this.pCodigoSucursal; }
            set { this.pCodigoSucursal = value; }
        }
        public string User
        {
            get { return this.pUsuario; }
            set { this.pUsuario = value; }
        }
        public string Password
        {
            get { return this.pContrasena; }
            set { this.pContrasena = value; }
        }
        public string Domain
        {
            get { return this.pDominio; }
            set { this.pDominio = value; }
        }
        public DateTime FechaEjecucion
        {
            get { return this.pFechaEjecucion; }
            set { this.pFechaEjecucion = value; }
        }
        public DateTime OpportunityUpdateCondGarantias
        {
            get { return this.opportunityUpdateCondGarantias; }
            set { this.opportunityUpdateCondGarantias = value; }
        }
        public DateTime ClienteUpdateModeloEvaluador
        {
            get { return this.clienteUpdateModeloEvaluador; }
            set { this.clienteUpdateModeloEvaluador = value; }
        }
        public int InfoDiaAntiguedad
        {
            get { return this.infoDiaAntiguedad; }
            set { this.infoDiaAntiguedad = value; }
        }
        
        public int Moneda
        {
            get { return this.pmoneda; }
            set { this.pmoneda = value; }
        }
        #endregion
    }
}
