using Microsoft.Xrm.Sdk;
using System;

namespace Efika.Crm.Plugins.MA.DatosPreAprobados
{
    internal class DatoPreAprobado
    {
        public string Identificacion { get; set; }
        public string CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public decimal? CupoMonto { get; set; }
        public Entity Producto { get; set; }
        public int TipoCliente { get; set; }
        public Guid IdCliente { get; set; }
        public int NumeroFila { get; set; }
        public decimal? Tasa { get; set; }
        public int? Plazo { get; set; }
        public decimal? Comision { get; set; }
        public string Atributo_1 { get; set; }
        public string Valor_1 { get; set; }
        public string Atributo_2 { get; set; }
        public string Valor_2 { get; set; }
        public string Atributo_3 { get; set; }
        public string Valor_3 { get; set; }


        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", Identificacion, CodigoProducto, NombreProducto, CupoMonto);
        }
    }
}
