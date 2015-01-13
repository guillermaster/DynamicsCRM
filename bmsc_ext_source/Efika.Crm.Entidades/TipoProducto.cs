using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Efika.Crm.Entidades
{
    public class TipoProducto
    {
        public string Nombre { get; set; }
        public Guid Id { get; set; }
        public Guid FamiliaId { get; set; }
        public string Icono { get; set; }
    }
}
