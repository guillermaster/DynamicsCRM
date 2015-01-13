namespace Efika.Crm.Entidades.ScoreEvaluate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Efika.Crm.Entidades.ScoreEvaluate;

    public class MsgResponseScoreEvaluate
    {
        #region Vars
        private string pMensaje;
        private string pRespuesta;
        private List<ScoreEvaluateResult> listScoreEvaluateResult = new List<ScoreEvaluateResult>();
        private int nroSolcicitud;
        private Guid clientID;
        private Guid opportunityID;
        private int opportunityOrden;
        private int numeroOferta;
        decimal tcContable = 0;
        decimal tcCompra = 0;
        decimal tcVenta = 0;
        #endregion

        #region Constructors
        public MsgResponseScoreEvaluate()
        { }
        #endregion

        #region Properties
        public string Mensaje
        {
            get { return this.pMensaje; }
            set { this.pMensaje = value; }
        }
        public string Respuesta
        {
            get { return this.pRespuesta; }
            set { this.pRespuesta = value; }

        }
        public int NroSolcicitud
        {
            get { return this.nroSolcicitud; }
            set { this.nroSolcicitud = value; }
        }
        public Guid ClientID
        {
            get { return clientID; }
            set { clientID = value; }
        }
        public Guid OpportunityID
        {
            get { return this.opportunityID; }
            set { this.opportunityID = value; }
        }
        public int OpportunityOrden
        {
            get { return this.opportunityOrden; }
            set { this.opportunityOrden = value; }
        }
        public int NumeroOferta
        {
            get { return this.numeroOferta; }
            set { this.numeroOferta = value; }
        }
        public List<ScoreEvaluateResult> Result
        {
            get { return this.listScoreEvaluateResult; }
            set { this.listScoreEvaluateResult = value; }
        }
        public decimal TCContable { get { return this.tcContable; } set { this.tcContable = value; } }
        public decimal TCCompra { get { return this.tcCompra; } set { this.tcCompra=value; } }
        public decimal TCVenta { get { return this.tcVenta; } set { this.tcVenta = value; } }
        #endregion


    }
}
