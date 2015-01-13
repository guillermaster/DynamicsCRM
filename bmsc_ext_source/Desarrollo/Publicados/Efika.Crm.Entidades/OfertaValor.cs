using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Efika.Crm.Entidades
{
    public class OfertaValor
    {
        public string Nombre { get; set; }
        public Guid Id { get; set; }
        public Guid ProductoId { get; set; }
        public Guid TipoProductoId { get; set; }
        public int PrioridadProducto { get; set; }
        public int PrioridadPortafolio { get; set; }
        public String Portafolio { get; set; }
    }
}
