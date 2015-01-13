using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Efika.Crm.Entidades
{
    public class ParametroOfertaValor
    {
        public class TipoParametro
        {
            public class ProbabMinimaAceptacion
            {
                public static string Valor
                {
                    get
                    {
                        return "221220000";
                    }
                }
            }
            public class ProbabMaxFugaProducto
            {
                public static string Valor
                {
                    get
                    {
                        return "221220003";
                    }
                }
            }
            public class ProbabMaxFugaClienteJuridico
            {
                public static string Valor
                {
                    get
                    {
                        return "221220001";
                    }
                }
            }
            public class ProbabMaxFugaClienteNatural
            {
                public static string Valor
                {
                    get
                    {
                        return "221220002";
                    }
                }
            }
            public class MaximoLlamadasEjecutivo
            {
                public static string Valor
                {
                    get
                    {
                        return "221220004";
                    }
                }
            }
        }
    }
}
