using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Efika.Crm.Entidades
{
    public class Cliente
    {
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Identificacion { get; set;}
        public int TipoIdentificacion { get; set; }
        public int TipoIdentificacionDesc { get; set; }
        public string TipoClienteDesc { get; set; }
        public int TipoClienteId { get; set; }
        public string SegmentoDesc { get; set; }
        public string RazonSocial { get; set; }
        public string FuenteIngresoOfValDesc { get; set; }
        public int FuenteIngresoOfValId { get; set; }
        public Guid Id { get; set; }

        public Guid TransactionCurrencyId { get; set; }
        public int Moneda { get; set; }

        public class TipoCliente
        {
            public static int Natural { get { return 221220000; } }
            public static int Juridico { get { return 221220001; } }
        }

        public class TipoBanca
        {
            public static int BancaCorporativaEmpresas { get { return 221220000; } }
            public static int SinTipo { get { return 221220001; } }
            public static int BancaPersonasNegocios { get { return 221220002; } }
            public static int BancaInstFinancieras { get { return 221220003; } }
            public static int CuentasEspeciales { get { return 221220004; } }
            public static int CuentasFiscales { get { return 221220005; } }
        }

        public class FuenteIngresos
        {
            public static int Independiente { get { return 221220000; } }
            public static int Dependiente { get { return 221220001; } }
            public static int NoTiene { get { return 100000000; } }
        }
    }
}
