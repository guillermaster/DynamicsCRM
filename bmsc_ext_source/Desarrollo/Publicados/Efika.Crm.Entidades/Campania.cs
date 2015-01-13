using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Efika.Crm.Entidades
{
    public class Campania
    {
        public string Nombre { get; set; }
        public DateTime FechaFinPropuesta { get; set; }
        public string Errores { get; set; }
        public decimal moneda { get; set; }
    }
}
