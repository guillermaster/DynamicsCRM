using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Efika.Crm.Entidades
{
    public class EvaluacionScore
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public decimal PuntajeScore { get; set; }
        public string PuntajeScoreRecomendacion { get; set; }
        public string PuntajeScoreSugerencia { get; set; }
        public decimal PuntajeGlobalFinal { get; set; }
        public decimal TasaCambio { get; set; }
        public int TipoScoreCod { get; set; }
        public string TipoScoreDesc { get; set; }
        public string CodMoneda { get; set; }
        public string Color { get; set; }
        public decimal LimComercGarantHipotec { get; set; }
        public string LimGarantHipotecSugerencia { get; set; }
        public decimal LimComercGarantPersona { get; set; }
        public string LimGarantPersonaSugerencia { get; set; }
        public decimal LimComercGarantSolaFirma { get; set; }
        public string LimGarantSolaFirmaSugerencia { get; set; }
        public decimal SaldoCaja { get; set; }
        public string SaldoCajaSugerencia { get; set; }
        public decimal PtosEdad { get; set; }
        public decimal PtosCantidadDependientes { get; set; }
        public decimal PtosPeorCalifDependientes { get; set; }
        public decimal PtosVecesCalifDist12mSF { get; set; }
        public decimal PtosVecesEstado12mSF { get; set; }
        public decimal PtosVecesClienteEstado14DiasVgnt2BMSC { get; set; }
        public decimal PtosGarantConstituidas { get; set; }
        public decimal PtosGarantCredSolic { get; set; }
        public decimal PtosRiesgoIndirectoTitular { get; set; }
        public decimal PtosRiesgoIndirectoGarante { get; set; }
        public decimal PtosFlujoCajaNegativo { get; set; }
        public decimal PtosDifCuotaMaxNegativa { get; set; }
        public decimal PtosSubParB { get; set; }
        public decimal PtosSubParC { get; set; }
        public decimal PtosFactorCliente { get; set; }
        public decimal PtosFactorComportSF { get; set; }
        public decimal PtosFactorComportBMSC { get; set; }
        public decimal PtosFactorGarant { get; set; }
        public decimal PtosRiesgoIndirecto { get; set; }
        public decimal PtosSobreEndeudamiento { get; set; }
        public Guid transactioncurrencyid { get; set; }

        public EvaluacionScore(Guid idEvalScore, Guid idCliente, decimal puntScore, string puntScoreRecom, string puntScoreSug, decimal puntGlobalFinal,
            decimal tasaCambio, int tipoScoreCod, string tipoScoreDesc, string codMoneda, string color, decimal limComGarHip, string limGarHipSug, decimal limComGarPers,
            string limGarPersSurg, decimal limComGarSolaFirma, string limGarSolaFirmaSug, decimal saldoCaja, string saldoCajaSug, decimal ptosEdad, decimal ptosCantDepend, decimal ptosPeorCalifDep,
            decimal ptosVecesCalifDist12mSF, decimal ptosVecesEstado12mSF, decimal ptosVeces14dVgnt2BMSC, decimal ptosGarConstituidas, decimal ptosGarCredSol, decimal ptosRiesgoIndTit,
            decimal ptosRiesgoIndGar, decimal ptosFlujoCajaNegat, decimal ptosDifCuotaMaxNegat, decimal ptosSubParB, decimal ptosSubParC, decimal ptosFactorCliente,
            decimal ptosFactorCompSF, decimal ptosFactorCompBMSC, decimal ptosFactorGarant, decimal ptosRiesgoInd, decimal ptosSobreEndeudam, Guid transactioncurrencyid)
        {
            Id = idEvalScore;
            ClienteId = idCliente;
            PuntajeScore = puntScore;
            PuntajeScoreRecomendacion = puntScoreRecom;
            PuntajeScoreSugerencia = puntScoreSug;
            PuntajeGlobalFinal = puntGlobalFinal;
            TasaCambio = tasaCambio;
            TipoScoreCod = tipoScoreCod;
            TipoScoreDesc = tipoScoreDesc;
            CodMoneda = codMoneda;
            Color = color;
            LimComercGarantHipotec = limComGarHip;
            LimGarantHipotecSugerencia = limGarHipSug;
            LimComercGarantPersona = limComGarPers;
            LimGarantPersonaSugerencia = limGarPersSurg;
            LimComercGarantSolaFirma = limComGarSolaFirma;
            LimGarantSolaFirmaSugerencia = limGarSolaFirmaSug;
            SaldoCaja = saldoCaja;
            PtosEdad = ptosEdad;
            PtosCantidadDependientes = ptosCantDepend;
            PtosPeorCalifDependientes = ptosPeorCalifDep;
            PtosVecesCalifDist12mSF = ptosVecesCalifDist12mSF;
            PtosVecesEstado12mSF = ptosVecesEstado12mSF;
            PtosVecesClienteEstado14DiasVgnt2BMSC = ptosVeces14dVgnt2BMSC;
            PtosGarantConstituidas = ptosGarConstituidas;
            PtosGarantCredSolic = ptosGarCredSol;
            PtosRiesgoIndirectoTitular = ptosRiesgoIndTit;
            PtosRiesgoIndirectoGarante = ptosRiesgoIndGar;
            PtosFlujoCajaNegativo = ptosFlujoCajaNegat;
            PtosDifCuotaMaxNegativa = ptosFlujoCajaNegat;
            PtosSubParB = ptosSubParB;
            PtosSubParC = ptosSubParC;
            PtosFactorCliente = ptosFactorCliente;
            PtosFactorComportSF = ptosFactorCompSF;
            PtosFactorComportBMSC = ptosFactorCompBMSC;
            PtosFactorGarant = ptosFactorGarant;
            PtosRiesgoIndirecto = ptosRiesgoInd;
            PtosSobreEndeudamiento = ptosSobreEndeudam;
            this.transactioncurrencyid = transactioncurrencyid;
        }
    }
}
