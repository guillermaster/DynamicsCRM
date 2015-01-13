using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;

namespace Efika.Crm.Negocio
{
    public class Privilegios
    {
        public static bool ValidarPrivilegio(string privilegio, Guid idUsuario, CredencialesCRM credenciales)
        {
            return PrivilegiosCRM.ValidarPrivilegio(privilegio, idUsuario, credenciales);
        }

        public class NombresPrivilegios
        {
            public static string SimulacionCredito { get { return "prvCreateefk_simulacion_crediticia"; } }
            public static string EnvioDatosOportunidadSistExt { get { return "prvWriteAccount"; } }
            public static string CierreOportunidad { get { return "prvCreateefk_formulario_cierre_oportunidad"; } }
            public static string ImportacionListaMarketing { get { return "prvCreateefk_formulario_lista_marketing"; } }
            public static string ImportacionPreAprobado { get { return "prvCreateefk_formulario_importar_preaprobados"; } }
            public static string RespuetaCampana { get { return "prvCreateefk_formulario_respuesta_campanna"; } }
            public static string ReabrirOferta { get { return "prvWriteAccount"; } }
            public static string ReabrirOfertaGerente { get { return "prvWriteAccount"; } }
            public static string LeerOportunidad { get { return "prvReadOpportunity"; } }
            public static string OfertaValorMicro { get { return "prvReadAccount"; } }
        }
    }
}
