using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.CargaDatos.crmSDK;
namespace Efika.Crm.CargaDatos
{
    public class CDProcesoCarga
    {
        private Guid procesoCargaid;
        public int? estado;
        public Guid InsertarLog(CrmService servicio, string strProceso)
        {
            try
            {
                //Creamos el objeto
                efk_proceso_carga_log objProcesoCarga = new efk_proceso_carga_log();
                //estado del proceso
                estado = 221220000; //estado EN PROCESO EJECUCION
                objProcesoCarga.efk_razon_estado = new Picklist();
                objProcesoCarga.efk_razon_estado.Value = estado.Value;
                objProcesoCarga.efk_name = strProceso;
                procesoCargaid = servicio.Create(objProcesoCarga);
                return procesoCargaid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       
    }
}
