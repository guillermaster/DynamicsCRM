using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Xrm.Sdk;

namespace Efika.Crm.Web
{
    public class ListaPreaprobado
    {
        public string Nombre { get; set; }
        public string RutaArchivoCarga { get; set; }
        public DateTime FechaInicioEjecucion { get; set; }
        public DateTime FechaFinEjecucion { get; set; }
        public string EstadoEjecucion { get; set; }
        public int CantidadRegistros { get; set; }
        public string Errores { get; set; }
        public Entity Campania { get; set; }


    }
}