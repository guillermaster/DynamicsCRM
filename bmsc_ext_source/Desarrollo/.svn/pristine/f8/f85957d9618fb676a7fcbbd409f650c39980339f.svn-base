using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;

namespace Efika.Crm.Plugins.MA.ListaMarketing
{
    internal class DatoListaMarketing
    {
        public const int TIPO_CLIENTE_NATURAL=1;
        public const int TIPO_CLIENTE_JURIDICO = 2;
        public const int OPCION_CLIENTE_NATURAL = 221220000;
        public const int OPCION_CLIENTE_JURIDICO = 221220001;

        public int? TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public int? TipoCliente { get; set; }
        public string TelefonoPrincipal{ get; set; }
        public string TelefonoTrabajo { get; set; }
        public string CorreoElectronicoPrincipal { get; set; }
        public string CorreoElectronicoTrabajo { get; set; }
        public string Ciudad { get; set; }
        public string Ejecutivo { get; set; }
        public Guid IdRegistroCRM { get; set; }
        public int numeroFila { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", TipoIdentificacion, Identificacion, Nombre, PrimerApellido, SegundoApellido);
        }

    }
}
