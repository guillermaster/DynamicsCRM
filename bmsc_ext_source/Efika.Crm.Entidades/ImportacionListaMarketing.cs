using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Efika.Crm.Entidades
{
    public class ImportacionListaMarketing
    {
        public string Nombre { get; set; }
        public string RutaArchivoCarga { get; set; }
        public DateTime FechaInicioEjecucion { get; set; }
        public DateTime FechaFinEjecucion { get; set; }
        public string EstadoEjecucion { get; set; }
        public int CantidadRegistros { get; set; }
        public int CantidadClientesNaturales { get; set; }
        public int CantidadClientesJuridicos { get; set; }
        public int CantidadProspectosExistentes { get; set; }
        public int CantidadProspectosNuevos { get; set; }
        public string Errores { get; set; }
        public string IdPropietario { get; set; }
    }
}
