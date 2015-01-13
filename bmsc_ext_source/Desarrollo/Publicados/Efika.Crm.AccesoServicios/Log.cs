using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace Efika.Crm.AccesoServicios
{
    public class Log
    {
        private string cPathLog;

        public Log(string pathLog)
        {
            cPathLog = pathLog;

            if (!System.IO.Directory.Exists(Path.GetDirectoryName(cPathLog)))
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(cPathLog));
        }

        public void Add(string mensaje, string clase, string metodo, string usuario, string stacktrace)
        {
            string nombreFile = cPathLog.Replace("yyyymmdd", String.Format("{0:yyyyMMdd}", DateTime.Now));

            try
            {
                File.AppendAllText(nombreFile, String.Format("{0:yyyyMMdd HH:mm:ss:ffff}", DateTime.Now) + ";" + "ISVCRM" + ";" + 
                    clase + ";" + metodo + ";" + usuario + ";" + mensaje + ";" + stacktrace + "\r\n");
            }
            catch
            {
                throw;
            }
        }
    }
}
