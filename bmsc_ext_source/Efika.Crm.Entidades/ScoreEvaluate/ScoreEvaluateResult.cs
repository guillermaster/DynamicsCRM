namespace Efika.Crm.Entidades.ScoreEvaluate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ScoreEvaluateResult
    {
        private string mCampo;
        private string mValor;

        #region Const
        public const string WSRESULT_SCORE = "SCORE";
        public const string WSRESULT_COLOR = "COLOR";
        public const string WSRESULT_LIMITE_GARANTIA_HIPOTECARIA = "LIMITE GARANTIA HIPOTECARIA";
        public const string WSRESULT_LIMITE_GARANTIA_PERSONAL = "LIMITE GARANTIA PERSONAL";
        public const string WSRESULT_LIMITE_A_SOLA_FIRMA = "LIMITE A SOLA FIRMA";
        public const string WSRESULT_SALDO_CAJA_LIBRE = "SALDO CAJA LIBRE";
        public const string WSRESULT_EDAD = "EDAD";
        public const string WSRESULT_CANTIDAD_DE_DEPENDIENTES = "CANTIDAD DEPENDIENTES";
        public const string WSRESULT_PEOR_CALIFICACION_SISTEMA_12_MESES = "PEOR CALIFICACION SISTEMA 12 MESES";
        public const string WSRESULT_NRO_VECES_CALIFICACION_DISTINTA_DE_A_EN12MESES = "NRO VECES CALIFICACION DISTINTA DE A EN 12 MESES";
        public const string WSRESULT_NRO_VECES_POR_ESTADO_EN_12_MESES_SISTEMA = "NRO VECES POR ESTADO EN 12 MESES";
        public const string WSRESULT_NRO_VECES_CLIENTE_ESTA_MAS_DE_14_DIAS_EN_VIGENTE2_BMSC = "NRO VECES CLIENTE ESTA MAS DE 14 DIAS EN VIGENTE2 BMSC";
        public const string WSRESULT_GARANTIAS_CONTITUIDAS = "GARANTIAS CONSTITUIDAS";
        public const string WSRESULT_GARANTIAS_CREDITO_SOLICITADO = "GARANTIAS CREDITO SOLICITADO";
        public const string WSRESULT_RIESGO_INDIRECTO_TITULAR = "RIESGO INDIRECTO TITULAR";
        public const string WSRESULT_RIESGO_INDIRECTO_GARANTE = "RIESGO INDIRECTO GARANTE";
        public const string WSRESULT_FLUJO_DE_CAJA_NEGATIVO = "FLUJO DE CAJA NEGATIVO";
        public const string WSRESULT_DIFERENCIA_DE_CUOTA_MAXIMA_NEGATIVA = "DIFERENCIA DE CUOTA MAXIMA NEGATIVA";
        public const string WSRESULT_SUBPARAMETRO_B = "SUBPARAMETRO B";
        public const string WSRESULT_SUBPARAMETRO_C = "SUBPARAMETRO C";
        public const string WSRESULT_CLIENTE = "CLIENTE";
        public const string WSRESULT_COMPORTAMIENTO_SISTEMA = "COMPORTAMIENTO SISTEMA";
        public const string WSRESULT_COMPORTAMIENTO_BMSC = "COMPORTAMIENTO BMSC";
        public const string WSRESULT_GARANTIAS = "GARANTIAS";
        public const string WSRESULT_RIESGO_INDIRECTO = "RIESGO INDIRECTO";
        public const string WSRESULT_SOBRE_ENDEUDAMIENTO = "SOBRE ENDEUDAMIENTO";
        public const string WSRESULT_PUNTUACIÓN_GLOBAL_FINAL = "PUNTUACION GLOBAL FINAL";
        
        public const string WSRESULT_SCORE_SUGERENCIA = "SCORE SUGERENCIA";
        public const string WSRESULT_LIMITE_GARANTIA_HIPOTECARIA_SUGERENCIA = "LIMITE GARANTIA HIPOTECARIA SUGERENCIA";
        public const string WSRESULT_LIMITE_GARANTIA_PERSONAL_SUGERENCIA = "LIMITE GARANTIA PERSONAL SUGERENCIA";
        public const string WSRESULT_LIMITE_A_SOLA_FIRMA_SUGERENCIA = "LIMITE A SOLA FIRMA SUGERENCIA";
        public const string WSRESULT_SALDO_CAJA_LIBRE_SUGERENCIA = "SALDO CAJA LIBRE SUGERENCIA";
        public const string WSRESULT_SCORE_RECOMENDACION = "SCORE RECOMENDACION";

        public const int INFO_DIAS_ANTIGUEDAD = 30;
        
        #endregion
        #region Constructors
        public ScoreEvaluateResult()
        { }

        public ScoreEvaluateResult(string campo, string valor)
        {
            this.mCampo = campo;
            this.mValor = valor;
        }
        #endregion
        #region Properties
        public string Campo
        {
            get { return mCampo; }
            set { mCampo = value; }
        }

        public string Valor
        {
            get { return mValor; }
            set { mValor = value; }
        }
        #endregion
    }
}
