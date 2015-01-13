using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;
using System.Web.Services.Protocols;


namespace Efika.Crm.Negocio
{
    public class RespuestaCampania
    {
        private CrmService Servicio;
        private CredencialesCRM credenciales;
        
        public RespuestaCampania(CredencialesCRM credenciales)
        {
            Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
            this.credenciales = credenciales;
        }

        public void SetOportunidadCreada(Guid idRespCampania, bool creada)
        {
            try
            {
                campaignresponse respCamp = new campaignresponse();
                respCamp.activityid = new Key();
                respCamp.activityid.Value = idRespCampania;
                respCamp.efk_oportunidad_creada = new CrmBoolean();
                respCamp.efk_oportunidad_creada.Value = creada;
                if (creada)
                {
                    respCamp.efk_cerrarrespuesta = new CrmBoolean();
                    respCamp.efk_cerrarrespuesta.Value = true;
                }
                Servicio.Update(respCamp);
            }
            catch (SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
