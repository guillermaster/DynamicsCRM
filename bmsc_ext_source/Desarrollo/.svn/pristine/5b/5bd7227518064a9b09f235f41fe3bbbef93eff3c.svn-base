using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.CargaDatos.ServiciosCarga;

namespace Efika.Crm.CargaDatos
{
    public class ClienteCRM
    {
        public static int IngresarOptionSetValue(string url, string nombreEsquema, string valor)
        {
            ServiciosCarga.ServiciosCarga servicio = new ServiciosCarga.ServiciosCarga();
            servicio.Url = url;
            servicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            servicio.PreAuthenticate = true;
            return servicio.IngresarOptionSetValue(nombreEsquema, valor);
        }

        public static void PublicarOptionSetValue(string url, string nombreEsquema)
        {
            ServiciosCarga.ServiciosCarga servicio = new ServiciosCarga.ServiciosCarga();
            servicio.Url = url;
            servicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
            servicio.PreAuthenticate = true;
            servicio.PublicarOptionSetValue(nombreEsquema);
        }

    }
}
