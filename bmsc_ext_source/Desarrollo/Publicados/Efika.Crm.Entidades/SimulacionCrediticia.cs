using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Efika.Crm.Entidades
{
    public class SimulacionCrediticia
    {
        public string Nombre { get; set; }
        public Guid Id { get; set; }
        public long NumeroOferta { get; set; }
        public Guid ClienteId { get; set; }
        public Guid DivisaId { get; set; }
        public Guid CampaniaId { get; set; }
        public Guid OwnerId { get; set; }
        public bool Guardada { get; set; }
        public Guid transactioncurrencyid { get; set; }
    }
}
