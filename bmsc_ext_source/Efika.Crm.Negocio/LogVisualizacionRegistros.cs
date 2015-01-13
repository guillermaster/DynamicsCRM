using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;

namespace Efika.Crm.Negocio
{
    public class LogVisualizacionRegistros
    {
        public static bool RegistrarLogVisualizacionRegistro(CredencialesCRM credenciales, string p_strIdCliente, string p_strIdUsuario, string p_strNombreOrganizacion,
                                                        string p_strCodigoMis, string p_strCodigoEvento)
        {

            CrmService Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);

            try
            {
                efk_log_visualizacion_registros log = new efk_log_visualizacion_registros();
                if (p_strCodigoEvento == "100000001")
                {
                    log.efk_accountid = new Lookup();
                    log.efk_accountid.type = "account";
                    log.efk_accountid.Value = new Guid(p_strIdCliente);
                }
                
                log.efk_systemuserid = new Lookup();
                log.efk_systemuserid.type = "systemuser";
                log.efk_systemuserid.Value = new Guid(p_strIdUsuario);

                log.efk_organizacion = p_strNombreOrganizacion;

                log.efk_codigo_mis = p_strCodigoMis;

                log.efk_evento = new Picklist();
                log.efk_evento.Value = Int32.Parse(p_strCodigoEvento);

                DateTime fechaActual = System.DateTime.Now;

                log.efk_fecha_evento = new CrmDateTime();
                log.efk_fecha_evento.Value = fechaActual.ToString("yyyy-MM-ddTHH:mm:ss");

                //Averiguamos los datos del usuario
                ColumnSet cols = new ColumnSet();
                cols.Attributes = new string[] { "fullname", "domainname" };
                systemuser usuario = (systemuser)Servicio.Retrieve("systemuser", new Guid(p_strIdUsuario), cols);

                if (usuario != null)
                {
                    log.efk_usuario_domainname = usuario.domainname;
                    log.efk_usuario_fullname = usuario.fullname;
                }

                log.efk_ip_equipo_cliente = System.Web.HttpContext.Current.Request.UserHostAddress;

                log.efk_nombre = usuario.fullname + " - " + fechaActual.ToString("MM/dd/yyyy hh:mm:ss");

                //Creamos el registro
                Servicio.Create(log);

                return true;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                throw new Exception(ex.Detail.InnerText);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                Servicio.Dispose();
            }
        }
    }
}
