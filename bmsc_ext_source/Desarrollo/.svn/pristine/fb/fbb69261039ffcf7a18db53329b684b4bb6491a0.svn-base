using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Efika.Crm.Entidades
{
    public class Producto
    {
        public string Nombre { get; set; }
        public Guid Id { get; set; }
        public Guid FamiliaProductosId { get; set; }
        public string FamiliaProductosNombre { get; set; }
        public Guid TipoProductoId { get; set; }
        public string TipoProductoNombre { get; set; }
        public Guid MonedaId { get; set; }
        public string MonedaNombre { get; set; }
        public int ProductoHomologado { get; set; }
        public bool HabilitadoComercializar { get; set; }
        public string Icono { get; set; }

        public class FamiliaTipos
        {
            public class Activo
            {
                public static int Codigo { get { return 221220000; } }
                public static string Creditos { get { return "CRÉDITOS"; } }
            }
            public class Pasivo
            {
                public static int Codigo { get { return 221220001; } }
                public static string Pasivos { get { return "PASIVOS"; } }
            }
            public class Servicio
            {
                public static int Codigo { get { return 221220002; } }
                public static string Seguros { get { return "SEGUROS"; } }
                public static string Servicios { get { return "SERVICIOS"; } }
                public static string ServiciosComex { get { return "SERVICIOS COMEX"; } }
                public static string Canales { get { return "CANALES"; } }
                public static string Otros { get { return "OTROS"; } }
            }
        }
    }
}
